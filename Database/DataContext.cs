using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionString"));
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Banneduser> Bannedusers { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Messages> Messages { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<State> States { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Banneduser>().ToTable("Bannedusers");
        modelBuilder.Entity<Like>().ToTable("Likes");
        modelBuilder.Entity<Messages>().ToTable("Messages");
        modelBuilder.Entity<Photo>().ToTable("Photos");
        modelBuilder.Entity<State>().ToTable("States");
    }





}