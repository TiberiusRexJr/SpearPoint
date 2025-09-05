using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpearPoint.Domain.Entities;

namespace SpearPoint.Infrastructure.Persistence.Configurations;

public class QuestionChoiceConfiguration : IEntityTypeConfiguration<QuestionChoice>
{
    public void Configure(EntityTypeBuilder<QuestionChoice> b)
    {
        b.ToTable("question_choices");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();
        b.HasIndex(x => x.QuestionId).HasDatabaseName("ix_qc_qid");
        b.Property(x => x.Label).HasMaxLength(8).IsRequired();
        b.Property(x => x.Text).IsRequired();
        b.Property(x => x.IsCorrect).HasDefaultValue(false);
    }
}
