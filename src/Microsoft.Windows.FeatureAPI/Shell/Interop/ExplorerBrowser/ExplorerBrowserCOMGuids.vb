'Copyright (c) Microsoft Corporation.  All rights reserved.

Namespace Controls
	Friend NotInheritable Class ExplorerBrowserIIDGuid
		Private Sub New()
		End Sub
		' IID GUID strings for relevant Shell COM interfaces.
		Friend Const IExplorerBrowser As String = "DFD3B6B5-C10C-4BE9-85F6-A66969F402F6"
		Friend Const IKnownFolderManager As String = "8BE2D872-86AA-4d47-B776-32CCA40C7018"
		Friend Const IFolderView As String = "cde725b0-ccc9-4519-917e-325d72fab4ce"
		Friend Const IFolderView2 As String = "1af3a467-214f-4298-908e-06b03e0b39f9"
		Friend Const IServiceProvider As String = "6d5140c1-7436-11ce-8034-00aa006009fa"
		Friend Const IExplorerPaneVisibility As String = "e07010ec-bc17-44c0-97b0-46c7c95b9edc"
		Friend Const IExplorerBrowserEvents As String = "361bbdc7-e6ee-4e13-be58-58e2240c810f"
		Friend Const IInputObject As String = "68284fAA-6A48-11D0-8c78-00C04fd918b4"
		Friend Const IShellView As String = "000214E3-0000-0000-C000-000000000046"
		Friend Const IDispatch As String = "00020400-0000-0000-C000-000000000046"
		Friend Const DShellFolderViewEvents As String = "62112AA2-EBE4-11cf-A5FB-0020AFE7292D"

		Friend Const ICommDlgBrowser As String = "000214F1-0000-0000-C000-000000000046"
		Friend Const ICommDlgBrowser2 As String = "10339516-2894-11d2-9039-00C04F8EEB3E"
		Friend Const ICommDlgBrowser3 As String = "c8ad25a1-3294-41ee-8165-71174bd01c57"

	End Class

	Friend NotInheritable Class ExplorerBrowserViewPanes
		Private Sub New()
		End Sub
		Friend Const Navigation As String = "cb316b22-25f7-42b8-8a09-540d23a43c2f"
		Friend Const Commands As String = "d9745868-ca5f-4a76-91cd-f5a129fbb076"
		Friend Const CommandsOrganize As String = "72e81700-e3ec-4660-bf24-3c3b7b648806"
		Friend Const CommandsView As String = "21f7c32d-eeaa-439b-bb51-37b96fd6a943"
		Friend Const Details As String = "43abf98b-89b8-472d-b9ce-e69b8229f019"
		Friend Const Preview As String = "893c63d1-45c8-4d17-be19-223be71be365"
		Friend Const Query As String = "65bcde4f-4f07-4f27-83a7-1afca4df7ddd"
		Friend Const AdvancedQuery As String = "b4e9db8b-34ba-4c39-b5cc-16a1bd2c411c"
	End Class

	Friend NotInheritable Class ExplorerBrowserCLSIDGuid
		Private Sub New()
		End Sub
		' CLSID GUID strings for relevant coclasses.
		Friend Const ExplorerBrowser As String = "71F96385-DDD6-48D3-A0C1-AE06E8B055FB"
	End Class

	Friend NotInheritable Class ExplorerBrowserViewDispatchIds
		Private Sub New()
		End Sub
		Friend Const SelectionChanged As Integer = 200
		Friend Const ContentsChanged As Integer = 207
		Friend Const FileListEnumDone As Integer = 201
		Friend Const SelectedItemChanged As Integer = 220
	End Class
End Namespace
