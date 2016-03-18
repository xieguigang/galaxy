'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
	''' <summary>
	''' Defines the shell property description information for a property.
	''' </summary>
	Public Class ShellPropertyDescription
		Implements IDisposable
		#Region "Private Fields"

		Private m_nativePropertyDescription As IPropertyDescription
		Private m_canonicalName As String
		Private m_propertyKey As PropertyKey
		Private m_displayName As String
		Private m_editInvitation As String
		Private m_varEnumType As System.Nullable(Of VarEnum) = Nothing
		Private m_displayType As System.Nullable(Of PropertyDisplayType)
		Private m_aggregationTypes As System.Nullable(Of PropertyAggregationType)
		Private m_defaultColumWidth As System.Nullable(Of UInteger)
		Private propertyTypeFlags As System.Nullable(Of PropertyTypeOptions)
		Private propertyViewFlags As System.Nullable(Of PropertyViewOptions)
		Private m_valueType As Type
		Private m_propertyEnumTypes As ReadOnlyCollection(Of ShellPropertyEnumType)
		Private m_columnState As System.Nullable(Of PropertyColumnStateOptions)
		Private m_conditionType As System.Nullable(Of PropertyConditionType)
		Private m_conditionOperation As System.Nullable(Of PropertyConditionOperation)
		Private m_groupingRange As System.Nullable(Of PropertyGroupingRange)
		Private m_sortDescription As System.Nullable(Of PropertySortDescription)

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' Gets the case-sensitive name of a property as it is known to the system, 
		''' regardless of its localized name.
		''' </summary>
		Public ReadOnly Property CanonicalName() As String
			Get
				If m_canonicalName Is Nothing Then
					PropertySystemNativeMethods.PSGetNameFromPropertyKey(m_propertyKey, m_canonicalName)
				End If

				Return m_canonicalName
			End Get
		End Property

		''' <summary>
		''' Gets the property key identifying the underlying property.
		''' </summary>
		Public ReadOnly Property PropertyKey() As PropertyKey
			Get
				Return m_propertyKey
			End Get
		End Property

		''' <summary>
		''' Gets the display name of the property as it is shown in any user interface (UI).
		''' </summary>
		Public ReadOnly Property DisplayName() As String
			Get
				If NativePropertyDescription IsNot Nothing AndAlso m_displayName Is Nothing Then
					Dim dispNameptr As IntPtr = IntPtr.Zero

					Dim hr As HResult = NativePropertyDescription.GetDisplayName(dispNameptr)

					If CoreErrorHelper.Succeeded(hr) AndAlso dispNameptr <> IntPtr.Zero Then
						m_displayName = Marshal.PtrToStringUni(dispNameptr)

						' Free the string
						Marshal.FreeCoTaskMem(dispNameptr)
					End If
				End If

				Return m_displayName
			End Get
		End Property

		''' <summary>
		''' Gets the text used in edit controls hosted in various dialog boxes.
		''' </summary>
		Public ReadOnly Property EditInvitation() As String
			Get
				If NativePropertyDescription IsNot Nothing AndAlso m_editInvitation Is Nothing Then
					' EditInvitation can be empty, so ignore the HR value, but don't throw an exception
					Dim ptr As IntPtr = IntPtr.Zero

					Dim hr As HResult = NativePropertyDescription.GetEditInvitation(ptr)

					If CoreErrorHelper.Succeeded(hr) AndAlso ptr <> IntPtr.Zero Then
						m_editInvitation = Marshal.PtrToStringUni(ptr)
						' Free the string
						Marshal.FreeCoTaskMem(ptr)
					End If
				End If

				Return m_editInvitation
			End Get
		End Property

		''' <summary>
		''' Gets the VarEnum OLE type for this property.
		''' </summary>
		Public ReadOnly Property VarEnumType() As VarEnum
			Get
				If NativePropertyDescription IsNot Nothing AndAlso m_varEnumType Is Nothing Then
					Dim tempType As VarEnum

					Dim hr As HResult = NativePropertyDescription.GetPropertyType(tempType)

					If CoreErrorHelper.Succeeded(hr) Then
						m_varEnumType = tempType
					End If
				End If

				Return If(m_varEnumType.HasValue, m_varEnumType.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the .NET system type for a value of this property, or
		''' null if the value is empty.
		''' </summary>
		Public ReadOnly Property ValueType() As Type
			Get
				If m_valueType Is Nothing Then
					m_valueType = ShellPropertyFactory.VarEnumToSystemType(VarEnumType)
				End If

				Return m_valueType
			End Get
		End Property

		''' <summary>
		''' Gets the current data type used to display the property.
		''' </summary>
		Public ReadOnly Property DisplayType() As PropertyDisplayType
			Get
				If NativePropertyDescription IsNot Nothing AndAlso m_displayType Is Nothing Then
					Dim tempDisplayType As PropertyDisplayType

					Dim hr As HResult = NativePropertyDescription.GetDisplayType(tempDisplayType)

					If CoreErrorHelper.Succeeded(hr) Then
						m_displayType = tempDisplayType
					End If
				End If

				Return If(m_displayType.HasValue, m_displayType.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the default user interface (UI) column width for this property.
		''' </summary>
		Public ReadOnly Property DefaultColumWidth() As UInteger
			Get
				If NativePropertyDescription IsNot Nothing AndAlso Not m_defaultColumWidth.HasValue Then
					Dim tempDefaultColumWidth As UInteger

					Dim hr As HResult = NativePropertyDescription.GetDefaultColumnWidth(tempDefaultColumWidth)

					If CoreErrorHelper.Succeeded(hr) Then
						m_defaultColumWidth = tempDefaultColumWidth
					End If
				End If

                Return If(m_defaultColumWidth.HasValue, m_defaultColumWidth.Value, 0UI)
            End Get
		End Property

		''' <summary>
		''' Gets a value that describes how the property values are displayed when 
		''' multiple items are selected in the user interface (UI).
		''' </summary>
		Public ReadOnly Property AggregationTypes() As PropertyAggregationType
			Get
				If NativePropertyDescription IsNot Nothing AndAlso m_aggregationTypes Is Nothing Then
					Dim tempAggregationTypes As PropertyAggregationType

					Dim hr As HResult = NativePropertyDescription.GetAggregationType(tempAggregationTypes)

					If CoreErrorHelper.Succeeded(hr) Then
						m_aggregationTypes = tempAggregationTypes
					End If
				End If

				Return If(m_aggregationTypes.HasValue, m_aggregationTypes.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets a list of the possible values for this property.
		''' </summary>
		Public ReadOnly Property PropertyEnumTypes() As ReadOnlyCollection(Of ShellPropertyEnumType)
			Get
				If NativePropertyDescription IsNot Nothing AndAlso m_propertyEnumTypes Is Nothing Then
					Dim propEnumTypeList As New List(Of ShellPropertyEnumType)()

					Dim guid As New Guid(ShellIIDGuid.IPropertyEnumTypeList)
					Dim nativeList As IPropertyEnumTypeList
					Dim hr As HResult = NativePropertyDescription.GetEnumTypeList(guid, nativeList)

					If nativeList IsNot Nothing AndAlso CoreErrorHelper.Succeeded(hr) Then

						Dim count As UInteger
						nativeList.GetCount(count)
						guid = New Guid(ShellIIDGuid.IPropertyEnumType)

                        For i As UInteger = 0 To count - 1UI
                            Dim nativeEnumType As IPropertyEnumType
                            nativeList.GetAt(i, guid, nativeEnumType)
                            propEnumTypeList.Add(New ShellPropertyEnumType(nativeEnumType))
                        Next
                    End If

					m_propertyEnumTypes = New ReadOnlyCollection(Of ShellPropertyEnumType)(propEnumTypeList)
				End If


				Return m_propertyEnumTypes
			End Get
		End Property

		''' <summary>
		''' Gets the column state flag, which describes how the property 
		''' should be treated by interfaces or APIs that use this flag.
		''' </summary>
		Public ReadOnly Property ColumnState() As PropertyColumnStateOptions
			Get
				' If default/first value, try to get it again, otherwise used the cached one.
				If NativePropertyDescription IsNot Nothing AndAlso m_columnState Is Nothing Then
					Dim state As PropertyColumnStateOptions

					Dim hr As HResult = NativePropertyDescription.GetColumnState(state)

					If CoreErrorHelper.Succeeded(hr) Then
						m_columnState = state
					End If
				End If

				Return If(m_columnState.HasValue, m_columnState.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the condition type to use when displaying the property in 
		''' the query builder user interface (UI). This influences the list 
		''' of predicate conditions (for example, equals, less than, and 
		''' contains) that are shown for this property.
		''' </summary>
		''' <remarks>For more information, see the <c>conditionType</c> attribute 
		''' of the <c>typeInfo</c> element in the property's .propdesc file.</remarks>
		Public ReadOnly Property ConditionType() As PropertyConditionType
			Get
				' If default/first value, try to get it again, otherwise used the cached one.
				If NativePropertyDescription IsNot Nothing AndAlso m_conditionType Is Nothing Then
					Dim tempConditionType As PropertyConditionType
					Dim tempConditionOperation As PropertyConditionOperation

					Dim hr As HResult = NativePropertyDescription.GetConditionType(tempConditionType, tempConditionOperation)

					If CoreErrorHelper.Succeeded(hr) Then
						m_conditionOperation = tempConditionOperation
						m_conditionType = tempConditionType
					End If
				End If

				Return If(m_conditionType.HasValue, m_conditionType.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the default condition operation to use 
		''' when displaying the property in the query builder user 
		''' interface (UI). This influences the list of predicate conditions 
		''' (for example, equals, less than, and contains) that are shown 
		''' for this property.
		''' </summary>
		''' <remarks>For more information, see the <c>conditionType</c> attribute of the 
		''' <c>typeInfo</c> element in the property's .propdesc file.</remarks>
		Public ReadOnly Property ConditionOperation() As PropertyConditionOperation
			Get
				' If default/first value, try to get it again, otherwise used the cached one.
				If NativePropertyDescription IsNot Nothing AndAlso m_conditionOperation Is Nothing Then
					Dim tempConditionType As PropertyConditionType
					Dim tempConditionOperation As PropertyConditionOperation

					Dim hr As HResult = NativePropertyDescription.GetConditionType(tempConditionType, tempConditionOperation)

					If CoreErrorHelper.Succeeded(hr) Then
						m_conditionOperation = tempConditionOperation
						m_conditionType = tempConditionType
					End If
				End If

				Return If(m_conditionOperation.HasValue, m_conditionOperation.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the method used when a view is grouped by this property.
		''' </summary>
		''' <remarks>The information retrieved by this method comes from 
		''' the <c>groupingRange</c> attribute of the <c>typeInfo</c> element in the 
		''' property's .propdesc file.</remarks>
		Public ReadOnly Property GroupingRange() As PropertyGroupingRange
			Get
				' If default/first value, try to get it again, otherwise used the cached one.
				If NativePropertyDescription IsNot Nothing AndAlso m_groupingRange Is Nothing Then
					Dim tempGroupingRange As PropertyGroupingRange

					Dim hr As HResult = NativePropertyDescription.GetGroupingRange(tempGroupingRange)

					If CoreErrorHelper.Succeeded(hr) Then
						m_groupingRange = tempGroupingRange
					End If
				End If

				Return If(m_groupingRange.HasValue, m_groupingRange.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the current sort description flags for the property, 
		''' which indicate the particular wordings of sort offerings.
		''' </summary>
		''' <remarks>The settings retrieved by this method are set 
		''' through the <c>sortDescription</c> attribute of the <c>labelInfo</c> 
		''' element in the property's .propdesc file.</remarks>
		Public ReadOnly Property SortDescription() As PropertySortDescription
			Get
				' If default/first value, try to get it again, otherwise used the cached one.
				If NativePropertyDescription IsNot Nothing AndAlso m_sortDescription Is Nothing Then
					Dim tempSortDescription As PropertySortDescription

					Dim hr As HResult = NativePropertyDescription.GetSortDescription(tempSortDescription)

					If CoreErrorHelper.Succeeded(hr) Then
						m_sortDescription = tempSortDescription
					End If
				End If

				Return If(m_sortDescription.HasValue, m_sortDescription.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the localized display string that describes the current sort order.
		''' </summary>
		''' <param name="descending">Indicates the sort order should 
		''' reference the string "Z on top"; otherwise, the sort order should reference the string "A on top".</param>
		''' <returns>The sort description for this property.</returns>
		''' <remarks>The string retrieved by this method is determined by flags set in the 
		''' <c>sortDescription</c> attribute of the <c>labelInfo</c> element in the property's .propdesc file.</remarks>
		Public Function GetSortDescriptionLabel(descending As Boolean) As String
			Dim ptr As IntPtr = IntPtr.Zero
			Dim label As String = String.Empty

			If NativePropertyDescription IsNot Nothing Then
				Dim hr As HResult = NativePropertyDescription.GetSortDescriptionLabel(descending, ptr)

				If CoreErrorHelper.Succeeded(hr) AndAlso ptr <> IntPtr.Zero Then
					label = Marshal.PtrToStringUni(ptr)
					' Free the string
					Marshal.FreeCoTaskMem(ptr)
				End If
			End If

			Return label
		End Function

		''' <summary>
		''' Gets a set of flags that describe the uses and capabilities of the property.
		''' </summary>
		Public ReadOnly Property TypeFlags() As PropertyTypeOptions
			Get
				If NativePropertyDescription IsNot Nothing AndAlso propertyTypeFlags Is Nothing Then
					Dim tempFlags As PropertyTypeOptions

					Dim hr As HResult = NativePropertyDescription.GetTypeFlags(PropertyTypeOptions.MaskAll, tempFlags)

					propertyTypeFlags = If(CoreErrorHelper.Succeeded(hr), tempFlags, Nothing)
				End If

				Return If(propertyTypeFlags.HasValue, propertyTypeFlags.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets the current set of flags governing the property's view.
		''' </summary>
		Public ReadOnly Property ViewFlags() As PropertyViewOptions
			Get
				If NativePropertyDescription IsNot Nothing AndAlso propertyViewFlags Is Nothing Then
					Dim tempFlags As PropertyViewOptions
					Dim hr As HResult = NativePropertyDescription.GetViewFlags(tempFlags)

					propertyViewFlags = If(CoreErrorHelper.Succeeded(hr), tempFlags, Nothing)
				End If

				Return If(propertyViewFlags.HasValue, propertyViewFlags.Value, Nothing)
			End Get
		End Property

		''' <summary>
		''' Gets a value that determines if the native property description is present on the system.
		''' </summary>
		Public ReadOnly Property HasSystemDescription() As Boolean
			Get
				Return NativePropertyDescription IsNot Nothing
			End Get
		End Property

		#End Region

		#Region "Internal Constructor"

		Friend Sub New(key As PropertyKey)
			Me.m_propertyKey = key
		End Sub

		#End Region

		#Region "Internal Methods"

		''' <summary>
		''' Get the native property description COM interface
		''' </summary>
		Friend ReadOnly Property NativePropertyDescription() As IPropertyDescription
			Get
				If m_nativePropertyDescription Is Nothing Then
					Dim guid As New Guid(ShellIIDGuid.IPropertyDescription)
					PropertySystemNativeMethods.PSGetPropertyDescription(m_propertyKey, guid, m_nativePropertyDescription)
				End If

				Return m_nativePropertyDescription
			End Get
		End Property

		#End Region

		#Region "IDisposable Members"

		''' <summary>
		''' Release the native objects
		''' </summary>
		''' <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
		Protected Overridable Sub Dispose(disposing As Boolean)
			If m_nativePropertyDescription IsNot Nothing Then
				Marshal.ReleaseComObject(m_nativePropertyDescription)
				m_nativePropertyDescription = Nothing
			End If

			If disposing Then
				' and the managed ones
				m_canonicalName = Nothing
				m_displayName = Nothing
				m_editInvitation = Nothing
				m_defaultColumWidth = Nothing
				m_valueType = Nothing
				m_propertyEnumTypes = Nothing
			End If
		End Sub

		''' <summary>
		''' Release the native objects
		''' </summary>
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		''' <summary>
		''' Release the native objects
		''' </summary>
		Protected Overrides Sub Finalize()
			Try
				Dispose(False)
			Finally
				MyBase.Finalize()
			End Try
		End Sub

		#End Region
	End Class
End Namespace
