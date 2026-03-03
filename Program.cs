using ct_foco_backend.Data;
using ct_foco_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<CtFocoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// CORS para React dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5084", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CtFocoDbContext>();
    db.Database.Migrate();
}
    

    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactDev");
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();
