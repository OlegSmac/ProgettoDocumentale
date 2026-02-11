using MediatR;
using ProgettoDocumentale.API.Utils;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.Institutions.Queries.GetInstitutions;
using ProgettoDocumentale.Infrastructure.Persistence;
using ProgettoDocumentale.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IDateTime, DateTimeService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<ProgettoDocumentale.Application.Common.Interfaces.IConfiguration, Configuration>();

var connectionString = builder.Configuration.GetConnectionString("ProgettoDocumentaleDb");
builder.Services.AddScoped<IProgettoDocContext>(sp =>
{    
    var dateTime = sp.GetService<IDateTime>();
    var currentUser = sp.GetService<ICurrentUserService>();
    
    return new ProgettoDocContext(connectionString, dateTime, currentUser);
});

builder.Services.AddMediatR(typeof(GetAllInstitutionsQuery).Assembly);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
