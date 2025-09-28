Imports System.ComponentModel
Imports System.Drawing.Design

Namespace JSON

    ''' <summary>
    ''' https://github.com/Sesshoumaru/JsonViewer
    ''' </summary>
    Partial Public Class JsonViewer : Inherits UserControl

        Dim WithEvents viewer As JsonRender

        <Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))>
        Public Property Json As String
            Get
                If viewer Is Nothing Then
                    Return Nothing
                End If
                Return viewer.ToString
            End Get
            Set(value As String)
                If viewer IsNot Nothing Then
                    Call viewer.Render(jsonstr:=value)
                End If
            End Set
        End Property

        Public Property RootTag As String
            Get
                If viewer Is Nothing Then
                    Return Nothing
                End If
                Return viewer.RootTag
            End Get
            Set(value As String)
                If viewer IsNot Nothing Then
                    Call viewer.SetRootLabel(label:=value)
                End If
            End Set
        End Property

        Public Event FindAction(node As JsonViewerTreeNode)

        Sub New()
            Call InitializeComponent()

            viewer = New JsonRender(tvJson)
            viewer.Render("{}")
        End Sub

        Private Sub mnuExpandAll_Click(sender As Object, e As System.EventArgs) Handles mnuExpandAll.Click
            If Not viewer Is Nothing Then
                Call viewer.ExpandAll()
            End If
        End Sub

        Private Sub mnuFind_Click(sender As Object, e As System.EventArgs) Handles mnuFind.Click
            If Not viewer Is Nothing Then
                RaiseEvent FindAction(viewer.GetSelectedTreeNode)
            End If
        End Sub

        Private Sub mnuCopy_Click(sender As Object, e As System.EventArgs) Handles mnuCopy.Click
            If Not viewer Is Nothing Then

            End If
        End Sub

        Private Sub mnuCopyValue_Click(sender As Object, e As System.EventArgs) Handles mnuCopyValue.Click
            If Not viewer Is Nothing Then

            End If
        End Sub
    End Class

End Namespace