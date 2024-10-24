using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfApp1.DataModel;

namespace WpfApp1.Model
{
    public partial class DBPersonContext : DbContext
    {
        public DBPersonContext()
        {
        }

        public DBPersonContext(DbContextOptions<DBPersonContext> options)
            : base(options)
        {
        }
                
        public virtual DbSet<Person> People { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\;Database=DBPersons;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Person>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("Person");

				entity.Property(e => e.PersonId)
	                .ValueGeneratedOnAdd()
	                .HasColumnName("person_id");

				entity.Property(e => e.Date)
					.HasMaxLength(20)
					.HasColumnName("date")
					.IsFixedLength();

				entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name")
                    .IsFixedLength()
                    .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd()));

				entity.Property(e => e.Surname)
					.HasMaxLength(50)
					.HasColumnName("surname")
					.IsFixedLength()
                    .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd()));

				entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name")
                    .IsFixedLength()
                    .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd())); ;

				entity.Property(e => e.City)
					.HasMaxLength(50)
					.HasColumnName("city")
					.IsFixedLength()
                    .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd())); ;

				entity.Property(e => e.Country)
					.HasMaxLength(50)
					.HasColumnName("country")
					.IsFixedLength()
                    .HasConversion(new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd())); ;

			});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
