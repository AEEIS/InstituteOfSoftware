﻿using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    [CheckLogin]
    public class CDinterviewController : Controller
    {
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private EmpClassBusiness dbempClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProClassSchedule dbproClassSchedule;
        private SpecialtyBusiness dbspecialty;
        private CDInterviewBusiness dbcDInterview;
        private SurveyRecordsBusiness dbsurveyRecords;
        private EmploymentStaffBusiness dbemploymentStaff;
        // GET: Obtainemployment/CDinterview
        public ActionResult CDinterviewIndex()
        {
            dbempClass = new EmpClassBusiness();
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var list = dbempClass.GetEmpClassesByempinfoid(user.EmpNumber);
            var aa = list.Select(a => new
            {
                ClassNumber =a.ClassId
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
        }

        [HttpGet]
        /// <summary>
        /// 访谈学生页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CDSResearchRecordRegister(int param0)
        {
            dbproClassSchedule = new ProClassSchedule();
           var query= dbproClassSchedule.GetEntity(param0);
            ViewBag.param0 = query.ClassNumber;
            ViewBag.param1 = param0;
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult SResearchRecordRegister(CDInterview param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbemploymentStaff = new EmploymentStaffBusiness();
                var query = dbemploymentStaff.Getloginuser();
                dbcDInterview = new CDInterviewBusiness();
                param0.CDIntDate = DateTime.Now;
                param0.EmpStaffID = query.ID;
                param0.IsDel = false;
                dbcDInterview.Insert(param0);
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {

                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="param0">班级id</param>
        /// <returns></returns>
        public ActionResult getmudata(int param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbproStudentInformation = new ProStudentInformationBusiness();
                dbproClassSchedule = new ProClassSchedule();
                dbspecialty = new SpecialtyBusiness();
                dbsurveyRecords = new SurveyRecordsBusiness();
                SResearchRecordRegisterView view = new SResearchRecordRegisterView();
                var queryclass = dbproClassSchedule.GetEntity(param0);
                var cdlist = dbsurveyRecords.GetCDSurveyRecordsByclassid(param0);
                List<StudentInformation> studentlist = new List<StudentInformation>();

                foreach (var item in cdlist)
                {
                    studentlist.Add(dbproStudentInformation.GetEntity(item.StudentNO));
                }


                view.Studentlist = studentlist.Select(aa => new
                {
                    StudentNumber = aa.StudentNumber,
                    Name = aa.Name
                }).ToList();

                var specialtylist = dbspecialty.GetIQueryable().Where(a => a.IsDelete == false).ToList();

                view.Specialtylist = specialtylist.Select(a => new
                {
                    Id = a.Id,
                    SpecialtyName = a.SpecialtyName
                }).ToList();
                ajaxResult.Success = true;
                ajaxResult.Data = view;
            }
            catch (Exception)
            {

                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 左侧表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public ActionResult SearchData0(int page, int limit, string param0, string param1)
        {
            //CacheHelper.Cache.RemoveCache("Coldairarrow.Fx.Net.Easyui.GitHub_Cache_Base_UserModel_Obtain");
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbempClass = new EmpClassBusiness();
            dbproClassSchedule = new ProClassSchedule();
            List<EmpClass> result = new List<EmpClass>();
            if (dbemploymentJurisdiction.isstaffJurisdiction(user))
            {
                result = dbempClass.GetIQueryable().Where(a => a.IsDel == false).ToList();
            }
            else
            {
                result = dbempClass.GetEmpClassesByempinfoid(user.EmpNumber);
            }
            switch (param0)
            {
                case "ing":
                    result = dbempClass.Leavebehinding(result);
                    break;
                case "ed":
                    result = dbempClass.Leavebehinded(result);
                    break;
                default:
                    result = dbempClass.Leavebehinding(result);
                    break;
            }

            if (!string.IsNullOrEmpty(param1))
            {
                result = dbempClass.CorrespondingByClassNumber(result, param1);
            }
            var aa = dbempClass.ConversiontoCD(result);

            var resultdata1 = aa.OrderByDescending(a => a.classid).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = aa.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 右侧
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="param0">班级编号</param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string param0,string param1)
        {
           
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbspecialty = new SpecialtyBusiness();
            dbcDInterview = new CDInterviewBusiness();

            var list1 =new List<CDInterview>();
            if (!string.IsNullOrEmpty(param0))
            {
                list1= dbcDInterview.GetCDInterviewsByClassid(int.Parse(param0));
            }
          

            var aa = list1.Select(a => new
            {
                ID = a.ID,
                StudentName = dbproStudentInformation.GetEntity(a.StudentNO).Name,
                EmpStaffName = dbemploymentStaff.GetEmpInfoByEmpID(a.EmpStaffID).EmpName,
                CDIntContent=a.CDIntContent,
                Remark = a.Remark,
                CDIntDate=a.CDIntDate,
                WantSpceName = dbspecialty.GetEntity(a.MajorID).SpecialtyName
            }).ToList();
            if (!string.IsNullOrEmpty(param1))
            {
               aa= aa.Where(a => a.StudentName == param1).ToList();
            }
            var resultdata1 = aa.OrderByDescending(a => a.CDIntDate).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = aa.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="param0">cd访谈记录id</param>
        /// <returns></returns>
        public ActionResult del(int param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbcDInterview = new CDInterviewBusiness();
                CDInterview cDInterview = dbcDInterview.GetEntity(param0);
                cDInterview.IsDel = true;
                dbcDInterview.Update(cDInterview);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
  
        }


        /// <summary>
        /// 修改cd 访谈记录
        /// </summary>
        /// <param name="param0">cd 访谈记录id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult edit(int param0) {
            dbcDInterview = new CDInterviewBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            CDInterview cDInterview = dbcDInterview.GetEntity(param0);
            ViewBag.data = Newtonsoft.Json.JsonConvert.SerializeObject(cDInterview);
            ViewBag.data1 = dbproStudentInformation.GetEntity(cDInterview.StudentNO).Name;
            ViewBag.data2 = dbproScheduleForTrainees.GetTraineesByStudentNumber(cDInterview.StudentNO).ClassID;
            return View();
        }

        /// <summary>
        /// 修改提交
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult edit(CDInterview param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbcDInterview = new CDInterviewBusiness();
               CDInterview cD= dbcDInterview.GetEntity(param0.ID);
                dbemploymentStaff = new EmploymentStaffBusiness();
                var query = dbemploymentStaff.Getloginuser();
                cD.CDIntContent = param0.CDIntContent;
                cD.EmpStaffID = query.ID;
                cD.MajorID = param0.MajorID;
                cD.Remark = param0.Remark;
                dbcDInterview.Update(cD);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}