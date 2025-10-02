namespace CarSale.Domain.Ads.Enums;

[Flags]
public enum AssistanceOptions
{
    None = 0,

    RainSensor = 1 << 0, // Датчик дождя
    RearViewCamera = 1 << 1, // Камера заднего вида
    ParkingSensors = 1 << 2, // Парктроники
    BlindSpotMonitor = 1 << 3, // Контроль мертвых зон
    FrontViewCamera = 1 << 4, // Камера переднего вида
    SurroundViewCamera = 1 << 5, // Камера 360
    AutoParking = 1 << 6, // Автопарковка
    DoorClosers = 1 << 7, // Доводчики дверей
    LaneKeepingAssist = 1 << 8, // Удержание в полосе
    TrafficSignRecognition = 1 << 9, // Распознавание знаков
    NightVision = 1 << 10, // Ночное видение
    HillAssist = 1 << 11, // Помощь при подъеме
    DescentAssist = 1 << 12, // Помощь при спуске
    HeadUpDisplay = 1 << 13 // Проекция на лобовое стекло
}