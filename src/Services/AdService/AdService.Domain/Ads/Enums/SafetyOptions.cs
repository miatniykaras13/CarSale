namespace AdService.Domain.Ads.Enums;

[Flags]
public enum SafetyOptions
{
    /// <summary>
    /// Ничего
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Антиблокировочная система (ABS)
    /// </summary>
    ABS = 1 << 0,

    /// <summary>
    /// Система стабилизации (ESP)
    /// </summary>
    ESP = 1 << 1,

    /// <summary>
    /// Ассистент удержания в полосе
    /// </summary>
    LANE_ASSIST = 1 << 2,

    /// <summary>
    /// Система автоматического торможения
    /// </summary>
    AUTO_BRAKING = 1 << 3,

    /// <summary>
    /// Контроль мёртвых зон
    /// </summary>
    BLIND_SPOT_MONITOR = 1 << 4,

    /// <summary>
    /// Иммобилайзер
    /// </summary>
    IMMOBILIZER = 1 << 5,

    /// <summary>
    /// Крепления для детских кресел ISOFIX
    /// </summary>
    ISOFIX = 1 << 6,

    /// <summary>
    /// Система экстренной остановки
    /// </summary>
    EMERGENCY_STOP = 1 << 7,
}