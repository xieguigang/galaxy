Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ShellExtensions

    ''' <summary>    
    ''' This class attribute is applied to a Preview Handler to specify registration parameters.
    ''' </summary>
    <AttributeUsage(AttributeTargets.[Class], AllowMultiple:=False, Inherited:=False)>
    Public NotInheritable Class PreviewHandlerAttribute
        Inherits Attribute

        ''' <summary>
        ''' Creates a new instance of the attribute.
        ''' </summary>
        ''' <param name="name__1">Name of the Handler</param>
        ''' <param name="extensions__2">Semi-colon-separated list of file extensions supported by the handler.</param>
        ''' <param name="appId__3">A unique guid used for process isolation.</param>
        Public Sub New(name__1 As String, extensions__2 As String, appId__3 As String)
            If name__1 Is Nothing Then
                Throw New ArgumentNullException("name")
            End If
            If extensions__2 Is Nothing Then
                Throw New ArgumentNullException("extensions")
            End If
            If appId__3 Is Nothing Then
                Throw New ArgumentNullException("appId")
            End If

            Name = name__1
            Extensions = extensions__2
            AppId = appId__3
            DisableLowILProcessIsolation = False
        End Sub

        ''' <summary>
        ''' Gets the name of the handler.
        ''' </summary>
        Public Property Name() As String

        ''' <summary>
        ''' Gets the semi-colon-separated list of extensions supported by the preview handler.
        ''' </summary>
        Public Property Extensions() As String

        ''' <summary>
        ''' Gets the AppId associated with the handler for use with the surrogate host process.
        ''' </summary>
        Public Property AppId() As String

        ''' <summary>
        ''' Disables low integrity-level process isolation.        
        ''' <remarks>This should be avoided as it could be a security risk.</remarks>
        ''' </summary>
        Public Property DisableLowILProcessIsolation() As Boolean

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
