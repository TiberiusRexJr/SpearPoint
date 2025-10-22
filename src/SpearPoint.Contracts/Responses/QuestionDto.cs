using SpearPoint.Domain.Entities;
namespace SpearPoint.Contracts.Responses;

public record QuestionDto(
    long Id,
    Section Section,                       // keep wire format simple (string)
    string Stem,
    IReadOnlyList<QuestionChoiceDto> Choices,
    int Difficulty,                    // string for wire format; map to enum in Domain
    IReadOnlyList<string> Tags);

public record QuestionChoiceDto(
    string Label,                         // e.g., "A", "B", "C", "D"
    string Text,
    bool IsCorrect);
