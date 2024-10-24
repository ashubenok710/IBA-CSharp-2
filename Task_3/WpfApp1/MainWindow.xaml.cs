using ClosedXML.Excel;
using Microsoft.Win32;
using System.Data;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.DataModel;
using WpfApp1.Model;
using WpfApp1.Pagination;
using System.Runtime.Serialization;
using System.Xml;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {

        private readonly DBPersonContext DBContext = new DBPersonContext();        

        private int NumberOfRecPerPage = 10; 

        IList<Person> PeopleList;

        static Paging PagedTable = new Paging();

        public MainWindow()
        {
            InitializeComponent();
            SetButtons(false);            
        }

        private void SetButtons(Boolean value) 
        {
            btnFirstPage.IsEnabled = value;
            btnPreviousPage.IsEnabled = value;
            btnNextPage.IsEnabled = value;
            btnLastPage.IsEnabled = value;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"d:\WORK\C#\IBA_CSharp_2\Task_3\WpfApp1\";
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                await LoadAsync(openFileDialog.FileName);
            }
        }

        private async Task LoadAsync(string FileName)
        {
            using (FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string headerLine = sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(';');

                    var person = new Person
                    {
                        Date = DateTime.ParseExact(words[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),                        
                        FirstName = words[1],
                        Surname = words[2],
                        LastName = words[3],
                        City = words[4],    
                        Country = words[5],
                    };

                    DBContext.People.Add(person);
                }
                await DBContext.SaveChangesAsync();
            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = PagedTable.First(PeopleList, NumberOfRecPerPage);
            table.DefaultView.AllowNew = false;
            DisplayGrid.ItemsSource = table.DefaultView;
            
            PageInfo.Content = PageNumberDisplay();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var selectedDate = datePicker.SelectedDate;                       

            PeopleList = DBContext.People.Where(x => 
                (string.IsNullOrWhiteSpace(x.FirstName) || x.FirstName.Contains(txtFirstName.Text)) &&
                (selectedDate == null || (x.Date == selectedDate)) &&            
                (string.IsNullOrWhiteSpace(x.Country) || x.Country.Contains(txtCountry.Text)) && 
                (string.IsNullOrWhiteSpace(x.City) || x.City.Contains(txtCity.Text)))
                    .OrderBy(b => b.City)
                    .ThenBy(b => b.FirstName)
                    .AsQueryable()
                    .ToList();

            DataTable table = PagedTable.SetPaging(ConvertToListOf<Person>(PeopleList.ToList()), NumberOfRecPerPage);
            table.DefaultView.AllowNew = false; 
            DisplayGrid.ItemsSource = table.DefaultView;

            PageInfo.Content = PageNumberDisplay();

            SetButtons(PeopleList.Count > 0 ? true : false); 
        }

        private static IList<T> ConvertToListOf<T>(IList iList)
        {
            IList<T> result = new List<T>();

            foreach (T value in iList)
                result.Add(value);

            return result;
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ExportXLS_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel |*.xlsx";
            saveFileDialog1.Title = "Save Excel File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var workbook = new XLWorkbook();                
                
                var dataTable = DbContextExtensions.ToDataTable(PeopleList.ToList());

                IXLWorksheet worksheet = workbook.AddWorksheet(dataTable);
                worksheet.Name = "People";

                workbook.SaveAs(saveFileDialog1.FileName + ".xlsx");
            }
        }

        private void ExportXML_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML |*.xml";
            saveFileDialog1.Title = "Save XML File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var people = PeopleList.ToList();
                //https://stackoverflow.com/questions/6234290/serialize-entity-framework-object-with-children-to-xml-file
                using (XmlWriter writer = XmlWriter.Create(saveFileDialog1.FileName + ".xml"))
                {
                    DataContractSerializer serializer = new DataContractSerializer(people.GetType());
                    serializer.WriteObject(writer, people);
                    writer.Close();
                }
            }
        }


        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = PagedTable.Last(PeopleList, NumberOfRecPerPage);
            table.DefaultView.AllowNew = false;
            DisplayGrid.ItemsSource = table.DefaultView;
            
            PageInfo.Content = PageNumberDisplay();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = PagedTable.Next(PeopleList, NumberOfRecPerPage);
            table.DefaultView.AllowNew = false;
            DisplayGrid.ItemsSource = table.DefaultView;
            
            PageInfo.Content = PageNumberDisplay();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = PagedTable.Previous(PeopleList, NumberOfRecPerPage);
            table.DefaultView.AllowNew = false;
            DisplayGrid.ItemsSource = table.DefaultView;
            
            PageInfo.Content = PageNumberDisplay();
        }

        public string PageNumberDisplay()
        {
            int PagedNumber = NumberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > PeopleList.Count)
            {
                PagedNumber = PeopleList.Count;
            }
            return PagedNumber + " of " + PeopleList.Count;
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = null;
            txtFirstName.Text = "";
            txtCountry.Text = "";
            txtCity.Text = "";
        }
    }
}