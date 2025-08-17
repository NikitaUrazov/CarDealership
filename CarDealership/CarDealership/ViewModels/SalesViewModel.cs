using CarDealership.Data;
using CarDealership.Dtos;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace CarDealership.ViewModels
{
    public class SalesViewModel : PagedViewModel
    {
        #region Fields

        private int _selectedYear;

        protected List<Order> _ordersRaw;
        protected HashSet<int> _loadedYears;

        /// <summary>
        /// Номер колонки в Excel для каждого месяца.
        /// </summary>
        private enum ExcelColumns
        {
            ModelName = 1,
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        #endregion

        #region Properties

        public ObservableCollection<SaleDto> Sales { get; set; }
        public ObservableCollection<int> AvailableYears { get; set; }
        public RelayCommand ExportCmd { get; protected set; }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    CurrentPage = 1;   // сброс страницы при смене года
                    LoadSalesPageByYear(_selectedYear);
                }
            }
        }

        #endregion

        #region Ctr

        public SalesViewModel()
        {
            _loadedYears = new HashSet<int>();

            _ordersRaw = new List<Order>();
            Sales = new ObservableCollection<SaleDto>();
            AvailableYears = new ObservableCollection<int>();

            NextPageCmd = new RelayCommand(_ => { CurrentPage++; LoadSalesPageByYear(SelectedYear); }, _ => CurrentPage < TotalPages);
            PrevPageCmd = new RelayCommand(_ => { CurrentPage--; LoadSalesPageByYear(SelectedYear); }, _ => CurrentPage > 1);
            ExportCmd = new RelayCommand(_ => { ExportToExcel(); });


            LoadAvailableYears();
            SelectedYear = AvailableYears.Max();
            LoadSalesPageByYear(SelectedYear);
        }

        #endregion

        #region Methods

        private void LoadOrdersRawByYear(int year)
        {
            if (_loadedYears.Contains(year))
                return;

            using var context = new ApplicationDbContext();
            var orders = context.Orders
                .Where(o => o.OrderDate.Year == year)
                .Include(o => o.CarConfiguration)
                    .ThenInclude(c => c.Model)
                        .ThenInclude(m => m.Brand)
                .ToList();

            _ordersRaw.AddRange(orders);
            _loadedYears.Add(year);
        }

        private void LoadSalesPageByYear(int year)
        {
            var query = GetSalesByYear(year);

            CalculateTotalPages(query.Count());

            List<SaleDto> sales = query
                        .OrderBy(r => r.ModelName)
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();

            Sales.Clear();
            foreach (var sale in sales)
                Sales.Add(sale);

        }

        private IEnumerable<SaleDto> GetSalesByYear(int year)
        {
            LoadOrdersRawByYear(year);

            var query = _ordersRaw
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => new { Brand = o.CarConfiguration.Model.Brand.Name, Model = o.CarConfiguration.Model.Name })
                .Select(g => new SaleDto
                {
                    ModelName = g.Key.Brand + " " + g.Key.Model,
                    January = g.Where(o => o.OrderDate.Month == 1).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    February = g.Where(o => o.OrderDate.Month == 2).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    March = g.Where(o => o.OrderDate.Month == 3).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    April = g.Where(o => o.OrderDate.Month == 4).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    May = g.Where(o => o.OrderDate.Month == 5).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    June = g.Where(o => o.OrderDate.Month == 6).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    July = g.Where(o => o.OrderDate.Month == 7).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    August = g.Where(o => o.OrderDate.Month == 8).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    September = g.Where(o => o.OrderDate.Month == 9).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    October = g.Where(o => o.OrderDate.Month == 10).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    November = g.Where(o => o.OrderDate.Month == 11).Sum(o => o.CarConfiguration.Price * o.Quantity),
                    December = g.Where(o => o.OrderDate.Month == 12).Sum(o => o.CarConfiguration.Price * o.Quantity),
                });

            return query;
        }

        private void LoadAvailableYears()
        {
            using var context = new ApplicationDbContext();
            var years = context.Orders
                .Select(o => o.OrderDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            AvailableYears.Clear();
            foreach (var y in years)
                AvailableYears.Add(y);
        }

        private void ExportToExcel()
        {

        }

        private void ExportToExcel(IEnumerable<SaleDto> sales, int year, string filePath)
        {
            string fullName = "UrazovNikita";
            ExcelPackage.License.SetNonCommercialPersonal(fullName);

            const int HeaderRow = 1;
            const int ColumnCount = 13; // 12 месяцев + модель

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"Продажи {year}");

                worksheet.Cells[HeaderRow, (int)ExcelColumns.ModelName].    Value = "Модель";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.January].      Value = "Январь";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.February].     Value = "Февраль";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.March].        Value = "Март";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.April].        Value = "Апрель";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.May].          Value = "Май";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.June].         Value = "Июнь";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.July].         Value = "Июль";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.August].       Value = "Август";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.September].    Value = "Сентябрь";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.October].      Value = "Октябрь";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.November].     Value = "Ноябрь";
                worksheet.Cells[HeaderRow, (int)ExcelColumns.December].     Value = "Декабрь";

                using (var range = worksheet.Cells[HeaderRow, 1, HeaderRow, ColumnCount])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                int row = HeaderRow + 1;
                foreach (var sale in sales)
                {
                    worksheet.Cells[row, (int)ExcelColumns.ModelName]   .Value = sale.ModelName;
                    worksheet.Cells[row, (int)ExcelColumns.January]     .Value = sale.January;
                    worksheet.Cells[row, (int)ExcelColumns.February]    .Value = sale.February;
                    worksheet.Cells[row, (int)ExcelColumns.March]       .Value = sale.March;
                    worksheet.Cells[row, (int)ExcelColumns.April]       .Value = sale.April;
                    worksheet.Cells[row, (int)ExcelColumns.May]         .Value = sale.May;
                    worksheet.Cells[row, (int)ExcelColumns.June]        .Value = sale.June;
                    worksheet.Cells[row, (int)ExcelColumns.July]        .Value = sale.July;
                    worksheet.Cells[row, (int)ExcelColumns.August]      .Value = sale.August;
                    worksheet.Cells[row, (int)ExcelColumns.September]   .Value = sale.September;
                    worksheet.Cells[row, (int)ExcelColumns.October]     .Value = sale.October;
                    worksheet.Cells[row, (int)ExcelColumns.November]    .Value = sale.November;
                    worksheet.Cells[row, (int)ExcelColumns.December]    .Value = sale.December;

                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                package.SaveAs(new FileInfo(filePath));
            }
        }

        #endregion
    }
}
