using Microsoft.EntityFrameworkCore;
using SpearPoint.Application.Abstractions;
using SpearPoint.Domain.Entities;
using SpearPoint.Infrastructure.Persistence;

namespace SpearPoint.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _db;
    public QuestionRepository(AppDbContext db) => _db = db;

    public async Task<Question> AddAsync(Question entity, CancellationToken ct = default)
    {
        _db.Questions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public Task<Question?> GetAsync(long id, CancellationToken ct = default) =>
    _db.Questions.Include(q => q.Choices).FirstOrDefaultAsync(q => q.Id == id, ct);

    public Task<Question?> FindByDedupeHashAsync(string dedupeHash, CancellationToken ct = default) =>
    _db.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.DedupeHash == dedupeHash, ct);

    public async Task<IReadOnlyList<Question>> ListAsync(string? section = null, int? difficulty = null, CancellationToken ct = default)
    {
        var q = _db.Questions.AsNoTracking().Include(x => x.Choices).AsQueryable();
        if (!string.IsNullOrWhiteSpace(section)) q = q.Where(x => x.Section == section);
        if (difficulty.HasValue) q = q.Where(x => x.Difficulty == difficulty);
        return await q.OrderBy(x => x.Section).ThenBy(x => x.Difficulty).ToListAsync(ct);
    }




}
