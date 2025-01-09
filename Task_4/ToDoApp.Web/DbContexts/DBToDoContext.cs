using ToDo.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDo.Web.Models;

namespace ToDo.Web.DbContexts;

public partial class DBToDoContext : DbContext
{
    public DBToDoContext()
    {
    }

    public DBToDoContext(DbContextOptions<DBToDoContext> options) : base(options) { }
    public DbSet<UserProfile> UserProfile { get; set; }
    public DbSet<ToDoItem> Tasks { get; set; }
    public virtual DbSet<Employee> People { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=.;Database=ToDo;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>(entity =>
        {
            //entity.HasNoKey();

            entity.ToTable("task");

            //entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Name)
               .HasMaxLength(10)
               .HasColumnName("name")
               .IsFixedLength();


            entity.Property(e => e.Priority)
                .HasMaxLength(10)
                .HasColumnName("priority")
                .IsFixedLength()
                .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd()));

            entity.Property(e => e.DueDate)
                .HasMaxLength(10)
                .HasColumnName("duedate")
                .IsFixedLength()
                .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd()));

            entity.Property(e => e.IsCompleted)
                .HasMaxLength(10)
                .HasColumnName("is_completed")
                .IsFixedLength();

            entity.Property(e => e.EmployeeID)
                .HasMaxLength(10)
                .HasColumnName("employee_id")
                .IsFixedLength();


        });

        modelBuilder.Entity<Employee>(entity =>
        {
            //entity.HasNoKey();

            entity.ToTable("employee");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name")
                .IsFixedLength();

            entity.Property(e => e.Age)
                .HasMaxLength(50)
                .HasColumnName("age")
                .IsFixedLength();

            entity.Property(e => e.Speciality)
                .ValueGeneratedOnAdd()
                .HasColumnName("speciality");

            entity.Property(e => e.EmployeeDate)
                .HasMaxLength(20)
                .HasColumnName("employee_date")
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}