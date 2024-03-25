using System;
using System.Collections.Generic;
using FixIt.Models;
using Microsoft.EntityFrameworkCore;

namespace FixIt;

public partial class PhoneSparePartsContext : DbContext
{
    public PhoneSparePartsContext()
    {
    }

    public PhoneSparePartsContext(DbContextOptions<PhoneSparePartsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Catagory> Catagories { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<SparePart> SpareParts { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=QUEEN; Database=Phone_Spare_Parts; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BId).HasName("PK__Brands__4B26EFE62DB619A0");

            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.BName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("B_Name");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Photo).HasColumnType("image");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__D6AB475945DA5CF7");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("Cart_Id");
            entity.Property(e => e.SId).HasColumnName("S_Id");
            entity.Property(e => e.UId).HasColumnName("U_Id");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__Cart__S_Id__2DE6D218");

            entity.HasOne(d => d.UIdNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UId)
                .HasConstraintName("FK__Cart__U_Id__2CF2ADDF");
        });

        modelBuilder.Entity<Catagory>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK__Catagori__A9FDEC1242B3CA69");

            entity.Property(e => e.CId).HasColumnName("C_ID");
            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.Photo).HasColumnType("image");

            entity.HasOne(d => d.BIdNavigation).WithMany(p => p.Catagories)
                .HasForeignKey(d => d.BId)
                .HasConstraintName("FK__Catagories__B_ID__4BAC3F29");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OitemId).HasName("PK__OrderIte__667D51E13456AD8A");

            entity.ToTable("OrderItem");

            entity.Property(e => e.OitemId).HasColumnName("OItem_Id");
            entity.Property(e => e.Oid).HasColumnName("OId");
            entity.Property(e => e.SId).HasColumnName("S_Id");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__OrderItem__OId__3493CFA7");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__OrderItem__S_Id__3587F3E0");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__payments__CB394B194A0AC658");

            entity.ToTable("payments");

            entity.Property(e => e.Oid).HasColumnName("OId");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__payments__UserId__31B762FC");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("PK__Reviews__3214EC078FB5264E");

            entity.Property(e => e.RId).HasColumnName("R_ID");
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SId).HasColumnName("S_ID");
            entity.Property(e => e.UId).HasColumnName("U_ID");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Products");

            entity.HasOne(d => d.UIdNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Users");
        });

        modelBuilder.Entity<SparePart>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__SPARE_PA__A3DFF16DAF32E7DA");

            entity.ToTable("SPARE_PARTS");

            entity.Property(e => e.SId).HasColumnName("S_ID");
            entity.Property(e => e.CId).HasColumnName("C_ID");
            entity.Property(e => e.Describtion)
                .HasColumnType("text")
                .HasColumnName("describtion");
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.SName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("S_Name");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.SpareParts)
                .HasForeignKey(d => d.CId)
                .HasConstraintName("FK__SPARE_PART__C_ID__4E88ABD4");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("PK__Users__5A2040DB6E9AAF88");

            entity.Property(e => e.UId).HasColumnName("U_ID");
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("E_mail");
            entity.Property(e => e.FName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F_Name");
            entity.Property(e => e.LName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("L_Name");
            entity.Property(e => e.Pass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Photo).HasColumnType("image");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
