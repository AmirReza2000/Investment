using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Users.Entities;
using Investment.Infrastructure.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Investment.Infrastructure;

public class InvestmentDbContext : DbContext
{

    public InvestmentDbContext(DbContextOptions options)
            : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<ValidationToken> ValidationTokens { get; set; }

    public DbSet<Transaction> Transactions{ get; set; }

    public DbSet<UserBalance> UserBalances{ get; set; }

    public  DbSet<Contract> Contracts { get; set; }

    public DbSet<ContractMarketType> ContractMarketTypes { get; set; }

    public DbSet<UserContract> UserContracts  { get; set; }

    public DbSet<UserContractLog> UserContractLogs{ get; set; }

    public DbSet<UserContractProfit> UserContractProfits{ get; set; }
}
