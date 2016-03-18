'Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices
Imports Microsoft.Windows.Resources
Imports Microsoft.Windows.Internal
Imports Microsoft.Windows.Controls.WindowsForms

Namespace Controls
    ''' <summary>
    ''' These options control how the content of the Explorer Browser 
    ''' is rendered.
    ''' </summary>
    Public Class ExplorerBrowserContentOptions
#Region "construction"
        Private eb As ExplorerBrowser
        Friend Sub New(eb As ExplorerBrowser)
            Me.eb = eb
        End Sub
#End Region

#Region "ViewMode property"
        ' This is a one-way property of the explorer browser. 
        ' Keeping it around for the get implementations.
        Friend folderSettings As New FolderSettings()

        ''' <summary>
        ''' The viewing mode of the Explorer Browser
        ''' </summary>
        Public Property ViewMode() As ExplorerBrowserViewMode
            Get
                Return CType(folderSettings.ViewMode, ExplorerBrowserViewMode)
            End Get
            Set
                folderSettings.ViewMode = CType(Value, FolderViewMode)

                If eb.explorerBrowserControl IsNot Nothing Then
                    eb.explorerBrowserControl.SetFolderSettings(folderSettings)
                End If
            End Set
        End Property
#End Region

#Region "Flags property"
        ''' <summary>
        ''' The binary representation of the ExplorerBrowser content flags
        ''' </summary>
        Public Property Flags() As ExplorerBrowserContentSectionOptions
            Get
                Return CType(folderSettings.Options, ExplorerBrowserContentSectionOptions)
            End Get
            Set
                folderSettings.Options = CType(Value, FolderOptions) Or FolderOptions.UseSearchFolders Or FolderOptions.NoWebView
                If eb.explorerBrowserControl IsNot Nothing Then
                    eb.explorerBrowserControl.SetFolderSettings(folderSettings)
                End If
            End Set
        End Property
#End Region

#Region "content flags to properties mapping"
        ''' <summary>
        ''' The view should be left-aligned. 
        ''' </summary>
        Public Property AlignLeft() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.AlignLeft)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.AlignLeft, Value)
            End Set
        End Property
        ''' <summary>
        ''' Automatically arrange the elements in the view. 
        ''' </summary>
        Public Property AutoArrange() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.AutoArrange)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.AutoArrange, Value)
            End Set
        End Property
        ''' <summary>
        ''' Turns on check mode for the view
        ''' </summary>
        Public Property CheckSelect() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.CheckSelect)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.CheckSelect, Value)
            End Set
        End Property
        ''' <summary>
        ''' When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.
        ''' </summary>
        Public Property ExtendedTiles() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.ExtendedTiles)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.ExtendedTiles, Value)
            End Set
        End Property
        ''' <summary>
        ''' When an item is selected, the item and all its sub-items are highlighted.
        ''' </summary>
        Public Property FullRowSelect() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.FullRowSelect)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.FullRowSelect, Value)
            End Set
        End Property
        ''' <summary>
        ''' The view should not display file names
        ''' </summary>
        Public Property HideFileNames() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.HideFileNames)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.HideFileNames, Value)
            End Set
        End Property
        ''' <summary>
        ''' The view should not save view state in the browser.
        ''' </summary>
        Public Property NoBrowserViewState() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.NoBrowserViewState)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.NoBrowserViewState, Value)
            End Set
        End Property
        ''' <summary>
        ''' Do not display a column header in the view in any view mode.
        ''' </summary>
        Public Property NoColumnHeader() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.NoColumnHeader)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.NoColumnHeader, Value)
            End Set
        End Property
        ''' <summary>
        ''' Only show the column header in details view mode.
        ''' </summary>
        Public Property NoHeaderInAllViews() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.NoHeaderInAllViews)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.NoHeaderInAllViews, Value)
            End Set
        End Property
        ''' <summary>
        ''' The view should not display icons. 
        ''' </summary>
        Public Property NoIcons() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.NoIcons)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.NoIcons, Value)
            End Set
        End Property
        ''' <summary>
        ''' Do not show subfolders. 
        ''' </summary>
        Public Property NoSubfolders() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.NoSubfolders)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.NoSubfolders, Value)
            End Set
        End Property
        ''' <summary>
        ''' Navigate with a single click
        ''' </summary>
        Public Property SingleClickActivate() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.SingleClickActivate)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.SingleClickActivate, Value)
            End Set
        End Property
        ''' <summary>
        ''' Do not allow more than a single item to be selected.
        ''' </summary>
        Public Property SingleSelection() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserContentSectionOptions.SingleSelection)
            End Get
            Set
                SetFlag(ExplorerBrowserContentSectionOptions.SingleSelection, Value)
            End Set
        End Property

        Private Function IsFlagSet(flag As ExplorerBrowserContentSectionOptions) As Boolean
            Return (folderSettings.Options And CType(flag, FolderOptions)) <> 0
        End Function

        Private Sub SetFlag(flag As ExplorerBrowserContentSectionOptions, value As Boolean)
            If value Then
                folderSettings.Options = folderSettings.Options Or CType(flag, FolderOptions)
            Else
                folderSettings.Options = folderSettings.Options And Not CType(flag, FolderOptions)
            End If

            If eb.explorerBrowserControl IsNot Nothing Then
                eb.explorerBrowserControl.SetFolderSettings(folderSettings)
            End If
        End Sub

#End Region

#Region "thumbnail size"
        ''' <summary>
        ''' The size of the thumbnails in pixels
        ''' </summary>
        Public Property ThumbnailSize() As Integer
            Get
                Dim iconSize As Integer = 0
                Dim iFV2 As IFolderView2 = eb.GetFolderView2()
                If iFV2 IsNot Nothing Then
                    Try
                        Dim fvm As Integer = 0
                        Dim hr As HResult = iFV2.GetViewModeAndIconSize(fvm, iconSize)
                        If hr <> HResult.Ok Then
                            Throw New CommonControlException(LocalizedMessages.ExplorerBrowserIconSize, hr)
                        End If
                    Finally
                        Marshal.ReleaseComObject(iFV2)
                        iFV2 = Nothing
                    End Try
                End If

                Return iconSize
            End Get
            Set
                Dim iFV2 As IFolderView2 = eb.GetFolderView2()
                If iFV2 IsNot Nothing Then
                    Try
                        Dim fvm As Integer = 0
                        Dim iconSize As Integer = 0
                        Dim hr As HResult = iFV2.GetViewModeAndIconSize(fvm, iconSize)
                        If hr <> HResult.Ok Then
                            Throw New CommonControlException(LocalizedMessages.ExplorerBrowserIconSize, hr)
                        End If
                        hr = iFV2.SetViewModeAndIconSize(fvm, Value)
                        If hr <> HResult.Ok Then
                            Throw New CommonControlException(LocalizedMessages.ExplorerBrowserIconSize, hr)
                        End If
                    Finally
                        Marshal.ReleaseComObject(iFV2)
                        iFV2 = Nothing
                    End Try
                End If
            End Set
        End Property
#End Region
    End Class

End Namespace
