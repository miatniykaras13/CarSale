namespace CarSale.Domain.Ads.Enums;

public enum AdStatus
{
    Unpublished, // не опубликовано
    Published, // опубликовано
    Deleted, // удалено
    Denied, // отклонено
    Pending, // на модерации
    Draft, // черновик(еще в процессе создания)
}