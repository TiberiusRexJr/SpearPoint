using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpearPoint.Domain.Entities;
using SpearPoint.Domain.Enums;
using SpearPoint.Infrastructure.Persistence.Converters;

namespace SpearPoint.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> b)
    {
        b.ToTable("questions");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();
        b.Property(x => x.Section).HasMaxLength(64).IsRequired();
        b.Property(x => x.Stem).IsRequired();
        b.Property(x => x.AnswerExplanation);
        b.Property(x => x.Difficulty).IsRequired();

        b.Property(x => x.Type)
        .HasConversion<string>()
        .HasMaxLength(16)
        .HasDefaultValue(QuestionType.MCQ)
        .IsRequired();

        b.Property(x => x.Source)
        .HasConversion<string>()
        .HasMaxLength(16)
        .HasDefaultValue(QuestionSource.Human)
        .IsRequired();

        b.Property(x => x.Status)
        .HasConversion<string>()
        .HasMaxLength(16)
        .HasDefaultValue(QuestionStatus.Active)
        .IsRequired();

        // Tags as JSON text + basic integrity guard
        b.Property(x => x.Tags)
        .HasConversion(new StringListToJsonConverter())
        .HasColumnType("nvarchar(max)")
        .HasDefaultValue("[]");
        b.ToTable(t => t.HasCheckConstraint("CK_questions_tags_json", "ISJSON([Tags]) = 1"));

        // Dedupe hash (hex) — unique if present
        b.Property(x => x.DedupeHash)
        .HasColumnName("dedupe_hash")
        .HasMaxLength(64)
        .IsUnicode(false);
        b.HasIndex(x => x.DedupeHash)
        .HasDatabaseName("ux_q_dedupe")
        .IsUnique()
        .HasFilter("[dedupe_hash] IS NOT NULL");

        // Canonical linkage
        b.Property(x => x.CanonicalId).HasColumnName("canonical_id");
        b.HasOne(x => x.Canonical)
        .WithMany()
        .HasForeignKey(x => x.CanonicalId)
        .OnDelete(DeleteBehavior.SetNull);

        // Audit
        b.Property(x => x.CreatedAtUtc).HasColumnName("created_at").HasDefaultValueSql("SYSUTCDATETIME()").ValueGeneratedOnAdd();
        b.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at").HasDefaultValueSql("SYSUTCDATETIME()");

        // Concurrency
        b.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();

        // Index for common query pattern
        b.HasIndex(x => new { x.Section, x.Difficulty })
        .HasDatabaseName("ix_q_section_diff");

        // Relationships
        b.HasMany(q => q.Choices)
        .WithOne(c => c.Question)
        .HasForeignKey(c => c.QuestionId)
        .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(q => q.GenerationMeta)
        .WithOne(g => g.Question)
        .HasForeignKey<QuestionGenerationMeta>(g => g.QuestionId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
