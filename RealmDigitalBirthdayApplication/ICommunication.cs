using RealmDigitalBirthdayApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealmDigitalBirthdayApplication
{
    public interface ICommunication
    {
        void SendEmail(Employee employee, string subject, string body);
    }

    public class Communation : ICommunication
    {
        public void SendEmail(Employee employee, string subject, string body)
        {
            var to = employee.Id;
            Console.WriteLine("Sending email to employee with Id Number:" + employee.Id);
            Console.WriteLine("Happy Birthay " + employee.Name + " " + employee.LastName); 
        }
    }
}
