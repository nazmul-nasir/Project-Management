using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project_Management.Models;
using Project_Management.Repository;
using Project_Management.Services;
using System.Text;

namespace Project_Management
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Management");
            });
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            // For Entity Framework  
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));


            services.Configure<AppSettings>(Configuration);

            services.AddIdentity<User, IdentityRole>()
           .AddEntityFrameworkStores<DataContext>()
           .AddDefaultTokenProviders();

            services.AddScoped<UserManager<Project_Management.Models.User>>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            /*// Configure JWT authentication
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });*/

            services.AddHttpContextAccessor();

            // Add other services and repositories
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDeveloperService, DeveloperService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            services.AddScoped<IAdminService, AdminService>();

            // Add interfaces
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<ITaskService, TaskService>();

            // Add repositories
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            // services.Configure<AppSettings>(Configuration.GetSection("Jwt"));
            // services.AddScoped<IOptions<AppSettings>>(provider => Options.Create(provider.GetService<AppSettings>()));




            /* // Configure CORS policy
             services.AddCors(options =>
             {
                 options.AddPolicy("AllowAnyOrigin",
                     builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
             });
 */

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Management", Version = "v1" });
            });
        }

    }
}
