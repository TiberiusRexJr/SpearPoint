using Microsoft.AspNetCore.Mvc;
using SpearPoint.Application.Abstractions;
using SpearPoint.Contracts.Requests;
using SpearPoint.Contracts.Responses;
using SpearPoint.Domain.Entities;
using SpearPoint.Infrastructure.Services;

namespace SpearPoint.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionGenerator _generator;
    private readonly IQuestionRepository _repo;

    public QuestionsController(IQuestionGenerator generator, IQuestionRepository repo)
    {
        _generator = generator;
        _repo = repo;
    }

    // POST api/v1/questions/generate
    // Generate N questions (no persistence by default)
    [HttpPost("generate")]
    public async Task<ActionResult<IReadOnlyList<QuestionDto>>> Generate([FromBody] GenerateQuestionsRequest req)
    {
        if (req.Count <= 0 || req.Count > 50) return BadRequest("count must be 1..50");

        var generated = await _generator.GenerateAsync(req.Section, req.Difficulty, req.Count);

        var dtos = generated.Select(q => new QuestionDto(
            q.Id,
            q.Section,
            q.Stem,
            q.Choices.Select(c => new QuestionChoiceDto(c.Label, c.Text, c.IsCorrect)).ToList(),
            q.Difficulty,
            q.Tags.ToList()
        )).ToList();

        return Ok(dtos);
    }

    // POST api/v1/questions
    // Create (persist) a question provided by the client
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
            Status = req.Status,
            Tags = req.Tags?.ToList() ?? new()
        };

        foreach (var c in req.Choices)
            q.Choices.Add(new QuestionChoice { Label = c.Label, Text = c.Text, IsCorrect = c.IsCorrect });

        q.DedupeHash = DedupeHashService.Compute(q);

        await _repo.AddAsync(q);
        return Ok(q.Id);
    }

    // GET api/v1/questions?section=...&difficulty=...
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuestionDto>>> List([FromQuery] Section? section, [FromQuery] int? difficulty)
    {
        var items = await _repo.ListAsync(section, difficulty);

        var dtos = items.Select(q => new QuestionDto(
            q.Id,
            q.Section,
            q.Stem,
            q.Choices.Select(c => new QuestionChoiceDto(c.Label, c.Text, c.IsCorrect)).ToList(),
            q.Difficulty,
            q.Tags.ToList()
        )).ToList();

        return Ok(dtos);
    }
}
