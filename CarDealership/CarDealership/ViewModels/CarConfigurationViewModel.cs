using CarDealership.Data;
using CarDealership.Dtos;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.ViewModels
{
    public class CarConfigurationViewModel : PagedViewModel
    {
        #region Prperties

        protected List<CarConfiguration> _carConfigurationsRaw;

        #endregion

        #region Fields

        public ObservableCollection<CarConfigurationDto> CarConfigurations{ get; set; }

        #endregion

        public CarConfigurationViewModel()
        {
            _carConfigurationsRaw = new List<CarConfiguration>();
            CarConfigurations = new ObservableCollection<CarConfigurationDto>();

            NextPageCmd = new RelayCommand(
                _ =>
                {
                    CurrentPage++;
                    LoadCarConfigurations();
                },
                _ => CurrentPage < TotalPages);

            PrevPageCmd = new RelayCommand(
                _ =>
                {
                    CurrentPage--;
                    LoadCarConfigurations();
                },
                _ => CurrentPage > 1);

            LoadCarConfigurationsRaw();
            CalculateTotalPages();
            LoadCarConfigurations();
        }

        #region Methods
        private void CalculateTotalPages() => CalculateTotalPages(_carConfigurationsRaw.Count());
        private void LoadCarConfigurationsRaw()
        {
            using var context = new ApplicationDbContext();

            _carConfigurationsRaw = context.CarConfigurations
            .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
            .OrderBy(b => b.Id)
            .ToList();
        }

        private void LoadCarConfigurations()
        {
            var carConfigurations = _carConfigurationsRaw
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new CarConfigurationDto
                {
                    Id = c.Id,
                    ModelName = c.Model.Name,
                    BrandName = c.Model.Brand.Name,
                    Color = c.Color,
                    Engine = c.Engine,
                    Package = c.Package,
                    Price = c.Price
                })
                .ToList();

            CarConfigurations.Clear();
            foreach (var carConfig in carConfigurations)
                CarConfigurations.Add(carConfig);
        }

        #endregion
    }
}
