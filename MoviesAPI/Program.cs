using Microsoft.AspNetCore.Authentication.JwtBearer;
using MoviesAPI;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Filter;
using MoviesAPI.APIBehavior;
using MoviesAPI.Helper;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.


builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
    sqlOptions => sqlOptions.UseNetTopologySuite())) ;



builder.Services.AddControllers(options => 
{
    options.Filters.Add(typeof(ParseBadRequest));   
}).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option => 
{
     var frontendURL = builder.Configuration.GetValue<string>("frontend_url");
    option.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    });
});


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton(provider => new MapperConfiguration(config => {
    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
    config.AddProfile(new AutoMapperProfiles(geometryFactory));
}).CreateMapper());

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices
    .Instance.CreateGeometryFactory(srid: 4326));

//builder.Services.AddScoped<IFileStorageService, AzureStorageService>();
//builder.Services.AddScoped<IFileStorageService, IFileAppStorageService>();
//builder.Services.AddHttpContextAccessor();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

//app.UseStaticFiles();

app.UseHttpsRedirection();

 
app.UseAuthorization();

 
app.MapControllers();

app.Run();
