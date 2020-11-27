using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RealmDigitalBirthdayApplication.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RealmDigitalBirthdayApplication
{
    public class BirthdayService : ICelebration, ICommunication
    {
        private readonly WorkerOptions _options;

        public BirthdayService(WorkerOptions options)
        {
            _options = options;
        }

        public IEnumerable<Employee> AllEmployees()
        {
            var url = _options.EmployeeServiceUri;
            var rs = GetAsync(url).GetAwaiter().GetResult();
            var pg = JsonConvert.DeserializeObject<IEnumerable<Employee>>((rs));
            return pg;
        }

        public IEnumerable<int> ExcludeBirthday()
        {
            var url= _options.ExclusionServiceUri;
            var rs = GetAsync(url).GetAwaiter().GetResult();
            var pg = JsonConvert.DeserializeObject<IEnumerable<int>>((rs));
            return pg;
        }

        public void SendEmail(Employee employee, string subject, string body)
        {
        }

        public List<Employee> EmployeesWithTodaysBirthDay(List<Employee> employees, int[] exludeEmployee)
        {
            var birthDayEmployees = new List<Employee>();
            var today = DateTime.Now;

            var DayAndMonth = $"{today.Day}/{today.Month}";

            foreach (var employee in employees)
            {
                if (employee.EmploymentEndDate.HasValue)
                {
                    Console.WriteLine("Excluded as the employee no longer works for Realm Digital:", employee.Name);
                    continue;
                }
                if (!employee.EmploymentStartDate.HasValue)
                {
                    Console.WriteLine("Excluded as the employee has not started working for Realm Digital:", employee.Name);
                    continue;
                }
                if (Array.Exists(exludeEmployee, e => e.Equals(employee.Id)))
                {
                    Console.WriteLine("Exclude employee specifically configured to not receive birthday wishes:", employee.Name);
                    continue;
                }
                var birthday = $"{employee.DateOfBirth.Day}/{employee.DateOfBirth.Month}";
                if (string.Equals(DayAndMonth, birthday))
                {
                    birthDayEmployees.Add(employee);
                }
                if (!DateTime.IsLeapYear(DateTime.Now.Year) && DayAndMonth == "01/03")
                {
                    var leapYearDate = "29/02";
                    if (birthday == leapYearDate)
                    {
                        birthDayEmployees.Add(employee);
                    }
                }
            }
            return birthDayEmployees;
        }

        public static async Task<string> GetAsync(string uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => (content));
        }
    }
}
