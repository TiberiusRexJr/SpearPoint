using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearPoint.Domain.Entities
{
    public class QuestionChoice
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; } = default!;
        // 'A','B','C','D'... (we keep string to allow >26 / multi-char)
        public string Label { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}
