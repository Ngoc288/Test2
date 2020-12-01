using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IEmployeeRepository: IBaseRepository<Employee>
    {
        Employee GetEmployeeByCode(string employeeCode);
    }
}
