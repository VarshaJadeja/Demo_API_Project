using DemoAPIProject.ChatHub;
using DemoAPIProject.Configuration;
using DemoAPIProject.Repositories;
using DemoAPIProject.Services;

using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetSection("Database")
    .Get<DbSetting>()?.ConnectionString;

var databaseName = builder.Configuration.GetSection("Database")
    .Get<DbSetting>()?.DatabaseName;

// Register MongoDB services
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
    return new MongoClient(mongoClientSettings);
});

// Add services to the container.
builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddGrpcClient<AuthService.Greeter.GreeterClient>(options =>
{
    options.Address = new Uri("https://localhost:7208"); // Set the address of gRPC server
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseRouting()
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ChatHub>("/chatHub");
    });
app.MapControllers();
app.Run();
