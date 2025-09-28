'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
    ' Disable warning if a method declaration hides another inherited from a parent COM interface
    ' To successfully import a COM interface, all inherited methods need to be declared again with 
    ' the exception of those already declared in "IUnknown"

    '#Pragma warning disable 108

#Region "Property System COM Interfaces"

    <ComImport> _
	<Guid(ShellIIDGuid.IPropertyStoreCapabilities)> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Interface IPropertyStoreCapabilities
		Function IsPropertyWritable(<[In]> ByRef propertyKey As PropertyKey) As HResult
	End Interface

	''' <summary>
	''' An in-memory property store cache
	''' </summary>
	<ComImport> _
	<Guid(ShellIIDGuid.IPropertyStoreCache)> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Interface IPropertyStoreCache
		''' <summary>
		''' Gets the state of a property stored in the cache
		''' </summary>
		''' <param name="key"></param>
		''' <param name="state"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetState(ByRef key As PropertyKey, <Out> ByRef state As PropertyStoreCacheState) As HResult

		''' <summary>
		''' Gets the valeu and state of a property in the cache
		''' </summary>
		''' <param name="propKey"></param>
		''' <param name="pv"></param>
		''' <param name="state"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetValueAndState(ByRef propKey As PropertyKey, <Out> pv As PropVariant, <Out> ByRef state As PropertyStoreCacheState) As HResult

		''' <summary>
		''' Sets the state of a property in the cache.
		''' </summary>
		''' <param name="propKey"></param>
		''' <param name="state"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SetState(ByRef propKey As PropertyKey, state As PropertyStoreCacheState) As HResult

		''' <summary>
		''' Sets the value and state in the cache.
		''' </summary>
		''' <param name="propKey"></param>
		''' <param name="pv"></param>
		''' <param name="state"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function SetValueAndState(ByRef propKey As PropertyKey, <[In]> pv As PropVariant, state As PropertyStoreCacheState) As HResult
	End Interface

	''' <summary>
	''' A property store
	''' </summary>
	<ComImport> _
	<Guid(ShellIIDGuid.IPropertyStore)> _
	<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Interface IPropertyStore
		''' <summary>
		''' Gets the number of properties contained in the property store.
		''' </summary>
		''' <param name="propertyCount"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetCount(<Out> ByRef propertyCount As UInteger) As HResult

		''' <summary>
		''' Get a property key located at a specific index.
		''' </summary>
		''' <param name="propertyIndex"></param>
		''' <param name="key"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetAt(<[In]> propertyIndex As UInteger, ByRef key As PropertyKey) As HResult

		''' <summary>
		''' Gets the value of a property from the store
		''' </summary>
		''' <param name="key"></param>
		''' <param name="pv"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetValue(<[In]> ByRef key As PropertyKey, <Out> pv As PropVariant) As HResult

		''' <summary>
		''' Sets the value of a property in the store
		''' </summary>
		''' <param name="key"></param>
		''' <param name="pv"></param>
		''' <returns></returns>
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime), PreserveSig> _
		Function SetValue(<[In]> ByRef key As PropertyKey, <[In]> pv As PropVariant) As HResult

		''' <summary>
		''' Commits the changes.
		''' </summary>
		''' <returns></returns>
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function Commit() As HResult
	End Interface

	<ComImport, Guid(ShellIIDGuid.IPropertyDescriptionList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IPropertyDescriptionList
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCount(ByRef pcElem As UInteger)
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetAt(<[In]> iElem As UInteger, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyDescription)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IPropertyDescription), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IPropertyDescription
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetPropertyKey(ByRef pkey As PropertyKey)
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCanonicalName(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetPropertyType(ByRef pvartype As VarEnum) As HResult
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime), PreserveSig> _
		Function GetDisplayName(ByRef ppszName As IntPtr) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetEditInvitation(ByRef ppszInvite As IntPtr) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetTypeFlags(<[In]> mask As PropertyTypeOptions, ByRef ppdtFlags As PropertyTypeOptions) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetViewFlags(ByRef ppdvFlags As PropertyViewOptions) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetDefaultColumnWidth(ByRef pcxChars As UInteger) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetDisplayType(ByRef pdisplaytype As PropertyDisplayType) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetColumnState(ByRef pcsFlags As PropertyColumnStateOptions) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetGroupingRange(ByRef pgr As PropertyGroupingRange) As HResult
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetRelativeDescriptionType(ByRef prdt As PropertySystemNativeMethods.RelativeDescriptionType)
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetRelativeDescription(<[In]> propvar1 As PropVariant, <[In]> propvar2 As PropVariant, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDesc1 As String, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDesc2 As String)
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetSortDescription(ByRef psd As PropertySortDescription) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetSortDescriptionLabel(<[In]> fDescending As Boolean, ByRef ppszDescription As IntPtr) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetAggregationType(ByRef paggtype As PropertyAggregationType) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetConditionType(ByRef pcontype As PropertyConditionType, ByRef popDefault As PropertyConditionOperation) As HResult
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function GetEnumTypeList(<[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyEnumTypeList) As HResult
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub CoerceToCanonicalValue(<[In], Out> propvar As PropVariant)
		' Note: this method signature may be wrong, but it is not used.
		<PreserveSig> _
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function FormatForDisplay(<[In]> propvar As PropVariant, <[In]> ByRef pdfFlags As PropertyDescriptionFormatOptions, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDisplay As String) As HResult
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Function IsValueCanonical(<[In]> propvar As PropVariant) As HResult
	End Interface

	<ComImport, Guid(ShellIIDGuid.IPropertyDescription2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IPropertyDescription2
		Inherits IPropertyDescription
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetPropertyKey(ByRef pkey As PropertyKey)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetCanonicalName(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetPropertyType(ByRef pvartype As VarEnum)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetDisplayName(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetEditInvitation(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszInvite As String)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetTypeFlags(<[In]> mask As PropertyTypeOptions, ByRef ppdtFlags As PropertyTypeOptions)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetViewFlags(ByRef ppdvFlags As PropertyViewOptions)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetDefaultColumnWidth(ByRef pcxChars As UInteger)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetDisplayType(ByRef pdisplaytype As PropertyDisplayType)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetColumnState(ByRef pcsFlags As UInteger)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetGroupingRange(ByRef pgr As PropertyGroupingRange)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetRelativeDescriptionType(ByRef prdt As PropertySystemNativeMethods.RelativeDescriptionType)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetRelativeDescription(<[In]> propvar1 As PropVariant, <[In]> propvar2 As PropVariant, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDesc1 As String, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDesc2 As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetSortDescription(ByRef psd As PropertySortDescription)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetSortDescriptionLabel(<[In]> fDescending As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDescription As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetAggregationType(ByRef paggtype As PropertyAggregationType)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetConditionType(ByRef pcontype As PropertyConditionType, ByRef popDefault As PropertyConditionOperation)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetEnumTypeList(<[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub CoerceToCanonicalValue(<[In], Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub FormatForDisplay(<[In]> propvar As PropVariant, <[In]> ByRef pdfFlags As PropertyDescriptionFormatOptions, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDisplay As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function IsValueCanonical(<[In]> propvar As PropVariant) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetImageReferenceForValue(<[In]> propvar As PropVariant, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszImageRes As String)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IPropertyEnumType), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IPropertyEnumType
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetEnumType(<Out> ByRef penumtype As PropEnumType)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetValue(<Out> ppropvar As PropVariant)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetRangeMinValue(<Out> ppropvar As PropVariant)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetRangeSetValue(<Out> ppropvar As PropVariant)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetDisplayText(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDisplay As String)
	End Interface

	<ComImport, Guid(ShellIIDGuid.IPropertyEnumType2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IPropertyEnumType2
		Inherits IPropertyEnumType
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetEnumType(<Out> ByRef penumtype As PropEnumType)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetValue(<Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetRangeMinValue(<Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetRangeSetValue(<Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetDisplayText(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDisplay As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetImageReference(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszImageRes As String)
	End Interface


	<ComImport, Guid(ShellIIDGuid.IPropertyEnumTypeList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IPropertyEnumTypeList
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetCount(<Out> ByRef pctypes As UInteger)

		' riid may be IID_IPropertyEnumType
		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetAt(<[In]> itype As UInteger, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyEnumType)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub GetConditionAt(<[In]> index As UInteger, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

		<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType := MethodCodeType.Runtime)> _
		Sub FindMatchingIndex(<[In]> propvarCmp As PropVariant, <Out> ByRef pnIndex As UInteger)
	End Interface

#End Region

    '#Pragma warning restore 108

End Namespace
