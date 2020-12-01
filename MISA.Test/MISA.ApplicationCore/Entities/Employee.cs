using MISA.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Thông tin nhân viên
    /// </summary>
    /// CreatedBy: NVMANH (26/11/2020)
    public class Employee : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Enums.Gender? Gender { get; set; }

       

        public string Address { get; set; }
        public string PositionName { get; set; }
        public string DepartmentName { get; set; }
        public double? Salary { get; set; }

        public WorkStatus? WorkStatus { get; set; }

    }
}
