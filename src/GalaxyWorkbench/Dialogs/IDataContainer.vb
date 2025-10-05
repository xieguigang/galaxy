''' <summary>
''' <see cref="DialogPromise"/>中可选的数据传递接口
''' </summary>
Public Interface IDataContainer

    ''' <summary>
    ''' set a general data to form
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetData(data As Object)

    ''' <summary>
    ''' get the general data from the form
    ''' </summary>
    ''' <returns></returns>
    Function GetData() As Object

End Interface