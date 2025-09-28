'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Shell.PropertySystem
Imports Microsoft.Windows.Taskbar

Namespace Shell
    Friend Enum SICHINTF
        SICHINT_DISPLAY = &H0
        SICHINT_CANONICAL = &H10000000
        SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = &H20000000
        SICHINT_ALLFIELDS = &H80000000
    End Enum

    ' Disable warning if a method declaration hides another inherited from a parent COM interface
    ' To successfully import a COM interface, all inherited methods need to be declared again with 
    ' the exception of those already declared in "IUnknown"
    '#Pragma warning disable 108

#Region "COM Interfaces"

    <ComImport, Guid(ShellIIDGuid.IModalWindow), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IModalWindow
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Function Show(<[In]> parent As IntPtr) As Integer
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellItem), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellItem
        ' Not supported: IBindCtx.
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function BindToHandler(<[In]> pbc As IntPtr, <[In]> ByRef bhid As Guid, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetParent(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDisplayName(<[In]> sigdnName As ShellNativeMethods.ShellItemDesignNameOptions, ByRef ppszName As IntPtr) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetAttributes(<[In]> sfgaoMask As ShellNativeMethods.ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellNativeMethods.ShellFileGetAttributesOptions)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Compare(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In]> hint As SICHINTF, ByRef piOrder As Integer) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellItem2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellItem2
        Inherits IShellItem
        ' Not supported: IBindCtx.
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function BindToHandler(<[In]> pbc As IntPtr, <[In]> ByRef bhid As Guid, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function GetParent(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Function GetDisplayName(<[In]> sigdnName As ShellNativeMethods.ShellItemDesignNameOptions, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetAttributes(<[In]> sfgaoMask As ShellNativeMethods.ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellNativeMethods.ShellFileGetAttributesOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub Compare(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In]> hint As UInteger, ByRef piOrder As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Function GetPropertyStore(<[In]> Flags As ShellNativeMethods.GetPropertyStoreOptions, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyStore) As Integer

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyStoreWithCreateObject(<[In]> Flags As ShellNativeMethods.GetPropertyStoreOptions, <[In], MarshalAs(UnmanagedType.IUnknown)> punkCreateObject As Object, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyStoreForKeys(<[In]> ByRef rgKeys As PropertyKey, <[In]> cKeys As UInteger, <[In]> Flags As ShellNativeMethods.GetPropertyStoreOptions, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As IPropertyStore)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyDescriptionList(<[In]> ByRef keyType As PropertyKey, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Update(<[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetProperty(<[In]> ByRef key As PropertyKey, <Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetCLSID(<[In]> ByRef key As PropertyKey, ByRef pclsid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetFileTime(<[In]> ByRef key As PropertyKey, ByRef pft As System.Runtime.InteropServices.ComTypes.FILETIME)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetInt32(<[In]> ByRef key As PropertyKey, ByRef pi As Integer)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetString(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppsz As String) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetUInt32(<[In]> ByRef key As PropertyKey, ByRef pui As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetUInt64(<[In]> ByRef key As PropertyKey, ByRef pull As ULong)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetBool(<[In]> ByRef key As PropertyKey, ByRef pf As Integer)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellItemArray), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellItemArray
        ' Not supported: IBindCtx.
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function BindToHandler(<[In], MarshalAs(UnmanagedType.[Interface])> pbc As IntPtr, <[In]> ByRef rbhid As Guid, <[In]> ByRef riid As Guid, ByRef ppvOut As IntPtr) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetPropertyStore(<[In]> Flags As Integer, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetPropertyDescriptionList(<[In]> ByRef keyType As PropertyKey, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetAttributes(<[In]> dwAttribFlags As ShellNativeMethods.ShellItemAttributeOptions, <[In]> sfgaoMask As ShellNativeMethods.ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellNativeMethods.ShellFileGetAttributesOptions) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetCount(ByRef pdwNumItems As UInteger) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetItemAt(<[In]> dwIndex As UInteger, <MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As HResult

        ' Not supported: IEnumShellItems (will use GetCount and GetItemAt instead).
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function EnumItems(<MarshalAs(UnmanagedType.[Interface])> ByRef ppenumShellItems As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellLibrary), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellLibrary
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function LoadLibraryFromItem(<[In], MarshalAs(UnmanagedType.[Interface])> library As IShellItem, <[In]> grfMode As AccessModes) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub LoadLibraryFromKnownFolder(<[In]> ByRef knownfidLibrary As Guid, <[In]> grfMode As AccessModes)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub AddFolder(<[In], MarshalAs(UnmanagedType.[Interface])> location As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub RemoveFolder(<[In], MarshalAs(UnmanagedType.[Interface])> location As IShellItem)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetFolders(<[In]> lff As ShellNativeMethods.LibraryFolderFilter, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItemArray) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub ResolveFolder(<[In], MarshalAs(UnmanagedType.[Interface])> folderToResolve As IShellItem, <[In]> timeout As UInteger, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultSaveFolder(<[In]> dsft As ShellNativeMethods.DefaultSaveFolderType, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetDefaultSaveFolder(<[In]> dsft As ShellNativeMethods.DefaultSaveFolderType, <[In], MarshalAs(UnmanagedType.[Interface])> si As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetOptions(ByRef lofOptions As ShellNativeMethods.LibraryOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetOptions(<[In]> lofMask As ShellNativeMethods.LibraryOptions, <[In]> lofOptions As ShellNativeMethods.LibraryOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetFolderType(ByRef ftid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetFolderType(<[In]> ByRef ftid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetIcon(<MarshalAs(UnmanagedType.LPWStr)> ByRef icon As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetIcon(<[In], MarshalAs(UnmanagedType.LPWStr)> icon As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub Commit()

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub Save(<[In], MarshalAs(UnmanagedType.[Interface])> folderToSaveIn As IShellItem, <[In], MarshalAs(UnmanagedType.LPWStr)> libraryName As String, <[In]> lsf As ShellNativeMethods.LibrarySaveOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef savedTo As IShellItem2)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SaveInKnownFolder(<[In]> ByRef kfidToSaveIn As Guid, <[In], MarshalAs(UnmanagedType.LPWStr)> libraryName As String, <[In]> lsf As ShellNativeMethods.LibrarySaveOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef savedTo As IShellItem2)
    End Interface

    <ComImportAttribute>
    <GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")>
    <InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IShellItemImageFactory
        <PreserveSig>
        Function GetImage(<[In], MarshalAs(UnmanagedType.Struct)> size As CoreNativeMethods.Size, <[In]> flags As ShellNativeMethods.SIIGBF, <Out> ByRef phbm As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IThumbnailCache), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IThumbnailCache
        Sub GetThumbnail(<[In]> pShellItem As IShellItem, <[In]> cxyRequestedThumbSize As UInteger, <[In]> flags As Shell.ShellNativeMethods.ThumbnailOptions, <Out> ByRef ppvThumb As ISharedBitmap, <Out> ByRef pOutFlags As Shell.ShellNativeMethods.ThumbnailCacheOptions, <Out> pThumbnailID As Shell.ShellNativeMethods.ThumbnailId)

        Sub GetThumbnailByID(<[In]> thumbnailID As Shell.ShellNativeMethods.ThumbnailId, <[In]> cxyRequestedThumbSize As UInteger, <Out> ByRef ppvThumb As ISharedBitmap, <Out> ByRef pOutFlags As Shell.ShellNativeMethods.ThumbnailCacheOptions)
    End Interface

    <ComImport, Guid(ShellIIDGuid.ISharedBitmap), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface ISharedBitmap
        Sub GetSharedBitmap(<Out> ByRef phbm As IntPtr)
        Sub GetSize(<Out> ByRef pSize As CoreNativeMethods.Size)
        Sub GetFormat(<Out> ByRef pat As ThumbnailAlphaType)
        Sub InitializeBitmap(<[In]> hbm As IntPtr, <[In]> wtsAT As ThumbnailAlphaType)
        Sub Detach(<Out> ByRef phbm As IntPtr)
    End Interface
    <ComImport, Guid(ShellIIDGuid.IShellFolder), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss>
    Friend Interface IShellFolder
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub ParseDisplayName(hwnd As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String, <[In], Out> ByRef pchEaten As UInteger, <Out> ppidl As IntPtr, <[In], Out> ByRef pdwAttributes As UInteger)
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function EnumObjects(<[In]> hwnd As IntPtr, <[In]> grfFlags As ShellNativeMethods.ShellFolderEnumerationOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef ppenumIDList As IEnumIDList) As HResult

        '[In, MarshalAs(UnmanagedType.Interface)] IBindCtx
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function BindToObject(<[In]> pidl As IntPtr, pbc As IntPtr, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub BindToStorage(<[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub CompareIDs(<[In]> lParam As IntPtr, <[In]> ByRef pidl1 As IntPtr, <[In]> ByRef pidl2 As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub CreateViewObject(<[In]> hwndOwner As IntPtr, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetAttributesOf(<[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In], Out> ByRef rgfInOut As UInteger)


        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetUIObjectOf(<[In]> hwndOwner As IntPtr, <[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In]> ByRef riid As Guid, <[In], Out> ByRef rgfReserved As UInteger, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDisplayNameOf(<[In]> ByRef pidl As IntPtr, <[In]> uFlags As UInteger, ByRef pName As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetNameOf(<[In]> hwnd As IntPtr, <[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String, <[In]> uFlags As UInteger, <Out> ppidlOut As IntPtr)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellFolder2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss>
    Friend Interface IShellFolder2
        Inherits IShellFolder
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub ParseDisplayName(<[In]> hwnd As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String, <[In], Out> ByRef pchEaten As UInteger, <Out> ppidl As IntPtr, <[In], Out> ByRef pdwAttributes As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub EnumObjects(<[In]> hwnd As IntPtr, <[In]> grfFlags As ShellNativeMethods.ShellFolderEnumerationOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef ppenumIDList As IEnumIDList)

        '[In, MarshalAs(UnmanagedType.Interface)] IBindCtx
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub BindToObject(<[In]> pidl As IntPtr, pbc As IntPtr, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub BindToStorage(<[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub CompareIDs(<[In]> lParam As IntPtr, <[In]> ByRef pidl1 As IntPtr, <[In]> ByRef pidl2 As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub CreateViewObject(<[In]> hwndOwner As IntPtr, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetAttributesOf(<[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In], Out> ByRef rgfInOut As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetUIObjectOf(<[In]> hwndOwner As IntPtr, <[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In]> ByRef riid As Guid, <[In], Out> ByRef rgfReserved As UInteger, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub GetDisplayNameOf(<[In]> ByRef pidl As IntPtr, <[In]> uFlags As UInteger, ByRef pName As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Overloads Sub SetNameOf(<[In]> hwnd As IntPtr, <[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String, <[In]> uFlags As UInteger, <Out> ppidlOut As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultSearchGUID(ByRef pguid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub EnumSearches(<Out> ByRef ppenum As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultColumn(<[In]> dwRes As UInteger, ByRef pSort As UInteger, ByRef pDisplay As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultColumnState(<[In]> iColumn As UInteger, ByRef pcsFlags As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDetailsEx(<[In]> ByRef pidl As IntPtr, <[In]> ByRef pscid As PropertyKey, <MarshalAs(UnmanagedType.Struct)> ByRef pv As Object)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDetailsOf(<[In]> ByRef pidl As IntPtr, <[In]> iColumn As UInteger, ByRef psd As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub MapColumnToSCID(<[In]> iColumn As UInteger, ByRef pscid As PropertyKey)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IEnumIDList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IEnumIDList
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function [Next](celt As UInteger, ByRef rgelt As IntPtr, ByRef pceltFetched As UInteger) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Skip(<[In]> celt As UInteger) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Reset() As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Clone(<MarshalAs(UnmanagedType.[Interface])> ByRef ppenum As IEnumIDList) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellLinkW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellLinkW
        'ref _WIN32_FIND_DATAW pfd,
        Sub GetPath(<Out, MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder, cchMaxPath As Integer, pfd As IntPtr, fFlags As UInteger)
        Sub GetIDList(ByRef ppidl As IntPtr)
        Sub SetIDList(pidl As IntPtr)
        Sub GetDescription(<Out, MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder, cchMaxName As Integer)
        Sub SetDescription(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Sub GetWorkingDirectory(<Out, MarshalAs(UnmanagedType.LPWStr)> pszDir As StringBuilder, cchMaxPath As Integer)
        Sub SetWorkingDirectory(<MarshalAs(UnmanagedType.LPWStr)> pszDir As String)
        Sub GetArguments(<Out, MarshalAs(UnmanagedType.LPWStr)> pszArgs As StringBuilder, cchMaxPath As Integer)
        Sub SetArguments(<MarshalAs(UnmanagedType.LPWStr)> pszArgs As String)
        Sub GetHotKey(ByRef wHotKey As Short)
        Sub SetHotKey(wHotKey As Short)
        Sub GetShowCmd(ByRef iShowCmd As UInteger)
        Sub SetShowCmd(iShowCmd As UInteger)
        Sub GetIconLocation(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef pszIconPath As StringBuilder, cchIconPath As Integer, ByRef iIcon As Integer)
        Sub SetIconLocation(<MarshalAs(UnmanagedType.LPWStr)> pszIconPath As String, iIcon As Integer)
        Sub SetRelativePath(<MarshalAs(UnmanagedType.LPWStr)> pszPathRel As String, dwReserved As UInteger)
        Sub Resolve(hwnd As IntPtr, fFlags As UInteger)
        Sub SetPath(<MarshalAs(UnmanagedType.LPWStr)> pszFile As String)
    End Interface

    <ComImport, Guid(ShellIIDGuid.CShellLink), ClassInterface(ClassInterfaceType.None)>
    Friend Class CShellLink
    End Class

    ' Summary:
    '     Provides the managed definition of the IPersistStream interface, with functionality
    '     from IPersist.
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("00000109-0000-0000-C000-000000000046")>
    Friend Interface IPersistStream
        ' Summary:
        '     Retrieves the class identifier (CLSID) of an object.
        '
        ' Parameters:
        '   pClassID:
        '     When this method returns, contains a reference to the CLSID. This parameter
        '     is passed uninitialized.
        <PreserveSig>
        Sub GetClassID(ByRef pClassID As Guid)
        '
        ' Summary:
        '     Checks an object for changes since it was last saved to its current file.
        '
        ' Returns:
        '     S_OK if the file has changed since it was last saved; S_FALSE if the file
        '     has not changed since it was last saved.
        <PreserveSig>
        Function IsDirty() As HResult

        <PreserveSig>
        Function Load(<[In], MarshalAs(UnmanagedType.[Interface])> stm As IStream) As HResult

        <PreserveSig>
        Function Save(<[In], MarshalAs(UnmanagedType.[Interface])> stm As IStream, fRemember As Boolean) As HResult

        <PreserveSig>
        Function GetSizeMax(ByRef cbSize As ULong) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.ICondition), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface ICondition
        Inherits IPersistStream
        ' Summary:
        '     Retrieves the class identifier (CLSID) of an object.
        '
        ' Parameters:
        '   pClassID:
        '     When this method returns, contains a reference to the CLSID. This parameter
        '     is passed uninitialized.
        <PreserveSig>
        Overloads Sub GetClassID(ByRef pClassID As Guid)
        '
        ' Summary:
        '     Checks an object for changes since it was last saved to its current file.
        '
        ' Returns:
        '     S_OK if the file has changed since it was last saved; S_FALSE if the file
        '     has not changed since it was last saved.
        <PreserveSig>
        Overloads Function IsDirty() As HResult

        <PreserveSig>
        Overloads Function Load(<[In], MarshalAs(UnmanagedType.[Interface])> stm As IStream) As HResult

        <PreserveSig>
        Overloads Function Save(<[In], MarshalAs(UnmanagedType.[Interface])> stm As IStream, fRemember As Boolean) As HResult

        <PreserveSig>
        Overloads Function GetSizeMax(ByRef cbSize As ULong) As HResult

        ' For any node, return what kind of node it is.
        <PreserveSig>
        Function GetConditionType(<Out> ByRef pNodeType As SearchConditionType) As HResult

        ' riid must be IID_IEnumUnknown, IID_IEnumVARIANT or IID_IObjectArray, or in the case of a negation node IID_ICondition.
        ' If this is a leaf node, E_FAIL will be returned.
        ' If this is a negation node, then if riid is IID_ICondition, *ppv will be set to a single ICondition, otherwise an enumeration of one.
        ' If this is a conjunction or a disjunction, *ppv will be set to an enumeration of the subconditions.
        <PreserveSig>
        Function GetSubConditions(<[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As Object) As HResult

        ' If this is not a leaf node, E_FAIL will be returned.
        ' Retrieve the property name, operation and value from the leaf node.
        ' Any one of ppszPropertyName, pcop and ppropvar may be NULL.
        <PreserveSig>
        Function GetComparisonInfo(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszPropertyName As String, <Out> ByRef pcop As SearchConditionOperation, <Out> ppropvar As PropVariant) As HResult

        ' If this is not a leaf node, E_FAIL will be returned.
        ' *ppszValueTypeName will be set to the semantic type of the value, or to NULL if this is not meaningful.
        <PreserveSig>
        Function GetValueType(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszValueTypeName As String) As HResult

        ' If this is not a leaf node, E_FAIL will be returned.
        ' If the value of the leaf node is VT_EMPTY, *ppszNormalization will be set to an empty string.
        ' If the value is a string (VT_LPWSTR, VT_BSTR or VT_LPSTR), then *ppszNormalization will be set to a
        ' character-normalized form of the value.
        ' Otherwise, *ppszNormalization will be set to some (character-normalized) string representation of the value.
        <PreserveSig>
        Function GetValueNormalization(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszNormalization As String) As HResult

        ' Return information about what parts of the input produced the property, the operation and the value.
        ' Any one of ppPropertyTerm, ppOperationTerm and ppValueTerm may be NULL.
        ' For a leaf node returned by the parser, the position information of each IRichChunk identifies the tokens that
        ' contributed the property/operation/value, the string value is the corresponding part of the input string, and
        ' the PROPVARIANT is VT_EMPTY.
        <PreserveSig>
        Function GetInputTerms(<Out> ByRef ppPropertyTerm As IRichChunk, <Out> ByRef ppOperationTerm As IRichChunk, <Out> ByRef ppValueTerm As IRichChunk) As HResult

        ' Make a deep copy of this ICondition.
        <PreserveSig>
        Function Clone(<Out> ByRef ppc As ICondition) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IRichChunk), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IRichChunk
        ' The position *pFirstPos is zero-based.
        ' Any one of pFirstPos, pLength, ppsz and pValue may be NULL.
        '[out, annotation("__out_opt")] ULONG* pFirstPos, [out, annotation("__out_opt")] ULONG* pLength, [out, annotation("__deref_opt_out_opt")] LPWSTR* ppsz, [out, annotation("__out_opt")] PROPVARIANT* pValue
        <PreserveSig>
        Function GetData() As HResult
    End Interface

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid(ShellIIDGuid.IEnumUnknown)>
    Friend Interface IEnumUnknown
        <PreserveSig>
        Function [Next](requestedNumber As UInt32, ByRef buffer As IntPtr, ByRef fetchedNumber As UInt32) As HResult
        <PreserveSig>
        Function Skip(number As UInt32) As HResult
        <PreserveSig>
        Function Reset() As HResult
        <PreserveSig>
        Function Clone(ByRef result As IEnumUnknown) As HResult
    End Interface


    <ComImport, Guid(ShellIIDGuid.IConditionFactory), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IConditionFactory
        <PreserveSig>
        Function MakeNot(<[In]> pcSub As ICondition, <[In]> fSimplify As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        <PreserveSig>
        Function MakeAndOr(<[In]> ct As SearchConditionType, <[In]> peuSubs As IEnumUnknown, <[In]> fSimplify As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        <PreserveSig>
        Function MakeLeaf(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropertyName As String, <[In]> cop As SearchConditionOperation, <[In], MarshalAs(UnmanagedType.LPWStr)> pszValueType As String, <[In]> ppropvar As PropVariant, richChunk1 As IRichChunk, richChunk2 As IRichChunk,
            richChunk3 As IRichChunk, <[In]> fExpand As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        '[In] ICondition pc, [In] STRUCTURED_QUERY_RESOLVE_OPTION sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved
        <PreserveSig>
        Function Resolve() As HResult

    End Interface

    <ComImport, Guid(ShellIIDGuid.IConditionFactory), CoClass(GetType(ConditionFactoryCoClass))>
    Friend Interface INativeConditionFactory
        Inherits IConditionFactory
    End Interface

    <ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.ConditionFactory)>
    Friend Class ConditionFactoryCoClass
    End Class



    <ComImport, Guid(ShellIIDGuid.ISearchFolderItemFactory), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface ISearchFolderItemFactory
        <PreserveSig>
        Function SetDisplayName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String) As HResult

        <PreserveSig>
        Function SetFolderTypeID(<[In]> ftid As Guid) As HResult

        <PreserveSig>
        Function SetFolderLogicalViewMode(<[In]> flvm As FolderLogicalViewMode) As HResult

        <PreserveSig>
        Function SetIconSize(<[In]> iIconSize As Integer) As HResult

        <PreserveSig>
        Function SetVisibleColumns(<[In]> cVisibleColumns As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgKey As PropertyKey()) As HResult

        <PreserveSig>
        Function SetSortColumns(<[In]> cSortColumns As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgSortColumns As SortColumn()) As HResult

        <PreserveSig>
        Function SetGroupColumn(<[In]> ByRef keyGroup As PropertyKey) As HResult

        <PreserveSig>
        Function SetStacks(<[In]> cStackKeys As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgStackKeys As PropertyKey()) As HResult

        <PreserveSig>
        Function SetScope(<[In], MarshalAs(UnmanagedType.[Interface])> ppv As IShellItemArray) As HResult

        <PreserveSig>
        Function SetCondition(<[In]> pCondition As ICondition) As HResult

        <PreserveSig>
        Function GetShellItem(ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem) As Integer

        <PreserveSig>
        Function GetIDList(<Out> ppidl As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.ISearchFolderItemFactory), CoClass(GetType(SearchFolderItemFactoryCoClass))>
    Friend Interface INativeSearchFolderItemFactory
        Inherits ISearchFolderItemFactory
    End Interface

    <ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.SearchFolderItemFactory)>
    Friend Class SearchFolderItemFactoryCoClass
    End Class

    <ComImport, Guid(ShellIIDGuid.IQuerySolution), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IQuerySolution
        Inherits IConditionFactory
        <PreserveSig>
        Overloads Function MakeNot(<[In]> pcSub As ICondition, <[In]> fSimplify As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        <PreserveSig>
        Overloads Function MakeAndOr(<[In]> ct As SearchConditionType, <[In]> peuSubs As IEnumUnknown, <[In]> fSimplify As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        <PreserveSig>
        Overloads Function MakeLeaf(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropertyName As String, <[In]> cop As SearchConditionOperation, <[In], MarshalAs(UnmanagedType.LPWStr)> pszValueType As String, <[In]> ppropvar As PropVariant, richChunk1 As IRichChunk, richChunk2 As IRichChunk,
            richChunk3 As IRichChunk, <[In]> fExpand As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        '[In] ICondition pc, [In] int sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved
        <PreserveSig>
        Overloads Function Resolve() As HResult

        ' Retrieve the condition tree and the "main type" of the solution.
        ' ppQueryNode and ppMainType may be NULL.
        <PreserveSig>
        Function GetQuery(<Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppQueryNode As ICondition, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppMainType As IEntity) As HResult

        ' Identify parts of the input string not accounted for.
        ' Each parse error is represented by an IRichChunk where the position information
        ' reflect token counts, the string is NULL and the value is a VT_I4
        ' where lVal is from the ParseErrorType enumeration. The valid
        ' values for riid are IID_IEnumUnknown and IID_IEnumVARIANT.
        ' void** 
        <PreserveSig>
        Function GetErrors(<[In]> ByRef riid As Guid, <Out> ByRef ppParseErrors As IntPtr) As HResult

        ' Report the query string, how it was tokenized and what LCID and word breaker were used (for recognizing keywords).
        ' ppszInputString, ppTokens, pLocale and ppWordBreaker may be NULL.
        ' ITokenCollection** 
        ' IUnknown** 
        <PreserveSig>
        Function GetLexicalData(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszInputString As String, <Out> ByRef ppTokens As IntPtr, <Out> ByRef plcid As UInteger, <Out> ByRef ppWordBreaker As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IQueryParser), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IQueryParser
        ' Parse parses an input string, producing a query solution.
        ' pCustomProperties should be an enumeration of IRichChunk objects, one for each custom property
        ' the application has recognized. pCustomProperties may be NULL, equivalent to an empty enumeration.
        ' For each IRichChunk, the position information identifies the character span of the custom property,
        ' the string value should be the name of an actual property, and the PROPVARIANT is completely ignored.
        <PreserveSig>
        Function Parse(<[In], MarshalAs(UnmanagedType.LPWStr)> pszInputString As String, <[In]> pCustomProperties As IEnumUnknown, <Out> ByRef ppSolution As IQuerySolution) As HResult

        ' Set a single option. See STRUCTURED_QUERY_SINGLE_OPTION above.
        <PreserveSig>
        Function SetOption(<[In]> [option] As StructuredQuerySingleOption, <[In]> pOptionValue As PropVariant) As HResult

        <PreserveSig>
        Function GetOption(<[In]> [option] As StructuredQuerySingleOption, <Out> pOptionValue As PropVariant) As HResult

        ' Set a multi option. See STRUCTURED_QUERY_MULTIOPTION above.
        <PreserveSig>
        Function SetMultiOption(<[In]> [option] As StructuredQueryMultipleOption, <[In], MarshalAs(UnmanagedType.LPWStr)> pszOptionKey As String, <[In]> pOptionValue As PropVariant) As HResult

        ' Get a schema provider for browsing the currently loaded schema.
        'ISchemaProvider
        <PreserveSig>
        Function GetSchemaProvider(<Out> ByRef ppSchemaProvider As IntPtr) As HResult

        ' Restate a condition as a query string according to the currently selected syntax.
        ' The parameter fUseEnglish is reserved for future use; must be FALSE.
        <PreserveSig>
        Function RestateToString(<[In]> pCondition As ICondition, <[In]> fUseEnglish As Boolean, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszQueryString As String) As HResult

        ' Parse a condition for a given property. It can be anything that would go after 'PROPERTY:' in an AQS expession.
        <PreserveSig>
        Function ParsePropertyValue(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropertyName As String, <[In], MarshalAs(UnmanagedType.LPWStr)> pszInputString As String, <Out> ByRef ppSolution As IQuerySolution) As HResult

        ' Restate a condition for a given property. If the condition contains a leaf with any other property name, or no property name at all,
        ' E_INVALIDARG will be returned.
        <PreserveSig>
        Function RestatePropertyValueToString(<[In]> pCondition As ICondition, <[In]> fUseEnglish As Boolean, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszPropertyName As String, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszQueryString As String) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IQueryParserManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IQueryParserManager
        ' Create a query parser loaded with the schema for a certain catalog localize to a certain language, and initialized with
        ' standard defaults. One valid value for riid is IID_IQueryParser.
        <PreserveSig>
        Function CreateLoadedParser(<[In], MarshalAs(UnmanagedType.LPWStr)> pszCatalog As String, <[In]> langidForKeywords As UShort, <[In]> ByRef riid As Guid, <Out> ByRef ppQueryParser As IQueryParser) As HResult

        ' In addition to setting AQS/NQS and automatic wildcard for the given query parser, this sets up standard named entity handlers and
        ' sets the keyboard locale as locale for word breaking.
        <PreserveSig>
        Function InitializeOptions(<[In]> fUnderstandNQS As Boolean, <[In]> fAutoWildCard As Boolean, <[In]> pQueryParser As IQueryParser) As HResult

        ' Change one of the settings for the query parser manager, such as the name of the schema binary, or the location of the localized and unlocalized
        ' schema binaries. By default, the settings point to the schema binaries used by Windows Shell.
        <PreserveSig>
        Function SetOption(<[In]> [option] As QueryParserManagerOption, <[In]> pOptionValue As PropVariant) As HResult

    End Interface

    <ComImport, Guid(ShellIIDGuid.IQueryParserManager), CoClass(GetType(QueryParserManagerCoClass))>
    Friend Interface INativeQueryParserManager
        Inherits IQueryParserManager
    End Interface

    <ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.QueryParserManager)>
    Friend Class QueryParserManagerCoClass
    End Class

    <ComImport, Guid("24264891-E80B-4fd3-B7CE-4FF2FAE8931F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IEntity
        ' TODO
    End Interface
#End Region

    '#Pragma warning restore 108
End Namespace
