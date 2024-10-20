﻿using Microsoft.EntityFrameworkCore;
using Syncer.APIs.Models.Domain;

namespace Syncer.APIs.Persistence;

public class SyncerDbContext(DbContextOptions<SyncerDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public const string ConnectionStringName = "SvcDbContext";
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Emoji> Emojis { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePresentation(modelBuilder);

        modelBuilder.Entity<Emoji>().HasKey(x => x.Code);
        modelBuilder.Entity<Emoji>().Property(x => x.Code).ValueGeneratedNever().IsUnicode();
        modelBuilder.Entity<Emoji>().Property(x => x.SortName).HasMaxLength(100).IsUnicode();
    }

    private static void ConfigurePresentation(ModelBuilder modelBuilder)
    {
        var presentation = modelBuilder.Entity<Presentation>();

        presentation.HasKey(X => X.Id);
        presentation.Property(X => X.Id).ValueGeneratedNever().IsUnicode(false);
        presentation.Property(X => X.Title).IsRequired().HasMaxLength(300).IsUnicode();
        presentation.Property(X => X.Description).IsRequired().HasMaxLength(2000).IsUnicode();
        presentation.Property(X => X.Speaker).IsRequired().IsUnicode(false);
        presentation.Property(x => x.Status).IsRequired();

        presentation.OwnsMany(x => x.Joiners, joinerBuilder =>
        {
            joinerBuilder.ToJson();
        });

        presentation.OwnsMany(x => x.Milestones, milestoneBuilder =>
        {
            milestoneBuilder.HasKey(x => x.Id);
            milestoneBuilder.ToTable("Milestones");

            milestoneBuilder.Property(x => x.Status).IsRequired();

            milestoneBuilder.Property(x => x.PresentationId).IsRequired();

            milestoneBuilder.Property(x => x.Title).IsRequired().HasMaxLength(100).IsUnicode();

            milestoneBuilder.Property(x => x.Description).IsRequired().HasMaxLength(500).IsUnicode();

            milestoneBuilder.OwnsMany(x => x.Reactions, reationsBuilder =>
            {
                reationsBuilder.ToTable("MilestoneReactions");
            });

            milestoneBuilder.OwnsMany(x => x.Emojis, reationsBuilder =>
            {
                reationsBuilder.ToTable("MilestoneEmojis");
            });
        });
    }
}
