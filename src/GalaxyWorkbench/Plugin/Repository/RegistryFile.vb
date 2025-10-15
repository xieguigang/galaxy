Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Text.Xml.Models

Namespace Plugin.Repository

    Public Class RegistryFile : Inherits ListOf(Of PluginMetadata)

        <XmlElement>
        Public Property plugins As PluginMetadata()

        Friend Shared ReadOnly defaultLocation As String = App.LocalData & "/plugins.xml"

        Default Friend ReadOnly Property GetElementById(id As String) As PluginMetadata
            Get
                Return plugins.KeyItem(id)
            End Get
        End Property

        Public Function IsDisabled(id As String) As Boolean
            Dim plugin As PluginMetadata = Me(id)

            If Not plugin Is Nothing Then
                Return plugin.status = "disable"
            Else
                Return False
            End If
        End Function

        Public Sub Save()
            Call Me.GetXml.SaveTo(defaultLocation)
        End Sub

        Protected Overrides Function getSize() As Integer
            Return plugins.TryCount
        End Function

        Protected Overrides Function getCollection() As IEnumerable(Of PluginMetadata)
            Return plugins
        End Function

        Public Shared Function LoadRegistry() As RegistryFile
            Dim registry = RegistryFile.defaultLocation.LoadXml(Of RegistryFile)(throwEx:=False)
            If registry Is Nothing Then
                registry = New RegistryFile With {.plugins = {}}
                registry.Save()
            End If
            Return registry
        End Function
    End Class
End Namespace