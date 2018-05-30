# DG-MOMENTON
This repo contains the source codes for the solution that I prepare, build, present for the Momenton's code-challenge.

### Requirement Specification
Below is employee data of a small company.
It represents the hierarchical relationship among employees. CEO of the company doesn't
have a manager.

| Employee Name | ID | Manager ID |
| ------ | ------ | ------ |
| Alan | 100 | 150 |
| Martin | 220 | 100 |
| Jamie | 150 |  |
| Alex | 275 | 100 |
| Steve | 400 | 150 |
| David | 190 | 400 |

Design a suitable representation of this data. Feel free to choose any database (RDBMS, inmemory
database etc), file system or even a data structure like List or Map. Then write code
(in any language and framework) that displays the organisation hierarchy as below:

|  |  |  |
| ------ | ------ | ------ |
| Jamie |  |  |
|  | Alan |  |
|  |  | Martin |
|  |  | Alex |
|  | Steve |  |
|  |  | David |

### Assumption

* When an employee has no manager-id set against, it will appear on the root level.

* When an employee has an invalid manager-id (i.e. not exist), the system will generate a 'stub' for the invalid-manager, and set its name as 'Unknown'. The link to the subordinate will still be established from here.

### Tech-stack

This project uses a number of components to work properly:

* [ASP .NET Core 2 Web API] - API framework
* [Entity Framework Core] - ORM to access the database
* [Ninject] - Dependency Injection
* [NLog] - logging framework
* [XUnit] - testing framework
* [NSubstitute] - mock framework
* [Newtonsoft] - JSON parser
* [SQL Server] - relational database

From the architectural point of view, this projects uses [Repository] & [CQRS] (Command Query Responsiblity Segregation) pattern to help manage its domain flow.

>Command and Query Responsibility Segregation (CQRS) is a pattern that segregates the operations that read data (queries) from the operations that update data (commands) by using separate interfaces. This means that the data models used for querying and updates are different. The models can then be isolated, as shown in the following figure, although that's not an absolute requirement.

[![CQRS-Pattern](https://martinfowler.com/bliki/images/cqrs/cqrs.png)](https://martinfowler.com/bliki/CQRS.html)

[ASP .NET Core 2 Web API]: <https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.0>
[Entity Framework Core]: <https://docs.microsoft.com/en-us/ef/core/>
[Ninject]: <http://www.ninject.org/>
[NLog]: <http://nlog-project.org/>
[XUnit]: <https://xunit.github.io/>
[NSubstitute]: <https://github.com/nsubstitute>
[Newtonsoft]: <https://www.newtonsoft.com/json>
[SQL Server]: <https://www.microsoft.com/en-au/sql-server/sql-server-2017>
[CQRS]: <https://martinfowler.com/bliki/CQRS.html>
[Repository]: <http://deviq.com/repository-pattern/>

## Getting Started

Here are few steps that needs to be done prior to running the application.
* Verify the DB connection-string located within the `appsettings.json` file.
Look for `DgMomentonDatabase` key, by default it is set to `"Server=.\\SQLExpress;Database=DgMomenton;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true"`
* Build the solution in Visual Studio
* Run the WebAPI project i.e. `DG.Momenton.API` (IIS Express would do)
On the very first time running, it will create the database at the location specified by the `DgMomentonDatabase` key within `appsettings.json` file.
* Run the `DG.Momenton.ConsoleApp`
This is the console app that consumes the API that we launch in the previous step.
This app will retrieve the employee data from the API and then format & display it to the console.

## Sample Output

```sh
Invoking Web API to get the employee hierarchy..

Employee Hierarchy Structure :

 + Jamie - ID:150
   + Allan - ID:100
     + Martin - ID:220
     + Alex - ID:275
   + Steve - ID:400
     + David - ID:190

 + Claudia - ID:189

 + (Unknown) - ID:888
   + Nicolas - ID:999
     + Carol - ID:777

Press any key to continue..
```

## Authors

* **Dany Gunawan**
