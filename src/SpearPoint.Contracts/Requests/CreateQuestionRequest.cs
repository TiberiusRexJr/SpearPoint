using SpearPoint.Domain.Entities;
using SpearPoint.Domain.Enums;
namespace SpearPoint.Contracts.Requests;

public record CreateQuestionRequest(
    Section Section,                        // e.g., "ArithmeticReasoning"
    QuestionType Type,                           // e.g., "MultipleChoice"
    string Stem,                           // the question text
    string? AnswerExplanation,             // optional
    int Difficulty,                     // e.g., "Easy" | "Medium" | "Hard"
    QuestionSource Source,                         // e.g., "FakeGen" | "Curated"
    QuestionStatus Status,                         // e.g., "Active" | "Draft"
    IReadOnlyList<string>? Tags,           // optional
    IReadOnlyList<CreateQuestionChoiceRequest> Choices
);

public record CreateQuestionChoiceRequest(
    string Label,                          // "A", "B", "C", "D"
    string Text,
    bool IsCorrect
);
