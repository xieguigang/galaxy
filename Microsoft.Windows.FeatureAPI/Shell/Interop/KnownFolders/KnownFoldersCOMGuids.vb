'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	Friend NotInheritable Class KnownFoldersIIDGuid
		Private Sub New()
		End Sub
		' IID GUID strings for relevant Shell COM interfaces.
		Friend Const IKnownFolder As String = "3AA7AF7E-9B36-420c-A8E3-F77D4674A488"
		Friend Const IKnownFolderManager As String = "8BE2D872-86AA-4d47-B776-32CCA40C7018"
	End Class

	Friend NotInheritable Class KnownFoldersCLSIDGuid
		Private Sub New()
		End Sub
		' CLSID GUID strings for relevant coclasses.
		Friend Const KnownFolderManager As String = "4df0c730-df9d-4ae3-9153-aa6b82e9795a"
	End Class

	Friend NotInheritable Class KnownFoldersKFIDGuid
		Private Sub New()
		End Sub
		Friend Const ComputerFolder As String = "0AC0837C-BBF8-452A-850D-79D08E667CA7"
		Friend Const Favorites As String = "1777F761-68AD-4D8A-87BD-30B759FA33DD"
		Friend Const Documents As String = "FDD39AD0-238F-46AF-ADB4-6C85480369C7"
		Friend Const Profile As String = "5E6C858F-0E22-4760-9AFE-EA3317B67173"
	End Class
End Namespace
