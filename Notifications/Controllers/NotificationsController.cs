using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get(int? userId = null)
        {
            var notificationsResult = await _notificationsService.GetNotifications(userId);
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
            var notificationsResult = await _notificationsService.CreateNotification(eventModel);
            return notificationsResult.IsSuccess
                    ? Ok(notificationsResult.Value)
                    : BadRequest(notificationsResult.Error);
        }
    }
}