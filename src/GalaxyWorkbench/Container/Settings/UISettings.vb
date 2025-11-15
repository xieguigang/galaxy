Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Container

    Public Class UISettings

        Public Property language As Languages = Languages.Default
        Public Property width As Integer
        Public Property height As Integer
        Public Property left As Integer
        Public Property top As Integer
        Public Property rememberLocation As Boolean = True
        Public Property windowState As FormWindowState
        Public Property windows As DockSettings()

        Public Shared ReadOnly Property DefaultConfigFile As String = App.ProductProgramData & "/ui_settings.json"

        Public Shared Function LoadSettings() As UISettings
            Dim config As UISettings = DefaultConfigFile.LoadJsonFile(Of UISettings)(throwEx:=False)

            If config Is Nothing Then
                config = New UISettings With {
                    .height = 800,
                    .language = Languages.Default,
                    .left = 100,
                    .top = 100,
                    .width = 1000,
                    .rememberLocation = True,
                    .windowState = FormWindowState.Normal
                }
            End If

            Return config
        End Function

        Public Sub Save()
            Try
                Call Me.GetJson.SaveTo(DefaultConfigFile)
            Catch ex As Exception
                Call App.LogException(ex)
            End Try
        End Sub

    End Class
End Namespace