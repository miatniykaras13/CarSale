namespace AdService.Domain.Ads.Enums;

[Flags]
public enum ComfortOptions
{
    /// <summary>
    /// Ничего
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Автозапуск двигателя
    /// </summary>
    REMOTE_START = 1 << 0,

    /// <summary>
    /// Круиз-контроль
    /// </summary>
    CRUISE_CONTROL = 1 << 1,

    /// <summary>
    /// Адаптивный круиз-контроль
    /// </summary>
    ADAPTIVE_CRUISE_CONTROL = 1 << 2,

    /// <summary>
    /// Старт-стоп система
    /// </summary>
    START_STOP_SYSTEM = 1 << 3,

    /// <summary>
    /// Управление мультимедиа с руля
    /// </summary>
    STEERING_WHEEL_MEDIA_CONTROL = 1 << 4,

    /// <summary>
    /// Электрорегулировка руля
    /// </summary>
    STEERING_WHEEL_ADJUSTMENT = 1 << 5,

    /// <summary>
    /// Электрорегулировка сидений
    /// </summary>
    POWER_SEATS = 1 << 6,

    /// <summary>
    /// Массаж сидений
    /// </summary>
    SEAT_MASSAGE = 1 << 7,

    /// <summary>
    /// Память положения сидений
    /// </summary>
    SEAT_MEMORY = 1 << 8,

    /// <summary>
    /// Передние электро-стеклоподъёмники
    /// </summary>
    FRONT_POWER_WINDOWS = 1 << 9,

    /// <summary>
    /// Задние электро-стеклоподъёмники
    /// </summary>
    REAR_POWER_WINDOWS = 1 << 10,

    /// <summary>
    /// Электроскладывание зеркал
    /// </summary>
    POWER_MIRROR_FOLDING = 1 << 11,

    /// <summary>
    /// Электропривод двери багажника
    /// </summary>
    POWER_TAILGATE = 1 << 12,

    /// <summary>
    /// Открытие багажника без рук
    /// </summary>
    HANDS_FREE_TAILGATE = 1 << 13,

    /// <summary>
    /// Беспроводная зарядка
    /// </summary>
    WIRELESS_CHARGING = 1 << 14,

    /// <summary>
    /// Пневмоподвеска
    /// </summary>
    AIR_SUSPENSION = 1 << 15,

    /// <summary>
    /// Бесключевой доступ
    /// </summary>
    KEYLESS_ENTRY = 1 << 16,

    /// <summary>
    /// Розетка 12V
    /// </summary>
    SOCKET_12V = 1 << 17,

    /// <summary>
    /// Розетка 220V
    /// </summary>
    SOCKET_220V = 1 << 18,

    /// <summary>
    /// Розетка 110V
    /// </summary>
    SOCKET_110V = 1 << 19,
}