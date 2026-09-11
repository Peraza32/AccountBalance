using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CardAPI.Domain.Entities
{
    public partial class CardDbContext : DbContext
    {
        public CardDbContext()
        {
        }

        public CardDbContext(DbContextOptions<CardDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CardStatus> CardStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=BD_TARJETA;User Id=project;Password=Majoras89;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("CARDS");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AvCredit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AV_CREDIT");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("CARD_NUMBER");

                entity.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.CreditLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CREDIT_LIMIT");

                entity.Property(e => e.IdCardStatus).HasColumnName("ID_CARD_STATUS");

                entity.Property(e => e.IdClient)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ID_CLIENT");

                entity.Property(e => e.Interest)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("INTEREST");

                entity.Property(e => e.LastdCard)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("LASTD_CARD");

                entity.Property(e => e.MinInterest)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("MIN_INTEREST");

                entity.Property(e => e.TotalCredit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TOTAL_CREDIT");

                entity.HasOne(d => d.IdCardStatusNavigation)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.IdCardStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CARDS_STATUS");
            });

            modelBuilder.Entity<CardStatus>(entity =>
            {
                entity.ToTable("CARD_STATUS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("STATUS_NAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
