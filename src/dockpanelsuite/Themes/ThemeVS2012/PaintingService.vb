Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    Public Class PaintingService
        Implements IPaintingService

        Private _penCache As IDictionary(Of KeyValuePair(Of Integer, Integer), Pen) = New Dictionary(Of KeyValuePair(Of Integer, Integer), Pen)()
        Private _brushCache As IDictionary(Of Integer, SolidBrush) = New Dictionary(Of Integer, SolidBrush)()

        Public Function GetBrush(color As Color) As SolidBrush Implements IPaintingService.GetBrush
            Dim key = color.ToArgb()
            If _brushCache.ContainsKey(key) Then
                Return _brushCache(key)
            End If

            Dim result = New SolidBrush(color)
            _brushCache.Add(key, result)
            Return result
        End Function

        Public Function GetPen(color As Color, Optional thickness As Integer = 1) As Pen Implements IPaintingService.GetPen
            Dim key = New KeyValuePair(Of Integer, Integer)(color.ToArgb(), thickness)
            If _penCache.ContainsKey(key) Then
                Return _penCache(key)
            End If

            Dim result = New Pen(color, thickness)
            _penCache.Add(key, result)
            Return result
        End Function

        Public Sub CleanUp() Implements IPaintingService.CleanUp
            For Each pen In _penCache
                pen.Value.Dispose()
            Next

            _penCache.Clear()

            For Each brush In _brushCache
                brush.Value.Dispose()
            Next

            _brushCache.Clear()
        End Sub
    End Class
End Namespace
