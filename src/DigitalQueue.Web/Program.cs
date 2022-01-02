using DigitalQueue.Web.Api;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Extensions;
using DigitalQueue.Web.Users;
using DigitalQueue.Web.Users.Extensions;

using Hellang.Middleware.ProblemDetails;

using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddDataContext(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Queue v1");
    });
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// application data initialization
await app.InitializeDefaultUser();
app.Run();
