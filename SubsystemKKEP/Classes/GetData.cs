using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubsystemKKEP.Classes
{
    public class GetData
    {
        /// <summary>
        /// Получение списка дисциплин для группы на курсе
        /// </summary>
        /// <param name="currentGroup">конкретнная группа</param>
        /// <returns></returns>
        public static List<Appointment> GetAppointments(Group currentGroup)
        {
            var currentDisciplines = new List<Appointment>();
            foreach (var disOfDep in App.DataBase.DisciplineOfDepartments.ToList())
            {
                foreach (var appointment in App.DataBase.Appointments.Where(p => p.IdGroup == currentGroup.Id).ToList())
                {
                    if (appointment.IdDiscipline == disOfDep.IdDiscipline
                        && disOfDep.CourseOfStudy == currentGroup.CourseOfStudy
                        && disOfDep.Department.User.Id == InterfaceManagement.ManagementUser.Id
                        && currentDisciplines.Where(p => p.Discipline.DisciplineName == appointment.Discipline.DisciplineName).Count() == 0)
                    {
                        currentDisciplines.Add(appointment);
                    }
                }
            }

            return currentDisciplines;
        }

        /// <summary>
        /// Есть ли дисциплины для данной группы
        /// </summary>
        /// <param name="group">конкретная группа</param>
        /// <returns>true - дисицплины есть, false - дисциплин нет</returns>
        public static bool IsCountDisciplinesNotNull(Group group)
        {
            if (GetAppointments(group).Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Есть ли студенты в данной группе
        /// </summary>
        /// <param name="group">конкретная группа</param>
        /// <returns>true - студенты есть, false - студентов нет</returns>
        public static bool IsCountStudentsNotNull(Group group)
        {
            if (App.DataBase.Students.Where(p => p.IdGroup == group.Id).Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
