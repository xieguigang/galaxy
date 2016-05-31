'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Controls
Imports Microsoft.Windows.Controls.WindowsForms

Namespace Internal

    ''' <summary>
    ''' This provides a connection point container compatible dispatch interface for
    ''' hooking into the ExplorerBrowser view.
    ''' </summary>    
    <ComVisible(True)>
    <ClassInterface(ClassInterfaceType.AutoDual)>
    Public Class ExplorerBrowserViewEvents
        Implements IDisposable

#Region "implementation"
        Private viewConnectionPointCookie As UInteger
        Private viewDispatch As Object

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")>
        Private nullPtr As IntPtr = IntPtr.Zero

        Private IID_DShellFolderViewEvents As New Guid(ExplorerBrowserIIDGuid.DShellFolderViewEvents)
        Private IID_IDispatch As New Guid(ExplorerBrowserIIDGuid.IDispatch)
        Private parent As ExplorerBrowser
#End Region

#Region "contstruction"
        ''' <summary>
        ''' Default constructor for ExplorerBrowserViewEvents
        ''' </summary>
        Public Sub New()
            Me.New(Nothing)
        End Sub

        Friend Sub New(parent As ExplorerBrowser)
            Me.parent = parent
        End Sub
#End Region

#Region "operations"
        Friend Sub ConnectToView(psv As IShellView)
            DisconnectFromView()

            Dim hr As HResult = psv.GetItemObject(ShellViewGetItemObject.Background, IID_IDispatch, viewDispatch)

            If hr = HResult.Ok Then
                hr = ExplorerBrowserNativeMethods.ConnectToConnectionPoint(Me, IID_DShellFolderViewEvents, True, viewDispatch, viewConnectionPointCookie, nullPtr)

                If hr <> HResult.Ok Then
                    Marshal.ReleaseComObject(viewDispatch)
                End If
            End If
        End Sub

        Friend Sub DisconnectFromView()
            If viewDispatch IsNot Nothing Then
                ExplorerBrowserNativeMethods.ConnectToConnectionPoint(IntPtr.Zero, IID_DShellFolderViewEvents, False, viewDispatch, viewConnectionPointCookie, nullPtr)

                Marshal.ReleaseComObject(viewDispatch)
                viewDispatch = Nothing
                viewConnectionPointCookie = 0
            End If
        End Sub
#End Region

#Region "IDispatch events"
        ' These need to be public to be accessible via AutoDual reflection

        ''' <summary>
        ''' The view selection has changed
        ''' </summary>
        <DispId(ExplorerBrowserViewDispatchIds.SelectionChanged)>
        Public Sub ViewSelectionChanged()
            parent.FireSelectionChanged()
        End Sub

        ''' <summary>
        ''' The contents of the view have changed
        ''' </summary>
        <DispId(ExplorerBrowserViewDispatchIds.ContentsChanged)>
        Public Sub ViewContentsChanged()
            parent.FireContentChanged()
        End Sub

        ''' <summary>
        ''' The enumeration of files in the view is complete
        ''' </summary>
        <DispId(ExplorerBrowserViewDispatchIds.FileListEnumDone)>
        Public Sub ViewFileListEnumDone()
            parent.FireContentEnumerationComplete()
        End Sub

        ''' <summary>
        ''' The selected item in the view has changed (not the same as the selection has changed)
        ''' </summary>
        <DispId(ExplorerBrowserViewDispatchIds.SelectedItemChanged)>
        Public Sub ViewSelectedItemChanged()
            parent.FireSelectedItemChanged()
        End Sub
#End Region

        ''' <summary>
        ''' Finalizer for ExplorerBrowserViewEvents
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

#Region "IDisposable Members"

        ''' <summary>
        ''' Disconnects and disposes object.
        ''' </summary>        
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Disconnects and disposes object.
        ''' </summary>
        ''' <param name="disposed"></param>
        Protected Overridable Sub Dispose(disposed As Boolean)
            If disposed Then
                DisconnectFromView()
            End If
        End Sub

#End Region
    End Class
End Namespace
