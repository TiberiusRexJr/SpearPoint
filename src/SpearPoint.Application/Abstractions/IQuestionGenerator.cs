using SpearPoint.Domain.Entities;
namespace SpearPoint.Application.Abstractions;

public interface IQuestionGenerator
{
    Task<IReadOnlyList<Question>> GenerateAsync(Section section, int difficulty, int count, CancellationToken ct = default);
}
