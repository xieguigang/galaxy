Namespace Gtk.CSSEngine.Serialization

    Module Reflector

        Public Function Fill(Of T As Class)(css As CSSFile) As T

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="value">CSS property value</param>
        ''' <param name="type">.NET object type</param>
        ''' <returns></returns>
        Public Function GetValue(value As String, type As Type) As Object
            Select Case type
                Case GetType(Color)
                    Return CSSUtils.GetARGB(value)
                Case GetType(Font)
                    Return CSSUtils.GetFont(value)
                Case Else
                    Return Scripting.CTypeDynamic(value, type)
            End Select
        End Function
    End Module
End Namespace