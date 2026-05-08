using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library Management System API",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Library Management System API")
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
               .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });

    if (ShouldOpenBrowserTabs(builder))
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            _ = OpenStartupBrowserTabsAsync(app);
        });
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static bool ShouldOpenBrowserTabs(WebApplicationBuilder builder)
{
    return TryGetLaunchBrowserFromLaunchSettings(builder.Environment.ContentRootPath)
        ?? TryGetBoolean(builder.Configuration["launchBrowser"])
        ?? false;
}

static bool? TryGetLaunchBrowserFromLaunchSettings(string contentRootPath)
{
    var launchSettingsPath = Path.Combine(contentRootPath, "Properties", "launchSettings.json");

    if (!File.Exists(launchSettingsPath))
    {
        return null;
    }

    try
    {
        using var launchSettings = JsonDocument.Parse(File.ReadAllText(launchSettingsPath));

        if (!launchSettings.RootElement.TryGetProperty("profiles", out var profiles))
        {
            return null;
        }

        var launchProfileName = Environment.GetEnvironmentVariable("DOTNET_LAUNCH_PROFILE");

        if (!string.IsNullOrWhiteSpace(launchProfileName)
            && profiles.TryGetProperty(launchProfileName, out var activeProfile)
            && activeProfile.TryGetProperty("launchBrowser", out var activeLaunchBrowser))
        {
            return activeLaunchBrowser.GetBoolean();
        }

        foreach (var profile in profiles.EnumerateObject())
        {
            if (profile.Value.TryGetProperty("commandName", out var commandName)
                && string.Equals(commandName.GetString(), "Project", StringComparison.OrdinalIgnoreCase)
                && profile.Value.TryGetProperty("launchBrowser", out var launchBrowser))
            {
                return launchBrowser.GetBoolean();
            }
        }
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"Could not read launchBrowser from launchSettings.json: {ex.Message}");
    }

    return null;
}

static bool? TryGetBoolean(string? value)
{
    return bool.TryParse(value, out var result) ? result : null;
}

static async Task OpenStartupBrowserTabsAsync(WebApplication app)
{
    await Task.Delay(1000);

    var baseUrl = app.Urls.FirstOrDefault(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        ?? app.Urls.FirstOrDefault();

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        Console.WriteLine("No application URL was available to open browser tabs.");
        return;
    }

    foreach (var url in new[] { baseUrl, $"{baseUrl.TrimEnd('/')}/scalar/v1" })
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

            await Task.Delay(300);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open {url}: {ex.Message}");
        }
    }
}
