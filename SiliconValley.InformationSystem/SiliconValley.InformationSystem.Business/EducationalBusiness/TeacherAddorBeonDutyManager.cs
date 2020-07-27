﻿using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class TeacherAddorBeonDutyManager:BaseBusiness<TeacherAddorBeonDuty>
    {
        public TeacherBusiness Teacher_Entity = new TeacherBusiness();
        #region 查询
        /// <summary>
        /// 获取值班视图所有数据
        /// </summary>
        /// <returns></returns>
        public List<TeacherAddorBeonDutyView> GetViewAll()
        {
           return this.GetListBySql<TeacherAddorBeonDutyView>("select * from TeacherAddorBeonDutyView");
        }
        /// <summary>
        /// 判断是否有已存在该数据
        /// </summary>
        /// <param name="evning"></param>
        /// <param name="date"></param>
        /// <param name="emp"></param>
        /// <returns></returns>
        public bool Exits(int evning ,DateTime date,string emp)
        {
            string sql = "select * from TeacherAddorBeonDutyView where evning_Id=" + evning + " and Anpaidate='" + date+ "' and Tearcher_Id='" + emp+"'";
            int count= this.GetListBySql<TeacherAddorBeonDutyView>(sql).Count;

            return count > 0 ? true : false;
        }
        /// <summary>
        /// 根据id查找数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherAddorBeonDuty Findid(int id)
        {
            string sql = "select * from TeacherAddorBeonDuty where Id=" + id;
            List<TeacherAddorBeonDuty> list= this.GetListBySql<TeacherAddorBeonDuty>(sql);

            return list.Count > 0 ? list[0] : null;
        }
        /// <summary>
        /// 根据id查找视图数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherAddorBeonDutyView FindViewid(int id)
        {
            string sql = "select * from TeacherAddorBeonDutyView where Id=" + id;
            List<TeacherAddorBeonDutyView> list = this.GetListBySql<TeacherAddorBeonDutyView>(sql);

            return list.Count > 0 ? list[0] : null;
        }
        /// <summary>
        /// 通过sql获取视图数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<TeacherAddorBeonDutyView> AttendSqlGetData(string sql)
        {
           return this.GetListBySql<TeacherAddorBeonDutyView>(sql);
        }
        #endregion

        #region 添加、编辑、删除数据
        /// <summary>
        /// 数据添加
        /// </summary>
        /// <param name="teacherAddor"></param>
        /// <returns></returns>
        public AjaxResult Add_data(TeacherAddorBeonDuty teacherAddor)
        {
            AjaxResult a = new AjaxResult() { Success=true,Msg="操作成功！"};

            try
            {
                this.Insert(teacherAddor);
            }
            catch (Exception)
            {

                a.Msg = "操作失败";
                a.Success = false;
            }

            return a;
        }
        
        public AjaxResult Add_data(List<TeacherAddorBeonDuty> teacherAddor)
        {
            AjaxResult a = new AjaxResult() { Success = true, Msg = "操作成功！" };
            try
            {
                this.Insert(teacherAddor);
            }
            catch (Exception)
            {
                a.Msg = "操作失败";
                a.Success = false;
            }

            return a;
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="teacherAddor"></param>
        /// <returns></returns>
        public AjaxResult Del_data(TeacherAddorBeonDuty teacherAddor)
        {
            AjaxResult a = new AjaxResult() { Success=true,Msg="操作成功！"};
            try
            {
                this.Delete(teacherAddor);
            }
            catch (Exception)
            {
                a.Success = false;
                a.Msg = "操作失败！";
                 
            }

            return a;
        }
        
        public AjaxResult Del_data(List<TeacherAddorBeonDuty> teacherAddor)
        {
            AjaxResult a = new AjaxResult() { Success = true, Msg = "操作成功！" };
            try
            {
                this.Delete(teacherAddor);
            }
            catch (Exception)
            {
                a.Success = false;
                a.Msg = "操作失败！";

            }

            return a;
        }
        #endregion
    }
}
