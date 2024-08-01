using SchoolsProjectBlazorDapper.Components;
using MudBlazor.Services;
using SchoolsProjectBlazorDapper.Data.Schools;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolsProjectBlazorDapper.Components.Account;
using SchoolsProjectBlazorDapper.Data.Account;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("Users") ?? throw new InvalidOperationException("Connection string 'Users' not found.");

builder.Services.AddDbContext<UsersDbAccessLayer>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UsersDbAccessLayer>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<SchoolsDbAccessLayer>();
builder.Services.AddMudServices();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add roles for authentication
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Teacher", "Student" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Create admin account if doesn't exist
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string email = "admin@admin.com";
    string password = "utOFlURzbYZPv9o@";

    // If admin account doesn't exist
    if (await userManager.FindByEmailAsync(email) == null)
    {
        // Create user
        var user = new User();
        user.UserName = email;
        user.Email = email;
        var result = await userManager.CreateAsync(user, password);

        // Assign admin role
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

// Create student (for testing) if doesn't exits
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string email = "student@student.com";
    string password = "utOFlURzbYZPv9o@";

    // If student account doesn't exist
    if (await userManager.FindByEmailAsync(email) == null)
    {
        // Create user
        var user = new User();
        user.UserName = email;
        user.Email = email;
        var result = await userManager.CreateAsync(user, password);

        // Assign Student role
        await userManager.AddToRoleAsync(user, "Student");
    }
}

// Create teacher (for testing) if doesn't exits
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string email = "teacher@teacher.com";
    string password = "utOFlURzbYZPv9o@";

    // If teacher account doesn't exist
    if (await userManager.FindByEmailAsync(email) == null)
    {
        // Create user
        var user = new User();
        user.UserName = email;
        user.Email = email;
        var result = await userManager.CreateAsync(user, password);

        // Assign Teacher role
        await userManager.AddToRoleAsync(user, "Teacher");
    }
}

app.Run();