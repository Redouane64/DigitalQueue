using DigitalQueue.Web.Areas.Teachers;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Extensions;

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

builder.Services.AddSession(options =>
{
    options.Cookie.MaxAge = TimeSpan.FromMinutes(5);
});

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataContext(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddApi(builder.Configuration);
builder.Services.AddTeachersServices();

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

app.UseCors("_AnyClient");

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.MapControllers();

// application data initialization
await app.InitializeDefaultUser();
app.Run();
