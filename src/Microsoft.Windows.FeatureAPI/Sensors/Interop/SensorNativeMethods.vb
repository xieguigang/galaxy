' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System.Runtime.InteropServices

Namespace Sensors

	Friend NotInheritable Class SensorNativeMethods
		Private Sub New()
		End Sub
		<DllImport("kernel32.dll")> _
		Friend Shared Function SystemTimeToFileTime(ByRef lpSystemTime As SystemTime, ByRef lpFileTime As System.Runtime.InteropServices.ComTypes.FILETIME) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function
	End Class
End Namespace
