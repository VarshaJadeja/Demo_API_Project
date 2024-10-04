using Account.Configuration;
using Account.Repositories;
using Account.Services;
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
builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddGrpc();

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

var app = builder.Build();

app.UseCors();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserService>();
});
app.MapControllers();

app.Run();
