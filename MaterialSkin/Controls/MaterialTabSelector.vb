Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports MaterialSkin.Animations

Namespace Controls
	Public Class MaterialTabSelector
		Inherits Control
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

		Private m_baseTabControl As MaterialTabControl
        Public Property BaseTabControl() As MaterialTabControl
            Get
                Return m_baseTabControl
            End Get
            Set
                m_baseTabControl = Value
                If m_baseTabControl Is Nothing Then
                    Return
                End If
                previousSelectedTabIndex = m_baseTabControl.SelectedIndex
                AddHandler m_baseTabControl.Deselected, Sub(sender, args)
                                                            previousSelectedTabIndex = m_baseTabControl.SelectedIndex

                                                        End Sub
                AddHandler m_baseTabControl.SelectedIndexChanged, Sub(sender, args)
                                                                      animationManager.SetProgress(0)
                                                                      animationManager.StartNewAnimation(AnimationDirection.[In])

                                                                  End Sub
                AddHandler m_baseTabControl.ControlAdded, Sub() Invalidate()
                AddHandler m_baseTabControl.ControlRemoved, Sub() Invalidate()
            End Set
        End Property

        Private previousSelectedTabIndex As Integer
		Private animationSource As Point
		Private ReadOnly animationManager As AnimationManager

		Private tabRects As List(Of Rectangle)
		Private Const TAB_HEADER_PADDING As Integer = 24
		Private Const TAB_INDICATOR_HEIGHT As Integer = 2

		Public Sub New()
			SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.OptimizedDoubleBuffer, True)
			Height = 48

			animationManager = New AnimationManager() With { _
			 .AnimationType = AnimationType.EaseOut, _
			 .Increment = 0.04 _
			}
            AddHandler animationManager.OnAnimationProgress, Sub(sender) Invalidate()
        End Sub

		Protected Overrides Sub OnPaint(e As PaintEventArgs)
			Dim g = e.Graphics
			g.TextRenderingHint = TextRenderingHint.AntiAlias

			g.Clear(SkinManager.ColorScheme.PrimaryColor)

			If m_baseTabControl Is Nothing Then
				Return
			End If

			If Not animationManager.IsAnimating() OrElse tabRects Is Nothing OrElse tabRects.Count <> m_baseTabControl.TabCount Then
				UpdateTabRects()
			End If

			Dim animationProgress As Double = animationManager.GetProgress()

			'Click feedback
			If animationManager.IsAnimating() Then
				Dim rippleBrush = New SolidBrush(Color.FromArgb(CInt(Math.Truncate(51 - (animationProgress * 50))), Color.White))
				Dim rippleSize = CInt(Math.Truncate(animationProgress * tabRects(m_baseTabControl.SelectedIndex).Width * 1.75))

				g.SetClip(tabRects(m_baseTabControl.SelectedIndex))
				g.FillEllipse(rippleBrush, New Rectangle(animationSource.X - rippleSize \ 2, animationSource.Y - rippleSize \ 2, rippleSize, rippleSize))
				g.ResetClip()
				rippleBrush.Dispose()
			End If

			'Draw tab headers
			For Each tabPage As TabPage In m_baseTabControl.TabPages
				Dim currentTabIndex As Integer = m_baseTabControl.TabPages.IndexOf(tabPage)
				Dim textBrush As Brush = New SolidBrush(Color.FromArgb(CalculateTextAlpha(currentTabIndex, animationProgress), SkinManager.ColorScheme.TextColor))

				g.DrawString(tabPage.Text.ToUpper(), SkinManager.ROBOTO_MEDIUM_10, textBrush, tabRects(currentTabIndex), New StringFormat() With { _
				 .Alignment = StringAlignment.Center, _
				 .LineAlignment = StringAlignment.Center _
				})
				textBrush.Dispose()
			Next

			'Animate tab indicator
			Dim previousSelectedTabIndexIfHasOne As Integer = If(previousSelectedTabIndex = -1, m_baseTabControl.SelectedIndex, previousSelectedTabIndex)
			Dim previousActiveTabRect As Rectangle = tabRects(previousSelectedTabIndexIfHasOne)
			Dim activeTabPageRect As Rectangle = tabRects(m_baseTabControl.SelectedIndex)

			Dim y As Integer = activeTabPageRect.Bottom - 2
			Dim x As Integer = previousActiveTabRect.X + CInt(Math.Truncate((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress))
			Dim width As Integer = previousActiveTabRect.Width + CInt(Math.Truncate((activeTabPageRect.Width - previousActiveTabRect.Width) * animationProgress))

			g.FillRectangle(SkinManager.ColorScheme.AccentBrush, x, y, width, TAB_INDICATOR_HEIGHT)
		End Sub

		Private Function CalculateTextAlpha(tabIndex As Integer, animationProgress As Double) As Integer
			Dim primaryA As Integer = SkinManager.ACTION_BAR_TEXT.A
			Dim secondaryA As Integer = SkinManager.ACTION_BAR_TEXT_SECONDARY.A

			If tabIndex = m_baseTabControl.SelectedIndex AndAlso Not animationManager.IsAnimating() Then
				Return primaryA
			End If
			If tabIndex <> previousSelectedTabIndex AndAlso tabIndex <> m_baseTabControl.SelectedIndex Then
				Return secondaryA
			End If
			If tabIndex = previousSelectedTabIndex Then
				Return primaryA - CInt(Math.Truncate((primaryA - secondaryA) * animationProgress))
			End If
			Return secondaryA + CInt(Math.Truncate((primaryA - secondaryA) * animationProgress))
		End Function

		Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
			MyBase.OnMouseUp(e)

			If tabRects Is Nothing Then
				UpdateTabRects()
			End If
			For i As Integer = 0 To tabRects.Count - 1
				If tabRects(i).Contains(e.Location) Then
					m_baseTabControl.SelectedIndex = i
				End If
			Next

			animationSource = e.Location
		End Sub

		Private Sub UpdateTabRects()
			tabRects = New List(Of Rectangle)()

			'If there isn't a base tab control, the rects shouldn't be calculated
			'If there aren't tab pages in the base tab control, the list should just be empty which has been set already; exit the void
			If m_baseTabControl Is Nothing OrElse m_baseTabControl.TabCount = 0 Then
				Return
			End If

			'Calculate the bounds of each tab header specified in the base tab control
			Using b = New Bitmap(1, 1)
				Using g = Graphics.FromImage(b)
					tabRects.Add(New Rectangle(SkinManager.FORM_PADDING, 0, TAB_HEADER_PADDING * 2 + CInt(Math.Truncate(g.MeasureString(m_baseTabControl.TabPages(0).Text, SkinManager.ROBOTO_MEDIUM_10).Width)), Height))
					For i As Integer = 1 To m_baseTabControl.TabPages.Count - 1
						tabRects.Add(New Rectangle(tabRects(i - 1).Right, 0, TAB_HEADER_PADDING * 2 + CInt(Math.Truncate(g.MeasureString(m_baseTabControl.TabPages(i).Text, SkinManager.ROBOTO_MEDIUM_10).Width)), Height))
					Next
				End Using
			End Using
		End Sub
	End Class
End Namespace
