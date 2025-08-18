using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Data.Configuration;

public static class VietnameseProvinceData
{
    public static List<Province> GetAllProvinces()
    {
        return new List<Province>
        {
            // Miền Bắc
            new() { Code = "01", Name = "Hà Nội", FullName = "Thành phố Hà Nội", Region = "Miền Bắc", IsActive = true },
            new() { Code = "02", Name = "Hà Giang", FullName = "Tỉnh Hà Giang", Region = "Miền Bắc", IsActive = true },
            new() { Code = "04", Name = "Cao Bằng", FullName = "Tỉnh Cao Bằng", Region = "Miền Bắc", IsActive = true },
            new() { Code = "06", Name = "Bắc Kạn", FullName = "Tỉnh Bắc Kạn", Region = "Miền Bắc", IsActive = true },
            new() { Code = "08", Name = "Tuyên Quang", FullName = "Tỉnh Tuyên Quang", Region = "Miền Bắc", IsActive = true },
            new() { Code = "10", Name = "Lào Cai", FullName = "Tỉnh Lào Cai", Region = "Miền Bắc", IsActive = true },
            new() { Code = "11", Name = "Điện Biên", FullName = "Tỉnh Điện Biên", Region = "Miền Bắc", IsActive = true },
            new() { Code = "12", Name = "Lai Châu", FullName = "Tỉnh Lai Châu", Region = "Miền Bắc", IsActive = true },
            new() { Code = "14", Name = "Sơn La", FullName = "Tỉnh Sơn La", Region = "Miền Bắc", IsActive = true },
            new() { Code = "15", Name = "Yên Bái", FullName = "Tỉnh Yên Bái", Region = "Miền Bắc", IsActive = true },
            new() { Code = "17", Name = "Hoà Bình", FullName = "Tỉnh Hoà Bình", Region = "Miền Bắc", IsActive = true },
            new() { Code = "19", Name = "Thái Nguyên", FullName = "Tỉnh Thái Nguyên", Region = "Miền Bắc", IsActive = true },
            new() { Code = "20", Name = "Lạng Sơn", FullName = "Tỉnh Lạng Sơn", Region = "Miền Bắc", IsActive = true },
            new() { Code = "22", Name = "Quảng Ninh", FullName = "Tỉnh Quảng Ninh", Region = "Miền Bắc", IsActive = true },
            new() { Code = "24", Name = "Bắc Giang", FullName = "Tỉnh Bắc Giang", Region = "Miền Bắc", IsActive = true },
            new() { Code = "25", Name = "Phú Thọ", FullName = "Tỉnh Phú Thọ", Region = "Miền Bắc", IsActive = true },
            new() { Code = "26", Name = "Vĩnh Phúc", FullName = "Tỉnh Vĩnh Phúc", Region = "Miền Bắc", IsActive = true },
            new() { Code = "27", Name = "Bắc Ninh", FullName = "Tỉnh Bắc Ninh", Region = "Miền Bắc", IsActive = true },
            new() { Code = "30", Name = "Hải Dương", FullName = "Tỉnh Hải Dương", Region = "Miền Bắc", IsActive = true },
            new() { Code = "31", Name = "Hải Phòng", FullName = "Thành phố Hải Phòng", Region = "Miền Bắc", IsActive = true },
            new() { Code = "33", Name = "Hưng Yên", FullName = "Tỉnh Hưng Yên", Region = "Miền Bắc", IsActive = true },
            new() { Code = "34", Name = "Thái Bình", FullName = "Tỉnh Thái Bình", Region = "Miền Bắc", IsActive = true },
            new() { Code = "35", Name = "Hà Nam", FullName = "Tỉnh Hà Nam", Region = "Miền Bắc", IsActive = true },
            new() { Code = "36", Name = "Nam Định", FullName = "Tỉnh Nam Định", Region = "Miền Bắc", IsActive = true },
            new() { Code = "37", Name = "Ninh Bình", FullName = "Tỉnh Ninh Bình", Region = "Miền Bắc", IsActive = true },

            // Miền Trung
            new() { Code = "38", Name = "Thanh Hóa", FullName = "Tỉnh Thanh Hóa", Region = "Miền Trung", IsActive = true },
            new() { Code = "40", Name = "Nghệ An", FullName = "Tỉnh Nghệ An", Region = "Miền Trung", IsActive = true },
            new() { Code = "42", Name = "Hà Tĩnh", FullName = "Tỉnh Hà Tĩnh", Region = "Miền Trung", IsActive = true },
            new() { Code = "44", Name = "Quảng Bình", FullName = "Tỉnh Quảng Bình", Region = "Miền Trung", IsActive = true },
            new() { Code = "45", Name = "Quảng Trị", FullName = "Tỉnh Quảng Trị", Region = "Miền Trung", IsActive = true },
            new() { Code = "46", Name = "Thừa Thiên Huế", FullName = "Tỉnh Thừa Thiên Huế", Region = "Miền Trung", IsActive = true },
            new() { Code = "48", Name = "Đà Nẵng", FullName = "Thành phố Đà Nẵng", Region = "Miền Trung", IsActive = true },
            new() { Code = "49", Name = "Quảng Nam", FullName = "Tỉnh Quảng Nam", Region = "Miền Trung", IsActive = true },
            new() { Code = "51", Name = "Quảng Ngãi", FullName = "Tỉnh Quảng Ngãi", Region = "Miền Trung", IsActive = true },
            new() { Code = "52", Name = "Bình Định", FullName = "Tỉnh Bình Định", Region = "Miền Trung", IsActive = true },
            new() { Code = "54", Name = "Phú Yên", FullName = "Tỉnh Phú Yên", Region = "Miền Trung", IsActive = true },
            new() { Code = "56", Name = "Khánh Hòa", FullName = "Tỉnh Khánh Hòa", Region = "Miền Trung", IsActive = true },
            new() { Code = "58", Name = "Ninh Thuận", FullName = "Tỉnh Ninh Thuận", Region = "Miền Trung", IsActive = true },
            new() { Code = "60", Name = "Bình Thuận", FullName = "Tỉnh Bình Thuận", Region = "Miền Trung", IsActive = true },
            new() { Code = "62", Name = "Kon Tum", FullName = "Tỉnh Kon Tum", Region = "Miền Trung", IsActive = true },
            new() { Code = "64", Name = "Gia Lai", FullName = "Tỉnh Gia Lai", Region = "Miền Trung", IsActive = true },
            new() { Code = "66", Name = "Đắk Lắk", FullName = "Tỉnh Đắk Lắk", Region = "Miền Trung", IsActive = true },
            new() { Code = "67", Name = "Đắk Nông", FullName = "Tỉnh Đắk Nông", Region = "Miền Trung", IsActive = true },
            new() { Code = "68", Name = "Lâm Đồng", FullName = "Tỉnh Lâm Đồng", Region = "Miền Trung", IsActive = true },

            // Miền Nam
            new() { Code = "70", Name = "Bình Phước", FullName = "Tỉnh Bình Phước", Region = "Miền Nam", IsActive = true },
            new() { Code = "72", Name = "Tây Ninh", FullName = "Tỉnh Tây Ninh", Region = "Miền Nam", IsActive = true },
            new() { Code = "74", Name = "Bình Dương", FullName = "Tỉnh Bình Dương", Region = "Miền Nam", IsActive = true },
            new() { Code = "75", Name = "Đồng Nai", FullName = "Tỉnh Đồng Nai", Region = "Miền Nam", IsActive = true },
            new() { Code = "77", Name = "Bà Rịa - Vũng Tàu", FullName = "Tỉnh Bà Rịa - Vũng Tàu", Region = "Miền Nam", IsActive = true },
            new() { Code = "79", Name = "TP. Hồ Chí Minh", FullName = "Thành phố Hồ Chí Minh", Region = "Miền Nam", IsActive = true },
            new() { Code = "80", Name = "Long An", FullName = "Tỉnh Long An", Region = "Miền Nam", IsActive = true },
            new() { Code = "82", Name = "Tiền Giang", FullName = "Tỉnh Tiền Giang", Region = "Miền Nam", IsActive = true },
            new() { Code = "83", Name = "Bến Tre", FullName = "Tỉnh Bến Tre", Region = "Miền Nam", IsActive = true },
            new() { Code = "84", Name = "Trà Vinh", FullName = "Tỉnh Trà Vinh", Region = "Miền Nam", IsActive = true },
            new() { Code = "86", Name = "Vĩnh Long", FullName = "Tỉnh Vĩnh Long", Region = "Miền Nam", IsActive = true },
            new() { Code = "87", Name = "Đồng Tháp", FullName = "Tỉnh Đồng Tháp", Region = "Miền Nam", IsActive = true },
            new() { Code = "89", Name = "An Giang", FullName = "Tỉnh An Giang", Region = "Miền Nam", IsActive = true },
            new() { Code = "91", Name = "Kiên Giang", FullName = "Tỉnh Kiên Giang", Region = "Miền Nam", IsActive = true },
            new() { Code = "92", Name = "Cần Thơ", FullName = "Thành phố Cần Thơ", Region = "Miền Nam", IsActive = true },
            new() { Code = "93", Name = "Hậu Giang", FullName = "Tỉnh Hậu Giang", Region = "Miền Nam", IsActive = true },
            new() { Code = "94", Name = "Sóc Trăng", FullName = "Tỉnh Sóc Trăng", Region = "Miền Nam", IsActive = true },
            new() { Code = "95", Name = "Bạc Liêu", FullName = "Tỉnh Bạc Liêu", Region = "Miền Nam", IsActive = true },
            new() { Code = "96", Name = "Cà Mau", FullName = "Tỉnh Cà Mau", Region = "Miền Nam", IsActive = true }
        };
    }

    public static List<ProvinceConfig> GetSampleConfigs()
    {
        return new List<ProvinceConfig>
        {
            new()
            {
                ProvinceCode = "01",
                DepartmentName = "Sở Xây dựng Hà Nội",
                WebsiteUrl = "https://soxaydung.hanoi.gov.vn",
                PriceListUrl = "https://soxaydung.hanoi.gov.vn/gia-vat-lieu-xay-dung",
                UpdateScheduleDay = 15,
                AutoUpdateEnabled = true,
                PriceTableSelector = ".price-table",
                LastModified = DateTime.UtcNow
            },
            new()
            {
                ProvinceCode = "79",
                DepartmentName = "Sở Xây dựng TP. Hồ Chí Minh",
                WebsiteUrl = "http://www.soc.hochiminhcity.gov.vn",
                PriceListUrl = "http://www.soc.hochiminhcity.gov.vn/gia-vat-lieu",
                UpdateScheduleDay = 15,
                AutoUpdateEnabled = true,
                PriceTableSelector = ".content-table",
                LastModified = DateTime.UtcNow
            },
            new()
            {
                ProvinceCode = "48",
                DepartmentName = "Sở Xây dựng Đà Nẵng",
                WebsiteUrl = "https://soxaydung.danang.gov.vn",
                PriceListUrl = "https://soxaydung.danang.gov.vn/gia-vat-lieu",
                UpdateScheduleDay = 15,
                AutoUpdateEnabled = true,
                PriceTableSelector = ".price-list",
                LastModified = DateTime.UtcNow
            }
        };
    }

    public static List<Material> GetExtendedMaterials()
    {
        return new List<Material>
        {
            // Bê tông
            new() { Id = 1, Code = "BT_B15", Name = "Bê tông thương phẩm B15", Unit = "m3", Category = MaterialCategory.Concrete, IsActive = true },
            new() { Id = 2, Code = "BT_B20", Name = "Bê tông thương phẩm B20", Unit = "m3", Category = MaterialCategory.Concrete, IsActive = true },
            new() { Id = 3, Code = "BT_B25", Name = "Bê tông thương phẩm B25", Unit = "m3", Category = MaterialCategory.Concrete, IsActive = true },
            new() { Id = 4, Code = "BT_B30", Name = "Bê tông thương phẩm B30", Unit = "m3", Category = MaterialCategory.Concrete, IsActive = true },

            // Thép
            new() { Id = 5, Code = "THEP_CB240T", Name = "Thép thanh trơn CB240-T", Unit = "kg", Category = MaterialCategory.Steel, IsActive = true },
            new() { Id = 6, Code = "THEP_CB300V", Name = "Thép thanh vằn CB300-V", Unit = "kg", Category = MaterialCategory.Steel, IsActive = true },
            new() { Id = 7, Code = "THEP_CB400V", Name = "Thép thanh vằn CB400-V", Unit = "kg", Category = MaterialCategory.Steel, IsActive = true },
            new() { Id = 8, Code = "THEP_CB500V", Name = "Thép thanh vằn CB500-V", Unit = "kg", Category = MaterialCategory.Steel, IsActive = true },

            // Gạch
            new() { Id = 9, Code = "GACH_RD", Name = "Gạch rỗng đỏ", Unit = "viên", Category = MaterialCategory.Brick, IsActive = true },
            new() { Id = 10, Code = "GACH_KN", Name = "Gạch không nung", Unit = "viên", Category = MaterialCategory.Brick, IsActive = true },
            new() { Id = 11, Code = "GACH_BT", Name = "Gạch bê tông", Unit = "viên", Category = MaterialCategory.Brick, IsActive = true },

            // Cát đá
            new() { Id = 12, Code = "CAT_XD", Name = "Cát xây dựng", Unit = "m3", Category = MaterialCategory.Sand, IsActive = true },
            new() { Id = 13, Code = "CAT_VT", Name = "Cát vàng tô", Unit = "m3", Category = MaterialCategory.Sand, IsActive = true },
            new() { Id = 14, Code = "DA_1X2", Name = "Đá dăm 1x2", Unit = "m3", Category = MaterialCategory.Stone, IsActive = true },
            new() { Id = 15, Code = "DA_4X6", Name = "Đá dăm 4x6", Unit = "m3", Category = MaterialCategory.Stone, IsActive = true },

            // Xi măng
            new() { Id = 16, Code = "XI_MANG_PCB30", Name = "Xi măng PCB30", Unit = "kg", Category = MaterialCategory.Cement, IsActive = true },
            new() { Id = 17, Code = "XI_MANG_PCB40", Name = "Xi măng PCB40", Unit = "kg", Category = MaterialCategory.Cement, IsActive = true },
            new() { Id = 18, Code = "XI_MANG_PC50", Name = "Xi măng PC50", Unit = "kg", Category = MaterialCategory.Cement, IsActive = true }
        };
    }
}