using CarDealership.Data;
using CarDealership.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarDealership.Dtos;

namespace CarDealership.ViewModels
{
    public class ModelViewModel : PagedViewModel
    {
        #region Prperties

        protected List<Model> _modelsRaw;

        #endregion

        #region Fields

        public ObservableCollection<ModelDto> Models { get; set; }

        #endregion

        public ModelViewModel()
        {
            _modelsRaw = new List<Model>();
            Models = new ObservableCollection<ModelDto>();

            NextPageCmd = new RelayCommand(
                _ =>
                {
                    CurrentPage++;
                    LoadModels();
                },
                _ => CurrentPage < TotalPages);

            PrevPageCmd = new RelayCommand(
                _ =>
                {
                    CurrentPage--;
                    LoadModels();
                },
                _ => CurrentPage > 1);

            LoadModelsRaw();
            CalculateTotalPages();
            LoadModels();
        }

        #region Methods

        private void CalculateTotalPages() => CalculateTotalPages(_modelsRaw.Count());

        //TODO: async version
        private void LoadModelsRaw()
        {
            using var context = new ApplicationDbContext();
            _modelsRaw = context.Models
                .Include(m => m.Brand)
                .OrderBy(b => b.Id)
                .ToList();
        }

        private void LoadModels()
        {
            var models = _modelsRaw
                .OrderBy(b => b.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .Select(m => new ModelDto
                {
                    Id = m.Id,
                    BrandName = m.Brand.Name,
                    Name = m.Name
                })
                .ToList();

            Models.Clear();
            foreach (var model in models)
                Models.Add(model);
        }

        #endregion
    }
}
