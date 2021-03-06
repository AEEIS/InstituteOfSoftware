﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
 

namespace SiliconValley.InformationSystem.Business.EmployeesBusiness
{
    /// <summary>
    ///  用于数据备案一系列的操作类
    /// </summary>
   public static class EmployandCounTeacherCoom
    {
       public static ConsultTeacherManeger Consult_entity = new ConsultTeacherManeger();
       public static ConsultManeger consult = new ConsultManeger();
       public static StudentDataKeepAndRecordBusiness Studentrecond= new StudentDataKeepAndRecordBusiness();

       public static FollwingInfoManeger follwing = new FollwingInfoManeger();
        /// <summary>
        /// 获取所有的咨询师，然后转化为员工集合
        /// </summary>
        /// <param name="all">false---返回在职咨询师，true--返回全部咨询师</param>
        /// <returns></returns>
        public static List<Emp_consult> getallCountTeacher(bool all)
        {             
            EmployeesInfoManage employeesInfo = new EmployeesInfoManage();
            List<Emp_consult> list = new List<Emp_consult>();//在职咨询师
            List<Emp_consult> list2 = new List<Emp_consult>();//全部咨询师
            Consult_entity.GetList().ForEach(c =>
            {
                Emp_consult e = new Emp_consult();
                e.consultercherid = c.Id;
                e.empname = employeesInfo.GetEntity(c.Employees_Id).EmpName;
                if (all) 
                {                     
                    list2.Add(e);                     
                }
                else 
                {
                    if (c.IsDelete == false)
                    {
                         list.Add(e);
                    }
                   
                }
                
            });

            return all==false? list:list2;


        }

        /// <summary>
        /// 添加分量数据
        /// </summary>
        /// <returns></returns>
        public static bool AddConsult(Consult consult)
        {
            bool s = false;
            ConsultManeger cont = new ConsultManeger();
            try
            {
                cont.Insert(consult);
                s = true;
            }
            catch (Exception)
            {

                s = false;
            }
            return s;
        }

         
        /// <summary>
        //根据咨询师编号获取员工名称
        /// </summary>
        /// <param name="c_id"></param>
        /// <returns></returns>
        public static string ConsultName(int c_id)
        {
            ConsultTeacherManeger consult = new ConsultTeacherManeger();
            ConsultTeacher findata= consult.GetEntity(c_id);
            EmployeesInfoManage business = new EmployeesInfoManage();
            return business.GetEntity(findata.Employees_Id).EmpName;
        }
        
        /// <summary>
        /// 根据员工编号获取咨询师编号
        /// </summary>
        /// <param name="idName"></param>
        /// <returns></returns>
        public static ConsultTeacher GetConsultTeacherId(string idName)
        {
           return Consult_entity.GetList().Where(c => c.Employees_Id == idName).FirstOrDefault();
        }

        /// <summary>
        /// 批量添加分量数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static AjaxResult AddConsultData(List<Consult> list)
        {
           return consult.Add_Data(list);
        }


        public static List<ConsultTeacher> GetTeacher()
        {
            EmployeesInfoManage employeesInfo = new EmployeesInfoManage();
           return Consult_entity.GetList().Select(c => new ConsultTeacher() { Employees_Id = employeesInfo.GetEntity(c.Employees_Id).EmpName, Id = c.Id }).ToList();
        }
             
        /// <summary>
        /// 添加咨询跟踪数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static AjaxResult AddFllow(List<FollwingInfo> list)
        {
            AjaxResult a = new AjaxResult();
            a.Success = follwing.Addlist(list);
            return a;
        }


        public static Consult findConsult(int sid)
        {
           return consult.AccordingStuIdGetConsultData(sid);
        }
 
       
    }
}
