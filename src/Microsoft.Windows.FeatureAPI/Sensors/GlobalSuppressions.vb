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
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope := "namespace", Target := "Sensors", MessageId := "API")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorTypes.#RfidScanner", MessageId := "Rfid", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorTypes.#MultivalueSwitch", MessageId := "Multivalue", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorTypes.#Gyrometer3D", MessageId := "Gyrometer", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorTypes.#LocationGps", MessageId := "Gps", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorTypes.#Gyrometer2D", MessageId := "Gyrometer", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorTypes.#Gyrometer1D", MessageId := "Gyrometer", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeSatellitesInViewPrns", MessageId := "Prns", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeLightLux", MessageId := "Lux", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeSatellitesUsedPrns", MessageId := "Prns", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeForceNewtons", MessageId := "Newtons", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeSatellitesInViewStnRatio", MessageId := "Stn", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeRfidTag40Bit", MessageId := "Rfid", Justification := "False positives")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope := "member", Target := "Sensors.SensorPropertyKeys.#SensorDataTypeMultivalueSwitchState", MessageId := "Multivalue", Justification := "False positives")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope := "member", Target := "Sensors.SensorManager.#GetSensorsByTypeId`1()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope := "member", Target := "Sensors.SensorManager.#GetSensorBySensorId`1(System.Guid)")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification := "There are places where unsigned values are used, which is considered not Cls compliant.")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope := "member", Justification := "Returns a new instance of an object.", Target := "Sensors.Sensor.#GetSupportedProperties()")>

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope := "type", Target := "Sensors.SensorList`1")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope := "type", Target := "Sensors.SensorData")>

#Region "LinkDemand related"
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#GetInterestingEvents()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#GetProperties(System.Int32[])")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#GetProperties(Shell.PropertySystem.PropertyKey[])")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#GetProperty(Shell.PropertySystem.PropertyKey)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#GetSupportedProperties()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#InternalUpdateData()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#SetEventInterest(System.Guid)")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#SetProperties(Sensors.DataFieldInfo[])")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.Sensor.#UpdateData()")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope := "member", Target := "Sensors.SensorData.#FromNativeReport(Sensors.ISensor,Sensors.ISensorDataReport)")>
#End Region
