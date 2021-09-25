using System.Collections.Generic;
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
        [ProducesResponseType(typeof(List<NotificationModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Get(int? userId = null)
        {
            var notificationsResult = _notificationsService.GetNotifications(userId);
            return notificationsResult.IsSuccess
                    ? Ok(notificationsResult.Value)
                    : BadRequest(notificationsResult.Error);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(NotificationModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Post(EventModel eventModel)
        {
            var notificationsResult = _notificationsService.CreateNotification(eventModel);
            return notificationsResult.IsSuccess
                    ? Ok(notificationsResult.Value)
                    : BadRequest(notificationsResult.Error);
        }
    }
}