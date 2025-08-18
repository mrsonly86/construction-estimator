using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class Labor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LaborSkillLevel SkillLevel { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal DailyRate { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}