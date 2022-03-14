using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.DTO;
using WpfApp15.Model;
using WpfApp15.Tools;
using Spire.Xls;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace WpfApp15.ViewModels
{
    class GenerateXlsxVM : BaseVM
    {
        public List<Prepod> Prepods { get; set; }
        public Prepod SelectedPrepod { get; set; }

        public List<string> Months { get; set; }
        public string SelectedMonth { get; set; }

        public CommandVM GenerateXLS { get; set; }
        public int SelectedYear { get; set; }

        public GenerateXlsxVM()
        {
            SelectedYear = DateTime.Now.Year;
            var sql = SqlModel.GetInstance();
            Prepods = sql.SelectAllPrepods();

            Months = new List<string>(new string[] {
                "Январь",
                "Февраль",
                "Март",
                "Апрель",
                "Май",
                "Июнь",
                "Июль",
                "Август",
                "Сентябрь",
                "Октябрь",
                "Ноябрь",
                "Декабрь" });

            GenerateXLS = new CommandVM(() => {
                if (SelectedMonth == null || SelectedPrepod == null)
                    return;

                Workbook wbToStream = new Workbook();
                Worksheet sheet = wbToStream.Worksheets[0];
                sheet.Range["A1:Q1"].Merge();
                sheet.Range["A1:Q1"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["A1:Q1"].Text = $"Успеваемость за {SelectedMonth} {SelectedYear} у.г.";
                sheet.Range["A2:Q2"].Merge();
                sheet.Range["A2:Q2"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["A2:Q2"].Text = SelectedPrepod.LastName;
                sheet.Range["A3:A4"].Merge();
                sheet.Range["A3"].Text = "№";
                sheet.Range["A3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["B3:B4"].Merge();
                sheet.Range["B3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["B3"].Text = "Группа";
                sheet.Range["C3:C4"].Merge();
                sheet.Range["C3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["C3"].Text = "Кол-во чел";
                sheet.Range["D3:E3"].Merge();
                sheet.Range["D3:E3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["D3:E3"].NumberValue = 5;
                sheet.Range["F3:G3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["F3:G3"].Merge();
                sheet.Range["F3:G3"].NumberValue = 4;
                sheet.Range["H3:I3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["H3:I3"].Merge();
                sheet.Range["H3:I3"].NumberValue = 3;
                sheet.Range["J3:K3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["J3:K3"].Merge();
                sheet.Range["J3:K3"].NumberValue = 2;
                sheet.Range["L3:M3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["L3:M3"].Merge();
                sheet.Range["L3:M3"].Text = @"н\а";
                sheet.Range["L4"].Text = sheet.Range["J4"].Text = sheet.Range["H4"].Text = sheet.Range["F4"].Text = sheet.Range["D4"].Text = "абс";
                sheet.Range["E4"].Text = sheet.Range["G4"].Text = sheet.Range["I4"].Text = sheet.Range["K4"].Text = sheet.Range["M4"].Text = "%";
                sheet.Range["N3:N4"].Merge();
                sheet.Range["N3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["N3"].Text = "Всего";
                sheet.Range["O3:O4"].Merge();
                sheet.Range["O3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["O3"].Text = "ср.балл";
                sheet.Range["P3:P4"].Merge();
                sheet.Range["P3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["P3"].Text = "качество";
                sheet.Range["Q3:Q4"].Merge();
                sheet.Range["Q3"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["Q3"].Text = "% успев";

                int nRow = 5, nCount = 1;
                List<Discipline> disciplinesPrepod = sql.SelectDisciplinesByPrepod(SelectedPrepod);
                Dictionary<Discipline, List<Journal>> journals = sql.SelectJournalByDisciplinesAndMonth(disciplinesPrepod, Months.IndexOf(SelectedMonth)+1, SelectedYear);
                int totalStudents = 0, total5 = 0, total4 = 0, total3 = 0, total2 = 0, totalNA = 0, totalValue = 0;
                
                foreach (var discipline in disciplinesPrepod)
                {
                    List<Group> groups = sql.SelectGroupsByDiscipline(discipline, Months.IndexOf(SelectedMonth)+1, SelectedYear);
                    foreach (var group in groups)
                    {
                        List<Journal> jGroup = SelectJournalValuesByGroup(group, journals[discipline]);
                        int studCount = sql.SelectCountStudentsInGroup(group);
                        totalStudents += studCount;
                        sheet.Range[$"A{nRow}"].NumberValue = nCount++;
                        sheet.Range[$"B{nRow}"].Text = $"{discipline.Title} {group.Title}";
                        sheet.Range[$"C{nRow}"].NumberValue = studCount;
                        
                        int count5 = jGroup.Count(s => s.Value == "5");
                        total5 += count5;
                        sheet.Range[$"D{nRow}"].NumberValue = count5;
                        sheet.Range[$"E{nRow}"].NumberFormat =  "0.00%";
                        sheet.Range[$"E{nRow}"].NumberValue = (double)count5 / studCount;

                        int count4 = jGroup.Count(s => s.Value == "4");
                        total4 += count4;
                        sheet.Range[$"F{nRow}"].NumberValue = count4;
                        sheet.Range[$"G{nRow}"].NumberFormat = "0.00%";
                        sheet.Range[$"G{nRow}"].NumberValue = (double)count4 / studCount;

                        int count3 = jGroup.Count(s => s.Value == "3");
                        total3 += count3;
                        sheet.Range[$"H{nRow}"].NumberValue = count3;
                        sheet.Range[$"I{nRow}"].NumberFormat = "0.00%";
                        sheet.Range[$"I{nRow}"].NumberValue = (double)count3 / studCount;

                        int count2 = jGroup.Count(s => s.Value == "2");
                        total2 += count2;
                        sheet.Range[$"J{nRow}"].NumberValue = count2;
                        sheet.Range[$"K{nRow}"].NumberFormat = "0.00%";
                        sheet.Range[$"K{nRow}"].NumberValue = (double)count2 / studCount;

                        int countNA = jGroup.Count(s => s.Value == @"н\a");
                        totalNA += countNA;
                        sheet.Range[$"J{nRow}"].NumberValue = countNA;
                        sheet.Range[$"K{nRow}"].NumberFormat = "0.00%";
                        sheet.Range[$"K{nRow}"].NumberValue = (double)countNA / studCount;

                        sheet.Range[$"N{nRow}"].NumberValue = count5 + count4 + count3 + count2 + countNA;
                        totalValue += count5 + count4 + count3 + count2 + countNA;
                        sheet.Range[$"O{nRow}"].NumberFormat = "0.00";
                        sheet.Range[$"O{nRow}"].NumberValue = ((double)count2 * 2 + count3 * 3 + countNA * 2 + count4 * 4 + count5 * 5) / studCount;

                        sheet.Range[$"P{nRow}"].NumberFormat = "0.00%";
                        sheet.Range[$"P{nRow}"].NumberValue = ((double)count4 + count5) / studCount;

                        sheet.Range[$"Q{nRow}"].NumberFormat = "0.00%";
                        sheet.Range[$"Q{nRow}"].NumberValue = ((double)count4 + count5 + count3) / studCount;
                        
                        nRow++;
                    }
                }

                sheet.Range[$"B{nRow}"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range[$"B{nRow}"].Text = "Итого";
                sheet.Range[$"C{nRow}"].NumberValue = totalStudents;
                sheet.Range[$"D{nRow}"].NumberValue = total5;
                sheet.Range[$"E{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"E{nRow}"].NumberValue = (double)total5 / totalStudents;
                sheet.Range[$"F{nRow}"].NumberValue = total4;
                sheet.Range[$"G{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"G{nRow}"].NumberValue = (double)total4 / totalStudents;
                sheet.Range[$"H{nRow}"].NumberValue = total3;
                sheet.Range[$"I{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"I{nRow}"].NumberValue = (double)total3 / totalStudents;
                sheet.Range[$"J{nRow}"].NumberValue = total2;
                sheet.Range[$"K{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"K{nRow}"].NumberValue = (double)total2 / totalStudents;
                sheet.Range[$"J{nRow}"].NumberValue = totalNA;
                sheet.Range[$"K{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"K{nRow}"].NumberValue = (double)totalNA / totalStudents;

                sheet.Range[$"N{nRow}"].NumberValue = totalValue;
                sheet.Range[$"O{nRow}"].NumberFormat = "0.00";
                sheet.Range[$"O{nRow}"].NumberValue = ((double)total2 * 2 + total3 * 3 + totalNA * 2 + total4 * 4 + total5 * 5) / totalStudents;
                sheet.Range[$"P{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"P{nRow}"].NumberValue = ((double)total4 + total5) / totalStudents;
                sheet.Range[$"Q{nRow}"].NumberFormat = "0.00%";
                sheet.Range[$"Q{nRow}"].NumberValue = ((double)total4 + total5 + total3) / totalStudents;

                CellRange range = sheet.Range[$"A3:Q{nRow}"];
                range.BorderInside(LineStyleType.Thin, Color.Black);
                range.BorderAround(LineStyleType.Medium, Color.Black);

                nRow += 3;

                sheet.Range[$"B{nRow}"].Text = "Преподаватель";
                sheet.Range[$"M{nRow}:O{nRow}"].Merge();
                sheet.Range[$"M{nRow}:O{nRow}"].HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range[$"M{nRow}:O{nRow}"].Text = SelectedPrepod.LastName;

                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();

                var file_stream = new FileStream("To_stream.xlsx", FileMode.Create);
                wbToStream.SaveToStream(file_stream);
                file_stream.Close();

            });
        }

        private List<Journal> SelectJournalValuesByGroup(Group group, List<Journal> journals)
        {
            return journals.Where(s=>s.Student.GroupId == group.ID).ToList();
        }
    }
}
