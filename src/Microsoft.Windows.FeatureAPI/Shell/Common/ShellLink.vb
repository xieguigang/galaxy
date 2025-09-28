'Copyright (c) Microsoft Corporation.  All rights reserved.


Namespace Shell
	''' <summary>
	''' Represents a link to existing FileSystem or Virtual item.
	''' </summary>
	Public Class ShellLink
		Inherits ShellObject
		''' <summary>
		''' Path for this file e.g. c:\Windows\file.txt,
		''' </summary>
		Private _internalPath As String

		#Region "Internal Constructors"

		Friend Sub New(shellItem As IShellItem2)
            m_nativeShellItem = shellItem
        End Sub

		#End Region

		#Region "Public Properties"

		''' <summary>
		''' The path for this link
		''' </summary>
		Public Overridable Property Path() As String
			Get
				If _internalPath Is Nothing AndAlso NativeShellItem IsNot Nothing Then
					_internalPath = MyBase.ParsingName
				End If
				Return _internalPath
			End Get
			Protected Set
				Me._internalPath = value
			End Set
		End Property

		Private internalTargetLocation As String
		''' <summary>
		''' Gets the location to which this link points to.
		''' </summary>
		Public Property TargetLocation() As String
			Get
				If String.IsNullOrEmpty(internalTargetLocation) AndAlso NativeShellItem2 IsNot Nothing Then
					internalTargetLocation = Me.Properties.System.Link.TargetParsingPath.Value
				End If
				Return internalTargetLocation
			End Get
			Set
				If value Is Nothing Then
					Return
				End If

				internalTargetLocation = value

				If NativeShellItem2 IsNot Nothing Then
					Me.Properties.System.Link.TargetParsingPath.Value = internalTargetLocation
				End If
			End Set
		End Property

		''' <summary>
		''' Gets the ShellObject to which this link points to.
		''' </summary>
		Public ReadOnly Property TargetShellObject() As ShellObject
			Get
				Return ShellObjectFactory.Create(TargetLocation)
			End Get
		End Property

		''' <summary>
		''' Gets or sets the link's title
		''' </summary>
		Public Property Title() As String
			Get
				If NativeShellItem2 IsNot Nothing Then
					Return Me.Properties.System.Title.Value
				End If
				Return Nothing
			End Get
			Set
				If value Is Nothing Then
					Throw New ArgumentNullException("value")
				End If

				If NativeShellItem2 IsNot Nothing Then
					Me.Properties.System.Title.Value = value
				End If
			End Set
		End Property

		Private internalArguments As String
		''' <summary>
		''' Gets the arguments associated with this link.
		''' </summary>
		Public ReadOnly Property Arguments() As String
			Get
				If String.IsNullOrEmpty(internalArguments) AndAlso NativeShellItem2 IsNot Nothing Then
					internalArguments = Me.Properties.System.Link.Arguments.Value
				End If

				Return internalArguments
			End Get
		End Property

		Private internalComments As String
		''' <summary>
		''' Gets the comments associated with this link.
		''' </summary>
		Public ReadOnly Property Comments() As String
			Get
				If String.IsNullOrEmpty(internalComments) AndAlso NativeShellItem2 IsNot Nothing Then
					internalComments = Me.Properties.System.Comment.Value
				End If

				Return internalComments
			End Get
		End Property


		#End Region
	End Class
End Namespace
