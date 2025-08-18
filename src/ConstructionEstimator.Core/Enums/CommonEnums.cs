namespace ConstructionEstimator.Core.Enums;

public enum ProjectStatus
{
    Draft = 0,
    InProgress = 1,
    Review = 2,
    Approved = 3,
    Completed = 4,
    Cancelled = 5
}

public enum EstimateItemType
{
    Material = 0,
    Labor = 1,
    Equipment = 2,
    Overhead = 3,
    Profit = 4,
    Contingency = 5
}

public enum UnitOfMeasure
{
    Piece = 0,         // cái, chiếc
    Meter = 1,         // mét
    SquareMeter = 2,   // mét vuông  
    CubicMeter = 3,    // mét khối
    Kilogram = 4,      // kilogram
    Ton = 5,           // tấn
    LiterMl = 6,       // lít
    Hour = 7,          // giờ
    Day = 8,           // ngày
    Month = 9,         // tháng
    Package = 10,      // gói
    Set = 11,          // bộ
    Box = 12,          // thùng
    Bag = 13           // bao
}

public enum MaterialCategory
{
    Concrete = 0,      // Bê tông
    Steel = 1,         // Thép
    Brick = 2,         // Gạch
    Wood = 3,          // Gỗ
    Paint = 4,         // Sơn
    Tile = 5,          // Gạch ốp lát
    Electrical = 6,    // Điện
    Plumbing = 7,      // Nước
    Hardware = 8,      // Kim khí
    Other = 9          // Khác
}

public enum LaborCategory
{
    General = 0,       // Công nhân phổ thông
    Skilled = 1,       // Công nhân có kỹ năng
    Technical = 2,     // Kỹ thuật viên
    Supervisor = 3,    // Giám sát
    Specialist = 4,    // Chuyên gia
    Equipment = 5      // Vận hành máy
}

public enum PriceListType
{
    Material = 0,
    Labor = 1,
    Equipment = 2,
    Standard = 3
}