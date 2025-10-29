namespace AdService.Domain.Ads.Enums;

public enum AdStatus
{
    /// <summary>
    /// Черновик
    /// </summary>
    DRAFT,

    /// <summary>
    /// Ожидает модерации
    /// </summary>
    PENDING,

    /// <summary>
    /// Опубликовано
    /// </summary>
    PUBLISHED,

    /// <summary>
    /// Отказано в публикации
    /// </summary>
    DENIED,

    /// <summary>
    /// Удалено администратором или модератором
    /// </summary>
    REMOVED,

    /// <summary>
    /// Заархивировано(продано или убрано)
    /// </summary>
    ARCHIVED,

    /// <summary>
    /// Удалено пользователем
    /// </summary>
    DELETED,

    /// <summary>
    /// Временно приостановлено пользователем или админом
    /// </summary>
    PAUSED,
}