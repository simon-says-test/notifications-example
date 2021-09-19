using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this._notificationsService = notificationsService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Get(int? userId = null)
        {
            var notificationsResult = _notificationsService.GetNotifications(userId);
            return Ok(notificationsResult.Value);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Post(EventModel eventModel)
        {
            var notificationsResult = _notificationsService.CreateNotification(eventModel);
            return notificationsResult.Finally((result) => result.IsSuccess ? Ok(notificationsResult.Value) : (ObjectResult)BadRequest(result.Error));
        }
    }
}