using Heavy.Web.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Heavy.Web.Data;
using Heavy.Web.Filters;
using Heavy.Web.Models;
using Heavy.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heavy.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //要用角色AddDefaultIdentity改成addidentity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<HeavyContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });

            services.AddScoped<IAlbumService, AlbumEfService>();

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("仅限管理员", policy => policy.RequireRole("Administrators"));
                    options.AddPolicy("编辑专辑", policy => policy.RequireClaim("Edit Album"));

                    #region 扩展和上面的效果是一样的，但可以使用更多的自定义操作
                    //options.AddPolicy("编辑专辑1", policy => policy.RequireAssertion(context =>
                    //{
                    //    if (context.User.HasClaim(x => x.Type == "Edit Album"))
                    //        return true;
                    //    return false;
                    //}));

                    options.AddPolicy("编辑专辑2", policy => policy.AddRequirements(
                       // new EmailRequirement("2295559393@qq.com"),
                        new QualifiedUserRequirment()
                        ));
                    #endregion
                }); //授权,定于策略
            services.AddSingleton<IAuthorizationHandler, EmailHandler>();
            services.AddSingleton<IAuthorizationHandler, CanEditAlbumHandler>();
            services.AddSingleton<IAuthorizationHandler, AdiminstratorsHandler>();

            //xsrf 参考：https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2或者https://www.cnblogs.com/Leo_wl/p/7456839.html
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });
            services.AddMvc(option =>
            {
                option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                #region 全局设置filter
                //option.Filters.Add(new LogResourceFilter());
                //option.Filters.Add(typeof(LogAsyncResourceFilter));
                option.Filters.Add<LogResourceFilter>();
                #endregion

                #region response缓存
                option.CacheProfiles.Add("Default", new CacheProfile
                {
                    Duration = 60
                });
                option.CacheProfiles.Add("Never", new CacheProfile
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
                #endregion

            });

            #region 使用缓存

            //services.AddMemoryCache();

            //redis缓存
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "redis-for-albums";
            });
            #endregion

            #region 压缩
            services.AddResponseCompression();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseStatusCodePages();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler("/home/MyError");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            #region 压缩
            app.UseResponseCompression();
            #endregion
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
