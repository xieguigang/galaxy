
''' <summary>
''' Hyperlink simulator for <see cref="PropertyGrid"/>
''' </summary>
Public Class HyperlinkSimulator

    Dim WithEvents propertyGrid As PropertyGrid
    Dim linkActions As New Dictionary(Of String, Action)

    Sub New(propertyGrid As PropertyGrid)
        Me.propertyGrid = propertyGrid
    End Sub

    Public Sub AddLink(propertyName As String, onclick As Action)
        linkActions(propertyName) = onclick
    End Sub

    Private Sub propertyGrid_MouseClick(sender As Object, e As MouseEventArgs) Handles propertyGrid.MouseClick
        Dim clickedItem As GridItem = propertyGrid.SelectedGridItem

        If clickedItem IsNot Nothing AndAlso clickedItem.PropertyDescriptor IsNot Nothing Then
            ' 检查被点击的属性名是否是我们想要的那个
            Dim name As String = clickedItem.PropertyDescriptor.Name

            If linkActions.ContainsKey(name) Then
                ' 在这里执行你的超链接点击逻辑
                Call linkActions(name)()
            End If
        End If
    End Sub

    ' --- 增强用户体验：当鼠标悬停在链接上时显示手形光标 ---
    Private Sub PropertyGrid1_MouseMove(sender As Object, e As MouseEventArgs) Handles propertyGrid.MouseMove
        ' 获取鼠标当前位置下的 GridItem
        Dim hoveredItem As GridItem = propertyGrid.SelectedGridItem

        Dim needsHandCursor As Boolean = False

        ' 检查鼠标悬停的属性是否是我们的目标
        If hoveredItem IsNot Nothing AndAlso hoveredItem.PropertyDescriptor IsNot Nothing Then
            If linkActions.ContainsKey(hoveredItem.PropertyDescriptor.Name) Then
                needsHandCursor = True
            End If
        End If

        ' 根据是否需要来设置光标
        If needsHandCursor Then
            If propertyGrid.Cursor <> Cursors.Hand Then
                propertyGrid.Cursor = Cursors.Hand
            End If
        Else
            If propertyGrid.Cursor <> Cursors.Default Then
                propertyGrid.Cursor = Cursors.Default
            End If
        End If
    End Sub

    ' --- 当鼠标离开控件时，确保光标恢复默认 ---
    Private Sub PropertyGrid1_MouseLeave(sender As Object, e As EventArgs) Handles propertyGrid.MouseLeave
        propertyGrid.Cursor = Cursors.Default
    End Sub
End Class