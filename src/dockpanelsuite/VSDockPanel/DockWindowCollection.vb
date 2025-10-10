Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace WeifenLuo.WinFormsUI.Docking
    Public Class DockWindowCollection
        Inherits ReadOnlyCollection(Of DockWindow)
        Friend Sub New(dockPanel As DockPanel)
            MyBase.New(New List(Of DockWindow)())
            Items.Add(dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow(dockPanel, DockState.Document))
            Items.Add(dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow(dockPanel, DockState.DockLeft))
            Items.Add(dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow(dockPanel, DockState.DockRight))
            Items.Add(dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow(dockPanel, DockState.DockTop))
            Items.Add(dockPanel.Theme.Extender.DockWindowFactory.CreateDockWindow(dockPanel, DockState.DockBottom))
        End Sub

        Default Public Overloads ReadOnly Property Item(dockState As DockState) As DockWindow
            Get
                If dockState = DockState.Document Then
                    Return Items(0)
                ElseIf dockState = DockState.DockLeft OrElse dockState = DockState.DockLeftAutoHide Then
                    Return Items(1)
                ElseIf dockState = DockState.DockRight OrElse dockState = DockState.DockRightAutoHide Then
                    Return Items(2)
                ElseIf dockState = DockState.DockTop OrElse dockState = DockState.DockTopAutoHide Then
                    Return Items(3)
                ElseIf dockState = DockState.DockBottom OrElse dockState = DockState.DockBottomAutoHide Then
                    Return Items(4)
                End If

                Throw (New ArgumentOutOfRangeException())
            End Get
        End Property
    End Class
End Namespace
