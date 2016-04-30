Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language

Namespace PlugIns

    Public Module MenuAPI

        <Extension>
        Public Function AddCommand(menu As ToolStripMenuItem, menuPath As String, Name As String) As ToolStripMenuItem
            Dim pathValue As String() =
                LinqAPI.Exec(Of String) <= From s As String
                                           In menuPath.Split("\"c)
                                           Where Not String.IsNullOrEmpty(s)
                                           Select s
            Return AddCommand(New __addMenuHelper With {.menu = menu}, pathValue, Name, Scan0)
        End Function

        ''' <summary>
        ''' Recursive function for create the menu item for each plugin command.(递归的添加菜单项)
        ''' </summary>
        ''' <param name="menu"></param>
        ''' <param name="menuPath"></param>
        ''' <param name="Name"></param>
        ''' <param name="p"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AddCommand(menu As __addMenuHelper, menuPath As String(), Name As String, p As Integer) As ToolStripMenuItem
            If p = menuPath.Length Then
                Return menu.NewItem(Name)
            Else
                Dim LQuery As ToolStripMenuItem =
                    LinqAPI.DefaultFirst(Of ToolStripMenuItem) <=
                        From menuItem As ToolStripMenuItem
                        In menu.DropDownItems
                        Where String.Equals(menuItem.Text, menuPath(p))
                        Select menuItem '
                If LQuery Is Nothing Then
                    LQuery = menu.NewItem(menuPath(p))
                End If

                Return AddCommand(
                    New __addMenuHelper With {.menu = LQuery},
                    menuPath,
                    Name,
                    p + 1)
            End If
        End Function

        Private Structure __addMenuHelper
            Public menu As ToolStripMenuItem

            Public ReadOnly Property DropDownItems As IEnumerable(Of ToolStripMenuItem)
                Get
                    Return menu.DropDownItems
                End Get
            End Property

            Public Function NewItem(name As String) As ToolStripMenuItem
                Dim MenuItem As New ToolStripMenuItem() With {
                    .Text = name
                }
                Call menu.DropDownItems.Add(MenuItem)
                Return MenuItem
            End Function
        End Structure
    End Module
End Namespace