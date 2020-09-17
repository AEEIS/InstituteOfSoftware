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
    /// 市场渠道的区域
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
        /// <summary>
        /// 负责区域时间
        /// </summary>
        public System.DateTime StaffAreaDate { get; set; }
        /// <summary>
        /// 负责区域时间
        /// </summary>
        public Nullable<System.DateTime> StopDate { get; set; }
    }
}
