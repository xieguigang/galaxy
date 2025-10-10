Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Globalization
Imports System.Windows.Forms

Namespace WeifenLuo.WinFormsUI.Docking
    Partial Class DockPanel
        Private NotInheritable Class Persistor
            Private Const ConfigFileVersion As String = "1.0"
            Private Shared CompatibleConfigFileVersions As String() = {}

            Private Class DummyContent
                Inherits DockContent
            End Class

            Private Structure DockPanelStruct
                Private m_dockLeftPortion As Double
                Public Property DockLeftPortion As Double
                    Get
                        Return m_dockLeftPortion
                    End Get
                    Set(value As Double)
                        m_dockLeftPortion = value
                    End Set
                End Property

                Private m_dockRightPortion As Double
                Public Property DockRightPortion As Double
                    Get
                        Return m_dockRightPortion
                    End Get
                    Set(value As Double)
                        m_dockRightPortion = value
                    End Set
                End Property

                Private m_dockTopPortion As Double
                Public Property DockTopPortion As Double
                    Get
                        Return m_dockTopPortion
                    End Get
                    Set(value As Double)
                        m_dockTopPortion = value
                    End Set
                End Property

                Private m_dockBottomPortion As Double
                Public Property DockBottomPortion As Double
                    Get
                        Return m_dockBottomPortion
                    End Get
                    Set(value As Double)
                        m_dockBottomPortion = value
                    End Set
                End Property

                Private m_indexActiveDocumentPane As Integer
                Public Property IndexActiveDocumentPane As Integer
                    Get
                        Return m_indexActiveDocumentPane
                    End Get
                    Set(value As Integer)
                        m_indexActiveDocumentPane = value
                    End Set
                End Property

                Private m_indexActivePane As Integer
                Public Property IndexActivePane As Integer
                    Get
                        Return m_indexActivePane
                    End Get
                    Set(value As Integer)
                        m_indexActivePane = value
                    End Set
                End Property
            End Structure

            Private Structure ContentStruct
                Private m_persistString As String
                Public Property PersistString As String
                    Get
                        Return m_persistString
                    End Get
                    Set(value As String)
                        m_persistString = value
                    End Set
                End Property

                Private m_autoHidePortion As Double
                Public Property AutoHidePortion As Double
                    Get
                        Return m_autoHidePortion
                    End Get
                    Set(value As Double)
                        m_autoHidePortion = value
                    End Set
                End Property

                Private m_isHidden As Boolean
                Public Property IsHidden As Boolean
                    Get
                        Return m_isHidden
                    End Get
                    Set(value As Boolean)
                        m_isHidden = value
                    End Set
                End Property

                Private m_isFloat As Boolean
                Public Property IsFloat As Boolean
                    Get
                        Return m_isFloat
                    End Get
                    Set(value As Boolean)
                        m_isFloat = value
                    End Set
                End Property
            End Structure

            Private Structure PaneStruct
                Private m_dockState As DockState
                Public Property DockState As DockState
                    Get
                        Return m_dockState
                    End Get
                    Set(value As DockState)
                        m_dockState = value
                    End Set
                End Property

                Private m_indexActiveContent As Integer
                Public Property IndexActiveContent As Integer
                    Get
                        Return m_indexActiveContent
                    End Get
                    Set(value As Integer)
                        m_indexActiveContent = value
                    End Set
                End Property

                Private m_indexContents As Integer()
                Public Property IndexContents As Integer()
                    Get
                        Return m_indexContents
                    End Get
                    Set(value As Integer())
                        m_indexContents = value
                    End Set
                End Property

                Private m_zOrderIndex As Integer
                Public Property ZOrderIndex As Integer
                    Get
                        Return m_zOrderIndex
                    End Get
                    Set(value As Integer)
                        m_zOrderIndex = value
                    End Set
                End Property
            End Structure

            Private Structure NestedPane
                Private m_indexPane As Integer
                Public Property IndexPane As Integer
                    Get
                        Return m_indexPane
                    End Get
                    Set(value As Integer)
                        m_indexPane = value
                    End Set
                End Property

                Private m_indexPrevPane As Integer
                Public Property IndexPrevPane As Integer
                    Get
                        Return m_indexPrevPane
                    End Get
                    Set(value As Integer)
                        m_indexPrevPane = value
                    End Set
                End Property

                Private m_alignment As DockAlignment
                Public Property Alignment As DockAlignment
                    Get
                        Return m_alignment
                    End Get
                    Set(value As DockAlignment)
                        m_alignment = value
                    End Set
                End Property

                Private m_proportion As Double
                Public Property Proportion As Double
                    Get
                        Return m_proportion
                    End Get
                    Set(value As Double)
                        m_proportion = value
                    End Set
                End Property
            End Structure

            Private Structure DockWindowStruct
                Private m_dockState As DockState
                Public Property DockState As DockState
                    Get
                        Return m_dockState
                    End Get
                    Set(value As DockState)
                        m_dockState = value
                    End Set
                End Property

                Private m_zOrderIndex As Integer
                Public Property ZOrderIndex As Integer
                    Get
                        Return m_zOrderIndex
                    End Get
                    Set(value As Integer)
                        m_zOrderIndex = value
                    End Set
                End Property

                Private m_nestedPanes As NestedPane()
                Public Property NestedPanes As NestedPane()
                    Get
                        Return m_nestedPanes
                    End Get
                    Set(value As NestedPane())
                        m_nestedPanes = value
                    End Set
                End Property
            End Structure

            Private Structure FloatWindowStruct
                Private m_bounds As Rectangle
                Public Property Bounds As Rectangle
                    Get
                        Return m_bounds
                    End Get
                    Set(value As Rectangle)
                        m_bounds = value
                    End Set
                End Property

                Private m_zOrderIndex As Integer
                Public Property ZOrderIndex As Integer
                    Get
                        Return m_zOrderIndex
                    End Get
                    Set(value As Integer)
                        m_zOrderIndex = value
                    End Set
                End Property

                Private m_nestedPanes As NestedPane()
                Public Property NestedPanes As NestedPane()
                    Get
                        Return m_nestedPanes
                    End Get
                    Set(value As NestedPane())
                        m_nestedPanes = value
                    End Set
                End Property
            End Structure

            Public Shared Sub SaveAsXml(dockPanel As DockPanel, fileName As String)
                SaveAsXml(dockPanel, fileName, Encoding.Unicode)
            End Sub

            Public Shared Sub SaveAsXml(dockPanel As DockPanel, fileName As String, encoding As Encoding)
                Using fs = New FileStream(fileName, FileMode.Create)
                    Try
                        SaveAsXml(dockPanel, fs, encoding)
                    Finally
                        fs.Close()
                    End Try
                End Using
            End Sub

            Public Shared Sub SaveAsXml(dockPanel As DockPanel, stream As Stream, encoding As Encoding)
                SaveAsXml(dockPanel, stream, encoding, False)
            End Sub

            Public Shared Sub SaveAsXml(dockPanel As DockPanel, stream As Stream, encoding As Encoding, upstream As Boolean)
                Dim xmlOut As XmlWriter = XmlWriter.Create(stream, New XmlWriterSettings() With {
    .Encoding = encoding,
    .Indent = True,
    .OmitXmlDeclaration = upstream
})

                ' Always begin file with identification and warning
                xmlOut.WriteComment(Strings.DockPanel_Persistor_XmlFileComment1)
                xmlOut.WriteComment(Strings.DockPanel_Persistor_XmlFileComment2)

                ' Associate a version number with the root element so that future version of the code
                ' will be able to be backwards compatible or at least recognise out of date versions
                xmlOut.WriteStartElement("DockPanel")
                xmlOut.WriteAttributeString("FormatVersion", ConfigFileVersion)
                xmlOut.WriteAttributeString("DockLeftPortion", dockPanel.DockLeftPortion.ToString(CultureInfo.InvariantCulture))
                xmlOut.WriteAttributeString("DockRightPortion", dockPanel.DockRightPortion.ToString(CultureInfo.InvariantCulture))
                xmlOut.WriteAttributeString("DockTopPortion", dockPanel.DockTopPortion.ToString(CultureInfo.InvariantCulture))
                xmlOut.WriteAttributeString("DockBottomPortion", dockPanel.DockBottomPortion.ToString(CultureInfo.InvariantCulture))

                If Not IsRunningOnMono Then
                    xmlOut.WriteAttributeString("ActiveDocumentPane", dockPanel.Panes.IndexOf(dockPanel.ActiveDocumentPane).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteAttributeString("ActivePane", dockPanel.Panes.IndexOf(dockPanel.ActivePane).ToString(CultureInfo.InvariantCulture))
                End If

                ' Contents
                xmlOut.WriteStartElement("Contents")
                xmlOut.WriteAttributeString("Count", dockPanel.Contents.Count.ToString(CultureInfo.InvariantCulture))
                For Each content In dockPanel.Contents
                    xmlOut.WriteStartElement("Content")
                    xmlOut.WriteAttributeString("ID", dockPanel.Contents.IndexOf(content).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteAttributeString("PersistString", content.DockHandler.PersistString)
                    xmlOut.WriteAttributeString("AutoHidePortion", content.DockHandler.AutoHidePortion.ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteAttributeString("IsHidden", content.DockHandler.IsHidden.ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteAttributeString("IsFloat", content.DockHandler.IsFloat.ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteEndElement()
                Next
                xmlOut.WriteEndElement()

                ' Panes
                xmlOut.WriteStartElement("Panes")
                xmlOut.WriteAttributeString("Count", dockPanel.Panes.Count.ToString(CultureInfo.InvariantCulture))
                For Each pane In dockPanel.Panes
                    xmlOut.WriteStartElement("Pane")
                    xmlOut.WriteAttributeString("ID", dockPanel.Panes.IndexOf(pane).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteAttributeString("DockState", pane.DockState.ToString())
                    xmlOut.WriteAttributeString("ActiveContent", dockPanel.Contents.IndexOf(pane.ActiveContent).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteStartElement("Contents")
                    xmlOut.WriteAttributeString("Count", pane.Contents.Count.ToString(CultureInfo.InvariantCulture))
                    For Each content In pane.Contents
                        xmlOut.WriteStartElement("Content")
                        xmlOut.WriteAttributeString("ID", pane.Contents.IndexOf(content).ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteAttributeString("RefID", dockPanel.Contents.IndexOf(content).ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteEndElement()
                    Next
                    xmlOut.WriteEndElement()
                    xmlOut.WriteEndElement()
                Next
                xmlOut.WriteEndElement()

                ' DockWindows
                xmlOut.WriteStartElement("DockWindows")
                Dim dockWindowId = 0
                For Each dw In dockPanel.DockWindows
                    xmlOut.WriteStartElement("DockWindow")
                    xmlOut.WriteAttributeString("ID", dockWindowId.ToString(CultureInfo.InvariantCulture))
                    dockWindowId += 1
                    xmlOut.WriteAttributeString("DockState", dw.DockState.ToString())
                    xmlOut.WriteAttributeString("ZOrderIndex", dockPanel.Controls.IndexOf(dw).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteStartElement("NestedPanes")
                    xmlOut.WriteAttributeString("Count", dw.NestedPanes.Count.ToString(CultureInfo.InvariantCulture))
                    For Each pane In dw.NestedPanes
                        xmlOut.WriteStartElement("Pane")
                        xmlOut.WriteAttributeString("ID", dw.NestedPanes.IndexOf(pane).ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteAttributeString("RefID", dockPanel.Panes.IndexOf(pane).ToString(CultureInfo.InvariantCulture))
                        Dim status = pane.NestedDockingStatus
                        xmlOut.WriteAttributeString("PrevPane", dockPanel.Panes.IndexOf(status.PreviousPane).ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteAttributeString("Alignment", status.Alignment.ToString())
                        xmlOut.WriteAttributeString("Proportion", status.Proportion.ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteEndElement()
                    Next
                    xmlOut.WriteEndElement()
                    xmlOut.WriteEndElement()
                Next
                xmlOut.WriteEndElement()

                ' FloatWindows
                Dim rectConverter As RectangleConverter = New RectangleConverter()
                xmlOut.WriteStartElement("FloatWindows")
                xmlOut.WriteAttributeString("Count", dockPanel.FloatWindows.Count.ToString(CultureInfo.InvariantCulture))
                For Each fw In dockPanel.FloatWindows
                    xmlOut.WriteStartElement("FloatWindow")
                    xmlOut.WriteAttributeString("ID", dockPanel.FloatWindows.IndexOf(fw).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteAttributeString("Bounds", rectConverter.ConvertToInvariantString(If(fw.WindowState = FormWindowState.Minimized, fw.RestoreBounds, fw.Bounds)))
                    xmlOut.WriteAttributeString("ZOrderIndex", fw.DockPanel.FloatWindows.IndexOf(fw).ToString(CultureInfo.InvariantCulture))
                    xmlOut.WriteStartElement("NestedPanes")
                    xmlOut.WriteAttributeString("Count", fw.NestedPanes.Count.ToString(CultureInfo.InvariantCulture))
                    For Each pane In fw.NestedPanes
                        xmlOut.WriteStartElement("Pane")
                        xmlOut.WriteAttributeString("ID", fw.NestedPanes.IndexOf(pane).ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteAttributeString("RefID", dockPanel.Panes.IndexOf(pane).ToString(CultureInfo.InvariantCulture))
                        Dim status = pane.NestedDockingStatus
                        xmlOut.WriteAttributeString("PrevPane", dockPanel.Panes.IndexOf(status.PreviousPane).ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteAttributeString("Alignment", status.Alignment.ToString())
                        xmlOut.WriteAttributeString("Proportion", status.Proportion.ToString(CultureInfo.InvariantCulture))
                        xmlOut.WriteEndElement()
                    Next
                    xmlOut.WriteEndElement()
                    xmlOut.WriteEndElement()
                Next
                xmlOut.WriteEndElement()   '	</FloatWindows>

                xmlOut.WriteEndElement()

                If Not upstream Then
                    xmlOut.WriteEndDocument()
                    xmlOut.Close()
                Else
                    xmlOut.Flush()
                End If
            End Sub

            Public Shared Sub LoadFromXml(dockPanel As DockPanel, fileName As String, deserializeContent As DeserializeDockContent)
                Using fs = New FileStream(fileName, FileMode.Open, FileAccess.Read)
                    Try
                        LoadFromXml(dockPanel, fs, deserializeContent, True)
                    Finally
                        fs.Close()
                    End Try
                End Using
            End Sub

            Private Shared Function LoadContents(xmlIn As XmlTextReader) As ContentStruct()
                Dim countOfContents = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture)
                Dim contents = New ContentStruct(countOfContents - 1) {}
                MoveToNextElement(xmlIn)
                For i = 0 To countOfContents - 1
                    Dim id = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                    If Not Equals(xmlIn.Name, "Content") OrElse id <> i Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)

                    contents(i).PersistString = xmlIn.GetAttribute("PersistString")
                    contents(i).AutoHidePortion = Convert.ToDouble(xmlIn.GetAttribute("AutoHidePortion"), CultureInfo.InvariantCulture)
                    contents(i).IsHidden = Convert.ToBoolean(xmlIn.GetAttribute("IsHidden"), CultureInfo.InvariantCulture)
                    contents(i).IsFloat = Convert.ToBoolean(xmlIn.GetAttribute("IsFloat"), CultureInfo.InvariantCulture)
                    MoveToNextElement(xmlIn)
                Next

                Return contents
            End Function

            Private Shared Function LoadPanes(xmlIn As XmlTextReader) As PaneStruct()
                Dim dockStateConverter As EnumConverter = New EnumConverter(GetType(DockState))
                Dim countOfPanes = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture)
                Dim panes = New PaneStruct(countOfPanes - 1) {}
                MoveToNextElement(xmlIn)
                For i = 0 To countOfPanes - 1
                    Dim id = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                    If Not Equals(xmlIn.Name, "Pane") OrElse id <> i Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)

                    panes(i).DockState = CType(dockStateConverter.ConvertFrom(xmlIn.GetAttribute("DockState")), DockState)
                    panes(i).IndexActiveContent = Convert.ToInt32(xmlIn.GetAttribute("ActiveContent"), CultureInfo.InvariantCulture)
                    panes(i).ZOrderIndex = -1

                    MoveToNextElement(xmlIn)
                    If Not Equals(xmlIn.Name, "Contents") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    Dim countOfPaneContents = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture)
                    panes(i).IndexContents = New Integer(countOfPaneContents - 1) {}
                    MoveToNextElement(xmlIn)
                    For j = 0 To countOfPaneContents - 1
                        Dim id2 = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                        If Not Equals(xmlIn.Name, "Content") OrElse id2 <> j Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)

                        panes(i).IndexContents(j) = Convert.ToInt32(xmlIn.GetAttribute("RefID"), CultureInfo.InvariantCulture)
                        MoveToNextElement(xmlIn)
                    Next
                Next

                Return panes
            End Function

            Private Shared Function LoadDockWindows(xmlIn As XmlTextReader, dockPanel As DockPanel) As DockWindowStruct()
                Dim dockStateConverter As EnumConverter = New EnumConverter(GetType(DockState))
                Dim dockAlignmentConverter As EnumConverter = New EnumConverter(GetType(DockAlignment))
                Dim countOfDockWindows = dockPanel.DockWindows.Count
                Dim dockWindows = New DockWindowStruct(countOfDockWindows - 1) {}
                MoveToNextElement(xmlIn)
                For i = 0 To countOfDockWindows - 1
                    Dim id = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                    If Not Equals(xmlIn.Name, "DockWindow") OrElse id <> i Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)

                    dockWindows(i).DockState = CType(dockStateConverter.ConvertFrom(xmlIn.GetAttribute("DockState")), DockState)
                    dockWindows(i).ZOrderIndex = Convert.ToInt32(xmlIn.GetAttribute("ZOrderIndex"), CultureInfo.InvariantCulture)
                    MoveToNextElement(xmlIn)
                    If Not Equals(xmlIn.Name, "DockList") AndAlso Not Equals(xmlIn.Name, "NestedPanes") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    Dim countOfNestedPanes = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture)
                    dockWindows(i).NestedPanes = New NestedPane(countOfNestedPanes - 1) {}
                    MoveToNextElement(xmlIn)
                    For j = 0 To countOfNestedPanes - 1
                        Dim id2 = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                        If Not Equals(xmlIn.Name, "Pane") OrElse id2 <> j Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                        dockWindows(i).NestedPanes(j).IndexPane = Convert.ToInt32(xmlIn.GetAttribute("RefID"), CultureInfo.InvariantCulture)
                        dockWindows(i).NestedPanes(j).IndexPrevPane = Convert.ToInt32(xmlIn.GetAttribute("PrevPane"), CultureInfo.InvariantCulture)
                        dockWindows(i).NestedPanes(j).Alignment = CType(dockAlignmentConverter.ConvertFrom(xmlIn.GetAttribute("Alignment")), DockAlignment)
                        dockWindows(i).NestedPanes(j).Proportion = Convert.ToDouble(xmlIn.GetAttribute("Proportion"), CultureInfo.InvariantCulture)
                        MoveToNextElement(xmlIn)
                    Next
                Next

                Return dockWindows
            End Function

            Private Shared Function LoadFloatWindows(xmlIn As XmlTextReader) As FloatWindowStruct()
                Dim dockAlignmentConverter As EnumConverter = New EnumConverter(GetType(DockAlignment))
                Dim rectConverter As RectangleConverter = New RectangleConverter()
                Dim countOfFloatWindows = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture)
                Dim floatWindows = New FloatWindowStruct(countOfFloatWindows - 1) {}
                MoveToNextElement(xmlIn)
                For i = 0 To countOfFloatWindows - 1
                    Dim id = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                    If Not Equals(xmlIn.Name, "FloatWindow") OrElse id <> i Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)

                    floatWindows(i).Bounds = CType(rectConverter.ConvertFromInvariantString(xmlIn.GetAttribute("Bounds")), Rectangle)
                    floatWindows(i).ZOrderIndex = Convert.ToInt32(xmlIn.GetAttribute("ZOrderIndex"), CultureInfo.InvariantCulture)
                    MoveToNextElement(xmlIn)
                    If Not Equals(xmlIn.Name, "DockList") AndAlso Not Equals(xmlIn.Name, "NestedPanes") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    Dim countOfNestedPanes = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture)
                    floatWindows(i).NestedPanes = New NestedPane(countOfNestedPanes - 1) {}
                    MoveToNextElement(xmlIn)
                    For j = 0 To countOfNestedPanes - 1
                        Dim id2 = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture)
                        If Not Equals(xmlIn.Name, "Pane") OrElse id2 <> j Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                        floatWindows(i).NestedPanes(j).IndexPane = Convert.ToInt32(xmlIn.GetAttribute("RefID"), CultureInfo.InvariantCulture)
                        floatWindows(i).NestedPanes(j).IndexPrevPane = Convert.ToInt32(xmlIn.GetAttribute("PrevPane"), CultureInfo.InvariantCulture)
                        floatWindows(i).NestedPanes(j).Alignment = CType(dockAlignmentConverter.ConvertFrom(xmlIn.GetAttribute("Alignment")), DockAlignment)
                        floatWindows(i).NestedPanes(j).Proportion = Convert.ToDouble(xmlIn.GetAttribute("Proportion"), CultureInfo.InvariantCulture)
                        MoveToNextElement(xmlIn)
                    Next
                Next

                Return floatWindows
            End Function

            Public Shared Sub LoadFromXml(dockPanel As DockPanel, stream As Stream, deserializeContent As DeserializeDockContent, closeStream As Boolean)
                If dockPanel.Contents.Count <> 0 Then Throw New InvalidOperationException(Strings.DockPanel_LoadFromXml_AlreadyInitialized)

                Dim dockPanelStruct As DockPanelStruct
                Dim contents As ContentStruct()
                Dim panes As PaneStruct()
                Dim dockWindows As DockWindowStruct()
                Dim floatWindows As FloatWindowStruct()
                Using xmlIn = New XmlTextReader(stream) With {
                    .WhitespaceHandling = WhitespaceHandling.None
                }
                    xmlIn.MoveToContent()

                    While Not xmlIn.Name.Equals("DockPanel")
                        If Not MoveToNextElement(xmlIn) Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    End While

                    Dim formatVersion = xmlIn.GetAttribute("FormatVersion")
                    If Not IsFormatVersionValid(formatVersion) Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidFormatVersion)

                    dockPanelStruct = New DockPanelStruct()
                    dockPanelStruct.DockLeftPortion = Convert.ToDouble(xmlIn.GetAttribute("DockLeftPortion"), CultureInfo.InvariantCulture)
                    dockPanelStruct.DockRightPortion = Convert.ToDouble(xmlIn.GetAttribute("DockRightPortion"), CultureInfo.InvariantCulture)
                    dockPanelStruct.DockTopPortion = Convert.ToDouble(xmlIn.GetAttribute("DockTopPortion"), CultureInfo.InvariantCulture)
                    dockPanelStruct.DockBottomPortion = Convert.ToDouble(xmlIn.GetAttribute("DockBottomPortion"), CultureInfo.InvariantCulture)
                    dockPanelStruct.IndexActiveDocumentPane = Convert.ToInt32(xmlIn.GetAttribute("ActiveDocumentPane"), CultureInfo.InvariantCulture)
                    dockPanelStruct.IndexActivePane = Convert.ToInt32(xmlIn.GetAttribute("ActivePane"), CultureInfo.InvariantCulture)

                    ' Load Contents
                    MoveToNextElement(xmlIn)
                    If Not Equals(xmlIn.Name, "Contents") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    contents = LoadContents(xmlIn)

                    ' Load Panes
                    If Not Equals(xmlIn.Name, "Panes") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    panes = LoadPanes(xmlIn)

                    ' Load DockWindows
                    If Not Equals(xmlIn.Name, "DockWindows") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    dockWindows = LoadDockWindows(xmlIn, dockPanel)

                    ' Load FloatWindows
                    If Not Equals(xmlIn.Name, "FloatWindows") Then Throw New ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat)
                    floatWindows = LoadFloatWindows(xmlIn)

                    If closeStream Then xmlIn.Close()
                End Using

                dockPanel.SuspendLayout(True)

                dockPanel.DockLeftPortion = dockPanelStruct.DockLeftPortion
                dockPanel.DockRightPortion = dockPanelStruct.DockRightPortion
                dockPanel.DockTopPortion = dockPanelStruct.DockTopPortion
                dockPanel.DockBottomPortion = dockPanelStruct.DockBottomPortion

                ' Set DockWindow ZOrders
                Dim prevMaxDockWindowZOrder = Integer.MaxValue
                For i = 0 To dockWindows.Length - 1
                    Dim maxDockWindowZOrder = -1
                    Dim index = -1
                    For j = 0 To dockWindows.Length - 1
                        If dockWindows(j).ZOrderIndex > maxDockWindowZOrder AndAlso dockWindows(j).ZOrderIndex < prevMaxDockWindowZOrder Then
                            maxDockWindowZOrder = dockWindows(j).ZOrderIndex
                            index = j
                        End If
                    Next

                    dockPanel.DockWindows(dockWindows(index).DockState).BringToFront()
                    prevMaxDockWindowZOrder = maxDockWindowZOrder
                Next

                ' Create Contents
                For i = 0 To contents.Length - 1
                    Dim content = deserializeContent(contents(i).PersistString)
                    If content Is Nothing Then content = New DummyContent()
                    content.DockHandler.DockPanel = dockPanel
                    content.DockHandler.AutoHidePortion = contents(i).AutoHidePortion
                    content.DockHandler.IsHidden = True
                    content.DockHandler.IsFloat = contents(i).IsFloat
                Next

                ' Create panes
                For i = 0 To panes.Length - 1
                    Dim pane As DockPane = Nothing
                    For j = 0 To panes(i).IndexContents.Length - 1
                        Dim content = dockPanel.Contents(panes(i).IndexContents(j))
                        If j = 0 Then
                            pane = dockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(content, panes(i).DockState, False)
                        ElseIf panes(i).DockState = DockState.Float Then
                            content.DockHandler.FloatPane = pane
                        Else
                            content.DockHandler.PanelPane = pane
                        End If
                    Next
                Next

                ' Assign Panes to DockWindows
                For i = 0 To dockWindows.Length - 1
                    For j = 0 To dockWindows(i).NestedPanes.Length - 1
                        Dim dw = dockPanel.DockWindows(dockWindows(i).DockState)
                        Dim indexPane = dockWindows(i).NestedPanes(j).IndexPane
                        Dim pane = dockPanel.Panes(indexPane)
                        Dim indexPrevPane = dockWindows(i).NestedPanes(j).IndexPrevPane
                        Dim prevPane = If(indexPrevPane = -1, dw.NestedPanes.GetDefaultPreviousPane(pane), dockPanel.Panes(indexPrevPane))
                        Dim alignment = dockWindows(i).NestedPanes(j).Alignment
                        Dim proportion = dockWindows(i).NestedPanes(j).Proportion
                        pane.DockTo(dw, prevPane, alignment, proportion)
                        If panes(indexPane).DockState = dw.DockState Then panes(indexPane).ZOrderIndex = dockWindows(i).ZOrderIndex
                    Next
                Next

                ' Create float windows
                For i = 0 To floatWindows.Length - 1
                    Dim fw As FloatWindow = Nothing
                    For j = 0 To floatWindows(i).NestedPanes.Length - 1
                        Dim indexPane = floatWindows(i).NestedPanes(j).IndexPane
                        Dim pane = dockPanel.Panes(indexPane)
                        If j = 0 Then
                            fw = dockPanel.Theme.Extender.FloatWindowFactory.CreateFloatWindow(dockPanel, pane, floatWindows(i).Bounds)
                        Else
                            Dim indexPrevPane = floatWindows(i).NestedPanes(j).IndexPrevPane
                            Dim prevPane = If(indexPrevPane = -1, Nothing, dockPanel.Panes(indexPrevPane))
                            Dim alignment = floatWindows(i).NestedPanes(j).Alignment
                            Dim proportion = floatWindows(i).NestedPanes(j).Proportion
                            pane.DockTo(fw, prevPane, alignment, proportion)
                        End If

                        If panes(indexPane).DockState = fw.DockState Then panes(indexPane).ZOrderIndex = floatWindows(i).ZOrderIndex
                    Next
                Next

                ' sort IDockContent by its Pane's ZOrder
                Dim sortedContents As Integer() = Nothing
                If contents.Length > 0 Then
                    sortedContents = New Integer(contents.Length - 1) {}
                    For i = 0 To contents.Length - 1
                        sortedContents(i) = i
                    Next

                    Dim lastDocument = contents.Length
                    For i = 0 To contents.Length - 1 - 1
                        For j = i + 1 To contents.Length - 1
                            Dim pane1 = dockPanel.Contents(sortedContents(i)).DockHandler.Pane
                            Dim ZOrderIndex1 = If(pane1 Is Nothing, 0, panes(dockPanel.Panes.IndexOf(pane1)).ZOrderIndex)
                            Dim pane2 = dockPanel.Contents(sortedContents(j)).DockHandler.Pane
                            Dim ZOrderIndex2 = If(pane2 Is Nothing, 0, panes(dockPanel.Panes.IndexOf(pane2)).ZOrderIndex)
                            If ZOrderIndex1 > ZOrderIndex2 Then
                                Dim temp = sortedContents(i)
                                sortedContents(i) = sortedContents(j)
                                sortedContents(j) = temp
                            End If
                        Next
                    Next
                End If

                ' show non-document IDockContent first to avoid screen flickers
                For i = 0 To contents.Length - 1
                    Dim content = dockPanel.Contents(sortedContents(i))
                    If content.DockHandler.Pane IsNot Nothing AndAlso content.DockHandler.Pane.DockState <> DockState.Document Then
                        content.DockHandler.SuspendAutoHidePortionUpdates = True
                        content.DockHandler.IsHidden = contents(sortedContents(i)).IsHidden
                        content.DockHandler.SuspendAutoHidePortionUpdates = False
                    End If
                Next

                ' after all non-document IDockContent, show document IDockContent
                For i = 0 To contents.Length - 1
                    Dim content = dockPanel.Contents(sortedContents(i))
                    If content.DockHandler.Pane IsNot Nothing AndAlso content.DockHandler.Pane.DockState = DockState.Document Then
                        content.DockHandler.SuspendAutoHidePortionUpdates = True
                        content.DockHandler.IsHidden = contents(sortedContents(i)).IsHidden
                        content.DockHandler.SuspendAutoHidePortionUpdates = False
                    End If
                Next

                For i = 0 To panes.Length - 1
                    dockPanel.Panes(i).ActiveContent = If(panes(i).IndexActiveContent = -1, Nothing, dockPanel.Contents(panes(i).IndexActiveContent))
                Next

                If dockPanelStruct.IndexActiveDocumentPane >= 0 AndAlso dockPanel.Panes.Count > dockPanelStruct.IndexActiveDocumentPane Then dockPanel.Panes(dockPanelStruct.IndexActiveDocumentPane).Activate()

                If dockPanelStruct.IndexActivePane >= 0 AndAlso dockPanel.Panes.Count > dockPanelStruct.IndexActivePane Then dockPanel.Panes(dockPanelStruct.IndexActivePane).Activate()

                For i = dockPanel.Contents.Count - 1 To 0 Step -1
                    If TypeOf dockPanel.Contents(i) Is DummyContent Then dockPanel.Contents(i).DockHandler.Form.Close()
                Next

                dockPanel.ResumeLayout(True, True)
            End Sub

            Private Shared Function MoveToNextElement(xmlIn As XmlTextReader) As Boolean
                If Not xmlIn.Read() Then Return False

                While xmlIn.NodeType = XmlNodeType.EndElement
                    If Not xmlIn.Read() Then Return False
                End While

                Return True
            End Function

            Private Shared Function IsFormatVersionValid(formatVersion As String) As Boolean
                If Equals(formatVersion, ConfigFileVersion) Then Return True

                For Each s In CompatibleConfigFileVersions
                    If Equals(s, formatVersion) Then Return True
                Next

                Return False
            End Function
        End Class

        Public Sub SaveAsXml(fileName As String)
            Persistor.SaveAsXml(Me, fileName)
        End Sub

        Public Sub SaveAsXml(fileName As String, encoding As Encoding)
            Persistor.SaveAsXml(Me, fileName, encoding)
        End Sub

        Public Sub SaveAsXml(stream As Stream, encoding As Encoding)
            Persistor.SaveAsXml(Me, stream, encoding)
        End Sub

        Public Sub SaveAsXml(stream As Stream, encoding As Encoding, upstream As Boolean)
            Persistor.SaveAsXml(Me, stream, encoding, upstream)
        End Sub

        ''' <summary>
        ''' Loads layout from XML file.
        ''' </summary>
        ''' <param name="fileName">The file name.</param>
        ''' <param name="deserializeContent">Deserialization handler.</param>
        ''' <exception cref="Exception">Deserialization might throw exceptions.</exception>
        Public Sub LoadFromXml(fileName As String, deserializeContent As DeserializeDockContent)
            Persistor.LoadFromXml(Me, fileName, deserializeContent)
        End Sub

        ''' <summary>
        ''' Loads layout from a stream.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="deserializeContent">Deserialization handler.</param>
        ''' <exception cref="Exception">Deserialization might throw exceptions.</exception>
        ''' <remarks>
        ''' The stream is closed after deserialization.
        ''' </remarks>
        Public Sub LoadFromXml(stream As Stream, deserializeContent As DeserializeDockContent)
            Persistor.LoadFromXml(Me, stream, deserializeContent, True)
        End Sub

        ''' <summary>
        ''' Loads layout from a stream.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="deserializeContent">Deserialization handler.</param>
        ''' <param name="closeStream">The flag to close the stream after deserialization.</param>
        ''' <exception cref="Exception">Deserialization might throw exceptions.</exception>
        Public Sub LoadFromXml(stream As Stream, deserializeContent As DeserializeDockContent, closeStream As Boolean)
            Persistor.LoadFromXml(Me, stream, deserializeContent, closeStream)
        End Sub
    End Class
End Namespace
