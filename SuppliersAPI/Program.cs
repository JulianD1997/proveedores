using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");

builder.Services.AddDbContext<SupplierDbContext>(options =>
{
    options.UseMongoDB(connectionString, "suppliers_db");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISupplierService, SupplierService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Urls.Add("http://*:80");

app.MapControllers();

app.Run();
