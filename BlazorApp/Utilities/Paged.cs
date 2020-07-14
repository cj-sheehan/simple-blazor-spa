using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Utilities
{
    public class Paged<T>
    {
        private IEnumerable<T> _items;
        public int PageSize { get; private set; }
        public int ItemCount { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }

        public int CurrentStartIndex { get; private set; }
        public int CurrentEndIndex { get; private set; }
        public IEnumerable<T> PagedItems { get; private set; }

        public string PagingDescription => $"Showing {CurrentStartIndex + 1}-{CurrentEndIndex + 1} of {ItemCount} items.";
        public bool HasPreviousPage => CurrentPage != 1;
        public bool HasNextPage => CurrentPage != TotalPages;

        public Paged(IEnumerable<T> items, int pageSize = 10, int initialPage = 1)
        {
            SetInitialValues(items, pageSize, initialPage);

            CalculatePageIndexes();
            PageItems();
        }

        public void PageNext()
        {
            var newPage = CurrentPage + 1;
            if (newPage <= TotalPages)
            {
                CurrentPage = newPage;
                CalculatePageIndexes();
                PageItems();
            }
        }

        public void PagePrevious()
        {
            if (CurrentPage != 1)
            {
                CurrentPage--;
                CalculatePageIndexes();
                PageItems();
            }
        }

        private void SetInitialValues(IEnumerable<T> items, int pageSize, int initialPage)
        {
            _items = items;
            PageSize = pageSize < 1 ? 1 : pageSize;
            ItemCount = items.Count();
            TotalPages = (int)Math.Ceiling(1M * ItemCount / PageSize);

            CurrentPage = initialPage < 1 ? 1 : initialPage;
            CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;
        }

        private void CalculatePageIndexes()
        {
            CurrentStartIndex = (CurrentPage - 1) * PageSize;
            CurrentEndIndex = Math.Min(CurrentStartIndex + PageSize - 1, ItemCount - 1);
        }

        private void PageItems()
        {
            PagedItems = _items.Skip(CurrentStartIndex).Take(CurrentEndIndex - CurrentStartIndex + 1);
        }
    }
}
