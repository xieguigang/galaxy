Imports System.Runtime.InteropServices

Namespace Shell
	''' <summary>
	''' A wrapper for the native POINT structure.
	''' </summary>
	<StructLayout(LayoutKind.Sequential)> _
	Public Structure NativePoint
        ''' <summary>
        ''' Initialize the NativePoint
        ''' </summary>
        ''' <param name="x__1">The x coordinate of the point.</param>
        ''' <param name="y__2">The y coordinate of the point.</param>
        Public Sub New(x__1 As Integer, y__2 As Integer)
			Me.New()
			X = x__1
			Y = y__2
		End Sub

		''' <summary>
		''' The X coordinate of the point
		''' </summary>        
		Public Property X() As Integer
			Get
				Return m_X
			End Get
			Set
				m_X = Value
			End Set
		End Property
		Private m_X As Integer

		''' <summary>
		''' The Y coordinate of the point
		''' </summary>                                
		Public Property Y() As Integer
			Get
				Return m_Y
			End Get
			Set
				m_Y = Value
			End Set
		End Property
		Private m_Y As Integer

		''' <summary>
		''' Determines if two NativePoints are equal.
		''' </summary>
		''' <param name="first">First NativePoint</param>
		''' <param name="second">Second NativePoint</param>
		''' <returns>True if first NativePoint is equal to the second; false otherwise.</returns>
		Public Shared Operator =(first As NativePoint, second As NativePoint) As Boolean
			Return first.X = second.X AndAlso first.Y = second.Y
		End Operator

		''' <summary>
		''' Determines if two NativePoints are not equal.
		''' </summary>
		''' <param name="first">First NativePoint</param>
		''' <param name="second">Second NativePoint</param>
		''' <returns>True if first NativePoint is not equal to the second; false otherwise.</returns>
		Public Shared Operator <>(first As NativePoint, second As NativePoint) As Boolean
			Return Not (first = second)
		End Operator

		''' <summary>
		''' Determines if this NativePoint is equal to another.
		''' </summary>
		''' <param name="obj">Another NativePoint to compare</param>
		''' <returns>True if this NativePoint is equal obj; false otherwise.</returns>
		Public Overrides Function Equals(obj As Object) As Boolean
			Return If((obj IsNot Nothing AndAlso TypeOf obj Is NativePoint), Me = CType(obj, NativePoint), False)
		End Function

		''' <summary>
		''' Gets a hash code for the NativePoint.
		''' </summary>
		''' <returns>Hash code for the NativePoint</returns>
		Public Overrides Function GetHashCode() As Integer
			Dim hash As Integer = X.GetHashCode()
			hash = hash * 31 + Y.GetHashCode()
			Return hash
		End Function
	End Structure

End Namespace
