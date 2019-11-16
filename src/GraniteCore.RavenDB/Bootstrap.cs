using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using System;

namespace GraniteCore.RavenDB
{
    public static class Bootstrap
    {
        public static void AddGraniteRavenDB(
           this IServiceCollection services,
           Func<IDocumentStore> createStore
           )
        {
            if (createStore == null)
                throw new ArgumentNullException(nameof(createStore));

            // This could be better... first pass
            var store = createStore.Invoke();
            store.Initialize();
            services.AddSingleton(typeof(IDocumentStore), store);
            services.AddScoped(typeof(IBaseRepository<,,>), typeof(RavenDBBaseRepository<,,,>));
        }
    }
}
