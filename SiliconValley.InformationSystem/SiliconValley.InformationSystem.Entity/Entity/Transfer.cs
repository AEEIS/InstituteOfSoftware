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
    /// 转班
    /// </summary>
    [Table(name: "Transfer")]
    public partial class Transfer
    {
    
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string Studentnumber { get; set; }
        /// <summary>
        /// 转班原因
        /// </summary>
        public string Reasonsforshifting { get; set; }
        /// <summary>
        /// 转班日期
        /// </summary>
        public Nullable<System.DateTime> TransferDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> Dateofregistration { get; set; }
        /// <summary>
        /// 希望转入班级
        /// </summary>
        public int Hopetoransfer { get; set; }
    

    }
}
