using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;
using FinanceManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceManager.Infrastructure.Persistence;

public class FinanceManagerDbContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;

	public DbSet<Category> Categories { get; set; } = null!;

	public DbSet<Transaction> Transactions { get; set; } = null!;

	public DbSet<Budget> Budgets { get; set; } = null!;

	public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

	public FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		ConfigureUserEntity(modelBuilder);
		ConfigureCategoryEntity(modelBuilder);
		ConfigureTransactionEntity(modelBuilder);
		ConfigureBudgetEntity(modelBuilder);
		ConfigureRefreshTokenEntity(modelBuilder);

		ApplySoftDeleteFilter<User>(modelBuilder);
		ApplySoftDeleteFilter<Category>(modelBuilder);
		ApplySoftDeleteFilter<Transaction>(modelBuilder);
		ApplySoftDeleteFilter<Budget>(modelBuilder);
	}

	private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
		where TEntity : class, ISoftDeletable
	{
		modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
		modelBuilder.Entity<TEntity>().Property(e => e.IsDeleted).HasDefaultValue(false);
	}

	private static void ConfigureUserEntity(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId);

			entity.Property(e => e.UserId)
				.ValueGeneratedOnAdd();

			entity.Property(e => e.Username)
				.IsRequired()
				.HasMaxLength(100);

			entity.Property(e => e.PasswordHash)
				.IsRequired()
				.HasMaxLength(255);

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("GETUTCDATE()");

			entity.HasIndex(e => e.Username)
				.IsUnique();

			entity.HasMany(e => e.Categories)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(e => e.Transactions)
				.WithOne(t => t.User)
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(e => e.Budgets)
				.WithOne(b => b.User)
				.HasForeignKey(b => b.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		});
	}

	private static void ConfigureCategoryEntity(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>(entity =>
		{
			entity.HasKey(e => e.CategoryId);

			entity.Property(e => e.CategoryId)
				.ValueGeneratedOnAdd();

			entity.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(100);

			entity.Property(e => e.Type)
				.HasConversion(new EnumToStringConverter<TransactionType>());

			entity.Property(e => e.BudgetAmount)
				.HasPrecision(18, 2);

			entity.HasIndex(new[] { nameof(Category.UserId) });

			entity.HasMany(e => e.Transactions)
				.WithOne(t => t.Category)
				.HasForeignKey(t => t.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasMany(e => e.Budgets)
				.WithOne(b => b.Category)
				.HasForeignKey(b => b.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		});
	}

	private static void ConfigureTransactionEntity(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Transaction>(entity =>
		{
			entity.HasKey(e => e.TransactionId);

			entity.Property(e => e.TransactionId)
				.ValueGeneratedOnAdd();

			entity.Property(e => e.Amount)
				.HasPrecision(18, 2);

			entity.Property(e => e.TransactionDate)
				.IsRequired();

			entity.Property(e => e.Description)
				.HasMaxLength(500);

			entity.Property(e => e.Type)
				.HasConversion(new EnumToStringConverter<TransactionType>());

			entity.HasIndex(new[] { nameof(Transaction.UserId) });

			entity.HasIndex(new[] { nameof(Transaction.TransactionDate) });

			entity.HasIndex(new[] { nameof(Transaction.CategoryId) });
		});
	}

	private static void ConfigureBudgetEntity(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Budget>(entity =>
		{
			entity.HasKey(e => e.BudgetId);

			entity.Property(e => e.BudgetId)
				.ValueGeneratedOnAdd();

			entity.Property(e => e.MonthlyLimit)
				.HasPrecision(18, 2)
				.IsRequired();

			entity.Property(e => e.Year)
				.IsRequired();

			entity.Property(e => e.Month)
				.IsRequired();

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("GETUTCDATE()");

			entity.HasIndex(new[] { nameof(Budget.UserId), nameof(Budget.CategoryId), nameof(Budget.Year), nameof(Budget.Month) })
				.IsUnique();

			entity.HasIndex(new[] { nameof(Budget.UserId) });
		});
	}

	private static void ConfigureRefreshTokenEntity(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RefreshToken>(entity =>
		{
			entity.HasKey(e => e.TokenId);

			entity.Property(e => e.TokenId).ValueGeneratedOnAdd();

			entity.Property(e => e.TokenHash)
				.IsRequired()
				.HasMaxLength(255);

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("GETUTCDATE()");

			entity.HasIndex(e => e.TokenHash);

			entity.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
