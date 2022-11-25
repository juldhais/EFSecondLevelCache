using EFSecondLevelCache.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var dataContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
dataContext.Database.EnsureCreated();
dataContext.Generate();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();