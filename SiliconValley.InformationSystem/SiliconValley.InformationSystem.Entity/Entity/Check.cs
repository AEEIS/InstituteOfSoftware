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
    [Table("Check")]
    //盘点表
    public partial class Check
    {
        [Key]
        //ID
        public int Id { get; set; }
        //采购ID
        public Nullable<int> PurchaseApply_Id { get; set; }
        //盘点人
        public string EmployeesInfo_Id { get; set; }
        //盘点时间
        public Nullable<System.DateTime> CheckDate { get; set; }
        //盘点状态
        public string CheckType { get; set; }
        //入库日期
        public Nullable<System.DateTime> InDate { get; set; }
        //备注
        public string Rmark { get; set; }
        //是否删除
        public Nullable<bool> IsDelete { get; set; }
    
    }
}
