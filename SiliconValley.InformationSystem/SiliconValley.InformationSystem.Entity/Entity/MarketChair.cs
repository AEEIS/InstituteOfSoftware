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
    /// 讲座记录表
    /// </summary>
    [Table(name: "MarketChair")]
    public partial class MarketChair
    {
        /// <summary>
        /// 讲座记录编号
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 参与讲座的市场老师
        /// </summary>
        public string TerCharName { get; set; }
        /// <summary>
        /// 讲座人
        /// </summary>
        public string ChairName { get; set; }
        /// <summary>
        /// 观众人数
        /// </summary>
        public Nullable<int> ManCount { get; set; }
        /// <summary>
        /// 讲座地点
        /// </summary>
        public string ChairAddress { get; set; }
        /// <summary>
        /// 讲座日期
        /// </summary>
        public DateTime ChairTime { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string Employees_Id { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Rmark { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        
    }
}
