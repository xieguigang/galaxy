'Copyright (c) Microsoft Corporation.  All rights reserved.
Imports Microsoft.Windows.Controls.WindowsForms

Namespace Controls

    ''' <summary>
    ''' These options control the results subsequent navigations of the ExplorerBrowser
    ''' </summary>
    Public Class ExplorerBrowserNavigationOptions

#Region "construction"
        Private eb As ExplorerBrowser
        Friend Sub New(eb As ExplorerBrowser)
            Me.eb = eb
            PaneVisibility = New ExplorerBrowserPaneVisibility()
        End Sub
#End Region

#Region "Flags property"
        ''' <summary>
        ''' The binary flags that are passed to the explorer browser control's GetOptions/SetOptions methods
        ''' </summary>
        Public Property Flags() As ExplorerBrowserNavigateOptions
            Get
                Dim ebo As New ExplorerBrowserOptions()
                If eb.explorerBrowserControl IsNot Nothing Then
                    eb.explorerBrowserControl.GetOptions(ebo)
                    Return CType(ebo, ExplorerBrowserNavigateOptions)
                End If
                Return CType(ebo, ExplorerBrowserNavigateOptions)
            End Get
            Set
                Dim ebo As ExplorerBrowserOptions = CType(Value, ExplorerBrowserOptions)
                If eb.explorerBrowserControl IsNot Nothing Then
                    ' Always forcing SHOWFRAMES because we handle IExplorerPaneVisibility
                    eb.explorerBrowserControl.SetOptions(ebo Or ExplorerBrowserOptions.ShowFrames)
                End If
            End Set
        End Property
#End Region

#Region "control flags to properties mapping"
        ''' <summary>
        ''' Do not navigate further than the initial navigation.
        ''' </summary>
        Public Property NavigateOnce() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserNavigateOptions.NavigateOnce)
            End Get
            Set
                SetFlag(ExplorerBrowserNavigateOptions.NavigateOnce, Value)
            End Set
        End Property
        ''' <summary>
        ''' Always navigate, even if you are attempting to navigate to the current folder.
        ''' </summary>
        Public Property AlwaysNavigate() As Boolean
            Get
                Return IsFlagSet(ExplorerBrowserNavigateOptions.AlwaysNavigate)
            End Get
            Set
                SetFlag(ExplorerBrowserNavigateOptions.AlwaysNavigate, Value)
            End Set
        End Property

        Private Function IsFlagSet(flag As ExplorerBrowserNavigateOptions) As Boolean
            Return (Flags And flag) <> 0
        End Function

        Private Sub SetFlag(flag As ExplorerBrowserNavigateOptions, value As Boolean)
            If value Then
                Flags = Flags Or flag
            Else
                Flags = Flags And Not flag
            End If
        End Sub
#End Region

#Region "ExplorerBrowser pane visibility"

        ''' <summary>
        ''' Controls the visibility of the various ExplorerBrowser panes on subsequent navigation
        ''' </summary>
        Public Property PaneVisibility() As ExplorerBrowserPaneVisibility
#End Region
    End Class
End Namespace
