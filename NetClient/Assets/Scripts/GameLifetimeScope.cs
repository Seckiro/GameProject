using VContainer;
using VContainer.Unity;
using MessagePipe;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MessagePipeOptions options = builder.RegisterMessagePipe(/* configure option */);
        options.InstanceLifetime = InstanceLifetime.Scoped;
        builder.RegisterBuildCallback(container =>
        {
            GlobalMessagePipe.SetProvider(container.AsServiceProvider());
        });

        builder.RegisterMessageBroker<MassagePipe, int>(options);

        builder.RegisterMessageBroker<GamePresenter, int>(options);

        builder.Register<MassagePipe>(Lifetime.Singleton);

        builder.Register<GamePresenter>(Lifetime.Singleton);

        builder.RegisterEntryPoint<MassagePipe>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GamePresenter>(Lifetime.Singleton);

    }
}
