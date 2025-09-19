Imports System
Imports System.IO
Imports Microsoft.VisualBasic

Namespace EPocalipse.Json.Viewer
    <Serializable>
    Public Class UnbufferedStringReader
        Inherits TextReader
        ' Fields
        Private _length As Integer
        Private _pos As Integer
        Private _s As String

        ' Methods
        Public Sub New(s As String)
            If Equals(s, Nothing) Then
                Throw New ArgumentNullException("s")
            End If
            _s = s
            _length = If(Equals(s, Nothing), 0, s.Length)
        End Sub

        Public Overrides Sub Close()
            Dispose(True)
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            _s = Nothing
            _pos = 0
            _length = 0
            MyBase.Dispose(disposing)
        End Sub

        Public Overrides Function Peek() As Integer
            If Equals(_s, Nothing) Then
                Throw New Exception("object closed")
            End If
            If _pos = _length Then
                Return -1
            End If
            Return Strings.AscW(_s(_pos))
        End Function

        Public Overrides Overloads Function Read() As Integer
            If Equals(_s, Nothing) Then
                Throw New Exception("object closed")
            End If
            If _pos = _length Then
                Return -1
            End If
            Return Strings.AscW(_s(Math.Min(Threading.Interlocked.Increment(_pos), _pos - 1)))
        End Function

        Public Overrides Overloads Function Read(buffer As Char(), index As Integer, count As Integer) As Integer
            If buffer Is Nothing Then
                Throw New ArgumentNullException("buffer")
            End If
            If index < 0 Then
                Throw New ArgumentOutOfRangeException("index")
            End If
            If count < 0 Then
                Throw New ArgumentOutOfRangeException("count")
            End If
            If buffer.Length - index < count Then
                Throw New ArgumentException("invalid offset length")
            End If
            If Equals(_s, Nothing) Then
                Throw New Exception("object closed")
            End If
            Dim num = _length - _pos
            If num > 0 Then
                If num > count Then
                    num = count
                End If
                _s.CopyTo(_pos, buffer, index, num)
                _pos += num
            End If
            Return num
        End Function

        Public Overrides Function ReadLine() As String
            If Equals(_s, Nothing) Then
                Throw New Exception("object closed")
            End If
            Dim num = _pos
            While num < _length
                Dim ch = _s(num)
                Select Case ch
                    Case Microsoft.VisualBasic.Strings.ChrW(13), Microsoft.VisualBasic.Strings.ChrW(10)
                        Dim text = _s.Substring(_pos, num - _pos)
                        _pos = num + 1
                        If ch = Microsoft.VisualBasic.Strings.ChrW(13) AndAlso _pos < _length AndAlso _s(_pos) = Microsoft.VisualBasic.Strings.ChrW(10) Then
                            _pos += 1
                        End If
                        Return text
                End Select
                num += 1
            End While
            If num > _pos Then
                Dim text2 = _s.Substring(_pos, num - _pos)
                _pos = num
                Return text2
            End If
            Return Nothing
        End Function

        Public Overrides Function ReadToEnd() As String
            Dim text As String
            If Equals(_s, Nothing) Then
                Throw New Exception("object closed")
            End If
            If _pos = 0 Then
                text = _s
            Else
                text = _s.Substring(_pos, _length - _pos)
            End If
            _pos = _length
            Return text
        End Function

        Public ReadOnly Property Position As Integer
            Get
                Return _pos
            End Get
        End Property
    End Class

End Namespace
