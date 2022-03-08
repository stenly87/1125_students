using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.DTO;
using WpfApp15.Model;

namespace WpfApp15.ViewModels
{
    class ViewValuesVM : BaseVM
    {
        private Group selectedGroup;
        private Discipline selectedDiscipline;

        public List<Discipline> DisciplineArray { get; set; }
        public List<Group> GroupArray { get; set; }
        public List<Journal> ValuesArray { get; set; }

        public Discipline SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                selectedDiscipline = value;
                Filter();
            }
        }

        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                Filter();
            }
        }

        public ViewValuesVM()
        {
            var db = SqlModel.GetInstance();
            DisciplineArray = db.SelectDisciplines();
            GroupArray = db.SelectGroupsRange(0, 100);
        }

        private void Filter()
        {
            if (SelectedDiscipline == null ||
                SelectedGroup == null)
                return;

            var db = SqlModel.GetInstance();
            ValuesArray = db.SelectJournalByDisciplineAndGroup(SelectedDiscipline, SelectedGroup);
            Signal(nameof(ValuesArray));
        }
    }
}
