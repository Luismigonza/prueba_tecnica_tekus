namespace ProviderServices.Application.DTOs;

public class PagedFilter
{
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class ProviderFilter : PagedFilter
{
    public string? Country { get; set; }
}

public class ServiceFilter : PagedFilter
{
}