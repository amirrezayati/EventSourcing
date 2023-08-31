using EventSourcing.Application.Common.Interfaces;
using EventSourcing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.Infrastructure.Persistence;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _context;

    public CatalogItemRepository(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }
    public async Task AddAsync(CatalogItem catalogItem)
    {
        await _context.AddAsync(catalogItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CatalogItem catalogItem)
    {
        _context.Entry(catalogItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}