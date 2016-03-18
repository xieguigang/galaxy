' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Threading

Namespace ExtendedLinguisticServices

	''' <summary>
	''' <see cref="System.IAsyncResult">IAsyncResult</see> implementation for use with asynchronous calls to ELS.
	''' </summary>
	Public Class MappingAsyncResult
		Implements IAsyncResult
		Implements IDisposable
		Private _callerData As Object
		Private _bag As MappingPropertyBag
		Private _resultState As MappingResultState
		Private _waitHandle As ManualResetEvent
		Private _asyncCallback As AsyncCallback

		Friend Sub New(callerData As Object, asyncCallback As AsyncCallback)
			_callerData = callerData
			_asyncCallback = asyncCallback
			_waitHandle = New ManualResetEvent(False)
		End Sub

		Friend ReadOnly Property AsyncCallback() As AsyncCallback
			Get
				Return _asyncCallback
			End Get
		End Property

		''' <summary>
		''' Queries whether the operation completed successfully.
		''' </summary>
		Public ReadOnly Property Succeeded() As Boolean
			Get
				Return _bag IsNot Nothing AndAlso _resultState.HResult = 0
			End Get
		End Property

		''' <summary>
		''' Gets the resulting <see cref="MappingPropertyBag">MappingPropertyBag</see> (if it exists).
		''' </summary>
		Public ReadOnly Property PropertyBag() As MappingPropertyBag
			Get
				Return _bag
			End Get
		End Property

		''' <summary>
		''' Returns the current result state associated with this operation.
		''' </summary>
		Public ReadOnly Property ResultState() As MappingResultState
			Get
				Return _resultState
			End Get
		End Property

		''' <summary>
		''' Returns the caller data associated with this operation.
		''' </summary>
		Public ReadOnly Property CallerData() As Object
			Get
				Return _callerData
			End Get
		End Property

		Friend Sub SetResult(bag As MappingPropertyBag, resultState As MappingResultState)
			_resultState = resultState
			_bag = bag
		End Sub

		#Region "IAsyncResult Members"

		' returns MappingResultState
		''' <summary>
		''' Returns the result state.
		''' </summary>
		Public ReadOnly Property AsyncState() As Object Implements IAsyncResult.AsyncState
			Get
				Return ResultState
			End Get
		End Property

		''' <summary>
		''' Gets the WaitHandle which will be notified when
		''' the opration completes (successfully or not).
		''' </summary>
		Public ReadOnly Property AsyncWaitHandle() As WaitHandle Implements IAsyncResult.AsyncWaitHandle
			Get
				Return _waitHandle
			End Get
		End Property

		''' <summary>
		''' From MSDN:
		''' Most implementers of the IAsyncResult interface
		''' will not use this property and should return false.
		''' </summary>
		Public ReadOnly Property CompletedSynchronously() As Boolean Implements IAsyncResult.CompletedSynchronously
			Get
				Return False
			End Get
		End Property

		''' <summary>
		''' Queries whether the operation has completed.
		''' </summary>
		Public ReadOnly Property IsCompleted() As Boolean Implements IAsyncResult.IsCompleted
			Get
				Thread.MemoryBarrier()
				Return AsyncWaitHandle.WaitOne(0, False)
			End Get
		End Property

		#End Region

		#Region "IDisposable Members"

		''' <summary>
		''' Dispose the MappingAsyncresult
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Dispose the MappingAsyncresult
		''' </summary>
		Protected Overridable Sub Dispose(disposed As Boolean)
			If disposed Then
				_waitHandle.Close()
			End If
		End Sub

		#End Region
	End Class

End Namespace
