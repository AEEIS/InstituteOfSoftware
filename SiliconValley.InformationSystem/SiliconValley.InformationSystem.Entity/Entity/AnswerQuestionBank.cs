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
    
    public partial class AnswerQuestionBank
    {
       
        public int ID { get; set; }
        public string Title { get; set; }
        public string ReferenceAnswer { get; set; }
        public Nullable<int> Course { get; set; }
        public Nullable<int> Proposition { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<bool> IsUsing { get; set; }
        public string Remark { get; set; }
    
     
    }
}
