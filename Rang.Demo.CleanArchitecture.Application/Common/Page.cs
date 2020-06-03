using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.Common
{
    public class Page<T>
    {
        //properties
        public int PageNumber { get; }
        public int ItemsPerPage { get; }
        public ICollection<T> Items { get; }
        public int ItemsCount { get => Items.Count; }
        public long TotalItems { get; }
        public long TotalPages { get; }

        //constructors
        public Page(int pageNumber, int itemsPerPage, long totalPages, long totalItems, ICollection<T> items)
        {
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
            Items = items ?? new List<T>();
        }
    }
}
