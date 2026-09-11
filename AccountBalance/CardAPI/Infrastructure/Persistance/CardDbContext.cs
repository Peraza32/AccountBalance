using System;
using System.Collections.Generic;
using CardAPI.Domain.Entities.DTO;
using CardAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CardAPI.Infrastructure.Persistance
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
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Doctype> Doctypes { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MovementsTc> MovementsTcs { get; set; }
        public virtual DbSet<PaymentsTc> PaymentsTcs { get; set; }
        public virtual DbSet<TransactionState> TransactionStates { get; set; }

        public DbSet<UserWithCardsDTO> UserWithCards { get; set; } // For the Stored Procedure result mapping
        public DbSet<NewPurchaseResultDTO> PurchaseResults { get; set; } // PROCESS_PURCHASE
        public DbSet<AccountBalanceResponseDTO> AccountBalances { get; set; } // GET_ACCOUNT_BALANCE
        
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
                    .HasColumnName("CARD_NUMBER")
                    .UseCollation("Latin1_General_BIN2");

                entity.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.CreditLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CREDIT_LIMIT");

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

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CLIENTE_CC");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.DocNumber)
                    .HasName("PK__CLIENT__9532861F8B0F21F2");

                entity.ToTable("CLIENT");

                entity.Property(e => e.DocNumber)
                    .HasMaxLength(20)
                    .HasColumnName("DOC_NUMBER");

                entity.Property(e => e.Cellphone)
                    .HasMaxLength(100)
                    .HasColumnName("CELLPHONE");

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("CLIENT_NAME");

                entity.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(100)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.IdDoctype).HasColumnName("ID_DOCTYPE");

                entity.HasOne(d => d.IdDoctypeNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.IdDoctype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CLIENT_DOCTYPE");
            });

            modelBuilder.Entity<Doctype>(entity =>
            {
                entity.ToTable("DOCTYPE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Docname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DOCNAME");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("LOGS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LogDescription)
                    .HasMaxLength(600)
                    .HasColumnName("LOG_DESCRIPTION");

                entity.Property(e => e.LogDt).HasColumnName("LOG_DT");

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("ORIGIN");
            });

            modelBuilder.Entity<MovementsTc>(entity =>
            {
                entity.ToTable("MOVEMENTS_TC");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.IdCard).HasColumnName("ID_CARD");

                entity.Property(e => e.IdState).HasColumnName("ID_STATE");

                entity.Property(e => e.MvDate).HasColumnName("MV_DATE");

                entity.Property(e => e.MvDescription)
                    .HasMaxLength(300)
                    .HasColumnName("MV_DESCRIPTION");

                entity.HasOne(d => d.IdCardNavigation)
                    .WithMany(p => p.MovementsTcs)
                    .HasForeignKey(d => d.IdCard)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CARD_MV");

                entity.HasOne(d => d.IdStateNavigation)
                    .WithMany(p => p.MovementsTcs)
                    .HasForeignKey(d => d.IdState)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_STATE_MV");
            });

            modelBuilder.Entity<PaymentsTc>(entity =>
            {
                entity.ToTable("PAYMENTS_TC");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.IdCard).HasColumnName("ID_CARD");

                entity.Property(e => e.IdState).HasColumnName("ID_STATE");

                entity.Property(e => e.MvDate).HasColumnName("MV_DATE");

                entity.Property(e => e.MvDescription)
                    .HasMaxLength(300)
                    .HasColumnName("MV_DESCRIPTION");

                entity.HasOne(d => d.IdCardNavigation)
                    .WithMany(p => p.PaymentsTcs)
                    .HasForeignKey(d => d.IdCard)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CARD_PY");

                entity.HasOne(d => d.IdStateNavigation)
                    .WithMany(p => p.PaymentsTcs)
                    .HasForeignKey(d => d.IdState)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_STATE_PY");
            });

            modelBuilder.Entity<TransactionState>(entity =>
            {
                entity.ToTable("TRANSACTION_STATE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Tstate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TSTATE");
            });

            // Registering DTO for Stored procedure: GET_CLIENTWITHCARDS 

            modelBuilder.Entity<UserWithCardsDTO>()
                .HasNoKey()
                .ToView(null); // This indicates that this entity is not mapped to a table or view

            modelBuilder.Entity<UserWithCardsDTO>()
                .Property(e => e.clientId) 
                .HasColumnName("DOC_NUMBER");

            modelBuilder.Entity<UserWithCardsDTO>()
                .Property(e => e.clientName)
                .HasColumnName("CLIENT_NAME");

            modelBuilder.Entity<UserWithCardsDTO>()
                .Property(e => e.cardNumber)
                .HasColumnName("CARD_NUMBER");

            modelBuilder.Entity<UserWithCardsDTO>()
                .Property(e => e.idCard)
                .HasColumnName("CARD_ID");

            

            //STORED PROCEDURE: PROCESS_PURCHASE
            modelBuilder.Entity<NewPurchaseResultDTO>()
                .HasNoKey()
                .ToView(null);

            //STORED PROCEDURE: GET_ACCOUNT_BALANCE
            modelBuilder.Entity<AccountBalanceResponseDTO>()
     .HasNoKey()
     .ToView(null);

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.cardHolderName)
                .HasColumnName("CARD_HOLDER");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.cardNumber)
                .HasColumnName("CARD_NUMBER");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.currentCredit)
                .HasColumnName("TOTAL_CREDIT");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.availableCredit)
                .HasColumnName("AVAILABLE_CREDIT");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.creditLimit)
                .HasColumnName("CREDIT_LIMIT");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.totalActualMonthPurchases)
                .HasColumnName("TOTAL_PURCHASES_CURRENT_MONTH");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.previousMonthPurchases)
                .HasColumnName("TOTAL_PURCHASES_PREVIOUS_MONTH");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.bonificationInterest)
                .HasColumnName("BONIFI_INTEREST");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.minimumPayment)
                .HasColumnName("MIN_PAYMENT");

            modelBuilder.Entity<AccountBalanceResponseDTO>()
                .Property(e => e.totalWithInterest)
                .HasColumnName("TOTAL_WITH_INTEREST");

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
