' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Threading

Namespace ExtendedLinguisticServices

	''' <summary>
	''' Contains the text recognition data properties retrieved by MappingService.RecognizeText or
	''' MappingService.BeginRecognizeText.
	''' </summary>
	Public Class MappingPropertyBag
		Inherits CriticalFinalizerObject
		Implements IDisposable
		Friend _win32PropertyBag As Win32PropertyBag
		Friend _options As IntPtr = IntPtr.Zero
		Friend _text As GCHandle
		Private _serviceCache As ServiceCache
		Private _isFinalized As Integer

		Friend Sub New(options As MappingOptions, text As String)
			_serviceCache = ServiceCache.Instance
			If Not _serviceCache.RegisterResource() Then
				Throw New LinguisticException()
			End If
			_win32PropertyBag._size = InteropTools.SizeOfWin32PropertyBag
			If options IsNot Nothing Then
				_options = InteropTools.Pack(options._win32Options)
			End If
			_text = GCHandle.Alloc(text, GCHandleType.Pinned)
		End Sub

		''' <summary>
		''' Frees all unmanaged resources allocated for the property bag, if needed.
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		''' <summary>
		''' An array of <see cref="MappingDataRange">MappingDataRange</see> objects containing all recognized text range results. This member is populated
		''' by MappingService.RecognizeText or asynchronously with
		''' MappingService.BeginRecognizeText.
		''' </summary>
		Public Function GetResultRanges() As MappingDataRange()
            Dim result As MappingDataRange() = New MappingDataRange(CInt(_win32PropertyBag._rangesCount - 1)) {}
            For i As Integer = 0 To result.Length - 1
				Dim range As New MappingDataRange()
				range._win32DataRange = InteropTools.Unpack(Of Win32DataRange)(CType(CType(_win32PropertyBag._ranges, UInt64) + (CType(i, UInt64) * InteropTools.SizeOfWin32DataRange), IntPtr))
				result(i) = range
			Next
			Return result
		End Function

        ''' <summary>
        ''' Formats the low-level data contained in this <see cref="MappingPropertyBag">MappingPropertyBag</see> using an implementation of the
        ''' <see cref="IMappingFormatter(Of T)">IMappingFormatter</see> interface.
        ''' </summary>
        ''' <typeparam name="T">The type with which <see cref="IMappingFormatter(Of T)">IMappingFormatter</see> is parameterized.</typeparam>
        ''' <param name="formatter">The formatter to be used in the formatting.</param>
        ''' <returns></returns>
        Public Function FormatData(Of T)(formatter As IMappingFormatter(Of T)) As T()
			If formatter Is Nothing Then
				Throw New ArgumentNullException("formatter")
			End If
			Return formatter.FormatAll(Me)
		End Function

		Private Function DisposeInternal() As Boolean
			If _win32PropertyBag._context = IntPtr.Zero Then
				Return True
			End If
			Dim hResult As UInt32 = Win32NativeMethods.MappingFreePropertyBag(_win32PropertyBag)
			If hResult <> 0 Then
				Throw New LinguisticException(hResult)
			End If
			Return True
		End Function

		''' <summary>
		''' Frees all unmanaged resources allocated for the property bag.
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Clean up both managed and native resources.
		''' </summary>
		''' <param name="disposed"></param>
		Protected Overridable Sub Dispose(disposed As Boolean)
			If Interlocked.CompareExchange(_isFinalized, 0, 0) = 0 Then
				Dim result As Boolean = DisposeInternal()
				If result Then
					_serviceCache.UnregisterResource()
					InteropTools.Free(Of Win32Options)(_options)
					_text.Free()
					Interlocked.CompareExchange(_isFinalized, 1, 0)
				End If
			End If
		End Sub
	End Class

End Namespace
