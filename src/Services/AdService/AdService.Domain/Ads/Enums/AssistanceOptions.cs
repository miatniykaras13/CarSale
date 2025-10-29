namespace AdService.Domain.Ads.Enums;

[Flags]
public enum AssistanceOptions
{
    /// <summary>
    /// Ничего
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Датчик дождя
    /// </summary>
    RAIN_SENSOR = 1 << 0,

    /// <summary>
    /// Камера заднего вида
    /// </summary>
    REAR_VIEW_CAMERA = 1 << 1,

    /// <summary>
    /// Парктроники
    /// </summary>
    PARKING_SENSORS = 1 << 2,

    /// <summary>
    /// Контроль мертвых зон
    /// </summary>
    BLIND_SPOT_MONITOR = 1 << 3,

    /// <summary>
    /// Камера переднего вида
    /// </summary>
    FRONT_VIEW_CAMERA = 1 << 4,

    /// <summary>
    /// Камера 360
    /// </summary>
    SURROUND_VIEW_CAMERA = 1 << 5,

    /// <summary>
    /// Автопарковка
    /// </summary>
    AUTO_PARKING = 1 << 6,

    /// <summary>
    /// Доводчики дверей
    /// </summary>
    DOOR_CLOSERS = 1 << 7,

    /// <summary>
    /// Удержание в полосе
    /// </summary>
    LANE_KEEPING_ASSIST = 1 << 8,

    /// <summary>
    /// Распознавание знаков
    /// </summary>
    TRAFFIC_SIGN_RECOGNITION = 1 << 9,

    /// <summary>
    /// Ночное видение
    /// </summary>
    NIGHT_VISION = 1 << 10,

    /// <summary>
    /// Помощь при подъеме
    /// </summary>
    HILL_ASSIST = 1 << 11,

    /// <summary>
    /// Помощь при спуске
    /// </summary>
    DESCENT_ASSIST = 1 << 12,

    /// <summary>
    /// Проекция на лобовое стекло
    /// </summary>
    HEAD_UP_DISPLAY = 1 << 13,
}