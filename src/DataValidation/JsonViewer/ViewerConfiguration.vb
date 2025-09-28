Imports System.Configuration

Namespace JSON

    Public Class ViewerConfiguration : Inherits ConfigurationSection

        <ConfigurationProperty("plugins")>
        Public ReadOnly Property Plugins As KeyValueConfigurationCollection
            Get
                Return CType(MyBase.Item("plugins"), KeyValueConfigurationCollection)
            End Get
        End Property
    End Class

End Namespace