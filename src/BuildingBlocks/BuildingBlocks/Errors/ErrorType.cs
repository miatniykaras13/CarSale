namespace BuildingBlocks.Errors;

public enum ErrorType
{
    /// <summary>
    /// Неизвестная ошибка
    /// </summary>
    UNKNOWN,

    /// <summary>
    /// Не найдено
    /// </summary>
    NOT_FOUND,

    /// <summary>
    /// Серверная ошибка
    /// </summary>
    INTERNAL,

    /// <summary>
    /// Конфликт
    /// </summary>
    CONFLICT,

    /// <summary>
    /// Ошибка валидации
    /// </summary>
    VALIDATION,
}