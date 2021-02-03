# Notifications Microservice - Scenario

The Notifications microservice manages user notifications. It responds to system wide events and depending on the type of event it will generate a new user notification and store it in its store. 

When a user logs in, the user will fetch all of their notifications.

---

## Appointment cancelled notification 

### Requirements:
The notification service listens for the ‘Appointment Cancelled’ event and on receiving the event the service will create a new notification model and store it in the notifications database. Event data includes:
```
- FirstName 
- AppointmentDateTime
- OrganisationName 
- Reason
```

The notification service stores a predefined notification template associated to the Appointment Cancelled event. On generating a new notification, the notificaiton template is retrieved from the store, it’s notification body is interpolated with the event data, and stored with the user id.
The notification service can return back notifications for a given user id.

---

### What you need to do:

1. Read the requirements above, and ensure you understand them.

2. Create a branch in this repo named `assessment-[your-name]`

3. Create a SQL database named `notifications-db` and set the connection string (this is hardcoded in `Startup.cs`)

4. Store the following notification template record (use either data seeding or a migration):
    ```
    Id: Guid
    EventType: AppointmentCancelled
    Body: ‘Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.’
    Title: ‘Appointment Cancelled’
    ```

5. Add an API for the notification service to receive events. Events should include the following properties:
    ```
    Type
    Data
    UserId
    ```

6. Handle new events:
    - Read template from store based on EventType
    - Create new User Notification by combining the template with the data received in the Event.
    - Store the User notification

7. Add an API to fetch notifications by user id. 

8. Appropriate unit and/or integration tests proving that the code fulfills the requirements.

9. **Push your changes** and **create a pull request to master**
    - Please make sure you include all necessary files. Hint: download your branch as a zip file and make sure it compiles & runs!
    - If we need to do *anything more than build & run the solution* please include instructions!

10. Email us the URL to your pull request :)

---

## OPTIONAL  if you would like to demo your front end skills
### Only if you have time. You will not be marked down for leaving this out.

- Add a BASIC front end client, using React or your choice of UI framework.

    - The UI should have two pages & a menu system to navigate between them.
    - Create event page: Web form to create a new Event. Hard-code the userId.
    - List user notifications page: List all the notifications for the hard-coded userId above.
