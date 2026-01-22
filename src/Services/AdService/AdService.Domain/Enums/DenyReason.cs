using System.Text.Json.Serialization;

namespace AdService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DenyReason
{
    /// <summary>
    /// Содержит запрещённый или оскорбительный контент
    /// </summary>
    INAPPROPRIATE_CONTENT,

    /// <summary>
    /// Нарушает правила платформы (спам, реклама, дубли)
    /// </summary>
    POLICY_VIOLATION,

    /// <summary>
    /// Недостаточно информации о товаре или услуге
    /// </summary>
    INSUFFICIENT_INFORMATION,

    /// <summary>
    /// Ложные или вводящие в заблуждение данные
    /// </summary>
    MISLEADING_INFORMATION,

    /// <summary>
    /// Нарушение авторских прав или прав собственности
    /// </summary>
    COPYRIGHT_VIOLATION,

    /// <summary>
    /// Запрещённый товар или услуга
    /// </summary>
    PROHIBITED_ITEM,

    /// <summary>
    /// Подозрение на мошенничество или небезопасное предложение
    /// </summary>
    FRAUD_SUSPICION,

    /// <summary>
    /// Нарушение правил модерации изображений (некачественные, чужие, запрещённые фото)
    /// </summary>
    INVALID_IMAGES,

    /// <summary>
    /// Нарушение возрастных или юридических ограничений
    /// </summary>
    LEGAL_RESTRICTION,
}