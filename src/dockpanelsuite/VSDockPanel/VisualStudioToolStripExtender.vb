Imports System
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Collections.Generic

Namespace WeifenLuo.WinFormsUI.Docking
    <ProvideProperty("EnableVSStyle", GetType(ToolStrip))>
    Public Partial Class VisualStudioToolStripExtender
        Inherits Component
        Implements IExtenderProvider
        Private Class ToolStripProperties
            Private version As VsVersion = VsVersion.Unknown
            Private ReadOnly strip As ToolStrip
            Private ReadOnly menuText As Dictionary(Of ToolStripItem, String) = New Dictionary(Of ToolStripItem, String)()


            Public Sub New(toolstrip As ToolStrip)
                If toolstrip Is Nothing Then Throw New ArgumentNullException(NameOf(toolstrip))
                strip = toolstrip

                If TypeOf strip Is MenuStrip Then SaveMenuStripText()
            End Sub

            Public Property VsVersion As VsVersion
                Get
                    Return version
                End Get
                Set(value As VsVersion)
                    version = value
                    UpdateMenuText(version = VsVersion.Vs2012 OrElse version = VsVersion.Vs2013)
                End Set
            End Property

            Private Sub SaveMenuStripText()
                For Each item As ToolStripItem In strip.Items
                    menuText.Add(item, item.Text)
                Next
            End Sub

            Public Sub UpdateMenuText(caps As Boolean)
                For Each item In menuText.Keys
                    Dim text = menuText(item)
                    item.Text = If(caps, text.ToUpper(), text)
                Next
            End Sub
        End Class

        Private ReadOnly strips As Dictionary(Of ToolStrip, ToolStripProperties) = New Dictionary(Of ToolStrip, ToolStripProperties)()

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(container As IContainer)
            container.Add(Me)

            InitializeComponent()
        End Sub

#Region "IExtenderProvider Members"

        Public Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
            Return TypeOf extendee Is ToolStrip
        End Function

#End Region

        Public Property DefaultRenderer As ToolStripRenderer

        <DefaultValue(False)>
        Public Function GetStyle(strip As ToolStrip) As VsVersion
            If strips.ContainsKey(strip) Then Return strips(strip).VsVersion

            Return VsVersion.Unknown
        End Function

        Public Sub SetStyle(strip As ToolStrip, version As VsVersion, theme As ThemeBase)
            Dim properties As ToolStripProperties = Nothing

            If Not strips.ContainsKey(strip) Then
                properties = New ToolStripProperties(strip) With {
                    .VsVersion = version
                }
                strips.Add(strip, properties)
            Else
                properties = strips(strip)
            End If

            If theme Is Nothing Then
                If DefaultRenderer IsNot Nothing Then strip.Renderer = DefaultRenderer
            Else
                theme.ApplyTo(strip)
            End If
            properties.VsVersion = version
        End Sub

        Public Enum VsVersion
            Unknown
            Vs2003
            Vs2005
            Vs2008
            Vs2010
            Vs2012
            Vs2013
            Vs2015
        End Enum
    End Class
End Namespace
