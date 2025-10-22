using SpearPoint.Application.Abstractions;
using SpearPoint.Domain.Entities;

namespace SpearPoint.Infrastructure.Services;

public class FakeQuestionGenerator : IQuestionGenerator
{
    public Task<IReadOnlyList<Question>> GenerateAsync(Section section, int difficulty, int count, CancellationToken ct = default)
    {
        var list = Enumerable.Range(1, count).Select(i => new Question
        {
            Section = section,
            Difficulty = difficulty,
            Stem = $"[{section}] Q{i}: 2 + 3 = ?",
            Choices = new List<QuestionChoice>
            {
                new QuestionChoice
                {
                    Label = "A",
                    Text = "Option Alpha",
                    IsCorrect = false
                },
                new QuestionChoice
                {
                    Label = "B",
                    Text = "Option Bravo",
                    IsCorrect = false
                },
                new QuestionChoice
                {
                    Label = "C",
                    Text = "Option Charlie",
                    IsCorrect = true
                },
                new QuestionChoice
                {
                    Label = "D",
                    Text = "Option Delta",
                    IsCorrect = false
                }
            },
            Tags = new() { "mock", "demo" }
        }).ToList();

        return Task.FromResult<IReadOnlyList<Question>>(list);
    }
}
