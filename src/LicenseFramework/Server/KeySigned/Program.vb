Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/generate")>
    <Description("generates the software license file")>
    <Usage("/generate --data <request.txt> --mysqli <mysqli.txt>")>
    Public Function GenerateLicense(data As String, mysqli As String, args As CommandLine) As Integer

    End Function
End Module
