Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Windows.Forms.Animations

Namespace Controls

    Public Class MaterialSingleLineTextField
        Inherits Control
        Implements IMaterialControl

        'Properties for managing the material design properties
        <Browsable(False)>
        Public Property Depth() As Integer Implements IMaterialControl.Depth

        <Browsable(False)>
        Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
            Get
                Return MaterialSkinManager.Instance
            End Get
        End Property
        <Browsable(False)>
        Public Property MouseState() As MouseState Implements IMaterialControl.MouseState

        Public Overrides Property Text() As String
            Get
                Return baseTextBox.Text
            End Get
            Set
                baseTextBox.Text = Value
            End Set
        End Property
        Public Shadows Property Tag() As Object
            Get
                Return baseTextBox.Tag
            End Get
            Set
                baseTextBox.Tag = Value
            End Set
        End Property
        Public Shadows Property MaxLength() As Integer
            Get
                Return baseTextBox.MaxLength
            End Get
            Set
                baseTextBox.MaxLength = Value
            End Set
        End Property

        Public Property SelectedText() As String
            Get
                Return baseTextBox.SelectedText
            End Get
            Set
                baseTextBox.SelectedText = Value
            End Set
        End Property
        Public Property Hint() As String
            Get
                Return baseTextBox.Hint
            End Get
            Set
                baseTextBox.Hint = Value
            End Set
        End Property

        Public Property SelectionStart() As Integer
            Get
                Return baseTextBox.SelectionStart
            End Get
            Set
                baseTextBox.SelectionStart = Value
            End Set
        End Property
        Public Property SelectionLength() As Integer
            Get
                Return baseTextBox.SelectionLength
            End Get
            Set
                baseTextBox.SelectionLength = Value
            End Set
        End Property
        Public ReadOnly Property TextLength() As Integer
            Get
                Return baseTextBox.TextLength
            End Get
        End Property

        Public Property UseSystemPasswordChar() As Boolean
            Get
                Return baseTextBox.UseSystemPasswordChar
            End Get
            Set
                baseTextBox.UseSystemPasswordChar = Value
            End Set
        End Property
        Public Property PasswordChar() As Char
            Get
                Return baseTextBox.PasswordChar
            End Get
            Set
                baseTextBox.PasswordChar = Value
            End Set
        End Property

        Public Sub SelectAll()
            baseTextBox.SelectAll()
        End Sub
        Public Sub Clear()
            baseTextBox.Clear()
        End Sub


#Region "Forwarding events to baseTextBox"
        Public Custom Event AcceptsTabChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.AcceptsTabChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.AcceptsTabChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event AutoSizeChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.AutoSizeChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.AutoSizeChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event BackgroundImageChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.BackgroundImageChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.BackgroundImageChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event BackgroundImageLayoutChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.BackgroundImageLayoutChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.BackgroundImageLayoutChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event BindingContextChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.BindingContextChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.BindingContextChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Custom Event BorderStyleChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.BorderStyleChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.BorderStyleChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event CausesValidationChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.CausesValidationChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.CausesValidationChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ChangeUICues As UICuesEventHandler
            AddHandler(ByVal value As UICuesEventHandler)
                AddHandler baseTextBox.ChangeUICues, value
            End AddHandler
            RemoveHandler(ByVal value As UICuesEventHandler)
                RemoveHandler baseTextBox.ChangeUICues, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Click As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Click, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Click, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ClientSizeChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ClientSizeChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ClientSizeChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ContextMenuChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ContextMenuChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ContextMenuChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ContextMenuStripChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ContextMenuStripChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ContextMenuStripChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ControlAdded As ControlEventHandler
            AddHandler(ByVal value As ControlEventHandler)
                AddHandler baseTextBox.ControlAdded, value
            End AddHandler
            RemoveHandler(ByVal value As ControlEventHandler)
                RemoveHandler baseTextBox.ControlAdded, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ControlRemoved As ControlEventHandler
            AddHandler(ByVal value As ControlEventHandler)
                AddHandler baseTextBox.ControlRemoved, value
            End AddHandler
            RemoveHandler(ByVal value As ControlEventHandler)
                RemoveHandler baseTextBox.ControlRemoved, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event CursorChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.CursorChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.CursorChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Disposed As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Disposed, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Disposed, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event DockChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.DockChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.DockChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event DoubleClick As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.DoubleClick, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.DoubleClick, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event DragDrop As DragEventHandler
            AddHandler(ByVal value As DragEventHandler)
                AddHandler baseTextBox.DragDrop, value
            End AddHandler
            RemoveHandler(ByVal value As DragEventHandler)
                RemoveHandler baseTextBox.DragDrop, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event DragEnter As DragEventHandler
            AddHandler(ByVal value As DragEventHandler)
                AddHandler baseTextBox.DragEnter, value
            End AddHandler
            RemoveHandler(ByVal value As DragEventHandler)
                RemoveHandler baseTextBox.DragEnter, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event DragLeave As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.DragLeave, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.DragLeave, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event DragOver As DragEventHandler
            AddHandler(ByVal value As DragEventHandler)
                AddHandler baseTextBox.DragOver, value
            End AddHandler
            RemoveHandler(ByVal value As DragEventHandler)
                RemoveHandler baseTextBox.DragOver, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As DragEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event EnabledChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.EnabledChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.EnabledChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Enter As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Enter, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Enter, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event FontChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.FontChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.FontChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ForeColorChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ForeColorChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ForeColorChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event GiveFeedback As GiveFeedbackEventHandler
            AddHandler(ByVal value As GiveFeedbackEventHandler)
                AddHandler baseTextBox.GiveFeedback, value
            End AddHandler
            RemoveHandler(ByVal value As GiveFeedbackEventHandler)
                RemoveHandler baseTextBox.GiveFeedback, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As GiveFeedbackEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event GotFocus As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.GotFocus, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.GotFocus, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event HandleCreated As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.HandleCreated, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.HandleCreated, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event HandleDestroyed As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.HandleDestroyed, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.HandleDestroyed, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event HelpRequested As HelpEventHandler
            AddHandler(ByVal value As HelpEventHandler)
                AddHandler baseTextBox.HelpRequested, value
            End AddHandler
            RemoveHandler(ByVal value As HelpEventHandler)
                RemoveHandler baseTextBox.HelpRequested, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As HelpEventArgs)
            End RaiseEvent
        End Event

        Public Custom Event HideSelectionChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.HideSelectionChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.HideSelectionChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ImeModeChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ImeModeChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ImeModeChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Invalidated As InvalidateEventHandler
            AddHandler(ByVal value As InvalidateEventHandler)
                AddHandler baseTextBox.Invalidated, value
            End AddHandler
            RemoveHandler(ByVal value As InvalidateEventHandler)
                RemoveHandler baseTextBox.Invalidated, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As InvalidateEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event KeyDown As KeyEventHandler
            AddHandler(ByVal value As KeyEventHandler)
                AddHandler baseTextBox.KeyDown, value
            End AddHandler
            RemoveHandler(ByVal value As KeyEventHandler)
                RemoveHandler baseTextBox.KeyDown, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As KeyEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event KeyPress As KeyPressEventHandler
            AddHandler(ByVal value As KeyPressEventHandler)
                AddHandler baseTextBox.KeyPress, value
            End AddHandler
            RemoveHandler(ByVal value As KeyPressEventHandler)
                RemoveHandler baseTextBox.KeyPress, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As KeyPressEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event KeyUp As KeyEventHandler
            AddHandler(ByVal value As KeyEventHandler)
                AddHandler baseTextBox.KeyUp, value
            End AddHandler
            RemoveHandler(ByVal value As KeyEventHandler)
                RemoveHandler baseTextBox.KeyUp, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As KeyEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Layout As LayoutEventHandler
            AddHandler(ByVal value As LayoutEventHandler)
                AddHandler baseTextBox.Layout, value
            End AddHandler
            RemoveHandler(ByVal value As LayoutEventHandler)
                RemoveHandler baseTextBox.Layout, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As LayoutEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Leave As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Leave, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Leave, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event LocationChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.LocationChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.LocationChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event LostFocus As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.LostFocus, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.LostFocus, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MarginChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.MarginChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.MarginChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Custom Event ModifiedChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ModifiedChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ModifiedChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseCaptureChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.MouseCaptureChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.MouseCaptureChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseClick As MouseEventHandler
            AddHandler(ByVal value As MouseEventHandler)
                AddHandler baseTextBox.MouseClick, value
            End AddHandler
            RemoveHandler(ByVal value As MouseEventHandler)
                RemoveHandler baseTextBox.MouseClick, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As MouseEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseDoubleClick As MouseEventHandler
            AddHandler(ByVal value As MouseEventHandler)
                AddHandler baseTextBox.MouseDoubleClick, value
            End AddHandler
            RemoveHandler(ByVal value As MouseEventHandler)
                RemoveHandler baseTextBox.MouseDoubleClick, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As MouseEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseDown As MouseEventHandler
            AddHandler(ByVal value As MouseEventHandler)
                AddHandler baseTextBox.MouseDown, value
            End AddHandler
            RemoveHandler(ByVal value As MouseEventHandler)
                RemoveHandler baseTextBox.MouseDown, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As MouseEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseEnter As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.MouseEnter, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.MouseEnter, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseHover As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.MouseHover, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.MouseHover, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseLeave As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.MouseLeave, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.MouseLeave, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseMove As MouseEventHandler
            AddHandler(ByVal value As MouseEventHandler)
                AddHandler baseTextBox.MouseMove, value
            End AddHandler
            RemoveHandler(ByVal value As MouseEventHandler)
                RemoveHandler baseTextBox.MouseMove, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As MouseEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseUp As MouseEventHandler
            AddHandler(ByVal value As MouseEventHandler)
                AddHandler baseTextBox.MouseUp, value
            End AddHandler
            RemoveHandler(ByVal value As MouseEventHandler)
                RemoveHandler baseTextBox.MouseUp, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As MouseEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event MouseWheel As MouseEventHandler
            AddHandler(ByVal value As MouseEventHandler)
                AddHandler baseTextBox.MouseWheel, value
            End AddHandler
            RemoveHandler(ByVal value As MouseEventHandler)
                RemoveHandler baseTextBox.MouseWheel, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As MouseEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Move As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Move, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Move, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Custom Event MultilineChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.MultilineChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.MultilineChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event PaddingChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.PaddingChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.PaddingChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Paint As PaintEventHandler
            AddHandler(ByVal value As PaintEventHandler)
                AddHandler baseTextBox.Paint, value
            End AddHandler
            RemoveHandler(ByVal value As PaintEventHandler)
                RemoveHandler baseTextBox.Paint, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As PaintEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event ParentChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ParentChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ParentChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event PreviewKeyDown As PreviewKeyDownEventHandler
            AddHandler(ByVal value As PreviewKeyDownEventHandler)
                AddHandler baseTextBox.PreviewKeyDown, value
            End AddHandler
            RemoveHandler(ByVal value As PreviewKeyDownEventHandler)
                RemoveHandler baseTextBox.PreviewKeyDown, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As PreviewKeyDownEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event QueryAccessibilityHelp As QueryAccessibilityHelpEventHandler
            AddHandler(ByVal value As QueryAccessibilityHelpEventHandler)
                AddHandler baseTextBox.QueryAccessibilityHelp, value
            End AddHandler
            RemoveHandler(ByVal value As QueryAccessibilityHelpEventHandler)
                RemoveHandler baseTextBox.QueryAccessibilityHelp, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As QueryAccessibilityHelpEventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event QueryContinueDrag As QueryContinueDragEventHandler
            AddHandler(ByVal value As QueryContinueDragEventHandler)
                AddHandler baseTextBox.QueryContinueDrag, value
            End AddHandler
            RemoveHandler(ByVal value As QueryContinueDragEventHandler)
                RemoveHandler baseTextBox.QueryContinueDrag, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As QueryContinueDragEventArgs)
            End RaiseEvent
        End Event

        Public Custom Event ReadOnlyChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.ReadOnlyChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.ReadOnlyChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event RegionChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.RegionChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.RegionChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Resize As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Resize, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Resize, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event RightToLeftChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.RightToLeftChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.RightToLeftChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event SizeChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.SizeChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.SizeChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event StyleChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.StyleChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.StyleChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event SystemColorsChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.SystemColorsChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.SystemColorsChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event TabIndexChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.TabIndexChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.TabIndexChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event TabStopChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.TabStopChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.TabStopChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Custom Event TextAlignChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.TextAlignChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.TextAlignChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event TextChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.TextChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.TextChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Validated As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.Validated, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.Validated, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event

        Public Shadows Custom Event Validating As CancelEventHandler
            AddHandler(ByVal value As CancelEventHandler)
                AddHandler baseTextBox.Validating, value
            End AddHandler
            RemoveHandler(ByVal value As CancelEventHandler)
                RemoveHandler baseTextBox.Validating, value
            End RemoveHandler

            RaiseEvent(sender As Object, args As CancelEventArgs)

            End RaiseEvent
        End Event

        Public Shadows Custom Event VisibleChanged As EventHandler
            AddHandler(ByVal value As EventHandler)
                AddHandler baseTextBox.VisibleChanged, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler baseTextBox.VisibleChanged, value
            End RemoveHandler
            RaiseEvent(sender As Object, args As EventArgs)
            End RaiseEvent
        End Event
#End Region

        Private ReadOnly animationManager As AnimationManager

        Private ReadOnly baseTextBox As __baseTextBox
        Public Sub New()
            SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.DoubleBuffer, True)

            animationManager = New AnimationManager() With {
             .Increment = 0.06,
             .AnimationType = AnimationType.EaseInOut,
             .InterruptAnimation = False
            }
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()

            baseTextBox = New __baseTextBox() With {
             .BorderStyle = BorderStyle.None,
             .Font = SkinManager.ROBOTO_REGULAR_11,
             .ForeColor = SkinManager.GetPrimaryTextColor(),
             .Location = New Point(0, 0),
             .Width = Width,
             .Height = Height - 5
            }

            If Not Controls.Contains(baseTextBox) AndAlso Not DesignMode Then
                Controls.Add(baseTextBox)
            End If

            AddHandler baseTextBox.GotFocus, Sub(sender, args) animationManager.StartNewAnimation(AnimationDirection.[In])
            AddHandler baseTextBox.LostFocus, Sub(sender, args) animationManager.StartNewAnimation(AnimationDirection.Out)
            AddHandler BackColorChanged, Sub(sender, args)
                                             baseTextBox.BackColor = BackColor
                                             baseTextBox.ForeColor = SkinManager.GetPrimaryTextColor()
                                         End Sub
            'Fix for tabstop
            baseTextBox.TabStop = True
            Me.TabStop = False
        End Sub

        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            Dim g = pevent.Graphics
            g.Clear(Parent.BackColor)

            Dim lineY As Integer = baseTextBox.Bottom + 3

            If Not animationManager.IsAnimating() Then
                'No animation
                g.FillRectangle(If(baseTextBox.Focused, SkinManager.ColorScheme.PrimaryBrush, SkinManager.GetDividersBrush()), baseTextBox.Location.X, lineY, baseTextBox.Width, If(baseTextBox.Focused, 2, 1))
            Else
                'Animate
                Dim animationWidth As Integer = CInt(Math.Truncate(baseTextBox.Width * animationManager.GetProgress()))
                Dim halfAnimationWidth As Integer = animationWidth \ 2
                Dim animationStart As Integer = baseTextBox.Location.X + baseTextBox.Width \ 2

                'Unfocused background
                g.FillRectangle(SkinManager.GetDividersBrush(), baseTextBox.Location.X, lineY, baseTextBox.Width, 1)

                'Animated focus transition
                g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, animationStart - halfAnimationWidth, lineY, animationWidth, 2)
            End If
        End Sub

        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)

            baseTextBox.Location = New Point(0, 0)
            baseTextBox.Width = Width

            Height = baseTextBox.Height + 5
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            baseTextBox.BackColor = Parent.BackColor
            baseTextBox.ForeColor = SkinManager.GetPrimaryTextColor()
        End Sub

        Private Class __baseTextBox
            Inherits TextBox
            <DllImport("user32.dll", CharSet:=CharSet.Auto)>
            Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As Integer, lParam As String) As IntPtr
            End Function

            Private Const EM_SETCUEBANNER As Integer = &H1501
            Private Const EmptyChar As Char = ChrW(0)
            Private Const VisualStylePasswordChar As Char = "●"c
            Private Const NonVisualStylePasswordChar As Char = "*"c

            Private m_hint As String = String.Empty
            Public Property Hint() As String
                Get
                    Return m_hint
                End Get
                Set
                    m_hint = Value
                    SendMessage(Handle, EM_SETCUEBANNER, CInt(IntPtr.Zero), Hint)
                End Set
            End Property

            Private m_passwordChar As Char = EmptyChar
            Public Shadows Property PasswordChar() As Char
                Get
                    Return m_passwordChar
                End Get
                Set
                    m_passwordChar = Value
                    SetBasePasswordChar()
                End Set
            End Property

            Public Shadows Sub SelectAll()
                BeginInvoke(DirectCast(Sub()
                                           MyBase.Focus()
                                           MyBase.SelectAll()

                                       End Sub, MethodInvoker))
            End Sub


            Private m_useSystemPasswordChar As Char = EmptyChar
            Public Shadows Property UseSystemPasswordChar() As Boolean
                Get
                    Return m_useSystemPasswordChar <> EmptyChar
                End Get
                Set
                    If Value Then
                        m_useSystemPasswordChar = If(Application.RenderWithVisualStyles, VisualStylePasswordChar, NonVisualStylePasswordChar)
                    Else
                        m_useSystemPasswordChar = EmptyChar
                    End If

                    SetBasePasswordChar()
                End Set
            End Property

            Private Sub SetBasePasswordChar()
                MyBase.PasswordChar = If(UseSystemPasswordChar, m_useSystemPasswordChar, m_passwordChar)
            End Sub

            Public Sub New()
                Dim cms As MaterialContextMenuStrip = New TextBoxContextMenuStrip()
                AddHandler cms.Opening, AddressOf ContextMenuStripOnOpening
                AddHandler cms.OnItemClickStart, AddressOf ContextMenuStripOnItemClickStart

                ContextMenuStrip = cms
            End Sub

            Private Sub ContextMenuStripOnItemClickStart(sender As Object, toolStripItemClickedEventArgs As ToolStripItemClickedEventArgs)
                Select Case toolStripItemClickedEventArgs.ClickedItem.Text
                    Case "Undo"
                        Undo()
                        Exit Select
                    Case "Cut"
                        Cut()
                        Exit Select
                    Case "Copy"
                        Copy()
                        Exit Select
                    Case "Paste"
                        Paste()
                        Exit Select
                    Case "Delete"
                        SelectedText = String.Empty
                        Exit Select
                    Case "Select All"
                        SelectAll()
                        Exit Select
                End Select
            End Sub

            Private Sub ContextMenuStripOnOpening(sender As Object, cancelEventArgs As CancelEventArgs)
                Dim strip = TryCast(sender, TextBoxContextMenuStrip)
                If strip IsNot Nothing Then
                    strip.undo.Enabled = CanUndo
                    strip.cut.Enabled = Not String.IsNullOrEmpty(SelectedText)
                    strip.copy.Enabled = Not String.IsNullOrEmpty(SelectedText)
                    strip.paste.Enabled = Clipboard.ContainsText()
                    strip.delete.Enabled = Not String.IsNullOrEmpty(SelectedText)
                    strip.selectAll.Enabled = Not String.IsNullOrEmpty(Text)
                End If
            End Sub
        End Class

        Private Class TextBoxContextMenuStrip
            Inherits MaterialContextMenuStrip
            Public ReadOnly undo As ToolStripItem = New MaterialToolStripMenuItem() With {
             .Text = "Undo"
            }
            Public ReadOnly seperator1 As ToolStripItem = New ToolStripSeparator()
            Public ReadOnly cut As ToolStripItem = New MaterialToolStripMenuItem() With {
             .Text = "Cut"
            }
            Public ReadOnly copy As ToolStripItem = New MaterialToolStripMenuItem() With {
             .Text = "Copy"
            }
            Public ReadOnly paste As ToolStripItem = New MaterialToolStripMenuItem() With {
             .Text = "Paste"
            }
            Public ReadOnly delete As ToolStripItem = New MaterialToolStripMenuItem() With {
             .Text = "Delete"
            }
            Public ReadOnly seperator2 As ToolStripItem = New ToolStripSeparator()
            Public ReadOnly selectAll As ToolStripItem = New MaterialToolStripMenuItem() With {
             .Text = "Select All"
            }

            Public Sub New()
                Items.AddRange({undo, seperator1, cut, copy, paste, delete,
                    seperator2, selectAll})
            End Sub
        End Class
    End Class
End Namespace
