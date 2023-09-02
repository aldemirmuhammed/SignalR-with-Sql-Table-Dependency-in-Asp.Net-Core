using SignalR_SqlTableDependency.SubscribeTableDependencies;

namespace SignalR_SqlTableDependency.Middleware
{
    public static class ApplicationBuilderExtension
    {

        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString)
            where T : ISubscribeTableDependency
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<T>();
            if (service != null)
            {
                service.SubscribeTableDependency(connectionString);
            }
        }


      
    }
}
