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
    [Table("Costlist")]
    //费用价格单
    public partial class Costlist
    {
         [Key]  
        public int ID { get; set; }
        //名称
        public string 名称 { get; set; }
        //单价
        public Nullable<decimal> 单价 { get; set; }
        //备注
        public string Remarks { get; set; }
        //是否禁用
        public Nullable<bool> IsDelete { get; set; }
        //添加时间
        public Nullable<System.DateTime> AddTime { get; set; }     
    }
}
