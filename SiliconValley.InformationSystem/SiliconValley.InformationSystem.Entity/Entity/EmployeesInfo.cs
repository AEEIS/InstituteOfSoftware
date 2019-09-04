//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 员工表
    /// </summary>
    [Table(name: "EmployeesInfo")]
    public partial class EmployeesInfo
    {
        
        [Key]
        public string EmployeeId { get; set; }
        public Nullable<int> DDAppId { get; set; }
        public string EmpName { get; set; }
        public int PositionId { get; set; }
        public string Sex { get; set; }
        public Nullable<int> Age { get; set; }
        public string Nation { get; set; }
        public string Phone { get; set; }
        public string IdCardNum { get; set; }
        public Nullable<System.DateTime> ContractStartTime { get; set; }
        public Nullable<System.DateTime> ContractEndTime { get; set; }
        public System.DateTime EntryTime { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Birthday { get; set; }
        public Nullable<System.DateTime> PositiveDate { get; set; }
        public string UrgentPhone { get; set; }
        public string DomicileAddress { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public Nullable<bool> MaritalStatus { get; set; }
        public Nullable<System.DateTime> IdCardIndate { get; set; }
        public string PoliticsStatus { get; set; }
        public string WorkExperience { get; set; }
        public Nullable<decimal> ProbationSalary { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<System.DateTime> SSStartMonth { get; set; }
        public string BCNum { get; set; }
        public string Material { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }
    }
}
