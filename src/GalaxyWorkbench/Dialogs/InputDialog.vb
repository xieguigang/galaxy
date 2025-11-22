#Region "Microsoft.VisualBasic::2a2051f5d5cfcae3f58f6f8ceb194065, mzkit\src\mzkit\mzkit\forms\Inputs\InputDialog.vb"

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

'   Total Lines: 19
'    Code Lines: 15
' Comment Lines: 0
'   Blank Lines: 4
'     File Size: 731.00 B


' Class InputDialog
' 
'     Sub: Input
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace CommonDialogs

    Public Class InputDialog

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Input(setConfig As Action(Of InputDialog), Optional cancel As Action = Nothing)
            Call Input(setConfig, cancel, config:=Me)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <typeparam name="Form"></typeparam>
        ''' <param name="setConfig">
        ''' Just needs for display the information, then 
        ''' this delegate function could be nothing.
        ''' </param>
        ''' <param name="cancel"></param>
        ''' <param name="config"></param>
        Public Shared Function Input(Of Form As {New, InputDialog})(
            Optional setConfig As Action(Of Form) = Nothing,
            Optional cancel As Action = Nothing,
            Optional config As Form = Nothing
        ) As Form

            If CommonRuntime.AppHost Is Nothing Then
                Throw New NullReferenceException("the required workbench windows main form is nothing!")
            End If

            Dim getConfig As Form = If(config, New Form)
            Dim mask As MaskForm = MaskForm.CreateMask(CommonRuntime.AppHost)

            If mask.ShowDialogForm(getConfig) = DialogResult.OK Then
                If Not setConfig Is Nothing Then
                    Call setConfig(getConfig)
                End If
            ElseIf Not cancel Is Nothing Then
                Call cancel()
            End If

            Return getConfig
        End Function

        ''' <summary>
        ''' just used this method for display some information
        ''' </summary>
        ''' <typeparam name="Form"></typeparam>
        ''' <param name="config"></param>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overloads Shared Sub ShowDialog(Of Form As {New, InputDialog})(Optional config As Form = Nothing)
            Call Input(Nothing, Nothing, config:=If(config, New Form))
        End Sub

        Public Shared Function OpenDialog(Of Form As {New, InputDialog})(Optional initialData As Object = Nothing, Optional config As Form = Nothing) As DialogPromise
            Return DialogPromise.OpenDialog(Of Form)(initialData)
        End Function
    End Class
End Namespace