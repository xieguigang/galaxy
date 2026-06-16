Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("/generate")>
    <Description("generates the software license file")>
    <Usage("/generate --data <request.txt> --mysqli <mysqli.txt> [--outfile <license.lic>]")>
    <Argument("--data", , CLITypes.File, PipelineTypes.std_in, Description:="the license request file")>
    <Argument("--mysqli",, CLITypes.File, Description:="the text file that contains the mysql connection uri string.")>
    <Argument("--outfile",, CLITypes.File, PipelineTypes.std_out, Description:="the generated license file.")>
    Public Function GenerateLicense(data As String, mysqli As String, args As CommandLine) As Integer

    End Function
End Module
