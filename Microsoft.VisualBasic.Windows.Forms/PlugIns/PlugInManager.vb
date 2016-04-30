Imports System.Windows.Forms
Imports System.Text
Imports System.Drawing
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.ComponentModel

Namespace PlugIns

    Public Class PlugInManager : Inherits ITextFile

        ''' <summary>
        ''' The file path for the disabled plugin assembly module.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlElement> Public Property DisabledPlugIns As List(Of String)

        Protected Friend PlugInList As List(Of PlugInEntry) = New List(Of PlugInEntry)

        Public Shared Function LoadPlugins(Menu As MenuStrip, pluginDIR As String, ProfileXml As String) As PlugInManager
            Dim PluginManager As PlugInManager = PlugInManager.Load(ProfileXml)
            Dim LoadFileList = (From Path As String In FileIO.FileSystem.GetFiles(pluginDIR, FileIO.SearchOption.SearchTopLevelOnly, "*.dll", "*.exe")
                                Where PluginManager.DisabledPlugIns.IndexOf(Path) = -1
                                Select Path).ToArray 'Get the load plugin file list, ignore the plugin file which is in disabled plugin list.

            Call PluginManager.PlugInList.AddRange((From PlugInAssembly As String In LoadFileList Select PlugIns.LoadPlugIn(Menu, PlugInAssembly)).ToArray)
            Call PluginManager.PlugInList.AddRange((From PlugInAssembly As String In PluginManager.DisabledPlugIns Select PlugInLoader.LoadMainModules(PlugInAssembly)).ToArray)
            Call PluginManager.PlugInList.RemoveAll(Function(PlugInEntry) PlugInEntry Is Nothing)

            Return PluginManager
        End Function

        Public Function IsDisabled(AssemblyPath As String) As Boolean
            Return DisabledPlugIns.IndexOf(AssemblyPath) > -1
        End Function

        Public Sub ShowDialog(Optional ShowWarnDialog As Boolean = True)
            Call New PlugInManagerGUI() With {.PlugInManager = Me}.ShowDialog()
            Call Save()
            If ShowWarnDialog Then
                MsgBox("You should restart this program to makes the changes take effect.", MsgBoxStyle.Information)
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return $"{FilePath.ToFileURL}, {DisabledPlugIns.Count} plugins is disabled by user."
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Xml">XML文件的文件路径</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Load(Xml As String) As PlugInManager
            Dim PlugInManager As PlugInManager =
                Xml.LoadTextDoc(Of PlugInManager)(ThrowEx:=False)

            If PlugInManager Is Nothing Then
                PlugInManager = New PlugInManager With {
                    .FilePath = Xml
                }
            End If

            If PlugInManager.DisabledPlugIns Is Nothing Then
                PlugInManager.DisabledPlugIns = New List(Of String)
            End If

            Return PlugInManager
        End Function

        Public Sub LoadPlugIns(ListView As ListView, ImageList As ImageList)
            Dim Index As Integer
            ListView.LargeImageList = ImageList
            ListView.SmallImageList = ImageList

            For Each PlugIn As PlugInEntry In PlugInList
                Dim Icon = CType(PlugIn.IconImage, Bitmap)
                Dim Item As ListViewItem = New ListViewItem({PlugIn.Name, PlugIn.Description, PlugIn.AssemblyPath})
                Item.Checked = Not IsDisabled(PlugIn.AssemblyPath)

                Call ListView.Items.Add(Item)

                If Not Icon Is Nothing Then
                    Call ImageList.Images.Add(Icon)
                    Item.ImageIndex = Index
                    Index += 1
                End If
            Next
        End Sub

        Public Shared Function GetDisabledPlugIns(ListView As ListView, PlugInManager As PlugInManager) As List(Of String)
            Return LinqAPI.MakeList(Of String) <=
                From item As ListViewItem
                In ListView.Items
                Where Not item.Checked
                Select PlugInManager.PlugInList(item.Index).AssemblyPath '
        End Function

        Public Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            Return Me.GetXml.SaveTo(getPath(FilePath), Encoding)
        End Function
    End Class
End Namespace