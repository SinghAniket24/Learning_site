
# Project: Learning Site

This document provides a detailed overview of the Learning Site project, a C# ASP.NET Core web application.

## 1. Project Overview

The Learning Site is a web application designed to help users create, manage, and track their learning plans. It is particularly focused on creating plans from YouTube videos.

- **Backend:** C# with ASP.NET Core and Razor Pages.
- **Frontend:** HTML, CSS, JavaScript, and Bootstrap.
- **Database:** SQL Server LocalDB with Entity Framework Core.
- **Authentication:** Supabase (email/password and Google OAuth).
- **External APIs:**
    - YouTube Data API: For searching videos and creating plans.
    - Zen Quotes API: For displaying motivational quotes on the dashboard.

## 2. Core Features

### 2.1. User Authentication

- Users can sign up for a new account or log in with an existing one.
- Authentication is handled by Supabase, supporting both email/password and Google OAuth.
- Authenticated pages are protected and redirect to the login page if the user is not logged in.

### 2.2. Dashboard

- The main landing page after a user logs in.
- Displays a summary of the user's current learning plans.
- Shows a random motivational quote fetched from the Zen Quotes API.

### 2.3. Plan Management (CRUD)

- Users can perform Create, Read, Update, and Delete (CRUD) operations on their learning plans.
- A learning plan is defined by the `Management` model.

### 2.4. YouTube Integration

- The `YouTubePlanService` is responsible for interacting with the YouTube Data API.
- It can search for YouTube videos by topic and optionally by channel name.
- It can generate a structured daily learning plan based on a list of videos, a number of days, and a daily time commitment.

### 2.5. Learning Plan Pages

- **My Plans:** Displays all of the user's created study plans.
- **Weekly Plans:** Shows a weekly view of the learning schedule.
- **Video Player:** A dedicated page for watching videos, likely from YouTube.

### 2.6. Settings

- A page where users can manage their account settings.

## 3. Project Structure

```
/
├── Data/
│   └── Learning_siteContext.cs   # EF Core database context.
├── Migrations/                   # EF Core database migrations.
├── Models/
│   └── Management.cs             # The main data model for a learning plan.
├── Pages/
│   ├── Managements/              # CRUD pages for the Management model.
│   │   ├── Create.cshtml
│   │   ├── Delete.cshtml
│   │   ├── Details.cshtml
│   │   ├── Edit.cshtml
│   │   └── Index.cshtml
│   ├── AuthenticatedPageModel.cs # Base class for pages requiring authentication.
│   ├── dashboard.cshtml          # The user dashboard.
│   ├── Index.cshtml              # The landing page, redirects to the dashboard.
│   ├── login.cshtml              # User login page.
│   ├── MyPlans.cshtml            # Page to display user's plans.
│   ├── settings.cshtml           # User settings page.
│   ├── signup.cshtml             # User signup page.
│   ├── videoplayer.cshtml        # Page for playing videos.
│   ├── weeklyplans.cshtml        # Page for displaying the weekly plan.
│   └── YoutubePlanService.cs     # Service for YouTube API interaction.
├── wwwroot/                        # Static assets (CSS, JS, images).
├── appsettings.json              # Application configuration.
├── Program.cs                    # The main entry point of the application.
└── Learning_site.csproj          # The C# project file with dependencies.
```

## 4. Data Model

### `Management.cs`

This is the core model representing a learning plan.

```csharp
public class Management : BaseModel
{
    [Key]
    public string Title { get; set; }
    public string Channel { get; set; }
    public string Description { get; set; }
    public TimeSpan DailyTime { get; set; }
    public int Days { get; set; }
}
```

## 5. Key Services and Logic

### `Program.cs`

- Configures all the essential services for the application:
    - Razor Pages
    - `DbContext` for Entity Framework Core
    - `HttpClientFactory` for making HTTP requests
    - Supabase client for authentication
- Sets up the HTTP request pipeline (middleware), including routing and authorization.

### `YouTubePlanService.cs`

- Contains the logic for interacting with the YouTube Data API.
- `SearchVideosAsync`: Searches for videos on YouTube.
- `CreateDailyPlan`: Organizes a list of videos into a daily plan.

### `AuthenticatedPageModel.cs`

- A base page model that checks if a user is authenticated with Supabase.
- Any page that inherits from this model will automatically be protected.

## 6. Environment Variables

The application uses a `.env` file to store sensitive information.

- `SUPABASE_URL`: The URL of the Supabase project.
- `SUPABASE_KEY`: The public API key for the Supabase project.

This documentation should provide a solid foundation for understanding the project.
