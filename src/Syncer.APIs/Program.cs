
using Microsoft.EntityFrameworkCore;
using Syncer.APIs;
using Syncer.APIs.Endpoints;
using Syncer.APIs.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddHostedService<MemoryCachSetup>();

builder.Services.AddDbContext<SyncerDbContext>((sp,configure) =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString(SyncerDbContext.ConnectionStringName);
    configure.UseSqlServer(connectionString);
}); 

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEmojiEndpoints();
app.MapPresentationEndpoints();

app.Run();