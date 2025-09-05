using System;
using System.Collections.Generic;
using SpearPoint.Domain.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearPoint.Domain.Entities
{
    public class Question
    {
        public long Id { get; set; }
        // e.g., "Arithmetic Reasoning", "Electronics"
        public string Section { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.MCQ;
        // Stem and optional rationale/explanation
        public string Stem { get; set; } = string.Empty;
        public string? AnswerExplanation { get; set; }
        // 1..5 (or whatever scale we adopt)
        public int Difficulty { get; set; } = 1;
        public QuestionSource Source { get; set; } = QuestionSource.Human;
        public QuestionStatus Status { get; set; } = QuestionStatus.Active;
        // Stored as JSON text via ValueConverter
        public List<string> Tags { get; set; } = new();
        // Normalized hash of (stem + canonicalized choices)
        public string? DedupeHash { get; set; }
        // Link variants to a canonical form
        public long? CanonicalId { get; set; }
        public Question? Canonical { get; set; }
        // Audit
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        // Concurrency
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public ICollection<QuestionChoice> Choices { get; set; } = new List<QuestionChoice>();
        public QuestionGenerationMeta? GenerationMeta { get; set; }
    }
}
