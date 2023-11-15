using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Roleta.Aplicacao;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio.Identity;
using Roleta.Persistencia;
using Roleta.Persistencia.Interface;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

ConfigureDbContext(builder);
ConfigureServices(builder);
ConfigureAuthentication(builder);

var app = builder.Build();
ConfigureAplication(app);

//app.UseIPWhitelist();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void ConfigureDbContext(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<RoletaContext>(
        context => {
            context.UseMySql(builder.Configuration.GetConnectionString("ConnDB"), 
                             ServerVersion.Parse("10.6.15-MariaDB"));
            //context.UseSqlite(builder.Configuration.GetConnectionString("ConnDB")); //for SQLite
            //context.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //somente para SQLServer
            context.EnableSensitiveDataLogging();
        });
}

static void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddIdentityCore<User>(options =>
                    {
                        // Password settings.
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                        options.SignIn.RequireConfirmedEmail = false;

                        // Lockout settings.
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.AllowedForNewUsers = true;

                        // User settings.
                        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                        options.User.RequireUniqueEmail = false;
                    }
                        )
                    .AddRoles<Role>()
                    .AddRoleManager<RoleManager<Role>>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddRoleValidator<RoleValidator<Role>>()
                    .AddEntityFrameworkStores<RoletaContext>()
                    .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options => {
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

    builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromHours(3);
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers()
                    .AddJsonOptions(x =>
                    {
                        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("BetBrazil",
            policy =>
            {
                policy.WithOrigins("https://www.betbrazil.pro", "https://betbrazil.pro", "http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
            });

        options.AddPolicy("AcessoLivre",
            policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin();
            });
    });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IProdutoService, ProdutoService>();
    builder.Services.AddScoped<IPagamentoService, PagamentoService>();
    builder.Services.AddScoped<ISaqueService, SaqueService>();    
    builder.Services.AddScoped<IRoletaService, RoletaService>();

    builder.Services.AddScoped<IUserPersist, UserPersist>();
    builder.Services.AddScoped<IProdutoPersist, ProdutoPersist>();
    builder.Services.AddScoped<IPagamentoPersist, PagamentoPersist>();
    builder.Services.AddScoped<ISaquePersist, SaquePersist>();
    builder.Services.AddScoped<IGiroRoletaPersist, GiroRoletaPersist>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Roleta.API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header usando Bearer.
                            Entre com 'Bearer' [espaço] entao coloque seu token.
                            Exemplo: 'Bearer 1234abcdfghijk'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });

    

//builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IpListOptions"));

var CredentialPayment = new EzzePayConfig();
    builder.Configuration.GetSection("EzzePay").Bind(CredentialPayment);
    builder.Services.AddSingleton<IEzzePayService, EzzePayService>(_ => new(CredentialPayment));

    var smtp = new SmtpConfig();
    builder.Configuration.GetSection("Smtp").Bind(smtp);
    builder.Services.AddSingleton<IEmailService, EmailService>(_ => new(smtp));
}

static void ConfigureAplication(WebApplication app)
{
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI();
    //}

    app.UseCors();

    //app.UseCors(cors => cors.WithOrigins("https://www.betbrazil.pro", "https://betbrazil.pro", "http://localhost:4200")
    //                        .AllowAnyHeader()
    //                        .AllowAnyMethod()
    //                        .AllowCredentials());

    app.UseStaticFiles(new StaticFileOptions()
    {
        //FileProvider = new PhysicalFileProvider(Path.Combine(app.Configuration["FileProvider"])),
        //RequestPath = new PathString($"/{app.Configuration["FileProvider"]}")
    });

    app.UseRequestLocalization(
        new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR")
        }
    );
}
