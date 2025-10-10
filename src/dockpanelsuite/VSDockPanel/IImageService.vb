Imports Microsoft.VisualBasic.Drawing
Imports Microsoft.VisualBasic.Emit.Marshal
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace Docking
    Public Interface IImageService
        ReadOnly Property Dockindicator_PaneDiamond As Bitmap
        ReadOnly Property Dockindicator_PaneDiamond_Fill As Bitmap
        ReadOnly Property Dockindicator_PaneDiamond_Hotspot As Bitmap
        ReadOnly Property DockIndicator_PaneDiamond_HotspotIndex As Bitmap
        ReadOnly Property DockIndicator_PanelBottom As Image
        ReadOnly Property DockIndicator_PanelFill As Image
        ReadOnly Property DockIndicator_PanelLeft As Image
        ReadOnly Property DockIndicator_PanelRight As Image
        ReadOnly Property DockIndicator_PanelTop As Image
        ReadOnly Property DockPane_Close As Bitmap
        ReadOnly Property DockPane_List As Bitmap
        ReadOnly Property DockPane_Dock As Bitmap
        ReadOnly Property DockPaneActive_AutoHide As Bitmap
        ReadOnly Property DockPane_Option As Bitmap
        ReadOnly Property DockPane_OptionOverflow As Bitmap
        ReadOnly Property DockPaneActive_Close As Bitmap
        ReadOnly Property DockPaneActive_Dock As Bitmap
        ReadOnly Property DockPaneActive_Option As Bitmap
        ReadOnly Property DockPaneHover_Close As Bitmap
        ReadOnly Property DockPaneHover_List As Bitmap
        ReadOnly Property DockPaneHover_Dock As Bitmap
        ReadOnly Property DockPaneActiveHover_AutoHide As Bitmap
        ReadOnly Property DockPaneHover_Option As Bitmap
        ReadOnly Property DockPaneHover_OptionOverflow As Bitmap
        ReadOnly Property DockPanePress_Close As Bitmap
        ReadOnly Property DockPanePress_List As Bitmap
        ReadOnly Property DockPanePress_Dock As Bitmap
        ReadOnly Property DockPanePress_AutoHide As Bitmap
        ReadOnly Property DockPanePress_Option As Bitmap
        ReadOnly Property DockPanePress_OptionOverflow As Bitmap
        ReadOnly Property DockPaneActiveHover_Close As Bitmap
        ReadOnly Property DockPaneActiveHover_Dock As Bitmap
        ReadOnly Property DockPaneActiveHover_Option As Bitmap
        ReadOnly Property TabActive_Close As Image
        ReadOnly Property TabInactive_Close As Image
        ReadOnly Property TabLostFocus_Close As Image
        ReadOnly Property TabHoverActive_Close As Image
        ReadOnly Property TabHoverInactive_Close As Image
        ReadOnly Property TabHoverLostFocus_Close As Image
        ReadOnly Property TabPressActive_Close As Image
        ReadOnly Property TabPressInactive_Close As Image
        ReadOnly Property TabPressLostFocus_Close As Image
    End Interface

    Public Module ImageServiceHelper
        ''' <summary>
        ''' Gets images for tabs and captions.
        ''' </summary>
        ''' <param name="mask"></param>
        ''' <param name="glyph"></param>
        ''' <param name="background"></param>
        ''' <param name="border"></param>
        ''' <returns></returns>
        Public Function GetImage(mask As Bitmap, glyph As Color, background As Color, Optional border As Color? = Nothing) As Bitmap
            Dim width = mask.Width
            Dim height = mask.Height
            Dim input As Bitmap = New Bitmap(width, height)
            Using gfx = Graphics.FromImage(input)
                Dim brush As SolidBrush = New SolidBrush(glyph)
                gfx.FillRectangle(brush, 0, 0, width, height)
            End Using

            Dim output As Bitmap = New Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb)
            Dim rect = New Rectangle(0, 0, input.Width, input.Height)
            Dim bitsMask = bitmap32bit(mask).CreateBuffer
            Dim bitsInput = bitmap32bit(input).CreateBuffer
            Dim bitsOutput = output.CreateBuffer
            Dim maskBuf = bitsMask.RawBuffer
            Dim inputBuf = bitsInput.RawBuffer
            Dim outputBuf = bitsOutput.RawBuffer

            For y = 0 To input.Height - 1
                'byte* ptrMask = (byte*)bitsMask.Scan0 + y * bitsMask.Stride;
                'byte* ptrInput = (byte*)bitsInput.Scan0 + y * bitsInput.Stride;
                'byte* ptrOutput = (byte*)bitsOutput.Scan0 + y * bitsOutput.Stride;
                Dim ptrMask = New Span(Of Byte)(maskBuf, y * bitsMask.Stride, input.Width * 4)
                Dim ptrInput = New Span(Of Byte)(inputBuf, y * bitsInput.Stride, input.Width * 4)
                Dim ptrOutput = New Span(Of Byte)(outputBuf, y * bitsOutput.Stride, input.Width * 4)

                For x = 0 To input.Width - 1
                    ptrOutput(4 * x) = ptrInput(4 * x)           ' blue
                    ptrOutput(4 * x + 1) = ptrInput(4 * x + 1)   ' green
                    ptrOutput(4 * x + 2) = ptrInput(4 * x + 2)   ' red
                    ptrOutput(4 * x + 3) = ptrMask(4 * x)        ' alpha
                Next
            Next

            'mask.UnlockBits(bitsMask);
            'input.UnlockBits(bitsInput);
            'output.UnlockBits(bitsOutput);
            bitsMask.Dispose()
            bitsInput.Dispose()
            bitsOutput.Dispose()
            input.Dispose()

            If border Is Nothing Then
                border = background
            End If

            Dim back As Bitmap = New Bitmap(width, height)
            Using gfx = Graphics.FromImage(back)
                Dim brush As SolidBrush = New SolidBrush(background)
                Dim brush2 As SolidBrush = New SolidBrush(border.Value)
                gfx.FillRectangle(brush2, 0, 0, width, height)
                If background <> border.Value Then
                    gfx.FillRectangle(brush, 1, 1, width - 2, height - 2)
                End If

                gfx.DrawImageUnscaled(output, 0, 0)
            End Using

            output.Dispose()
            Return back
        End Function

        Public Function GetBackground(innerBorder As Color, outerBorder As Color, width As Integer, painting As IPaintingService) As Bitmap
            Dim back As Bitmap = New Bitmap(width, width)
            Using gfx = Graphics.FromImage(back)
                Dim brush = painting.GetBrush(innerBorder)
                Dim brush2 = painting.GetBrush(outerBorder)
                gfx.FillRectangle(brush2, 0, 0, width, width)
                gfx.FillRectangle(brush, 1, 1, width - 2, width - 2)
            End Using

            Return back
        End Function

        Public Function GetLayerImage(color As Color, width As Integer, painting As IPaintingService) As Bitmap
            Dim back As Bitmap = New Bitmap(width, width)
            Using gfx = Graphics.FromImage(back)
                Dim brush = painting.GetBrush(color)
                gfx.FillRectangle(brush, 0, 0, width, width)
            End Using

            Return back
        End Function

        ''' <summary>
        ''' Gets images for docking indicators.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDockIcon(maskArrow As Bitmap, layerArrow As Bitmap, maskWindow As Bitmap, layerWindow As Bitmap, maskBack As Bitmap, background As Color, painting As IPaintingService, Optional maskCore As Bitmap = Nothing, Optional layerCore As Bitmap = Nothing, Optional separator As Color? = Nothing) As Bitmap
            Dim width = maskBack.Width
            Dim height = maskBack.Height
            Dim rect = New Rectangle(0, 0, width, height)
            Dim arrowOut As Bitmap = Nothing

            If maskArrow IsNot Nothing Then
                Dim input = layerArrow
                arrowOut = MaskImages(input, maskArrow)
            End If

            Dim windowIn = layerWindow
            Dim windowOut = MaskImages(windowIn, maskWindow)

            Dim coreOut As Bitmap = Nothing
            If layerCore IsNot Nothing Then
                Dim coreIn = layerCore
                coreOut = MaskImages(coreIn, maskCore)
            End If

            Dim backIn As Bitmap = New Bitmap(width, height)
            Using gfx = Graphics.FromImage(backIn)
                Dim brush = painting.GetBrush(background)
                gfx.FillRectangle(brush, 0, 0, width, height)
                gfx.DrawImageUnscaled(windowOut, 0, 0)
                windowOut.Dispose()
                If layerCore IsNot Nothing Then
                    gfx.DrawImageUnscaled(coreOut, 0, 0)
                    coreOut.Dispose()
                End If

                If separator IsNot Nothing Then
                    Dim sep = painting.GetPen(separator.Value)
                    gfx.DrawRectangle(sep, 0, 0, width - 1, height - 1)
                End If
            End Using

            Dim backOut = MaskImages(backIn, maskBack)
            backIn.Dispose()

            Using gfx = Graphics.FromImage(backOut)
                If arrowOut IsNot Nothing Then
                    gfx.DrawImageUnscaled(arrowOut, 0, 0)
                    arrowOut.Dispose()
                End If
            End Using

            Return backOut
        End Function

        Public Function MaskImages(input As Bitmap, maskArrow As Bitmap) As Bitmap
            Dim width = input.Width
            Dim height = input.Height
            Dim rect = New Rectangle(0, 0, width, height)
            Dim arrowOut = New Bitmap(width, height, PixelFormat.Format32bppArgb)
            Dim bitsMask As BitmapBuffer = bitmap32bit(maskArrow).CreateBuffer
            Dim bitsInput As BitmapBuffer = bitmap32bit(input).CreateBuffer
            Dim bitsOutput As BitmapBuffer = arrowOut.CreateBuffer
            Dim maskBuf = bitsMask.RawBuffer
            Dim inputBuf = bitsInput.RawBuffer
            Dim outputBuf = bitsOutput.RawBuffer

            For y = 0 To height - 1
                'byte* ptrMask = (byte*)bitsMask.Scan0 + y * bitsMask.Stride;
                'byte* ptrInput = (byte*)bitsInput.Scan0 + y * bitsInput.Stride;
                'byte* ptrOutput = (byte*)bitsOutput.Scan0 + y * bitsOutput.Stride;
                Dim ptrMask = New Span(Of Byte)(maskBuf, y * bitsMask.Stride, bitsMask.Stride)
                Dim ptrInput = New Span(Of Byte)(inputBuf, y * bitsInput.Stride, bitsInput.Stride)
                Dim ptrOutput = New Span(Of Byte)(outputBuf, y * bitsOutput.Stride, bitsOutput.Stride)

                For x = 0 To width - 1
                    ptrOutput(4 * x) = ptrInput(4 * x)           ' blue
                    ptrOutput(4 * x + 1) = ptrInput(4 * x + 1)   ' green
                    ptrOutput(4 * x + 2) = ptrInput(4 * x + 2)   ' red
                    ptrOutput(4 * x + 3) = ptrMask(4 * x)        ' alpha
                Next
            Next

            bitsMask.Write()
            bitsInput.Write()
            bitsOutput.Write()

            maskArrow.UnlockBits(DirectCast(bitsMask.GetHandleObject, BitmapData))
            input.UnlockBits(DirectCast(bitsInput.GetHandleObject, BitmapData))
            arrowOut.UnlockBits(DirectCast(bitsOutput.GetHandleObject, BitmapData))

            Return arrowOut
        End Function

        Private Function bitmap32bit(ByRef x As Bitmap) As Bitmap
            Dim b32 = New Bitmap(x.Width, x.Height, PixelFormat.Format32bppArgb)

            Using gfx = Graphics.FromImage(b32)
                gfx.DrawImage(x, New Point())
            End Using

            x = b32

            Return b32
        End Function

        Public Function GetDockImage(icon As Bitmap, background As Bitmap) As Bitmap
            Dim result = New Bitmap(background)
            Dim offset As Single = CSng((background.Width - icon.Width) / 2)
            Using gfx = Graphics.FromImage(result)
                gfx.DrawImage(icon, offset, offset)
            End Using

            Return result
        End Function

        Public Function CombineFive(five As Bitmap, bottom As Bitmap, center As Bitmap, left As Bitmap, right As Bitmap, top As Bitmap) As Bitmap
            Dim result = New Bitmap(five)
            Dim cell As Integer = CInt((result.Width - bottom.Width) / 2)
            Dim offset As Integer = CInt((cell - bottom.Width) / 2.0)
            Using gfx = Graphics.FromImage(result)
                gfx.DrawImageUnscaled(top, cell, offset)
                gfx.DrawImageUnscaled(center, cell, cell)
                gfx.DrawImageUnscaled(bottom, cell, 2 * cell - offset)
                gfx.DrawImageUnscaled(left, offset, cell)
                gfx.DrawImageUnscaled(right, 2 * cell - offset, cell)
            End Using

            Return result
        End Function

        Public Function GetFiveBackground(mask As Bitmap, innerBorder As Color, outerBorder As Color, painting As IPaintingService) As Bitmap
            ' TODO: calculate points using functions.
            Using input = GetLayerImage(innerBorder, mask.Width, painting)
                Using gfx = Graphics.FromImage(input)
                    Dim pen = painting.GetPen(outerBorder)
                    gfx.DrawLines(pen, {New Point(36, 25), New Point(36, 0), New Point(75, 0), New Point(75, 25)})
                    gfx.DrawLines(pen, {New Point(86, 36), New Point(111, 36), New Point(111, 75), New Point(86, 75)})
                    gfx.DrawLines(pen, {New Point(75, 86), New Point(75, 111), New Point(36, 111), New Point(36, 86)})
                    gfx.DrawLines(pen, {New Point(25, 75), New Point(0, 75), New Point(0, 36), New Point(25, 36)})
                    Dim pen2 = painting.GetPen(outerBorder, 2)
                    gfx.DrawLine(pen2, New Point(36, 25), New Point(25, 36))
                    gfx.DrawLine(pen2, New Point(75, 25), New Point(86, 36))
                    gfx.DrawLine(pen2, New Point(86, 75), New Point(75, 86))
                    gfx.DrawLine(pen2, New Point(36, 86), New Point(25, 75))
                End Using

                Return MaskImages(input, mask)
            End Using
        End Function
    End Module

End Namespace
