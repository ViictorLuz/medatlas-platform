using Amazon.S3;
using MedAtlas.Domain.Modules.Library.Interfaces;
using MedAtlas.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace MedAtlas.Infrastructure.DependencyInjection;

public static class InjetorDeDependencias
{
    public static IServiceCollection AdicionarInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        var secaoAws = configuration.GetSection("AWS");

        var configS3 = new AmazonS3Config
        {
            ServiceURL = secaoAws["ServiceURL"],
            ForcePathStyle = secaoAws.GetValue<bool>("ForcePathStyle")
        };

        services.AddSingleton<IAmazonS3>(_ =>
            new AmazonS3Client(secaoAws["AccessKey"], secaoAws["SecretKey"], configS3));

        services.AddScoped<IStorageService, MinIoStorageService>();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var secaoBroker = configuration.GetSection("MessageBroker");

                cfg.Host(secaoBroker["Host"] ?? "localhost", "/", h =>
                {
                    h.Username(secaoBroker["Usuario"] ?? "guest");
                    h.Password(secaoBroker["Senha"] ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}