using CarDealership.Data;
using CarDealership.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarDealership.ViewModels
{
    public class PagedViewModel : BaseViewModel
    {
        #region Properties

        private int _currentPage = 1;
        private int _totalPages;


        public const int PageSize = 30;

        #endregion

        #region Fields

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value && value > 0 && value <= _totalPages)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));

                    NextPageCmd?.RaiseCanExecuteChanged();
                    PrevPageCmd?.RaiseCanExecuteChanged();
                }
            }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            private set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        public RelayCommand NextPageCmd { get; protected set;}
        public RelayCommand PrevPageCmd { get; protected set; }

        #endregion

        #region Methods

        public void CalculateTotalPages(int elementsCount)
        {
            TotalPages = (elementsCount + PageSize - 1) / PageSize;
        }

        #endregion
    }
}
