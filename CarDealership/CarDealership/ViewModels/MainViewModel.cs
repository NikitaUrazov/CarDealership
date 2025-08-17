using CarDealership.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarDealership.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private object _currentView;

        private object _tablesView;
        private object _salesView;

        #endregion

        #region Fields

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public RelayCommand ShowTablesCommand { get; }
        public RelayCommand ShowSalesCommand { get; }

        #endregion

        #region Ctr

        public MainViewModel()
        {
            ShowTablesCommand = new RelayCommand(_ => CurrentView = _tablesView ?? new TablesView());
            ShowSalesCommand = new RelayCommand(_ => CurrentView = _salesView ?? new SalesView());

            _salesView = new SalesView();
            CurrentView = _salesView;
        }

        #endregion
    }
}
