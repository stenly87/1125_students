using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.DTO;
using WpfApp15.Model;
using WpfApp15.Pages;
using WpfApp15.Tools;

namespace WpfApp15.ViewModels
{
    public class EditStudentVM : BaseVM
    {
        public Student EditStudent { get; }
        public CommandVM SaveStudent { get; set; }
        public Group StudentGroup
        {
            get => studentGroup;
            set
            {
                studentGroup = value;
                Signal();
            }
        }

        public List<Group> Groups { get; set; }

        private CurrentPageControl currentPageControl;
        private Group studentGroup;

        public EditStudentVM(CurrentPageControl currentPageControl)
        {
            this.currentPageControl = currentPageControl;
            EditStudent = new Student();
            Init();
        }

        public EditStudentVM(Student editStudent, CurrentPageControl currentPageControl)
        {
            EditStudent = editStudent;
            this.currentPageControl = currentPageControl;
            Init();
            StudentGroup = Groups.FirstOrDefault(s => s.ID == editStudent.GroupId);
        }

        private void Init()
        {
            Groups = SqlModel.GetInstance().SelectGroupsRange(0, 100);
            SaveStudent = new CommandVM(() => {
                if (StudentGroup == null)
                {
                    System.Windows.MessageBox.Show("Нужно выбрать группу для продолжения");
                    return;
                }
                EditStudent.GroupId = StudentGroup.ID;
                var model = SqlModel.GetInstance();
                if (EditStudent.ID == 0)
                    model.Insert(EditStudent);
                else
                    model.Update(EditStudent);
                currentPageControl.SetPage(new ViewStudentsPage(StudentGroup));
            });
        }

    }
}
