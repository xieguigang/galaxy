Imports Microsoft.VisualBasic.Linq
Imports Microsoft.Windows.Taskbar
Imports sys = System.Math

Public Class TabControl

    Public ReadOnly Property TabPages As IEnumerable(Of TabPage)
        Get
            Return __labels.ToArray(Function(x) x.TabPage)
        End Get
    End Property

    ReadOnly __labels As New List(Of TabLabel)

    Public Property TabLabelSkin As Gtk.CSSEngine.Models.VisualObject
    Public ReadOnly Property Current As TabLabel
    Public Property MaxWidth As Integer = 120

    Public Function Add(Name As String, page As TabPage) As TabLabel
        Dim avgWidth As Integer = If(__labels.Count = 0, MaxWidth, Width / (__labels.Count * 1.25))
        Dim label As New TabLabel(page, container:=Me) With {
            .Text = Name,
            .Height = FlowLayoutPanelLabelsContainer.Height - 2,
            .Width = sys.Min(MaxWidth, avgWidth)
        }

        For Each xlb As TabLabel In __labels
            xlb.Width = label.Width
        Next

        Call __labels.Add(label)
        Call PanelPagesContainer.Controls.Add(page)
        Call FlowLayoutPanelLabelsContainer.Controls.Add(label)

        page.Location = New Point
        page.Size = PanelPagesContainer.Size
        TabIndicator1.LabelWidth = label.Width
        Call __addThumbnail(page)

        Return label
    End Function

    Public Function Add(Name As String) As TabLabel
        Return Add(Name, New TabPage)
    End Function

    Public Overloads Sub [Select](label As TabLabel)
        If Not Current Is Nothing AndAlso Current.Equals(label) Then
            Return
        End If
        If Not Current Is Nothing Then
            Current.TabPage.Visible = False
        End If
        _Current = label
        _Current.TabPage.Visible = True
    End Sub

    Private Sub __addThumbnail(newTab As TabPage)
        ' Add thumbnail toolbar buttons
        Call TaskbarManager.Instance.ThumbnailToolBars.AddButtons(newTab.Handle)

        ' Add a new preview
        Dim preview As New TabbedThumbnail(Me.Handle, newTab.Handle)

        ' Event handlers for this preview
        ' AddHandler preview.TabbedThumbnailActivated, AddressOf preview_TabbedThumbnailActivated
        ' AddHandler preview.TabbedThumbnailClosed, AddressOf preview_TabbedThumbnailClosed
        ' AddHandler preview.TabbedThumbnailMaximized, AddressOf preview_TabbedThumbnailMaximized
        ' AddHandler preview.TabbedThumbnailMinimized, AddressOf preview_TabbedThumbnailMinimized

        TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(preview)

        ' Select the tab in the application UI as well as taskbar tabbed thumbnail list
        TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(newTab)
    End Sub
End Class
