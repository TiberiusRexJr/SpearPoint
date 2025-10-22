using Microsoft.EntityFrameworkCore;
using SpearPoint.Application.Abstractions;
using SpearPoint.Domain.Entities;
using SpearPoint.Infrastructure.Persistence;

namespace SpearPoint.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _context;

    public QuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Question> AddAsync(Question question, CancellationToken cancellationToken = default)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);
        return question;
    }

    public Task<Question?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return _context.Questions
            .Include(question => question.Choices)
            .FirstOrDefaultAsync(question => question.Id == id, cancellationToken);
    }

    public Task<Question?> FindByDedupeHashAsync(string dedupHash, CancellationToken cancellationToken = default)
    {
        return _context.Questions
            .AsNoTracking()
            .FirstOrDefaultAsync(question => question.DedupeHash == dedupHash, cancellationToken);
    }

    public async Task<IReadOnlyList<Question>> ListAsync(
            Section? section = null,
            int? difficulty = null,
            CancellationToken cancellationToken = default)
    {
        IQueryable<Question> query = _context.Questions
            .AsNoTracking()
            .Include(question => question.Choices)
            .AsQueryable();

        if (section.HasValue)
        {
            query = query.Where(question => question.Section == section);
        }

        if (difficulty.HasValue)
        {
            query = query.Where(question => question.Difficulty == difficulty);
        }

        return await query
            .OrderBy(question => question.Section)
            .ThenBy(question => question.Difficulty)
            .ToListAsync(cancellationToken);
    }
}




