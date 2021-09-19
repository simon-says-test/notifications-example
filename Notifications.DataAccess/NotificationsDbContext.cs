using System;
using Microsoft.EntityFrameworkCore;
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

            modelBuilder.Entity<TemplateEntity>().HasData(
                new TemplateEntity(
                    Guid.NewGuid(),
                    EventType.AppointmentCancelled,
                    EventBody.Create("Hi {FirstName}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.").Value,
                    EventTitle.Create("Appointment Cancelled").Value));
        }
    }
}
