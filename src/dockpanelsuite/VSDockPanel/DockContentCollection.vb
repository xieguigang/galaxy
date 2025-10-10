Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Docking
    Public Class DockContentCollection
        Inherits ReadOnlyCollection(Of IDockContent)
        Private Shared _emptyList As List(Of IDockContent) = New List(Of IDockContent)(0)

        Friend Sub New()
            MyBase.New(New List(Of IDockContent)())
        End Sub

        Friend Sub New(pane As DockPane)
            MyBase.New(_emptyList)
            m_dockPane = pane
        End Sub

        Private m_dockPane As DockPane = Nothing
        Private ReadOnly Property DockPane As DockPane
            Get
                Return m_dockPane
            End Get
        End Property

        Default Public Overloads ReadOnly Property Item(index As Integer) As IDockContent
            Get
                If DockPane Is Nothing Then
                    Return TryCast(Items(index), IDockContent)
                Else
                    Return GetVisibleContent(index)
                End If
            End Get
        End Property

        Friend Function Add(content As IDockContent) As Integer
#If DEBUG
            If DockPane IsNot Nothing Then Throw New InvalidOperationException()
#End If

            If Contains(content) Then Return IndexOf(content)

            Items.Add(content)
            Return Count - 1
        End Function

        Friend Sub AddAt(content As IDockContent, index As Integer)
#If DEBUG
            If DockPane IsNot Nothing Then Throw New InvalidOperationException()
#End If

            If index < 0 OrElse index > Items.Count - 1 Then Return

            If Contains(content) Then Return

            Items.Insert(index, content)
        End Sub

        Public Overloads Function Contains(content As IDockContent) As Boolean
            If DockPane Is Nothing Then
                Return Items.Contains(content)
            Else
                Return GetIndexOfVisibleContents(content) <> -1
            End If
        End Function

        Public Overloads ReadOnly Property Count As Integer
            Get
                If DockPane Is Nothing Then
                    Return MyBase.Count
                Else
                    Return CountOfVisibleContents
                End If
            End Get
        End Property

        Public Overloads Function IndexOf(content As IDockContent) As Integer
            If DockPane Is Nothing Then
                If Not Contains(content) Then
                    Return -1
                Else
                    Return Items.IndexOf(content)
                End If
            Else
                Return GetIndexOfVisibleContents(content)
            End If
        End Function

        Friend Sub Remove(content As IDockContent)
            If DockPane IsNot Nothing Then Throw New InvalidOperationException()

            If Not Contains(content) Then Return

            Items.Remove(content)
        End Sub

        Private ReadOnly Property CountOfVisibleContents As Integer
            Get
#If DEBUG
                If DockPane Is Nothing Then Throw New InvalidOperationException()
#End If

                Dim count = 0
                For Each content In DockPane.Contents
                    If content.DockHandler.DockState = DockPane.DockState Then count += 1
                Next
                Return count
            End Get
        End Property

        Private Function GetVisibleContent(index As Integer) As IDockContent
#If DEBUG
            If DockPane Is Nothing Then Throw New InvalidOperationException()
#End If

            Dim currentIndex = -1
            For Each content In DockPane.Contents
                If content.DockHandler.DockState = DockPane.DockState Then currentIndex += 1

                If currentIndex = index Then Return content
            Next

            Return Nothing
        End Function

        Private Function GetIndexOfVisibleContents(content As IDockContent) As Integer
#If DEBUG
            If DockPane Is Nothing Then Throw New InvalidOperationException()
#End If

            If content Is Nothing Then Return -1

            Dim index = -1
            For Each c In DockPane.Contents
                If c.DockHandler.DockState = DockPane.DockState Then
                    index += 1

                    If c Is content Then Return index
                End If
            Next
            Return -1
        End Function
    End Class
End Namespace
