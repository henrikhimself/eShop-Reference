using EPiServer.Events;
using EPiServer.Events.Providers;

namespace Hj.Commerce.Features.Messaging;

internal class EmulatorEventProvider : EventProvider
{
  public override void SendMessage(EventMessage message)
  {
  }

  public override Task SendMessageAsync(EventMessage message, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
