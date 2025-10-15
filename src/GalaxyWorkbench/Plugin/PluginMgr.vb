Imports System.Runtime.InteropServices
Imports Galaxy.Workbench.Plugin.Repository
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Plugin

    <ClassInterface(ClassInterfaceType.AutoDual)>
    <ComVisible(True)>
    Public Class PluginMgr

        Dim registry As RegistryFile

        Private Sub New()
        End Sub

        Public Sub SetStatus(id As String, status As String)
            Dim plugin As PluginMetadata = registry(id)

            If Not plugin Is Nothing Then
                plugin.status = status
            End If

            Call registry.Save()
        End Sub

        Public Function GetPlugins() As String
            ' reload and then returns the registry list data
            registry = RegistryFile.LoadRegistry
            Return registry.plugins.GetJson
        End Function

        Public Function Query(nameOrGuid As String) As Plugin
            Dim guid As Guid = Nothing
            Dim q As Plugin

            If Guid.TryParse(nameOrGuid, guid) Then
                q = MZKitPlugin.InMemoryLoaderRegistry.Values _
                    .Where(Function(app) app.guid.Equals(guid)) _
                    .FirstOrDefault
            Else
                q = MZKitPlugin.InMemoryLoaderRegistry.Values _
                    .Where(Function(app) app.Name.TextEquals(nameOrGuid)) _
                    .FirstOrDefault
            End If

            Return q
        End Function

        Public Sub Exec(id As String)
            Dim plugin As Plugin = MZKitPlugin.InMemoryLoaderRegistry.TryGetValue(id)

            If Not plugin Is Nothing Then
                Call plugin.Exec()
            End If
        End Sub

        Public Sub InstallLocal()
            Using file As New OpenFileDialog With {.Filter = "MZKit plugin(*.dll)|*.dll"}
                If file.ShowDialog = DialogResult.OK Then
                    Call file.FileName.FileCopy($"{App.LocalData}/plugins/{file.FileName.FileName}")
                    Call registry.Save()
                    Call Plugin.LoadPlugins(println:=AddressOf Workbench.StatusMessage)

                    registry = RegistryFile.LoadRegistry
                End If
            End Using
        End Sub

        Public Sub Save()
            Call registry.Save()
        End Sub

        Public Shared Function Load() As PluginMgr
            Return New PluginMgr With {.registry = RegistryFile.LoadRegistry}
        End Function
    End Class
End Namespace