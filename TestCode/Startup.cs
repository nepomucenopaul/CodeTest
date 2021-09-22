using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.OData.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using System.Linq;
using TestCode.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
namespace TestCode
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(mvcOptions =>
                mvcOptions.EnableEndpointRouting = false);

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie();
            services.AddOData();
            services.AddODataAuthorization(options =>
            {
                options.ScopesFinder = context =>
                {
                    var userScopes = context.User.FindAll("Scope").Select(claim => claim.Value);
                    return Task.FromResult(userScopes);
                };
                options.ConfigureAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();
            });
            //This Line Connects segment connects the context to the database using the connection string specified on the application.json
            string CodeTestConnection = Configuration.GetConnectionString("CodeTestConnection");
            services.AddDbContext<CodeTestContext>(
                 options => options.UseSqlServer(CodeTestConnection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseODataAuthorization();
            //app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                //Enabled odata functions.
                routeBuilder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }
        IEdmModel GetEdmModel()
        {
            //Specify the End Points Included in Routing This Basically means that Model "Account" = Odata Route "Accounts"
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Account>("Accounts");
            odataBuilder.EntitySet<Payment>("Payments");
            odataBuilder.EntitySet<Status>("Statuses");
            return odataBuilder.GetEdmModel();
        }
    }
}
