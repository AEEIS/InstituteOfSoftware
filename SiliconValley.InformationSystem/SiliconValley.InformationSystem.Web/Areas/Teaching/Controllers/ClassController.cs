﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.Entity;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;

    [CheckLogin]
    public class ClassController : Controller
    {
        // GET: Teaching/Class


        private readonly TeacherClassBusiness db_teacherclass;
        private readonly TeacherBusiness db_teacher;
        private readonly StuHomeWorkBusiness db_homework;

        /// <summary>
        /// 学员对应班级的业务类实例
        /// </summary>
        private readonly BaseBusiness<ScheduleForTrainees> db_student_class;

        public ClassController()
        {
            db_homework = new StuHomeWorkBusiness();
            db_teacherclass = new TeacherClassBusiness();
            db_teacher = new TeacherBusiness();
            db_student_class = new BaseBusiness<ScheduleForTrainees>();
        }

        public ActionResult Index()
        {

            //提供我的班级列表

            //获取当前登陆的教员

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacherlist = db_teacher.GetTeachers();

            var teacher = teacherlist.Where(d=>d.EmployeeId==user.EmpNumber).FirstOrDefault();

            var myclasslist =  db_teacherclass.GetCrrentMyClass(teacher.TeacherID);

            List<ClassTableView> resultlist = new List<ClassTableView>();

            foreach (var item in myclasslist)
            {

               var temp = db_teacherclass.GetClassTableView(item);

                if (temp != null)
                    resultlist.Add(temp);
            }

            // 

            //ViewBag.Fclasslist = classlist;


            ViewBag.classlist = resultlist;


            return View();
        }


        public ActionResult LoadOtherClass()
        {
            AjaxResult result = new AjaxResult();

            try
            {
                var teacherlist = db_teacher.GetTeachers();

                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                List<ClassTableView> classlist = new List<ClassTableView>();

                SatisfactionSurveyBusiness dd = new SatisfactionSurveyBusiness();

                var emplist = dd.GetMyDepEmp(user);

                foreach (var item in emplist)
                {
                    if (item.EmployeeId != user.EmpNumber)
                    {
                        var tempteacher = teacherlist.Where(d => d.EmployeeId == item.EmployeeId).FirstOrDefault();

                        if (tempteacher != null)
                        {
                            var tempteachaerclass = db_teacherclass.GetCrrentMyClass(tempteacher.TeacherID);

                            foreach (var item1 in tempteachaerclass)
                            {
                                var temp = db_teacherclass.GetClassTableView(item1);

                                classlist.Add(temp);
                            }
                        }
                    }
                }

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = classlist;
            }
            catch (Exception)
            {
                result.ErrorCode = 500;
                result.Msg = "失败";
                result.Data = null;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获取班级学员
        /// </summary>
        /// <param name="classnumber">班级编号</param>
        /// <returns></returns>
        public ActionResult GetStudentByClass(int classnumber)
        {

            ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();

            //var list = scheduleForTraineesBusiness.ClassStudent(classnumber);

           var list = db_teacherclass.GetStudentByClass(classnumber);

            List<StudentDetailView> resultlist = new List<StudentDetailView>();

            //foreach (var item in list)
            //{
            //   var temp = db_teacherclass.GetStudetentDetailView(item);

            //    if (temp != null)
            //        resultlist.Add(temp);
            //}

            return Json(list, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 学生详细页面
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult StudentDetailInfo(string studentnumber)
        {

           var student = db_teacherclass.GetStudentByNumber(studentnumber);

           var teacher = db_teacherclass.GetStudetentDetailView(student);

            return View(teacher);

        }



        /// <summary>
        ///获取班级详细信息
        /// </summary>
        /// <param name="classnumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetClassInfo(string classnumber)
        {

            AjaxResult result = new AjaxResult();

            try
            {
                var temp1 = db_teacherclass.GetClassByClassNumber(classnumber);

               var obj = db_teacherclass.GetClassTableView(temp1);

                result.Data = obj;
                result.ErrorCode = 200;
                result.Msg="成功";

            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;
            }


            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查看学生作业提交情况
        /// </summary>
        /// <param name="studentnumber">学员编号</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult StuHomeWorkSubmission(string studentnumber)
        {

            ViewBag.studentnumber = studentnumber;

            return View();
        }


        /// <summary>
        /// 记录学生作业的未完成记录
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecordStuHomeWorkSubmission(string studentnumber)
        {
           StudentInformation student = db_teacherclass.GetStudentByNumber(studentnumber);


            return View(student);
        }


        /// <summary>
        /// 记录学生作业的未完成记录
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RecordStuHomeWorkSubmission(HomeWorkFinishRate homeWork)
        {
            AjaxResult result = new AjaxResult();

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            var ClassroomHomeWork = Request.Form["ClassroomHomeWork"];
           var AfterClassHomeWork = Request.Form["AfterClassHomeWork"];

            homeWork.AfterClassHomeWork = AfterClassHomeWork == "on" ? false : true;
            homeWork.ClassroomHomeWork = ClassroomHomeWork == "on" ? false : true;
            homeWork.CheckDate = DateTime.Now;
            homeWork.ChekTeacher = teacher.TeacherID;
            homeWork.IsDel = false;

            try
            {
                //首先判断是否重复记录

                if (db_homework.GetHomeWorkFinishRates().Where(d => d.StudentNumber == homeWork.StudentNumber && d.ReleaseDate == homeWork.ReleaseDate).FirstOrDefault() == null)
                {
                    db_homework.Insert(homeWork);

                    result.Data = null;
                    result.ErrorCode = 200;
                    result.Msg = "成功";
                }
                else
                {
                    result.Data = null;
                    result.ErrorCode = 501;
                    result.Msg = "失败";
                }

                Base_UserBusiness.WriteSysLog("成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

                Base_UserBusiness.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetStuHomeWorkStatus(string studentnumber, string Month)
        {

            AjaxResult result = new AjaxResult();

            List<StuHomeWorkView> resultlist = new List<StuHomeWorkView>();
            try
            {
                DateTime date = DateTime.Parse(Month);

                var temp1 = db_homework.GetHomeWorkFinishRates().Where(d => d.StudentNumber == studentnumber).ToList();


                foreach (var item in temp1)
                {
                    DateTime date1 = item.ReleaseDate;

                    if (date.Year == date1.Year && date.Month == date1.Month)
                    {

                        //转为视图模型
                        StuHomeWorkView workView = new StuHomeWorkView();
                        workView.ID = item.ID;
                        workView.IsDel = item.IsDel;
                        workView.Notes = item.Notes;
                        workView.ReleaseDate = item.ReleaseDate;
                        workView.StudentNumber = item.StudentNumber;
                        workView.AfterClassHomeWork=item.AfterClassHomeWork==true?"完成":"未完成";
                        workView.ClassroomHomeWork= item.ClassroomHomeWork == true ? "完成" : "未完成";


                        resultlist.Add(workView);
                    }

                }

                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg="成功";


            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;
            }

          

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取班级班干部
        /// </summary>
        /// <param name="classnumber"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetClassCadres(int classnumber)
        {

            AjaxResult result = new AjaxResult();

            Dictionary<string, StudentInformation> list = new Dictionary<string, StudentInformation>();

            try
            {
                list =  db_teacherclass.GetClassCadres(classnumber);

                result.Data = list;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = list;
                result.ErrorCode = 500;
                result.Msg =ex.Message ;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetStuHomeWorkDetail(int id)
        {
           var obj = db_homework.GetList().Where(d => d.IsDel == false && d.ID == id).FirstOrDefault(); ;


            //转为视图模型
            StuHomeWorkView workView = new StuHomeWorkView();
            workView.ID = obj.ID;
            workView.IsDel = obj.IsDel;
            workView.Notes = obj.Notes;
            workView.ReleaseDate = obj.ReleaseDate;
            workView.StudentNumber = obj.StudentNumber;
            workView.AfterClassHomeWork = obj.AfterClassHomeWork == true ? "完成" : "未完成";
            workView.ClassroomHomeWork = obj.ClassroomHomeWork == true ? "完成" : "未完成";
            workView.StudentName = db_teacherclass.GetStudentByNumber(obj.StudentNumber).Name;


            return View(workView);
        }


        /// <summary>
        /// 学员详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentDetailData(string studentNumber)
        {
            AjaxResult result = new AjaxResult();

            try
            {
               StudentInformation student = db_teacherclass.GetStudentByNumber(studentNumber);

                var stuDetail = db_teacherclass.GetStudetentDetailView(student);

                result.Data = stuDetail;
                result.Msg = "成功";
                result.ErrorCode = 200;
            }
            catch (Exception ex)
            {

                result.Data = null;
                result.Msg = "失败";
                result.ErrorCode = 500;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 教员带班记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherArrangementRecord()
        {
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            db_teacher.GetMyGrandTeacher(user);

            //提供班级
             ViewBag.classlist = db_teacherclass.GrandClassByUser(user);


            return View();
        }

        public ActionResult TeacherArrangementTeacherData()
        {

            //返回的结果
            resultdtree result = new resultdtree();

            //状态
            dtreestatus dtreestatus = new dtreestatus();


            try
            {

                List<dtreeview> childrendtreedata = new List<dtreeview>();

                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                var emplist = db_teacher.GetMyGrandTeacher(user);

                List<Grand> grandlist = new List<Grand>();
                GrandBusiness dbgrand = new GrandBusiness();
                foreach (var item in emplist)
                {
                  var teacher =  db_teacher.GetTeachers().Where(d => d.EmployeeId == item.EmployeeId).FirstOrDefault();

                    var grand = db_teacher.GetGrandByTeacherID(teacher.TeacherID);

                    foreach (var item1 in grand)
                    {
                        if (!dbgrand.IsContains(grandlist, item1))
                        {
                            grandlist.Add(item1);
                        }
                    }

                   
                }


                for (int i = 0; i < grandlist.Count; i++)
                {
                    //第一层
                    dtreeview seconddtree = new dtreeview();

                    seconddtree.context = grandlist[i].GrandName;
                    seconddtree.last = false;
                    seconddtree.level = 0;
                    seconddtree.nodeId = grandlist[i].Id.ToString();
                    seconddtree.parentId = "0";
                    seconddtree.spread = false;

                    //第二层

                    var tememplist1 = db_teacher.BrushSelectionByGrand(grandlist[i].Id);

                    List<EmployeesInfo> tememplist = new List<EmployeesInfo>();

                    foreach (var item in tememplist1)
                    {
                        var temp = db_teacher.GetEmpByEmpNo(item.EmployeeId);

                        if (temp != null)
                        {
                            tememplist.Add(temp);
                        }
                    }

                    if (tememplist.Count >= 0)
                    {

                        List<dtreeview> Quarterlist = new List<dtreeview>();
                        foreach (var item in tememplist)
                        {
                            dtreeview treeemp = new dtreeview();
                            treeemp.nodeId = item.EmployeeId;
                            treeemp.context = item.EmpName;
                            treeemp.last = true;
                            treeemp.parentId = grandlist[i].Id.ToString();
                            treeemp.level = 1;

                            Quarterlist.Add(treeemp);
                        }

                        seconddtree.children = Quarterlist;


                        childrendtreedata.Add(seconddtree);


                    }
                    else
                    {
                        seconddtree.last = true;
                    }

                }

                result.status = dtreestatus;
                result.data = childrendtreedata;

                dtreestatus.code = "200";
                dtreestatus.message = "操作成功";



            }
            catch (Exception ex)
            {


                dtreestatus.code = "1";
                dtreestatus.message = "操作失败";
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TeacherArrangementData(int page, string empnumber, string classnumber)
        {


            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == empnumber).FirstOrDefault();

            List<ClassTeacher> list = new List<ClassTeacher>();

            if (classnumber != null)
            {
                list = db_teacherclass.TeacherArrangementRecord(classnumber);
                
            }
            else
            {
                list = db_teacherclass.TeacherArrangementRecord(teacher.TeacherID);
            }


            var skplist = list.Skip((page - 1) * 8).Take(8).ToList();

            List<ClassCourseView> viewlist = new List<ClassCourseView>();
            CourseBusiness dbcourse = new CourseBusiness();
            foreach (var item in skplist)
            {
               var temp = dbcourse.ConvertToView(item);

                if (temp != null)
                    viewlist.Add(temp);
            }

            var objresult = new
            {

                status = 0,
                message = "成功",
                total = list.Count,
                data = viewlist.OrderByDescending(d=>d.BeginDate).ToList()

            };

            return Json(objresult, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CommnetView(string studentNumber)
        {

            var currentUser = Base_UserBusiness.GetCurrentUser();

            //获取学生信息
            BaseBusiness<StudentInformation> dbstu = new BaseBusiness<StudentInformation>();

            StudentInformation student = dbstu.GetEntity(studentNumber);

            ViewBag.student = student;

            return View();
        }

        public ActionResult DoToComment(string studentNumber, string Comment)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                BaseBusiness<ThearToStuComment> dbstucomment = new BaseBusiness<ThearToStuComment>();
                var currentUser = Base_UserBusiness.GetCurrentUser();

                ThearToStuComment stuComment = new ThearToStuComment();

                    stuComment.Commnet = Comment;
                    stuComment.CommnetDate = DateTime.Now;
                    stuComment.CommnetEr = currentUser.EmpNumber;
                    stuComment.CommnetObj = studentNumber;

                    dbstucomment.Insert(stuComment);

            
               

                result.ErrorCode = 200;
                result.Data = stuComment;
                result.Msg = "成功！";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = "失败！";
            }


            return Json(result);


        }

        public ActionResult StuCommentList(string studentNumber)
        {
            //所有评价
            //获取学生信息
            EmployeesInfoManage dbemp = new EmployeesInfoManage();
            BaseBusiness<StudentInformation> dbstu = new BaseBusiness<StudentInformation>();
            //获取对他的评价
            BaseBusiness<ThearToStuComment> dbstucomment = new BaseBusiness<ThearToStuComment>();

            var list = dbstucomment.GetList().Where(d => d.CommnetObj == studentNumber).ToList();

            if (list == null) list = new List<ThearToStuComment>();

            List<ThearStuCommentView> resultlist = new List<ThearStuCommentView>();

            list.ForEach(d=>
            {
                ThearStuCommentView  view = new ThearStuCommentView();

                view.Commnet = d.Commnet;
                view.CommnetDate = d.CommnetDate;
                view.CommnetEr = dbemp.GetInfoByEmpID(d.CommnetEr);
                view.CommnetObj = dbstu.GetEntity(d.CommnetObj);

                resultlist.Add(view);
            });


            ViewBag.Commentlist = resultlist;

            return View();

        }
    }
}