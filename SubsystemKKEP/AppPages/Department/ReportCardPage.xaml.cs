using SubsystemKKEP.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using Word = Microsoft.Office.Interop.Word;

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
                MessageBox.Show("Отсутствуют студенты. Обратитесь к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                BtnAnalis.IsEnabled = false;
                BtnExcel.IsEnabled = false;
                BtnPDF.IsEnabled = false;
                BtnWord.IsEnabled = false;
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
            if (GetData.IsCountDisciplinesNotNull(currentGroup))
            {
                ExportWordOrPDF(true);
            }
            else
            {
                MessageBox.Show("Отсутствуют дисциплины у группы. Обратитесь к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            if (GetData.IsCountDisciplinesNotNull(currentGroup))
            {
                ExportWordOrPDF(false);
            }
            else
            {
                MessageBox.Show("Отсутствуют дисциплины у группы. Обратитесь к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            if (GetData.IsCountDisciplinesNotNull(currentGroup))
            {
                ExportExcel();
            }
            else
            {
                MessageBox.Show("Отсутствуют дисциплины у группы. Обратитесь к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnOpenReportCard_Click(object sender, RoutedEventArgs e)
        {
            if (GetData.IsCountDisciplinesNotNull(currentGroup))
            {
                InterfaceManagement.ManagementPage.Navigate(new ReportCardConcreteStudentPage((sender as System.Windows.Controls.Button).DataContext as Student));
            }
            else
            {
                MessageBox.Show("Отсутствуют дисциплины у группы. Обратитесь к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ExportExcel()
        {

        }

        /// <summary>
        /// Экспорт в Word или PDF
        /// </summary>
        /// <param name="isPDF">если true - экспорт только в PDF, false - экспорт только в Word</param>
        private void ExportWordOrPDF(bool isPDF)
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

            var currentDisciplines = GetData.GetAppointments(currentGroup);

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
            reportTable.Rows[1].Height = 150;

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

            Word.Paragraph departament = document.Paragraphs.Add();
            Word.Range departamentRange = header.Range;
            departamentRange.InsertParagraphAfter();
            departamentRange.InsertParagraphAfter();
            departamentRange.InsertParagraphAfter();
            departamentRange.InsertParagraphAfter();
            departamentRange.Text = $"{InterfaceManagement.ManagementUser.UserName}";
            departamentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;


            var path = $"Ведомость группы {currentGroup.GroupName} от {date.ToString("d")}";
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = path;
                sfd.Title = $"Сохранение ведомости группы {currentGroup.GroupName}";
                if (isPDF)
                {
                    sfd.DefaultExt = "pdf";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        document.SaveAs2(sfd.FileName, Word.WdExportFormat.wdExportFormatPDF);
                        MessageBox.Show("Ведомость сформирована", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    sfd.DefaultExt = "docx";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        document.SaveAs(sfd.FileName);
                        MessageBox.Show("Ведомость сформирована", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                application.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
