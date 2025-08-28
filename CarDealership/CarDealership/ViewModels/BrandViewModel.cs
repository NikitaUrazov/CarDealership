using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CarDealership.Models;
using CarDealership.Data;
using System.Windows.Input;

namespace CarDealership.ViewModels
{
    public class BrandViewModel : PagedViewModel
    {
        #region Fields

        protected List<Brand> _brandsRaw;

        #endregion

        #region Properties

        //TODO: BrandDto?
        public ObservableCollection<Brand> Brands { get; set; }

        #endregion

        #region Ctr

        public BrandViewModel()
        {
            _brandsRaw = new List<Brand>();
            Brands = new ObservableCollection<Brand>();

            NextPageCmd = new RelayCommand(
                _ => 
                { 
                    CurrentPage++;
                    LoadBrands();
                },
                _ => CurrentPage < TotalPages);

            PrevPageCmd = new RelayCommand(
                _ => 
                {
                    CurrentPage--;
                    LoadBrands();
                },
                _ => CurrentPage > 1);

            LoadBrandsCache();
            CalculateTotalPages();
            LoadBrands();
        }

        #endregion

        #region Methods

        private void CalculateTotalPages() => CalculateTotalPages(_brandsRaw.Count());

        private void LoadBrandsCache()
        {
            using var context = new ApplicationDbContext();
            _brandsRaw = context.Brands
                .OrderBy(b => b.Id)
                .ToList();
        }

        private void LoadBrands()
        {
            var brands = _brandsRaw
                .OrderBy(b => b.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            Brands.Clear();
            foreach (var b in brands)
                Brands.Add(b);
        }

        #endregion
    }
}
