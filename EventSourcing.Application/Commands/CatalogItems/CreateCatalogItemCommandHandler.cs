using EventSourcing.Application.Common.Interfaces;
using EventSourcing.Domain.Entities;
using MediatR;

namespace EventSourcing.Application.Commands.CatalogItems;

public class CreateCatalogItemCommandHandler : INotificationHandler<CreateCatalogItemCommand>
{
    private readonly IAggregateRepository<CatalogItem, Guid> _aggregateRepository;
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CreateCatalogItemCommandHandler(IAggregateRepository<CatalogItem, Guid> aggregateRepository,
        ICatalogItemRepository catalogItemRepository)
    {
        _aggregateRepository = aggregateRepository;
        _catalogItemRepository = catalogItemRepository;
    }

    #region INotificationHandler implementation
    public async Task Handle(CreateCatalogItemCommand notification, CancellationToken cancellationToken)
    {
        // Insert event into event store db
        var catalogItem = CatalogItem.Create(notification.Name, notification.Description, notification.Price, notification.AvailableStock,
            notification.RestockThreshold, notification.MaxStockThreshold, notification.OnReorder);
        await _aggregateRepository.AppendAsync(catalogItem, cancellationToken);

        // Save data into database
        //await _catalogItemAggregateRepository.SaveAsync(catalogItem.Events.FirstOrDefault());
        await _catalogItemRepository.AddAsync(catalogItem);

        // Dispatch events to any event/service bus to do next actions
        // We can also register EventStore db Subscription to receive Event Notification
    }
    #endregion
}