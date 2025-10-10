Imports System.ComponentModel
Imports Microsoft.VisualStudio.WinForms.Docking

Namespace ThemeVS2012
    ''' <summary>
    ''' Dock window of Visual Studio 2012 Light theme.
    ''' </summary>
    <ToolboxItem(False)>
    Friend Class VS2012DockWindow
        Inherits DockWindow
        ''' <summary>
        ''' Initializes a new instance of the <see cref="VS2012DockWindow"/> class.
        ''' </summary>
        ''' <param name="dockPanel">The dock panel.</param>
        ''' <param name="dockState">State of the dock.</param>
        Public Sub New(dockPanel As DockPanel, dockState As DockState)
            MyBase.New(dockPanel, dockState)
        End Sub

        Public Overrides ReadOnly Property DisplayingRectangle As Rectangle
            Get
                Dim rect = ClientRectangle
                If DockState = DockState.DockLeft Then
                    rect.Width -= DockPanel.Theme.Measures.SplitterSize
                ElseIf DockState = DockState.DockRight Then
                    rect.X += DockPanel.Theme.Measures.SplitterSize
                    rect.Width -= DockPanel.Theme.Measures.SplitterSize
                ElseIf DockState = DockState.DockTop Then
                    rect.Height -= DockPanel.Theme.Measures.SplitterSize
                ElseIf DockState = DockState.DockBottom Then
                    rect.Y += DockPanel.Theme.Measures.SplitterSize
                    rect.Height -= DockPanel.Theme.Measures.SplitterSize
                End If

                Return rect
            End Get
        End Property
    End Class
End Namespace
