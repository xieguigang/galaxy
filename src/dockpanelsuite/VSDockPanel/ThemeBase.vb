Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Language

Namespace WeifenLuo.WinFormsUI.Docking
    Public MustInherit Class ThemeBase
        Inherits Component






        Private _Skin As WeifenLuo.WinFormsUI.Docking.DockPanelSkin, _ColorPalette As WeifenLuo.WinFormsUI.Docking.DockPanelColorPalette, _ImageService As WeifenLuo.WinFormsUI.Docking.IImageService, _PaintingService As WeifenLuo.WinFormsUI.Docking.IPaintingService
        Dim _Extender As WeifenLuo.WinFormsUI.Docking.DockPanelExtender
        Private _dockBackColor As Color
        Private _showAutoHideContentOnHover As Boolean = True

        Protected Sub New()
            Extender = New DockPanelExtender()
        End Sub

        Public Property Skin As DockPanelSkin
            Get
                Return _Skin
            End Get
            Protected Set(value As DockPanelSkin)
                _Skin = value
            End Set
        End Property

        Public Property ColorPalette As DockPanelColorPalette
            Get
                Return _ColorPalette
            End Get
            Protected Set(value As DockPanelColorPalette)
                _ColorPalette = value
            End Set
        End Property

        Public Property ImageService As IImageService
            Get
                Return _ImageService
            End Get
            Protected Set(value As IImageService)
                _ImageService = value
            End Set
        End Property

        Public Property PaintingService As IPaintingService
            Get
                Return _PaintingService
            End Get
            Protected Set(value As IPaintingService)
                _PaintingService = value
            End Set
        End Property

        Protected Property ToolStripRenderer As ToolStripRenderer

        Private _stripBefore As Dictionary(Of ToolStrip, KeyValuePair(Of ToolStripRenderMode, ToolStripRenderer)) = New Dictionary(Of ToolStrip, KeyValuePair(Of ToolStripRenderMode, ToolStripRenderer))()

        Public Sub ApplyTo(toolStrip As ToolStrip)
            If toolStrip Is Nothing Then Return

            _stripBefore(toolStrip) = New KeyValuePair(Of ToolStripRenderMode, ToolStripRenderer)(toolStrip.RenderMode, toolStrip.Renderer)
            If ToolStripRenderer IsNot Nothing Then toolStrip.Renderer = ToolStripRenderer

            If IsRunningOnMono Then
                For Each item In toolStrip.Items.OfType(Of ToolStripDropDownItem)()
                    ItemResetOwnerHack(item)
                Next
            End If
        End Sub

        Private Sub ItemResetOwnerHack(item As ToolStripDropDownItem)
            Dim oldOwner = item.DropDown.OwnerItem
            item.DropDown.OwnerItem = Nothing
            item.DropDown.OwnerItem = oldOwner

            For Each child In item.DropDownItems.OfType(Of ToolStripDropDownItem)()
                ItemResetOwnerHack(child)
            Next
        End Sub

        Private _managerBefore As KeyValuePair(Of ToolStripManagerRenderMode, ToolStripRenderer)

        Public Sub ApplyToToolStripManager()
            _managerBefore = New KeyValuePair(Of ToolStripManagerRenderMode, ToolStripRenderer)(ToolStripManager.RenderMode, ToolStripManager.Renderer)
        End Sub

        Public ReadOnly Property Measures As Measures = New Measures()

        Public Property ShowAutoHideContentOnHover As Boolean
            Get
                Return _showAutoHideContentOnHover
            End Get
            Protected Set(value As Boolean)
                _showAutoHideContentOnHover = value
            End Set
        End Property

        Public Sub ApplyTo(dockPanel As DockPanel)
            If Extender.AutoHideStripFactory Is Nothing OrElse Extender.AutoHideWindowFactory Is Nothing OrElse Extender.DockIndicatorFactory Is Nothing OrElse Extender.DockOutlineFactory Is Nothing OrElse Extender.DockPaneCaptionFactory Is Nothing OrElse Extender.DockPaneFactory Is Nothing OrElse Extender.DockPaneSplitterControlFactory Is Nothing OrElse Extender.DockPaneStripFactory Is Nothing OrElse Extender.DockWindowFactory Is Nothing OrElse Extender.FloatWindowFactory Is Nothing OrElse Extender.PaneIndicatorFactory Is Nothing OrElse Extender.PanelIndicatorFactory Is Nothing OrElse Extender.WindowSplitterControlFactory Is Nothing Then
                Throw New InvalidOperationException(Strings.Theme_MissingFactory)
            End If

            If dockPanel.Panes.Count > 0 Then Throw New InvalidOperationException(Strings.Theme_PaneNotClosed)

            If dockPanel.FloatWindows.Count > 0 Then Throw New InvalidOperationException(Strings.Theme_FloatWindowNotClosed)

            If dockPanel.Contents.Count > 0 Then Throw New InvalidOperationException(Strings.Theme_DockContentNotClosed)

            If ColorPalette Is Nothing Then
                dockPanel.ResetDummy()
            Else
                _dockBackColor = dockPanel.DockBackColor
                dockPanel.DockBackColor = ColorPalette.MainWindowActive.Background
                dockPanel.SetDummy()
            End If

            _showAutoHideContentOnHover = dockPanel.ShowAutoHideContentOnHover
            dockPanel.ShowAutoHideContentOnHover = ShowAutoHideContentOnHover
        End Sub

        Friend Sub PostApply(dockPanel As DockPanel)
            dockPanel.ResetAutoHideStripControl()
            dockPanel.ResetAutoHideStripWindow()
            dockPanel.ResetDockWindows()
        End Sub

        Public Overridable Sub CleanUp(dockPanel As DockPanel)
            If dockPanel IsNot Nothing Then
                If ColorPalette IsNot Nothing Then
                    dockPanel.DockBackColor = _dockBackColor
                End If

                dockPanel.ShowAutoHideContentOnHover = _showAutoHideContentOnHover
            End If

            For Each item In _stripBefore
                Dim strip = item.Key
                Dim cache = item.Value
                If cache.Key = ToolStripRenderMode.Custom Then
                    If cache.Value IsNot Nothing Then strip.Renderer = cache.Value
                Else
                    strip.RenderMode = cache.Key
                End If
            Next

            _stripBefore.Clear()
            If _managerBefore.Key = ToolStripManagerRenderMode.Custom Then
                If _managerBefore.Value IsNot Nothing Then ToolStripManager.Renderer = _managerBefore.Value
            Else
                ToolStripManager.RenderMode = _managerBefore.Key
            End If
        End Sub

        Public Property Extender As DockPanelExtender
            Get
                Return _Extender
            End Get
            Private Set(value As DockPanelExtender)
                _Extender = value
            End Set
        End Property

        Public Shared Function Decompress(fileToDecompress As Byte()) As Byte()
            Using originalFileStream As MemoryStream = New MemoryStream(fileToDecompress)
                Using decompressedFileStream As MemoryStream = New MemoryStream()
                    Using decompressionStream As GZipStream = New GZipStream(originalFileStream, CompressionMode.Decompress)
                        'Copy the decompression stream into the output file.
                        Dim buffer = New Byte(4095) {}
                        Dim numRead As Value(Of Integer) = 0
                        While (numRead = decompressionStream.Read(buffer, 0, buffer.Length)) <> 0
                            decompressedFileStream.Write(buffer, 0, CInt(numRead))
                        End While

                        Return decompressedFileStream.ToArray()
                    End Using
                End Using
            End Using
        End Function
    End Class
End Namespace
