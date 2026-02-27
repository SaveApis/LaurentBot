var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.StartAsync().ConfigureAwait(false);
await app.WaitForShutdownAsync().ConfigureAwait(false);
