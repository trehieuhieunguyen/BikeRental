using BikeService.Sonic.Extensions;
using BikeService.Sonic.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScopedServices();
builder.Services.AddSingletonServices(builder.Configuration);
builder.Services.AddElasticClient(builder.Configuration);
builder.Services.AddOktaAuthenticationService(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials().SetIsOriginAllowed(_ => true));
});
builder.Services.AddDbContextService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.RunMigrations();


// App pipelines
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<BikeLocationHub>("/bikeLocationHub");

app.Run();
