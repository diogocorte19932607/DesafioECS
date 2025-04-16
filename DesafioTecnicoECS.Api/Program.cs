using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DesafioTecnicoECS.Domain.Configuration;
using DesafioTecnicoECS.Domain.Mapping;
using DesafioTecnicoECS.Domain.Service;
using DesafioTecnicoECS.Domain.Service.Generic;
using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Repositories;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;
using DesafioTecnicoECS.Infra.Repository;
using DesafioTecnicoECS.Infra.UnitofWork;
using DesafioTecnicoECS.Infra.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("clienteDB");

builder.Services.AddDbContext<ClientContext>(options =>
    options.UseSqlServer(
        connectionString,
        sql => sql.MigrationsAssembly("DesafioTecnicoECS.Infra")
    )
);

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitofWork, UnitOfWork>();

builder.Services.AddScoped(typeof(ProductService<,>), typeof(ProductService<,>));
builder.Services.AddScoped(typeof(GenericServiceAsync<,>), typeof(GenericServiceAsync<,>));
builder.Services.AddScoped(typeof(IServiceAsync<,>), typeof(GenericServiceAsync<,>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret ?? "");

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = appSettings.ValidoEm,
            ValidIssuer = appSettings.Emissor,
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
