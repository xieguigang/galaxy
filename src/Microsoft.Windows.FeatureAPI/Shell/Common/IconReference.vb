'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	''' <summary>
	''' A refence to an icon resource 
	''' </summary>    
	Public Structure IconReference
#Region "Private members"

		Private m_moduleName As String
		Private m_referencePath As String
		Private Shared commaSeparator As Char() = New Char() {","c}

#End Region

		''' <summary>
		''' Overloaded constructor takes in the module name and resource id for the icon reference.
		''' </summary>
		''' <param name="moduleName">String specifying the name of an executable file, DLL, or icon file</param>
		''' <param name="resourceId__1">Zero-based index of the icon</param>
		Public Sub New(moduleName As String, resourceId__1 As Integer)
			Me.New()
			If String.IsNullOrEmpty(moduleName) Then
				Throw New ArgumentNullException("moduleName")
			End If

			Me.m_moduleName = moduleName
			ResourceId = resourceId__1
			m_referencePath = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0},{1}", moduleName, resourceId__1)
		End Sub

		''' <summary>
		''' Overloaded constructor takes in the module name and resource id separated by a comma.
		''' </summary>
		''' <param name="refPath">Reference path for the icon consiting of the module name and resource id.</param>
		Public Sub New(refPath As String)
			Me.New()
			If String.IsNullOrEmpty(refPath) Then
				Throw New ArgumentNullException("refPath")
			End If

			Dim refParams As String() = refPath.Split(commaSeparator)

			If refParams.Length <> 2 OrElse String.IsNullOrEmpty(refParams(0)) OrElse String.IsNullOrEmpty(refParams(1)) Then
				Throw New ArgumentException(LocalizedMessages.InvalidReferencePath, "refPath")
			End If

			m_moduleName = refParams(0)
			ResourceId = Integer.Parse(refParams(1), System.Globalization.CultureInfo.InvariantCulture)

			Me.m_referencePath = refPath
		End Sub

		''' <summary>
		''' String specifying the name of an executable file, DLL, or icon file
		''' </summary>
		Public Property ModuleName() As String
			Get
				Return m_moduleName
			End Get
			Set
				If String.IsNullOrEmpty(Value) Then
					Throw New ArgumentNullException("value")
				End If
				m_moduleName = Value
			End Set
		End Property

		''' <summary>
		''' Zero-based index of the icon
		''' </summary>
		Public Property ResourceId() As Integer
			Get
				Return m_ResourceId
			End Get
			Set
				m_ResourceId = Value
			End Set
		End Property
		Private m_ResourceId As Integer

		''' <summary>
		''' Reference to a specific icon within a EXE, DLL or icon file.
		''' </summary>
		Public Property ReferencePath() As String
			Get
				Return m_referencePath
			End Get
			Set
				If String.IsNullOrEmpty(Value) Then
					Throw New ArgumentNullException("value")
				End If

				Dim refParams As String() = Value.Split(commaSeparator)

				If refParams.Length <> 2 OrElse String.IsNullOrEmpty(refParams(0)) OrElse String.IsNullOrEmpty(refParams(1)) Then
					Throw New ArgumentException(LocalizedMessages.InvalidReferencePath, "value")
				End If

				ModuleName = refParams(0)
				ResourceId = Integer.Parse(refParams(1), System.Globalization.CultureInfo.InvariantCulture)

				m_referencePath = Value
			End Set
		End Property

		''' <summary>
		''' Implements the == (equality) operator.
		''' </summary>
		''' <param name="icon1">First object to compare.</param>
		''' <param name="icon2">Second object to compare.</param>
		''' <returns>True if icon1 equals icon1; false otherwise.</returns>
		Public Shared Operator =(icon1 As IconReference, icon2 As IconReference) As Boolean
			Return (icon1.ModuleName = icon2.ModuleName) AndAlso (icon1.ReferencePath = icon2.ReferencePath) AndAlso (icon1.ResourceId = icon2.ResourceId)
		End Operator

		''' <summary>
		''' Implements the != (unequality) operator.
		''' </summary>
		''' <param name="icon1">First object to compare.</param>
		''' <param name="icon2">Second object to compare.</param>
		''' <returns>True if icon1 does not equals icon1; false otherwise.</returns>
		Public Shared Operator <>(icon1 As IconReference, icon2 As IconReference) As Boolean
			Return Not (icon1 = icon2)
		End Operator

		''' <summary>
		''' Determines if this object is equal to another.
		''' </summary>
		''' <param name="obj">The object to compare</param>
		''' <returns>Returns true if the objects are equal; false otherwise.</returns>
		Public Overrides Function Equals(obj As Object) As Boolean
			If obj Is Nothing OrElse Not (TypeOf obj Is IconReference) Then
				Return False
			End If
			Return (Me = CType(obj, IconReference))
		End Function

		''' <summary>
		''' Generates a nearly unique hashcode for this structure.
		''' </summary>
		''' <returns>A hash code.</returns>
		Public Overrides Function GetHashCode() As Integer
			Dim hash As Integer = Me.m_moduleName.GetHashCode()
			hash = hash * 31 + Me.m_referencePath.GetHashCode()
			hash = hash * 31 + Me.ResourceId.GetHashCode()
			Return hash
		End Function

	End Structure

End Namespace
