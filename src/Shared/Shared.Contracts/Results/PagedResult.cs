namespace Shared.Contracts.Pagination;

/// <summary>
/// Represents a paginated collection of items with metadata for API responses.
/// This is a pure DTO without any business logic, designed for consistent pagination responses across APIs.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// The current page of items.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// The 1-based index of the current page.
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// The maximum number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// The total number of pages available.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Indicates whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates whether there is a next page available.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Creates a new instance of PagedResult.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="count">The total number of items across all pages.</param>
    /// <param name="pageNumber">The 1-based index of the current page.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    public PagedResult(IReadOnlyList<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    /// <summary>
    /// Creates a new instance of PagedResult.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="count">The total number of items across all pages.</param>
    /// <param name="pageNumber">The 1-based index of the current page.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    /// <returns>A new PagedResult instance.</returns>
    public static PagedResult<T> Create(IReadOnlyList<T> items, int count, int pageNumber, int pageSize)
        => new(items, count, pageNumber, pageSize);
}
