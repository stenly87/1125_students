using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp15.Pages;
using WpfApp15.Tools;

namespace WpfApp15.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        CurrentPageControl currentPageControl;

        public Page CurrentPage
        {
            get => currentPageControl.Page;
        }

        public CommandVM CreateGroup { get; set; }
        public CommandVM ViewGroups { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void Signal(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public MainVM()
        {
            currentPageControl = new CurrentPageControl();
            currentPageControl.PageChanged += CurrentPageControl_PageChanged;
            currentPageControl.SetPage(new OptionPage());
            CreateGroup = new CommandVM(()=> {
                currentPageControl.SetPage(new EditGroupPage(new EditGroupVM(currentPageControl)));
            });
            ViewGroups = new CommandVM(()=> {
                currentPageControl.SetPage(new ViewGroupsPage());
            });
        }

        private void CurrentPageControl_PageChanged(object sender, EventArgs e)
        {
            Signal(nameof(CurrentPage));
        }
    }
}
