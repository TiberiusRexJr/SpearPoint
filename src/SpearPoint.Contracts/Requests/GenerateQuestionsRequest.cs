using SpearPoint.Domain.Entities;
namespace SpearPoint.Contracts.Requests;

public record GenerateQuestionsRequest(
    Section Section,
    int Difficulty,
    int Count);
