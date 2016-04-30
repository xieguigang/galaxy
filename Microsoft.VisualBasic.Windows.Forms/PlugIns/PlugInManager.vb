Imports System.Windows.Forms
Imports System.Text
Imports System.Drawing
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Language.UnixBash

Namespace PlugIns

    Public Class PlugInManager : Inherits ITextFile

        ''' <summary>
        ''' The file path for the disabled plugin assembly module.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlElement> Public Property DisabledPlugIns As List(Of String)

        Protected PlugInList As New List(Of PlugInEntry)

        Public Shared Function LoadPlugins(Menu As MenuStrip, pluginDIR As String, ProfileXml As String) As PlugInManager
            Dim PluginManager As PlugInManager = PlugInManager.Load(ProfileXml)
            Dim LoadFileList As String() =
                LinqAPI.Exec(Of String) <= From path As String
                                           In ls - l - wildcards("*.dll", "*.exe") <= pluginDIR
                                           Where PluginManager.DisabledPlugIns.IndexOf(path) = -1
                                           Select path ' Get the load plugin file list, ignore the plugin file which is in disabled plugin list.

            PluginManager.PlugInList += From PlugInAssembly As String
                                        In LoadFileList
                                        Select PlugIns.LoadPlugIn(Menu, PlugInAssembly)
            PluginManager.PlugInList += From PlugInAssembly As String
                                        In PluginManager.DisabledPlugIns
                                        Select PlugInLoader.LoadMainModules(PlugInAssembly)
            PluginManager.PlugInList -= Function(PlugInEntry) PlugInEntry Is Nothing

            Return PluginManager
        End Function

        Public Function IsDisabled(path As String) As Boolean
            Return DisabledPlugIns.IndexOf(path) > -1
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
                Dim Item As ListViewItem = New ListViewItem({PlugIn.base.Name, PlugIn.base.Description, PlugIn.AssemblyPath})
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