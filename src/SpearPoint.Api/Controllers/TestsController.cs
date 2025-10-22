using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpearPoint.Contracts.Requests;
using SpearPoint.Domain.Entities;
using SpearPoint.Infrastructure.Persistence;

namespace SpearPoint.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestsController : ControllerBase
{
    private readonly SpearPointDbContext _db;
    public TestsController(SpearPointDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateTestSessionRequest req, CancellationToken ct)
    {
        var session = new TestSession
        {
            Items = req.Questions.Select(q => new TestItem
            {
                QuestionId = q.Id,
                Section = q.Section,
            }).ToList()
        };
        _db.Add(session);
        await _db.SaveChangesAsync(ct);
        return Ok(session.Id);
    }

    [HttpPost("{id:long}/answers")]
    public async Task<ActionResult> Submit(long id, [FromBody] SubmitAnswerRequest req, CancellationToken ct)
    {
        var s = await _db.TestSessions.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (s is null) return NotFound();

        var item = s.Items.FirstOrDefault(i => i.QuestionId == req.QuestionId);
        if (item is null) return BadRequest("Unknown question");
        item.SelectedIndex = req.SelectedIndex;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpPost("{id:long}/finalize")]
    public async Task<ActionResult<ScoreSummary>> FinalizeAsync(long testSessionId, CancellationToken cancellationToken)
    {
        // Retrieve the full test session, including its items, questions, and choices
        var testSession = await _db.TestSessions
            .Include(session => session.Items)
                .ThenInclude(item => item.Question)
                    .ThenInclude(question => question.Choices)
            .FirstOrDefaultAsync(session => session.Id == testSessionId, cancellationToken);

        if (testSession is null)
            return NotFound();

        int totalQuestionCount = testSession.Items.Count;

        // Count how many questions were answered correctly
        int correctAnswerCount = testSession.Items.Count(item =>
            item.SelectedIndex.HasValue &&
            item.Question.Choices.Any(choice =>
                choice.Id == item.SelectedIndex && choice.IsCorrect));

        // Build the overall summary
        var scoreSummary = new ScoreSummary
        {
            Raw = correctAnswerCount,
            Percent = totalQuestionCount == 0
                ? 0
                : correctAnswerCount * 100.0 / totalQuestionCount
        };

        // Build per-section breakdowns (correct vs. total)
        foreach (var sectionGroup in testSession.Items.GroupBy(item => item.Question.Section))
        {
            int sectionCorrectCount = sectionGroup.Count(item =>
                item.SelectedIndex.HasValue &&
                item.Question.Choices.Any(choice =>
                    choice.Id == item.SelectedIndex && choice.IsCorrect));

            scoreSummary.SectionBreakdown[sectionGroup.Key] =
                (sectionCorrectCount, sectionGroup.Count());
        }

        // Update the test session and persist changes
        testSession.Summary = scoreSummary;
        testSession.EndedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return Ok(scoreSummary);
    }


}
