namespace CarSale.Domain.Ads.Enums;

public enum ComfortOptions
{
    None = 0,
    RemoteStart = 1 << 0, // Автозапуск двигателя
    CruiseControl = 1 << 1, // Круиз-контроль
    AdaptiveCruiseControl = 1 << 2, // Круиз-контроль адаптивный
    StartStopSystem = 1 << 3, // Старт-стоп
    SteeringWheelMediaControl = 1 << 4, // Управление мультимедиа с руля
    SteeringWheelAdjustment = 1 << 5, // Электрорегулировка руля
    PowerSeats = 1 << 6, // Электрорегулировка сидений
    SeatMassage = 1 << 7, // Массаж сидений
    SeatMemory = 1 << 8, // Память положения сидений
    FrontPowerWindows = 1 << 9, // Передние электро-стеклоподъёмники
    RearPowerWindows = 1 << 10, // Задние электро-стеклоподъёмники
    PowerMirrorFolding = 1 << 11, // Электроскладывание зеркал
    PowerTailgate = 1 << 12, // Электропривод двери багажника
    HandsFreeTailgate = 1 << 13, // Открытие багажника без рук
    WirelessCharging = 1 << 14, // Беспроводная зарядка
    AirSuspension = 1 << 15, // Пневмоподвеска
    KeylessEntry = 1 << 16, // Бесключевой доступ
    Socket12V = 1 << 17, // Розетка 12V
    Socket220V = 1 << 18, // Розетка 220V
    Socket110V = 1 << 19, // Розетка 110V
}