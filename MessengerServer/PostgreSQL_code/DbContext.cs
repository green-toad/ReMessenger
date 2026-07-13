using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerServer
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDevices> UserDevices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDevices>()
            .HasKey(ud => new { ud.UserSUID, ud.deviceID, ud.sessionID });

            modelBuilder.Entity<Participant>()
            .HasKey(p => new { p.UserSUID, p.ChatSUID });

            modelBuilder.Entity<Participant>()
            .HasOne(p => p.ChatLink)
            .WithMany()
            .HasForeignKey(p => p.ChatSUID);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Participants)
                .WithOne(p => p.ChatLink)
                .HasForeignKey(p => p.ChatSUID);
        }
    }
}
