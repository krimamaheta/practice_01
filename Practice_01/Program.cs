using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Practice_01.Authentication;
using Practice_01.Data;

using Practice_01.PaymentRepository;
using Practice_01.Repository;
using practice1.Services;
using Stripe;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Practice_01.ViewModel;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowSetOrigins", options =>
        {
            options.WithOrigins("https://localhost:3000");
            options.WithOrigins();
            options.AllowAnyHeader();
            options.AllowAnyMethod();
            options.AllowCredentials();
        });


});

builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

//for identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//image
builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var cloudinary = new Cloudinary(new CloudinaryDotNet.Account(
        configuration["Cloudinary:CloudName"],
        configuration["Cloudinary:ApiKey"],
        configuration["Cloudinary:ApiSecret"]
    ));
    return cloudinary;
});


builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVendorEventRepository, VendorEventRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ChargeService>();

StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeOptions:SecretKey");
builder.Services.AddScoped<IStripeRepository, StripeRepository>();



//to add loop linq 
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });



//add email configuration
var emailconfigu = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfigu);
builder.Services.AddTransient<practice1.Services.IEmailService, EmailService>();





//add authentication
builder.Services.AddSingleton<SymmetricSecurityKey>(provider =>
{
    var key = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!);
    return new SymmetricSecurityKey(key);
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));




builder.Services.AddSingleton<SymmetricSecurityKey>(provider =>
{
    var key = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!);
    return new SymmetricSecurityKey(key);
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));


builder.Services.AddControllers();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add jwt bearer
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["JWT:ValidIssuer"];
    opt.Audience = builder.Configuration["JWT:ValidAudience"];
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
    };
});

builder.Services.AddCors();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });



    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
{
new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
Reference = new Microsoft.OpenApi.Models.OpenApiReference
{
Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
Id = "Bearer"
},
Scheme = "oauth2",
Name = "Bearer",
In = Microsoft.OpenApi.Models.ParameterLocation.Header,
},
new List<string>()
}
});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options =>
     options.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();


app.Run();
