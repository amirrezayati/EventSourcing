﻿using MediatR;

namespace EventSourcing.Application.Commands.CatalogItems;

public class UpdateCatalogItemCommand : INotification
{
    // Update Catalog Item Command
    public UpdateCatalogItemCommand(Guid id, string name, string description, double price, int availableStock, int restockThreshold,
        int maxStockThreshold, bool onReorder)
    {
        CatalogItemId = id;
        Name = name;
        Description = description;
        Price = price;
        AvailableStock = availableStock;
        RestockThreshold = restockThreshold;
        MaxStockThreshold = maxStockThreshold;
        OnReorder = onReorder;
    }

    public Guid CatalogItemId { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public double Price { get; private set; }

    // Quantity in stock
    public int AvailableStock { get; private set; }

    // Available stock at which we should reorder
    public int RestockThreshold { get; private set; }

    // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
    public int MaxStockThreshold { get; private set; }

    /// <summary>
    /// True if item is on reorder
    /// </summary>
    public bool OnReorder { get; private set; }

}