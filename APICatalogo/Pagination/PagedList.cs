namespace APICatalogo.Pagination
{
    public class PagedList<T>: List<T> where T : class
    {
        //Número da página atual
        public int CurrentPage { get; private set; }
        //Total de páginas
        public int TotalPages { get; private set; }
        // Tamanho de items que estará na página
        public int PageSize { get; private set; }
        //Número total de elementos da fonte de dados
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);

        }


    }
}
