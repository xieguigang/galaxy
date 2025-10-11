''' <summary>
''' A method of <see cref="GetMainToolStrip"/> must be overrides
''' and implemented for get the toolstrip for the external 3rd 
''' plugin
''' </summary>
Public Class ToolStripWindow

    Public Overridable Function GetMainToolStrip() As ToolStrip
        Throw New NotImplementedException
    End Function
End Class