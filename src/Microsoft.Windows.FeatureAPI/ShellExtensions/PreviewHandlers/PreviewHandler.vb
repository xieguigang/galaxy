Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.Shell.Interop
Imports Microsoft.Windows.ShellExtensions.Interop

Namespace ShellExtensions

    ''' <summary>
    ''' This is the base class for all preview handlers and provides their basic functionality.
    ''' To create a custom preview handler a class must derive from this, use the <see cref="PreviewHandlerAttribute"/>,
    ''' and implement 1 or more of the following interfaces: 
    ''' <see cref="IPreviewFromStream"/>, 
    ''' <see cref="IPreviewFromShellObject"/>, 
    ''' <see cref="IPreviewFromFile"/>.   
    ''' </summary>
    Public MustInherit Class PreviewHandler
        Implements ICustomQueryInterface
        Implements ShellExtensions.Interop.IPreviewHandler
        Implements ShellExtensions.Interop.IPreviewHandlerVisuals
        Implements ShellExtensions.Interop.IOleWindow
        Implements ShellExtensions.Interop.IObjectWithSite
        Implements ShellExtensions.Interop.IInitializeWithStream
        Implements ShellExtensions.Interop.IInitializeWithItem
        Implements ShellExtensions.Interop.IInitializeWithFile

        Private _parentHwnd As IntPtr
        Private _frame As ShellExtensions.Interop.IPreviewHandlerFrame

        ''' <summary>
        ''' Gets whether the preview is currently showing
        ''' </summary>
        Public ReadOnly Property IsPreviewShowing() As Boolean

        ''' <summary>
        ''' Called immediately before the preview is to be shown.        
        ''' </summary>
        Protected Overridable Sub Initialize()
        End Sub

        ''' <summary>
        ''' Called when the preview is no longer shown.
        ''' </summary>
        Protected Overridable Sub Uninitialize()
        End Sub

#Region "Required functions - Abstract functions"
        ''' <summary>
        ''' This should return the window handle to be displayed in the Preview.
        ''' </summary>
        Protected MustOverride ReadOnly Property Handle() As IntPtr

        ''' <summary>
        ''' Called to update the bounds and position of the preview control
        ''' </summary>
        ''' <param name="bounds"></param>
        Protected MustOverride Sub UpdateBounds(bounds As NativeRect)

        ''' <summary>
        ''' Called when an exception occurs during the initialization of the control
        ''' </summary>
        ''' <param name="caughtException"></param>
        Protected MustOverride Sub HandleInitializeException(caughtException As Exception)

        ''' <summary>
        ''' Called when the preview control obtains focus.
        ''' </summary>
        Protected MustOverride Sub SetFocus()

        ''' <summary>
        ''' Called when a request is received to set or change the background color according to the user's preferences.
        ''' </summary>
        ''' <param name="argb">An int representing the ARGB color</param>
        Protected MustOverride Sub SetBackground(argb As Integer)

        ''' <summary>
        ''' Called when a request is received to set or change the foreground color according to the user's preferences.
        ''' </summary>
        ''' <param name="argb">An int representing the ARGB color</param>
        Protected MustOverride Sub SetForeground(argb As Integer)

        ''' <summary>
        ''' Called to set the font of the preview control according to the user's preferences.
        ''' </summary>
        ''' <param name="font"></param>
        Protected MustOverride Sub SetFont(font As ShellExtensions.Interop.LogFont)

        ''' <summary>
        ''' Called to set the parent of the preview control.
        ''' </summary>
        ''' <param name="handle"></param>
        Protected MustOverride Sub SetParentHandle(handle As IntPtr)
#End Region

#Region "IPreviewHandler"
        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_SetWindow(hwnd As IntPtr, ByRef rect As NativeRect) Implements ShellExtensions.Interop.IPreviewHandler.SetWindow
            _parentHwnd = hwnd
            UpdateBounds(rect)
            SetParentHandle(_parentHwnd)
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_SetRect(ByRef rect As NativeRect) Implements ShellExtensions.Interop.IPreviewHandler.SetRect
            UpdateBounds(rect)
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_DoPreview() Implements ShellExtensions.Interop.IPreviewHandler.DoPreview
            _IsPreviewShowing = True
            Try
                Initialize()
            Catch exc As Exception
                HandleInitializeException(exc)
            End Try
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_Unload() Implements ShellExtensions.Interop.IPreviewHandler.Unload
            Uninitialize()
            _IsPreviewShowing = False
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_SetFocus() Implements ShellExtensions.Interop.IPreviewHandler.SetFocus
            SetFocus()
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_QueryFocus(ByRef phwnd As IntPtr) Implements ShellExtensions.Interop.IPreviewHandler.QueryFocus
            phwnd = ShellExtensions.Interop.HandlerNativeMethods.GetFocus()
        End Sub

        Private Function Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandler_TranslateAccelerator(ByRef pmsg As Message) As HResult Implements ShellExtensions.Interop.IPreviewHandler.TranslateAccelerator
            Return If(_frame IsNot Nothing, _frame.TranslateAccelerator(pmsg), HResult.[False])
        End Function
#End Region

#Region "IPreviewHandlerVisuals"
        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandlerVisuals_SetBackgroundColor(color As NativeColorRef) Implements ShellExtensions.Interop.IPreviewHandlerVisuals.SetBackgroundColor
            SetBackground(CInt(color.Dword))
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandlerVisuals_SetTextColor(color As NativeColorRef) Implements ShellExtensions.Interop.IPreviewHandlerVisuals.SetTextColor
            SetForeground(CInt(color.Dword))
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IPreviewHandlerVisuals_SetFont(ByRef plf As ShellExtensions.Interop.LogFont) Implements ShellExtensions.Interop.IPreviewHandlerVisuals.SetFont
            SetFont(plf)
        End Sub
#End Region

#Region "IOleWindow"
        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IOleWindow_GetWindow(ByRef phwnd As IntPtr) Implements ShellExtensions.Interop.IOleWindow.GetWindow
            phwnd = Handle
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IOleWindow_ContextSensitiveHelp(fEnterMode As Boolean) Implements ShellExtensions.Interop.IOleWindow.ContextSensitiveHelp
            ' Preview handlers don't support context sensitive help. (As far as I know.)
            Throw New NotImplementedException()
        End Sub
#End Region

#Region "IObjectWithSite"
        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IObjectWithSite_SetSite(pUnkSite As Object) Implements ShellExtensions.Interop.IObjectWithSite.SetSite
            _frame = TryCast(pUnkSite, ShellExtensions.Interop.IPreviewHandlerFrame)
        End Sub

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IObjectWithSite_GetSite(ByRef riid As Guid, ByRef ppvSite As Object) Implements ShellExtensions.Interop.IObjectWithSite.GetSite
            ppvSite = DirectCast(_frame, Object)
        End Sub
#End Region

#Region "IInitializeWithStream Members"

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IInitializeWithStream_Initialize(stream As System.Runtime.InteropServices.ComTypes.IStream, fileMode As Shell.AccessModes) Implements ShellExtensions.Interop.IInitializeWithStream.Initialize
            Dim preview As IPreviewFromStream = TryCast(Me, IPreviewFromStream)
            If preview Is Nothing Then
                Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.PreviewHandlerUnsupportedInterfaceCalled, "IPreviewFromStream"))
            End If
            Using storageStream = New StorageStream(stream, fileMode <> Shell.AccessModes.ReadWrite)
                preview.Load(storageStream)
            End Using
        End Sub

#End Region

#Region "IInitializeWithItem Members"

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IInitializeWithItem_Initialize(shellItem As IShellItem, accessMode As Shell.AccessModes) Implements ShellExtensions.Interop.IInitializeWithItem.Initialize
            Dim preview As IPreviewFromShellObject = TryCast(Me, IPreviewFromShellObject)
            If preview Is Nothing Then
                Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.PreviewHandlerUnsupportedInterfaceCalled, "IPreviewFromShellObject"))
            End If
            Using shellObject = Shell.ShellObjectFactory.Create(shellItem)
                preview.Load(shellObject)
            End Using
        End Sub

#End Region

#Region "IInitializeWithFile Members"

        Private Sub Microsoft_WindowsAPICodePack_ShellExtensions_Interop_IInitializeWithFile_Initialize(filePath As String, fileMode As Shell.AccessModes) Implements ShellExtensions.Interop.IInitializeWithFile.Initialize
            Dim preview As IPreviewFromFile = TryCast(Me, IPreviewFromFile)
            If preview Is Nothing Then
                Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.PreviewHandlerUnsupportedInterfaceCalled, "IPreviewFromFile"))
            End If
            preview.Load(New FileInfo(filePath))
        End Sub

#End Region

#Region "ComRegistration"
        ''' <summary>
        ''' Called when the assembly is registered via RegAsm.
        ''' </summary>
        ''' <param name="registerType">Type to register.</param>
        <ComRegisterFunction>
        Private Shared Sub Register(registerType As Type)
            If registerType IsNot Nothing AndAlso registerType.IsSubclassOf(GetType(PreviewHandler)) Then
                Dim attrs As Object() = DirectCast(registerType.GetCustomAttributes(GetType(PreviewHandlerAttribute), True), Object())
                If attrs IsNot Nothing AndAlso attrs.Length = 1 Then
                    Dim attr As PreviewHandlerAttribute = TryCast(attrs(0), PreviewHandlerAttribute)
                    ThrowIfNotValid(registerType)
                    RegisterPreviewHandler(registerType.GUID, attr)
                Else
                    Throw New NotSupportedException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.PreviewHandlerInvalidAttributes, registerType.Name))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Called when the assembly is Unregistered via RegAsm.
        ''' </summary>
        ''' <param name="registerType">Type to unregister</param>
        <ComUnregisterFunction>
        Private Shared Sub Unregister(registerType As Type)
            If registerType IsNot Nothing AndAlso registerType.IsSubclassOf(GetType(PreviewHandler)) Then
                Dim attrs As Object() = DirectCast(registerType.GetCustomAttributes(GetType(PreviewHandlerAttribute), True), Object())
                If attrs IsNot Nothing AndAlso attrs.Length = 1 Then
                    Dim attr As PreviewHandlerAttribute = TryCast(attrs(0), PreviewHandlerAttribute)
                    UnregisterPreviewHandler(registerType.GUID, attr)
                End If
            End If
        End Sub

        Private Shared Sub RegisterPreviewHandler(previewerGuid As Guid, attribute As PreviewHandlerAttribute)
            Dim guid As String = previewerGuid.ToString("B")
            ' Create a new prevhost AppID so that this always runs in its own isolated process
            Using appIdsKey As RegistryKey = Registry.ClassesRoot.OpenSubKey("AppID", True)
                Using appIdKey As RegistryKey = appIdsKey.CreateSubKey(attribute.AppId)
                    appIdKey.SetValue("DllSurrogate", "%SystemRoot%\system32\prevhost.exe", RegistryValueKind.ExpandString)
                End Using
            End Using

            ' Add preview handler to preview handler list
            Using handlersKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", True)
                handlersKey.SetValue(guid, attribute.Name, RegistryValueKind.[String])
            End Using

            ' Modify preview handler registration
            Using clsidKey As RegistryKey = Registry.ClassesRoot.OpenSubKey("CLSID")
                Using idKey As RegistryKey = clsidKey.OpenSubKey(guid, True)
                    idKey.SetValue("DisplayName", attribute.Name, RegistryValueKind.[String])
                    idKey.SetValue("AppID", attribute.AppId, RegistryValueKind.[String])
                    idKey.SetValue("DisableLowILProcessIsolation", If(attribute.DisableLowILProcessIsolation, 1, 0), RegistryValueKind.DWord)

                    Using inproc As RegistryKey = idKey.OpenSubKey("InprocServer32", True)
                        inproc.SetValue("ThreadingModel", "Apartment", RegistryValueKind.[String])
                    End Using
                End Using
            End Using

            For Each extension As String In attribute.Extensions.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)
                Trace.WriteLine("Registering extension '" & extension & "' with previewer '" & guid & "'")

                ' Set preview handler for specific extension
                Using extensionKey As RegistryKey = Registry.ClassesRoot.CreateSubKey(extension)
                    Using shellexKey As RegistryKey = extensionKey.CreateSubKey("shellex")
                        Using previewKey As RegistryKey = shellexKey.CreateSubKey(ShellExtensions.Interop.HandlerNativeMethods.IPreviewHandlerGuid.ToString("B"))
                            previewKey.SetValue(Nothing, guid, RegistryValueKind.[String])
                        End Using
                    End Using
                End Using
            Next
        End Sub

        Private Shared Sub UnregisterPreviewHandler(previewerGuid As Guid, attribute As PreviewHandlerAttribute)
            Dim guid As String = previewerGuid.ToString("B")
            For Each extension As String In attribute.Extensions.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)
                Trace.WriteLine("Unregistering extension '" & extension & "' with previewer '" & guid & "'")
                Using shellexKey As RegistryKey = Registry.ClassesRoot.OpenSubKey(extension & "\shellex", True)
                    shellexKey.DeleteSubKey(ShellExtensions.Interop.HandlerNativeMethods.IPreviewHandlerGuid.ToString(), False)
                End Using
            Next

            Using appIdsKey As RegistryKey = Registry.ClassesRoot.OpenSubKey("AppID", True)
                appIdsKey.DeleteSubKey(attribute.AppId, False)
            End Using

            Using classesKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", True)
                classesKey.DeleteValue(guid, False)
            End Using
        End Sub

        Private Shared Sub ThrowIfNotValid(type As Type)
            Dim interfaces = type.GetInterfaces()
            If Not interfaces.Any(Function(x) x Is GetType(IPreviewFromStream) OrElse x Is GetType(IPreviewFromShellObject) OrElse x Is GetType(IPreviewFromFile)) Then
                Throw New NotImplementedException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.PreviewHandlerInterfaceNotImplemented, type.Name))
            End If
        End Sub

#End Region

#Region "ICustomQueryInterface Members"

        Private Function ICustomQueryInterface_GetInterface(ByRef iid As Guid, ByRef ppv As IntPtr) As CustomQueryInterfaceResult Implements ICustomQueryInterface.GetInterface
            ppv = IntPtr.Zero
            ' Forces COM to not use the managed (free threaded) marshaler
            If iid = ShellExtensions.Interop.HandlerNativeMethods.IMarshalGuid Then
                Return CustomQueryInterfaceResult.Failed
            End If

            If (iid = ShellExtensions.Interop.HandlerNativeMethods.IInitializeWithStreamGuid AndAlso Not (TypeOf Me Is IPreviewFromStream)) OrElse (iid = ShellExtensions.Interop.HandlerNativeMethods.IInitializeWithItemGuid AndAlso Not (TypeOf Me Is IPreviewFromShellObject)) OrElse (iid = ShellExtensions.Interop.HandlerNativeMethods.IInitializeWithFileGuid AndAlso Not (TypeOf Me Is IPreviewFromFile)) Then
                Return CustomQueryInterfaceResult.Failed
            End If

            Return CustomQueryInterfaceResult.NotHandled
        End Function


#End Region
    End Class
End Namespace
