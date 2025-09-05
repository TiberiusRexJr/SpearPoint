using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearPoint.Domain.Entities
{
    public class QuestionGenerationMeta
    {
        public long QuestionId { get; set; }
        public Question Question { get; set; } = default!;
        public string Model { get; set; } = string.Empty; // e.g., "gpt-4o"
        public string? ResponseId { get; set; }
        public string Prompt { get; set; } = string.Empty;
        public string? RawResponseJson { get; set; }
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
        public decimal? CostUsd { get; set; }
        public DateTime StartedAtUtc { get; set; }
        public DateTime FinishedAtUtc { get; set; }
        public int DurationMs { get; set; }
    }
}
