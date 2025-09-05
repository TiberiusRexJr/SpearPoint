using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpearPoint.Domain.Entities;

namespace SpearPoint.Infrastructure.Persistence.Configurations;

public class QuestionGenerationMetaConfiguration : IEntityTypeConfiguration<QuestionGenerationMeta>
{
    public void Configure(EntityTypeBuilder<QuestionGenerationMeta> b)
    {
        b.ToTable("question_generation_meta");
        b.HasKey(x => x.QuestionId);
        b.Property(x => x.Model).HasMaxLength(64).IsRequired();
        b.Property(x => x.ResponseId).HasMaxLength(128);
        b.Property(x => x.Prompt).HasColumnType("nvarchar(max)");
        b.Property(x => x.RawResponseJson).HasColumnType("nvarchar(max)");
        b.Property(x => x.PromptTokens);
        b.Property(x => x.CompletionTokens);
        b.Property(x => x.TotalTokens);
        b.Property(x => x.CostUsd).HasColumnType("decimal(18,4)");
        b.Property(x => x.StartedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
        b.Property(x => x.FinishedAtUtc);
        b.Property(x => x.DurationMs);
    }
}
