#Region "Microsoft.VisualBasic::379e050c44f144a03cc35ae8108903a3, mzkit\src\mzkit\mzkit\pages\dockWindow\tools\PropertyWindow.vb"

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

'   Total Lines: 26
'    Code Lines: 20
' Comment Lines: 1
'   Blank Lines: 5
'     File Size: 777.00 B


'     Class PropertyWindow
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: getPropertyObject
' 
'         Sub: DummyPropertyWindow_Closing
' 
' 
' /********************************************************************************/

#End Region

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace DockDocument.Presets

    Partial Public Class PropertyWindow : Inherits ToolWindow

        Public Sub New()
            Call InitializeComponent()
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetPropertyObject() As Object
            Return propertyGrid.SelectedObject
        End Function

        ''' <summary>
        ''' set selected object and then refresh the <see cref="propertyGrid"/>
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="expands">
        ''' a list of the property name that should be in expaned status by default.
        ''' </param>
        Public Sub SetObject(obj As Object, ParamArray expands As String())
            propertyGrid.SelectedObject = obj
            propertyGrid.Refresh()

            If Not expands Is Nothing Then
                For Each name As String In expands
                    Call ExpandPropertyInGrid(propertyGrid, name)
                Next
            End If
        End Sub

        Private Sub PropertyWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub

        Private Sub PropertyWindow_Load(sender As Object, e As EventArgs) Handles Me.Load
            DoubleBuffered = True
            TabText = CommonStrings.GetString("property_window")
        End Sub

        ''' <summary>
        ''' 使用反射展开 PropertyGrid 中的指定属性。
        ''' </summary>
        ''' <param name="grid">目标 PropertyGrid 控件。</param>
        ''' <param name="propertyName">要展开的属性的名称（不区分大小写）。</param>
        Public Shared Sub ExpandPropertyInGrid(grid As PropertyGrid, propertyName As String)
            If grid Is Nothing OrElse String.IsNullOrWhiteSpace(propertyName) Then Return
            If grid.SelectedObject Is Nothing Then Return

            Try
                Call ExpandPropertyInGridInternal(grid, propertyName)
            Catch ex As Exception
                ' 反射操作可能会因为.NET版本更新而失败，这里进行捕获以避免程序崩溃
                System.Diagnostics.Debug.WriteLine($"展开属性时出错: {ex.Message}")
            End Try
        End Sub

        Private Shared Sub ExpandPropertyInGridInternal(grid As PropertyGrid, propertyName As String)
            ' 1. 获取 PropertyGrid 的内部 PropertyGridView 对象
            ' 这个内部对象是真正负责绘制和管理的类
            Dim gridViewField = GetType(PropertyGrid).GetField("gridView", BindingFlags.NonPublic Or BindingFlags.Instance)
            If gridViewField Is Nothing Then Return ' 兼容性检查
            Dim gridView = gridViewField.GetValue(grid)

            ' 2. 获取 PropertyGridView 的所有行
            Dim rowsProperty = gridView.GetType().GetProperty("Rows", BindingFlags.NonPublic Or BindingFlags.Instance)
            If rowsProperty Is Nothing Then Return
            Dim gridEntries = rowsProperty.GetValue(gridView)

            ' 3. 遍历所有行，找到目标属性
            If TypeOf gridEntries Is IEnumerable Then
                For Each entry As Object In CType(gridEntries, IEnumerable)
                    ' 检查 entry 是否有效，并获取其标签
                    If entry IsNot Nothing Then
                        Dim labelProperty = entry.GetType().GetProperty("Label", BindingFlags.Public Or BindingFlags.Instance)
                        If labelProperty IsNot Nothing Then
                            Dim label = CStr(labelProperty.GetValue(entry))

                            ' 找到匹配的属性名（不区分大小写）
                            If String.Equals(label, propertyName, StringComparison.OrdinalIgnoreCase) Then
                                ' 4. 找到后，调用其内部的 Expand() 方法
                                Dim expandMethod = entry.GetType().GetMethod("Expand", BindingFlags.Public Or BindingFlags.Instance)
                                expandMethod?.Invoke(entry, Nothing)
                                Exit For ' 找到后就退出循环
                            End If
                        End If
                    End If
                Next
            End If
        End Sub
    End Class
End Namespace
