using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp15.DTO;
using WpfApp15.Model;
using WpfApp15.Pages;
using WpfApp15.Tools;

namespace WpfApp15.ViewModels
{
    class AddValuesVM : BaseVM
    {
        private Group selectedGroup;

        public List<Discipline> DisciplineArray { get; set; }
        public List<Group> GroupArray { get; set; }
        public List<Student> StudentArray { get; set; }

        public Discipline SelectedDiscipline { get; set; }
        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                StudentArray = SqlModel.GetInstance().SelectStudentsByGroup(selectedGroup);
                Signal(nameof(StudentArray));
            }
        }
        public Student SelectedStudent { get; set; }

        public DateTime Day { get; set; }
        public string Value { get; set; } = "5";
        public CommandVM SaveValue { get; set; }

        public AddValuesVM()
        {
            var db = SqlModel.GetInstance();
            Day = DateTime.Now;
            DisciplineArray = db.SelectDisciplines();
            GroupArray = db.SelectGroupsRange(0, 100);

            SaveValue = new CommandVM(()=> {
                if (SelectedDiscipline == null || SelectedStudent == null)
                {
                    MessageBox.Show("Не указаны все данные");
                    return;
                }
                db.Insert(new Journal
                {
                    Day = Day,
                    Value = Value,
                    DisciplineId = SelectedDiscipline.ID,
                    StudentId = SelectedStudent.ID
                });
            });
        }
    }
}
 