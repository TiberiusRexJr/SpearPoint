using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpearPoint.Domain.Entities;

namespace SpearPoint.Application.Abstractions
{
    public interface IQuestionRepository
    {
        Task<Question> AddAsync(Question entity, CancellationToken ct = default);
        Task<Question?> GetAsync(long id, CancellationToken ct = default);
        Task<Question?> FindByDedupeHashAsync(string dedupeHash, CancellationToken ct = default);
        Task<IReadOnlyList<Question>> ListAsync(Section? section = null, int? difficulty = null, CancellationToken ct = default);
    }
}
