using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using paf.api.Dtos;
using paf.api.Infrastructure;
using paf.api.Interfaces;
using paf.api.Middleware;
using paf.api.Models;
using paf.api.Services;
using paf.api.validation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddIdentity<IdentityUser,IdentityRole>(
    opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequiredLength = 4;
        opt.User.RequireUniqueEmail = true;
    }

    ).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("vaerylongtokentomaketheidentitymoresecure")),
        ValidIssuer = "http://localhost:500",
        ValidateIssuer = true,
        ValidateAudience = false,
    };
});

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IGenericRepostiory<User>, GenericRepostiory<User>>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IGenericRepostiory<Blog>,GenericRepostiory<Blog>>();
builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddScoped<IGenericRepostiory<Comment>, GenericRepostiory<Comment>>();
builder.Services.AddScoped<ICommentService, CommentService>();


builder.Services.AddAutoMapper(typeof(MapperProfile));
ValidatorDIConfig.RegisterValidatorDI(builder.Services);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbcontext.Database.EnsureCreated();
}
app.UseMiddleware<ExceptionMiddleWare>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
