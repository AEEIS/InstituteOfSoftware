﻿using SiliconValley.InformationSystem.Business.DormitoryMantainBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.DormitoryMaintenance.Controllers
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Entity.Entity;
    using SiliconValley.InformationSystem.Util;
    using System.Text;
    public class DormitoryDepositController : Controller
    {
        DormitoryDepositManeger Dormitory_Entity = new DormitoryDepositManeger();
        // GET: /DormitoryMaintenance/DormitoryDeposit/DoubleGetData

        #region 登记人操作

        public ActionResult DormitoryDepositIndex()
        {
            return View();
        }


        /// <summary>
        /// 第一次数据加载
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult GetData(int limit, int page)
        {
            string sql = "select * from DormitoryDeposit";

            List<DormitoryDeposit> listall = Dormitory_Entity.GetListBySql<DormitoryDeposit>(sql);
            var data = listall.OrderByDescending(l => l.CreaDate).Skip((page - 1) * limit).Take(limit).Select(l => new
            {
                ID = l.ID,
                Maintain = l.Maintain,//维修日期
                DorName = Dormitory_Entity.DormInformation_Entity.GetEntity(l.DormId).DormInfoName,//房间编号
                EmpName = Dormitory_Entity.EmployeesInfo_Entity.GetEntity(l.EntryPersonnel).EmpName,//录入人员
                GoodPrice = l.GoodPrice,
                Nameofarticle = Dormitory_Entity.DormitoryMaintenance_Entity.GetEntity(l.MaintainGood).Nameofarticle,//物品名称
                MaintainState = l.MaintainState,
                stuName = Dormitory_Entity.StudentInformation_Entity.GetEntity(l.StuNumber).Name,
            }).ToList();

            var jsondata = new { count = listall.Count, code = 0, data = data };

            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DoubleGetData(int limit, int page)
        {
            string stuname = Request.QueryString["stuName"];
            string startime = Request.QueryString["starTime"];
            string endtime = Request.QueryString["endTime"];

            StringBuilder sql = new StringBuilder("select * from DormitoryDeposit where 1=1");

            if (!string.IsNullOrEmpty(startime))
            {
                sql.Append(" and Maintain>= '" + startime + "'");
            }


            if (!string.IsNullOrEmpty(endtime))
            {
                sql.Append(" and Maintain<= '" + endtime + "'");
            }

            List<DormitoryDeposit> listall = Dormitory_Entity.GetListBySql<DormitoryDeposit>(sql.ToString());
            var data = listall.OrderByDescending(l => l.CreaDate).Skip((page - 1) * limit).Take(limit).Select(l => new
            {
                ID = l.ID,
                Maintain = l.Maintain,//维修日期
                DorName = Dormitory_Entity.DormInformation_Entity.GetEntity(l.DormId).DormInfoName,//房间编号
                EmpName = Dormitory_Entity.EmployeesInfo_Entity.GetEntity(l.EntryPersonnel).EmpName,//录入人员
                GoodPrice = l.GoodPrice,
                Nameofarticle = Dormitory_Entity.DormitoryMaintenance_Entity.GetEntity(l.MaintainGood).Nameofarticle,//物品名称
                MaintainState = l.MaintainState,
                stuName = Dormitory_Entity.StudentInformation_Entity.GetEntity(l.StuNumber).Name,
            }).ToList();


            if (!string.IsNullOrEmpty(stuname))
            {
                data= data.Where(d => d.stuName.Contains(stuname)).ToList();
            }

            var jsondata = new { count = listall.Count, code = 0, data = data };

            return Json(jsondata, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddView()
        {
            //获取宿舍楼地址
            ViewBag.tung = Dormitory_Entity.Tung_Entity.GetList().Select(s => new SelectListItem() { Value = s.Id.ToString(), Text = s.TungName }).ToList();
            //获取维修物品 
            ViewBag.goodname = Dormitory_Entity.DormitoryMaintenance_Entity.GetList().Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Nameofarticle }).ToList();
            return View();

        }

        /// <summary>
        /// 根据地址获取楼层
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAllFoor(int id)
        {
            string sqlstr = @"select t.Id,t.FloorId ,t.CreationTime, d.FloorName as'Remark',t.IsDel,t.TungId from TungFloor as t 
 left join Dormitoryfloor as d on t.FloorId = d.ID
 where t.IsDel = 0 and t.TungId = " + id;
            List<SelectListItem> list = Dormitory_Entity.GetListBySql<TungFloor>(sqlstr).Select(t => new SelectListItem() { Text = t.Remark, Value = t.Id.ToString() }).ToList();

            //tlist.ForEach(t=> {
            //     Dormitory_Entity.Dormitoryfloor_Entity.GetList().Where(d => d.ID == t.FloorId).ToList();
            //});

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取宿舍
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDormitory(int id)
        {
            List<SelectListItem> list = Dormitory_Entity.DormInformation_Entity.GetList().Where(s => s.TungFloorId == id).ToList().Select(s => new SelectListItem() { Text = s.DormInfoName, Value = s.ID.ToString() }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取某个日期中属于XX宿舍的所有学生
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSusheStudent()
        {
            DateTime date = Convert.ToDateTime(Request.Form["date"]);//日期
            int susheNumber = Convert.ToInt32(Request.Form["sushenumber"]);//宿舍编号


            List<Accdationinformation> list = Dormitory_Entity.GetStudentSushe(date, susheNumber);//获取属于这个寝室的所有学生

            List<SelectListItem> studentlist = new List<SelectListItem>();

            list.ForEach(l =>
            {
                StudentInformation student = Dormitory_Entity.StudentInformation_Entity.GetEntity(l.Studentnumber);
                if (student != null)
                {
                    SelectListItem item = new SelectListItem() { Text = student.Name, Value = student.StudentNumber };

                    studentlist.Add(item);
                }
            });

            AjaxResult result = new AjaxResult() { Data = studentlist, Success = true };
            if (list.Count < 0)
            {
                result.Success = false;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFunction()
        {

            AjaxResult result = new AjaxResult() { Success = true, Msg = "登记成功！" };


            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息

            DateTime mantinDate = Convert.ToDateTime(Request.Form["mantinDate"]);//维修的日期

            int weixiugood = Convert.ToInt32(Request.Form["weixiugood"]);//维修物品

            int dorname = Convert.ToInt32(Request.Form["dorname"]);//宿舍编号

            int JieType = Convert.ToInt32(Request.Form["JieType"]);//类型

            Pricedormitoryarticles goods = Dormitory_Entity.Pricedormitoryarticles_Entity.GetEntity(weixiugood);//查询维修物品的信息

            List<DormitoryDeposit> Dormlist = new List<DormitoryDeposit>();//存放学生维修费用

            string sturadi = Request.Form["sturadi"];//学生编号

            if (JieType == 1)
            {

                //寝室平摊
                List<Accdationinformation> list = Dormitory_Entity.GetStudentSushe(mantinDate, dorname); //获取属于这个寝室的学生

                List<StudentInformation> studentlist = new List<StudentInformation>();


                list.ForEach(l =>
                {
                    StudentInformation student = Dormitory_Entity.StudentInformation_Entity.GetEntity(l.Studentnumber);
                    if (student != null)
                    {
                        studentlist.Add(student);

                    }
                });

                if (studentlist.Count > 0)
                {
                    foreach (var stu in studentlist)
                    {
                        DormitoryDeposit dormitory = new DormitoryDeposit();
                        dormitory.ID = Guid.NewGuid().ToSequentialGuid();
                        dormitory.CreaDate = DateTime.Now;
                        dormitory.DormId = dorname;
                        dormitory.EntryPersonnel = UserName.EmpNumber;
                        dormitory.GoodPrice = goods.Reentry / studentlist.Count;
                        dormitory.Maintain = mantinDate;
                        dormitory.MaintainGood = weixiugood;
                        dormitory.MaintainState = 1;
                        dormitory.StuNumber = stu.StudentNumber;

                        Dormlist.Add(dormitory);
                    }

                    bool s = Dormitory_Entity.AddData(Dormlist);

                    if (s == false)
                    {
                        result.Msg = "网络异常，请刷新重试！";
                        result.Success = false;
                    }
                }
                else
                {
                    result.Msg = "该寝室没有学生入住！";
                    result.Success = false;
                }






            }
            else if (JieType == 2)
            {
                //个人承担
                DormitoryDeposit dormitory = new DormitoryDeposit();
                dormitory.ID = Guid.NewGuid().ToSequentialGuid();
                dormitory.CreaDate = DateTime.Now;
                dormitory.DormId = dorname;
                dormitory.EntryPersonnel = UserName.EmpNumber;
                dormitory.GoodPrice = goods.Reentry;
                dormitory.Maintain = mantinDate;
                dormitory.MaintainGood = weixiugood;
                dormitory.MaintainState = 1;
                dormitory.StuNumber = sturadi;


                bool s = Dormitory_Entity.AddData(dormitory);

                if (s == false)
                {
                    result.Msg = "网络异常，请刷新重试！";
                    result.Success = false;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region 财务操作
        public ActionResult FinanceIndex()
        {
            return View();
        }


        public ActionResult SercherData()
        {
            string classnumber= Request.Form["stuName"];

            string stardate = Request.Form["starTime"];

            string endTime = Request.Form["endTime"];



            return null;
        }



        public ActionResult GetStudentData() {
            string stuname = Request.Form["stuname"];
            List<StudentInformation> stulist= Dormitory_Entity.GetListBySql<StudentInformation>("select * from StudentInformation where Name like'%" + stuname + "%'");
            return null;
        }

        #endregion

    }
}