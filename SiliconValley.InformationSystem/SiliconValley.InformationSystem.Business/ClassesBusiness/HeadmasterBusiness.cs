﻿using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ClassesBusiness
{
  public  class HeadmasterBusiness:BaseBusiness<Headmaster>
    {
        //添加班主任
        public bool AddHeadmaster(string informatiees_Id)
        {
            bool str = true;
          
            try
            {
                Headmaster informatiees = new Headmaster();
            informatiees.IsDelete = false;
            informatiees.AddTime = DateTime.Now;
            informatiees.informatiees_Id = informatiees_Id;
                
                this.Insert(informatiees);
                BusHelper.WriteSysLog("添加班主任成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

            }
            catch (Exception ex)
            {
                
                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return str;

        }


        //班主任离职时间
        public bool QuitEntity(string informatiees_Id)
        {
            bool str = true;
            try
            {
                var x = this.GetEntity(informatiees_Id);
                x.IsDelete = true;
                this.Update(x);
            }
            catch (Exception ex)
            {

                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            
            }
            return str;
        }
    }
}