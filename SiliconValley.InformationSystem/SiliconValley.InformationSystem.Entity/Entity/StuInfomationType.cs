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

    [Table(name: "StuInfomationType")]
    public partial class StuInfomationType
    {
       
    [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rmark { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        
    }
}
