using Microsoft.AspNetCore.Mvc;
using SpearPoint.Application.Abstractions;
using SpearPoint.Domain.Entities;
using SpearPoint.Domain.Enums;
using SpearPoint.Infrastructure.Services;

namespace SpearPoint.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionRepository _repo;
    public QuestionsController(IQuestionRepository repo) => _repo = repo;

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateQuestionRequest req)
    {
        var q = new Question
        {
            Section = req.Section,
            Type = req.Type,
            Stem = req.Stem,
            AnswerExplanation = req.AnswerExplanation,
            Difficulty = req.Difficulty,
            Source = req.Source,
            Status = QuestionStatus.Active,
            Tags = req.Tags?.ToList() ?? new()
        };
        foreach (var c in req.Choices)
            q.Choices.Add(new QuestionChoice { Label = c.Label, Text = c.Text, IsCorrect = c.IsCorrect });


        q.DedupeHash = DedupeHashService.Compute(q);


        await _repo.AddAsync(q);
        return Ok(q.Id);
    }

    [HttpGet]
    public async Task<ActionResult> List([FromQuery] string? section, [FromQuery] int? difficulty)
    => Ok(await _repo.ListAsync(section,
        difficulty));

    public record CreateQuestionRequest(
    string Section,
    QuestionType Type,
    string Stem,
    int Difficulty,
    string? AnswerExplanation,
    QuestionSource Source,
    IEnumerable<string>? Tags,
    IEnumerable<Choice> Choices
    );


    public record Choice(string Label, string Text, bool IsCorrect);
}
