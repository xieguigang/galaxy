Imports System.IO
Imports System.IO.Compression
Imports System.Reflection

Namespace Plugin

    Public Class PluginPkg

        Public Property guid As String
        Public Property Name As String
        Public Property Link As String
        Public Property Description As String
        Public Property assets As String()

        Public Function BuildPackage(pkg As ZipArchive, rootDir As String) As Boolean
            Dim assets As New List(Of String)

            For Each path As String In assets
                Dim relative As String = PathExtensions.RelativePath(rootDir, path)

                Call assets.Add(relative)

                Using file As Stream = pkg.CreateEntry($"/bin/{relative}").Open
                    Call New MemoryStream(path.ReadBinary).CopyTo(file)
                    Call file.Flush()
                End Using
            Next

            Me.assets = assets.ToArray

            Dim metadata As String = Me.GetXml

            Using buf As Stream = pkg.CreateEntry("/index.xml").Open
                Using text As New StreamWriter(buf)
                    Call text.WriteLine(metadata)
                End Using
            End Using

            Return True
        End Function

        Public Shared Function FromAppReleaseDirectory(dir As String) As PluginPkg
            Dim files As String() = dir.ListFiles.ToArray
            Dim plugin As Plugin = Nothing

            For Each path As String In files
                If path.ExtensionSuffix("dll") Then
                    Dim asm As Assembly = Assembly.LoadFile(path.GetFullPath)

                    If Not MZKitPlugin.GetFlag(asm) Then
                        Continue For
                    Else
                        Dim pluginType As Type = asm.ExportedTypes _
                            .Where(Function(type) type.IsInheritsFrom(GetType(Plugin))) _
                            .FirstOrDefault

                        If Not pluginType Is Nothing Then
                            plugin = Activator.CreateInstance(pluginType)
                            Exit For
                        End If
                    End If
                End If
            Next

            If plugin Is Nothing Then
                Return Nothing
            Else
                Return New PluginPkg With {
                    .assets = files.ToArray,
                    .Description = plugin.Description,
                    .guid = plugin.guid.ToString,
                    .Link = plugin.Link,
                    .Name = plugin.Name
                }
            End If
        End Function

    End Class
End Namespace