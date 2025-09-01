using Microsoft.Extensions.Hosting;

namespace AzagraBank.EventBus;

public static class Extensions
{
    public static TBuilder AddKafkaProducerWithBaseSettings<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddKafkaProducer<string, string>(
            "kafka",
            static settings =>
            {
                settings.Config.AllowAutoCreateTopics = true;
            });

        return builder;
    }

    public static TBuilder AddKafkaConsumerWithBaseSettings<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddKafkaConsumer<string, string>(
            "kafka",
            static settings =>
            {
                settings.Config.GroupId = Guid.NewGuid().ToString();
                settings.Config.AllowAutoCreateTopics = true;
            });

        return builder;
    }
}
