' Copyright (c) Microsoft Corporation.  All rights reserved.


Imports Microsoft.Windows.Shell.PropertySystem

Namespace Sensors
    ''' <summary>
    ''' Sensor Property Key identifiers. This class will be removed once wrappers are developed.
    ''' </summary>
    Public NotInheritable Class SensorPropertyKeys
        Private Sub New()
        End Sub
        ''' <summary>
        ''' The common Guid used by the property keys.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyCommonGuid As New Guid(&H7F8383EC, &HD3EC, &H495C, &HA8, &HCF, &HB8,
            &HBB, &HE8, &H5C, &H29, &H20)

        ''' <summary>
        ''' The sensor type property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyType As New PropertyKey(SensorPropertyCommonGuid, 2)

        ''' <summary>
        ''' The sensor state property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyState As New PropertyKey(SensorPropertyCommonGuid, 3)

        ''' <summary>
        ''' The sensor sampling rate property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertySamplingRate As New PropertyKey(SensorPropertyCommonGuid, 4)

        ''' <summary>
        ''' The sensor persistent unique id property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyPersistentUniqueId As New PropertyKey(SensorPropertyCommonGuid, 5)

        ''' <summary>
        ''' The sensor manufacturer property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyManufacturer As New PropertyKey(SensorPropertyCommonGuid, 6)

        ''' <summary>
        ''' The sensor model property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyModel As New PropertyKey(SensorPropertyCommonGuid, 7)

        ''' <summary>
        ''' The sensor serial number property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertySerialNumber As New PropertyKey(SensorPropertyCommonGuid, 8)

        ''' <summary>
        ''' The sensor friendly name property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyFriendlyName As New PropertyKey(SensorPropertyCommonGuid, 9)

        ''' <summary>
        ''' The sensor description property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyDescription As New PropertyKey(SensorPropertyCommonGuid, 10)

        ''' <summary>
        ''' The sensor connection type property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyConnectionType As New PropertyKey(SensorPropertyCommonGuid, 11)

        ''' <summary>
        ''' The sensor min report interval property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyMinReportInterval As New PropertyKey(SensorPropertyCommonGuid, 12)

        ''' <summary>
        ''' The sensor current report interval property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyCurrentReportInterval As New PropertyKey(SensorPropertyCommonGuid, 13)

        ''' <summary>
        ''' The sensor change sensitivity property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyChangeSensitivity As New PropertyKey(SensorPropertyCommonGuid, 14)

        ''' <summary>
        ''' The sensor device id property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyDeviceId As New PropertyKey(SensorPropertyCommonGuid, 15)

        ''' <summary>
        ''' The sensor accuracy property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyAccuracy As New PropertyKey(SensorPropertyCommonGuid, 16)

        ''' <summary>
        ''' The sensor resolution property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyResolution As New PropertyKey(SensorPropertyCommonGuid, 17)

        ''' <summary>
        ''' The sensor light response curve property key.
        ''' </summary>
        Public Shared ReadOnly SensorPropertyLightResponseCurve As New PropertyKey(SensorPropertyCommonGuid, 18)

        ''' <summary>
        ''' The sensor date time property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeTimestamp As New PropertyKey(New Guid(&HDB5E0CF2UI, &HCF1F, &H4C18, &HB4, &H6C, &HD8,
            &H60, &H11, &HD6, &H21, &H50), 2)

        ''' <summary>
        ''' The sensor latitude in degrees property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeLatitudeDegrees As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 2)

        ''' <summary>
        ''' The sensor longitude in degrees property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeLongitudeDegrees As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 3)

        ''' <summary>
        ''' The sensor altitude from sea level in meters property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAltitudeSeaLevelMeters As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 4)

        ''' <summary>
        ''' The sensor altitude in meters property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAltitudeEllipsoidMeters As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 5)

        ''' <summary>
        ''' The sensor speed in knots property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSpeedKnots As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 6)

        ''' <summary>
        ''' The sensor true heading in degrees property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeTrueHeadingDegrees As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 7)

        ''' <summary>
        ''' The sensor magnetic heading in degrees property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeMagneticHeadingDegrees As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 8)

        ''' <summary>
        ''' The sensor magnetic variation property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeMagneticVariation As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 9)

        ''' <summary>
        ''' The sensor data fix quality property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeFixQuality As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 10)

        ''' <summary>
        ''' The sensor data fix type property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeFixType As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 11)

        ''' <summary>
        ''' The sensor position dilution of precision property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypePositionDilutionOfPrecision As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 12)

        ''' <summary>
        ''' The sensor horizontal dilution of precision property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeHorizontalDilutionOfPrecision As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 13)

        ''' <summary>
        ''' The sensor vertical dilution of precision property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeVerticalDilutionOfPrecision As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 14)

        ''' <summary>
        ''' The sensor number of satelites used property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesUsedCount As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 15)

        ''' <summary>
        ''' The sensor number of satelites used property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesUsedPrns As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 16)

        ''' <summary>
        ''' The sensor view property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesInView As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 17)

        ''' <summary>
        ''' The sensor view property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesInViewPrns As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 18)

        ''' <summary>
        ''' The sensor elevation property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesInViewElevation As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 19)

        ''' <summary>
        ''' The sensor azimuth value for satelites in view property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesInViewAzimuth As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 20)

        ''' <summary>
        ''' The sensor signal to noise ratio for satelites in view property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeSatellitesInViewStnRatio As New PropertyKey(New Guid(&H55C74D8, &HCA6F, &H47D6, &H95, &HC6, &H1E,
            &HD3, &H63, &H7A, &HF, &HF4), 21)

        ''' <summary>
        ''' The sensor temperature in celsius property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeTemperatureCelsius As New PropertyKey(New Guid(&H8B0AA2F1UI, &H2D57, &H42EE, &H8C, &HC0, &H4D,
            &H27, &H62, &H2B, &H46, &HC4), 2)

        ''' <summary>
        ''' The sensor gravitational acceleration (X-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAccelerationXG As New PropertyKey(New Guid(&H3F8A69A2, &H7C5, &H4E48, &HA9, &H65, &HCD,
            &H79, &H7A, &HAB, &H56, &HD5), 2)

        ''' <summary>
        ''' The sensor gravitational acceleration (Y-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAccelerationYG As New PropertyKey(New Guid(&H3F8A69A2, &H7C5, &H4E48, &HA9, &H65, &HCD,
            &H79, &H7A, &HAB, &H56, &HD5), 3)

        ''' <summary>
        ''' The sensor gravitational acceleration (Z-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAccelerationZG As New PropertyKey(New Guid(&H3F8A69A2, &H7C5, &H4E48, &HA9, &H65, &HCD,
            &H79, &H7A, &HAB, &H56, &HD5), 4)

        ''' <summary>
        ''' The sensor angular acceleration per second (X-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAngularAccelerationXDegreesPerSecond As New PropertyKey(New Guid(&H3F8A69A2, &H7C5, &H4E48, &HA9, &H65, &HCD,
            &H79, &H7A, &HAB, &H56, &HD5), 5)

        ''' <summary>
        ''' The sensor angular acceleration per second (Y-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAngularAccelerationYDegreesPerSecond As New PropertyKey(New Guid(&H3F8A69A2, &H7C5, &H4E48, &HA9, &H65, &HCD,
            &H79, &H7A, &HAB, &H56, &HD5), 6)

        ''' <summary>
        ''' The sensor angular acceleration per second (Z-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAngularAccelerationZDegreesPerSecond As New PropertyKey(New Guid(&H3F8A69A2, &H7C5, &H4E48, &HA9, &H65, &HCD,
            &H79, &H7A, &HAB, &H56, &HD5), 7)

        ''' <summary>
        ''' The sensor angle in degrees (X-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAngleXDegrees As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 2)

        ''' <summary>
        ''' The sensor angle in degrees (Y-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAngleYDegrees As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 3)

        ''' <summary>
        ''' The sensor angle in degrees (Z-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeAngleZDegrees As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 4)

        ''' <summary>
        ''' The sensor magnetic heading (X-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeMagneticHeadingXDegrees As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 5)

        ''' <summary>
        ''' The sensor magnetic heading (Y-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeMagneticHeadingYDegrees As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 6)

        ''' <summary>
        ''' The sensor magnetic heading (Z-axis) property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeMagneticHeadingZDegrees As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 7)

        ''' <summary>
        ''' The sensor distance (X-axis) data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeDistanceXMeters As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 8)

        ''' <summary>
        ''' The sensor distance (Y-axis) data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeDistanceYMeters As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 9)

        ''' <summary>
        ''' The sensor distance (Z-axis) data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeDistanceZMeters As New PropertyKey(New Guid(&HC2FB0F5FUI, &HE2D2, &H4C78, &HBC, &HD0, &H35,
            &H2A, &H95, &H82, &H81, &H9D), 10)

        ''' <summary>
        ''' The sensor boolean switch data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeBooleanSwitchState As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 2)

        ''' <summary>
        ''' The sensor multi-value switch data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeMultivalueSwitchState As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 3)

        ''' <summary>
        ''' The sensor boolean switch array state data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeBooleanSwitchArrayState As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 10)

        ''' <summary>
        ''' The sensor force (in newtons) data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeForceNewtons As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 4)

        ''' <summary>
        ''' The sensor weight (in kilograms) data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeWeightKilograms As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 5)

        ''' <summary>
        ''' The sensor pressure data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypePressurePascal As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 6)

        ''' <summary>
        ''' The sensor strain data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeStrain As New PropertyKey(New Guid(&H38564A7C, &HF2F2, &H49BB, &H9B, &H2B, &HBA,
            &H60, &HF6, &H6A, &H58, &HDF), 7)

        ''' <summary>
        ''' The sensor human presence data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeHumanPresence As New PropertyKey(New Guid(&H2299288A, &H6D9E, &H4B0B, &HB7, &HEC, &H35,
            &H28, &HF8, &H9E, &H40, &HAF), 2)

        ''' <summary>
        ''' The sensor human proximity data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeHumanProximity As New PropertyKey(New Guid(&H2299288A, &H6D9E, &H4B0B, &HB7, &HEC, &H35,
            &H28, &HF8, &H9E, &H40, &HAF), 3)

        ''' <summary>
        ''' The sensor light data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeLightLux As New PropertyKey(New Guid(&HE4C77CE2UI, &HDCB7, &H46E9, &H84, &H39, &H4F,
            &HEC, &H54, &H88, &H33, &HA6), 2)

        ''' <summary>
        ''' The sensor 40-bit RFID tag data property key.
        ''' </summary>
        Public Shared ReadOnly SensorDataTypeRfidTag40Bit As New PropertyKey(New Guid(&HD7A59A3CUI, &H3421, &H44AB, &H8D, &H3A, &H9D,
            &HE8, &HAB, &H6C, &H4C, &HAE), 2)
    End Class
End Namespace
