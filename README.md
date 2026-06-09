# EventEase-app-using-Blazor
1.EventCard.razor
Copilot Generated Event Card Component + Data Binding
<div class="event-card">
    <h3>@(string.IsNullOrWhiteSpace(EventName)
        ? "Unnamed Event"
        : EventName)</h3>

    <p>
        Date:
        @(EventDate == default
            ? "TBD"
            : EventDate.ToString("MMMM dd, yyyy"))
    </p>

    <p>
        Location:
        @(string.IsNullOrWhiteSpace(EventLocation)
            ? "Location TBD"
            : EventLocation)
    </p>

    <p>
        Attendees: @AttendeeCount
    </p>
</div>

@code{
    [Parameter]
    public string EventName { get; set; }

    [Parameter]
    public DateTime EventDate { get; set; }

    [Parameter]
    public string EventLocation { get; set; }

    [Parameter]
    public int AttendeeCount { get; set; }
}

2. EventModel.cs
namespace EventEase.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public int AttendeeCount { get; set; }
    }
}

3. EventList.razor
Routing + Performance Optimization
@page "/events"

@using EventEase.Models

<h2>Upcoming Events</h2>

<Virtualize Items="events" Context="ev">
    <EventCard
        EventName="@ev.Name"
        EventDate="@ev.Date"
        EventLocation="@ev.Location"
        AttendeeCount="@ev.AttendeeCount" />

    <NavLink href="@($"/event/{ev.Id}")">
        View Details
    </NavLink>

    <hr />
</Virtualize>

@code{

    private List<EventModel> events = new()
    {
        new()
        {
            Id = 1,
            Name = "AI Summit",
            Date = new DateTime(2026,7,20),
            Location = "Delhi",
            AttendeeCount = 100
        },

        new()
        {
            Id = 2,
            Name = "Tech Conference",
            Date = new DateTime(2026,8,12),
            Location = "Kolkata",
            AttendeeCount = 75
        }
    };
}

4. EventDetails.razor
Debugged Routing
@page "/event/{Id:int}"

@using EventEase.Models

@if(Event == null)
{
    <p>
        Event not found.
    </p>

    <NavLink href="/events">
        Back to Events
    </NavLink>
}
else
{
    <h2>@Event.Name</h2>

    <p>Date: @Event.Date.ToShortDateString()</p>

    <p>Location: @Event.Location</p>

    <p>Attendees: @Event.AttendeeCount</p>

    <NavLink href="/registration">
        Register
    </NavLink>
}

@code{

    [Parameter]
    public int Id { get; set; }

    private EventModel Event;

    protected override void OnInitialized()
    {
        Event = new EventModel
        {
            Id = Id,
            Name = "AI Summit",
            Date = DateTime.Now.AddDays(30),
            Location = "Delhi",
            AttendeeCount = 100
        };
    }
}

5. RegistrationModel.cs
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

6. UserSessionService.cs
State Management
namespace EventEase.Services
{
    public class UserSessionService
    {
        public string UserName { get; set; }

        public bool IsRegistered { get; set; }
    }
}

7. Registration.razor
Form Validation + Attendance Tracker
@page "/registration"

@using EventEase.Models
@using EventEase.Services

@inject UserSessionService Session

<h2>Register for Event</h2>

@if(Session.IsRegistered)
{
    <div>
        Welcome Back @Session.UserName
    </div>
}

<EditForm Model="@registration"
          OnValidSubmit="RegisterUser">

    <DataAnnotationsValidator />

    <ValidationSummary />

    <div>
        <label>Name</label>

        <InputText
            @bind-Value="registration.Name" />
    </div>

    <div>
        <label>Email</label>

        <InputText
            @bind-Value="registration.Email" />
    </div>

    <button type="submit">
        Register
    </button>

</EditForm>

@if(showSuccess)
{
    <h4>
        Registration Successful
    </h4>
}

@code{

    private RegistrationModel registration = new();

    private bool showSuccess;

    private int attendeeCount = 100;

    private void RegisterUser()
    {
        Session.UserName = registration.Name;

        Session.IsRegistered = true;

        attendeeCount++;

        showSuccess = true;
    }
}


8. Program.cs
using EventEase.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<UserSessionService>();

var app = builder.Build();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
