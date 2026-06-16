Public Module FingerprintParser

    Public Function Parse(content As String) As String
        ' 尝试从文件中提取指纹哈希
        Dim startMarker = "硬件指纹哈希 (Hardware Fingerprint Hash):"
        Dim idx = content.IndexOf(startMarker)
        Dim endIdx As Integer

        If idx >= 0 Then
            idx += startMarker.Length
            endIdx = content.IndexOf("---", idx)

            If endIdx >= 0 Then
                Return content.Substring(idx, endIdx - idx).Trim
            End If
        Else
            Return content.Trim
        End If

        Return Nothing
    End Function
End Module
