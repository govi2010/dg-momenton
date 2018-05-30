using DG.Momenton.Domain.Employee.Queries;
using DG.Momenton.Domain.Employee.QueryHandlers;
using DG.Momenton.Domain.Employee.TransferObjects;
using DG.Momenton.Infrastructure.Repository;
using DG.Momenton.Infrastructure.Repository.Models;
using NLog;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dg.Momenton.Domain.Test
{
    public class GetEmployeeHierarchyQueryHandlerTest : IDisposable
    {
        #region Private members

        /// <summary>
        /// The Employee Repository
        /// </summary>
        private readonly IRepository<Employee> _employeeRepository;

        /// <summary>
        /// The query-handler to get employee hierarchy
        /// </summary>
        private readonly GetEmployeeHierarchyQueryHandler _handler;

        /// <summary>
        /// The NLog instance
        /// </summary>
        private readonly ILogger _logger;

        #endregion
        #region ctor

        /// <summary>
        /// The Test constructor
        /// </summary>
        public GetEmployeeHierarchyQueryHandlerTest()
        {
            // Mock the repo
            this._employeeRepository = Substitute.For<IRepository<Employee>>();
            this._logger = Substitute.For<ILogger>();

            // And then pass the mock-repo to the handler
            this._handler = new GetEmployeeHierarchyQueryHandler(_employeeRepository, _logger);
        }

        #endregion
        #region Helper methods

        #region GetMockedEmployees

        /// <summary>
        /// Seed the mock-up data
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Employee> GetMockedEmployees()
        {
            // Employee with no-manager
            yield return new Employee
            {
                Id = 1,
                Name = "King"
            };

            yield return new Employee
            {
                Id = 2,
                Name = "Anton",
                ManagerId = 1
            };
            yield return new Employee
            {
                Id = 3,
                Name = "Josh",
                ManagerId = 1
            };
            yield return new Employee
            {
                Id = 4,
                Name = "Luis",
                ManagerId = 2
            };
            yield return new Employee
            {
                Id = 5,
                Name = "Martha",
                ManagerId = 3
            };

            // Employee with invalid manager-id
            yield return new Employee
            {
                Id = 6,
                Name = "Budi",
                ManagerId = 999
            };
        }

        #endregion
        #region FindEmployeeTO

        /// <summary>
        /// Helper function used to help finding the employee within the collection (by their ID)
        /// </summary>
        /// <param name="employeeTOs">The Employee collection</param>
        /// <param name="id">The employee-id to search</param>
        /// <returns></returns>
        private EmployeeTO FindEmployeeTO(IEnumerable<EmployeeTO> employeeTOs,int id)
        {
            EmployeeTO output = employeeTOs.FirstOrDefault(x => x.Id == id);
            if (output == null)
            {
                foreach (var employeeTO in employeeTOs)
                {
                    output = FindEmployeeTO(employeeTO.Subordinates, id);
                    if (output != null)
                    {
                        break;
                    }
                }
            }

            return output;
        }

        #endregion

        #endregion
        #region Theory

        [Theory]
        [InlineData(1, new int[] {2,3})]
        [InlineData(2, new int[] {4})]
        [InlineData(3, new int[] {5})]
        public void Given_ManagerHasSubordinates_When_Retrieved_Then_ExistWithinTheManagersSubordinateList(
            int managerId, int[] subordinateIds)
        {
            #region  Arrange

            var query = new GetEmployeeHierarchyQuery();
            this._employeeRepository.All().Returns(GetMockedEmployees().AsQueryable());

            #endregion
            #region Act

            var result = _handler.Handle(query);
            var manager = FindEmployeeTO(result.RootEmployees,managerId);

            #endregion
            #region Assert

            // Make sure the manager is exist
            Assert.NotNull(manager);

            // Make sure the manager only has the right amount of subordinates
            Assert.Equal(manager.Subordinates.Count,subordinateIds.Length);

            // Make sure the manager has subordinate-id within his/her subordinate-list
            foreach (var id in subordinateIds)
            {
                Assert.Contains(manager.Subordinates, x => x.Id == id);
            }

            #endregion
        }

        [Theory]
        [InlineData(999)]
        public void Given_EmployeeHasInvalidManagerId_When_Retrieved_Then_InvalidManagerWillBeStoredAtRootAsUnknown(int unknownManagerId)
        {
            #region  Arrange

            var query = new GetEmployeeHierarchyQuery();
            this._employeeRepository.All().Returns(GetMockedEmployees().AsQueryable());

            #endregion
            #region Act

            var result = _handler.Handle(query);
            var unknownManager = result.RootEmployees.FirstOrDefault(x => x.Id == unknownManagerId);

            #endregion
            #region Assert

            // Make sure 'unknown-manager' is exist
            Assert.NotNull(unknownManager);

            #endregion
        }

        [Theory]
        [InlineData(1)]
        public void Given_EmployeeHasNoManager_When_Retrieved_Then_ShouldAppearAtTheRootLevel(int employeeId)
        {
            #region  Arrange

            var query = new GetEmployeeHierarchyQuery();
            this._employeeRepository.All().Returns(GetMockedEmployees().AsQueryable());

            #endregion
            #region Act

            var result = _handler.Handle(query);
            var king = result.RootEmployees.FirstOrDefault(x => x.Id == employeeId);

            #endregion
            #region Assert

            // Make sure employee is exist
            Assert.NotNull(king);

            #endregion
        }
        
        #endregion
        #region Fact

        [Fact]
        public void Given_EmployeeExist_When_Retrieved_Then_ReturnsOnlyTopManagerAtItsRootLevel()
        {
            #region  Arrange

            var query = new GetEmployeeHierarchyQuery();
            this._employeeRepository.All().Returns(GetMockedEmployees().AsQueryable());

            #endregion
            #region Act

            var result = _handler.Handle(query);

            #endregion
            #region Assert

            Assert.All(result.RootEmployees, x => Assert.False(x.ManagerId.HasValue));

            #endregion
        }

        [Fact]
        public void Given_NoEmployeeExist_When_Retrieved_Then_ReturnsNothing()
        {
            #region  Arrange

            var query = new GetEmployeeHierarchyQuery();
            this._employeeRepository.All().Returns(new List<Employee>().AsQueryable());

            #endregion
            #region Act

            var result = _handler.Handle(query);

            #endregion
            #region Assert

            Assert.Empty(result.RootEmployees);

            #endregion
        }
     
        #endregion
        #region Dispose

        public void Dispose()
        {
        }
        
        #endregion
    }
}
