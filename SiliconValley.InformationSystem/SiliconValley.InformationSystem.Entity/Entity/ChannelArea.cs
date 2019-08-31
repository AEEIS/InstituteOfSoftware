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
    /// 渠道的区域
    /// </summary>
    [Table(name: "ChannelArea")]
    public partial class ChannelArea
    {
        [Key]
        public int ID { get; set; } 
        public int RegionID { get; set; }
        public Nullable<int> RegionalDirectorID { get; set; }
        public int ChannelStaffID { get; set; }
        public bool IsDel { get; set; }
        public System.DateTime StaffAreaDate { get; set; }
    }
}