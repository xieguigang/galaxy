Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualStudio.WinForms.Docking
Imports ThemeVS2012

Namespace ThemeVS2013

    <ToolboxItem(False)>
    Friend Class VS2013DockPaneCaption
        Inherits DockPaneCaptionBase
#Region "consts"
        Private Const TextGapTop As Integer = 3
        Private Const TextGapBottom As Integer = 2
        Private Const TextGapLeft As Integer = 2
        Private Const TextGapRight As Integer = 3
        Private Const ButtonGapTop As Integer = 4
        Private Const ButtonGapBottom As Integer = 3
        Private Const ButtonGapBetween As Integer = 1
        Private Const ButtonGapLeft As Integer = 1
        Private Const ButtonGapRight As Integer = 5
#End Region

        Private m_buttonClose As InertButtonBase
        Private ReadOnly Property ButtonClose As InertButtonBase
            Get
                If m_buttonClose Is Nothing Then
                    m_buttonClose = New VS2012DockPaneCaptionInertButton(Me, DockPane.DockPanel.Theme.ImageService.DockPaneHover_Close, DockPane.DockPanel.Theme.ImageService.DockPane_Close, DockPane.DockPanel.Theme.ImageService.DockPanePress_Close, DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Close, DockPane.DockPanel.Theme.ImageService.DockPaneActive_Close)
                    m_toolTip.SetToolTip(m_buttonClose, ToolTipClose)
                    AddHandler m_buttonClose.Click, New EventHandler(AddressOf Close_Click)
                    Controls.Add(m_buttonClose)
                End If

                Return m_buttonClose
            End Get
        End Property

        Private m_buttonAutoHide As InertButtonBase
        Private ReadOnly Property ButtonAutoHide As InertButtonBase
            Get
                If m_buttonAutoHide Is Nothing Then
                    m_buttonAutoHide = New VS2012DockPaneCaptionInertButton(Me, DockPane.DockPanel.Theme.ImageService.DockPaneHover_Dock, DockPane.DockPanel.Theme.ImageService.DockPane_Dock, DockPane.DockPanel.Theme.ImageService.DockPanePress_Dock, DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Dock, DockPane.DockPanel.Theme.ImageService.DockPaneActive_Dock, DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_AutoHide, DockPane.DockPanel.Theme.ImageService.DockPaneActive_AutoHide, DockPane.DockPanel.Theme.ImageService.DockPanePress_AutoHide)
                    m_toolTip.SetToolTip(m_buttonAutoHide, ToolTipAutoHide)
                    AddHandler m_buttonAutoHide.Click, New EventHandler(AddressOf AutoHide_Click)
                    Controls.Add(m_buttonAutoHide)
                End If

                Return m_buttonAutoHide
            End Get
        End Property

        Private m_buttonOptions As InertButtonBase
        Private ReadOnly Property ButtonOptions As InertButtonBase
            Get
                If m_buttonOptions Is Nothing Then
                    m_buttonOptions = New VS2012DockPaneCaptionInertButton(Me, DockPane.DockPanel.Theme.ImageService.DockPaneHover_Option, DockPane.DockPanel.Theme.ImageService.DockPane_Option, DockPane.DockPanel.Theme.ImageService.DockPanePress_Option, DockPane.DockPanel.Theme.ImageService.DockPaneActiveHover_Option, DockPane.DockPanel.Theme.ImageService.DockPaneActive_Option)
                    m_toolTip.SetToolTip(m_buttonOptions, ToolTipOptions)
                    AddHandler m_buttonOptions.Click, New EventHandler(AddressOf Options_Click)
                    Controls.Add(m_buttonOptions)
                End If
                Return m_buttonOptions
            End Get
        End Property

        Private m_components As IContainer
        Private ReadOnly Property Components As IContainer
            Get
                Return m_components
            End Get
        End Property

        Private m_toolTip As ToolTip

        Public Sub New(pane As DockPane)
            MyBase.New(pane)
            SuspendLayout()

            m_components = New Container()
            m_toolTip = New ToolTip(Components)

            ResumeLayout()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then Components.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Public ReadOnly Property TextFont As Font
            Get
                Return DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont
            End Get
        End Property

        Private Shared _toolTipClose As String
        Private Shared ReadOnly Property ToolTipClose As String
            Get
                If Equals(_toolTipClose, Nothing) Then _toolTipClose = My.Resources.DockPaneCaption_ToolTipClose
                Return _toolTipClose
            End Get
        End Property

        Private Shared _toolTipOptions As String
        Private Shared ReadOnly Property ToolTipOptions As String
            Get
                If Equals(_toolTipOptions, Nothing) Then _toolTipOptions = My.Resources.DockPaneCaption_ToolTipOptions

                Return _toolTipOptions
            End Get
        End Property

        Private Shared _toolTipAutoHide As String
        Private Shared ReadOnly Property ToolTipAutoHide As String
            Get
                If Equals(_toolTipAutoHide, Nothing) Then _toolTipAutoHide = My.Resources.DockPaneCaption_ToolTipAutoHide
                Return _toolTipAutoHide
            End Get
        End Property

        Private ReadOnly Property TextColor As Color
            Get
                If DockPane.IsActivePane Then
                    Return DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Text
                Else
                    Return DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive.Text
                End If
            End Get
        End Property

        Private Shared _textFormat As TextFormatFlags = TextFormatFlags.SingleLine Or TextFormatFlags.EndEllipsis Or TextFormatFlags.VerticalCenter
        Private ReadOnly Property TextFormat As TextFormatFlags
            Get
                If MyBase.RightToLeft = RightToLeft.No Then
                    Return _textFormat
                Else
                    Return _textFormat Or TextFormatFlags.RightToLeft Or TextFormatFlags.Right
                End If
            End Get
        End Property

        Protected Overrides Function MeasureHeight() As Integer
            Dim height = TextFont.Height + TextGapTop + TextGapBottom

            If height < ButtonClose.Image.Height + ButtonGapTop + ButtonGapBottom Then height = ButtonClose.Image.Height + ButtonGapTop + ButtonGapBottom

            Return height
        End Function

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            DrawCaption(e.Graphics)
        End Sub

        Private Sub DrawCaption(g As Graphics)
            If ClientRectangle.Width = 0 OrElse ClientRectangle.Height = 0 Then Return

            Dim rect = ClientRectangle
            Dim border = DockPane.DockPanel.Theme.ColorPalette.ToolWindowBorder
            Dim palette As ToolWindowCaptionPalette
            If DockPane.IsActivePane Then
                palette = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionActive
            Else
                palette = DockPane.DockPanel.Theme.ColorPalette.ToolWindowCaptionInactive
            End If

            Dim captionBrush = DockPane.DockPanel.Theme.PaintingService.GetBrush(palette.Background)
            g.FillRectangle(captionBrush, rect)

            g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(border), rect.Left, rect.Top, rect.Left, rect.Bottom)
            g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(border), rect.Left, rect.Top, rect.Right, rect.Top)
            g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(border), rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom)

            Dim rectCaption = rect

            Dim rectCaptionText = rectCaption
            rectCaptionText.X += TextGapLeft
            rectCaptionText.Width -= TextGapLeft + TextGapRight
            rectCaptionText.Width -= ButtonGapLeft + ButtonClose.Width + ButtonGapRight
            If ShouldShowAutoHideButton Then rectCaptionText.Width -= ButtonAutoHide.Width + ButtonGapBetween
            If HasTabPageContextMenu Then rectCaptionText.Width -= ButtonOptions.Width + ButtonGapBetween
            rectCaptionText.Y += TextGapTop
            rectCaptionText.Height -= TextGapTop + TextGapBottom

            TextRenderer.DrawText(g, DockPane.CaptionText, TextFont, RtlTransform(Me, rectCaptionText), palette.Text, TextFormat)

            Dim rectDotsStrip = rectCaptionText
            Dim textLength = CInt(g.MeasureString(DockPane.CaptionText, TextFont).Width) + TextGapLeft
            rectDotsStrip.X += textLength
            rectDotsStrip.Width -= textLength
            rectDotsStrip.Height = ClientRectangle.Height

            DrawDotsStrip(g, rectDotsStrip, palette.Grip)
        End Sub

        Protected Sub DrawDotsStrip(g As Graphics, rectStrip As Rectangle, colorDots As Color)
            If rectStrip.Width <= 0 OrElse rectStrip.Height <= 0 Then Return

            Dim penDots = DockPane.DockPanel.Theme.PaintingService.GetPen(colorDots, 1)
            penDots.DashStyle = DashStyle.Custom
            penDots.DashPattern = New Single() {1, 3}
            Dim positionY As Integer = rectStrip.Height / 2

            g.DrawLine(penDots, rectStrip.X + 2, positionY, rectStrip.X + rectStrip.Width - 2, positionY)

            g.DrawLine(penDots, rectStrip.X, positionY - 2, rectStrip.X + rectStrip.Width, positionY - 2)
            g.DrawLine(penDots, rectStrip.X, positionY + 2, rectStrip.X + rectStrip.Width, positionY + 2)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            SetButtonsPosition()
            MyBase.OnLayout(levent)
        End Sub

        Protected Overrides Sub OnRefreshChanges()
            SetButtons()
            Invalidate()
        End Sub

        Private ReadOnly Property CloseButtonEnabled As Boolean
            Get
                Return If(DockPane.ActiveContent IsNot Nothing, DockPane.ActiveContent.DockHandler.CloseButton, False)
            End Get
        End Property

        ''' <summary>
        ''' Determines whether the close button is visible on the content
        ''' </summary>
        Private ReadOnly Property CloseButtonVisible As Boolean
            Get
                Return If(DockPane.ActiveContent IsNot Nothing, DockPane.ActiveContent.DockHandler.CloseButtonVisible, False)
            End Get
        End Property

        Private ReadOnly Property ShouldShowAutoHideButton As Boolean
            Get
                Return Not DockPane.IsFloat
            End Get
        End Property

        Private Sub SetButtons()
            ButtonClose.Enabled = CloseButtonEnabled
            ButtonClose.Visible = CloseButtonVisible
            ButtonAutoHide.Visible = ShouldShowAutoHideButton
            ButtonOptions.Visible = HasTabPageContextMenu
            ButtonClose.RefreshChanges()
            ButtonAutoHide.RefreshChanges()
            ButtonOptions.RefreshChanges()

            SetButtonsPosition()
        End Sub

        Private Sub SetButtonsPosition()
            ' set the size and location for close and auto-hide buttons
            Dim rectCaption = ClientRectangle
            Dim buttonWidth = ButtonClose.Image.Width
            Dim buttonHeight = ButtonClose.Image.Height

            Dim buttonSize As Size = New Size(buttonWidth, buttonHeight)
            Dim x = rectCaption.X + rectCaption.Width - ButtonGapRight - m_buttonClose.Width
            Dim y = rectCaption.Y + ButtonGapTop
            Dim point As Point = New Point(x, y)
            ButtonClose.Bounds = RtlTransform(Me, New Rectangle(point, buttonSize))

            ' If the close button is not visible draw the auto hide button overtop.
            ' Otherwise it is drawn to the left of the close button.
            If CloseButtonVisible Then point.Offset(-(buttonWidth + ButtonGapBetween), 0)

            ButtonAutoHide.Bounds = RtlTransform(Me, New Rectangle(point, buttonSize))
            If ShouldShowAutoHideButton Then point.Offset(-(buttonWidth + ButtonGapBetween), 0)
            ButtonOptions.Bounds = RtlTransform(Me, New Rectangle(point, buttonSize))
        End Sub

        Private Sub Close_Click(sender As Object, e As EventArgs)
            DockPane.CloseActiveContent()
        End Sub

        Private Sub AutoHide_Click(sender As Object, e As EventArgs)
            DockPane.DockState = ToggleAutoHideState(DockPane.DockState)
            If IsDockStateAutoHide(DockPane.DockState) Then
                DockPane.DockPanel.ActiveAutoHideContent = Nothing
                DockPane.NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(DockPane)
            End If
        End Sub

        Private Sub Options_Click(sender As Object, e As EventArgs)
            ShowTabPageContextMenu(PointToClient(MousePosition))
        End Sub

        Protected Overrides Sub OnRightToLeftChanged(e As EventArgs)
            MyBase.OnRightToLeftChanged(e)
            PerformLayout()
        End Sub

        Protected Overrides ReadOnly Property CanDragAutoHide As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace
