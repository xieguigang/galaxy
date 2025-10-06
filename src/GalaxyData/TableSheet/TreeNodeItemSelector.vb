#Region "License"
' Advanced DataGridView
'
' Copyright (c), 2014 Davide Gironi <davide.gironi@gmail.com>
' Original work Copyright (c), 2013 Zuby <zuby@me.com>
'
' Please refer to LICENSE file for licensing information.
#End Region

Imports System
Imports System.Windows.Forms

Namespace Zuby.ADGV
    <ComponentModel.DesignerCategory("")>
    Friend Class TreeNodeItemSelector
        Inherits TreeNode






#Region "public enum"

        Public Enum CustomNodeType As Byte
            [Default]
            SelectAll
            SelectEmpty
            DateTimeNode
        End Enum

#End Region


#Region "class properties"

        Private _checkState As CheckState = CheckState.Unchecked
        Private _parent As TreeNodeItemSelector

#End Region


#Region "constructor"

        ''' <summary>
        ''' TreeNodeItemSelector constructor
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="value"></param>
        ''' <param name="state"></param>
        ''' <param name="nodeType"></param>
        Private Sub New(text As String, value As Object, state As CheckState, nodeType As CustomNodeType)
            MyBase.New(text)
            CheckState = state
            Me.NodeType = nodeType
            Me.Value = value
        End Sub

#End Region


#Region "public clone method"

        ''' <summary>
        ''' Clone a Node
        ''' </summary>
        ''' <returns></returns>
        Public Overloads Function Clone() As TreeNodeItemSelector
            Dim n As TreeNodeItemSelector = New TreeNodeItemSelector(Text, Value, _checkState, NodeType) With {
    .NodeFont = NodeFont
}

            If GetNodeCount(False) > 0 Then
                For Each child As TreeNodeItemSelector In Nodes
                    n.AddChild(child.Clone())
                Next
            End If

            Return n
        End Function

        ''' <summary>
        ''' Get Node NodeType
        ''' </summary>
        Public Property NodeType As CustomNodeType

        ''' <summary>
        ''' Get Node value
        ''' </summary>
        Public Property Value As Object

        ''' <summary>
        ''' Get Node parent
        ''' </summary>
        Overloads Public Property Parent As TreeNodeItemSelector
            Get
                If TypeOf _parent Is TreeNodeItemSelector Then
                    Return _parent
                Else
                    Return Nothing
                End If
            End Get
            Set(value As TreeNodeItemSelector)
                _parent = value
            End Set
        End Property

        ''' <summary>
        ''' Node is Checked
        ''' </summary>
        Overloads Public Property Checked As Boolean
            Get
                Return _checkState = CheckState.Checked
            End Get
            Set(value As Boolean)
                CheckState = If(value = True, CheckState.Checked, CheckState.Unchecked)
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the current Node CheckState
        ''' </summary>
        Public Property CheckState As CheckState
            Get
                Return _checkState
            End Get
            Set(value As CheckState)
                _checkState = value
                Select Case _checkState
                    Case CheckState.Checked
                        StateImageIndex = 1

                    Case CheckState.Indeterminate
                        StateImageIndex = 2
                    Case Else
                        StateImageIndex = 0
                End Select
            End Set
        End Property

#End Region


#Region "public create nodes methods"

        ''' <summary>
        ''' Create a Node
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="value"></param>
        ''' <param name="state"></param>
        ''' <param name="type"></param>
        ''' <returns></returns>
        Public Shared Function CreateNode(text As String, value As Object, state As CheckState, type As CustomNodeType) As TreeNodeItemSelector
            Return New TreeNodeItemSelector(text, value, state, type)
        End Function

        ''' <summary>
        ''' Create a child Node
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="value"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        Public Function CreateChildNode(text As String, value As Object, state As CheckState) As TreeNodeItemSelector
            Dim n As TreeNodeItemSelector = Nothing

            'specific method for datetimenode
            If NodeType = CustomNodeType.DateTimeNode Then
                n = New TreeNodeItemSelector(text, value, state, CustomNodeType.DateTimeNode)
            End If

            If n IsNot Nothing Then AddChild(n)

            Return n
        End Function
        Public Function CreateChildNode(text As String, value As Object) As TreeNodeItemSelector
            Return CreateChildNode(text, value, _checkState)
        End Function

        ''' <summary>
        ''' Add a child Node to this Node
        ''' </summary>
        ''' <param name="child"></param>
        Protected Sub AddChild(child As TreeNodeItemSelector)
            child.Parent = Me
            Nodes.Add(child)
        End Sub

#End Region

    End Class
End Namespace
