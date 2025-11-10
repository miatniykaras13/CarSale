namespace AdService.Domain.Ads.Enums;

public enum InactiveReason
{
    /// <summary>
    /// Удалено пользователем
    /// </summary>
    REMOVED_BY_USER,

    /// <summary>
    /// Заархивировано пользователем
    /// </summary>
    ARCHIVED_BY_USER,

    /// <summary>
    /// Продано
    /// </summary>
    SOLD,

    /// <summary>
    /// Удалено модератором
    /// </summary>
    REMOVED_BY_MODERATOR,

    /// <summary>
    /// Отказано в публицкации
    /// </summary>
    DENIED,

    /// <summary>
    /// Просрочено
    /// </summary>
    EXPIRED,

    /// <summary>
    /// Приостановлено
    /// </summary>
    PAUSED,
}