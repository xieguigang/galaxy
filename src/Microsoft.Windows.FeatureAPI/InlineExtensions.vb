Imports System.Runtime.CompilerServices

Module InlineExtensions

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="copyTo"></param>
    ''' <param name="source">This copy's data source.</param>
    ''' <returns></returns>
    <Extension> Public Function DirectCopy(Of T)(ByRef copyTo As T, ByRef source As T) As T
        copyTo = source
        Return source
    End Function
End Module
