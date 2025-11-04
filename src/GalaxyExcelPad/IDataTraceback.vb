Public Interface IDataTraceback

    Property SourceName As String
    Property InstanceGuid As String

    ''' <summary>
    ''' the application object reference type of the source application
    ''' </summary>
    ''' <returns></returns>
    Property AppSource As Type

End Interface
