Imports System.Runtime.CompilerServices

Namespace Gtk.CSSEngine.Serialization

    Public Module Reflector

        <Extension>
        Public Function Fill(Of T As CSSEngine.Controls.Control)(css As CSSFile) As T
            Dim type As Type = GetType(T)
            Dim obj As Object = Activator.CreateInstance(type)



            Return DirectCast(obj, T)
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