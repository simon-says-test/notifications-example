using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifications.Attributes;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationsService notificationsService, ILogger<NotificationsController> logger)
        {
            _notificationsService = notificationsService;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        [ValidateQueryParametersAttribute]
        [ProducesResponseType(typeof(List<NotificationModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get(int? userId = null)
        {
            var notificationsResult = await _notificationsService.GetNotifications(userId)
                .OnFailure(result => _logger.LogInformation($"Error: {result}"))
                .Tap(result => _logger.LogInformation($"{result.Count} records returned"));
            return notificationsResult.IsSuccess
                    ? Ok(notificationsResult.Value)
                    : BadRequest(notificationsResult.Error);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(NotificationModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post(EventModel eventModel)
        {
            var notificationsResult = await _notificationsService.CreateNotification(eventModel)
                .OnFailure(result => _logger.LogInformation($"Error: {result}"))
                .Tap(result => _logger.LogInformation($"Record ID {result.Id} returned"));
            return notificationsResult.IsSuccess
                    ? Ok(notificationsResult.Value)
                    : BadRequest(notificationsResult.Error);
        }
    }
}