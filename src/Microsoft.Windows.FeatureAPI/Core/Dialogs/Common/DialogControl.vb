'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Dialogs
	''' <summary>
	''' Abstract base class for all dialog controls
	''' </summary>
	Public MustInherit Class DialogControl
		Private Shared nextId As Integer = DialogsDefaults.MinimumDialogControlId

		''' <summary>
		''' Creates a new instance of a dialog control
		''' </summary>
		Protected Sub New()
			Id = nextId

			' Support wrapping of control IDs in case you create a lot of custom controls
			If nextId = Int32.MaxValue Then
				nextId = DialogsDefaults.MinimumDialogControlId
			Else
				nextId += 1
			End If
		End Sub

		''' <summary>
		''' Creates a new instance of a dialog control with the specified name.
		''' </summary>
		''' <param name="name__1">The name for this dialog.</param>
		Protected Sub New(name__1 As String)
			Me.New()
			Name = name__1
		End Sub

		''' <summary>
		''' The native dialog that is hosting this control. This property is null is
		''' there is not associated dialog
		''' </summary>
		Public Property HostingDialog() As IDialogControlHost
			Get
				Return m_HostingDialog
			End Get
			Set
				m_HostingDialog = Value
			End Set
		End Property
		Private m_HostingDialog As IDialogControlHost

		Private m_name As String
		''' <summary>
		''' Gets the name for this control.
		''' </summary>
		''' <value>A <see cref="System.String"/> value.</value>        
		Public Property Name() As String
			Get
				Return m_name
			End Get
			Set
				' Names for controls need to be quite stable, 
				' as we are going to maintain a mapping between 
				' the names and the underlying Win32/COM control IDs.
				If String.IsNullOrEmpty(Value) Then
					Throw New ArgumentException(GlobalLocalizedMessages.DialogControlNameCannotBeEmpty)
				End If

				If Not String.IsNullOrEmpty(m_name) Then
					Throw New InvalidOperationException(GlobalLocalizedMessages.DialogControlsCannotBeRenamed)
				End If

				' Note that we don't notify the hosting dialog of 
				' the change, as the initial set of name is (must be)
				' always legal, and renames are always illegal.
				Me.m_name = Value
			End Set
		End Property

		''' <summary>
		''' Gets the identifier for this control.
		''' </summary>
		''' <value>An <see cref="System.Int32"/> value.</value>
		Public Property Id() As Integer
			Get
				Return m_Id
			End Get
			Private Set
				m_Id = Value
			End Set
		End Property
		Private m_Id As Integer

		'''<summary>
		''' Calls the hosting dialog, if it exists, to check whether the 
		''' property can be set in the dialog's current state. 
		''' The host should throw an exception if the change is not supported.
		''' Note that if the dialog isn't set yet, 
		''' there are no restrictions on setting the property.
		''' </summary>
		''' <param name="propName">The name of the property that is changing</param>
		Protected Sub CheckPropertyChangeAllowed(propName As String)
			System.Diagnostics.Debug.Assert(Not String.IsNullOrEmpty(propName), "Property to change was not specified")

			If HostingDialog IsNot Nothing Then
				' This will throw if the property change is not allowed.
				HostingDialog.IsControlPropertyChangeAllowed(propName, Me)
			End If
		End Sub

		'''<summary>
		''' Calls the hosting dialog, if it exists, to
		''' to indicate that a property has changed, and that 
		''' the dialog should do whatever is necessary 
		''' to propagate the change to the native control.
		''' Note that if the dialog isn't set yet, 
		''' there are no restrictions on setting the property.
		''' </summary>
		''' <param name="propName">The name of the property that is changing.</param>
		Protected Sub ApplyPropertyChange(propName As String)
			System.Diagnostics.Debug.Assert(Not String.IsNullOrEmpty(propName), "Property changed was not specified")

			If HostingDialog IsNot Nothing Then
				HostingDialog.ApplyControlPropertyChange(propName, Me)
			End If
		End Sub

		''' <summary>
		''' Compares two objects to determine whether they are equal
		''' </summary>
		''' <param name="obj">The object to compare against.</param>
		''' <returns>A <see cref="System.Boolean"/> value.</returns>
		Public Overrides Function Equals(obj As Object) As Boolean
			Dim control As DialogControl = TryCast(obj, DialogControl)

			If control IsNot Nothing Then
				Return (Me.Id = control.Id)
			End If

			Return False
		End Function

		''' <summary>
		''' Serves as a hash function for a particular type. 
		''' </summary>
		''' <returns>An <see cref="System.Int32"/> hash code for this control.</returns>
		Public Overrides Function GetHashCode() As Integer
			If Name Is Nothing Then
				Return Me.ToString().GetHashCode()
			End If

			Return Name.GetHashCode()
		End Function
	End Class
End Namespace
