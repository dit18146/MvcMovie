using MvcMovie.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IMovieService, MemoryMovieService>();   //When IMovieService is used an instance of MemoryMovieService is created


//builder.Services.AddSingleton<ISqlHelper, SqlHelper>();

builder.Services.AddSingleton<IMovieService, DbMovieService>();

//builder.Services.AddSingleton<IMovieService, DapperMovieService>();

builder.Services.AddSingleton<IMovieTypeService, DbMovieTypeService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddKendo();


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

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();