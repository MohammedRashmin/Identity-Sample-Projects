using System.Security.Claims;
using Identiry_Sample;
using Identiry_Sample.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme).AddBearerToken(IdentityConstants.BearerScheme); // Enables cookie-based authentication

builder.Services.AddIdentityCore<Customer>()
    .AddEntityFrameworkStores<AuthDbcontext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<AuthDbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply pending migrations automatically (prevents manual migrations on startup)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbcontext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error"); // Handles errors gracefully in production
}

app.UseHttpsRedirection();

app.UseAuthentication(); // REQUIRED: Ensures user authentication before authorization
app.UseAuthorization();

// Simple authorization endpoint using Identity
app.MapGet("customers/me", async (ClaimsPrincipal claims, AuthDbcontext context) =>
{
    string? customerId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (customerId == null)
    {
        return Results.Unauthorized(); // Prevents null reference errors
    }

    var user = await context.Users.FindAsync(customerId);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.RequireAuthorization();

app.MapControllers();
app.MapIdentityApi<Customer>();

app.Run();
