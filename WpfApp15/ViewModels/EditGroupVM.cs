using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.DTO;
using WpfApp15.Model;
using WpfApp15.Tools;

namespace WpfApp15.ViewModels
{
    public class EditGroupVM
    {
        public Group EditGroup { get; set; }
        public CommandVM SaveGroup { get; set; }

        private CurrentPageControl currentPageControl;
        public EditGroupVM(CurrentPageControl currentPageControl)
        {
            this.currentPageControl = currentPageControl;
            EditGroup = new Group();
            InitCommand();
        }
        public EditGroupVM(Group editGroup, CurrentPageControl currentPageControl)
        {
            this.currentPageControl = currentPageControl;
            EditGroup = editGroup;
            InitCommand();
        }

        private void InitCommand()
        {
            SaveGroup = new CommandVM(()=> {
                var model = SqlModel.GetInstance();
                if (EditGroup.ID == 0)
                    model.Insert(EditGroup);
                else
                    model.Update(EditGroup);
                currentPageControl.Back();
            });
        }

        

    }
}
