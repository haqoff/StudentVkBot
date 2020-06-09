using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ScheduleTemplateModel;

namespace StudentBotCore.Model
{
    public class StuDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventOrganizer> EventOrganizers { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<EveryDayRegularEventNotification> EveryDayRegularEventNotifications { get; set; }
        public DbSet<EveryDayRegularSchedule> EveryDayRegularSchedules { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<VkChat> VkChats { get; set; }
        public DbSet<VkChatAdmin> VkChatAdmins { get; set; }
        public DbSet<VkUser> VkUsers { get; set; }

        public StuDbContext(string connectionString)
        {
            Console.WriteLine("DbContext created");

            _connectionString = connectionString;
        }

        public StuDbContext(DbContextOptions<StuDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseMySql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(o =>
            {
                o.HasKey(category => category.Id);
                o.HasOne(category => category.Chat)
                    .WithMany(chat => chat.EventCategories)
                    .HasForeignKey(category => category.ChatId)
                    .OnDelete(DeleteBehavior.ClientCascade);
                o.Property(category => category.Name).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<Event>(o =>
            {
                o.HasKey(ev => ev.Id);
                o.Property(ev => ev.Description).HasMaxLength(300).IsRequired(false);
                o.Property(ev => ev.Location).HasMaxLength(64).IsRequired(false);
                o.HasOne(ev => ev.Category)
                    .WithMany(category => category.Events)
                    .HasForeignKey(ev => ev.CategoryId)
                    .OnDelete(DeleteBehavior.ClientCascade);
                o.HasOne(ev => ev.Tag)
                    .WithMany(tag => tag.Events)
                    .HasForeignKey(ev => ev.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<EventOrganizer>(o =>
            {
                o.HasKey(eo => new {eo.EventId, eo.PersonId});
                o.HasOne(eo => eo.Event)
                    .WithMany(ev => ev.Organizers)
                    .HasForeignKey(eo => eo.EventId);
                o.HasOne(eo => eo.Person)
                    .WithMany(person => person.OrganizerInEvents)
                    .HasForeignKey(eo => eo.PersonId);
            });
            modelBuilder.Entity<EventParticipant>(o =>
            {
                o.HasKey(ep => new {ep.EventId, ep.PersonId});
                o.HasOne(ep => ep.Event)
                    .WithMany(ev => ev.Participants)
                    .HasForeignKey(ep => ep.EventId);
                o.HasOne(ep => ep.Person)
                    .WithMany(person => person.ParticipantInEvents)
                    .HasForeignKey(ep => ep.PersonId);
            });
            modelBuilder.Entity<EveryDayRegularEventNotification>(o =>
            {
                o.HasKey(n => new {n.ChatId, n.StartUtcTime});
                o.HasOne(n => n.Chat)
                    .WithMany(chat => chat.Notifications)
                    .HasForeignKey(n => n.ChatId);
            });
            modelBuilder.Entity<EveryDayRegularSchedule>(o =>
            {
                o.HasKey(n => new {n.ChatId, n.StartUtcTime, n.Duration});
                o.HasOne(n => n.Chat)
                    .WithMany(chat => chat.ScheduleOrders)
                    .HasForeignKey(n => n.ChatId);
            });
            modelBuilder.Entity<Person>(o =>
            {
                o.HasKey(p => p.Id);
                o.Property(p => p.FirstName).IsRequired().HasMaxLength(32);
                o.Property(p => p.LastName).IsRequired().HasMaxLength(32);
                o.Property(p => p.Patronymic).IsRequired(false).HasMaxLength(32);
                o.Property(p => p.Email).IsRequired(false).HasMaxLength(320);
            });
            modelBuilder.Entity<Tag>(o =>
            {
                o.HasKey(t => t.Id);
                o.Property(t => t.Name).IsRequired().HasMaxLength(32);

                // TODO:
                var defaultData = EnumExtension.GetEnumValuesIdAndName<TagTemplate>()
                    .Select(i => new Tag() {Id = (ulong) i.id, Name = i.description});
                o.HasData(defaultData);
            });
            modelBuilder.Entity<VkChat>(o => { o.HasKey(c => c.Id); });
            modelBuilder.Entity<VkChatAdmin>(o =>
            {
                o.HasKey(ca => new {ca.ChatId, ca.VkUserId});
                o.HasOne(ca => ca.Chat)
                    .WithMany(c => c.Admins)
                    .HasForeignKey(ca => ca.ChatId);
                o.HasOne(ca => ca.VkUser)
                    .WithMany(u => u.AdminInChats)
                    .HasForeignKey(ca => ca.VkUserId);
            });
            modelBuilder.Entity<VkUser>(o =>
            {
                o.HasKey(u => u.Id);
                o.HasOne(u => u.Person)
                    .WithOne(p => p.VkUser)
                    .HasForeignKey<Person>(p => p.VkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        public override void Dispose()
        {
            Console.WriteLine("DbContext disposed");
            base.Dispose();
        }
    }
}