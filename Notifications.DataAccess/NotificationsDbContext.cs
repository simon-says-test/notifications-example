using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notifications.Common.Enums;
using Notifications.Common.Fields;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        { }

        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<TemplateEntity> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var eventBodyConverter = new CastingConverter<EventBody, string>();
            modelBuilder
                .Entity<TemplateEntity>()
                .Property(e => e.Body)
                .HasConversion(eventBodyConverter);
            modelBuilder
                .Entity<NotificationEntity>()
                .Property(e => e.Body)
                .HasConversion(eventBodyConverter);

            var eventTitleConverter = new CastingConverter<EventTitle, string>();
            modelBuilder
                .Entity<TemplateEntity>()
                .Property(e => e.Title)
                .HasConversion(eventTitleConverter);
            modelBuilder
                .Entity<NotificationEntity>()
                .Property(e => e.Title)
                .HasConversion(eventTitleConverter);

            modelBuilder.Entity<TemplateEntity>().HasData(
                new TemplateEntity(
                    Guid.NewGuid(),
                    EventType.AppointmentCancelled,
                    EventBody.Create("Hi {FirstName}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.").Value,
                    EventTitle.Create("Appointment Cancelled").Value));
        }
    }
}
