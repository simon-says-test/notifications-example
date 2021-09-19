using System;
using System.Collections.Generic;
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
        public IReadOnlyCollection<NotificationModel> Get(int? userId = null)
        {
            try
            {
                return _notificationsService.GetNotifications(userId);
            }
            catch
            {
                // Anything else is a problem 
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return null;
            }
        }

        [Route("")]
        [HttpPost]
        public ActionResult Post(EventModel eventModel)
        {
            try
            {
                return Ok(_notificationsService.CreateNotification(eventModel));
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                return BadRequest(ex.Message);  // Inform consumer of bad request data
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}