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
        public ReportCardConcreteStudentPage(Student student)
        {
            InitializeComponent();
            TextStudent.Text = $"Студент: {student.FullName}";

            var marks = App.DataBase.Marks.Where(p => p.IdStudent == student.Id).
                GroupBy(q=> q.Discipline).Select(w=> new { Dis = w.Key, Avg = w.Average(s => s.MarkValue)}).ToList();
            var report = new Dictionary<string, string>();

            var currentDisciplines = new List<Appointment>();
            foreach (var disOfDep in App.DataBase.DisciplineOfDepartments.ToList())
            {
                foreach (var appointment in App.DataBase.Appointments.Where(p => p.IdGroup == student.IdGroup).ToList())
                {
                    if (appointment.IdDiscipline == disOfDep.IdDiscipline
                        && disOfDep.CourseOfStudy == student.Group.CourseOfStudy
                        && disOfDep.Department.User.Id == InterfaceManagement.ManagementUser.Id)
                    {
                        currentDisciplines.Add(appointment);
                    }
                }
            }

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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }
    }
}
