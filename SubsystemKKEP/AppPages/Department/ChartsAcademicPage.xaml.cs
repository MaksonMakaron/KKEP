using SubsystemKKEP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace SubsystemKKEP.AppPages.Department
{
    /// <summary>
    /// Логика взаимодействия для ChartsAcademicPage.xaml
    /// </summary>
    public partial class ChartsAcademicPage : Page
    {
        /// <summary>
        /// Текущая группа
        /// </summary>
        private Group currentGroup = new Group();

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="group">конкретная группа</param>
        public ChartsAcademicPage(Group group)
        {
            InitializeComponent();
            currentGroup = group;
            ChartAnalysis.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Analysis")
            {
                IsValueShownAsLabel = true
            };
            ChartAnalysis.Series.Add(currentSeries);
            LoadChart();
        }
        
        /// <summary>
        /// Загрузка диаграммы на странице
        /// </summary>
        private void LoadChart()
        {
            Series currentSeries = ChartAnalysis.Series.FirstOrDefault();
            currentSeries.ChartType = SeriesChartType.Doughnut;
            currentSeries.Points.Clear();

            var excellents = 0; //отличники
            var goods = 0; //хорошисты
            var acceptables = 0; //троечники
            var bad = 0; //двоечники
            var currentDisciplines = GetData.GetAppointments(currentGroup);

            foreach (var student in currentGroup.Students.ToList())
            {
                var report = new Dictionary<string, string>();
                var marks = App.DataBase.Marks.Where(p => p.IdStudent == student.Id).
                                            GroupBy(q => q.Discipline).Select(w => new { Dis = w.Key, Avg = w.Average(s => s.MarkValue) }).ToList();

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

                if (report.ContainsValue("2") || report.ContainsValue("н/а"))
                {
                    bad++;
                }
                else if (report.ContainsValue("3"))
                {
                    acceptables++;
                }
                else if (report.ContainsValue("4"))
                {
                    goods++;
                }
                else
                {
                    excellents++;
                }
            }
            if (bad > 0)
            {
                currentSeries.Points.AddXY("Неудовл.", bad);
            }
            if (acceptables > 0)
            {
                currentSeries.Points.AddXY("Удов.", acceptables);
            }
            if (goods > 0)
            {
                currentSeries.Points.AddXY("Хор.", goods);
            }
            if (excellents > 0)
            {
                currentSeries.Points.AddXY("Отл.", excellents);
            }
        }

        /// <summary>
        /// По нажатию на кнопку - отображается окно со справкой
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnHelper_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("График показывает анализ успеваемости\n" +
                "Под категорию отличники попадают студенты, которые имеют по всем предметам среднюю оценку 5\n" +
                "Под категорию хорошисты попадают студенты, которые имеют по всем предметам среднюю оценку 5 или 4\n" +
                "Под категорию удов. попадают студенты, которые имеют по одному из предметов среднюю оценку 3\n" +
                "Под категорию неудов. попадают студенты, которые имеют по одному из предметов среднюю оценку 2 или н/а\n", "Справка", MessageBoxButton.OK, MessageBoxImage.Question);
        }
    }
}
