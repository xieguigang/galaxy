Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class ShadowForm

    <DllImport("dwmapi.dll")>
    Public Shared Function DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByRef pMarInset As MARGINS) As Integer
    End Function

    <DllImport("dwmapi.dll")>
    Public Shared Function DwmSetWindowAttribute(ByVal hwnd As IntPtr, ByVal attr As Integer, ByRef attrValue As Integer, ByVal attrSize As Integer) As Integer
    End Function

    <DllImport("dwmapi.dll")>
    Public Shared Function DwmIsCompositionEnabled(ByRef pfEnabled As Integer) As Integer
    End Function

    ' 定义边距结构
    Public Structure MARGINS
        Public leftWidth As Integer
        Public rightWidth As Integer
        Public topHeight As Integer
        Public bottomHeight As Integer
    End Structure

    ' 检查Aero效果是否启用
    Public Shared Function CheckAeroEnabled() As Boolean
        If Environment.OSVersion.Version.Major >= 6 Then
            Dim enabled As Integer = 0
            DwmIsCompositionEnabled(enabled)
            Return (enabled = 1)
        End If
        Return False
    End Function

    Public Shared Sub SetFormShadow(form As Form)
        ' 1. 设置窗体为无边框
        form.FormBorderStyle = FormBorderStyle.None

        ' 2. 检查系统支持
        If CheckAeroEnabled() Then
            ' 3. 设置窗口属性以启用黑暗模式等（可选，但有助于阴影显示）
            Dim attrValue As Integer = 1 ' 例如，1可能代表启用黑暗模式
            DwmSetWindowAttribute(form.Handle, 20, attrValue, 4)

            ' 4. 扩展窗口框架到客户区，创建阴影
            Dim margins As MARGINS
            With margins
                .leftWidth = 10   ' 左侧阴影宽度
                .rightWidth = 10  ' 右侧阴影宽度
                .topHeight = 10   ' 顶部阴影宽度
                .bottomHeight = 10 ' 底部阴影宽度
            End With
            DwmExtendFrameIntoClientArea(form.Handle, margins)
        End If
    End Sub

    Public Shared Sub WndProc(form As Form, ByRef m As Message)
        ' MyBase.WndProc(m)

        Const WM_NCHITTEST As Integer = &H84
        Const HTCLIENT As Integer = 1
        Const HTCAPTION As Integer = 2

        If m.Msg = WM_NCHITTEST AndAlso CInt(m.Result) = HTCLIENT Then
            m.Result = CType(HTCAPTION, IntPtr)
        End If
    End Sub
End Class