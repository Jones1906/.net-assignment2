using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Assignment1.Data; 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Use an in-memory database (no connection string needed)
builder.Services.AddDbContext<Assignment1DbContext>(options =>
    options.UseInMemoryDatabase("VetClinicDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data once at startup
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<Assignment1DbContext>();
    DbInitializer.Initialize(ctx);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
