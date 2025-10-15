Imports System.Reflection
Imports Galaxy.Workbench.Plugin.Repository
Imports Microsoft.VisualBasic.ApplicationServices.Development
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq

Namespace Plugin

    ''' <summary>
    ''' the mzkit plugin model abstract
    ''' </summary>
    Public MustInherit Class Plugin

        Public MustOverride ReadOnly Property guid As Guid
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property Link As String
        Public MustOverride ReadOnly Property Description As String

        ''' <summary>
        ''' run this plugin
        ''' </summary>
        Public MustOverride Function Init(println As Action(Of String)) As Boolean
        Public MustOverride Sub Exec()

        Public Function GetMetadata() As PluginMetadata
            Dim asm As AssemblyInfo = Me.GetType.Assembly.FromAssembly

            Return New PluginMetadata With {
                .author = asm.AssemblyCompany,
                .desc = Description,
                .id = guid.ToString,
                .name = Name,
                .url = Link,
                .ver = asm.AssemblyVersion
            }
        End Function

        Private Shared Iterator Function PopulateDllFiles() As IEnumerable(Of String())
            Yield $"{App.HOME}/plugins".EnumerateFiles("*.dll").ToArray
            Yield $"{App.LocalData}/plugins/".EnumerateFiles("*.dll").ToArray
            Yield $"{App.HOME}".EnumerateFiles("*.dll").ToArray

            ' plugins_test used for test the internal plugin development
            If Container.AppEnvironment.IsDevelopmentMode Then
                Yield $"{App.HOME}/plugins_test".EnumerateFiles("*.dll").ToArray
            End If
        End Function

        Public Shared Sub LoadPlugins(println As Action(Of String))
            Dim files As String() = PopulateDllFiles _
                .IteratesALL _
                .Where(Function(dll) Not dll.FileName.ToLower.StartsWith("microsoft")) _
                .Where(Function(dll) Not dll.FileName.ToLower.StartsWith("bionovogene")) _
                .Where(Function(dll) Not dll.FileName.ToLower.StartsWith("system")) _
                .Where(Function(dll) Not dll.FileName.ToLower.StartsWith("smrucc")) _
                .ToArray
            Dim loaded As Index(Of String) = New String() {}
            Dim registry As RegistryFile = RegistryFile.LoadRegistry
            Dim hashlist = registry.plugins.SafeQuery.ToDictionary(Function(p) p.id)
            Dim asm As Assembly

            Call println($"load plugins: get {files.Length} dll modules")

            For Each path As String In files
                Try
                    path = path.GetFullPath.Replace("/", "\")
                    asm = Assembly.LoadFile(path)
                Catch ex As Exception
                    ' 20250219 there is a bug about the clr assembly reference to the 
                    ' ThermoFisher assembly files
                    ' which will cause the error of assembly is not allowed loading from the remote location
                    '
                    ' "E:\mzkit\ThermoFisher\Libs\Net471\ThermoFisher.CommonCore.BackgroundSubtraction.dll"
                    ' "E:\mzkit\ThermoFisher\Libs\Net471\ThermoFisher.CommonCore.Data.dll"
                    ' "E:\mzkit\ThermoFisher\Libs\Net471\ThermoFisher.CommonCore.MassPrecisionEstimator.dll"
                    ' "E:\mzkit\ThermoFisher\Libs\Net471\ThermoFisher.CommonCore.RawFileReader.dll"
                    '
                    ' do not reference to these assembly file in plugin project, instead of 
                    ' reference to the 
                    '
                    ' BioNovoGene.Analytical.MassSpectrometry.Assembly.ThermoRawFileReader.dll
                    '
                    ' wrapper project
                    '
                    Call App.LogException(New Exception("incorrect clr assembly file: " & path, ex))
                    Continue For
                End Try

                If Not MZKitPlugin.GetFlag(asm) Then
                    Continue For
                Else
                    Call println($"scan plugins from '{path.FileName}'!")
                End If

                Dim types As Type() = asm.GetExportedTypes

                For Each type As Type In types
                    If type.IsInheritsFrom(GetType(Plugin)) Then
                        Dim obj As Object = Activator.CreateInstance(type)
                        Dim plugin As Plugin = DirectCast(obj, Plugin)
                        Dim pluginData As PluginMetadata = plugin.GetMetadata

                        If pluginData.id Like loaded Then
                            Continue For
                        Else
                            loaded.Add(pluginData.id)
                            MZKitPlugin.InMemoryLoaderRegistry(pluginData.id) = plugin

                            If Not hashlist.ContainsKey(pluginData.id) Then
                                Call hashlist.Add(pluginData.id, pluginData)
                            Else
                                pluginData.status = hashlist(pluginData.id).status
                                hashlist(pluginData.id) = pluginData
                            End If
                        End If
                    End If
                Next
            Next

            registry.plugins = hashlist.Values.ToArray

            Call registry.Save()
            Call println("scan plugin job done!")
        End Sub

        Public Shared Sub InitPlugins(println As Action(Of String))
            Dim registry As RegistryFile = RegistryFile.LoadRegistry
            Dim hashlist = registry.plugins.SafeQuery.ToDictionary(Function(p) p.id)

            For Each plugin_id As String In MZKitPlugin.InMemoryLoaderRegistry.Keys
                If registry.IsDisabled(plugin_id) Then
                    Continue For
                End If

                Dim plugin As Plugin = MZKitPlugin.InMemoryLoaderRegistry(plugin_id)

                Try
                    If Not plugin.Init(println) Then
                        ' pluginData.status = "incompatible"
                    End If
                Catch ex As Exception
                    hashlist(plugin_id).status = "incompatible"

                    Call println($"Load plugin error: {ex.Message}")
                    Call App.LogException(ex)
                End Try
            Next

            Call registry.Save()
        End Sub
    End Class
End Namespace