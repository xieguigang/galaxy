Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports Microsoft.Windows.Shell
Imports Microsoft.Windows.ShellExtensions.Interop
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Taskbar

Namespace ShellExtensions

    ''' <summary>
    ''' This is the base class for all thumbnail providers and provides their basic functionality.
    ''' To create a custom thumbnail provider a class must derive from this, use the <see cref="ThumbnailProviderAttribute"/>,
    ''' and implement 1 or more of the following interfaces: 
    ''' <see cref="IThumbnailFromStream"/>, <see cref="IThumbnailFromShellObject"/>, <see cref="IThumbnailFromFile"/>.   
    ''' </summary>
    Public MustInherit Class ThumbnailProvider
        Implements IThumbnailProvider
        Implements ICustomQueryInterface
        Implements IDisposable
        Implements IInitializeWithStream
        Implements IInitializeWithItem
        Implements IInitializeWithFile

        ' Determines which interface should be called to return a bitmap
        Private Function GetBitmap(sideLength As Integer) As Bitmap
            Dim stream As IThumbnailFromStream
            Dim shellObject As IThumbnailFromShellObject
            Dim file As IThumbnailFromFile

            If _stream IsNot Nothing AndAlso stream.DirectCopy(TryCast(Me, IThumbnailFromStream)) IsNot Nothing Then
                Return stream.ConstructBitmap(_stream, sideLength)
            End If
            If _shellObject IsNot Nothing AndAlso shellObject.DirectCopy(TryCast(Me, IThumbnailFromShellObject)) IsNot Nothing Then
                Return shellObject.ConstructBitmap(_shellObject, sideLength)
            End If
            If _info IsNot Nothing AndAlso file.DirectCopy(TryCast(Me, IThumbnailFromFile)) IsNot Nothing Then
                Return file.ConstructBitmap(_info, sideLength)
            End If

            Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.ThumbnailProviderInterfaceNotImplemented, Me.[GetType]().Name))
        End Function

        ''' <summary>
        ''' Sets the AlphaType of the generated thumbnail.
        ''' Override this method in a derived class to change the thumbnails AlphaType, default is Unknown.
        ''' </summary>
        ''' <returns>ThumnbailAlphaType</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")>
        Public Overridable Function GetThumbnailAlphaType() As ThumbnailAlphaType
            Return ThumbnailAlphaType.Unknown
        End Function

        Private _stream As StorageStream = Nothing
        Private _info As FileInfo = Nothing
        Private _shellObject As ShellObject = Nothing

#Region "IThumbnailProvider Members"

        Private Sub IThumbnailProvider_GetThumbnail(sideLength As UInteger, ByRef hBitmap As IntPtr, ByRef alphaType As UInteger) Implements IThumbnailProvider.GetThumbnail
            Using map As Bitmap = GetBitmap(CInt(sideLength))
                hBitmap = map.GetHbitmap()
            End Using
            alphaType = CUInt(GetThumbnailAlphaType())
        End Sub

#End Region

#Region "ICustomQueryInterface Members"

        Private Function ICustomQueryInterface_GetInterface(ByRef iid As Guid, ByRef ppv As IntPtr) As CustomQueryInterfaceResult Implements ICustomQueryInterface.GetInterface
            ppv = IntPtr.Zero

            ' Forces COM to not use the managed (free threaded) marshaler
            If iid = HandlerNativeMethods.IMarshalGuid Then
                Return CustomQueryInterfaceResult.Failed
            End If

            If (iid = HandlerNativeMethods.IInitializeWithStreamGuid AndAlso Not (TypeOf Me Is IThumbnailFromStream)) OrElse (iid = HandlerNativeMethods.IInitializeWithItemGuid AndAlso Not (TypeOf Me Is IThumbnailFromShellObject)) OrElse (iid = HandlerNativeMethods.IInitializeWithFileGuid AndAlso Not (TypeOf Me Is IThumbnailFromFile)) Then
                Return CustomQueryInterfaceResult.Failed
            End If

            Return CustomQueryInterfaceResult.NotHandled
        End Function

#End Region

#Region "COM Registration"

        ''' <summary>
        ''' Called when the assembly is registered via RegAsm.
        ''' </summary>
        ''' <param name="registerType">Type to be registered.</param>
        <ComRegisterFunction>
        Private Shared Sub Register(registerType As Type)
            If registerType IsNot Nothing AndAlso registerType.IsSubclassOf(GetType(ThumbnailProvider)) Then
                Dim attributes As Object() = registerType.GetCustomAttributes(GetType(ThumbnailProviderAttribute), True)
                If attributes IsNot Nothing AndAlso attributes.Length = 1 Then
                    Dim attribute As ThumbnailProviderAttribute = TryCast(attributes(0), ThumbnailProviderAttribute)
                    ThrowIfInvalid(registerType, attribute)
                    RegisterThumbnailHandler(registerType.GUID.ToString("B"), attribute)
                End If
            End If
        End Sub

        Private Shared Sub RegisterThumbnailHandler(guid As String, attribute As ThumbnailProviderAttribute)
            ' set process isolation
            Using clsidKey As RegistryKey = Registry.ClassesRoot.OpenSubKey("CLSID")
                Using guidKey As RegistryKey = clsidKey.OpenSubKey(guid, True)
                    guidKey.SetValue("DisableProcessIsolation", If(attribute.DisableProcessIsolation, 1, 0), RegistryValueKind.DWord)

                    Using inproc As RegistryKey = guidKey.OpenSubKey("InprocServer32", True)
                        inproc.SetValue("ThreadingModel", "Apartment", RegistryValueKind.[String])
                    End Using
                End Using
            End Using

            ' register file as an approved extension
            Using approvedShellExtensions As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", True)
                approvedShellExtensions.SetValue(guid, attribute.Name, RegistryValueKind.[String])
            End Using

            ' register extension with each extension in the list
            Dim extensions As String() = attribute.Extensions.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)
            For Each extension As String In extensions
                Using extensionKey As RegistryKey = Registry.ClassesRoot.CreateSubKey(extension)
                    ' Create makes it writable
                    Using shellExKey As RegistryKey = extensionKey.CreateSubKey("shellex")
                        Using providerKey As RegistryKey = shellExKey.CreateSubKey(HandlerNativeMethods.IThumbnailProviderGuid.ToString("B"))
                            providerKey.SetValue(Nothing, guid, RegistryValueKind.[String])

                            If attribute.ThumbnailCutoff = ThumbnailCutoffSize.Square20 Then
                                extensionKey.DeleteValue("ThumbnailCutoff", False)
                            Else
                                extensionKey.SetValue("ThumbnailCutoff", CInt(attribute.ThumbnailCutoff), RegistryValueKind.DWord)
                            End If


                            If attribute.TypeOverlay IsNot Nothing Then
                                extensionKey.SetValue("TypeOverlay", attribute.TypeOverlay, RegistryValueKind.[String])
                            End If

                            If attribute.ThumbnailAdornment = ThumbnailAdornment.[Default] Then
                                extensionKey.DeleteValue("Treatment", False)
                            Else
                                extensionKey.SetValue("Treatment", CInt(attribute.ThumbnailAdornment), RegistryValueKind.DWord)
                            End If
                        End Using
                    End Using
                End Using
            Next
        End Sub


        ''' <summary>
        ''' Called when the assembly is registered via RegAsm.
        ''' </summary>
        ''' <param name="registerType">Type to register.</param>
        <ComUnregisterFunction>
        Private Shared Sub Unregister(registerType As Type)
            If registerType IsNot Nothing AndAlso registerType.IsSubclassOf(GetType(ThumbnailProvider)) Then
                Dim attributes As Object() = registerType.GetCustomAttributes(GetType(ThumbnailProviderAttribute), True)
                If attributes IsNot Nothing AndAlso attributes.Length = 1 Then
                    Dim attribute As ThumbnailProviderAttribute = TryCast(attributes(0), ThumbnailProviderAttribute)
                    UnregisterThumbnailHandler(registerType.GUID.ToString("B"), attribute)
                End If
            End If
        End Sub

        Private Shared Sub UnregisterThumbnailHandler(guid As String, attribute As ThumbnailProviderAttribute)
            Dim extensions As String() = attribute.Extensions.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)
            For Each extension As String In extensions
                Using extKey As RegistryKey = Registry.ClassesRoot.OpenSubKey(extension, True)
                    Using shellexKey As RegistryKey = extKey.OpenSubKey("shellex", True)
                        shellexKey.DeleteSubKey(HandlerNativeMethods.IThumbnailProviderGuid.ToString("B"), False)

                        extKey.DeleteValue("ThumbnailCutoff", False)
                        extKey.DeleteValue("TypeOverlay", False)
                        ' Thumbnail adornment
                        extKey.DeleteValue("Treatment", False)
                    End Using
                End Using
            Next

            Using approvedShellExtensions As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", True)
                approvedShellExtensions.DeleteValue(guid, False)
            End Using
        End Sub

        Private Shared Sub ThrowIfInvalid(type As Type, attribute As ThumbnailProviderAttribute)
            Dim interfaces = type.GetInterfaces()
            Dim interfaced As Boolean = interfaces.Any(Function(x) x Is GetType(IThumbnailFromStream))

            If interfaces.Any(Function(x) x Is GetType(IThumbnailFromShellObject) OrElse x Is GetType(IThumbnailFromFile)) Then
                ' According to MSDN (http://msdn.microsoft.com/en-us/library/cc144114(v=VS.85).aspx)
                ' A thumbnail provider that does not implement IInitializeWithStream must opt out of 
                ' running in the isolated process. The default behavior of the indexer opts in
                ' to process isolation regardless of which interfaces are implemented.                
                If Not interfaced AndAlso Not attribute.DisableProcessIsolation Then
                    Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.ThumbnailProviderDisabledProcessIsolation, type.Name))
                End If
                interfaced = True
            End If

            If Not interfaced Then
                Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.InvariantCulture, LocalizedMessages.ThumbnailProviderInterfaceNotImplemented, type.Name))
            End If
        End Sub

#End Region

#Region "IInitializeWithStream Members"

        Private Sub IInitializeWithStream_Initialize(stream As System.Runtime.InteropServices.ComTypes.IStream, fileMode As Shell.AccessModes) Implements IInitializeWithStream.Initialize
            _stream = New StorageStream(stream, fileMode <> Shell.AccessModes.ReadWrite)
        End Sub

#End Region

#Region "IInitializeWithItem Members"

        Private Sub IInitializeWithItem_Initialize(shellItem As Shell.IShellItem, accessMode As Shell.AccessModes) Implements IInitializeWithItem.Initialize
            _shellObject = ShellObjectFactory.Create(shellItem)
        End Sub

#End Region

#Region "IInitializeWithFile Members"

        Private Sub IInitializeWithFile_Initialize(filePath As String, fileMode As Shell.AccessModes) Implements IInitializeWithFile.Initialize
            _info = New FileInfo(filePath)
        End Sub

#End Region

#Region "IDisposable Members"

        ''' <summary>
        ''' Finalizer for the thumbnail provider.
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ''' <summary>
        ''' Disposes the thumbnail provider.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Disploses the thumbnail provider.
        ''' </summary>
        ''' <param name="disposing"></param>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposing AndAlso _stream IsNot Nothing Then
                _stream.Dispose()
            End If
        End Sub
#End Region

    End Class
End Namespace
