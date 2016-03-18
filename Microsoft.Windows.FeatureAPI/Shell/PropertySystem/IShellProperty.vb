'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports Microsoft.Windows.Shell

Namespace Shell.PropertySystem
	''' <summary>
	''' Defines the properties used by a Shell Property.
	''' </summary>
	Public Interface IShellProperty
		''' <summary>
		''' Gets the property key that identifies this property.
		''' </summary>
		ReadOnly Property PropertyKey() As PropertyKey

		''' <summary>
		''' Gets a formatted, Unicode string representation of a property value.
		''' </summary>
		''' <param name="format">One or more <c>PropertyDescriptionFormat</c> flags 
		''' chosen to produce the desired display format.</param>
		''' <returns>The formatted value as a string.</returns>
		Function FormatForDisplay(format As PropertyDescriptionFormatOptions) As String

		''' <summary>
		''' Get the property description object.
		''' </summary>
		ReadOnly Property Description() As ShellPropertyDescription

		''' <summary>
		''' Gets the case-sensitive name of the property as it is known to the system, 
		''' regardless of its localized name.
		''' </summary>
		ReadOnly Property CanonicalName() As String

		''' <summary>
		''' Gets the value for this property using the generic Object type.
		''' </summary>
		''' <remarks>
		''' To obtain a specific type for this value, use the more strongly-typed 
		''' <c>Property&lt;T&gt;</c> class.
		''' You can only set a value for this type using the <c>Property&lt;T&gt;</c> 
		''' class.
		''' </remarks>
		ReadOnly Property ValueAsObject() As Object

		''' <summary>
		''' Gets the <c>System.Type</c> value for this property.
		''' </summary>
		ReadOnly Property ValueType() As Type

		''' <summary>
		''' Gets the image reference path and icon index associated with a property value. 
		''' This API is only available in Windows 7.
		''' </summary>
		ReadOnly Property IconReference() As IconReference
	End Interface
End Namespace
