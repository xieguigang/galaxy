Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.ComponentModel.Design

Namespace WeifenLuo.WinFormsUI.Docking
    Partial Class DockPanel
        '  This class comes from Jacob Slusser's MdiClientController class:
        '  http://www.codeproject.com/cs/miscctrl/mdiclientcontroller.asp
        Private Class MdiClientController
            Inherits NativeWindow
            Implements IComponent, IDisposable
            Private m_autoScroll As Boolean = True
            Private m_borderStyle As BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Private m_mdiClient As MdiClient = Nothing
            Private m_parentForm As Form = Nothing
            Private m_site As ISite = Nothing

            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub

            Protected Overridable Sub Dispose(disposing As Boolean)
                If disposing Then
                    If Site IsNot Nothing AndAlso Site.Container IsNot Nothing Then Site.Container.Remove(Me)

                    RaiseEvent Disposed(Me, EventArgs.Empty)
                End If
            End Sub

            Public Property AutoScroll As Boolean
                Get
                    Return m_autoScroll
                End Get
                Set(value As Boolean)
                    ' By default the MdiClient control scrolls. It can appear though that
                    ' there are no scrollbars by turning them off when the non-client
                    ' area is calculated. I decided to expose this method following
                    ' the .NET vernacular of an AutoScroll property.
                    m_autoScroll = value
                    If MdiClient IsNot Nothing Then UpdateStyles()
                End Set
            End Property

            Public WriteOnly Property BorderStyle As BorderStyle
                Set(value As BorderStyle)
                    ' Error-check the enum.
                    If Not [Enum].IsDefined(GetType(BorderStyle), value) Then Throw New InvalidEnumArgumentException()

                    m_borderStyle = value

                    If MdiClient Is Nothing Then Return

                    ' This property can actually be visible in design-mode,
                    ' but to keep it consistent with the others,
                    ' prevent this from being show at design-time.
                    If Site IsNot Nothing AndAlso Site.DesignMode Then Return

                    ' There is no BorderStyle property exposed by the MdiClient class,
                    ' but this can be controlled by Win32 functions. A Win32 ExStyle
                    ' of WS_EX_CLIENTEDGE is equivalent to a Fixed3D border and a
                    ' Style of WS_BORDER is equivalent to a FixedSingle border.

                    ' This code is inspired Jason Dori's article:
                    ' "Adding designable borders to user controls".
                    ' http://www.codeproject.com/cs/miscctrl/CsAddingBorders.asp

                    If Not IsRunningOnMono Then
                        ' Get styles using Win32 calls
                        Dim style = GetWindowLong(MdiClient.Handle, Win32.GetWindowLongIndex.GWL_STYLE)
                        Dim exStyle = GetWindowLong(MdiClient.Handle, Win32.GetWindowLongIndex.GWL_EXSTYLE)

                        ' Add or remove style flags as necessary.
                        Select Case m_borderStyle
                            Case Windows.Forms.BorderStyle.Fixed3D
                                exStyle = exStyle Or Win32.WindowExStyles.WS_EX_CLIENTEDGE
                                style = style And Not CInt(Win32.WindowStyles.WS_BORDER)

                            Case Windows.Forms.BorderStyle.FixedSingle
                                exStyle = exStyle And Not Win32.WindowExStyles.WS_EX_CLIENTEDGE
                                style = style Or CInt(Win32.WindowStyles.WS_BORDER)

                            Case Windows.Forms.BorderStyle.None
                                style = style And Not CInt(Win32.WindowStyles.WS_BORDER)
                                exStyle = exStyle And Not Win32.WindowExStyles.WS_EX_CLIENTEDGE
                        End Select

                        ' Set the styles using Win32 calls
                        SetWindowLong(MdiClient.Handle, Win32.GetWindowLongIndex.GWL_STYLE, style)
                        SetWindowLong(MdiClient.Handle, Win32.GetWindowLongIndex.GWL_EXSTYLE, exStyle)
                    End If

                    ' Cause an update of the non-client area.
                    UpdateStyles()
                End Set
            End Property

            Public ReadOnly Property MdiClient As MdiClient
                Get
                    Return m_mdiClient
                End Get
            End Property

            <Browsable(False)>
            <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
            Public Property ParentForm As Form
                Get
                    Return m_parentForm
                End Get
                Set(value As Form)
                    ' If the ParentForm has previously been set,
                    ' unwire events connected to the old parent.
                    If m_parentForm IsNot Nothing Then
                        RemoveHandler m_parentForm.HandleCreated, New EventHandler(AddressOf ParentFormHandleCreated)
                        RemoveHandler m_parentForm.MdiChildActivate, New EventHandler(AddressOf ParentFormMdiChildActivate)
                    End If

                    m_parentForm = value

                    If m_parentForm Is Nothing Then Return

                    ' If the parent form has not been created yet,
                    ' wait to initialize the MDI client until it is.
                    If m_parentForm.IsHandleCreated Then
                        InitializeMdiClient()
                        RefreshProperties()
                    Else
                        AddHandler m_parentForm.HandleCreated, New EventHandler(AddressOf ParentFormHandleCreated)
                    End If

                    AddHandler m_parentForm.MdiChildActivate, New EventHandler(AddressOf ParentFormMdiChildActivate)
                End Set
            End Property

            Public Property Site As ISite Implements IComponent.Site
                Get
                    Return m_site
                End Get
                Set(value As ISite)
                    m_site = value

                    If m_site Is Nothing Then Return

                    ' If the component is dropped onto a form during design-time,
                    ' set the ParentForm property.
                    Dim host As IDesignerHost = TryCast(value.GetService(GetType(IDesignerHost)), IDesignerHost)
                    If host IsNot Nothing Then
                        Dim parent As Form = TryCast(host.RootComponent, Form)
                        If parent IsNot Nothing Then ParentForm = parent
                    End If
                End Set
            End Property

            Public Sub RenewMdiClient()
                ' Reinitialize the MdiClient and its properties.
                InitializeMdiClient()
                RefreshProperties()
            End Sub

            Public Event Disposed As EventHandler Implements IComponent.Disposed

            Public Event HandleAssigned As EventHandler

            Public Event MdiChildActivate As EventHandler

            Public Event Layout As LayoutEventHandler

            Protected Overridable Sub OnHandleAssigned(e As EventArgs)
                ' Raise the HandleAssigned event.
                RaiseEvent HandleAssigned(Me, e)
            End Sub

            Protected Overridable Sub OnMdiChildActivate(e As EventArgs)
                ' Raise the MdiChildActivate event
                RaiseEvent MdiChildActivate(Me, e)
            End Sub

            Protected Overridable Sub OnLayout(e As LayoutEventArgs)
                ' Raise the Layout event
                RaiseEvent Layout(Me, e)
            End Sub

            Public Event Paint As PaintEventHandler

            Protected Overridable Sub OnPaint(e As PaintEventArgs)
                ' Raise the Paint event.
                RaiseEvent Paint(Me, e)
            End Sub

            Protected Overrides Sub WndProc(ByRef m As Message)
                Select Case m.Msg
                    Case Win32.Msgs.WM_NCCALCSIZE
                        ' If AutoScroll is set to false, hide the scrollbars when the control
                        ' calculates its non-client area.
                        If Not AutoScroll Then
                            If Not IsRunningOnMono Then
                                ShowScrollBar(m.HWnd, Win32.ScrollBars.SB_BOTH, 0)
                            End If
                        End If
                End Select

                MyBase.WndProc(m)
            End Sub

            Private Sub ParentFormHandleCreated(sender As Object, e As EventArgs)
                ' The form has been created, unwire the event, and initialize the MdiClient.
                RemoveHandler m_parentForm.HandleCreated, New EventHandler(AddressOf ParentFormHandleCreated)
                InitializeMdiClient()
                RefreshProperties()
            End Sub

            Private Sub ParentFormMdiChildActivate(sender As Object, e As EventArgs)
                OnMdiChildActivate(e)
            End Sub

            Private Sub MdiClientLayout(sender As Object, e As LayoutEventArgs)
                OnLayout(e)
            End Sub

            Private Sub MdiClientHandleDestroyed(sender As Object, e As EventArgs)
                ' If the MdiClient handle has been released, drop the reference and
                ' release the handle.
                If m_mdiClient IsNot Nothing Then
                    RemoveHandler m_mdiClient.HandleDestroyed, New EventHandler(AddressOf MdiClientHandleDestroyed)
                    m_mdiClient = Nothing
                End If

                MyBase.ReleaseHandle()
            End Sub

            Private Sub InitializeMdiClient()
                ' If the mdiClient has previously been set, unwire events connected
                ' to the old MDI.
                If MdiClient IsNot Nothing Then
                    RemoveHandler MdiClient.HandleDestroyed, New EventHandler(AddressOf MdiClientHandleDestroyed)
                    RemoveHandler MdiClient.Layout, New LayoutEventHandler(AddressOf MdiClientLayout)
                End If

                If ParentForm Is Nothing Then Return

                ' Get the MdiClient from the parent form.
                For Each control As Control In ParentForm.Controls
                    ' If the form is an MDI container, it will contain an MdiClient control
                    ' just as it would any other control.

                    m_mdiClient = TryCast(control, MdiClient)
                    If m_mdiClient Is Nothing Then Continue For

                    ' Assign the MdiClient Handle to the NativeWindow.
                    MyBase.ReleaseHandle()
                    AssignHandle(MdiClient.Handle)

                    ' Raise the HandleAssigned event.
                    OnHandleAssigned(EventArgs.Empty)

                    ' Monitor the MdiClient for when its handle is destroyed.
                    AddHandler MdiClient.HandleDestroyed, New EventHandler(AddressOf MdiClientHandleDestroyed)
                    AddHandler MdiClient.Layout, New LayoutEventHandler(AddressOf MdiClientLayout)

                    Exit For
                Next
            End Sub

            Private Sub RefreshProperties()
                ' Refresh all the properties
                BorderStyle = m_borderStyle
                AutoScroll = m_autoScroll
            End Sub

            Private Sub UpdateStyles()
                ' To show style changes, the non-client area must be repainted. Using the
                ' control's Invalidate method does not affect the non-client area.
                ' Instead use a Win32 call to signal the style has changed.
                If Not IsRunningOnMono Then SetWindowPos(MdiClient.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32.FlagsSetWindowPos.SWP_NOACTIVATE Or Win32.FlagsSetWindowPos.SWP_NOMOVE Or Win32.FlagsSetWindowPos.SWP_NOSIZE Or Win32.FlagsSetWindowPos.SWP_NOZORDER Or Win32.FlagsSetWindowPos.SWP_NOOWNERZORDER Or Win32.FlagsSetWindowPos.SWP_FRAMECHANGED)
            End Sub
        End Class

        Private m_mdiClientController As MdiClientController = Nothing
        Private Function GetMdiClientController() As MdiClientController
            If m_mdiClientController Is Nothing Then
                m_mdiClientController = New MdiClientController()
                AddHandler m_mdiClientController.HandleAssigned, New EventHandler(AddressOf MdiClientHandleAssigned)
                AddHandler m_mdiClientController.MdiChildActivate, New EventHandler(AddressOf ParentFormMdiChildActivate)
                AddHandler m_mdiClientController.Layout, New LayoutEventHandler(AddressOf MdiClient_Layout)
            End If

            Return m_mdiClientController
        End Function

        Private Sub ParentFormMdiChildActivate(sender As Object, e As EventArgs)
            If GetMdiClientController().ParentForm Is Nothing Then Return

            Dim content As IDockContent = TryCast(GetMdiClientController().ParentForm.ActiveMdiChild, IDockContent)
            If content Is Nothing Then Return

            If content.DockHandler.DockPanel Is Me AndAlso content.DockHandler.Pane IsNot Nothing Then
                If content.DockHandler.Pane.DisplayingContents.Contains(content) Then
                    content.DockHandler.Pane.ActiveContent = content
                ElseIf EnableActiveControlFix <> True Then
                    content.DockHandler.Pane.ActiveContent = content
                End If
            End If
        End Sub

        Private ReadOnly Property MdiClientExists As Boolean
            Get
                Return GetMdiClientController().MdiClient IsNot Nothing
            End Get
        End Property

        Private Sub SetMdiClientBounds(bounds As Rectangle)
            GetMdiClientController().MdiClient.Bounds = bounds
        End Sub

        Private Sub SuspendMdiClientLayout()
            If GetMdiClientController().MdiClient IsNot Nothing Then GetMdiClientController().MdiClient.SuspendLayout()
        End Sub

        Private Sub ResumeMdiClientLayout(perform As Boolean)
            If GetMdiClientController().MdiClient IsNot Nothing Then GetMdiClientController().MdiClient.ResumeLayout(perform)
        End Sub

        Private Sub PerformMdiClientLayout()
            If GetMdiClientController().MdiClient IsNot Nothing Then GetMdiClientController().MdiClient.PerformLayout()
        End Sub

        ' Called when:
        ' 1. DockPanel.DocumentStyle changed
        ' 2. DockPanel.Visible changed
        ' 3. MdiClientController.Handle assigned
        Private Sub SetMdiClient()
            Dim controller As MdiClientController = GetMdiClientController()

            If DocumentStyle = DocumentStyle.DockingMdi Then
                controller.AutoScroll = False
                controller.BorderStyle = BorderStyle.None
                If MdiClientExists Then controller.MdiClient.Dock = DockStyle.Fill
            ElseIf DocumentStyle = DocumentStyle.DockingSdi OrElse DocumentStyle = DocumentStyle.DockingWindow Then
                controller.AutoScroll = True
                controller.BorderStyle = BorderStyle.Fixed3D
                If MdiClientExists Then controller.MdiClient.Dock = DockStyle.Fill
            ElseIf DocumentStyle = DocumentStyle.SystemMdi Then
                controller.AutoScroll = True
                controller.BorderStyle = BorderStyle.Fixed3D
                If controller.MdiClient IsNot Nothing Then
                    controller.MdiClient.Dock = DockStyle.None
                    controller.MdiClient.Bounds = SystemMdiClientBounds
                End If
            End If
        End Sub

        Friend Function RectangleToMdiClient(rect As Rectangle) As Rectangle
            If MdiClientExists Then
                Return GetMdiClientController().MdiClient.RectangleToClient(rect)
            Else
                Return Rectangle.Empty
            End If
        End Function
    End Class
End Namespace
