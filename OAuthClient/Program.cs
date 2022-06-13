var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = "ClientCookie";
        config.DefaultSignInScheme = "ClientCookie";
        config.DefaultChallengeScheme = "OurServer";
    })
    .AddCookie("ClientCookie")
    .AddOAuth("OurServer", option =>
    {
        option.ClientId = "client_id";
        option.ClientSecret = "client_secret";
        option.CallbackPath = "/oauth/callback";
        option.AuthorizationEndpoint = "https://localhost:7150/oauth/authorize";
        option.TokenEndpoint = "https://localhost:7150/oauth/token";

        option.SaveTokens = true;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapDefaultControllerRoute();
});

app.Run();