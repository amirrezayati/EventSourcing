using EventSourcing.Application.Common.Interfaces;
using EventSourcing.Domain.Entities;
using MediatR;

namespace EventSourcing.Application.Commands.CatalogItems;

public class DeleteCatalogItemCommandHandler : INotificationHandler<DeleteCatalogItemCommand>
{
    private readonly IAggregateRepository<CatalogItem, Guid> _aggregateRepository;
    private readonly ICatalogItemRepository _catalogItemRepository;

    public DeleteCatalogItemCommandHandler(IAggregateRepository<CatalogItem, Guid> aggregateRepository, ICatalogItemRepository catalogItemRepository)
    {
        _aggregateRepository = aggregateRepository;
        _catalogItemRepository = catalogItemRepository;
    }

    #region INotificationHandler implementation
    public async Task Handle(DeleteCatalogItemCommand notification, CancellationToken cancellationToken)
    {
        var catalogItem = await _aggregateRepository.RehydrateAsync(notification.Id, cancellationToken);
        if (catalogItem == null)
        {
            throw new Exception("Invalid Catalog Item information");
        }

        catalogItem.Delete(catalogItem.Id);
        await _aggregateRepository.AppendAsync(catalogItem, cancellationToken);

        // Save data into database
        await _catalogItemRepository.UpdateAsync(catalogItem);
    }
    #endregion
}