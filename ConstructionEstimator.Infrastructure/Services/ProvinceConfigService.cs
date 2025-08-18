using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.Infrastructure.Services;

public class ProvinceConfigService : IProvinceConfigService
{
    private readonly Dictionary<string, string> _provinceConfigs = new()
    {
        { "Hà Nội", "https://api.mock.hanoi.gov.vn/material-prices" },
        { "Hồ Chí Minh", "https://api.mock.hcmc.gov.vn/material-prices" },
        { "Đà Nẵng", "https://api.mock.danang.gov.vn/material-prices" },
        { "Hải Phòng", "https://api.mock.haiphong.gov.vn/material-prices" },
        { "Cần Thơ", "https://api.mock.cantho.gov.vn/material-prices" },
        { "An Giang", "https://api.mock.angiang.gov.vn/material-prices" },
        { "Bà Rịa - Vũng Tàu", "https://api.mock.brvt.gov.vn/material-prices" },
        { "Bắc Giang", "https://api.mock.bacgiang.gov.vn/material-prices" },
        { "Bắc Kạn", "https://api.mock.backan.gov.vn/material-prices" },
        { "Bạc Liêu", "https://api.mock.baclieu.gov.vn/material-prices" },
        { "Bắc Ninh", "https://api.mock.bacninh.gov.vn/material-prices" },
        { "Bến Tre", "https://api.mock.bentre.gov.vn/material-prices" },
        { "Bình Định", "https://api.mock.binhdinh.gov.vn/material-prices" },
        { "Bình Dương", "https://api.mock.binhduong.gov.vn/material-prices" },
        { "Bình Phước", "https://api.mock.binhphuoc.gov.vn/material-prices" },
        { "Bình Thuận", "https://api.mock.binhthuan.gov.vn/material-prices" },
        { "Cà Mau", "https://api.mock.camau.gov.vn/material-prices" },
        { "Cao Bằng", "https://api.mock.caobang.gov.vn/material-prices" },
        { "Đắk Lắk", "https://api.mock.daklak.gov.vn/material-prices" },
        { "Đắk Nông", "https://api.mock.daknong.gov.vn/material-prices" },
        { "Điện Biên", "https://api.mock.dienbien.gov.vn/material-prices" },
        { "Đồng Nai", "https://api.mock.dongnai.gov.vn/material-prices" },
        { "Đồng Tháp", "https://api.mock.dongthap.gov.vn/material-prices" },
        { "Gia Lai", "https://api.mock.gialai.gov.vn/material-prices" },
        { "Hà Giang", "https://api.mock.hagiang.gov.vn/material-prices" },
        { "Hà Nam", "https://api.mock.hanam.gov.vn/material-prices" },
        { "Hà Tĩnh", "https://api.mock.hatinh.gov.vn/material-prices" },
        { "Hải Dương", "https://api.mock.haiduong.gov.vn/material-prices" },
        { "Hậu Giang", "https://api.mock.haugiang.gov.vn/material-prices" },
        { "Hòa Bình", "https://api.mock.hoabinh.gov.vn/material-prices" },
        { "Hưng Yên", "https://api.mock.hungyen.gov.vn/material-prices" },
        { "Khánh Hòa", "https://api.mock.khanhhoa.gov.vn/material-prices" },
        { "Kiên Giang", "https://api.mock.kiengiang.gov.vn/material-prices" },
        { "Kon Tum", "https://api.mock.kontum.gov.vn/material-prices" },
        { "Lai Châu", "https://api.mock.laichau.gov.vn/material-prices" },
        { "Lâm Đồng", "https://api.mock.lamdong.gov.vn/material-prices" },
        { "Lạng Sơn", "https://api.mock.langson.gov.vn/material-prices" },
        { "Lào Cai", "https://api.mock.laocai.gov.vn/material-prices" },
        { "Long An", "https://api.mock.longan.gov.vn/material-prices" },
        { "Nam Định", "https://api.mock.namdinh.gov.vn/material-prices" },
        { "Nghệ An", "https://api.mock.nghean.gov.vn/material-prices" },
        { "Ninh Bình", "https://api.mock.ninhbinh.gov.vn/material-prices" },
        { "Ninh Thuận", "https://api.mock.ninhthuan.gov.vn/material-prices" },
        { "Phú Thọ", "https://api.mock.phutho.gov.vn/material-prices" },
        { "Phú Yên", "https://api.mock.phuyen.gov.vn/material-prices" },
        { "Quảng Bình", "https://api.mock.quangbinh.gov.vn/material-prices" },
        { "Quảng Nam", "https://api.mock.quangnam.gov.vn/material-prices" },
        { "Quảng Ngãi", "https://api.mock.quangngai.gov.vn/material-prices" },
        { "Quảng Ninh", "https://api.mock.quangninh.gov.vn/material-prices" },
        { "Quảng Trị", "https://api.mock.quangtri.gov.vn/material-prices" },
        { "Sóc Trăng", "https://api.mock.soctrang.gov.vn/material-prices" },
        { "Sơn La", "https://api.mock.sonla.gov.vn/material-prices" },
        { "Tây Ninh", "https://api.mock.tayninh.gov.vn/material-prices" },
        { "Thái Bình", "https://api.mock.thaibinh.gov.vn/material-prices" },
        { "Thái Nguyên", "https://api.mock.thainguyen.gov.vn/material-prices" },
        { "Thanh Hóa", "https://api.mock.thanhhoa.gov.vn/material-prices" },
        { "Thừa Thiên Huế", "https://api.mock.hue.gov.vn/material-prices" },
        { "Tiền Giang", "https://api.mock.tiengiang.gov.vn/material-prices" },
        { "Trà Vinh", "https://api.mock.travinh.gov.vn/material-prices" },
        { "Tuyên Quang", "https://api.mock.tuyenquang.gov.vn/material-prices" },
        { "Vĩnh Long", "https://api.mock.vinhlong.gov.vn/material-prices" },
        { "Vĩnh Phúc", "https://api.mock.vinhphuc.gov.vn/material-prices" },
        { "Yên Bái", "https://api.mock.yenbai.gov.vn/material-prices" }
    };

    public IEnumerable<string> GetProvinces()
    {
        return _provinceConfigs.Keys;
    }

    public string GetDataSourceUrl(string province)
    {
        return _provinceConfigs.TryGetValue(province, out var url) ? url : string.Empty;
    }

    public bool IsProvinceSupported(string province)
    {
        return _provinceConfigs.ContainsKey(province);
    }

    public async Task<Dictionary<string, string>> GetProvinceConfigsAsync()
    {
        await Task.Delay(100); // Simulate async operation
        return _provinceConfigs;
    }
}