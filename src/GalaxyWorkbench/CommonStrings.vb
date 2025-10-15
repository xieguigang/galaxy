Imports System.Globalization
Imports System.Resources
Imports System.Runtime.CompilerServices

''' <summary>
''' Common string resource in different culture and language
''' </summary>
Public Module CommonStrings

    ReadOnly languages As String() = {"zh", "en"}

    Dim lang As Languages = Workbench.Languages.Default

    Public ReadOnly Property language As String
        Get
            If lang = Workbench.Languages.Default Then
                Dim currentCulture As CultureInfo = CultureInfo.CurrentCulture

                For Each flag As String In languages
                    If currentCulture.Name.StartsWith(flag) Then
                        Return flag
                    End If
                Next

                Return Nothing
            Else
                Return lang.Description
            End If
        End Get
    End Property

    Public Sub SetLanguage(lang As Languages)
        CommonStrings.lang = lang
    End Sub

    ''' <summary>
    ''' get string from resource data with culture and language configuration.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    Public Function GetString(key As String, Optional res As ResourceManager = Nothing) As String
        Dim lang As String = language
        Dim key_lang As String = If(lang Is Nothing, key, $"{key}_{lang}")
        Dim str As String = If(res, My.Resources.ResourceManager).GetString(key_lang)

        If str Is Nothing Then
            str = If(res, My.Resources.ResourceManager).GetString(key)
        End If

        Return str
    End Function

    <Extension>
    Public Function GetStringValue(res As ResourceManager, key As String, ParamArray args As Object()) As String
        Return String.Format(GetString(key, res), args)
    End Function

    ''' <summary>
    ''' Get string with <see cref="System.String.Format"/> applied
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetString(key As String, ParamArray args As Object()) As String
        Return String.Format(GetString(key), args)
    End Function

End Module
