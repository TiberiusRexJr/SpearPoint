namespace SpearPoint.Contracts.Requests;

public record SubmitAnswerRequest(
    long QuestionId,
    int SelectedIndex);
