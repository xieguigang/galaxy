#Region "Microsoft.VisualBasic::faf2c69a212d48cb3bec21da3761d262, mzkit\src\mzkit\mzkit\application\TaskbarWindow.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 97
'    Code Lines: 59
' Comment Lines: 19
'   Blank Lines: 19
'     File Size: 4.08 KB


' Module TaskBarWindow
' 
'     Function: GetTabPages
' 
'     Sub: preview_TabbedThumbnailActivated, preview_TabbedThumbnailClosed, preview_TabbedThumbnailMaximized, preview_TabbedThumbnailMinimized, UpdatePreviewBitmap
' 
' /********************************************************************************/

#End Region

Imports Microsoft.Windows.Taskbar

Module TaskBarWindow

    Private Function GetTabPages() As DocumentWindow()
        Return Workbench.AppHost.DockPanel.Documents _
            .Where(Function(page) TypeOf page Is DocumentWindow) _
            .Select(Function(doc) DirectCast(doc, DocumentWindow)) _
            .ToArray
    End Function

    Friend Sub preview_TabbedThumbnailActivated(sender As Object, e As TabbedThumbnailEventArgs)
        ' User selected a tab via the thumbnail preview
        ' Select the corresponding control in our app
        For Each page As DocumentWindow In GetTabPages()
            If page.Handle = e.WindowHandle Then
                ' Select the tab in the application UI as well as taskbar tabbed thumbnail list
                page.Show(Workbench.AppHost.DockPanel)
                TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(page.preview)
            End If
        Next

        ' Also activate our parent form (incase we are minimized, this will restore it)
        If Workbench.AppHost.WindowState = FormWindowState.Minimized Then
            Workbench.AppHost.WindowState = FormWindowState.Normal
        End If
    End Sub

    Friend Sub preview_TabbedThumbnailClosed(sender As Object, e As TabbedThumbnailClosedEventArgs)
        Dim pageClosed As DocumentWindow = Nothing

        ' Find the tabpage that was "closed" by the user (via the taskbar tabbed thumbnail)
        For Each page As DocumentWindow In GetTabPages()
            If page.Handle = e.WindowHandle Then
                pageClosed = page
                Exit For
            End If
        Next

        If pageClosed IsNot Nothing Then
            ' Remove the event handlers
            ' Dispose the tab
            pageClosed.Close()
        End If

        Dim tabbedThumbnail As TabbedThumbnail = TryCast(sender, TabbedThumbnail)
        If tabbedThumbnail IsNot Nothing Then
            ' Remove the event handlers from the tab preview
            RemoveHandler tabbedThumbnail.TabbedThumbnailActivated, AddressOf preview_TabbedThumbnailActivated
            RemoveHandler tabbedThumbnail.TabbedThumbnailClosed, AddressOf preview_TabbedThumbnailClosed
            RemoveHandler tabbedThumbnail.TabbedThumbnailMaximized, AddressOf preview_TabbedThumbnailMaximized
            RemoveHandler tabbedThumbnail.TabbedThumbnailMinimized, AddressOf preview_TabbedThumbnailMinimized
        End If
    End Sub

    Friend Sub preview_TabbedThumbnailMaximized(sender As Object, e As TabbedThumbnailEventArgs)
        ' User clicked on the maximize button on the thumbnail's context menu
        ' Maximize the app
        Workbench.AppHost.WindowState = FormWindowState.Maximized

        ' If there is a selected tab, take it's screenshot
        ' invalidate the tab's thumbnail
        ' update the "preview" object with the new thumbnail
        If Workbench.AppHost.DockPanel.ActiveDocument IsNot Nothing Then
            UpdatePreviewBitmap(Workbench.AppHost.DockPanel.ActiveDocument)
        End If
    End Sub

    Friend Sub preview_TabbedThumbnailMinimized(sender As Object, e As TabbedThumbnailEventArgs)
        ' User clicked on the minimize button on the thumbnail's context menu
        ' Minimize the app
        Workbench.AppHost.WindowState = FormWindowState.Minimized
    End Sub

    ''' <summary>
    ''' Helper method to update the thumbnail preview for a given tab page.
    ''' </summary>
    ''' <param name="tabPage"></param>
    Friend Sub UpdatePreviewBitmap(tabPage As DocumentWindow)
        If tabPage IsNot Nothing Then
            Dim preview As TabbedThumbnail = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(tabPage)

            If preview IsNot Nothing Then
                Dim bitmap As Bitmap = TabbedThumbnailScreenCapture.GrabWindowBitmap(tabPage.Handle, tabPage.Size)
                preview.SetImage(bitmap)
            End If
        End If
    End Sub
End Module
