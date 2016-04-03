Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace Controls
	Public Class MaterialListView
		Inherits ListView
		Implements IMaterialControl
		<Browsable(False)> _
		Public Property Depth() As Integer Implements IMaterialControl.Depth
			Get
				Return m_Depth
			End Get
			Set
				m_Depth = Value
			End Set
		End Property
		Private m_Depth As Integer
		<Browsable(False)> _
		Public ReadOnly Property SkinManager() As MaterialSkinManager Implements IMaterialControl.SkinManager
			Get
				Return MaterialSkinManager.Instance
			End Get
		End Property
		<Browsable(False)> _
		Public Property MouseState() As MouseState Implements IMaterialControl.MouseState
			Get
				Return m_MouseState
			End Get
			Set
				m_MouseState = Value
			End Set
		End Property
		Private m_MouseState As MouseState
		<Browsable(False)> _
		Public Property MouseLocation() As Point
			Get
				Return m_MouseLocation
			End Get
			Set
				m_MouseLocation = Value
			End Set
		End Property
		Private m_MouseLocation As Point

		Public Sub New()
			GridLines = False
			FullRowSelect = True
			HeaderStyle = ColumnHeaderStyle.Nonclickable
			View = View.Details
			OwnerDraw = True
			ResizeRedraw = True
			BorderStyle = BorderStyle.None
			SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.OptimizedDoubleBuffer, True)

			'Fix for hovers, by default it doesn't redraw
			'TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
			MouseLocation = New Point(-1, -1)
			MouseState = MouseState.OUT
			AddHandler MouseEnter, Sub() MouseState = MouseState.HOVER
			AddHandler MouseLeave, Sub() 
			MouseState = MouseState.OUT
			MouseLocation = New Point(-1, -1)
			Invalidate()

End Sub
			AddHandler MouseDown, Sub() MouseState = MouseState.DOWN
			AddHandler MouseUp, Sub() MouseState = MouseState.HOVER
			AddHandler MouseMove, Sub(sender As Object, args As MouseEventArgs) 
			MouseLocation = args.Location
			Invalidate()

End Sub
		End Sub

		Protected Overrides Sub OnDrawColumnHeader(e As DrawListViewColumnHeaderEventArgs)
			e.Graphics.FillRectangle(New SolidBrush(SkinManager.GetApplicationBackgroundColor()), New Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height))
			e.Graphics.DrawString(e.Header.Text, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetSecondaryTextBrush(), New Rectangle(e.Bounds.X + ITEM_PADDING, e.Bounds.Y + ITEM_PADDING, e.Bounds.Width - ITEM_PADDING * 2, e.Bounds.Height - ITEM_PADDING * 2), getStringFormat())
		End Sub

		Private Const ITEM_PADDING As Integer = 12
		Protected Overrides Sub OnDrawItem(e As DrawListViewItemEventArgs)
			'We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
			Dim b = New Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height)
			Dim g = Graphics.FromImage(b)

			'always draw default background
			g.FillRectangle(New SolidBrush(SkinManager.GetApplicationBackgroundColor()), New Rectangle(New Point(e.Bounds.X, 0), e.Bounds.Size))

			If e.State.HasFlag(ListViewItemStates.Selected) Then
				'selected background
				g.FillRectangle(SkinManager.GetFlatButtonPressedBackgroundBrush(), New Rectangle(New Point(e.Bounds.X, 0), e.Bounds.Size))
			ElseIf e.Bounds.Contains(MouseLocation) AndAlso MouseState = MouseState.HOVER Then
				'hover background
				g.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(), New Rectangle(New Point(e.Bounds.X, 0), e.Bounds.Size))
			End If


			'Draw separator
			g.DrawLine(New Pen(SkinManager.GetDividersColor()), e.Bounds.Left, 0, e.Bounds.Right, 0)

			For Each subItem As ListViewItem.ListViewSubItem In e.Item.SubItems
				'Draw text
				g.DrawString(subItem.Text, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), New Rectangle(subItem.Bounds.Location.X + ITEM_PADDING, ITEM_PADDING, subItem.Bounds.Width - 2 * ITEM_PADDING, subItem.Bounds.Height - 2 * ITEM_PADDING), getStringFormat())
			Next

			e.Graphics.DrawImage(DirectCast(b.Clone(), Image), e.Item.Bounds.Location)
			g.Dispose()
			b.Dispose()
		End Sub

		Private Function getStringFormat() As StringFormat
            Return New StringFormat() With {
             .FormatFlags = StringFormatFlags.LineLimit,
             .Trimming = StringTrimming.EllipsisCharacter,
             .Alignment = StringAlignment.Near,
             .LineAlignment = StringAlignment.Center
            }
        End Function

		Protected Overrides Sub OnCreateControl()
			MyBase.OnCreateControl()

			'This is a hax for the needed padding.
			'Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
			'This will do for now.
			Font = New Font(SkinManager.ROBOTO_MEDIUM_12.FontFamily, 24)
		End Sub
	End Class
End Namespace
