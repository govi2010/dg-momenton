using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DG.Momenton.API.Ninject;
using DG.Momenton.Domain.Employee;
using DG.Momenton.Infrastructure.CQRS;
using DG.Momenton.Infrastructure.NLog;
using DG.Momenton.Infrastructure.Repository;
using DG.Momenton.Infrastructure.Repository.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;

namespace DG.Momenton.API
{
    #region Startup

    public class Startup
    {
        #region Ninject Setup for ASP .NET core

        #region Private members

        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
        private IKernel Kernel { get; set; }
        private object Resolve(Type type) => Kernel.Get(type);
        private object RequestScope(IContext context) => scopeProvider.Value;
        private sealed class Scope : DisposableObject { }
        #endregion
        #region RegisterApplicationComponents

        /// <summary>
        /// Register application components to use with Ninject
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private async Task<IKernel> RegisterApplicationComponents(IApplicationBuilder app)
        {
            // IKernelConfiguration config = new KernelConfiguration();
            var kernel = new StandardKernel();

            // Register application services
            foreach (var ctrlType in app.GetControllerTypes())
            {
                kernel.Bind(ctrlType).ToSelf().InScope(RequestScope);
            }

            // Load Ninject Module
            kernel.Load<CqrsNinjectModule>();
            kernel.Load<EmployeeNinjectModule>();
            kernel.Load<RepositoryNinjectModule>();
            kernel.Load<NLogNinjectModule>();

            // Setup EF Data Context
            var connection = Configuration.GetConnectionString("DgMomentonDatabase");
            kernel.Bind<DataContext>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("options", 
                new DbContextOptionsBuilder<DataContext>().UseSqlServer(connection).Options);

            // Init the database
            var dataContext = kernel.Get<DataContext>();
            dataContext.Database.Migrate();
            await InitData(dataContext);

            // Cross-wire required framework services
            kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            return kernel;
        }

        #endregion

        #endregion
        #region InitData

        /// <summary>
        /// Initialize the data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task InitData(DataContext context)
        {
            if (context.Employees.Any())
                return;

            context.Add(new Employee { Id = 100, Name = "Allan", ManagerId = 150 });
            context.Add(new Employee { Id = 220, Name = "Martin", ManagerId = 100 });
            context.Add(new Employee { Id = 150, Name = "Jamie" });
            context.Add(new Employee { Id = 275, Name = "Alex", ManagerId = 100 });
            context.Add(new Employee { Id = 400, Name = "Steve", ManagerId = 150 });
            context.Add(new Employee { Id = 190, Name = "David", ManagerId = 400 });

            // Adding scenario where employee got invalid manager-id
            context.Add(new Employee { Id = 999, Name = "Nicolas", ManagerId = 888 });
            context.Add(new Employee { Id = 777, Name = "Carol", ManagerId = 999 });

            // Adding scenario where employee has no manager
            context.Add(new Employee { Id = 189, Name = "Claudia" });

            await context.SaveChangesAsync();
        }

        #endregion
        #region ctor

        /// <summary>
        /// The Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion
        #region Members

        public IConfiguration Configuration { get; }

        #endregion
        #region ConfigureServices

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collections</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Ninject
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());
            services.AddCustomControllerActivation(Resolve);
            services.AddCustomViewComponentActivation(Resolve);
        }

        #endregion
        #region Configure

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder concrete</param>
        /// <param name="env">The hosting environment concrete</param>
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Ninject registration
            this.Kernel = await this.RegisterApplicationComponents(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        #endregion
    }

    #endregion
}
