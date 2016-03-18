' Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace ExtendedLinguisticServices

	''' <summary>
	''' A <see cref="MappingAsyncResult">MappingAsyncResult</see> subclass to be used only for asynchronous calls to service actions.
	''' <seealso cref="MappingService.BeginDoAction">MappingService.BeginDoAction</seealso>
	''' </summary>
	Public Class MappingActionAsyncResult
		Inherits MappingAsyncResult

		Friend Sub New(callerData As Object, asyncCallback As AsyncCallback, bag As MappingPropertyBag, rangeIndex__1 As Integer, actionId__2 As String)
			MyBase.New(callerData, asyncCallback)
			MyBase.SetResult(bag, New MappingResultState())
			RangeIndex = rangeIndex__1
			ActionId = actionId__2
		End Sub

		''' <summary>
		''' Gets the range index parameter for <see cref="MappingService.DoAction">MappingService.DoAction</see> or <see cref="MappingService.BeginDoAction">MappingService.BeginDoAction</see>.
		''' </summary>
		Public Property RangeIndex() As Integer
			Get
				Return m_RangeIndex
			End Get
			Private Set
				m_RangeIndex = Value
			End Set
		End Property
		Private m_RangeIndex As Integer

		''' <summary>
		''' Gets the action ID parameter for <see cref="MappingService.DoAction">MappingService.DoAction</see> or <see cref="MappingService.BeginDoAction">MappingService.BeginDoAction</see>.
		''' </summary>
		Public Property ActionId() As String
			Get
				Return m_ActionId
			End Get
			Private Set
				m_ActionId = Value
			End Set
		End Property
		Private m_ActionId As String
	End Class

End Namespace
