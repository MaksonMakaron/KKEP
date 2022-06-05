using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubsystemKKEP.Classes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Word= Microsoft.Office.Interop.Word;
using System.Windows.Shapes;

namespace SubsystemKKEP.AppPages.Department
{
    /// <summary>
    /// Логика взаимодействия для ReportCardPage.xaml
    /// </summary>
    public partial class ReportCardPage : Page
    {
        private Group currentGroup = new Group();

        public ReportCardPage(Group group)
        {
            InitializeComponent();
            currentGroup = group;
            TextGroup.Text = $"Студенты группы: {currentGroup.GroupName}";
            if (currentGroup.Students.Count == 0)
            {
                MessageBox.Show("Отсутствуют студенты. Обратитесь к администратору");
                return;
            }
            DGridGroups.ItemsSource = currentGroup.Students.ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

        private void BtnAnalis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPDF_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            ExportWord();
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel();
        }

        private void BtnOpenReportCard_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new ReportCardConcreteStudentPage((sender as Button).DataContext as Student));
        }

        private void ExportExcel()
        {

        }

        private void ExportWord()
        {
            var application = new Word.Application();
            Word.Document document = application.Documents.Add();
            DateTime date = DateTime.Now;
            Word.Paragraph headerGeneral = document.Paragraphs.Add();
            Word.Range headerGeneralRange = headerGeneral.Range;
            headerGeneralRange.Text = "С В Е Д Е Н И Я";
            headerGeneralRange.ParagraphFormat.SpaceAfter = 0;
            headerGeneralRange.Font.Size = 18;
            headerGeneralRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            headerGeneralRange.Bold = 1;
            headerGeneralRange.InsertParagraphAfter();

            Word.Paragraph header = document.Paragraphs.Add();
            Word.Range headerRange = header.Range;
            headerRange.Bold = 0;
            headerRange.Font.Size = 14;
            headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            headerRange.Text = $"Об успеваемости группы {currentGroup.GroupName} ({currentGroup.CourseOfStudy} курс)\nна {date.Date.ToString("D")}";
            headerRange.InsertParagraphAfter();
            headerRange.InsertParagraphAfter();

            var currentDisciplines = new List<Appointment>();
            foreach (var disOfDep in App.DataBase.DisciplineOfDepartments.ToList())
            {
                foreach (var appointment in App.DataBase.Appointments.Where(p => p.IdGroup == currentGroup.Id).ToList())
                {
                    if (appointment.IdDiscipline == disOfDep.IdDiscipline
                        && disOfDep.CourseOfStudy == currentGroup.CourseOfStudy
                        && disOfDep.Department.User.Id == InterfaceManagement.ManagementUser.Id
                        && currentDisciplines.Where(p=>p.Discipline.DisciplineName == appointment.Discipline.DisciplineName).Count() == 0)
                    {
                        currentDisciplines.Add(appointment);
                    }
                }
            }

            var students = currentGroup.Students.ToList();
            var allMarks = App.DataBase.Marks.Where(p => p.Student.IdGroup == currentGroup.Id).ToList();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table reportTable = document.Tables.Add(tableRange, currentGroup.Students.Count + 1, currentDisciplines.Count + 2);

            reportTable.Borders.InsideLineStyle = reportTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            reportTable.Range.Font.Size = 10;
            reportTable.Columns[1].Width = 30;
            reportTable.Columns[2].Width = 127;
            Word.Range cellRange;

            cellRange = reportTable.Cell(1, 1).Range;
            cellRange.Text = "№ п/п";
            cellRange = reportTable.Cell(1, 2).Range;
            cellRange.Text = "Фамилия, И. О.";
            reportTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            reportTable.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            reportTable.Rows[1].Height = 100;

            for (int j = 0; j < students.Count; j++)
            {
                var student = students[j];
                cellRange = reportTable.Cell(j + 2, 1).Range;
                cellRange.Text = (j + 1).ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                cellRange = reportTable.Cell(j + 2, 2).Range;
                cellRange.Text = student.ShortName.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                for (int i = 0; i < currentDisciplines.Count; i++)
                {
                    var curDis = currentDisciplines[i].Discipline;
                    reportTable.Columns[i + 3].Width = 30;
                    cellRange = reportTable.Cell(1, i + 3).Range;
                    cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    cellRange.Text = curDis.DisciplineName;
                    cellRange.Orientation = Word.WdTextOrientation.wdTextOrientationUpward;

                    cellRange = reportTable.Cell(j + 2, i + 3).Range;
                    if (App.DataBase.Marks.Where(p => p.IdStudent == student.Id && p.Discipline.DisciplineName == curDis.DisciplineName).Count() > 0)
                    {
                        double avg = App.DataBase.Marks.Where(p => p.IdStudent == student.Id && p.Discipline.DisciplineName == curDis.DisciplineName).Average(p => p.MarkValue);
                        cellRange.Text = Math.Round(avg).ToString();
                    }
                    else
                    {
                        cellRange.Text = "н/а";
                    }
                }

            }

            //for (int k = 0; k < allMarks.Count; k++)
            //{
            //    var avg = App.DataBase.Marks.
            //        Where(p => p.IdStudent == student.Id
            //        && p.Discipline.DisciplineName == curDis.DisciplineName)
            //        .Average(p => p.MarkValue);
            //    cellRange = reportTable.Cell(i + 2, i + 3).Range;
            //    cellRange.Text = avg.ToString();
            //}

            Word.Paragraph departament = document.Paragraphs.Add();
            Word.Range departamentRange = header.Range;
            departamentRange.InsertParagraphAfter();
            departamentRange.InsertParagraphAfter();
            departamentRange.InsertParagraphAfter();
            departamentRange.InsertParagraphAfter();
            departamentRange.Text = $"{InterfaceManagement.ManagementUser.UserName}";
            departamentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;


            var path = $"Ведомость группы {currentGroup.GroupName} от {date.ToString("d")}";
            var saveDocx = $"{path}.docx";
            try
            {
                document.SaveAs2($"D:\\{saveDocx}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
            application.Quit();
            MessageBox.Show("Ведомость сформирована", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
