using DG.Momenton.Infrastructure.CQRS;
using Microsoft.AspNetCore.Mvc;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Momenton.API.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public ICommandDispatcher CommandDispatcher { get; set; }
        [Inject]
        public IQueryDispatcher QueryDispatcher { get; set; }
    }
}
