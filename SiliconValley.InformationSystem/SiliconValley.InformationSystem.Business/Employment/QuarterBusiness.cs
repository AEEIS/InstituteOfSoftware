﻿using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业毕业计划业务类
    /// </summary>
    public class QuarterBusiness : BaseBusiness<Quarter>
    {
        /// <summary>
        /// 获取全部可用的就业计划
        /// </summary>
        /// <returns></returns>
        public List<Quarter> GetQuarters()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 年度 eg：2019 /2020
        /// </summary>
        /// <returns></returns>
        public List<EmploymentYearView> yearplan()
        {
            var data = this.GetQuarters();
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = data.Count - 1; j > i; j--)
                {
                    if (data[i].RegDate.Year == data[j].RegDate.Year)
                    {
                        data.Remove(data[j]);
                    }
                }
            }

            List<EmploymentYearView> result = new List<EmploymentYearView>();
            foreach (var item in data)
            {
                EmploymentYearView view = new EmploymentYearView();
                view.YearTitle = item.RegDate.Year + "年度";
                view.Year = item.RegDate.Year;
                result.Add(view);
            }
           
            return result;
        }

        /// <summary>
        /// 根据年度获取这次年度的记录
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Quarter> GetQuartersByYear(int year) {
          return  this.GetQuarters().Where(a => a.RegDate.Year == year).ToList();
        }

       
    }
}
