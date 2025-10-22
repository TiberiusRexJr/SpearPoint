using SpearPoint.Contracts.Responses;

namespace SpearPoint.Contracts.Requests;

public record CreateTestSessionRequest(
    IReadOnlyList<QuestionDto> Questions);
