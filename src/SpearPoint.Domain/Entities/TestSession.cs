namespace SpearPoint.Domain.Entities;

public class TestItem
{
    public long QuestionId { get; set; }
    public int? SelectedIndex { get; set; }
    public Section Section { get; set; }
    public Question? Question { get; set; }

}

public class ScoreSummary
{
    public int Raw { get; set; }
    public double Percent { get; set; }
    public Dictionary<Section, (int Correct, int Total)> SectionBreakdown { get; set; } = new();
}

public class TestSession
{
    public long Id { get; set; } 
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAtUtc { get; set; }
    public List<TestItem> Items { get; set; } = new();
    public ScoreSummary Summary { get; set; } = new();
}

