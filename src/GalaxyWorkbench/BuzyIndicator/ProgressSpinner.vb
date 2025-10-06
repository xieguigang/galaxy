#Region "Microsoft.VisualBasic::6fef7c51950409b66f5caa6fcdb0a9ae, mzkit\src\mzkit\mzkit\forms\Task\frmProgressSpinner.vb"

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

'   Total Lines: 58
'    Code Lines: 39
' Comment Lines: 6
'   Blank Lines: 13
'     File Size: 1.91 KB


' Class frmProgressSpinner
' 
'     Sub: CloseWindow, DoLoading, frmProgressSpinner_Closed, frmProgressSpinner_Load, frmProgressSpinner_Paint
'          OnFrameChanged, RunAnimation
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Mzkit_win32.BasicMDIForm.CommonDialogs

Public Class ProgressSpinner

    Dim theImage As Image = My.Resources.spinner

    Public ReadOnly Property ex As Exception = Nothing

    <DebuggerStepThrough>
    Sub RunAnimation()
        ' the image Is set up for animation using the
        ' ImageAnimator class And an event handler
        ImageAnimator.Animate(theImage, New EventHandler(AddressOf OnFrameChanged))
    End Sub

    <DebuggerStepThrough>
    Private Sub OnFrameChanged(o As Object, e As EventArgs)
        Me.Invalidate()
        Application.DoEvents()
        ' BackgroundImage = theImage
    End Sub

    <DebuggerStepThrough>
    Private Sub frmProgressSpinner_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Dim g = e.Graphics

        'Get the next frame ready for rendering for the image in this thread.
        ImageAnimator.UpdateFrames()
        'Draw the current frame in the animation for all of the images
        g.DrawImageUnscaled(theImage, 0, 0, theImage.Width, theImage.Height)
    End Sub

    Private Sub frmProgressSpinner_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.TransparencyKey = Color.Green
        Me.BackColor = Color.Green
        Me.DoubleBuffered = False

        RunAnimation()
        TaskbarStatus.SetLoopStatus()
    End Sub

    Private Sub frmProgressSpinner_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        TaskbarStatus.Stop()
    End Sub

    ''' <summary>
    ''' run task and then close the spinner dialog
    ''' </summary>
    ''' <param name="loading"></param>
    ''' <remarks>
    ''' 这个函数会阻塞线程
    ''' </remarks>
    Public Shared Sub DoLoading(loading As Action, Optional host As Form = Nothing, Optional ByRef ex As Exception = Nothing)
        Dim spinner As New ProgressSpinner
        Dim mask As MaskForm = MaskForm.CreateMask(Workbench.AppHost)
        Dim task = getLoadingTask(loading, host, spinner)

        Call task.Start()
        Call mask.ShowDialogForm(spinner)

        ex = spinner.ex
    End Sub

    Private Shared Function getLoadingTask(loading As Action, host As Form, spinner As ProgressSpinner) As Tasks.Task
        Dim run As Action =
            Sub()
                Call Thread.Sleep(500)

                Try
                    If host Is Nothing Then
                        Call loading()
                    Else
                        Call host.Invoke(loading)
                    End If
                Catch ex As Exception
                    Call App.LogException(ex)
                    Call Workbench.Warning(ex.ToString)
                    Call Thread.Sleep(1000)

                    spinner._ex = ex
                End Try

                Call spinner.CloseWindow()
            End Sub

        Return New Task(run)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Friend Sub CloseWindow()
        Try
            Call Invoke(Sub() Call Close())
        Catch ex As Exception

        End Try
    End Sub
End Class
