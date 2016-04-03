Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Namespace Animations
	Class AnimationManager
		Public Property InterruptAnimation() As Boolean
			Get
				Return m_InterruptAnimation
			End Get
			Set
				m_InterruptAnimation = Value
			End Set
		End Property
		Private m_InterruptAnimation As Boolean
		Public Property Increment() As Double
			Get
				Return m_Increment
			End Get
			Set
				m_Increment = Value
			End Set
		End Property
		Private m_Increment As Double
		Public Property SecondaryIncrement() As Double
			Get
				Return m_SecondaryIncrement
			End Get
			Set
				m_SecondaryIncrement = Value
			End Set
		End Property
		Private m_SecondaryIncrement As Double
		Public Property AnimationType() As AnimationType
			Get
				Return m_AnimationType
			End Get
			Set
				m_AnimationType = Value
			End Set
		End Property
		Private m_AnimationType As AnimationType
		Public Property Singular() As Boolean
			Get
				Return m_Singular
			End Get
			Set
				m_Singular = Value
			End Set
		End Property
		Private m_Singular As Boolean

		Public Delegate Sub AnimationFinished(sender As Object)
		Public Event OnAnimationFinished As AnimationFinished

		Public Delegate Sub AnimationProgress(sender As Object)
		Public Event OnAnimationProgress As AnimationProgress

		Private ReadOnly animationProgresses As List(Of Double)
		Private ReadOnly animationSources As List(Of Point)
		Private ReadOnly animationDirections As List(Of AnimationDirection)
		Private ReadOnly animationDatas As List(Of Object())

		Private Const MIN_VALUE As Double = 0.0
		Private Const MAX_VALUE As Double = 1.0

        Private ReadOnly animationTimer As New Timer() With {
         .Interval = 5,
         .Enabled = False
        }

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="singular__1">
        ''' If true, only one animation is supported. The current animation will be replaced with the new one. If false, a new animation is added to the list.
        ''' </param>
        Public Sub New(Optional singular__1 As Boolean = True)
			animationProgresses = New List(Of Double)()
			animationSources = New List(Of Point)()
			animationDirections = New List(Of AnimationDirection)()
			animationDatas = New List(Of Object())()

			Increment = 0.03
			SecondaryIncrement = 0.03
			AnimationType = AnimationType.Linear
			InterruptAnimation = True
			Singular = singular__1

			If Singular Then
				animationProgresses.Add(0)
				animationSources.Add(New Point(0, 0))
				animationDirections.Add(AnimationDirection.[In])
			End If

			AddHandler animationTimer.Tick, AddressOf AnimationTimerOnTick
		End Sub

		Private Sub AnimationTimerOnTick(sender As Object, eventArgs As EventArgs)
			For i As Integer = 0 To animationProgresses.Count - 1
				UpdateProgress(i)

				If Not Singular Then
					If (animationDirections(i) = AnimationDirection.InOutIn AndAlso animationProgresses(i) = MAX_VALUE) Then
						animationDirections(i) = AnimationDirection.InOutOut
					ElseIf (animationDirections(i) = AnimationDirection.InOutRepeatingIn AndAlso animationProgresses(i) = MIN_VALUE) Then
						animationDirections(i) = AnimationDirection.InOutRepeatingOut
					ElseIf (animationDirections(i) = AnimationDirection.InOutRepeatingOut AndAlso animationProgresses(i) = MIN_VALUE) Then
						animationDirections(i) = AnimationDirection.InOutRepeatingIn
					ElseIf (animationDirections(i) = AnimationDirection.[In] AndAlso animationProgresses(i) = MAX_VALUE) OrElse (animationDirections(i) = AnimationDirection.Out AndAlso animationProgresses(i) = MIN_VALUE) OrElse (animationDirections(i) = AnimationDirection.InOutOut AndAlso animationProgresses(i) = MIN_VALUE) Then
						animationProgresses.RemoveAt(i)
						animationSources.RemoveAt(i)
						animationDirections.RemoveAt(i)
						animationDatas.RemoveAt(i)
					End If
				Else
					If (animationDirections(i) = AnimationDirection.InOutIn AndAlso animationProgresses(i) = MAX_VALUE) Then
						animationDirections(i) = AnimationDirection.InOutOut
					ElseIf (animationDirections(i) = AnimationDirection.InOutRepeatingIn AndAlso animationProgresses(i) = MAX_VALUE) Then
						animationDirections(i) = AnimationDirection.InOutRepeatingOut
					ElseIf (animationDirections(i) = AnimationDirection.InOutRepeatingOut AndAlso animationProgresses(i) = MIN_VALUE) Then
						animationDirections(i) = AnimationDirection.InOutRepeatingIn
					End If
				End If
			Next

			RaiseEvent OnAnimationProgress(Me)
		End Sub

		Public Function IsAnimating() As Boolean
			Return animationTimer.Enabled
		End Function

		Public Sub StartNewAnimation(animationDirection As AnimationDirection, Optional data As Object() = Nothing)
			StartNewAnimation(animationDirection, New Point(0, 0), data)
		End Sub

		Public Sub StartNewAnimation(animationDirection__1 As AnimationDirection, animationSource As Point, Optional data As Object() = Nothing)
			If Not IsAnimating() OrElse InterruptAnimation Then
				If Singular AndAlso animationDirections.Count > 0 Then
					animationDirections(0) = animationDirection__1
				Else
					animationDirections.Add(animationDirection__1)
				End If

				If Singular AndAlso animationSources.Count > 0 Then
					animationSources(0) = animationSource
				Else
					animationSources.Add(animationSource)
				End If

				If Not (Singular AndAlso animationProgresses.Count > 0) Then
					Select Case animationDirections(animationDirections.Count - 1)
						Case AnimationDirection.InOutRepeatingIn, AnimationDirection.InOutIn, AnimationDirection.[In]
							animationProgresses.Add(MIN_VALUE)
							Exit Select
						Case AnimationDirection.InOutRepeatingOut, AnimationDirection.InOutOut, AnimationDirection.Out
							animationProgresses.Add(MAX_VALUE)
							Exit Select
						Case Else
							Throw New Exception("Invalid AnimationDirection")
					End Select
				End If

				If Singular AndAlso animationDatas.Count > 0 Then
					animationDatas(0) = If(data, New Object() {})
				Else
					animationDatas.Add(If(data, New Object() {}))

				End If
			End If

			animationTimer.Start()
		End Sub

		Public Sub UpdateProgress(index As Integer)
			Select Case animationDirections(index)
				Case AnimationDirection.InOutRepeatingIn, AnimationDirection.InOutIn, AnimationDirection.[In]
					IncrementProgress(index)
					Exit Select
				Case AnimationDirection.InOutRepeatingOut, AnimationDirection.InOutOut, AnimationDirection.Out
					DecrementProgress(index)
					Exit Select
				Case Else
					Throw New Exception("No AnimationDirection has been set")
			End Select
		End Sub

		Private Sub IncrementProgress(index As Integer)
			animationProgresses(index) += Increment
			If animationProgresses(index) > MAX_VALUE Then
				animationProgresses(index) = MAX_VALUE

				For i As Integer = 0 To GetAnimationCount() - 1
					If animationDirections(i) = AnimationDirection.InOutIn Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.InOutRepeatingIn Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.InOutRepeatingOut Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.InOutOut AndAlso animationProgresses(i) <> MAX_VALUE Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.[In] AndAlso animationProgresses(i) <> MAX_VALUE Then
						Return
					End If
				Next

				animationTimer.[Stop]()
				RaiseEvent OnAnimationFinished(Me)
			End If
		End Sub

		Private Sub DecrementProgress(index As Integer)
			animationProgresses(index) -= If((animationDirections(index) = AnimationDirection.InOutOut OrElse animationDirections(index) = AnimationDirection.InOutRepeatingOut), SecondaryIncrement, Increment)
			If animationProgresses(index) < MIN_VALUE Then
				animationProgresses(index) = MIN_VALUE

				For i As Integer = 0 To GetAnimationCount() - 1
					If animationDirections(i) = AnimationDirection.InOutIn Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.InOutRepeatingIn Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.InOutRepeatingOut Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.InOutOut AndAlso animationProgresses(i) <> MIN_VALUE Then
						Return
					End If
					If animationDirections(i) = AnimationDirection.Out AndAlso animationProgresses(i) <> MIN_VALUE Then
						Return
					End If
				Next

				animationTimer.[Stop]()
				RaiseEvent OnAnimationFinished(Me)
			End If
		End Sub

		Public Function GetProgress() As Double
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationProgresses.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			Return GetProgress(0)
		End Function

		Public Function GetProgress(index As Integer) As Double
			If Not (index < GetAnimationCount()) Then
				Throw New IndexOutOfRangeException("Invalid animation index")
			End If

			Select Case AnimationType
				Case AnimationType.Linear
					Return AnimationLinear.CalculateProgress(animationProgresses(index))
				Case AnimationType.EaseInOut
					Return AnimationEaseInOut.CalculateProgress(animationProgresses(index))
				Case AnimationType.EaseOut
					Return AnimationEaseOut.CalculateProgress(animationProgresses(index))
				Case AnimationType.CustomQuadratic
					Return AnimationCustomQuadratic.CalculateProgress(animationProgresses(index))
				Case Else
					Throw New NotImplementedException("The given AnimationType is not implemented")
			End Select

		End Function

		Public Function GetSource(index As Integer) As Point
			If Not (index < GetAnimationCount()) Then
				Throw New IndexOutOfRangeException("Invalid animation index")
			End If

			Return animationSources(index)
		End Function

		Public Function GetSource() As Point
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationSources.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			Return animationSources(0)
		End Function

		Public Function GetDirection() As AnimationDirection
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationDirections.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			Return animationDirections(0)
		End Function

		Public Function GetDirection(index As Integer) As AnimationDirection
			If Not (index < animationDirections.Count) Then
				Throw New IndexOutOfRangeException("Invalid animation index")
			End If

			Return animationDirections(index)
		End Function

		Public Function GetData() As Object()
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationDatas.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			Return animationDatas(0)
		End Function

		Public Function GetData(index As Integer) As Object()
			If Not (index < animationDatas.Count) Then
				Throw New IndexOutOfRangeException("Invalid animation index")
			End If

			Return animationDatas(index)
		End Function

		Public Function GetAnimationCount() As Integer
			Return animationProgresses.Count
		End Function

		Public Sub SetProgress(progress As Double)
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationProgresses.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			animationProgresses(0) = progress
		End Sub

		Public Sub SetDirection(direction As AnimationDirection)
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationProgresses.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			animationDirections(0) = direction
		End Sub

		Public Sub SetData(data As Object())
			If Not Singular Then
				Throw New Exception("Animation is not set to Singular.")
			End If

			If animationDatas.Count = 0 Then
				Throw New Exception("Invalid animation")
			End If

			animationDatas(0) = data
		End Sub
	End Class
End Namespace
