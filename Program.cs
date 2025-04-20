using QuestPDF.Infrastructure;
using TransactionsApi.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = SecretKey.KeySecret;
builder.Services.ConfigureAuthentication(key);

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
