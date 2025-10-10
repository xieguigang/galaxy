Imports System
Imports System.ComponentModel

Namespace Docking

    Public Partial Class DockPanel
        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DockPanelSkin")>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        <Browsable(False)>
        <Obsolete("Use Theme.Skin instead.")>
        Public ReadOnly Property Skin As DockPanelSkin
            Get
                Return Nothing
            End Get
        End Property

        Private m_dockPanelTheme As ThemeBase = New DefaultTheme()

        <LocalizedCategory("Category_Docking")>
        <LocalizedDescription("DockPanel_DockPanelTheme")>
        Public Property Theme As ThemeBase
            Get
                Return m_dockPanelTheme
            End Get
            Set(value As ThemeBase)
                If value Is Nothing Then
                    Return
                End If

                If m_dockPanelTheme.GetType() Is value.GetType() Then
                    Return
                End If

                m_dockPanelTheme?.CleanUp(Me)
                m_dockPanelTheme = value
                m_dockPanelTheme.ApplyTo(Me)
                m_dockPanelTheme.PostApply(Me)
            End Set
        End Property
    End Class
End Namespace
