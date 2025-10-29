#Region "Microsoft.VisualBasic::0224131679af9a5d60b7c7f3645e36d2, mzkit\src\mzkit\Task\Studio\WorkStudio.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 6
'    Code Lines: 5
' Comment Lines: 0
'   Blank Lines: 1
'     File Size: 177.00 B


' Class WorkStudio
' 
'     Sub: RunTaskScript
' 
' /********************************************************************************/

#End Region

Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Microsoft.VisualBasic.Text

Public NotInheritable Class WorkStudio

    Shared ReadOnly logfile As String = $"{App.ProductProgramData}/pipeline_calls_{Now.ToString("yyyy-MM")}.txt"

    Private Sub New()
    End Sub

    Private Shared Function CreateLogger() As LogFile
        Return New LogFile(path:=logfile, autoFlush:=True, append:=True, appendHeader:=False)
    End Function

    Public Shared Sub LogCommandLine(cmd As Process)
        Call LogCommandLine(cmd.StartInfo.FileName, cmd.StartInfo.Arguments, cmd.StartInfo.WorkingDirectory)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="host">the commandline application</param>
    ''' <param name="commandline">the commandline argument string</param>
    ''' <param name="workdir">the workdir of the application</param>
    Public Shared Sub LogCommandLine(host As String, commandline As String, workdir As String)
        Dim logText As New StringBuilder

        Call logText.AppendLine($"host: {host}")
        Call logText.AppendLine($"arguments: {commandline}")
        Call logText.AppendLine($"workdir: {workdir}")
        Call logText.AppendLine()
        Call logText.AppendLine($"***** full workflow commandline *****")
        Call logText.AppendLine()
        Call logText.AppendLine($"cd {workdir.CLIPath}")
        Call logText.AppendLine($"{host.CLIPath} {commandline.TrimNewLine}")
        Call logText.AppendLine()

        Using log As LogFile = CreateLogger()
            Call log.log(MSG_TYPES.DEBUG, logText)
        End Using
    End Sub

    Public Shared Function TryParse(log As String) As (cd As String, cmd As String)
        Dim lines As String() = Strings.Trim(log) _
            .LineTokens _
            .Where(Function(si) Not si.StartsWith("//")) _
            .Where(Function(si) Not Strings.Trim(si).StringEmpty(, True)) _
            .ToArray
        Dim cd As String = lines(lines.Length - 2)
        Dim cmd As String = lines(lines.Length - 1)

        cd = cd.GetTagValue(" ").Value.Trim(""""c)

        Return (cd, cmd)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cmdlog">
    ''' log value parsed from the <see cref="LogReader.Parse"/>
    ''' </param>
    Public Shared Sub launch_cmd(cmdlog As LogEntry)
        Dim run = TryParse(cmdlog.message)
        Dim batch As New StringBuilder($"{run.Item1.Split(":"c).First}:" & vbCrLf & vbCrLf)
        batch.AppendLine("CD " & run.Item1.CLIPath)
        batch.AppendLine(run.Item2)

        Dim batch_file As String = App.GetTempFile & ".cmd"
        Dim cmd As New Process
        cmd.StartInfo.FileName = "cmd.exe"
        cmd.StartInfo.Arguments = "/k " & batch_file.CLIPath
        cmd.StartInfo.CreateNoWindow = False

        Call batch.ToString.SaveTo(batch_file, Encodings.UTF8WithoutBOM.CodePage)
        Call cmd.Start()
    End Sub
End Class

