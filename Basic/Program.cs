using System.Reflection.Metadata;
using System.Text;
using Basic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", option =>
    {
        var secretBytes = Encoding.UTF8.GetBytes(Contants.Secret);
        var key = new SymmetricSecurityKey(secretBytes);

        option.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Query.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Query["access_token"];
                }
                
                return Task.CompletedTask;
            }
        };

        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = Contants.Issuer,
            ValidAudience = Contants.Audience,
            IssuerSigningKey = key
        };
    });

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapDefaultControllerRoute();
});

app.Run();