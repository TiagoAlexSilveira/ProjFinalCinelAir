using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAir.CommonCore.Helper;
using ProjFinalCinelAirAdmin.Data;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace ProjFinalCinelAirAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }




        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider; //gera um token 
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;
            })
                .AddDefaultTokenProviders()        //Extension Methods, m�todos que chamam outros
                .AddEntityFrameworkStores<DataContext>();

            //services.AddAuthentication().AddCookie().AddJwtBearer(cfg =>
            //{
            //    cfg.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidIssuer = this.Configuration["Tokens:Issuer"],
            //        ValidAudience = this.Configuration["Tokens:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
            //    };
            //});
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });


            // Adicionar o servi�o para gerar Tokens: (Nuget n�o pode ser a �ltima vers�o)
            services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = this.Configuration["Tokens:Issuer"],
                    ValidAudience = this.Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(this.Configuration["Tokens:key"]))
                };
            });



            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddTransient<SeedDb>();

            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IMailHelper, MailHelper>();            
            services.AddScoped<IClientHelper, ClientHelper>();            
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IAwardTicketRepository, AwardTicketRepository>();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddControllersWithViews();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Para todas as p�ginas: Lidar com o erro 404 qd as actions n�o existem
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
       
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
