﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity.SalaryView;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class EmpSalaryManagementController : Controller
    {
        //员工工资管理页面
        // GET: Personnelmatters/EmpSalaryManagement
        public ActionResult SalaryManageIndex()
        {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            var time = msrmanage.GetList().Where(s=>s.IsDel==false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
            ViewBag.yearandmonth = mytime; 
            return View();
        }
        //工资表数据加载
        public ActionResult EmpSalaryData(int page, int limit,string AppCondition)
        {
            EmplSalaryEmbodyManage empsemanage = new EmplSalaryEmbodyManage();//员工工资体系表
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            EmployeesInfoManage empmanage = new EmployeesInfoManage();//员工信息表
            var eselist = msrmanage.GetList().Where(s => s.IsDel == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];
                string Empstate = str[3];
                eselist = eselist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname))
                {
                    eselist = eselist.Where(e => empmanage.GetDeptByEmpid(e.EmployeeId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    eselist = eselist.Where(e => empmanage.GetPositionByEmpid(e.EmployeeId).Pid == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(Empstate))
                {
                    eselist = eselist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).IsDel ==bool.Parse(Empstate)).ToList();
                }

            }
            var newlist = eselist.OrderBy(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            List<MySalaryObjView> result = new List<MySalaryObjView>();
            
            foreach (var item in newlist)
            {
                MySalaryObjView view = new MySalaryObjView();
                view.Id = item.Id;//工资编号
                view.EmployeeId = item.EmployeeId;//员工编号
                view.empName = empmanage.GetEntity(item.EmployeeId).EmpName;//员工姓名
                view.Depart = empmanage.GetDeptByEmpid(item.EmployeeId).DeptName;//所属部门
                view.Position = empmanage.GetPositionByEmpid(item.EmployeeId).PositionName;//所属岗位
                view.EmpState= empmanage.GetEntity(item.EmployeeId).IsDel;
                //拿到该员工工资体系对象
                var eseobj = msrmanage.GetEmpsalaryByEmpid(item.EmployeeId);
                view.baseSalary = eseobj.BaseSalary;//基本工资
                view.positionSalary = eseobj.PositionSalary;//岗位工资
                if (msrmanage.GetMCByEmpid(item.EmployeeId, (DateTime)item.YearAndMonth) == null)
                {
                    view.finalGrade =null;//绩效分

                }
                else {
                    view.finalGrade = msrmanage.GetMCByEmpid(item.EmployeeId, (DateTime)item.YearAndMonth).FinalGrade;
                   
                }
                if (view.finalGrade == null)
                {
                    view.PerformanceSalary = null;//绩效工资
                }
                else {
                    view.PerformanceSalary =msrmanage.GetempPerformanceSalary(view.finalGrade,eseobj.PerformancePay);
                }
              
                view.netbookSubsidy = eseobj.NetbookSubsidy;//笔记本补助
                view.socialSecuritySubsidy = eseobj.SocialSecuritySubsidy;//社保补贴
                #region 应发工资1赋值
                var one = view.baseSalary + view.positionSalary;
              
                view.SalaryOne = msrmanage.GetSalaryone(one,view.PerformanceSalary,view.netbookSubsidy,view.socialSecuritySubsidy);
                #endregion
                //考勤表对象
                var attendobj = msrmanage.GetAttendanceInfoByEmpid(item.EmployeeId, (DateTime)item.YearAndMonth);
                if (attendobj == null)
                {
                    view.toRegularDays = null;//到勤天数
                    view.leavedays = null;//请假天数
                }
                else
                {
                    view.toRegularDays = attendobj.ToRegularDays;
                    view.leavedays = attendobj.LeaveDays;
                    view.LeaveDeductions = msrmanage.GetLeaveDeductions(view.Id, one, view.PerformanceSalary, attendobj.DeserveToRegularDays, view.leavedays);//请假扣款
                    view.TardyWithhold = attendobj.TardyWithhold;//迟到扣款
                    view.LeaveWithhold = attendobj.LeaveWithhold;//早退扣款
                    view.NoClockWithhold = attendobj.NoClockWithhold;//缺卡扣款
                }

                view.OvertimeCharges = item.OvertimeCharges;//加班费用
                view.Bonus = item.Bonus;//奖金
   
                view.OtherDeductions = item.OtherDeductions;//其他扣款
               
                #region 应发工资2赋值
                view.SalaryTwo =msrmanage.GetSalarytwo(view.SalaryOne, view.OvertimeCharges,view.Bonus,view.LeaveDeductions,view.TardyWithhold,view.LeaveWithhold,view.NoClockWithhold,view.OtherDeductions);
                #endregion
                view.PersonalSocialSecurity = eseobj.PersonalSocialSecurity;//个人社保
                view.PersonalIncomeTax = eseobj.PersonalIncomeTax;//个税
                item.Total=msrmanage.GetTotal(view.Id,view.SalaryTwo,view.PersonalSocialSecurity,view.PersonalIncomeTax);
                view.Total = item.Total;//合计
                view.PayCardSalary = msrmanage.GetPaycardSalary(view.Id,view.Total,view.PersonalSocialSecurity,eseobj.ContributionBase);//工资卡工资
                view.CashSalary =msrmanage.GetCashSalary(view.Id,view.Total,view.PayCardSalary);//现金工资
                result.Add(view);
            }
            
             var newobj = new
            {
                code = 0,
                msg = "",
                count = eselist.Count(),
                data = result
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

       /// <summary>
       /// 年月份改变
       /// </summary>
       /// <returns></returns>
        public ActionResult UpdateTime() {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
            ViewBag.time = mytime;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateTime(string CurrentTime) {
            var AjaxResultxx = new AjaxResult();
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            try
            {
                var msrlist=msrmanage.GetList().Where(s=>s.IsDel==false).ToList();
                for (int i = 0; i < msrlist.Count(); i++)
                {
                    msrlist[i].YearAndMonth =Convert.ToDateTime(CurrentTime);
                    msrmanage.Update(msrlist[i]);
                    AjaxResultxx = msrmanage.Success();
                }
            }
            catch (Exception ex)
            {
               AjaxResultxx= msrmanage.Error(ex.Message);
            }   
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 工资表中员工禁用
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSalaryManageEmp(string list)
        {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            AttendanceInfoManage admanage = new AttendanceInfoManage();
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                string[] ids = list.Split(',');
                for (int i = 0; i < ids.Length - 1; i++)
                {
                    string id = ids[i];
                    var ad = msrmanage.GetEntity(int.Parse(id));
                  AjaxResultxx.Success=  msrmanage.EditEmpMS(ad.EmployeeId);
                    if (AjaxResultxx.Success) {
                        bool e = esemanage.EditEmpSalaryState(ad.EmployeeId);//员工体系表禁用该员工
                        AjaxResultxx.Success = e;
                    }
                    if (AjaxResultxx.Success)
                    {
                        bool a = admanage.EditEmpStateToAds(ad.EmployeeId);//员工考勤表禁用该员工
                        AjaxResultxx.Success = a;
                    }
                    if (AjaxResultxx.Success)
                    {
                        bool e = mcmanage.EditEmpStateToMC(ad.EmployeeId);//员工绩效表禁用该员工
                        AjaxResultxx.Success = e;
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = msrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditEmpSalary(int id) {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();
             var msr=msrmanage.GetEntity(id);
            ViewBag.id = id;
            return View(msr);
        }
        public ActionResult GetMSRById(int id)
        {
            MonthlySalaryRecordManage esemanage = new MonthlySalaryRecordManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var ese = esemanage.GetEntity(id);
            var newobj = new
            {
                ese.Id,
                ese.EmployeeId,
                empName = empmanage.GetEntity(ese.EmployeeId).EmpName,
                deptName = empmanage.GetDeptByEmpid(ese.EmployeeId).DeptName,
                pName = empmanage.GetPositionByEmpid(ese.EmployeeId).PositionName,
                ese.Bonus,
                ese.OvertimeCharges,
                ese.OtherDeductions,
                ese.IsDel
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditEmpSalary(MonthlySalaryRecord msr) {
            var AjaxResultxx = new AjaxResult();
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();
            try
            {
                var mobj = msrmanage.GetEntity(msr.Id);
                mobj.OvertimeCharges = msr.OvertimeCharges;
                mobj.Bonus = msr.Bonus;
                mobj.OtherDeductions = msr.OtherDeductions;
                msrmanage.Update(mobj);
                AjaxResultxx = msrmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = msrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
    }
}