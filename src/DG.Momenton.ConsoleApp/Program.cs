using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DG.Momenton.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {  
            // Setting up the access to appsettings.json
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            // Getting the API Endpoint from config
            var apiEndpoint = configuration["apiEndpoint"];

            Console.WriteLine("Invoking Web API to get the employee hierarchy..");
            Console.WriteLine();

            // Invoking WebAPI
            var employeeHierarchyManager = new EmployeeHierarchyManager();
            Console.WriteLine("Employee Hierarchy Structure : ");
            Task<string> task = employeeHierarchyManager.GetEmployeeHierarchyAsync(apiEndpoint);
            task.Wait();
            Console.WriteLine(task.Result);

            Console.WriteLine("\n\nPress any key to continue..");
            Console.ReadLine();
        }
    }
}
