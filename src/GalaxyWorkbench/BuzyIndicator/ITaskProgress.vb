Public Interface ITaskProgress

    ReadOnly Property TaskCanceled As Boolean

    Sub SetProgressMode()
    ''' <summary>
    ''' progress value between [0,100]
    ''' </summary>
    ''' <param name="p">[0,100]</param>
    Sub SetProgress(p As Integer)
    ''' <summary>
    ''' progress value between [0,100]
    ''' </summary>
    ''' <param name="p">[0,100]</param>
    ''' <param name="msg"></param>
    Sub SetProgress(p As Integer, msg As String)
    Sub SetTitle(title As String)
    Sub SetInfo(message As String)
    Sub TaskFinish()

End Interface