using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Bo.Web.UI.Framework;

/// <summary>
///     Every module's startup must inherit for this interface
/// </summary>
public interface IModuleStartup
{
    /// <summary>
    ///     Configure services for a module
    /// </summary>
    /// <param name="services"></param>
    void ConfigureServices(IServiceCollection services);

    /// <summary>
    ///     Configure application builder for a module
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    void Configure(IApplicationBuilder app, IWebHostEnvironment env);
}