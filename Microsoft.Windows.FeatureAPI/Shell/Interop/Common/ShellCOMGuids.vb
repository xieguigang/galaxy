'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Shell
	Friend NotInheritable Class ShellIIDGuid
		Private Sub New()
		End Sub

		' IID GUID strings for relevant Shell COM interfaces.
		Friend Const IModalWindow As String = "B4DB1657-70D7-485E-8E3E-6FCB5A5C1802"
		Friend Const IFileDialog As String = "42F85136-DB7E-439C-85F1-E4075D135FC8"
		Friend Const IFileOpenDialog As String = "D57C7288-D4AD-4768-BE02-9D969532D960"
		Friend Const IFileSaveDialog As String = "84BCCD23-5FDE-4CDB-AEA4-AF64B83D78AB"
		Friend Const IFileDialogEvents As String = "973510DB-7D7F-452B-8975-74A85828D354"
		Friend Const IFileDialogControlEvents As String = "36116642-D713-4B97-9B83-7484A9D00433"
		Friend Const IFileDialogCustomize As String = "E6FDD21A-163F-4975-9C8C-A69F1BA37034"

		Friend Const IShellItem As String = "43826D1E-E718-42EE-BC55-A1E261C37BFE"
		Friend Const IShellItem2 As String = "7E9FB0D3-919F-4307-AB2E-9B1860310C93"
		Friend Const IShellItemArray As String = "B63EA76D-1F85-456F-A19C-48159EFA858B"
		Friend Const IShellLibrary As String = "11A66EFA-382E-451A-9234-1E0E12EF3085"
		Friend Const IThumbnailCache As String = "F676C15D-596A-4ce2-8234-33996F445DB1"
		Friend Const ISharedBitmap As String = "091162a4-bc96-411f-aae8-c5122cd03363"
		Friend Const IShellFolder As String = "000214E6-0000-0000-C000-000000000046"
		Friend Const IShellFolder2 As String = "93F2F68C-1D1B-11D3-A30E-00C04F79ABD1"
		Friend Const IEnumIDList As String = "000214F2-0000-0000-C000-000000000046"
		Friend Const IShellLinkW As String = "000214F9-0000-0000-C000-000000000046"
		Friend Const CShellLink As String = "00021401-0000-0000-C000-000000000046"

		Friend Const IPropertyStore As String = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"
		Friend Const IPropertyStoreCache As String = "3017056d-9a91-4e90-937d-746c72abbf4f"
		Friend Const IPropertyDescription As String = "6F79D558-3E96-4549-A1D1-7D75D2288814"
		Friend Const IPropertyDescription2 As String = "57D2EDED-5062-400E-B107-5DAE79FE57A6"
		Friend Const IPropertyDescriptionList As String = "1F9FC1D0-C39B-4B26-817F-011967D3440E"
		Friend Const IPropertyEnumType As String = "11E1FBF9-2D56-4A6B-8DB3-7CD193A471F2"
		Friend Const IPropertyEnumType2 As String = "9B6E051C-5DDD-4321-9070-FE2ACB55E794"
		Friend Const IPropertyEnumTypeList As String = "A99400F4-3D84-4557-94BA-1242FB2CC9A6"
		Friend Const IPropertyStoreCapabilities As String = "c8e2d566-186e-4d49-bf41-6909ead56acc"

		Friend Const ICondition As String = "0FC988D4-C935-4b97-A973-46282EA175C8"
		Friend Const ISearchFolderItemFactory As String = "a0ffbc28-5482-4366-be27-3e81e78e06c2"
		Friend Const IConditionFactory As String = "A5EFE073-B16F-474f-9F3E-9F8B497A3E08"
		Friend Const IRichChunk As String = "4FDEF69C-DBC9-454e-9910-B34F3C64B510"
		Friend Const IPersistStream As String = "00000109-0000-0000-C000-000000000046"
		Friend Const IPersist As String = "0000010c-0000-0000-C000-000000000046"
		Friend Const IEnumUnknown As String = "00000100-0000-0000-C000-000000000046"
		Friend Const IQuerySolution As String = "D6EBC66B-8921-4193-AFDD-A1789FB7FF57"
		Friend Const IQueryParser As String = "2EBDEE67-3505-43f8-9946-EA44ABC8E5B0"
		Friend Const IQueryParserManager As String = "A879E3C4-AF77-44fb-8F37-EBD1487CF920"
	End Class

	Friend NotInheritable Class ShellCLSIDGuid
		Private Sub New()
		End Sub

		' CLSID GUID strings for relevant coclasses.
		Friend Const FileOpenDialog As String = "DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7"
		Friend Const FileSaveDialog As String = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B"
		Friend Const KnownFolderManager As String = "4DF0C730-DF9D-4AE3-9153-AA6B82E9795A"
		Friend Const ShellLibrary As String = "D9B3211D-E57F-4426-AAEF-30A806ADD397"
		Friend Const SearchFolderItemFactory As String = "14010e02-bbbd-41f0-88e3-eda371216584"
		Friend Const ConditionFactory As String = "E03E85B0-7BE3-4000-BA98-6C13DE9FA486"
		Friend Const QueryParserManager As String = "5088B39A-29B4-4d9d-8245-4EE289222F66"
	End Class

	Friend NotInheritable Class ShellKFIDGuid
		Private Sub New()
		End Sub

		Friend Const ComputerFolder As String = "0AC0837C-BBF8-452A-850D-79D08E667CA7"
		Friend Const Favorites As String = "1777F761-68AD-4D8A-87BD-30B759FA33DD"
		Friend Const Documents As String = "FDD39AD0-238F-46AF-ADB4-6C85480369C7"
		Friend Const Profile As String = "5E6C858F-0E22-4760-9AFE-EA3317B67173"

		Friend Const GenericLibrary As String = "5c4f28b5-f869-4e84-8e60-f11db97c5cc7"
		Friend Const DocumentsLibrary As String = "7d49d726-3c21-4f05-99aa-fdc2c9474656"
		Friend Const MusicLibrary As String = "94d6ddcc-4a68-4175-a374-bd584a510b78"
		Friend Const PicturesLibrary As String = "b3690e58-e961-423b-b687-386ebfd83239"
		Friend Const VideosLibrary As String = "5fa96407-7e77-483c-ac93-691d05850de8"

		Friend Const Libraries As String = "1B3EA5DC-B587-4786-B4EF-BD1DC332AEAE"
	End Class

	Friend NotInheritable Class ShellBHIDGuid
		Private Sub New()
		End Sub
		Friend Const ShellFolderObject As String = "3981e224-f559-11d3-8e3a-00c04f6837d5"
	End Class
End Namespace
