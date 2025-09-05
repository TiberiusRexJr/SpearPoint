using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearPoint.Application.DTOs
{
    public record QuestionDto(
    long Id,
    string Section,
    string Type,
    string Stem,
    int Difficulty,
    string Source,
    string Status,
    IReadOnlyList<string> Tags,
    string? AnswerExplanation,
    IEnumerable<(string Label, string Text, bool IsCorrect)> Choices);
}
