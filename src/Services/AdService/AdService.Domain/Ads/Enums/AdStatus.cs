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
    /// Объявление неактивно
    /// </summary>
    INACTIVE,
}