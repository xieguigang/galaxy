Imports System.Text

Namespace ThemeColorPicker
    Public Class ColorSelectedArg
        Inherits EventArgs
        Private _selectedColor As Color
        Private _hexColor As String

        Public ReadOnly Property Color As Color
            Get
                Return _selectedColor
            End Get
        End Property
        Public ReadOnly Property HexColor As String
            Get
                Return _hexColor
            End Get
        End Property
        Public ReadOnly Property R As Integer
            Get
                Return _selectedColor.R
            End Get
        End Property
        Public ReadOnly Property G As Integer
            Get
                Return _selectedColor.R
            End Get
        End Property
        Public ReadOnly Property B As Integer
            Get
                Return _selectedColor.B
            End Get
        End Property

        Public Sub New(selectedColor As Color)
            _selectedColor = selectedColor
            Dim sb As StringBuilder = New StringBuilder()
            sb.AppendFormat("#")
            sb.Append(BitConverter.ToString(New Byte() {_selectedColor.R}))
            sb.Append(BitConverter.ToString(New Byte() {_selectedColor.G}))
            sb.Append(BitConverter.ToString(New Byte() {_selectedColor.B}))
            _hexColor = sb.ToString()
        End Sub
    End Class
End Namespace
