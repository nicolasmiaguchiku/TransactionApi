using QuestPDF.Infrastructure;
using TransactionsApi.Settings;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.ConfigureAuthentication(SecretKey.Key);

builder.Services.ConfigureDateBase(builder.Configuration);

builder.Services.ConfigureCustomServices();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
