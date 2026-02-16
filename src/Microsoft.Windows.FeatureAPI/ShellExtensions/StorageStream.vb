Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes

Namespace ShellExtensions

    ''' <summary>
    ''' A wrapper for the native IStream object.
    ''' </summary>
    Public Class StorageStream
        Inherits Stream
        Implements IDisposable
        Private _stream As IStream
        Private _isReadOnly As Boolean = False

        Friend Sub New(stream As IStream, [readOnly] As Boolean)
            If stream Is Nothing Then
                Throw New ArgumentNullException("stream")
            End If
            _isReadOnly = [readOnly]
            Me._stream = stream
        End Sub

        ''' <summary>
        ''' Reads a single byte from the stream, moving the current position ahead by 1.
        ''' </summary>
        ''' <returns>A single byte from the stream, -1 if end of stream.</returns>
        Public Overrides Function ReadByte() As Integer
            ThrowIfDisposed()

            Dim buffer As Byte() = New Byte(0) {}
            If Read(buffer, 0, 1) > 0 Then
                Return buffer(0)
            End If
            Return -1
        End Function

        ''' <summary>
        ''' Writes a single byte to the stream
        ''' </summary>
        ''' <param name="value">Byte to write to stream</param>
        Public Overrides Sub WriteByte(value As Byte)
            ThrowIfDisposed()
            Dim buffer As Byte() = New Byte() {value}
            Write(buffer, 0, 1)
        End Sub

        ''' <summary>
        ''' Gets whether the stream can be read from.
        ''' </summary>
        Public Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return _stream IsNot Nothing
            End Get
        End Property

        ''' <summary>
        ''' Gets whether seeking is supported by the stream.
        ''' </summary>
        Public Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return _stream IsNot Nothing
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the stream can be written to.
        ''' Always false.
        ''' </summary>
        Public Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return _stream IsNot Nothing AndAlso Not _isReadOnly
            End Get
        End Property

        ''' <summary>
        ''' Reads a buffer worth of bytes from the stream.
        ''' </summary>
        ''' <param name="buffer">Buffer to fill</param>
        ''' <param name="offset">Offset to start filling in the buffer</param>
        ''' <param name="count">Number of bytes to read from the stream</param>
        ''' <returns></returns>
        Public Overrides Function Read(buffer As Byte(), offset As Integer, count As Integer) As Integer
            ThrowIfDisposed()

            If buffer Is Nothing Then
                Throw New ArgumentNullException("buffer")
            End If
            If offset < 0 Then
                Throw New ArgumentOutOfRangeException("offset", GlobalLocalizedMessages.StorageStreamOffsetLessThanZero)
            End If
            If count < 0 Then
                Throw New ArgumentOutOfRangeException("count", GlobalLocalizedMessages.StorageStreamCountLessThanZero)
            End If
            If offset + count > buffer.Length Then
                Throw New ArgumentException(GlobalLocalizedMessages.StorageStreamBufferOverflow, "count")
            End If

            Dim bytesRead As Integer = 0
            If count > 0 Then
                Dim ptr As IntPtr = Marshal.AllocCoTaskMem(8)
                Try
                    If offset = 0 Then
                        _stream.Read(buffer, count, ptr)
                        bytesRead = CInt(Marshal.ReadInt64(ptr))
                    Else
                        Dim tempBuffer As Byte() = New Byte(count - 1) {}
                        _stream.Read(tempBuffer, count, ptr)

                        bytesRead = CInt(Marshal.ReadInt64(ptr))
                        If bytesRead > 0 Then
                            Array.Copy(tempBuffer, 0, buffer, offset, bytesRead)
                        End If
                    End If
                Finally
                    Marshal.FreeCoTaskMem(ptr)
                End Try
            End If
            Return bytesRead
        End Function

        ''' <summary>
        ''' Writes a buffer to the stream if able to do so.
        ''' </summary>
        ''' <param name="buffer">Buffer to write</param>
        ''' <param name="offset">Offset in buffer to start writing</param>
        ''' <param name="count">Number of bytes to write to the stream</param>
        Public Overrides Sub Write(buffer As Byte(), offset As Integer, count As Integer)
            ThrowIfDisposed()

            If _isReadOnly Then
                Throw New InvalidOperationException(GlobalLocalizedMessages.StorageStreamIsReadonly)
            End If
            If buffer Is Nothing Then
                Throw New ArgumentNullException("buffer")
            End If
            If offset < 0 Then
                Throw New ArgumentOutOfRangeException("offset", GlobalLocalizedMessages.StorageStreamOffsetLessThanZero)
            End If
            If count < 0 Then
                Throw New ArgumentOutOfRangeException("count", GlobalLocalizedMessages.StorageStreamCountLessThanZero)
            End If
            If offset + count > buffer.Length Then
                Throw New ArgumentException(GlobalLocalizedMessages.StorageStreamBufferOverflow, "count")
            End If

            If count > 0 Then
                Dim ptr As IntPtr = Marshal.AllocCoTaskMem(8)
                Try
                    If offset = 0 Then
                        _stream.Write(buffer, count, ptr)
                    Else
                        Dim tempBuffer As Byte() = New Byte(count - 1) {}
                        Array.Copy(buffer, offset, tempBuffer, 0, count)
                        _stream.Write(tempBuffer, count, ptr)
                    End If
                Finally
                    Marshal.FreeCoTaskMem(ptr)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Gets the length of the IStream
        ''' </summary>
        Public Overrides ReadOnly Property Length() As Long
            Get
                ThrowIfDisposed()
                Const STATFLAG_NONAME As Integer = 1
                Dim stats As System.Runtime.InteropServices.ComTypes.STATSTG
                _stream.Stat(stats, STATFLAG_NONAME)
                Return stats.cbSize
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the current position within the underlying IStream.
        ''' </summary>
        Public Overrides Property Position() As Long
            Get
                ThrowIfDisposed()
                Return Seek(0, SeekOrigin.Current)
            End Get
            Set
                ThrowIfDisposed()
                Seek(Value, SeekOrigin.Begin)
            End Set
        End Property

        ''' <summary>
        ''' Seeks within the underlying IStream.
        ''' </summary>
        ''' <param name="offset">Offset</param>
        ''' <param name="origin">Where to start seeking</param>
        ''' <returns></returns>
        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
            ThrowIfDisposed()
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(8)
            Try
                _stream.Seek(offset, CInt(origin), ptr)
                Return Marshal.ReadInt64(ptr)
            Finally
                Marshal.FreeCoTaskMem(ptr)
            End Try
        End Function

        ''' <summary>
        ''' Sets the length of the stream
        ''' </summary>
        ''' <param name="value"></param>
        Public Overrides Sub SetLength(value As Long)
            ThrowIfDisposed()
            _stream.SetSize(value)
        End Sub

        ''' <summary>
        ''' Commits data to be written to the stream if it is being cached.
        ''' </summary>
        Public Overrides Sub Flush()
            _stream.Commit(CInt(StorageStreamCommitOptions.None))
        End Sub

        ''' <summary>
        ''' Disposes the stream.
        ''' </summary>
        ''' <param name="disposing">True if called from Dispose(), false if called from finalizer.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            _stream = Nothing
            MyBase.Dispose(disposing)
        End Sub

        Private Sub ThrowIfDisposed()
            If _stream Is Nothing Then
                Throw New ObjectDisposedException([GetType]().Name)
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Options for commiting (flushing) an IStream storage stream
    ''' </summary>
    <Flags>
    Friend Enum StorageStreamCommitOptions
        ''' <summary>
        ''' Uses default options
        ''' </summary>
        None = 0

        ''' <summary>
        ''' Overwrite option
        ''' </summary>
        Overwrite = 1

        ''' <summary>
        ''' Only if current
        ''' </summary>
        OnlyIfCurrent = 2

        ''' <summary>
        ''' Commits to disk cache dangerously
        ''' </summary>
        DangerouslyCommitMerelyToDiskCache = 4

        ''' <summary>
        ''' Consolidate
        ''' </summary>
        Consolidate = 8
    End Enum
End Namespace
