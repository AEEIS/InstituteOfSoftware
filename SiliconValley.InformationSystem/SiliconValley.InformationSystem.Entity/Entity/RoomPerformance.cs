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
    [Table(name: "RoomPerformance")]
    //课堂表现
    public partial class RoomPerformance
    {
        [Key]
        public int Id { get; set; }
        //教室id
        public Nullable<int> Classroom_Id { get; set; }
        //班级编号
        public string ClassNumber { get; set; }
        //教室id
        public Nullable<int> Cla_Id { get; set; }
        //是否删除
        public Nullable<bool> IsDelete { get; set; }
        //添加时间
        public Nullable<System.DateTime> Addtime { get; set; }
    
       
    }
}
