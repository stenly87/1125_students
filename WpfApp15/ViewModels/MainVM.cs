using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp15.Pages;

namespace WpfApp15.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        private Page currentPage;

        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal(nameof(CurrentPage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void Signal(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public MainVM()
        {
            CurrentPage = new OptionPage();

        }
    }
}
