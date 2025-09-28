' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem

Namespace Sensors
    ''' <summary>
    ''' Holds a collection of PROPERTYKEY values. This interface can be retrieved from a method 
    ''' or, if a new object is required, call CoCreate with CLSID_PortableDeviceKeyCollection.
    ''' </summary>
    <ComImport, Guid("DADA2357-E0AD-492E-98DB-DD61C53BA353"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IPortableDeviceKeyCollection
        Sub GetCount(ByRef pcElems As UInt32)
        <PreserveSig>
        Function GetAt(<[In]> dwIndex As UInt32, ByRef pKey As PropertyKey) As HResult
        Sub Add(<[In]> ByRef Key As PropertyKey)
        Sub Clear()
        Sub RemoveAt(<[In]> dwIndex As UInt32)
    End Interface

    ''' <summary>
    ''' The nativeIPortableDeviceValues interface holds a collection of PROPERTYKEY/PropVariant pairs. 
    ''' Values in the collection do not need to be all the same VARTYPE. Values are stored as key-value 
    ''' pairs; each key must be unique in the collection. Clients can search for items by PROPERTYKEY 
    ''' or zero-based index. Data values are stored as PropVariant structures. You can add or retrieve 
    ''' values of any type by using the generic methods SetValue and GetValue, or you add items by using 
    ''' the method specific to the type of data added. 
    ''' </summary>
    <ComImport, Guid("6848F6F2-3155-4F86-B6F5-263EEEAB3143"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IPortableDeviceValues
        Sub GetCount(<[In]> ByRef pcelt As UInteger)
        Sub GetAt(<[In]> index As UInteger, <[In], Out> ByRef pKey As PropertyKey, <[In], Out> pValue As PropVariant)
        Sub SetValue(<[In]> ByRef key As PropertyKey, <[In]> pValue As PropVariant)
        Sub GetValue(<[In]> ByRef key As PropertyKey, <Out> pValue As PropVariant)
        Sub SetStringValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.LPWStr)> Value As String)
        Sub GetStringValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.LPWStr)> ByRef pValue As String)
        Sub SetUnsignedIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As UInteger)
        Sub GetUnsignedIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As UInteger)
        Sub SetSignedIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Integer)
        Sub GetSignedIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Integer)
        Sub SetUnsignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As ULong)
        Sub GetUnsignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As ULong)
        Sub SetSignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Long)
        Sub GetSignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Long)
        Sub SetFloatValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Single)
        Sub GetFloatValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Single)
        Sub SetErrorValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Error])> Value As Integer)
        Sub GetErrorValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Error])> ByRef pValue As Integer)
        Sub SetKeyValue(<[In]> ByRef key As PropertyKey, <[In]> ByRef Value As PropertyKey)
        Sub GetKeyValue(<[In]> ByRef key As PropertyKey, ByRef pValue As PropertyKey)
        Sub SetBoolValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Integer)
        Sub GetBoolValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Integer)
        Sub SetIUnknownValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.IUnknown)> pValue As Object)
        Sub GetIUnknownValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppValue As Object)
        Sub SetGuidValue(<[In]> ByRef key As PropertyKey, <[In]> ByRef Value As Guid)
        Sub GetGuidValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Guid)
        Sub SetBufferValue(<[In]> ByRef key As PropertyKey, <[In]> pValue As Byte(), <[In]> cbValue As UInteger)
        Sub GetBufferValue(<[In]> ByRef key As PropertyKey, <Out> ppValue As IntPtr, ByRef pcbValue As UInteger)
        Sub SetnativeIPortableDeviceValuesValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDeviceValues)
        Sub GetnativeIPortableDeviceValuesValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDeviceValues)
        Sub SetIPortableDevicePropVariantCollectionValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDevicePropVariantCollection)
        Sub GetIPortableDevicePropVariantCollectionValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDevicePropVariantCollection)
        Sub SetIPortableDeviceKeyCollectionValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDeviceKeyCollection)
        Sub GetIPortableDeviceKeyCollectionValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDeviceKeyCollection)
        Sub SetnativeIPortableDeviceValuesCollectionValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDeviceValuesCollection)
        Sub GetnativeIPortableDeviceValuesCollectionValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDeviceValuesCollection)
        Sub RemoveValue(<[In]> ByRef key As PropertyKey)
        Sub CopyValuesFromPropertyStore(<[In], MarshalAs(UnmanagedType.[Interface])> pStore As IPropertyStore)
        Sub CopyValuesToPropertyStore(<[In], MarshalAs(UnmanagedType.[Interface])> pStore As IPropertyStore)
        Sub Clear()
    End Interface

    ''' <summary>
    ''' Holds a collection of indexed nativeIPortableDeviceValues interfaces. This interface can be 
    ''' retrieved from a method, or if a new object is required, call CoCreate with 
    ''' CLSID_PortableDeviceValuesCollection.
    ''' </summary>
    <ComImport, Guid("6E3F2D79-4E07-48C4-8208-D8C2E5AF4A99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IPortableDeviceValuesCollection
        ''' <summary>
        ''' Retrieves the number of items in the collection.
        ''' </summary>
        ''' <param name="pcElems">Pointer to a DWORD that contains the number of nativeIPortableDeviceValues interfaces in the collection.</param>
        Sub GetCount(<[In]> ByRef pcElems As UInteger)

        ''' <summary>
        ''' Retrieves an item from the collection by a zero-based index.
        ''' </summary>
        ''' <param name="dwIndex">DWORD that specifies a zero-based index in the collection.</param>
        ''' <param name="ppValues">Address of a variable that receives a pointer to an nativeIPortableDeviceValues interface from the collection. The caller is responsible for calling Release on this interface when done with it</param>
        Sub GetAt(<[In]> dwIndex As UInteger, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValues As IPortableDeviceValues)

        ''' <summary>
        ''' Adds an item to the collection.
        ''' </summary>
        ''' <param name="pValues">Pointer to an nativeIPortableDeviceValues interface to add to the collection. The interface is not actually copied, but AddRef is called on it</param>
        Sub Add(<[In], MarshalAs(UnmanagedType.[Interface])> pValues As IPortableDeviceValues)

        ''' <summary>
        ''' Releases all items from the collection.
        ''' </summary>
        Sub Clear()

        ''' <summary>
        ''' Removes an item from the collection by a zero-based index.
        ''' </summary>
        ''' <param name="dwIndex">DWORD that specifies a zero-based index in the collection.</param>
        Sub RemoveAt(<[In]> dwIndex As UInteger)
    End Interface

    ''' <summary>
    ''' Holds a collection of PropVariant values of the same VARTYPE. The VARTYPE of the first item 
    ''' that is added to the collection determines the VARTYPE of the collection. An attempt to add 
    ''' an item of a different VARTYPE may fail if the PropVariant value cannot be changed to the 
    ''' collection?s current VARTYPE. To change the VARTYPE of the collection manually, call ChangeType
    ''' </summary>
    <ComImport, Guid("89B2E422-4F1B-4316-BCEF-A44AFEA83EB3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IPortableDevicePropVariantCollection
        Sub GetCount(<[In]> ByRef pcElems As UInteger)
        Sub GetAt(<[In]> dwIndex As UInteger, <Out> pValue As PropVariant)
        Sub Add(<[In]> pValue As PropVariant)
        Sub [GetType](ByRef pvt As UShort)
        Sub ChangeType(<[In]> vt As UShort)
        Sub Clear()
        Sub RemoveAt(<[In]> dwIndex As UInteger)
    End Interface

    ''' <summary>
    ''' Exposes methods for enumerating, getting, and setting property values.
    ''' </summary>
    <ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IPropertyStore
        Sub GetCount(ByRef cProps As UInteger)
        Sub GetAt(<[In]> iProp As UInteger, ByRef pKey As PropertyKey)
        Sub GetValue(<[In]> ByRef key As PropertyKey, <Out> pv As PropVariant)
        Sub SetValue(<[In]> ByRef key As PropertyKey, <[In]> propvar As PropVariant)
        Sub Commit()
    End Interface

    <ComImport, Guid("DE2D022D-2480-43BE-97F0-D1FA2CF98F4F"), ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate)>
    Friend Class PortableDeviceKeyCollection
        Implements IPortableDeviceKeyCollection
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetCount(ByRef pcElems As UInt32) Implements IPortableDeviceKeyCollection.GetCount
        End Sub

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Function GetAt(<[In]> dwIndex As UInt32, ByRef pKey As PropertyKey) As HResult Implements IPortableDeviceKeyCollection.GetAt
        End Function

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Add(<[In]> ByRef Key As PropertyKey) Implements IPortableDeviceKeyCollection.Add
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Clear() Implements IPortableDeviceKeyCollection.Clear
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub RemoveAt(<[In]> dwIndex As UInt32) Implements IPortableDeviceKeyCollection.RemoveAt
        End Sub
    End Class

    <ComImport, Guid("0C15D503-D017-47CE-9016-7B3F978721CC"), ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate)>
    Friend Class PortableDeviceValues
        Implements IPortableDeviceValues
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetCount(<[In]> ByRef pcelt As UInteger) Implements IPortableDeviceValues.GetCount
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetAt(<[In]> index As UInteger, <[In], Out> ByRef pKey As PropertyKey, <[In], Out> pValue As PropVariant) Implements IPortableDeviceValues.GetAt
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetValue(<[In]> ByRef key As PropertyKey, <[In]> pValue As PropVariant) Implements IPortableDeviceValues.SetValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetValue(<[In]> ByRef key As PropertyKey, <Out> pValue As PropVariant) Implements IPortableDeviceValues.GetValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetStringValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.LPWStr)> Value As String) Implements IPortableDeviceValues.SetStringValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetStringValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.LPWStr)> ByRef pValue As String) Implements IPortableDeviceValues.GetStringValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetUnsignedIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As UInteger) Implements IPortableDeviceValues.SetUnsignedIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetUnsignedIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As UInteger) Implements IPortableDeviceValues.GetUnsignedIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetSignedIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Integer) Implements IPortableDeviceValues.SetSignedIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetSignedIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Integer) Implements IPortableDeviceValues.GetSignedIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetUnsignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As ULong) Implements IPortableDeviceValues.SetUnsignedLargeIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetUnsignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As ULong) Implements IPortableDeviceValues.GetUnsignedLargeIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetSignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Long) Implements IPortableDeviceValues.SetSignedLargeIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetSignedLargeIntegerValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Long) Implements IPortableDeviceValues.GetSignedLargeIntegerValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetFloatValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Single) Implements IPortableDeviceValues.SetFloatValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetFloatValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Single) Implements IPortableDeviceValues.GetFloatValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetErrorValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Error])> Value As Integer) Implements IPortableDeviceValues.SetErrorValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetErrorValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Error])> ByRef pValue As Integer) Implements IPortableDeviceValues.GetErrorValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetKeyValue(<[In]> ByRef key As PropertyKey, <[In]> ByRef Value As PropertyKey) Implements IPortableDeviceValues.SetKeyValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetKeyValue(<[In]> ByRef key As PropertyKey, ByRef pValue As PropertyKey) Implements IPortableDeviceValues.GetKeyValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetBoolValue(<[In]> ByRef key As PropertyKey, <[In]> Value As Integer) Implements IPortableDeviceValues.SetBoolValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetBoolValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Integer) Implements IPortableDeviceValues.GetBoolValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetIUnknownValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.IUnknown)> pValue As Object) Implements IPortableDeviceValues.SetIUnknownValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetIUnknownValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppValue As Object) Implements IPortableDeviceValues.GetIUnknownValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetGuidValue(<[In]> ByRef key As PropertyKey, <[In]> ByRef Value As Guid) Implements IPortableDeviceValues.SetGuidValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetGuidValue(<[In]> ByRef key As PropertyKey, ByRef pValue As Guid) Implements IPortableDeviceValues.GetGuidValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetBufferValue(<[In]> ByRef key As PropertyKey, <[In]> pValue As Byte(), <[In]> cbValue As UInteger) Implements IPortableDeviceValues.SetBufferValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetBufferValue(<[In]> ByRef key As PropertyKey, <Out> ppValue As IntPtr, ByRef pcbValue As UInteger) Implements IPortableDeviceValues.GetBufferValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetnativeIPortableDeviceValuesValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDeviceValues) Implements IPortableDeviceValues.SetnativeIPortableDeviceValuesValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetnativeIPortableDeviceValuesValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDeviceValues) Implements IPortableDeviceValues.GetnativeIPortableDeviceValuesValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetIPortableDevicePropVariantCollectionValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDevicePropVariantCollection) Implements IPortableDeviceValues.SetIPortableDevicePropVariantCollectionValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetIPortableDevicePropVariantCollectionValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDevicePropVariantCollection) Implements IPortableDeviceValues.GetIPortableDevicePropVariantCollectionValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetIPortableDeviceKeyCollectionValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDeviceKeyCollection) Implements IPortableDeviceValues.SetIPortableDeviceKeyCollectionValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetIPortableDeviceKeyCollectionValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDeviceKeyCollection) Implements IPortableDeviceValues.GetIPortableDeviceKeyCollectionValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub SetnativeIPortableDeviceValuesCollectionValue(<[In]> ByRef key As PropertyKey, <[In], MarshalAs(UnmanagedType.[Interface])> pValue As IPortableDeviceValuesCollection) Implements IPortableDeviceValues.SetnativeIPortableDeviceValuesCollectionValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub GetnativeIPortableDeviceValuesCollectionValue(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.[Interface])> ByRef ppValue As IPortableDeviceValuesCollection) Implements IPortableDeviceValues.GetnativeIPortableDeviceValuesCollectionValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub RemoveValue(<[In]> ByRef key As PropertyKey) Implements IPortableDeviceValues.RemoveValue
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub CopyValuesFromPropertyStore(<[In], MarshalAs(UnmanagedType.[Interface])> pStore As IPropertyStore) Implements IPortableDeviceValues.CopyValuesFromPropertyStore
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub CopyValuesToPropertyStore(<[In], MarshalAs(UnmanagedType.[Interface])> pStore As IPropertyStore) Implements IPortableDeviceValues.CopyValuesToPropertyStore
        End Sub

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Public Overridable Sub Clear() Implements IPortableDeviceValues.Clear
        End Sub
    End Class
End Namespace
