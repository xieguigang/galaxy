Imports System.Globalization
Imports System.Resources
Imports System.Threading

Namespace Docking

    Friend Module ResourceHelper

        Private _resourceManager As ResourceManager = Nothing
        Private resourceCulture As CultureInfo

        Private ReadOnly Property ResourceManager As ResourceManager
            Get
                If _resourceManager Is Nothing Then _resourceManager = New ResourceManager("Microsoft.VisualStudio.WinForms.Strings", GetType(ResourceHelper).Assembly)
                Return _resourceManager
            End Get

        End Property

        Sub New()
            resourceCulture = Thread.CurrentThread.CurrentCulture
        End Sub

        Public Function GetString(name As String) As String
            Return ResourceManager.GetString(name)
        End Function

        '''<summary>
        '''  Looks up a localized string similar to Docking.
        '''</summary>
        Public ReadOnly Property Category_Docking() As String
            Get
                Return ResourceManager.GetString("Category_Docking", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Docking Notification.
        '''</summary>
        Public ReadOnly Property Category_DockingNotification() As String
            Get
                Return ResourceManager.GetString("Category_DockingNotification", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Performance.
        '''</summary>
        Public ReadOnly Property Category_Performance() As String
            Get
                Return ResourceManager.GetString("Category_Performance", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Property Changed.
        '''</summary>
        Public ReadOnly Property Category_PropertyChanged() As String
            Get
                Return ResourceManager.GetString("Category_PropertyChanged", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to (Float).
        '''</summary>
        Public ReadOnly Property DockAreaEditor_FloatCheckBoxText() As String
            Get
                Return ResourceManager.GetString("DockAreaEditor_FloatCheckBoxText", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Determines if end user drag and drop docking is allowed..
        '''</summary>
        Public ReadOnly Property DockContent_AllowEndUserDocking_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_AllowEndUserDocking_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The size to display the content in auto hide mode. Value &lt; 1 to specify the size in portion; value &gt;= 1 to specify the size in pixel..
        '''</summary>
        Public ReadOnly Property DockContent_AutoHidePortion_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_AutoHidePortion_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Enable/Disable the close button of the content..
        '''</summary>
        Public ReadOnly Property DockContent_CloseButton_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_CloseButton_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Shows or hides the close button of the content. This property does not function with System MDI Document Style..
        '''</summary>
        Public ReadOnly Property DockContent_CloseButtonVisible_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_CloseButtonVisible_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The form must be of type IDockContent..
        '''</summary>
        Public ReadOnly Property DockContent_Constructor_InvalidForm() As String
            Get
                Return ResourceManager.GetString("DockContent_Constructor_InvalidForm", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Gets or sets a value indicating in which area of the DockPanel the content allowed to show..
        '''</summary>
        Public ReadOnly Property DockContent_DockAreas_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_DockAreas_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when the value of DockState property changed..
        '''</summary>
        Public ReadOnly Property DockContent_DockStateChanged_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_DockStateChanged_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Indicates the content will be hidden instead of being closed..
        '''</summary>
        Public ReadOnly Property DockContent_HideOnClose_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_HideOnClose_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The desired docking state when first showing..
        '''</summary>
        Public ReadOnly Property DockContent_ShowHint_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_ShowHint_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Context menu displayed for the dock pane tab strip..
        '''</summary>
        Public ReadOnly Property DockContent_TabPageContextMenu_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_TabPageContextMenu_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The tab text displayed in the dock pane. If not set, the Text property will be used..
        '''</summary>
        Public ReadOnly Property DockContent_TabText_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_TabText_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The text displayed when mouse hovers over the tab..
        '''</summary>
        Public ReadOnly Property DockContent_ToolTipText_Description() As String
            Get
                Return ResourceManager.GetString("DockContent_ToolTipText_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The provided value is out of range..
        '''</summary>
        Public ReadOnly Property DockContentHandler_AutoHidePortion_OutOfRange() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_AutoHidePortion_OutOfRange", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Value: The value of DockAreas conflicts with current DockState..
        '''</summary>
        Public ReadOnly Property DockContentHandler_DockAreas_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_DockAreas_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The pane is invalid. Check the IsFloat and DockPanel properties of this dock pane..
        '''</summary>
        Public ReadOnly Property DockContentHandler_DockPane_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_DockPane_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The pane is invalid. Check the IsFloat and DockPanel properties of this dock pane..
        '''</summary>
        Public ReadOnly Property DockContentHandler_FloatPane_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_FloatPane_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid value, conflicts with DockableAreas  property..
        '''</summary>
        Public ReadOnly Property DockContentHandler_IsFloat_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_IsFloat_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The dock state is invalid..
        '''</summary>
        Public ReadOnly Property DockContentHandler_SetDockState_InvalidState() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_SetDockState_InvalidState", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The dock panel is null..
        '''</summary>
        Public ReadOnly Property DockContentHandler_SetDockState_NullPanel() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_SetDockState_NullPanel", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid beforeContent, it must be contained by the pane..
        '''</summary>
        Public ReadOnly Property DockContentHandler_Show_InvalidBeforeContent() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_Show_InvalidBeforeContent", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid DockState: Content can not be showed as &quot;Unknown&quot; or &quot;Hidden&quot;..
        '''</summary>
        Public ReadOnly Property DockContentHandler_Show_InvalidDockState() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_Show_InvalidDockState", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The previous pane is invalid. It can not be null, and its docking state must not be auto-hide..
        '''</summary>
        Public ReadOnly Property DockContentHandler_Show_InvalidPrevPane() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_Show_InvalidPrevPane", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to DockPanel can not be null..
        '''</summary>
        Public ReadOnly Property DockContentHandler_Show_NullDockPanel() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_Show_NullDockPanel", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The Pane can not be null..
        '''</summary>
        Public ReadOnly Property DockContentHandler_Show_NullPane() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_Show_NullPane", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid value, check DockableAreas property..
        '''</summary>
        Public ReadOnly Property DockContentHandler_ShowHint_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockContentHandler_ShowHint_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Context menu displayed for the dock pane tab strip..
        '''</summary>
        Public ReadOnly Property DockHandler_TabPageContextMenuStrip_Description() As String
            Get
                Return ResourceManager.GetString("DockHandler_TabPageContextMenuStrip_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Content: ActiveContent must be one of the visible contents, or null if there is no visible content..
        '''</summary>
        Public ReadOnly Property DockPane_ActiveContent_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockPane_ActiveContent_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid argument: Content can not be &quot;null&quot;..
        '''</summary>
        Public ReadOnly Property DockPane_Constructor_NullContent() As String
            Get
                Return ResourceManager.GetString("DockPane_Constructor_NullContent", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid argument: The content&apos;s DockPanel can not be &quot;null&quot;..
        '''</summary>
        Public ReadOnly Property DockPane_Constructor_NullDockPanel() As String
            Get
                Return ResourceManager.GetString("DockPane_Constructor_NullDockPanel", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The specified container conflicts with the IsFloat property..
        '''</summary>
        Public ReadOnly Property DockPane_DockTo_InvalidContainer() As String
            Get
                Return ResourceManager.GetString("DockPane_DockTo_InvalidContainer", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The previous pane does not exist in the nested docking pane collection..
        '''</summary>
        Public ReadOnly Property DockPane_DockTo_NoPrevPane() As String
            Get
                Return ResourceManager.GetString("DockPane_DockTo_NoPrevPane", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The container can not be null..
        '''</summary>
        Public ReadOnly Property DockPane_DockTo_NullContainer() As String
            Get
                Return ResourceManager.GetString("DockPane_DockTo_NullContainer", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The previous pane can not be null when the nested docking pane collection is not empty..
        '''</summary>
        Public ReadOnly Property DockPane_DockTo_NullPrevPane() As String
            Get
                Return ResourceManager.GetString("DockPane_DockTo_NullPrevPane", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The previous pane can not be itself..
        '''</summary>
        Public ReadOnly Property DockPane_DockTo_SelfPrevPane() As String
            Get
                Return ResourceManager.GetString("DockPane_DockTo_SelfPrevPane", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Content: Content not within the collection..
        '''</summary>
        Public ReadOnly Property DockPane_SetContentIndex_InvalidContent() As String
            Get
                Return ResourceManager.GetString("DockPane_SetContentIndex_InvalidContent", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Index: The index is out of range..
        '''</summary>
        Public ReadOnly Property DockPane_SetContentIndex_InvalidIndex() As String
            Get
                Return ResourceManager.GetString("DockPane_SetContentIndex_InvalidIndex", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The state for the dock pane is invalid..
        '''</summary>
        Public ReadOnly Property DockPane_SetDockState_InvalidState() As String
            Get
                Return ResourceManager.GetString("DockPane_SetDockState_InvalidState", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Content: The content must be auto-hide state and associates with this DockPanel..
        '''</summary>
        Public ReadOnly Property DockPanel_ActiveAutoHideContent_InvalidValue() As String
            Get
                Return ResourceManager.GetString("DockPanel_ActiveAutoHideContent_InvalidValue", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when the value of the AutoHideWindow&apos;s ActiveContent changed..
        '''</summary>
        Public ReadOnly Property DockPanel_ActiveAutoHideContentChanged_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ActiveAutoHideContentChanged_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when the value of ActiveContentProperty changed..
        '''</summary>
        Public ReadOnly Property DockPanel_ActiveContentChanged_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ActiveContentChanged_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when the value of ActiveDocument property changed..
        '''</summary>
        Public ReadOnly Property DockPanel_ActiveDocumentChanged_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ActiveDocumentChanged_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when the value of ActivePane property changed..
        '''</summary>
        Public ReadOnly Property DockPanel_ActivePaneChanged_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ActivePaneChanged_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Determines if the drag and drop docking is allowed..
        '''</summary>
        Public ReadOnly Property DockPanel_AllowEndUserDocking_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_AllowEndUserDocking_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Determines if the drag and drop nested docking is allowed..
        '''</summary>
        Public ReadOnly Property DockPanel_AllowEndUserNestedDocking_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_AllowEndUserNestedDocking_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when a content added to the DockPanel..
        '''</summary>
        Public ReadOnly Property DockPanel_ContentAdded_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ContentAdded_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when a content removed from the DockPanel..
        '''</summary>
        Public ReadOnly Property DockPanel_ContentRemoved_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ContentRemoved_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The default size of float window..
        '''</summary>
        Public ReadOnly Property DockPanel_DefaultFloatWindowSize_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DefaultFloatWindowSize_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Provides Visual Studio .Net style docking..
        '''</summary>
        Public ReadOnly Property DockPanel_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Size of the bottom docking window. Value &lt; 1 to specify the size in portion; value &gt; 1 to specify the size in pixels..
        '''</summary>
        Public ReadOnly Property DockPanel_DockBottomPortion_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DockBottomPortion_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Size of the left docking window. Value &lt; 1 to specify the size in portion; value &gt; 1 to specify the size in pixels..
        '''</summary>
        Public ReadOnly Property DockPanel_DockLeftPortion_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DockLeftPortion_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The visual skin to use when displaying the docked windows..
        '''</summary>
        Public ReadOnly Property DockPanel_DockPanelSkin_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DockPanelSkin_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Size of the right docking window. Value &lt; 1 to specify the size in portion; value &gt; 1 to specify the size in pixels..
        '''</summary>
        Public ReadOnly Property DockPanel_DockRightPortion_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DockRightPortion_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Size of the top docking window. Value &lt; 1 to specify the size in portion; value &gt; 1 to specify the size in pixels..
        '''</summary>
        Public ReadOnly Property DockPanel_DockTopPortion_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DockTopPortion_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Occurs when a document or pane is dragged within the dock panel..
        '''</summary>
        Public ReadOnly Property DockPanel_DocumentDragged_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DocumentDragged_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The style of the document window..
        '''</summary>
        Public ReadOnly Property DockPanel_DocumentStyle_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DocumentStyle_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Determines where the tab strip for Document style content is drawn..
        '''</summary>
        Public ReadOnly Property DockPanel_DocumentTabStripLocation_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_DocumentTabStripLocation_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The DockPanel has already been initialized..
        '''</summary>
        Public ReadOnly Property DockPanel_LoadFromXml_AlreadyInitialized() As String
            Get
                Return ResourceManager.GetString("DockPanel_LoadFromXml_AlreadyInitialized", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The configuration file&apos;s version is invalid..
        '''</summary>
        Public ReadOnly Property DockPanel_LoadFromXml_InvalidFormatVersion() As String
            Get
                Return ResourceManager.GetString("DockPanel_LoadFromXml_InvalidFormatVersion", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to The XML file format is invalid..
        '''</summary>
        Public ReadOnly Property DockPanel_LoadFromXml_InvalidXmlFormat() As String
            Get
                Return ResourceManager.GetString("DockPanel_LoadFromXml_InvalidXmlFormat", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid parent form. When using DockingMdi or SystemMdi document style, the DockPanel control must be the child control of the main MDI container form..
        '''</summary>
        Public ReadOnly Property DockPanel_ParentForm_Invalid() As String
            Get
                Return ResourceManager.GetString("DockPanel_ParentForm_Invalid", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to DockPanel configuration file. Author: Weifen Luo, all rights reserved..
        '''</summary>
        Public ReadOnly Property DockPanel_Persistor_XmlFileComment1() As String
            Get
                Return ResourceManager.GetString("DockPanel_Persistor_XmlFileComment1", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to !!! AUTOMATICALLY GENERATED FILE. DO NOT MODIFY !!!.
        '''</summary>
        Public ReadOnly Property DockPanel_Persistor_XmlFileComment2() As String
            Get
                Return ResourceManager.GetString("DockPanel_Persistor_XmlFileComment2", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Indicates whether the control layout is right-to-left when the RightToLeft property is set to Yes..
        '''</summary>
        Public ReadOnly Property DockPanel_RightToLeftLayout_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_RightToLeftLayout_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Index: The index is out of range..
        '''</summary>
        Public ReadOnly Property DockPanel_SetPaneIndex_InvalidIndex() As String
            Get
                Return ResourceManager.GetString("DockPanel_SetPaneIndex_InvalidIndex", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid Pane: DockPane not within the collection..
        '''</summary>
        Public ReadOnly Property DockPanel_SetPaneIndex_InvalidPane() As String
            Get
                Return ResourceManager.GetString("DockPanel_SetPaneIndex_InvalidPane", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Shows the hidden autohide content when hovering over the tab.  When disabled, the tab must be clicked to show the content..
        '''</summary>
        Public ReadOnly Property DockPanel_ShowAutoHideContentOnHover_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ShowAutoHideContentOnHover_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Determines if the document icon will be displayed in the tab strip..
        '''</summary>
        Public ReadOnly Property DockPanel_ShowDocumentIcon_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_ShowDocumentIcon_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Support deeply nested controls.  Disabling this setting may improve resize performance but may cause heavily nested content not to resize..
        '''</summary>
        Public ReadOnly Property DockPanel_SupportDeeplyNestedContent_Description() As String
            Get
                Return ResourceManager.GetString("DockPanel_SupportDeeplyNestedContent_Description", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid argument: DockPanel can not be &quot;null&quot;..
        '''</summary>
        Public ReadOnly Property FloatWindow_Constructor_NullDockPanel() As String
            Get
                Return ResourceManager.GetString("FloatWindow_Constructor_NullDockPanel", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Invalid DockPanel..
        '''</summary>
        Public ReadOnly Property IDockDragSource_DockTo_InvalidPanel() As String
            Get
                Return ResourceManager.GetString("IDockDragSource_DockTo_InvalidPanel", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Before applying themes all dock contents must be closed..
        '''</summary>
        Public ReadOnly Property Theme_DockContentNotClosed() As String
            Get
                Return ResourceManager.GetString("Theme_DockContentNotClosed", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Before applying themes all float windows must be closed..
        '''</summary>
        Public ReadOnly Property Theme_FloatWindowNotClosed() As String
            Get
                Return ResourceManager.GetString("Theme_FloatWindowNotClosed", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Before applying themes all factories must be configured..
        '''</summary>
        Public ReadOnly Property Theme_MissingFactory() As String
            Get
                Return ResourceManager.GetString("Theme_MissingFactory", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to DockPanel.Theme must be set to a valid theme..
        '''</summary>
        Public ReadOnly Property Theme_NoTheme() As String
            Get
                Return ResourceManager.GetString("Theme_NoTheme", resourceCulture)
            End Get
        End Property

        '''<summary>
        '''  Looks up a localized string similar to Before applying themes all panes must be closed..
        '''</summary>
        Public ReadOnly Property Theme_PaneNotClosed() As String
            Get
                Return ResourceManager.GetString("Theme_PaneNotClosed", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
