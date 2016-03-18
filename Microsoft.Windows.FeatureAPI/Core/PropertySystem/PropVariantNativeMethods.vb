'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Internal

Namespace Shell.PropertySystem
    Friend NotInheritable Class PropVariantNativeMethods
        Private Sub New()
        End Sub
        ' returns hresult
        <DllImport("Ole32.dll", PreserveSig:=False)>
        Friend Shared Sub PropVariantClear(<[In], Out> pvar As PropVariant)
        End Sub

        ' psa is actually returned, not hresult
        <DllImport("OleAut32.dll", PreserveSig:=True)>
        Friend Shared Function SafeArrayCreateVector(vt As UShort, lowerBound As Integer, cElems As UInteger) As IntPtr
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayAccessData(psa As IntPtr) As IntPtr
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Sub SafeArrayUnaccessData(psa As IntPtr)
        End Sub

        ' retuns uint32
        <DllImport("OleAut32.dll", PreserveSig:=True)>
        Friend Shared Function SafeArrayGetDim(psa As IntPtr) As UInteger
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayGetLBound(psa As IntPtr, nDim As UInteger) As Integer
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayGetUBound(psa As IntPtr, nDim As UInteger) As Integer
        End Function

        ' This decl for SafeArrayGetElement is only valid for cDims==1!
        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayGetElement(psa As IntPtr, ByRef rgIndices As Integer) As <MarshalAs(UnmanagedType.IUnknown)> Object
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromPropVariantVectorElem(<[In]> propvarIn As PropVariant, iElem As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromFileTime(<[In]> ByRef pftIn As System.Runtime.InteropServices.ComTypes.FILETIME, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Shared Function PropVariantGetElementCount(<[In]> propVar As PropVariant) As <MarshalAs(UnmanagedType.I4)> Integer
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetBooleanElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out, MarshalAs(UnmanagedType.Bool)> ByRef pfVal As Boolean)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetInt16Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Short)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetUInt16Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UShort)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetInt32Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Integer)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetUInt32Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UInteger)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetInt64Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Int64)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetUInt64Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UInt64)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetDoubleElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Double)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetFileTimeElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out, MarshalAs(UnmanagedType.Struct)> ByRef pftVal As System.Runtime.InteropServices.ComTypes.FILETIME)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetStringElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszVal As String)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromBooleanVector(<[In], MarshalAs(UnmanagedType.LPArray)> prgf As Boolean(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromInt16Vector(<[In], Out> prgn As Int16(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromUInt16Vector(<[In], Out> prgn As UInt16(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromInt32Vector(<[In], Out> prgn As Int32(), cElems As UInteger, <Out> propVar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromUInt32Vector(<[In], Out> prgn As UInt32(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromInt64Vector(<[In], Out> prgn As Int64(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromUInt64Vector(<[In], Out> prgn As UInt64(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromDoubleVector(<[In], Out> prgn As Double(), cElems As UInteger, <Out> propvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromFileTimeVector(<[In], Out> prgft As System.Runtime.InteropServices.ComTypes.FILETIME(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromStringVector(<[In], Out> prgsz As String(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub
    End Class
End Namespace
