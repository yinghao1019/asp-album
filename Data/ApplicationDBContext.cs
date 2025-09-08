
using Microsoft.EntityFrameworkCore;
using asp_album.Models.Entity;

namespace asp_album.Data
{
    public partial class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public virtual DbSet<AlbumEntity> Albums { get; set; } = null!;
        public virtual DbSet<AlbumCategoryEntity> AlbumCategories { get; set; } = null!;
        public virtual DbSet<MemberEntity> Members { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumEntity>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("album");

                entity.Property(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.MemberId).HasColumnName("member_id");
                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ReleaseTime)
                    .HasColumnType("datetime")
                    .HasColumnName("release_time");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<AlbumCategoryEntity>(entity =>
            {
                entity.HasKey(e => e.Id)
                     .HasName("PRIMARY");

                entity.ToTable("album_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<MemberEntity>(entity =>
            {
                entity.HasKey(e => e.Id)
          .HasName("PRIMARY");

                entity.ToTable("member");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Uid)
                    .HasMaxLength(50)
                    .HasColumnName("uid");

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .HasColumnName("role");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}