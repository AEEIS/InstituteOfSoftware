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
    [Table(name: "JobTransferAppply")]
    //  转岗申请表
    public partial class JobTransferAppply
    {
        [Key]
        public int Id { get; set; }
        //员工编号
        public string EmployeeId { get; set; }
        //拟转岗部门
        public Nullable<int> PlanTurnDeptId { get; set; }
        //拟转岗位
        public Nullable<int> PlanTurnPositionId { get; set; }
        //转岗后工资
        public Nullable<decimal> TurnAfterSalary { get; set; }
        //转岗原因
        public string Reason { get; set; }
        //转岗申请时间
        public Nullable<System.DateTime> ApplicationTime { get; set; }
        //是否删除
        public Nullable<bool> IsPass { get; set; }
        public Nullable<bool> IsApproval { get; set; }
        //public  EmployeesInfo EmployeesInfo { get; set; }
    }
}
