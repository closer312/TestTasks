using EmployeeAPI.Interfaces;
using SoapCore;
using Microsoft.EntityFrameworkCore;
using EmployeeAPI.Services;
using EmployeeApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EmployeeDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IEmployeeService, EmployeeService>();

builder.Services.AddSoapCore();

builder.Services.AddControllers();
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
app.UseAuthorization();

var settings = app.Configuration.GetSection("FileWSDL").Get<WsdlFileOptions>();
settings.AppPath = app.Environment.ContentRootPath;
app.UseEndpoints(endpoints =>
{
    endpoints.UseSoapEndpoint<IEmployeeService>("/EmployeeService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
});

app.MapControllers();

app.Run();
