using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealmDigitalBirthdayApplication.Models;

namespace RealmDigitalBirthdayApplication
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerOptions _options;
        private readonly ICelebration _birthdayService;
        private readonly ICommunication comms;
        private const int DayCycle = 86400;

        public Worker(ILogger<Worker> logger, WorkerOptions options, ICelebration birthdayService, ICommunication comms)
        {
            _logger = logger;
            _options = options;
            _birthdayService = birthdayService;
            this.comms = comms;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                var allEmployees = _birthdayService.AllEmployees().ToList();
                var exclude = _birthdayService.ExcludeBirthday().ToArray();
                var todaysCelebrants = _birthdayService.EmployeesWithTodaysBirthDay(allEmployees, exclude);
                foreach (var employee in todaysCelebrants) 
                {
                    comms.SendEmail(employee, "Happy Birthay", $"Happy Birthday {employee.Name} {employee.LastName}");
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(DayCycle, stoppingToken);
            }
        }
    }
}
