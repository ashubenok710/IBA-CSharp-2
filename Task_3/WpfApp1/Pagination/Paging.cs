using System.Data;
using WpfApp1.DataModel;

namespace WpfApp1.Pagination
{
    public class Paging
    {
        public int PageIndex { get; set; }

        DataTable PagedList = new DataTable();

        public DataTable Next(IList<Person> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Previous(IList<Person> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable First(IList<Person> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Last(IList<Person> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable SetPaging(IList<Person> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Person> PagedList = new List<Person>();

            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();             

            DataTable FinalPaging = PagedTable(PagedList);

            return FinalPaging;
        }

        private DataTable PagedTable<T>(IList<T> SourceList)
        {
            Type columnType = typeof(T);
            DataTable TableToReturn = new DataTable();

            foreach (var Column in columnType.GetProperties())
            {
                TableToReturn.Columns.Add(Column.Name, Column.PropertyType);
            }

            foreach (object item in SourceList)
            {
                DataRow ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties())
                {
                    ReturnTableRow[Column.Name] = Column.GetValue(item);
                }
                TableToReturn.Rows.Add(ReturnTableRow);
            }
            return TableToReturn;
        }
    }
}
