using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Momenton.Domain.Employee.Queries;
using DG.Momenton.Domain.Employee.TransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace DG.Momenton.API.Controllers 
{
    [ResponseCache(Duration = 60)]
    [Produces("application/json")]
    [Route("api/employees")]
    public class EmployeesController : BaseController
    {        
        // GET: api/employees/hierarchy
        [HttpGet("hierarchy", Name = "GetEmployeeHierarchy")]
        public IEnumerable<EmployeeTO> GetEmployeeHierarchy()
        {
            var result = QueryDispatcher
                .Query<GetEmployeeHierarchyQuery,GetEmployeeHierarchyQueryOutput>(new GetEmployeeHierarchyQuery())
                .RootEmployees;
            return result;
        }
    }
}
