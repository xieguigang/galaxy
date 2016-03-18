' This file is used by Code Analysis to maintain SuppressMessage 
' attributes that are applied to this project.
' Project-level suppressions either have no target or are given 
' a specific target and scoped to a namespace, type, member, etc.
'
' To add a suppression to this file, right-click the message in the 
' Error List, point to "Suppress Message(s)", and click 
' "In Project Suppression File".
' You do not need to add suppressions to this file manually.


<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope := "member", Target := "ShellExtensions.Interop.NativeColorRef.#set_Dword(System.UInt32)")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope := "member", Target := "ShellExtensions.PreviewHandler.#ShellExtensions.Interop.IPreviewHandler.DoPreview()")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ShellExtensions.WpfPreviewHandler.#Initialize()")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Scope := "member", Target := "ShellExtensions.ThumbnailProvider.#.ctor()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Scope := "member", Target := "ShellExtensions.WpfPreviewHandler.#.ctor()")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId := "API")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope := "namespace", Target := "ShellExtensions.Interop", MessageId := "API")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope := "namespace", Target := "ShellExtensions", MessageId := "API")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "ShellExtensions.PreviewHandler.#SetBackground(System.Int32)", MessageId := "argb")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "ShellExtensions.PreviewHandler.#SetForeground(System.Int32)", MessageId := "argb")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope := "type", Target := "ShellExtensions.IThumbnailFromStream")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope := "type", Target := "ShellExtensions.IPreviewFromStream")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope := "member", Target := "ShellExtensions.ThumbnailProvider.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope := "member", Target := "ShellExtensions.PreviewHandler.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope := "member", Target := "ShellExtensions.ThumbnailProvider.#GetThumbnailAlphaType()")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId := "API")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId := "API", Scope := "namespace", Target := "ShellExtensions")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId := "API", Scope := "namespace", Target := "ShellExtensions.Interop")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope := "type", Target := "ShellExtensions.IPreviewFromStream")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope := "type", Target := "ShellExtensions.IThumbnailFromStream")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope := "member", Target := "ShellExtensions.PreviewHandlers.PreviewHandler.#ShellExtensions.Interop.IPreviewHandler.DoPreview()", Justification := "Exception is handled later")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope := "member", Target := "ShellExtensions.PreviewHandler.#ShellExtensions.Interop.IPreviewHandler.DoPreview()", Justification := "Exception is handled later")>

' All link demand related warnings suppressed.
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ShellExtensions.ThumbnailProvider.#ShellExtensions.Interop.IThumbnailProvider.GetThumbnail(System.UInt32,System.IntPtr&,System.UInt32&)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ShellExtensions.WpfPreviewHandler.#Initialize()")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope := "member", Justification := "Interfaces are implemented explicitly because they are native COM interfaces, we do not want child types to call them.", Target := "ShellExtensions.ThumbnailProvider.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope := "member", Justification := "Interfaces are implemented explicitly because they are native COM interfaces, we do not want child types to call them.", Target := "ShellExtensions.PreviewHandler.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification := "There are places where unsigned values are used, which is considered not Cls compliant.")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope := "member", Justification := "Populated via marshaling.", Target := "ShellExtensions.Interop.NativeColorRef.#Dword")>
