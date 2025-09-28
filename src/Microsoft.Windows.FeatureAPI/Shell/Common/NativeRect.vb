Imports System.Runtime.InteropServices

Namespace Shell
	''' <summary>
	''' A wrapper for a RECT struct
	''' </summary>
	<StructLayout(LayoutKind.Sequential)> _
	Public Structure NativeRect
		''' <summary>
		''' Position of left edge
		''' </summary>            
		Public Property Left() As Integer
			Get
				Return m_Left
			End Get
			Set
				m_Left = Value
			End Set
		End Property
		Private m_Left As Integer

		''' <summary>
		''' Position of top edge
		''' </summary>            
		Public Property Top() As Integer
			Get
				Return m_Top
			End Get
			Set
				m_Top = Value
			End Set
		End Property
		Private m_Top As Integer

		''' <summary>
		''' Position of right edge
		''' </summary>            
		Public Property Right() As Integer
			Get
				Return m_Right
			End Get
			Set
				m_Right = Value
			End Set
		End Property
		Private m_Right As Integer

		''' <summary>
		''' Position of bottom edge
		''' </summary>            
		Public Property Bottom() As Integer
			Get
				Return m_Bottom
			End Get
			Set
				m_Bottom = Value
			End Set
		End Property
		Private m_Bottom As Integer

        ''' <summary>
        ''' Creates a new NativeRect initialized with supplied values.
        ''' </summary>
        ''' <param name="left__1">Position of left edge</param>
        ''' <param name="top__2">Position of top edge</param>
        ''' <param name="right__3">Position of right edge</param>
        ''' <param name="bottom__4">Position of bottom edge</param>
        Public Sub New(left__1 As Integer, top__2 As Integer, right__3 As Integer, bottom__4 As Integer)
			Me.New()
			Left = left__1
			Top = top__2
			Right = right__3
			Bottom = bottom__4
		End Sub

		''' <summary>
		''' Determines if two NativeRects are equal.
		''' </summary>
		''' <param name="first">First NativeRect</param>
		''' <param name="second">Second NativeRect</param>
		''' <returns>True if first NativeRect is equal to second; false otherwise.</returns>
		Public Shared Operator =(first As NativeRect, second As NativeRect) As Boolean
			Return first.Left = second.Left AndAlso first.Top = second.Top AndAlso first.Right = second.Right AndAlso first.Bottom = second.Bottom
		End Operator

		''' <summary>
		''' Determines if two NativeRects are not equal
		''' </summary>
		''' <param name="first">First NativeRect</param>
		''' <param name="second">Second NativeRect</param>
		''' <returns>True if first is not equal to second; false otherwise.</returns>
		Public Shared Operator <>(first As NativeRect, second As NativeRect) As Boolean
			Return Not (first = second)
		End Operator

		''' <summary>
		''' Determines if the NativeRect is equal to another Rect.
		''' </summary>
		''' <param name="obj">Another NativeRect to compare</param>
		''' <returns>True if this NativeRect is equal to the one provided; false otherwise.</returns>
		Public Overrides Function Equals(obj As Object) As Boolean
			Return If((obj IsNot Nothing AndAlso TypeOf obj Is NativeRect), Me = CType(obj, NativeRect), False)
		End Function

		''' <summary>
		''' Creates a hash code for the NativeRect
		''' </summary>
		''' <returns>Returns hash code for this NativeRect</returns>
		Public Overrides Function GetHashCode() As Integer
			Dim hash As Integer = Left.GetHashCode()
			hash = hash * 31 + Top.GetHashCode()
			hash = hash * 31 + Right.GetHashCode()
			hash = hash * 31 + Bottom.GetHashCode()
			Return hash
		End Function
	End Structure

End Namespace
