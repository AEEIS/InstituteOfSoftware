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
    [Table(name: "GoodsProvide")]
    public partial class GoodsProvide
    {
        [Key]
        public int Id { get; set; }
        public Nullable<int> StockInfo_Id { get; set; }
        public string EmployeesInfo_Id { get; set; }
        public Nullable<int> UseCount { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Rmark { get; set; }
        public Nullable<bool> IsDel { get; set; }
    
   
    }
}
