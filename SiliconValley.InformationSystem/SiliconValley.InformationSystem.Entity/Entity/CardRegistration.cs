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
    [Table("CardRegistration")]
    //宿舍领电子卡登记表
    public partial class CardRegistration
    {
        [Key]
        public int ID { get; set; }
        //是否删除
        public Nullable<bool> Dateofregistration { get; set; }
        //添加时间
        public Nullable<System.DateTime> Add_time { get; set; }
        //备注
        public string Remarks { get; set; }
        //日期
        public string Departmentname { get; set; }
        //学号
        public string StudentNumber { get; set; }
        //寝室号
        public Nullable<int> Dormitorynumber { get; set; }
    }
}
