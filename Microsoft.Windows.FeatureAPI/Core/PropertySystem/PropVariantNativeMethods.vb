'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem

    Public Module PropVariantNativeMethods

        ' returns hresult
        <DllImport("Ole32.dll", PreserveSig:=False)>
        Public Sub PropVariantClear(<[In], Out> pvar As PropVariant)
        End Sub

        ' psa is actually returned, not hresult
        <DllImport("OleAut32.dll", PreserveSig:=True)>
        Public Function SafeArrayCreateVector(vt As UShort, lowerBound As Integer, cElems As UInteger) As IntPtr
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Public Function SafeArrayAccessData(psa As IntPtr) As IntPtr
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Public Sub SafeArrayUnaccessData(psa As IntPtr)
        End Sub

        ' retuns uint32
        <DllImport("OleAut32.dll", PreserveSig:=True)>
        Public Function SafeArrayGetDim(psa As IntPtr) As UInteger
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Public Function SafeArrayGetLBound(psa As IntPtr, nDim As UInteger) As Integer
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Public Function SafeArrayGetUBound(psa As IntPtr, nDim As UInteger) As Integer
        End Function

        ' This decl for SafeArrayGetElement is only valid for cDims==1!
        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Public Function SafeArrayGetElement(psa As IntPtr, ByRef rgIndices As Integer) As <MarshalAs(UnmanagedType.IUnknown)> Object
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromPropVariantVectorElem(<[In]> propvarIn As PropVariant, iElem As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromFileTime(<[In]> ByRef pftIn As System.Runtime.InteropServices.ComTypes.FILETIME, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function PropVariantGetElementCount(<[In]> propVar As PropVariant) As <MarshalAs(UnmanagedType.I4)> Integer
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetBooleanElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out, MarshalAs(UnmanagedType.Bool)> ByRef pfVal As Boolean)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetInt16Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Short)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetUInt16Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UShort)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetInt32Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Integer)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetUInt32Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UInteger)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetInt64Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Int64)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetUInt64Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UInt64)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetDoubleElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Double)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetFileTimeElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out, MarshalAs(UnmanagedType.Struct)> ByRef pftVal As System.Runtime.InteropServices.ComTypes.FILETIME)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub PropVariantGetStringElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszVal As String)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromBooleanVector(<[In], MarshalAs(UnmanagedType.LPArray)> prgf As Boolean(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromInt16Vector(<[In], Out> prgn As Int16(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromUInt16Vector(<[In], Out> prgn As UInt16(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromInt32Vector(<[In], Out> prgn As Int32(), cElems As UInteger, <Out> propVar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromUInt32Vector(<[In], Out> prgn As UInt32(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromInt64Vector(<[In], Out> prgn As Int64(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromUInt64Vector(<[In], Out> prgn As UInt64(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromDoubleVector(<[In], Out> prgn As Double(), cElems As UInteger, <Out> propvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromFileTimeVector(<[In], Out> prgft As System.Runtime.InteropServices.ComTypes.FILETIME(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Public Sub InitPropVariantFromStringVector(<[In], Out> prgsz As String(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub
    End Module
End Namespace
