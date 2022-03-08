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
    class MainVM : BaseVM
    {
        CurrentPageControl currentPageControl;

        public Page CurrentPage
        {
            get => currentPageControl.Page;
        }

        public CommandVM CreateGroup { get; set; }
        public CommandVM ViewGroups { get; set; }
        public CommandVM CreateStudent { get; set; }
        public CommandVM ViewStudents { get; set; }

        public CommandVM StudentValues { get; set; }
        public CommandVM ViewStudentValues { get; set; }

        public MainVM()
        {
            currentPageControl = new CurrentPageControl();
            currentPageControl.PageChanged += CurrentPageControl_PageChanged;
            currentPageControl.SetPage(new OptionPage());
            CreateGroup = new CommandVM(() => {
                currentPageControl.SetPage(new EditGroupPage(new EditGroupVM(currentPageControl)));
            });
            ViewGroups = new CommandVM(() => {
                currentPageControl.SetPage(new ViewGroupsPage());
            });
            CreateStudent = new CommandVM(() => {
                currentPageControl.SetPage(new EditStudentPage(new EditStudentVM(currentPageControl)));
            });
            ViewStudents = new CommandVM(()=> {
                currentPageControl.SetPage(new ViewStudentsPage(null));
            });

            StudentValues = new CommandVM(()=> {
                currentPageControl.SetPage(new AddValuesPage());
            });

            ViewStudentValues = new CommandVM(() =>
            {
                currentPageControl.SetPage(new ViewValuesPage());
            });
        }

        private void CurrentPageControl_PageChanged(object sender, EventArgs e)
        {
            Signal(nameof(CurrentPage));
        }
    }
}
