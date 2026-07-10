using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Interfaces;
using ProviderServices.Domain.Entities;
using ProviderServices.Infrastructure.Persistence;

namespace ProviderServices.Infrastructure.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _context;

    public ProviderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Provider?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Providers.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> ExistsByNitAsync(string nit, CancellationToken ct = default)
    {
        return await _context.Providers.AnyAsync(p => p.Nit == nit, ct);
    }

    public async Task<(IReadOnlyList<Provider> Items, int Total)> GetAllAsync(ProviderFilter filter, CancellationToken ct = default)
    {
        var query = _context.Providers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(p =>
                p.Name.Contains(filter.Search) ||
                p.Nit.Contains(filter.Search) ||
                p.Email.Contains(filter.Search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Country))
            query = query.Where(p => p.Country == filter.Country);

        query = ApplySorting(query, filter.SortBy, filter.SortDescending);

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task AddAsync(Provider provider, CancellationToken ct = default)
    {
        await _context.Providers.AddAsync(provider, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

    private static IQueryable<Provider> ApplySorting(IQueryable<Provider> query, string? sortBy, bool descending)
    {
        Expression<Func<Provider, object>> keySelector = sortBy?.ToLowerInvariant() switch
        {
            "nit" => p => p.Nit,
            "country" => p => p.Country,
            "createdat" => p => p.CreatedAt,
            _ => p => p.Name
        };

        return descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }
}
