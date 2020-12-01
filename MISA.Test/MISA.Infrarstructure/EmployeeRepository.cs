using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;

namespace MISA.Infarstructure
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region DECLARE
        #endregion
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override IEnumerable<Employee> GetEntities()
        {
            return base.GetEntities("Proc_GetEmployees");
        }
        public Employee GetEmployeeByCode(string employeeCode)
        {

            return _dbConnection.Query<Employee>($"SELECT * FROM Employee E WHERE EmployeeCode = '{employeeCode}'").FirstOrDefault();
        }
    }
}
