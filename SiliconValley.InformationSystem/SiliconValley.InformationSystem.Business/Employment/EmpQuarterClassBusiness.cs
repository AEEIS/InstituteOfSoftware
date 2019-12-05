﻿using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 处于毕业计划中的班级 中间表业务类
    /// </summary>
    public class EmpQuarterClassBusiness : BaseBusiness<EmpQuarterClass>
    {
        private QuarterBusiness dbquarter;
        private ProClassSchedule dbproClassSchedule;
        /// <summary>
        ///获取可用的数据
        /// </summary>
        /// <returns></returns>
        public List<EmpQuarterClass> GetEmpQuarters()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据季度id返回参与季度的班级中间表对象
        /// </summary>
        /// <returns></returns>
        public List<EmpQuarterClass> GetEmpQuartersByYearID(int QuarterID) {
          return  this.GetEmpQuarters().Where(a => a.QuarterID == QuarterID).ToList();
        }

        /// <summary>
        /// 根据班级id返回这个班级--计划表
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public EmpQuarterClass GetQuartClassByclassid(int classid) {
          return  this.GetEmpQuarters().Where(a => a.Classid == classid).FirstOrDefault();
        }

        /// <summary>
        /// 根据年份 获取这一年毕业的班级
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetClassesByYear(int year)
        {
            dbquarter = new QuarterBusiness();
          var query=  dbquarter.GetQuartersByYear(year);
            dbproClassSchedule = new ProClassSchedule();
            List<ClassSchedule> result = new List<ClassSchedule>();
            List<EmpQuarterClass> list = new List<EmpQuarterClass>();
            foreach (var item in query)
            {
                list.AddRange(this.GetEmpQuartersByYearID(item.ID));
            }
            foreach (var item in list)
            {
                result.Add(dbproClassSchedule.GetEntity(item.Classid));
            }
            return result;
        }

    }
}
