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
    [Table(name: "Grand")]
    public partial class Grand
    {

        [Key]
        public int Id { get; set; }
        public string GrandName { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Reaks { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }


        public static bool IsInList(List<Grand> sources, Grand grand)
        {
            foreach (var item in sources)
            {
                if (item.Id == grand.Id)
                    return true;
            }
            return false;
        }

    }
}
