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
    [Table("CancelLeaveApply")]
    //销假单
    public partial class CancelLeaveApply
    {
        [Key]
        //销假编号
        public int Cid { get; set; }
        //请假编号
        public Nullable<int> LeaveId { get; set; }
        //销假理由
        public string CancelLeaveReason { get; set; }
        //复原上班时间
        public Nullable<System.DateTime> BackToWorkTime { get; set; }
        //是否删除
        public Nullable<bool> IsDel { get; set; }
    
    }
}
