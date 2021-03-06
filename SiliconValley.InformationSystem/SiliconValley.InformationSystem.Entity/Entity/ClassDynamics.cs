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
    /// 学员异动表
    /// </summary>
    [Table("ClassDynamics")]
    public partial class ClassDynamics
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public Nullable<System.DateTime> Addtime { get; set; }
        /// <summary>
        /// 之前班级
        /// </summary>
        public int FormerClass { get; set; }
        /// <summary>
        /// 当前班级
        /// </summary>
        public int CurrentClass { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string Studentnumber { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public Nullable<int> States { get; set; }
        /// <summary>
        /// 转班id
        /// </summary>
        public Nullable<int> TransferID { get; set; }
        /// <summary>
        /// 试学id、
        /// </summary>
        public Nullable<int> TrialapplctionID { get; set; }
        /// <summary>
        /// 复学id
        /// </summary>
        public Nullable<int> RestudyID { get; set; }
        /// <summary>
        /// 退学id
        /// </summary>
        public Nullable<int> ApplicationDropoutID { get; set; }
        /// <summary>
        /// 重修id
        /// </summary>
        public Nullable<int> ApplicationRepairID { get; set; }
        /// <summary>
        /// 休学id
        /// </summary>
        public Nullable<int> SuspensionofschoolID { get; set; }
        /// <summary>
        /// 开除id
        /// </summary>
        public Nullable<int> ExpelsID { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public Nullable<bool> IsaDopt { get; set; }
        /// <summary>
        /// 宿舍地址
        /// </summary>
        public string Dormitoryaddress { get; set; }
    }
}
