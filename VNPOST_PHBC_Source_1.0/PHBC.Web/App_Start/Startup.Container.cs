namespace PHBC.Web
{
    using System.Reflection;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using PHBC.Web.Services;
    using Owin;
    using PHBC.DAO.Bussiness;

    /// <summary>
    /// Register types into the Autofac Inversion of Control (IOC) container.
    /// </summary>
    public partial class Startup
    {
        public static void ConfigureContainer(IAppBuilder app)
        {
            IContainer container = CreateContainer();
            app.UseAutofacMiddleware(container);

            // Register MVC Types 
            app.UseAutofacMvc();
        }

        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            RegisterServices(builder);
            RegisterMvc(builder, assembly);

            IContainer container = builder.Build();

            SetMvcDependencyResolver(container);

            return container;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerRequest();
            builder.RegisterType<FeedService>().As<IFeedService>().InstancePerRequest();
            builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();
            builder.RegisterType<OpenSearchService>().As<IOpenSearchService>().InstancePerRequest();
            builder.RegisterType<RobotsService>().As<IRobotsService>().InstancePerRequest();
            builder.RegisterType<SitemapService>().As<ISitemapService>().InstancePerRequest();
            builder.RegisterType<SitemapPingerService>().As<ISitemapPingerService>().InstancePerRequest();

            //custom
            builder.RegisterType<SysActionBussiness>().As<ISysActionBussiness>().InstancePerRequest();
            builder.RegisterType<RoleBussiness>().As<IRoleBussiness>().InstancePerRequest();
            builder.RegisterType<SysDMTypeBussiness>().As<ISysDMTypeBussiness>().InstancePerRequest();
            builder.RegisterType<SysDMPublicBussiness>().As<ISysDMPublicBussiness>().InstancePerRequest();
            builder.RegisterType<UserBussiness>().As<IUserBussiness>().InstancePerRequest();
            builder.RegisterType<DMToaSoanBussiness>().As<IDMToaSoanBussiness>().InstancePerRequest();
            builder.RegisterType<MenuBussiness>().As<IMenuBussiness>().InstancePerRequest();
            builder.RegisterType<DMDiemInBussiness>().As<IDMDiemInBussiness>().InstancePerRequest();
            builder.RegisterType<DMLoaiAnPhamBussiness>().As<IDMLoaiAnPhamBussiness>().InstancePerRequest();
            builder.RegisterType<BPHNCBussiness>().As<IBPHNCBussiness>().InstancePerRequest();
            builder.RegisterType<ThongTinBaoBussiness>().As<IThongTinBaoBussiness>().InstancePerRequest();
            builder.RegisterType<KeHoachXuatBanBussiness>().As<IKeHoachXuatBanBussiness>().InstancePerRequest();
            builder.RegisterType<DieuChinhKHXBBussiness>().As<IDieuChinhKHXBBussiness>().InstancePerRequest();
            builder.RegisterType<BDieuChinhPHNCBussiness>().As<IBDieuChinhPHNCBussiness>().InstancePerRequest();
            builder.RegisterType<ThongTinGiaBaoBussiness>().As<IThongTinGiaBaoBussiness>().InstancePerRequest();
            builder.RegisterType<UnitBussiness>().As<IUnitBussiness>().InstancePerRequest();
            builder.RegisterType<SysLibraryBussiness>().As<ISysLibraryBussiness>().InstancePerRequest();
            builder.RegisterType<BDiemTiepNhanBussiness>().As<IBDiemTiepNhanBussiness>().InstancePerRequest();
        }

        private static void RegisterMvc(ContainerBuilder builder, Assembly assembly)
        {
            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(assembly);
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetMvcDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}