namespace ConstructionEstimator.Shared.Constants;

public static class AppConstants
{
    public const string ApplicationName = "Construction Estimator Vietnam";
    public const string ApplicationNameVietnamese = "Phần mềm Dự toán Xây dựng Việt Nam";
    public const string Version = "1.0.0";
    
    public static class DatabaseConstants
    {
        public const string DefaultConnectionString = "Data Source=construction_estimator.db";
        public const string LogTableName = "Logs";
    }
    
    public static class VietnameseUnits
    {
        public const string SquareMeter = "m2";
        public const string CubicMeter = "m3";
        public const string Meter = "m";
        public const string Kilogram = "kg";
        public const string Ton = "tấn";
        public const string Piece = "cái";
        public const string Set = "bộ";
        public const string Bag = "bao";
        public const string Bucket = "thùng";
        public const string Sheet = "tấm";
        public const string Hour = "giờ";
        public const string Day = "ngày";
    }
    
    public static class MaterialCategories
    {
        public const string Cement = "Xi măng";
        public const string Sand = "Cát";
        public const string Stone = "Đá";
        public const string Steel = "Thép";
        public const string Brick = "Gạch";
        public const string Wood = "Gỗ";
        public const string Tile = "Gạch ốp lát";
        public const string Paint = "Sơn";
        public const string Pipe = "Ống nước";
        public const string Electrical = "Điện";
        public const string Door = "Cửa";
        public const string Glass = "Kính";
        public const string Waterproofing = "Chống thấm";
        public const string SteelStructure = "Thép hình";
        public const string Insulation = "Cách nhiệt";
        public const string Finishing = "Hoàn thiện";
        public const string Sanitary = "Thiết bị vệ sinh";
    }
    
    public static class LaborCategories
    {
        public const string Construction = "Xây dựng";
        public const string Steel = "Cốt thép";
        public const string Welding = "Hàn";
        public const string Carpentry = "Mộc";
        public const string Painting = "Sơn";
        public const string Electrical = "Điện";
        public const string Plumbing = "Nước";
        public const string Tiling = "Ốp lát";
        public const string AluminumGlass = "Nhôm kính";
        public const string General = "Phổ thông";
        public const string Management = "Quản lý";
        public const string Equipment = "Máy móc";
        public const string Waterproofing = "Chống thấm";
        public const string Insulation = "Cách nhiệt";
        public const string Finishing = "Hoàn thiện";
    }
}