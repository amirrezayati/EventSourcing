using MediatR;

namespace EventSourcing.Application.Commands.CatalogItems;

public class DeleteCatalogItemCommand : INotification
{
    public DeleteCatalogItemCommand(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}