namespace SikkerhedOgTest.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SikkerhedOgTest.Models;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cpr> Cprs { get; set; } = default!;
    public DbSet<TodoItem> TodoItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {


        // Configure relationships and constraints

    }
}