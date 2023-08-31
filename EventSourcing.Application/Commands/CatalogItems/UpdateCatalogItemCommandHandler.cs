using EventSourcing.Application.Common.Interfaces;
using EventSourcing.Domain.Entities;
using MediatR;

namespace EventSourcing.Application.Commands.CatalogItems;

public class UpdateCatalogItemCommandHandler : INotificationHandler<UpdateCatalogItemCommand>
{
    private readonly IAggregateRepository<CatalogItem, Guid> _aggregateRepository;
    private readonly ICatalogItemRepository _catalogItemRepository;
    public UpdateCatalogItemCommandHandler(IAggregateRepository<CatalogItem, Guid> aggregateRepository,
        ICatalogItemRepository catalogItemRepository)
    {
        _aggregateRepository = aggregateRepository;
        _catalogItemRepository = catalogItemRepository;
    }

    #region INotificationHandler implementation
    public async Task Handle(UpdateCatalogItemCommand notification, CancellationToken cancellationToken)
    {
        var catalogItem = await _aggregateRepository.RehydrateAsync(notification.CatalogItemId, cancellationToken);

        if (catalogItem is null)
        {
            throw new Exception("Invalid catalog item information");
        }

        catalogItem.Update(notification.CatalogItemId, notification.Name, notification.Description, notification.Price,
            notification.AvailableStock, notification.RestockThreshold, notification.MaxStockThreshold, notification.OnReorder);

        // Save data into store
        await _aggregateRepository.AppendAsync(catalogItem, cancellationToken);

        // Save data into database
        await _catalogItemRepository.UpdateAsync(catalogItem);
    }

    #endregion
}