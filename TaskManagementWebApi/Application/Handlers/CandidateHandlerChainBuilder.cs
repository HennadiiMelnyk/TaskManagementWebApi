using TaskManagementWebApi.Application.Handlers.Interfaces;

namespace TaskManagementWebApi.Application.Handlers;

public class CandidateHandlerChainBuilder
{
    private readonly IServiceProvider _provider;

    public CandidateHandlerChainBuilder(IServiceProvider provider)
    {
        _provider = provider;
    }

    public ICandidateSelectionHandler Build()
    {
        var handlers = _provider.GetServices<ICandidateSelectionHandler>().ToArray();

        if (handlers.Length == 0)
            throw new InvalidOperationException("No candidate selection handlers registered.");

        for (int i = 0; i < handlers.Length - 1; i++)
        {
            handlers[i].SetNext(handlers[i + 1]);
        }

        return handlers[0]; // Head
    }
}
