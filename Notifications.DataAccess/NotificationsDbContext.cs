﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Notifications.Common.Enums;
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
                new TemplateEntity
                {
                    Id = Guid.NewGuid(),
                    EventType = EventType.AppointmentCancelled,
                    Body = "Hi {FirstName}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                    Title = "Appointment Cancelled"
                });
        }
    }
}
