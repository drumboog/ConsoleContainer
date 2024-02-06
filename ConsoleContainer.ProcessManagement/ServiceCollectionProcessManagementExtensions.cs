using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContainer.ProcessManagement
{
    public static class ServiceCollectionProcessManagementExtensions
    {
        public static IServiceCollection AddProcessManagement(this IServiceCollection services)
        {
            services.AddSingleton<IProcessManager, ProcessManager>();
            services.AddTransient<IProcessWrapperFactory, ProcessWrapperFactory>();
            return services;
        }
    }
}
