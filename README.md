# Notifications Microservice - Scenario

The Notifications service manages user notifications for system events. To do this, it receives system events and depending on the type of event it will generate a new user notification and store it in its store.

When a user logs in, the user will fetch all of their notifications.

---

## Appointment cancelled notification 

### Requirements:
The notification service must accept ‘Appointment Cancelled’ events (received via HTTP).
On receiving this type of event the service will create a new notification model and store it in the notifications database.
All incoming Events will have the following basic structure:
```
 Type
 Data
 UserId
```
For ‘Appointment Cancelled’ events (specifically), the **Data** attribute will include:
```
- FirstName 
- AppointmentDateTime
- OrganisationName 
- Reason
```

The notification service stores a predefined notification template associated to the Appointment Cancelled event. On generating a new notification, the notificaiton template is retrieved from the store, the notification body is interpolated with the event data, and then stored with the user id.
The notification service can return back notifications for a given user id.

---

