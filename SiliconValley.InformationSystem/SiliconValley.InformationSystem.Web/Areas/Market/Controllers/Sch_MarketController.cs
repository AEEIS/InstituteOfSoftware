﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.Consult_Business;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class Sch_MarketController : BaseMvcController
    {
        // GET: /Market/Sch_Market/UpdateFunction
        Sch_MarketManeger s_entity = new Sch_MarketManeger();
        /// <summary>
        /// 数据详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sch_MarketDetailsView(int id)
        {
            Sch_Market s = s_entity.GetEntity(id);
            return View(s);
        }

        /// <summary>
        /// 数据编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Sch_MarketEdit(int id)
        {
            Sch_Market s = s_entity.GetEntity(id);
            ViewBag.selectconsult = EmployandCounTeacherCoom.getallCountTeacher(false);

            //获取市场部的所有员工
            List<SelectListItem> list= EmployandCounTeacherCoom.Studentrecond.Enplo_Entity.GetEmpsByDeptid(3).Select(p=>new SelectListItem() { Text=p.EmpName,Value=p.EmpName}).ToList();
            ViewBag.empmarket = list;

            //获取区域
           List<SelectListItem> list2= EmployandCounTeacherCoom.Studentrecond.region_Entity.GetList().Where(r => r.IsDel == false).Select(r => new SelectListItem() { Text = r.RegionName, Value = r.RegionName }).ToList();

            ViewBag.region = list2;

            List<SelectListItem> list3 = EmployandCounTeacherCoom.Studentrecond.StuInfomationType_Entity.GetList().Where(t=>t.IsDelete==false).Select(t => new SelectListItem() { Text = t.Name, Value = t.Name }).ToList();
            ViewBag.infomation = list3;

            return View(s);
        }
        /// <summary>
        /// 编辑方法
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateFunction(Sch_Market news)
        {
            Sch_Market old= s_entity.GetEntity(news.Id);
            bool s = true;
            AjaxResult a = new AjaxResult();
             string Teacher=  Request.Form["TeacherId"];
            if (!string.IsNullOrEmpty(Teacher))
            {
                ConsultManeger c_entiey = new ConsultManeger();
                //判断是否指派了
                Consult find= c_entiey.AccordingStuIdGetConsultData(old.Id);
                if (find==null)
                {
                    //修改了或指派咨询师
                    Consult consult = new Consult();
                    consult.TeacherName = Convert.ToInt32(Teacher);
                    consult.StuName = old.Id;
                    consult.ComDate = DateTime.Now;
                    consult.IsDelete = false;


                    s = c_entiey.AddSing(consult);
                    a.Success = s;
                }
                else
                {
                    //修改数据
                    find.TeacherName= Convert.ToInt32(Teacher);
                    s= c_entiey.MyUpdate(find).Success;
                }                               
            }
            
            if (s)
            {
                old.StudentName = news.StudentName;
                old.Sex = news.Sex;
                old.Phone = news.Phone;
                old.Remark = news.Remark;
                old.QQ = news.QQ;
                old.School = news.School;
                old.Inquiry = news.Inquiry;
                old.Source = news.Source;
                old.Area = news.Area;
                old.Education = news.Education;
                old.Info = news.Info;
                old.SalePerson = news.SalePerson;
                old.RelatedPerson = news.RelatedPerson;
                old.Age = news.Age;
                old.MarketState = news.MarketState;
                old.MarketType = news.MarketType;

                a = s_entity.MyUpdate(old);
            }
            else
            {
                a.Msg = "系统错误，请刷新重试！！！";
            }
             

            return Json(a,JsonRequestBehavior.AllowGet);
        }
    }
}