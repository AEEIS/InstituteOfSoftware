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

    [Table(name: "DormitoryMaintenance")]
    //宿舍维修记录
    public partial class DormitoryMaintenance
    {
        [Key]
        public int ID { get; set; }
        //宿舍号
        public Nullable<int> Key_name { get; set; }
        //报修时间
        public Nullable<System.DateTime> Reentry { get; set; }  
        //故障原因
        public string Causesoffailure { get; set; }
        //备注
        public string Remarks { get; set; }
        //是否删除
        public Nullable<bool> Dateofregistration { get; set; }
        //添加时间
        public Nullable<System.DateTime> Addtime { get; set; }
    
    }
}
