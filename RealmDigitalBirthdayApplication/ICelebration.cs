using RealmDigitalBirthdayApplication.Models;
using System.Collections.Generic;

namespace RealmDigitalBirthdayApplication
{
    public interface ICelebration
    {
        IEnumerable<Employee> AllEmployees();
        List<Employee> EmployeesWithTodaysBirthDay(List<Employee> employees, int[] exludeEmployee);
        IEnumerable<int> ExcludeBirthday();

    }
}