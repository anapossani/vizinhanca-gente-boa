using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

public partial class Program { }

public class WebApiApplication
{
    public static void Main(string[] args)
    {

    var builder = WebApplication.CreateBuilder(args);
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    
    builder.Configuration.AddJsonFile("/etc/secrets/secrets.json", optional: true, reloadOnChange: true);
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
        options.UseNpgsql(connectionString, npgsqlOptionsAction: sqlOptions =>
        {
            // Habilita a resiliência de conexão
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Tenta se reconectar até 5 vezes
                maxRetryDelay: TimeSpan.FromSeconds(30), // Espera até 30s entre as tentativas
                errorCodesToAdd: null);
        })
        .UseSnakeCaseNamingConvention()
);

        //builder.Services.AddRateLimiter(options =>
       // {
       //     options.AddFixedWindowLimiter(policyName: "fixed", opt =>
       //     {
       //         opt.PermitLimit = 5; 
       //         opt.Window = TimeSpan.FromSeconds(60); 
       //         opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; 
       //         opt.QueueLimit = 2; 
       //     });
//
 //           options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
 //       });        

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
                                policy.WithOrigins(corsOrigins.Split(',')) 
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                            }
                        });
    });

    var app = builder.Build();
    
    app.UseCors(MyAllowSpecificOrigins); 
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    //app.UseRateLimiter();
    app.UseRouting();
    
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
    }
}     