using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

// Add services to the container.
var builder = ServicesAggregator.AddServices(WebApplication.CreateBuilder(args));

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(builder =>
  {
    builder
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});

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

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
