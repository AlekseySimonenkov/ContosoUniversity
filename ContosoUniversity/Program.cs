using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ContosoUniversityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContosoUniversityContext") ?? throw new InvalidOperationException("Connection string 'ContosoUniversityContext' not found.")));
builder.Services.AddDbContext<ConUnContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConUnContext") ?? throw new InvalidOperationException("Connection string 'ConUnContext' not found.")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ConUnContext>();
     DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();