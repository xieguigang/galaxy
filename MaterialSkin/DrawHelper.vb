Imports System.Drawing
Imports System.Drawing.Drawing2D

NotInheritable Class DrawHelper
	Private Sub New()
	End Sub
	Public Shared Function CreateRoundRect(x As Single, y As Single, width As Single, height As Single, radius As Single) As GraphicsPath
		Dim gp As New GraphicsPath()
		gp.AddLine(x + radius, y, x + width - (radius * 2), y)
		gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90)
		gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2))
		gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90)
		gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height)
		gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90)
		gp.AddLine(x, y + height - (radius * 2), x, y + radius)
		gp.AddArc(x, y, radius * 2, radius * 2, 180, 90)
		gp.CloseFigure()
		Return gp
	End Function

	Public Shared Function CreateRoundRect(rect As Rectangle, radius As Single) As GraphicsPath
		Return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius)
	End Function

	Public Shared Function BlendColor(backgroundColor As Color, frontColor As Color, blend As Double) As Color
		Dim ratio As Double = blend / 255.0
		Dim invRatio As Double = 1.0 - ratio
		Dim r As Integer = CInt(Math.Truncate((backgroundColor.R * invRatio) + (frontColor.R * ratio)))
		Dim g As Integer = CInt(Math.Truncate((backgroundColor.G * invRatio) + (frontColor.G * ratio)))
		Dim b As Integer = CInt(Math.Truncate((backgroundColor.B * invRatio) + (frontColor.B * ratio)))
		Return Color.FromArgb(r, g, b)
	End Function

	Public Shared Function BlendColor(backgroundColor As Color, frontColor As Color) As Color
		Return BlendColor(backgroundColor, frontColor, frontColor.A)
	End Function
End Class
