using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;
using System.Text;

public partial class Program { }

public class WebApiApplication
{
    public static void Main(string[] args)
    {

    var builder = WebApplication.CreateBuilder(args);
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    builder.Services.AddControllers(); 
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IdentityService>();
    builder.Services.AddScoped<UsuarioService>();
    builder.Services.AddScoped<ComentarioService>();
    builder.Services.AddScoped<CategoriaAjudaService>();
    builder.Services.AddScoped<ParticipacaoService>();
    builder.Services.AddScoped<PedidoAjudaService>();
    builder.Services.AddScoped<TokenService>();
    builder.Services.AddScoped<IdentityService>();


    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");

    var jwtKey = builder.Configuration["Jwt:Key"] 
        ?? throw new InvalidOperationException("A chave do JWT (Jwt:Key) não foi encontrada.");

    var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
        ?? throw new InvalidOperationException("O emissor do JWT (Jwt:Issuer) não foi encontrado.");

    var jwtAudience = builder.Configuration["Jwt:Audience"] 
        ?? throw new InvalidOperationException("A audiência do JWT (Jwt:Audience) não foi encontrada.");

    builder.Services.AddDbContext<VizinhancaContext>(options =>
        options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()

        );

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer, 
            ValidateAudience = true,
            ValidAudience = jwtAudience, 
            ValidateLifetime = true
        };
    });

    var corsOrigins = builder.Configuration.GetValue<string>("CorsOrigins");
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            if (corsOrigins != null)
                            {
                                policy.WithOrigins(corsOrigins.Split(',')) // Separa por vírgula para múltiplas origens
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                            }
                        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(MyAllowSpecificOrigins);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();


    app.Run();
    }
}     