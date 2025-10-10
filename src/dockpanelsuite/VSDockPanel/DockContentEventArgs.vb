Imports System

Namespace WeifenLuo.WinFormsUI.Docking
    Public Class DockContentEventArgs
        Inherits EventArgs
        Private m_content As IDockContent

        Public Sub New(content As IDockContent)
            m_content = content
        End Sub

        Public ReadOnly Property Content As IDockContent
            Get
                Return m_content
            End Get
        End Property
    End Class
End Namespace
