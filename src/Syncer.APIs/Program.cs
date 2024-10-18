using Microsoft.EntityFrameworkCore;
using Syncer.APIs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SyncerDbContext>(configure =>
{
    configure.UseInMemoryDatabase("SyncerDb");
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();