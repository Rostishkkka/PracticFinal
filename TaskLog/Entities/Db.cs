using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskLog.Entities
{
    public partial class Db : DbContext
    {
        public virtual DbSet<Components> Components { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public Db()
        {
        }

        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                //optionsBuilder.UseSqlServer(ConnectionStringProvider.GetDataSource());
                optionsBuilder.UseSqlServer("Data Source=DEREKOVSKY\\SQLEXPRESS;" +
                    "Initial Catalog=TaskLog;" +
                    "Integrated Security=True;" +
                    "Trust Server Certificate=True;Command Timeout=300;" +
                    "MultipleActiveResultSets=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Components>(entity =>
            {
                entity.HasKey(e => e.CompId);

                entity.Property(e => e.CompId).HasColumnName("COMP_ID");

                entity.Property(e => e.CompOemId)
                    .HasMaxLength(50)
                    .HasColumnName("COMP_OEM_ID");

                entity.Property(e => e.CompOemName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("COMP_OEM_NAME");

                entity.Property(e => e.CompOemVer)
                    .HasMaxLength(50)
                    .HasColumnName("COMP_OEM_VER");

                entity.Property(e => e.SwVer)
                    .HasMaxLength(50)
                    .HasColumnName("SW_VER");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.EventId).HasColumnName("EVENT_ID");

                entity.Property(e => e.EventTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("EVENT_TIMESTAMP");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("EVENT_TYPE");

                entity.Property(e => e.TaskId).HasColumnName("TASK_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.EventLog)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventLog_Tasks");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventLog_Users");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasKey(e => e.TaskId);

                entity.Property(e => e.TaskId).HasColumnName("TASK_ID");

                entity.Property(e => e.Comments)
                    .HasMaxLength(512)
                    .HasColumnName("COMMENTS");

                entity.Property(e => e.CompId).HasColumnName("COMP_ID");

                entity.Property(e => e.CompSn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("COMP_SN");

                entity.Property(e => e.TaskDescr)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("TASK_DESCR");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Comp)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.CompId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Components");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.HashedPass)
                    .IsRequired()
                    .HasMaxLength(68)
                    .HasColumnName("HASHED_PASS");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("USER_EMAIL");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("USER_NAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
