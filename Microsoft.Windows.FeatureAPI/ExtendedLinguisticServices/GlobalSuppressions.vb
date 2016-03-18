' This file is used by Code Analysis to maintain SuppressMessage 
' attributes that are applied to this project. 
' Project-level suppressions either have no target or are given 
' a specific target and scoped to a namespace, type, member, etc. 
'
' To add a suppression to this file, right-click the message in the 
' Error List, point to "Suppress Message(s)", and click 
' "In Project Suppression File". 
' You do not need to add suppressions to this file manually. 


<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId := "API")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope := "namespace", Target := "ExtendedLinguisticServices", MessageId := "API")>


<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "ExtendedLinguisticServices.MappingAvailableServices.#TransliterationDevanagariToLatin", MessageId := "Devanagari")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "ExtendedLinguisticServices.MappingAvailableServices.#TransliterationHantToHans", MessageId := "Hant")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "ExtendedLinguisticServices.MappingAvailableServices.#TransliterationHansToHant", MessageId := "Hant")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification := "There are places where unsigned values are used, which is considered not Cls compliant.")>

#Region "LinkDemand related"
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.InteropTools.#Free`1(System.IntPtr&)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.InteropTools.#Pack`1(!!0&)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.InteropTools.#Unpack`1(System.IntPtr)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.InteropTools.#UnpackStringArray(System.IntPtr,System.UInt32)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.LinguisticException.#.ctor()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.LinguisticException.#.ctor(System.String)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.LinguisticException.#.ctor(System.String,System.Exception)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.LinguisticException.#.ctor(System.UInt32)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.MappingDataRange.#GetData()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.MappingPropertyBag.#Dispose(System.Boolean)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.MappingPropertyBag.#.ctor(ExtendedLinguisticServices.MappingOptions,System.String)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.MappingService.#GetServices(ExtendedLinguisticServices.MappingEnumOptions)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.MappingService.#.ctor(System.Guid)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.MappingService.#RecognizeText(System.String,System.Int32,System.Int32,ExtendedLinguisticServices.MappingOptions)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.ServiceCache.#RollBack(System.IntPtr,System.Int32)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "ExtendedLinguisticServices.ServiceCache.#TryRegisterServices(System.IntPtr,System.IntPtr[],System.Boolean&)")>

#End Region

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope := "type", Target := "ExtendedLinguisticServices.ServiceCache")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope := "member", Target := "ExtendedLinguisticServices.MappingService.#EndDoAction(ExtendedLinguisticServices.MappingActionAsyncResult)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope := "member", Target := "ExtendedLinguisticServices.MappingService.#EndRecognizeText(ExtendedLinguisticServices.MappingRecognizeAsyncResult)")>
