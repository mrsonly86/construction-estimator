using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Enums;
using ConstructionEstimator.Infrastructure.Data;

namespace ConstructionEstimator.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ConstructionEstimatorDbContext context)
    {
        await SeedMaterialsAsync(context);
        await SeedLaborsAsync(context);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMaterialsAsync(ConstructionEstimatorDbContext context)
    {
        if (context.Materials.Any()) return;

        var materials = new List<Material>
        {
            // Xi măng và chất kết dính
            new Material { Name = "Xi măng Portland PCB40", Code = "XM001", Unit = "bao", UnitPrice = 105000, Category = "Xi măng", Supplier = "Xi măng Hà Tiên", Description = "Xi măng Portland thông dụng 50kg/bao" },
            new Material { Name = "Xi măng Portland PCB50", Code = "XM002", Unit = "bao", UnitPrice = 115000, Category = "Xi măng", Supplier = "Xi măng Hòa Phát", Description = "Xi măng Portland cao cấp 50kg/bao" },
            new Material { Name = "Vôi tôi", Code = "VT001", Unit = "kg", UnitPrice = 3500, Category = "Chất kết dính", Supplier = "Vôi Thanh Hóa", Description = "Vôi tôi xây dựng" },

            // Cát, đá, sỏi
            new Material { Name = "Cát xây dựng", Code = "CT001", Unit = "m3", UnitPrice = 420000, Category = "Cát", Supplier = "Mỏ cát Hòa Bình", Description = "Cát xây dựng loại 1" },
            new Material { Name = "Cát bê tông", Code = "CT002", Unit = "m3", UnitPrice = 380000, Category = "Cát", Supplier = "Mỏ cát Vĩnh Phúc", Description = "Cát bê tông loại A" },
            new Material { Name = "Đá 1x2cm", Code = "DA001", Unit = "m3", UnitPrice = 520000, Category = "Đá", Supplier = "Mỏ đá Hòa Bình", Description = "Đá dăm 1x2cm" },
            new Material { Name = "Đá 2x4cm", Code = "DA002", Unit = "m3", UnitPrice = 480000, Category = "Đá", Supplier = "Mỏ đá Hòa Bình", Description = "Đá dăm 2x4cm" },
            new Material { Name = "Sỏi 4x6cm", Code = "SO001", Unit = "m3", UnitPrice = 450000, Category = "Đá", Supplier = "Mỏ sỏi Vĩnh Phúc", Description = "Sỏi tự nhiên 4x6cm" },

            // Gạch xây
            new Material { Name = "Gạch đỏ nung 220x105x60", Code = "GD001", Unit = "viên", UnitPrice = 820, Category = "Gạch", Supplier = "Gạch Hà Nam", Description = "Gạch đỏ nung tiêu chuẩn" },
            new Material { Name = "Gạch block 190x90x190", Code = "GB001", Unit = "viên", UnitPrice = 4500, Category = "Gạch", Supplier = "Gạch AAC Hebel", Description = "Gạch bê tông nhẹ AAC" },
            new Material { Name = "Gạch 6 lỗ 220x105x60", Code = "G6L001", Unit = "viên", UnitPrice = 950, Category = "Gạch", Supplier = "Gạch Thanh Hóa", Description = "Gạch nung 6 lỗ" },

            // Thép xây dựng
            new Material { Name = "Thép CB240-T phi 6", Code = "CB006", Unit = "kg", UnitPrice = 15800, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB240-T đường kính 6mm" },
            new Material { Name = "Thép CB240-T phi 8", Code = "CB008", Unit = "kg", UnitPrice = 15600, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB240-T đường kính 8mm" },
            new Material { Name = "Thép CB240-T phi 10", Code = "CB010", Unit = "kg", UnitPrice = 15400, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB240-T đường kính 10mm" },
            new Material { Name = "Thép CB300-V phi 12", Code = "CB012", Unit = "kg", UnitPrice = 15200, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB300-V đường kính 12mm" },
            new Material { Name = "Thép CB300-V phi 14", Code = "CB014", Unit = "kg", UnitPrice = 15200, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB300-V đường kính 14mm" },
            new Material { Name = "Thép CB300-V phi 16", Code = "CB016", Unit = "kg", UnitPrice = 15200, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB300-V đường kính 16mm" },
            new Material { Name = "Thép CB300-V phi 18", Code = "CB018", Unit = "kg", UnitPrice = 15200, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB300-V đường kính 18mm" },
            new Material { Name = "Thép CB300-V phi 20", Code = "CB020", Unit = "kg", UnitPrice = 15200, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB300-V đường kính 20mm" },
            new Material { Name = "Thép CB300-V phi 22", Code = "CB022", Unit = "kg", UnitPrice = 15200, Category = "Thép", Supplier = "Thép Hòa Phát", Description = "Thép CB300-V đường kính 22mm" },

            // Gỗ xây dựng
            new Material { Name = "Gỗ thông xẻ 2x8x400cm", Code = "GO001", Unit = "m3", UnitPrice = 4200000, Category = "Gỗ", Supplier = "Gỗ Quảng Trị", Description = "Gỗ thông xẻ làm ván khuôn" },
            new Material { Name = "Gỗ keo 5x10x400cm", Code = "GO002", Unit = "m3", UnitPrice = 3800000, Category = "Gỗ", Supplier = "Gỗ Phú Thọ", Description = "Gỗ keo làm dầm chống" },
            new Material { Name = "Ván ép phủ phim 15mm", Code = "VP001", Unit = "tấm", UnitPrice = 420000, Category = "Gỗ", Supplier = "Ván ép Việt Nam", Description = "Ván ép phủ phim 1.22x2.44m" },

            // Gạch ốp lát
            new Material { Name = "Gạch ceramic 30x60cm", Code = "GC001", Unit = "m2", UnitPrice = 135000, Category = "Gạch ốp lát", Supplier = "Gạch Đồng Tâm", Description = "Gạch ceramic ốp tường" },
            new Material { Name = "Gạch granite 60x60cm", Code = "GG001", Unit = "m2", UnitPrice = 285000, Category = "Gạch ốp lát", Supplier = "Gạch Viglacera", Description = "Gạch granite lát nền" },
            new Material { Name = "Gạch mosaic thủy tinh", Code = "GM001", Unit = "m2", UnitPrice = 180000, Category = "Gạch ốp lát", Supplier = "Mosaic Hà Nội", Description = "Gạch mosaic ốp phòng tắm" },

            // Sơn và lót
            new Material { Name = "Sơn nước ngoại thất", Code = "SN001", Unit = "thùng", UnitPrice = 850000, Category = "Sơn", Supplier = "Sơn Jotun", Description = "Sơn nước ngoại thất 18L" },
            new Material { Name = "Sơn dầu nội thất", Code = "SD001", Unit = "thùng", UnitPrice = 780000, Category = "Sơn", Supplier = "Sơn Kova", Description = "Sơn dầu nội thất 18L" },
            new Material { Name = "Lót chống kiềm", Code = "LCK001", Unit = "thùng", UnitPrice = 620000, Category = "Sơn", Supplier = "Sơn Nippon", Description = "Lót chống kiềm 18L" },

            // Ống nước và phụ kiện
            new Material { Name = "Ống nước PPR D20", Code = "ON020", Unit = "m", UnitPrice = 18500, Category = "Ống nước", Supplier = "Ống Nhựa Tiền Phong", Description = "Ống PPR D20 nước lạnh" },
            new Material { Name = "Ống nước PPR D25", Code = "ON025", Unit = "m", UnitPrice = 24000, Category = "Ống nước", Supplier = "Ống Nhựa Tiền Phong", Description = "Ống PPR D25 nước lạnh" },
            new Material { Name = "Ống nước PPR D32", Code = "ON032", Unit = "m", UnitPrice = 35000, Category = "Ống nước", Supplier = "Ống Nhựa Tiền Phong", Description = "Ống PPR D32 nước lạnh" },

            // Thiết bị vệ sinh
            new Material { Name = "Bồn cầu 1 khối", Code = "BC001", Unit = "bộ", UnitPrice = 1850000, Category = "Thiết bị vệ sinh", Supplier = "TOTO", Description = "Bồn cầu 1 khối cao cấp" },
            new Material { Name = "Chậu rửa mặt", Code = "CRM001", Unit = "cái", UnitPrice = 680000, Category = "Thiết bị vệ sinh", Supplier = "American Standard", Description = "Chậu rửa mặt đặt bàn" },
            new Material { Name = "Sen tắm đứng", Code = "ST001", Unit = "bộ", UnitPrice = 1250000, Category = "Thiết bị vệ sinh", Supplier = "Hansgrohe", Description = "Sen tắm đứng cao cấp" },

            // Điện
            new Material { Name = "Dây điện Cu 1.5mm2", Code = "DD15", Unit = "m", UnitPrice = 12500, Category = "Điện", Supplier = "Dây điện Cadivi", Description = "Dây điện đồng 1.5mm2" },
            new Material { Name = "Dây điện Cu 2.5mm2", Code = "DD25", Unit = "m", UnitPrice = 18500, Category = "Điện", Supplier = "Dây điện Cadivi", Description = "Dây điện đồng 2.5mm2" },
            new Material { Name = "Ống luồn dây D20", Code = "OLD20", Unit = "m", UnitPrice = 8500, Category = "Điện", Supplier = "Ống điện Hà Nội", Description = "Ống luồn dây PVC D20" },

            // Cửa và khung cửa
            new Material { Name = "Cửa gỗ HDF veneer", Code = "CG001", Unit = "bộ", UnitPrice = 2800000, Category = "Cửa", Supplier = "Cửa Austdoor", Description = "Cửa gỗ HDF veneer kèm khung" },
            new Material { Name = "Cửa nhôm kính", Code = "CNK001", Unit = "m2", UnitPrice = 850000, Category = "Cửa", Supplier = "Nhôm Xingfa", Description = "Cửa nhôm kính cường lực" },

            // Kính xây dựng
            new Material { Name = "Kính cường lực 8mm", Code = "KCL008", Unit = "m2", UnitPrice = 380000, Category = "Kính", Supplier = "Kính Việt Nhật", Description = "Kính cường lực trong suốt 8mm" },
            new Material { Name = "Kính hộp 6+9A+6", Code = "KH696", Unit = "m2", UnitPrice = 520000, Category = "Kính", Supplier = "Kính Guardian", Description = "Kính hộp Low-E 6+9A+6" },

            // Chống thấm
            new Material { Name = "Màng chống thấm SBS", Code = "MCT001", Unit = "m2", UnitPrice = 85000, Category = "Chống thấm", Supplier = "Chống thấm Sika", Description = "Màng chống thấm SBS 4mm" },
            new Material { Name = "Sơn chống thấm polyurethane", Code = "SCT001", Unit = "kg", UnitPrice = 125000, Category = "Chống thấm", Supplier = "Chống thấm Fosroc", Description = "Sơn chống thấm PU 2 thành phần" },

            // Kết cấu thép
            new Material { Name = "Thép hình I100", Code = "THI100", Unit = "kg", UnitPrice = 18500, Category = "Thép hình", Supplier = "Thép Hòa Phát", Description = "Thép hình chữ I 100x50x5" },
            new Material { Name = "Thép hình U100", Code = "THU100", Unit = "kg", UnitPrice = 18200, Category = "Thép hình", Supplier = "Thép Hòa Phát", Description = "Thép hình chữ U 100x50x5" },
            new Material { Name = "Thép hộp 50x50x2", Code = "TH5050", Unit = "kg", UnitPrice = 19500, Category = "Thép hình", Supplier = "Thép Hòa Phát", Description = "Thép hộp vuông 50x50x2mm" },

            // Vật liệu cách nhiệt
            new Material { Name = "Tấm cách nhiệt XPS 50mm", Code = "CN001", Unit = "m2", UnitPrice = 185000, Category = "Cách nhiệt", Supplier = "Cách nhiệt Kingspan", Description = "Tấm cách nhiệt XPS dày 50mm" },
            new Material { Name = "Bông thủy tinh 75mm", Code = "BTT001", Unit = "m2", UnitPrice = 65000, Category = "Cách nhiệt", Supplier = "Bông cách nhiệt Paroc", Description = "Bông thủy tinh dày 75mm" },

            // Vật liệu hoàn thiện
            new Material { Name = "Thạch cao trang trí", Code = "TG001", Unit = "m2", UnitPrice = 75000, Category = "Hoàn thiện", Supplier = "Thạch cao Knauf", Description = "Tấm thạch cao chịu ẩm 12mm" },
            new Material { Name = "Tấm ốp PVC vân gỗ", Code = "PVC001", Unit = "m2", UnitPrice = 145000, Category = "Hoàn thiện", Supplier = "PVC An Cường", Description = "Tấm ốp PVC vân gỗ cao cấp" },
        };

        foreach (var material in materials)
        {
            material.LastUpdated = DateTime.Now;
        }

        await context.Materials.AddRangeAsync(materials);
    }

    private static async Task SeedLaborsAsync(ConstructionEstimatorDbContext context)
    {
        if (context.Labors.Any()) return;

        var labors = new List<Labor>
        {
            // Thợ xây
            new Labor { Name = "Thợ xây trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 35000, DailyRate = 280000, Category = "Xây dựng", Description = "Thợ xây kinh nghiệm trên 10 năm" },
            new Labor { Name = "Thợ xây chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 28000, DailyRate = 224000, Category = "Xây dựng", Description = "Thợ xây kinh nghiệm 5-10 năm" },
            new Labor { Name = "Thợ xây phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 22000, DailyRate = 176000, Category = "Xây dựng", Description = "Thợ xây kinh nghiệm dưới 5 năm" },

            // Thợ sắt
            new Labor { Name = "Thợ sắt trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 38000, DailyRate = 304000, Category = "Cốt thép", Description = "Thợ sắt bậc cao" },
            new Labor { Name = "Thợ sắt chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 30000, DailyRate = 240000, Category = "Cốt thép", Description = "Thợ sắt có kinh nghiệm" },
            new Labor { Name = "Thợ sắt phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 24000, DailyRate = 192000, Category = "Cốt thép", Description = "Thợ sắt học việc" },

            // Thợ hàn
            new Labor { Name = "Thợ hàn bậc cao", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 40000, DailyRate = 320000, Category = "Hàn", Description = "Thợ hàn chứng chỉ quốc tế" },
            new Labor { Name = "Thợ hàn chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 32000, DailyRate = 256000, Category = "Hàn", Description = "Thợ hàn có chứng chỉ" },

            // Thợ gỗ
            new Labor { Name = "Thợ mộc trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 36000, DailyRate = 288000, Category = "Mộc", Description = "Thợ mộc bậc thầy" },
            new Labor { Name = "Thợ mộc chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 28000, DailyRate = 224000, Category = "Mộc", Description = "Thợ mộc có kinh nghiệm" },
            new Labor { Name = "Thợ mộc phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 22000, DailyRate = 176000, Category = "Mộc", Description = "Thợ mộc học việc" },

            // Thợ sơn
            new Labor { Name = "Thợ sơn trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 32000, DailyRate = 256000, Category = "Sơn", Description = "Thợ sơn bậc thầy" },
            new Labor { Name = "Thợ sơn chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 26000, DailyRate = 208000, Category = "Sơn", Description = "Thợ sơn có kinh nghiệm" },
            new Labor { Name = "Thợ sơn phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 20000, DailyRate = 160000, Category = "Sơn", Description = "Thợ sơn học việc" },

            // Thợ điện
            new Labor { Name = "Thợ điện trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 38000, DailyRate = 304000, Category = "Điện", Description = "Thợ điện có chứng chỉ cao áp" },
            new Labor { Name = "Thợ điện chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 30000, DailyRate = 240000, Category = "Điện", Description = "Thợ điện có chứng chỉ hành nghề" },
            new Labor { Name = "Thợ điện phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 24000, DailyRate = 192000, Category = "Điện", Description = "Thợ điện học việc" },

            // Thợ nước
            new Labor { Name = "Thợ nước trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 35000, DailyRate = 280000, Category = "Nước", Description = "Thợ nước bậc cao" },
            new Labor { Name = "Thợ nước chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 28000, DailyRate = 224000, Category = "Nước", Description = "Thợ nước có kinh nghiệm" },
            new Labor { Name = "Thợ nước phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 22000, DailyRate = 176000, Category = "Nước", Description = "Thợ nước học việc" },

            // Thợ ốp lát
            new Labor { Name = "Thợ ốp lát trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 34000, DailyRate = 272000, Category = "Ốp lát", Description = "Thợ ốp lát bậc thầy" },
            new Labor { Name = "Thợ ốp lát chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 27000, DailyRate = 216000, Category = "Ốp lát", Description = "Thợ ốp lát có kinh nghiệm" },
            new Labor { Name = "Thợ ốp lát phụ", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 21000, DailyRate = 168000, Category = "Ốp lát", Description = "Thợ ốp lát học việc" },

            // Thợ cửa nhôm kính
            new Labor { Name = "Thợ nhôm kính trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 36000, DailyRate = 288000, Category = "Nhôm kính", Description = "Thợ nhôm kính bậc cao" },
            new Labor { Name = "Thợ nhôm kính chính", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 29000, DailyRate = 232000, Category = "Nhôm kính", Description = "Thợ nhôm kính có kinh nghiệm" },

            // Công nhân phổ thông
            new Labor { Name = "Công nhân vận chuyển", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 18000, DailyRate = 144000, Category = "Phổ thông", Description = "Công nhân vận chuyển vật liệu" },
            new Labor { Name = "Công nhân làm vệ sinh", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 16000, DailyRate = 128000, Category = "Phổ thông", Description = "Công nhân vệ sinh công trình" },
            new Labor { Name = "Công nhân bốc xếp", SkillLevel = LaborSkillLevel.Apprentice, HourlyRate = 17000, DailyRate = 136000, Category = "Phổ thông", Description = "Công nhân bốc xếp nguyên vật liệu" },

            // Giám sát và quản lý
            new Labor { Name = "Kỹ sư giám sát", SkillLevel = LaborSkillLevel.Supervisor, HourlyRate = 50000, DailyRate = 400000, Category = "Quản lý", Description = "Kỹ sư giám sát thi công" },
            new Labor { Name = "Trưởng ca", SkillLevel = LaborSkillLevel.Supervisor, HourlyRate = 42000, DailyRate = 336000, Category = "Quản lý", Description = "Trưởng ca thi công" },
            new Labor { Name = "Tổ trưởng", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 38000, DailyRate = 304000, Category = "Quản lý", Description = "Tổ trưởng các hạng mục" },

            // Thợ máy
            new Labor { Name = "Thợ máy xúc", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 32000, DailyRate = 256000, Category = "Máy móc", Description = "Thợ vận hành máy xúc" },
            new Labor { Name = "Thợ máy trộn", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 30000, DailyRate = 240000, Category = "Máy móc", Description = "Thợ vận hành máy trộn bê tông" },
            new Labor { Name = "Thợ cần cẩu", SkillLevel = LaborSkillLevel.Expert, HourlyRate = 45000, DailyRate = 360000, Category = "Máy móc", Description = "Thợ vận hành cần cẩu tháp" },

            // Thợ chuyên ngành khác
            new Labor { Name = "Thợ chống thấm", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 32000, DailyRate = 256000, Category = "Chống thấm", Description = "Thợ thi công chống thấm" },
            new Labor { Name = "Thợ cách nhiệt", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 28000, DailyRate = 224000, Category = "Cách nhiệt", Description = "Thợ thi công cách nhiệt" },
            new Labor { Name = "Thợ thạch cao", SkillLevel = LaborSkillLevel.Skilled, HourlyRate = 30000, DailyRate = 240000, Category = "Hoàn thiện", Description = "Thợ thi công thạch cao trang trí" },
        };

        await context.Labors.AddRangeAsync(labors);
    }
}