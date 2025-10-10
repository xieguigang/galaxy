Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Globalization

Namespace Docking
#Region "DockPanelSkin classes"
    ''' <summary>
    ''' The skin to use when displaying the DockPanel.
    ''' The skin allows custom gradient color schemes to be used when drawing the
    ''' DockStrips and Tabs.
    ''' </summary>
    <TypeConverter(GetType(DockPanelSkinConverter))>
    Public Class DockPanelSkin
        Private m_autoHideStripSkin As AutoHideStripSkin = New AutoHideStripSkin()
        Private m_dockPaneStripSkin As DockPaneStripSkin = New DockPaneStripSkin()

        ''' <summary>
        ''' The skin used to display the auto hide strips and tabs.
        ''' </summary>
        Public Property AutoHideStripSkin As AutoHideStripSkin
            Get
                Return m_autoHideStripSkin
            End Get
            Set(value As AutoHideStripSkin)
                m_autoHideStripSkin = value
            End Set
        End Property

        ''' <summary>
        ''' The skin used to display the Document and ToolWindow style DockStrips and Tabs.
        ''' </summary>
        Public Property DockPaneStripSkin As DockPaneStripSkin
            Get
                Return m_dockPaneStripSkin
            End Get
            Set(value As DockPaneStripSkin)
                m_dockPaneStripSkin = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' The skin used to display the auto hide strip and tabs.
    ''' </summary>
    <TypeConverter(GetType(AutoHideStripConverter))>
    Public Class AutoHideStripSkin
        Private m_dockStripGradient As DockPanelGradient = New DockPanelGradient()
        Private m_TabGradient As TabGradient = New TabGradient()
        Private m_DockStripBackground As DockStripBackground = New DockStripBackground()

        Private m_textFont As Font = SystemFonts.MenuFont

        ''' <summary>
        ''' The gradient color skin for the DockStrips.
        ''' </summary>
        Public Property DockStripGradient As DockPanelGradient
            Get
                Return m_dockStripGradient
            End Get
            Set(value As DockPanelGradient)
                m_dockStripGradient = value
            End Set
        End Property

        ''' <summary>
        ''' The gradient color skin for the Tabs.
        ''' </summary>
        Public Property TabGradient As TabGradient
            Get
                Return m_TabGradient
            End Get
            Set(value As TabGradient)
                m_TabGradient = value
            End Set
        End Property

        ''' <summary>
        ''' The gradient color skin for the Tabs.
        ''' </summary>
        Public Property DockStripBackground As DockStripBackground
            Get
                Return m_DockStripBackground
            End Get
            Set(value As DockStripBackground)
                m_DockStripBackground = value
            End Set
        End Property

        ''' <summary>
        ''' Font used in AutoHideStrip elements.
        ''' </summary>
        <DefaultValue(GetType(SystemFonts), "MenuFont")>
        Public Property TextFont As Font
            Get
                Return m_textFont
            End Get
            Set(value As Font)
                m_textFont = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' The skin used to display the document and tool strips and tabs.
    ''' </summary>
    <TypeConverter(GetType(DockPaneStripConverter))>
    Public Class DockPaneStripSkin
        Private m_DocumentGradient As DockPaneStripGradient = New DockPaneStripGradient()
        Private m_ToolWindowGradient As DockPaneStripToolWindowGradient = New DockPaneStripToolWindowGradient()
        Private m_textFont As Font = SystemFonts.MenuFont

        ''' <summary>
        ''' The skin used to display the Document style DockPane strip and tab.
        ''' </summary>
        Public Property DocumentGradient As DockPaneStripGradient
            Get
                Return m_DocumentGradient
            End Get
            Set(value As DockPaneStripGradient)
                m_DocumentGradient = value
            End Set
        End Property

        ''' <summary>
        ''' The skin used to display the ToolWindow style DockPane strip and tab.
        ''' </summary>
        Public Property ToolWindowGradient As DockPaneStripToolWindowGradient
            Get
                Return m_ToolWindowGradient
            End Get
            Set(value As DockPaneStripToolWindowGradient)
                m_ToolWindowGradient = value
            End Set
        End Property

        ''' <summary>
        ''' Font used in DockPaneStrip elements.
        ''' </summary>
        <DefaultValue(GetType(SystemFonts), "MenuFont")>
        Public Property TextFont As Font
            Get
                Return m_textFont
            End Get
            Set(value As Font)
                m_textFont = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' The skin used to display the DockPane ToolWindow strip and tab.
    ''' </summary>
    <TypeConverter(GetType(DockPaneStripGradientConverter))>
    Public Class DockPaneStripToolWindowGradient
        Inherits DockPaneStripGradient
        Private m_activeCaptionGradient As TabGradient = New TabGradient()
        Private m_inactiveCaptionGradient As TabGradient = New TabGradient()

        ''' <summary>
        ''' The skin used to display the active ToolWindow caption.
        ''' </summary>
        Public Property ActiveCaptionGradient As TabGradient
            Get
                Return m_activeCaptionGradient
            End Get
            Set(value As TabGradient)
                m_activeCaptionGradient = value
            End Set
        End Property

        ''' <summary>
        ''' The skin used to display the inactive ToolWindow caption.
        ''' </summary>
        Public Property InactiveCaptionGradient As TabGradient
            Get
                Return m_inactiveCaptionGradient
            End Get
            Set(value As TabGradient)
                m_inactiveCaptionGradient = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' The skin used to display the DockPane strip and tab.
    ''' </summary>
    <TypeConverter(GetType(DockPaneStripGradientConverter))>
    Public Class DockPaneStripGradient
        Private m_dockStripGradient As DockPanelGradient = New DockPanelGradient()
        Private m_activeTabGradient As TabGradient = New TabGradient()
        Private m_inactiveTabGradient As TabGradient = New TabGradient()
        Private m_hoverTabGradient As TabGradient = New TabGradient()


        ''' <summary>
        ''' The gradient color skin for the DockStrip.
        ''' </summary>
        Public Property DockStripGradient As DockPanelGradient
            Get
                Return m_dockStripGradient
            End Get
            Set(value As DockPanelGradient)
                m_dockStripGradient = value
            End Set
        End Property

        ''' <summary>
        ''' The skin used to display the active DockPane tabs.
        ''' </summary>
        Public Property ActiveTabGradient As TabGradient
            Get
                Return m_activeTabGradient
            End Get
            Set(value As TabGradient)
                m_activeTabGradient = value
            End Set
        End Property

        Public Property HoverTabGradient As TabGradient
            Get
                Return m_hoverTabGradient
            End Get
            Set(value As TabGradient)
                m_hoverTabGradient = value
            End Set
        End Property

        ''' <summary>
        ''' The skin used to display the inactive DockPane tabs.
        ''' </summary>
        Public Property InactiveTabGradient As TabGradient
            Get
                Return m_inactiveTabGradient
            End Get
            Set(value As TabGradient)
                m_inactiveTabGradient = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' The skin used to display the dock pane tab
    ''' </summary>
    <TypeConverter(GetType(DockPaneTabGradientConverter))>
    Public Class TabGradient
        Inherits DockPanelGradient
        Private m_textColor As Color = SystemColors.ControlText

        ''' <summary>
        ''' The text color.
        ''' </summary>
        <DefaultValue(GetType(SystemColors), "ControlText")>
        Public Property TextColor As Color
            Get
                Return m_textColor
            End Get
            Set(value As Color)
                m_textColor = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' The skin used to display the dock pane tab
    ''' </summary>
    <TypeConverter(GetType(DockPaneTabGradientConverter))>
    Public Class DockStripBackground
        Private m_startColor As Color = SystemColors.Control
        Private m_endColor As Color = SystemColors.Control
        'private LinearGradientMode m_linearGradientMode = LinearGradientMode.Horizontal;

        ''' <summary>
        ''' The beginning gradient color.
        ''' </summary>
        <DefaultValue(GetType(SystemColors), "Control")>
        Public Property StartColor As Color
            Get
                Return m_startColor
            End Get
            Set(value As Color)
                m_startColor = value
            End Set
        End Property

        ''' <summary>
        ''' The ending gradient color.
        ''' </summary>
        <DefaultValue(GetType(SystemColors), "Control")>
        Public Property EndColor As Color
            Get
                Return m_endColor
            End Get
            Set(value As Color)
                m_endColor = value
            End Set
        End Property
    End Class


    ''' <summary>
    ''' The gradient color skin.
    ''' </summary>
    <TypeConverter(GetType(DockPanelGradientConverter))>
    Public Class DockPanelGradient
        Private m_startColor As Color = SystemColors.Control
        Private m_endColor As Color = SystemColors.Control
        Private m_linearGradientMode As LinearGradientMode = LinearGradientMode.Horizontal

        ''' <summary>
        ''' The beginning gradient color.
        ''' </summary>
        <DefaultValue(GetType(SystemColors), "Control")>
        Public Property StartColor As Color
            Get
                Return m_startColor
            End Get
            Set(value As Color)
                m_startColor = value
            End Set
        End Property

        ''' <summary>
        ''' The ending gradient color.
        ''' </summary>
        <DefaultValue(GetType(SystemColors), "Control")>
        Public Property EndColor As Color
            Get
                Return m_endColor
            End Get
            Set(value As Color)
                m_endColor = value
            End Set
        End Property

        ''' <summary>
        ''' The gradient mode to display the colors.
        ''' </summary>
        <DefaultValue(LinearGradientMode.Horizontal)>
        Public Property LinearGradientMode As LinearGradientMode
            Get
                Return m_linearGradientMode
            End Get
            Set(value As LinearGradientMode)
                m_linearGradientMode = value
            End Set
        End Property
    End Class

#End Region

#Region "Converters"
    Public Class DockPanelSkinConverter
        Inherits ExpandableObjectConverter
        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(DockPanelSkin) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf value Is DockPanelSkin Then
                Return "DockPanelSkin"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class DockPanelGradientConverter
        Inherits ExpandableObjectConverter
        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(DockPanelGradient) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf value Is DockPanelGradient Then
                Return "DockPanelGradient"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class AutoHideStripConverter
        Inherits ExpandableObjectConverter
        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(AutoHideStripSkin) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf value Is AutoHideStripSkin Then
                Return "AutoHideStripSkin"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class DockPaneStripConverter
        Inherits ExpandableObjectConverter
        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(DockPaneStripSkin) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf value Is DockPaneStripSkin Then
                Return "DockPaneStripSkin"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class DockPaneStripGradientConverter
        Inherits ExpandableObjectConverter
        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(DockPaneStripGradient) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf value Is DockPaneStripGradient Then
                Return "DockPaneStripGradient"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class

    Public Class DockPaneTabGradientConverter
        Inherits ExpandableObjectConverter
        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType Is GetType(TabGradient) Then Return True

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            Dim val As TabGradient = TryCast(value, TabGradient)
            If destinationType Is GetType(String) AndAlso val IsNot Nothing Then
                Return "DockPaneTabGradient"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class
#End Region
End Namespace
