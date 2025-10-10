Imports System
Imports System.ComponentModel

Namespace WeifenLuo.WinFormsUI.Docking
    <AttributeUsage(AttributeTargets.All)>
    Friend NotInheritable Class LocalizedDescriptionAttribute
        Inherits DescriptionAttribute
        Private m_initialized As Boolean = False

        Public Sub New(key As String)
            MyBase.New(key)
        End Sub

        Public Overrides ReadOnly Property Description As String
            Get
                If Not m_initialized Then
                    Dim key = MyBase.Description
                    DescriptionValue = GetString(key)
                    If Equals(DescriptionValue, Nothing) Then DescriptionValue = String.Empty

                    m_initialized = True
                End If

                Return DescriptionValue
            End Get
        End Property
    End Class

    <AttributeUsage(AttributeTargets.All)>
    Friend NotInheritable Class LocalizedCategoryAttribute
        Inherits CategoryAttribute
        Public Sub New(key As String)
            MyBase.New(key)
        End Sub

        Protected Overrides Function GetLocalizedString(value As String) As String
            Return GetString(value)
        End Function
    End Class
End Namespace
