Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports System.Collections.Generic

' To simplify the process of finding the toolbox bitmap resource:
' #1 Create an internal class called "resfinder" outside of the root namespace.
' #2 Use "resfinder" in the toolbox bitmap attribute instead of the control name.
' #3 use the "<default namespace>.<resourcename>" string to locate the resource.
' See: http://www.bobpowell.net/toolboxbitmap.htm
Friend Class resfinder
End Class

Namespace Docking
    ''' <summary>
    ''' Deserialization handler of layout file/stream.
    ''' </summary>
    ''' <param name="persistString">Strings stored in layout file/stream.</param>
    ''' <returns>Dock content deserialized from layout/stream.</returns>
    ''' <remarks>
    ''' The deserialization handler method should handle all possible exceptions.
    ''' 
    ''' If any exception happens during deserialization and is not handled, the program might crash or experience other issues.
    ''' </remarks>
    <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId:="0#")>
    Public Delegate Function DeserializeDockContent(persistString As String) As IDockContent

    <LocalizedDescription("DockPanel_Description")>
    <Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")>
    <ToolboxBitmap(GetType(resfinder), "WeifenLuo.WinFormsUI.Docking.DockPanel.bmp")>
    <DefaultProperty("DocumentStyle")>
    <DefaultEvent("ActiveContentChanged")>
    Public Partial Class DockPanel
        Inherits Panel
        Private ReadOnly m_focusManager As FocusManagerImpl
        Private ReadOnly m_panes As DockPaneCollection
        Private ReadOnly m_floatWindows As FloatWindowCollection
        Private m_autoHideWindow As AutoHideWindowControl
        Private m_dockWindows As DockWindowCollection
        Private ReadOnly m_dummyContent As DockContent
        Private ReadOnly m_dummyControl As Control

        Public Sub New()
            ShowAutoHideContentOnHover = True

            m_focusManager = New FocusManagerImpl(Me)
            m_panes = New DockPaneCollection()
            m_floatWindows = New FloatWindowCollection()

            MyBase.SuspendLayout()

            m_dummyControl = New DummyControl()
            m_dummyControl.Bounds = New Rectangle(0, 0, 1, 1)
            Controls.Add(m_dummyControl)

            Theme.ApplyTo(Me)

            m_autoHideWindow = Theme.Extender.AutoHideWindowFactory.CreateAutoHideWindow(Me)
            m_autoHideWindow.Visible = False
            AddHandler m_autoHideWindow.ActiveContentChanged, AddressOf m_autoHideWindow_ActiveContentChanged
            SetAutoHideWindowParent()

            LoadDockWindows()

            m_dummyContent = New DockContent()
            MyBase.ResumeLayout()
        End Sub

        Friend Sub ResetDummy()
            DummyControl.ResetBackColor()
        End Sub

        Friend Sub SetDummy()
            DummyControl.BackColor = DockBackColor
        End Sub

        Private m_BackColor As Color

        ''' <summary>
        ''' Determines the color with which the client rectangle will be drawn.
        ''' If this property is used instead of the BackColor it will not have any influence on the borders to the surrounding controls (DockPane).
        ''' The BackColor property changes the borders of surrounding controls (DockPane).
        ''' Alternatively both properties may be used (BackColor to draw and define the color of the borders and DockBackColor to define the color of the client rectangle). 
        ''' For Backgroundimages: Set your prefered Image, then set the DockBackColor and the BackColor to the same Color (Control)
        ''' </summary>
        <ComponentModel.DescriptionAttribute("Determines the color with which the client rectangle will be drawn." & Microsoft.VisualBasic.Constants.vbCrLf & "If this property is used instead of the BackColor it will not have any influence on the borders to the surrounding controls (DockPane)." & Microsoft.VisualBasic.Constants.vbCrLf & "The BackColor property changes the borders of surrounding controls (DockPane)." & Microsoft.VisualBasic.Constants.vbCrLf & "Alternatively both properties may be used (BackColor to draw and define the color of the borders and DockBackColor to define the color of the client rectangle)." & Microsoft.VisualBasic.Constants.vbCrLf & "For Backgroundimages: Set your prefered Image, then set the DockBackColor and the BackColor to the same Color (Control).")>
        Public Property DockBackColor As Color
            Get
                Return If(Not m_BackColor.IsEmpty, m_BackColor, MyBase.BackColor)
            End Get

            Set(value As Color)
                If m_BackColor <> value Then
                    m_BackColor = value
                    MyBase.Refresh()
                End If
            End Set
        End Property

        Private Function ShouldSerializeDockBackColor() As Boolean
            Return Not m_BackColor.IsEmpty
        End Function

        Private m_autoHideStripControl As AutoHideStripBase

        Friend ReadOnly Property AutoHideStripControl As AutoHideStripBase
            Get
                If m_autoHideStripControl Is Nothing Then
                    m_autoHideStripControl = Theme.Extender.AutoHideStripFactory.CreateAutoHideStrip(Me)
                    Controls.Add(m_autoHideStripControl)
                End If

                Return m_autoHideStripControl
            End Get
        End Property

        Friend Sub ResetAutoHideStripControl()
            If m_autoHideStripControl IsNot Nothing Then m_autoHideStripControl.Dispose()

            m_autoHideStripControl = Nothing
        End Sub

        Private Sub MdiClientHandleAssigned(sender As Object, e As EventArgs)
            SetMdiClient()
            PerformLayout()
        End Sub

        Private Sub MdiClient_Layout(sender As Object, e As LayoutEventArgs)
            If DocumentStyle <> DocumentStyle.DockingMdi Then Return

            For Each pane In Panes
                If pane.DockState = DockState.Document Then pane.SetContentBounds()
            Next

            InvalidateWindowRegion()
        End Sub

        Private m_disposed As Boolean

        Protected Overrides Sub Dispose(disposing As Boolean)
            If Not m_disposed AndAlso disposing Then
                m_focusManager.Dispose()
                If m_mdiClientController IsNot Nothing Then
                    RemoveHandler m_mdiClientController.HandleAssigned, New EventHandler(AddressOf MdiClientHandleAssigned)
                    RemoveHandler m_mdiClientController.MdiChildActivate, New EventHandler(AddressOf ParentFormMdiChildActivate)
                    RemoveHandler m_mdiClientController.Layout, New LayoutEventHandler(AddressOf MdiClient_Layout)
                    m_mdiClientController.Dispose()
                End If
                FloatWindows.Dispose()
                Panes.Dispose()
                DummyContent.Dispose()

                m_disposed = True
            End If

            MyBase.Dispose(disposing)
        End Sub

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ActiveAutoHideContent As IDockContent
            Get
                Return AutoHideWindow.ActiveContent
            End Get
            Set(value As IDockContent)
                AutoHideWindow.ActiveContent = value
            End Set
        End Property

        Private m_allowEndUserDocking As Boolean = Not IsRunningOnMono
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_AllowEndUserDocking_Description")>
        <DefaultValue(True)>
        Public Property AllowEndUserDocking As Boolean
            Get
                If IsRunningOnMono AndAlso m_allowEndUserDocking Then m_allowEndUserDocking = False

                Return m_allowEndUserDocking
            End Get

            Set(value As Boolean)
                If IsRunningOnMono AndAlso value Then Throw New InvalidOperationException("AllowEndUserDocking can only be false if running on Mono")

                m_allowEndUserDocking = value
            End Set
        End Property

        Private m_allowEndUserNestedDocking As Boolean = Not IsRunningOnMono
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_AllowEndUserNestedDocking_Description")>
        <DefaultValue(True)>
        Public Property AllowEndUserNestedDocking As Boolean
            Get
                If IsRunningOnMono AndAlso m_allowEndUserDocking Then m_allowEndUserDocking = False
                Return m_allowEndUserNestedDocking
            End Get

            Set(value As Boolean)
                If IsRunningOnMono AndAlso value Then Throw New InvalidOperationException("AllowEndUserNestedDocking can only be false if running on Mono")

                m_allowEndUserNestedDocking = value
            End Set
        End Property

        Private m_contents As DockContentCollection = New DockContentCollection()
        <Browsable(False)>
        Public ReadOnly Property Contents As DockContentCollection
            Get
                Return m_contents
            End Get
        End Property

        Friend ReadOnly Property DummyContent As DockContent
            Get
                Return m_dummyContent
            End Get
        End Property

        Private m_rightToLeftLayout As Boolean = False
        <DefaultValue(False)>
        <LocalizedCategory("Appearance")>
        <LocalizedDescription("DockPanel_RightToLeftLayout_Description")>
        Public Property RightToLeftLayout As Boolean
            Get
                Return m_rightToLeftLayout
            End Get

            Set(value As Boolean)
                If m_rightToLeftLayout = value Then Return

                m_rightToLeftLayout = value
                For Each floatWindow In FloatWindows
                    floatWindow.RightToLeftLayout = value
                Next
            End Set
        End Property

        Protected Overrides Sub OnRightToLeftChanged(e As EventArgs)
            MyBase.OnRightToLeftChanged(e)
            For Each floatWindow In FloatWindows
                floatWindow.RightToLeft = MyBase.RightToLeft
            Next
        End Sub

        Private m_showDocumentIcon As Boolean = False
        <DefaultValue(False)>
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_ShowDocumentIcon_Description")>
        Public Property ShowDocumentIcon As Boolean
            Get
                Return m_showDocumentIcon
            End Get
            Set(value As Boolean)
                If m_showDocumentIcon = value Then Return

                m_showDocumentIcon = value
                MyBase.Refresh()
            End Set
        End Property

        <DefaultValue(DocumentTabStripLocation.Top)>
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DocumentTabStripLocation")>
        Public Property DocumentTabStripLocation As DocumentTabStripLocation = DocumentTabStripLocation.Top

        <Browsable(False)>
        <Obsolete("Use Theme.Extender instead.")>
        Public ReadOnly Property Extender As DockPanelExtender
            Get
                Return Nothing
            End Get
        End Property

        <Browsable(False)>
        <Obsolete("Use Theme.Extender instead.")>
        Public ReadOnly Property DockPaneFactory As DockPanelExtender.IDockPaneFactory
            Get
                Return Nothing
            End Get
        End Property

        <Browsable(False)>
        <Obsolete("Use Theme.Extender instead.")>
        Public ReadOnly Property FloatWindowFactory As DockPanelExtender.IFloatWindowFactory
            Get
                Return Nothing
            End Get
        End Property

        <Browsable(False)>
        <Obsolete("Use Theme.Extender instead.")>
        Public ReadOnly Property DockWindowFactory As DockPanelExtender.IDockWindowFactory
            Get
                Return Nothing
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property Panes As DockPaneCollection
            Get
                Return m_panes
            End Get
        End Property

        ''' <summary>
        ''' Dock area.
        ''' </summary>
        ''' <remarks>
        ''' This <see cref="Rectangle"/> is the center rectangle of <see cref="DockPanel"/> control.
        ''' 
        ''' Excluded spaces are for the following visual elements,
        ''' * Auto hide strips on four sides.
        ''' * Necessary paddings defined in themes.
        ''' 
        ''' Therefore, all dock contents mainly fall into this area (except auto hide window, which might slightly move beyond this area).
        ''' </remarks>
        Public ReadOnly Property DockArea As Rectangle
            Get
                Return New Rectangle(DockPadding.Left, DockPadding.Top, ClientRectangle.Width - DockPadding.Left - DockPadding.Right, ClientRectangle.Height - DockPadding.Top - DockPadding.Bottom)
            End Get
        End Property

        Private m_dockBottomPortion As Double = 0.25

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DockBottomPortion_Description")>
        <DefaultValue(0.25)>
        Public Property DockBottomPortion As Double
            Get
                Return m_dockBottomPortion
            End Get

            Set(value As Double)
                If value <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(value))

                If Math.Abs(value - m_dockBottomPortion) < Double.Epsilon Then Return

                m_dockBottomPortion = value

                If m_dockBottomPortion < 1 AndAlso m_dockTopPortion < 1 Then
                    If m_dockTopPortion + m_dockBottomPortion > 1 Then m_dockTopPortion = 1 - m_dockBottomPortion
                End If

                PerformLayout()
            End Set
        End Property

        Private m_dockLeftPortion As Double = 0.25

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DockLeftPortion_Description")>
        <DefaultValue(0.25)>
        Public Property DockLeftPortion As Double
            Get
                Return m_dockLeftPortion
            End Get

            Set(value As Double)
                If value <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(value))

                If Math.Abs(value - m_dockLeftPortion) < Double.Epsilon Then Return

                m_dockLeftPortion = value

                If m_dockLeftPortion < 1 AndAlso m_dockRightPortion < 1 Then
                    If m_dockLeftPortion + m_dockRightPortion > 1 Then m_dockRightPortion = 1 - m_dockLeftPortion
                End If
                PerformLayout()
            End Set
        End Property

        Private m_dockRightPortion As Double = 0.25

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DockRightPortion_Description")>
        <DefaultValue(0.25)>
        Public Property DockRightPortion As Double
            Get
                Return m_dockRightPortion
            End Get

            Set(value As Double)
                If value <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(value))

                If Math.Abs(value - m_dockRightPortion) < Double.Epsilon Then Return

                m_dockRightPortion = value

                If m_dockLeftPortion < 1 AndAlso m_dockRightPortion < 1 Then
                    If m_dockLeftPortion + m_dockRightPortion > 1 Then m_dockLeftPortion = 1 - m_dockRightPortion
                End If

                PerformLayout()
            End Set
        End Property

        Private m_dockTopPortion As Double = 0.25

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DockTopPortion_Description")>
        <DefaultValue(0.25)>
        Public Property DockTopPortion As Double
            Get
                Return m_dockTopPortion
            End Get

            Set(value As Double)
                If value <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(value))

                If Math.Abs(value - m_dockTopPortion) < Double.Epsilon Then Return

                m_dockTopPortion = value

                If m_dockTopPortion < 1 AndAlso m_dockBottomPortion < 1 Then
                    If m_dockTopPortion + m_dockBottomPortion > 1 Then m_dockBottomPortion = 1 - m_dockTopPortion
                End If
                PerformLayout()
            End Set
        End Property

        <Browsable(False)>
        Public ReadOnly Property DockWindows As DockWindowCollection
            Get
                Return m_dockWindows
            End Get
        End Property

        Public Sub UpdateDockWindowZOrder(dockStyle As DockStyle, fullPanelEdge As Boolean)
            If dockStyle = DockStyle.Left Then
                If fullPanelEdge Then
                    DockWindows(DockState.DockLeft).SendToBack()
                Else
                    DockWindows(DockState.DockLeft).BringToFront()
                End If
            ElseIf dockStyle = DockStyle.Right Then
                If fullPanelEdge Then
                    DockWindows(DockState.DockRight).SendToBack()
                Else
                    DockWindows(DockState.DockRight).BringToFront()
                End If
            ElseIf dockStyle = DockStyle.Top Then
                If fullPanelEdge Then
                    DockWindows(DockState.DockTop).SendToBack()
                Else
                    DockWindows(DockState.DockTop).BringToFront()
                End If
            ElseIf dockStyle = DockStyle.Bottom Then
                If fullPanelEdge Then
                    DockWindows(DockState.DockBottom).SendToBack()
                Else
                    DockWindows(DockState.DockBottom).BringToFront()
                End If
            End If
        End Sub

        <Browsable(False)>
        Public ReadOnly Property DocumentsCount As Integer
            Get
                Dim count = 0
                For Each content In Documents
                    count += 1
                Next

                Return count
            End Get
        End Property

        Public Function DocumentsToArray() As IDockContent()
            Dim count = DocumentsCount
            Dim documents = New IDockContent(count - 1) {}
            Dim i = 0
            For Each content In Me.Documents
                documents(i) = content
                i += 1
            Next

            Return documents
        End Function

        <Browsable(False)>
        Public ReadOnly Iterator Property Documents As IEnumerable(Of IDockContent)
            Get
                For Each content In Contents
                    If content.DockHandler.DockState = DockState.Document Then Yield content
                Next
            End Get
        End Property

        Private ReadOnly Property DummyControl As Control
            Get
                Return m_dummyControl
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property FloatWindows As FloatWindowCollection
            Get
                Return m_floatWindows
            End Get
        End Property

        <Category("Layout")>
        <LocalizedDescription("DockPanel_DefaultFloatWindowSize_Description")>
        Public Property DefaultFloatWindowSize As Size = New Size(300, 300)

        Private Function ShouldSerializeDefaultFloatWindowSize() As Boolean
            Return DefaultFloatWindowSize <> New Size(300, 300)
        End Function

        Private Sub ResetDefaultFloatWindowSize()
            DefaultFloatWindowSize = New Size(300, 300)
        End Sub

        Private m_documentStyle As DocumentStyle = DocumentStyle.DockingWindow
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DocumentStyle_Description")>
        <DefaultValue(DocumentStyle.DockingWindow)>
        Public Property DocumentStyle As DocumentStyle
            Get
                Return m_documentStyle
            End Get
            Set(value As DocumentStyle)
                If value = m_documentStyle Then Return

                If Not [Enum].IsDefined(GetType(DocumentStyle), value) Then Throw New InvalidEnumArgumentException()

                If value = DocumentStyle.SystemMdi AndAlso DockWindows(DockState.Document).VisibleNestedPanes.Count > 0 Then Throw New InvalidEnumArgumentException()

                m_documentStyle = value

                SuspendLayout(True)

                SetAutoHideWindowParent()
                SetMdiClient()
                InvalidateWindowRegion()

                For Each content In Contents
                    If content.DockHandler.DockState = DockState.Document Then content.DockHandler.SetPaneAndVisible(content.DockHandler.Pane)
                Next

                PerformMdiClientLayout()

                ResumeLayout(True, True)
            End Set
        End Property

        <LocalizedCategory("Category_Performance")>
        <LocalizedDescription("DockPanel_SupportDeeplyNestedContent_Description")>
        <DefaultValue(False)>
        Public Property SupportDeeplyNestedContent As Boolean

        ''' <summary>
        ''' Flag to show autohide content on mouse hover. Default value is <code>true</code>.
        ''' </summary>
        ''' <remarks>
        ''' This flag is ignored in VS2012/2013 themes. Such themes assume it is always <code>false</code>.
        ''' </remarks>
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_ShowAutoHideContentOnHover_Description")>
        <DefaultValue(True)>
        Public Property ShowAutoHideContentOnHover As Boolean

        Public Function GetDockWindowSize(dockState As DockState) As Integer
            If dockState = DockState.DockLeft OrElse dockState = DockState.DockRight Then
                Dim width = ClientRectangle.Width - DockPadding.Left - DockPadding.Right
                Dim dockLeftSize = If(m_dockLeftPortion >= 1, CInt(m_dockLeftPortion), CInt(width * m_dockLeftPortion))
                Dim dockRightSize = If(m_dockRightPortion >= 1, CInt(m_dockRightPortion), CInt(width * m_dockRightPortion))

                If dockLeftSize < MinSize Then dockLeftSize = MinSize
                If dockRightSize < MinSize Then dockRightSize = MinSize

                If dockLeftSize + dockRightSize > width - MinSize Then
                    Dim adjust = dockLeftSize + dockRightSize - (width - MinSize)
                    dockLeftSize -= CInt(adjust / 2)
                    dockRightSize -= CInt(adjust / 2)
                End If

                Return If(dockState = DockState.DockLeft, dockLeftSize, dockRightSize)
            End If

            If dockState = DockState.DockTop OrElse dockState = DockState.DockBottom Then
                Dim height = ClientRectangle.Height - DockPadding.Top - DockPadding.Bottom
                Dim dockTopSize = If(m_dockTopPortion >= 1, CInt(m_dockTopPortion), CInt(height * m_dockTopPortion))
                Dim dockBottomSize = If(m_dockBottomPortion >= 1, CInt(m_dockBottomPortion), CInt(height * m_dockBottomPortion))

                If dockTopSize < MinSize Then dockTopSize = MinSize
                If dockBottomSize < MinSize Then dockBottomSize = MinSize

                If dockTopSize + dockBottomSize > height - MinSize Then
                    Dim adjust = dockTopSize + dockBottomSize - (height - MinSize)
                    dockTopSize -= CInt(adjust / 2)
                    dockBottomSize -= CInt(adjust / 2)
                End If

                Return If(dockState = DockState.DockTop, dockTopSize, dockBottomSize)
            End If

            Return 0
        End Function

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            SuspendLayout(True)

            AutoHideStripControl.Bounds = ClientRectangle

            CalculateDockPadding()

            DockWindows(DockState.DockLeft).Width = GetDockWindowSize(DockState.DockLeft)
            DockWindows(DockState.DockRight).Width = GetDockWindowSize(DockState.DockRight)
            DockWindows(DockState.DockTop).Height = GetDockWindowSize(DockState.DockTop)
            DockWindows(DockState.DockBottom).Height = GetDockWindowSize(DockState.DockBottom)

            AutoHideWindow.Bounds = GetAutoHideWindowBounds(AutoHideWindowRectangle)

            Dim documentDockWindow = DockWindows(DockState.Document)

            If ReferenceEquals(documentDockWindow.Parent, AutoHideWindow.Parent) Then
                AutoHideWindow.Parent.Controls.SetChildIndex(AutoHideWindow, 0)
                documentDockWindow.Parent.Controls.SetChildIndex(documentDockWindow, 1)
            Else
                documentDockWindow.BringToFront()
                AutoHideWindow.BringToFront()
            End If

            MyBase.OnLayout(levent)

            If DocumentStyle = DocumentStyle.SystemMdi AndAlso MdiClientExists Then
                SetMdiClientBounds(SystemMdiClientBounds)
                InvalidateWindowRegion()
            ElseIf DocumentStyle = DocumentStyle.DockingMdi Then
                InvalidateWindowRegion()
            End If

            ResumeLayout(True, True)
        End Sub

        Friend Function GetTabStripRectangle(dockState As DockState) As Rectangle
            Return AutoHideStripControl.GetTabStripRectangle(dockState)
        End Function

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If DockBackColor.ToArgb() = MyBase.BackColor.ToArgb() Then Return

            Dim g = e.Graphics
            Dim bgBrush As SolidBrush = New SolidBrush(DockBackColor)
            g.FillRectangle(bgBrush, ClientRectangle)
        End Sub

        Friend Sub AddContent(content As IDockContent)
            If content Is Nothing Then Throw (New ArgumentNullException())

            If Not Contents.Contains(content) Then
                Contents.Add(content)
                OnContentAdded(New DockContentEventArgs(content))
            End If
        End Sub

        Friend Sub AddPane(pane As DockPane)
            If Panes.Contains(pane) Then Return

            Panes.Add(pane)
        End Sub

        Friend Sub AddFloatWindow(floatWindow As FloatWindow)
            If FloatWindows.Contains(floatWindow) Then Return

            FloatWindows.Add(floatWindow)
        End Sub

        Private Sub CalculateDockPadding()
            DockPadding.All = Theme.Measures.DockPadding
            Dim standard As Integer = AutoHideStripControl.MeasureHeight()
            If AutoHideStripControl.GetNumberOfPanes(DockState.DockLeftAutoHide) > 0 Then DockPadding.Left = standard
            If AutoHideStripControl.GetNumberOfPanes(DockState.DockRightAutoHide) > 0 Then DockPadding.Right = standard
            If AutoHideStripControl.GetNumberOfPanes(DockState.DockTopAutoHide) > 0 Then DockPadding.Top = standard
            If AutoHideStripControl.GetNumberOfPanes(DockState.DockBottomAutoHide) > 0 Then DockPadding.Bottom = standard
        End Sub

        Friend Sub RemoveContent(content As IDockContent)
            If content Is Nothing Then Throw (New ArgumentNullException())

            If Contents.Contains(content) Then
                Contents.Remove(content)
                OnContentRemoved(New DockContentEventArgs(content))
            End If
        End Sub

        Friend Sub RemovePane(pane As DockPane)
            If Not Panes.Contains(pane) Then Return

            Panes.Remove(pane)
        End Sub

        Friend Sub RemoveFloatWindow(floatWindow As FloatWindow)
            If Not FloatWindows.Contains(floatWindow) Then Return

            FloatWindows.Remove(floatWindow)
            If FloatWindows.Count <> 0 Then Return

            If ParentForm Is Nothing Then Return

            ParentForm.Focus()
        End Sub

        Public Sub SetPaneIndex(pane As DockPane, index As Integer)
            Dim oldIndex = Panes.IndexOf(pane)
            If oldIndex = -1 Then Throw (New ArgumentException(Strings.DockPanel_SetPaneIndex_InvalidPane))

            If index < 0 OrElse index > Panes.Count - 1 Then
                If index <> -1 Then Throw (New ArgumentOutOfRangeException(Strings.DockPanel_SetPaneIndex_InvalidIndex))
            End If

            If oldIndex = index Then Return
            If oldIndex = Panes.Count - 1 AndAlso index = -1 Then Return

            Panes.Remove(pane)
            If index = -1 Then
                Panes.Add(pane)
            ElseIf oldIndex < index Then
                Panes.AddAt(pane, index - 1)
            Else
                Panes.AddAt(pane, index)
            End If
        End Sub

        Public Overloads Sub SuspendLayout(allWindows As Boolean)
            FocusManager.SuspendFocusTracking()
            MyBase.SuspendLayout()
            If allWindows Then SuspendMdiClientLayout()
        End Sub

        Public Overloads Sub ResumeLayout(performLayout As Boolean, allWindows As Boolean)
            FocusManager.ResumeFocusTracking()
            MyBase.ResumeLayout(performLayout)
            If allWindows Then ResumeMdiClientLayout(performLayout)
        End Sub

        Friend ReadOnly Property ParentForm As Form
            Get
                If Not IsParentFormValid() Then Throw New InvalidOperationException(Strings.DockPanel_ParentForm_Invalid)

                Return GetMdiClientController().ParentForm
            End Get
        End Property

        Private Function IsParentFormValid() As Boolean
            If DocumentStyle = DocumentStyle.DockingSdi OrElse DocumentStyle = DocumentStyle.DockingWindow Then Return True

            If Not MdiClientExists Then GetMdiClientController().RenewMdiClient()

            Return MdiClientExists
        End Function

        Protected Overrides Sub OnParentChanged(e As EventArgs)
            SetAutoHideWindowParent()
            GetMdiClientController().ParentForm = TryCast(Parent, Form)
            MyBase.OnParentChanged(e)
        End Sub

        Private Sub SetAutoHideWindowParent()
            Dim parent As Control
            If DocumentStyle = DocumentStyle.DockingMdi OrElse DocumentStyle = DocumentStyle.SystemMdi Then
                parent = Me.Parent
            Else
                parent = Me
            End If
            If AutoHideWindow.Parent IsNot parent Then
                AutoHideWindow.Parent = parent
                AutoHideWindow.BringToFront()
            End If
        End Sub

        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)

            If Visible Then SetMdiClient()
        End Sub

        Private ReadOnly Property SystemMdiClientBounds As Rectangle
            Get
                If Not IsParentFormValid() OrElse Not Visible Then Return Rectangle.Empty

                Dim rect = ParentForm.RectangleToClient(RectangleToScreen(DocumentWindowBounds))
                Return rect
            End Get
        End Property

        Public ReadOnly Property DocumentWindowBounds As Rectangle
            Get
                Dim rectDocumentBounds = DisplayRectangle
                If DockWindows(DockState.DockLeft).Visible Then
                    rectDocumentBounds.X += DockWindows(DockState.DockLeft).Width
                    rectDocumentBounds.Width -= DockWindows(DockState.DockLeft).Width
                End If
                If DockWindows(DockState.DockRight).Visible Then rectDocumentBounds.Width -= DockWindows(DockState.DockRight).Width
                If DockWindows(DockState.DockTop).Visible Then
                    rectDocumentBounds.Y += DockWindows(DockState.DockTop).Height
                    rectDocumentBounds.Height -= DockWindows(DockState.DockTop).Height
                End If
                If DockWindows(DockState.DockBottom).Visible Then rectDocumentBounds.Height -= DockWindows(DockState.DockBottom).Height

                Return rectDocumentBounds

            End Get
        End Property

        Private m_dummyControlPaintEventHandler As PaintEventHandler = Nothing
        Private Sub InvalidateWindowRegion()
            If DesignMode Then Return

            If m_dummyControlPaintEventHandler Is Nothing Then m_dummyControlPaintEventHandler = New PaintEventHandler(AddressOf DummyControl_Paint)

            AddHandler DummyControl.Paint, m_dummyControlPaintEventHandler
            DummyControl.Invalidate()
        End Sub

        Private Sub DummyControl_Paint(sender As Object, e As PaintEventArgs)
            RemoveHandler DummyControl.Paint, m_dummyControlPaintEventHandler
            UpdateWindowRegion()
        End Sub

        Private Sub UpdateWindowRegion()
            If DocumentStyle = DocumentStyle.DockingMdi Then
                UpdateWindowRegion_ClipContent()
            ElseIf DocumentStyle = DocumentStyle.DockingSdi OrElse DocumentStyle = DocumentStyle.DockingWindow Then
                UpdateWindowRegion_FullDocumentArea()
            ElseIf DocumentStyle = DocumentStyle.SystemMdi Then
                UpdateWindowRegion_EmptyDocumentArea()
            End If
        End Sub

        Private Sub UpdateWindowRegion_FullDocumentArea()
            SetRegion(Nothing)
        End Sub

        Private Sub UpdateWindowRegion_EmptyDocumentArea()
            Dim rect = DocumentWindowBounds
            SetRegion(New Rectangle() {rect})
        End Sub

        Private Sub UpdateWindowRegion_ClipContent()
            Dim count = 0
            For Each pane In Panes
                If Not pane.Visible OrElse pane.DockState <> DockState.Document Then Continue For

                count += 1
            Next

            If count = 0 Then
                SetRegion(Nothing)
                Return
            End If

            Dim rects = New Rectangle(count - 1) {}
            Dim i = 0
            For Each pane In Panes
                If Not pane.Visible OrElse pane.DockState <> DockState.Document Then Continue For

                rects(i) = RectangleToClient(pane.RectangleToScreen(pane.ContentRectangle))
                i += 1
            Next

            SetRegion(rects)
        End Sub

        Private m_clipRects As Rectangle() = Nothing
        Private Sub SetRegion(clipRects As Rectangle())
            If Not IsClipRectsChanged(clipRects) Then Return

            m_clipRects = clipRects

            If m_clipRects Is Nothing OrElse m_clipRects.GetLength(0) = 0 Then
                Region = Nothing
            Else
                Dim region As Region = New Region(New Rectangle(0, 0, Width, Height))
                For Each rect In m_clipRects
                    region.Exclude(rect)
                Next
                If MyBase.Region IsNot Nothing Then
                    MyBase.Region.Dispose()
                End If

                MyBase.Region = region
            End If
        End Sub

        Private Function IsClipRectsChanged(clipRects As Rectangle()) As Boolean
            If clipRects Is Nothing AndAlso m_clipRects Is Nothing Then
                Return False
            ElseIf clipRects Is Nothing <> (m_clipRects Is Nothing) Then
                Return True
            End If

            For Each rect In clipRects
                Dim matched = False
                For Each rect2 In m_clipRects
                    If rect = rect2 Then
                        matched = True
                        Exit For
                    End If
                Next
                If Not matched Then Return True
            Next

            For Each rect2 In m_clipRects
                Dim matched = False
                For Each rect In clipRects
                    If rect = rect2 Then
                        matched = True
                        Exit For
                    End If
                Next
                If Not matched Then Return True
            Next
            Return False
        End Function

        Private Shared ReadOnly ActiveAutoHideContentChangedEvent As Object = New Object()
        <LocalizedCategory("Category_DockingNotification")>
        <LocalizedDescription("DockPanel_ActiveAutoHideContentChanged_Description")>
        Public Custom Event ActiveAutoHideContentChanged As EventHandler
            AddHandler(value As EventHandler)
                Events.AddHandler(ActiveAutoHideContentChangedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler)
                Events.RemoveHandler(ActiveAutoHideContentChangedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_DockingNotification")>
            <LocalizedDescription("DockPanel_ActiveAutoHideContentChanged_Description")>
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnActiveAutoHideContentChanged(e As EventArgs)
            Dim handler = CType(Events(ActiveAutoHideContentChangedEvent), EventHandler)
            If handler IsNot Nothing Then handler(Me, e)
        End Sub
        Private Sub m_autoHideWindow_ActiveContentChanged(sender As Object, e As EventArgs)
            OnActiveAutoHideContentChanged(e)
        End Sub


        Private Shared ReadOnly ContentAddedEvent As Object = New Object()
        <LocalizedCategory("Category_DockingNotification")>
        <LocalizedDescription("DockPanel_ContentAdded_Description")>
        Public Custom Event ContentAdded As EventHandler(Of DockContentEventArgs)
            AddHandler(value As EventHandler(Of DockContentEventArgs))
                Events.AddHandler(ContentAddedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler(Of DockContentEventArgs))
                Events.RemoveHandler(ContentAddedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_DockingNotification")>
            <LocalizedDescription("DockPanel_ContentAdded_Description")>
            RaiseEvent(sender As Object, e As DockContentEventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnContentAdded(e As DockContentEventArgs)
            Dim handler = CType(Events(ContentAddedEvent), EventHandler(Of DockContentEventArgs))
            If handler IsNot Nothing Then handler(Me, e)
        End Sub

        Private Shared ReadOnly ContentRemovedEvent As Object = New Object()
        <LocalizedCategory("Category_DockingNotification")>
        <LocalizedDescription("DockPanel_ContentRemoved_Description")>
        Public Custom Event ContentRemoved As EventHandler(Of DockContentEventArgs)
            AddHandler(value As EventHandler(Of DockContentEventArgs))
                Events.AddHandler(ContentRemovedEvent, value)
            End AddHandler
            RemoveHandler(value As EventHandler(Of DockContentEventArgs))
                Events.RemoveHandler(ContentRemovedEvent, value)
            End RemoveHandler
            <LocalizedCategory("Category_DockingNotification")>
            <LocalizedDescription("DockPanel_ContentRemoved_Description")>
            RaiseEvent(sender As Object, e As DockContentEventArgs)
            End RaiseEvent
        End Event
        Protected Overridable Sub OnContentRemoved(e As DockContentEventArgs)
            Dim handler = CType(Events(ContentRemovedEvent), EventHandler(Of DockContentEventArgs))
            If handler IsNot Nothing Then handler(Me, e)
        End Sub

        Friend Sub ResetDockWindows()
            If m_autoHideWindow Is Nothing Then
                Return
            End If

            Dim old = m_dockWindows
            LoadDockWindows()
            For Each dockWindow In old
                Controls.Remove(dockWindow)
                dockWindow.Dispose()
            Next
        End Sub

        Friend Sub LoadDockWindows()
            m_dockWindows = New DockWindowCollection(Me)
            For Each dockWindow In DockWindows
                Controls.Add(dockWindow)
            Next
        End Sub

        Public Sub ResetAutoHideStripWindow()
            Dim old = m_autoHideWindow
            m_autoHideWindow = Theme.Extender.AutoHideWindowFactory.CreateAutoHideWindow(Me)
            m_autoHideWindow.Visible = False
            SetAutoHideWindowParent()

            old.Visible = False
            old.Parent = Nothing
            old.Dispose()
        End Sub
    End Class
End Namespace
