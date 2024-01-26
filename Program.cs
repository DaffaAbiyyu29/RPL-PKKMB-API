using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PKKMB_API.EmailService;
using PKKMB_API.Model;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.Configure<EmailSettingModel>(builder.Configuration.GetSection("MailSettings"));
var emailSettings = builder.Configuration.GetSection("MailSettings").Get<EmailSettingModel>();
Console.WriteLine($"Email settings: {emailSettings.Email}, {emailSettings.Host}, {emailSettings.Port}");
builder.Services.AddTransient<IMailService, ResetPasswordRepository>();*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAnyOrigin", builder =>
	{
		builder.WithOrigins("https://localhost:7240")
			   .AllowAnyHeader()
			   .AllowAnyMethod()
			   .AllowCredentials();
	});
});

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.Authority = "http://localhost:7240";
		options.RequireHttpsMetadata = false;
		options.Audience = "api";
	});

builder.Services.AddAuthorization();

builder.Services.AddScoped<LoginRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();