using AutoMapper;
using com.allcard.common;
using com.allcard.institution.common;
using com.allcard.institution.repository;
using com.allcard.institution.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace com.allcard.institution
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Authorization")
                    );
            });

            services.AddDbContext<InstitutionContext>(options =>
            options
            .UseSqlServer(Configuration.GetConnectionString("connectionString")));
            //.UseLazyLoadingProxies()
            //.UseMySql(Configuration.GetConnectionString("connectionString")));
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<QueConfiguration>(appSettingsSection);


            var appSettings = appSettingsSection.Get<QueConfiguration>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddFacebook(f =>
            //{
            //    f.AppId = Configuration["Authentication:Facebook:AppId"]; ;
            //    f.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //})
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            services.AddAutoMapper();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IChainRepository, ChainRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IBranchScheduleRepository, BranchScheduleRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IBranchScheduleMemberRepository, BranchScheduleMemberRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRefCityMunicipalityRepository, RefCityMunicipalityRepository>();
            services.AddScoped<IRefProvinceRepository, RefProvinceRepository>();



            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<IChainService, ChainService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IBranchScheduleService, BranchScheduleService>();
            services.AddScoped<IBranchScheduleMemberService, BranchScheduleMemberService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IRefCityMunicipalityServices, RefCityMunicipalityServices>();




            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<InstitutionContext>();
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();

            //    var seedTaks = (SeedData.AddInitialdata(context));
            //    seedTaks.Wait();
            //}


            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
