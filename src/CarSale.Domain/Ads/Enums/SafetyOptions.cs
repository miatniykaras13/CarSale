namespace CarSale.Domain.Ads.Enums;

[Flags]
public enum SafetyOptions
{
    None = 0,
    ABS = 1 << 0,
    ESP = 1 << 1,
    LaneAssist = 1 << 2,
    AutoBraking = 1 << 3,
    BlindSpotMonitor = 1 << 4,
    Immobilizer = 1 << 5,
    Isofix = 1 << 6,
    EmergencyStop = 1 << 7,
}