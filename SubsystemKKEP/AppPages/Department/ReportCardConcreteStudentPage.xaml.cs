using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SubsystemKKEP.Classes;

namespace SubsystemKKEP.AppPages.Department
{
    /// <summary>
    /// Логика взаимодействия для ReportCardConcreteStudentPage.xaml
    /// </summary>
    public partial class ReportCardConcreteStudentPage : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="student">выбранный студент</param>
        public ReportCardConcreteStudentPage(Student student)
        {
            InitializeComponent();
            TextStudent.Text = $"Студент: {student.FullName}";
            ReportCardStudent(student);
        }

        /// <summary>
        /// Загрузка ведомости в DataGrid
        /// </summary>
        /// <param name="student">текущий студент</param>
        private void ReportCardStudent(Student student)
        {
            var marks = App.DataBase.Marks.Where(p => p.IdStudent == student.Id).
                GroupBy(q => q.Discipline).Select(w => new { Dis = w.Key, Avg = w.Average(s => s.MarkValue) }).ToList();
            var report = new Dictionary<string, string>();

            var currentDisciplines = GetData.GetAppointments(student.Group);

            foreach (var curDis in currentDisciplines)
            {
                foreach (var curMark in marks)
                {
                    if (curDis.Discipline.DisciplineName == curMark.Dis.DisciplineName && !report.ContainsKey(curDis.Discipline.DisciplineName))
                    {
                        report.Add(curDis.Discipline.DisciplineName, Math.Round(curMark.Avg).ToString());
                        break;
                    }
                }
                if (!report.ContainsKey(curDis.Discipline.DisciplineName))
                {
                    report.Add(curDis.Discipline.DisciplineName, "н/а");
                }
            }
            DGridReport.ItemsSource = report.ToList();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }
    }
}
