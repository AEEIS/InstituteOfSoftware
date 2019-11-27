﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Entity.Entity;
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;

    [CheckLogin]
    public class TeacherController : Controller
    {
        // GET: Teaching/Teacher


        // 教员上下文
        private readonly TeacherBusiness db_teacher;

        private readonly EmployeesInfoManage db_emp;

        private readonly TecharOnstageBearingBusiness db_teacher_sBearing;

        private readonly SpecialtyBusiness db_specialty;

        private readonly GrandBusiness db_grand;

        

        
        
        public TeacherController()
        {
            db_teacher = new TeacherBusiness();
            db_emp = new EmployeesInfoManage();
            db_teacher_sBearing = new TecharOnstageBearingBusiness();
            db_specialty = new SpecialtyBusiness();
            db_grand = new GrandBusiness();
        }
        public ActionResult TeachersInfo()
        {

           var majorlist = db_specialty.GetList();

            ViewBag.majors = majorlist;

            var grandlist = db_grand.GetList();
            ViewBag.grands = grandlist;
            //提供登录人信息

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var emp = db_teacher.GetEmpByEmpNo(user.EmpNumber);

            ViewBag.empDetail = db_teacher.ConvertToEmpDetailView(emp);
            return View();
        }


        /// <summary>
        /// 教员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Teacherlist()
        {

            var majorlist = db_specialty.GetList();

            ViewBag.majors = majorlist;

            var grandlist = db_grand.GetList();
            ViewBag.grands = grandlist;

           
            return View();

        }

        public ActionResult Teachers()
        {

            AjaxResult result = new AjaxResult();

            try
            {
               var list = db_teacher.GetTeachers();

                result.Data = list;
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

        public ActionResult GetEmpData(int limit, int page)
        {

            var list = db_teacher.employeesInfos().Where(d=>d.IsDel==false).ToList();

            var returnlist = list.Skip((page - 1) * limit).Take(limit).ToList().Select(x => new { EmpName = x.EmpName, Number = x.EmployeeId, Sex = x.Sex });

            var objresult = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = returnlist
            };

            return Json(objresult,JsonRequestBehavior.AllowGet);
        }
 
        public ActionResult TeacherData(int limit,int page,int  major, int  grand)
       {

            var list = new List<Teacher>();

            if (major == 0 && grand == 0)
            {

              
                list = db_teacher.GetTeachers().Where(d => d.IsDel == false).ToList().Skip((page - 1) * limit).Take(limit).ToList().OrderBy(d => d.TeacherID).ToList();
            }
            else
            {
                list = db_teacher.getTeacherByMajorAndGrand(major, grand).Skip((page - 1) * limit).Take(limit).ToList().OrderBy(d => d.TeacherID).ToList();
            }

          
           

            var returnlist = new List<object>();


            //组装返回对象
            foreach (var item in list)
            {
                var emp = db_emp.GetList().Where(t => t.EmployeeId == item.EmployeeId &&t.IsDel==false).FirstOrDefault();

                //获取专业信息
                //var major = db_teacher_sBearing.GetList().Where(t => t.TeacherID == item.TeacherID).FirstOrDefault();

                //var majorresult = db_specialty.GetList().Where(t => t.Id == major.Major).FirstOrDefault(); ;


                var obj = new {

                    EmployeeId = emp.EmployeeId,
                    Sex = emp.Sex,
                    TeacherID = item.TeacherID,
                    AttendClassStyle = item.AttendClassStyle,
                    ProjectExperience = item.ProjectExperience,
                    TeachingExperience = item.TeachingExperience,
                    WorkExperience = item.WorkExperience,
                    TeacherName = emp.EmpName,
                   

                };

                returnlist.Add(obj);


            }

            //var returnlist = list.Select(x => new { TeacherID = x.TeacherID, TeacherName = x.TeachingExperience, WorkExperience = x.WorkExperience });

            var objresult = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = returnlist
            };

            return Json(objresult, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 添加教员视图
        /// </summary>
        /// <returns>返回视图</returns>
        [HttpGet]

        public ActionResult AddTeacher(string empid)
        {
            var emp = db_emp.GetList().Where(d=>d.IsDel==false && d.EmployeeId==empid && d.IsDel==false).ToList().FirstOrDefault();

            ViewBag.Emp = emp;

            return View();

        }

        /// <summary>
        /// 获取阶段教员人数 和 阶段下各个专业的人数
        /// </summary>
        /// <returns></returns>
        public ActionResult MajorTeacherNumber()
        {
            AjaxResult result = new AjaxResult();

            try
            {

                List<object> returnlist = new List<object>();
                //获取所有教员
                var teacherlist = db_teacher.GetTeachers();

                //获取阶段

                var grands = db_grand.AllGrand();

                //所有专业
                var majors = db_specialty.GetSpecialties();

                foreach (var item in grands)
                {
                    //获取阶段总人数 :

                    var list = db_teacher.BrushSelectionByGrand(item.Id);


                    List<object> templist = new List<object>();

                    foreach (var item1 in majors)
                    {
                        var templist1 = db_teacher.getTeacherByMajorAndGrand(item1.Id, item.Id);

                        var tempobj = new
                        {

                            major = item1,
                            teachers = templist1


                        };

                        templist.Add(tempobj);

                    }

                    var obj = new
                    {
                        grand = item,
                        Tcount = list,
                        majors= templist

                    };

                    returnlist.Add(obj);


                }


                result.Data = returnlist;
                result.Msg = "";
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
        /// 对教员的操作的视图
        /// </summary>
        /// <param name="ID">教员Id</param>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult Operating(int? ID)
        {

            if (ID == null)
            {
                return View();

            }



            Teacher teacher = db_teacher.GetTeachers().Where(t => t.TeacherID == ID &&t.IsDel==false).ToList().FirstOrDefault();

            var emp = db_emp.GetList().Where(t=>t.EmployeeId==teacher.EmployeeId && t.IsDel==false).FirstOrDefault();


            //获取专业信息


            //var major = db_teacher_sBearing.GetList().Where(t => t.TeacherID == ID).FirstOrDefault();

            var majorresult = db_specialty.GetList().Select(d=>new SelectListItem() { Value=d.Id.ToString(),Text=d.SpecialtyName});

            ViewBag.Major = majorresult;

            ViewBag.Emp = emp;

            return View(teacher);

        }

        [HttpPost]

        public ActionResult GetTeacherByID(int Id)
        {
           Teacher teacher = db_teacher.GetTeachers().Where(t => t.TeacherID == Id && t.IsDel==false).ToList().FirstOrDefault();

            return Json(teacher, JsonRequestBehavior.AllowGet);


        }
        /// <summary>
        /// 修改教员信息
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns>结果类</returns>

        [HttpPost]
        public ActionResult DoEdit(Teacher teacher)
        {
            AjaxResult result = null;

           var t = db_teacher.GetTeachers().Where(x => x.TeacherID == teacher.TeacherID && x.IsDel==false).FirstOrDefault();

            t.AttendClassStyle = teacher.AttendClassStyle.Trim();
            t.ProjectExperience = teacher.ProjectExperience.Trim();
            t.TeachingExperience = teacher.TeachingExperience.Trim();
            t.WorkExperience = teacher.WorkExperience.Trim();

            try
            {
                db_teacher.Update(t);

                //更新缓存
                RedisCache redisCache = new RedisCache();
                redisCache.RemoveCache("TeacherList");

                result = new SuccessResult();
                result.Msg = "修改成功";
                result.Success = true;
            }
            catch (Exception)
            {
                result = new ErrorResult();
                result.ErrorCode = 500;
                result.Msg = "服务器错误1";

               
               
            }


            return Json(result,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 教员详细
        /// </summary>
        /// <param name="id">教员ID</param>
        /// <returns>教员详细视图</returns>
        /// 
        [HttpGet]
        public ActionResult TeacherDetailView(int id)
        {

           TeacherDetailView teacherResult = db_teacher.GetTeacherView(id);

            return View(teacherResult);

        }

        /// <summary>
        /// 编辑教员专业 阶段视图
        /// </summary>
        /// <param name="id">教员Id</param>
        /// <returns>视图</returns>
        /// 
        [HttpGet]
        public ActionResult EditMajorAndGrandView(int id)
        {
            //获取教员专业阶段

            //提供专业

            ViewBag.majors = db_teacher.GetMajorByTeacherID(id);


            ViewBag.Teacher = db_teacher.GetTeacherByID(id);
           

            return View();

        }


        /// <summary>
        /// 给教员分配阶段
        /// </summary>
        /// <param name="ids">阶段</param>
        /// <param name="majorid">专业id</param>
        /// <param name="teacherid">教员id</param>
        /// <returns></returns>
        public ActionResult SetGrandToTeacherOnMajor(string ids, int majorid, int teacherid)
        {
            var arry = ids.Split(',');

            var arry1 = arry.ToList();
            arry1.RemoveAt(arry.Length - 1);
            AjaxResult result = new AjaxResult();

            try
            {
                db_teacher.SetGrandToTeacherOnMajor(arry1.ToArray(), majorid, teacherid);


                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = "失败";
            }


            return Json(result,JsonRequestBehavior.AllowGet);  
        }

        /// <summary>
        /// 判断key是否在另一个字典包含
        /// </summary>
        /// <returns></returns>
        public bool ContainDic(Dictionary<Specialty, List<Grand>> source, Specialty key)
        {

            foreach (var item in source.Keys)
            {
                if(item==key)
                {

                    return true;
                }
            }

            return false;

        }

        [HttpPost]
        public ActionResult GetNoGrandOnMajor(int teacherId, int majorId)
        {

            AjaxResult result = new AjaxResult();

            if (teacherId != 0 && majorId != 0)
            {
                try
                {
                    var grandlist = db_teacher.GetNoGrand(teacherId, majorId);


                    result.ErrorCode = 200;
                    result.Data = grandlist;
                    result.Msg = "成功！";

                }
                catch (Exception ex)
                {

                    result.ErrorCode = 500;
                    result.Data = null;
                    result.Msg = "服务器错误！";
                }
                
            }
            else
            {
                result.ErrorCode = 401;
                result.Data = null;
                result.Msg = "此请求缺少参数！";
            }

            return Json(result,JsonRequestBehavior.AllowGet);

        }

     


        [HttpPost]
        public ActionResult AddGrandOnMajor(int teacherid, int majorid, int grandid)
        {

            AjaxResult result = new AjaxResult();


            TecharOnstageBearing onstageBearing = new TecharOnstageBearing();
            onstageBearing.Major = majorid;
            onstageBearing.Stage = grandid;
            onstageBearing.TeacherID = teacherid;
            onstageBearing.Remark = "测试数据";



            if (db_teacher_sBearing.GetList().Where(d => d.Major == majorid && d.Stage == grandid && d.TeacherID==teacherid).ToList().Count > 0)
            {
                result.ErrorCode = 501;
                result.Data = null;
                result.Msg = "数据错误";
            }
            else
            {
                try
                {
                    db_teacher_sBearing.Insert(onstageBearing);

                    result.ErrorCode = 200;
                    result.Data = null;
                    result.Msg = "成功";

                }
                catch (Exception)
                {

                    result.ErrorCode = 500;
                    result.Data = null;
                    result.Msg = "服务器错误";
                }

            }




            return Json(result, JsonRequestBehavior.AllowGet); ;


        }


        /// <summary>
        /// 获取教员专业所在那些阶段
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetHaveGrandData(int teacherid, int majorid)
        {

            AjaxResult result = new AjaxResult();
            var list = new List<Grand>();
            try
            {
                list = db_teacher.GetHaveGrand(teacherid, majorid);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = list;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "失败";
                result.Data = list;
            }


            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult GetTeacherByMajorOrGrand(int majorid, int grandid, int limit, int page)
        {

            List<Teacher> Teacherlist = new List<Teacher>();


            List<TeacherDetailView> resultlist = new List<TeacherDetailView>();

            if (majorid == 0 && grandid == 0)
            {

                Teacherlist = db_teacher.GetTeachers();
            }

            if (grandid == 0 && majorid != 0)
            {

                Teacherlist = db_teacher.BrushSelectionByMajor(majorid);
            }

            if (grandid != 0 && majorid == 0)
            {
                Teacherlist = db_teacher.BrushSelectionByGrand(grandid);
            }

            if (grandid != 0 && majorid != 0)
            {
                Teacherlist = db_teacher.getTeacherByMajorAndGrand(majorid, grandid);
            }

            //转为详细teacher模型

            foreach (var item in Teacherlist)
            {
                resultlist.Add(db_teacher.GetTeacherView(item.TeacherID));
            }


            var returnlist = new List<object>();

            foreach (var item in resultlist)
            {
                var obj1 = new
                {

                    EmployeeId = item.emp.EmployeeId,
                    Sex = item.emp.Sex,
                    TeacherID = item.TeacherID,
                    AttendClassStyle = item.AttendClassStyle,
                    ProjectExperience = item.ProjectExperience,
                    TeachingExperience = item.TeachingExperience,
                    WorkExperience = item.WorkExperience,
                    TeacherName = item.emp.EmpName,

                };

                returnlist.Add(obj1);


            }


            var obj = new {

                code = 0,
                msg = "",
                count = returnlist.Count(),
                data = returnlist.Skip((page - 1) * limit).Take(limit)

            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据电话姓名获取老师
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult BrushSelectionTeacher(string name, string phone, int limit, int page)
        {
            var resultlist = new List<TeacherDetailView> ();

            var list = db_teacher.GetTeachers();

            foreach (var item in list)
            {
                resultlist.Add( db_teacher.GetTeacherView(item.TeacherID));
            }
            var returnlist = new List<object>();

            foreach (var item in resultlist)
            {

                if (item.emp.EmpName.Contains(name) && item.emp.Phone.Contains(phone))
                {
                    var obj1 = new
                    {

                        EmployeeId = item.emp.EmployeeId,
                        Sex = item.emp.Sex,
                        TeacherID = item.TeacherID,
                        AttendClassStyle = item.AttendClassStyle,
                        ProjectExperience = item.ProjectExperience,
                        TeachingExperience = item.TeachingExperience,
                        WorkExperience = item.WorkExperience,
                        TeacherName = item.emp.EmpName,

                    };

                    returnlist.Add(obj1);
                }
            }


            var obj = new
            {

                code = 0,
                msg = "",
                count = returnlist.Count(),
                data = returnlist.Skip((page - 1) * limit).Take(limit)

            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 编辑教员擅长的技术
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult goodmajor(int id)
        {
            //提供专业

           ViewBag.majors = db_teacher.GetMajorByTeacherID(id);


            ViewBag.Teacher = db_teacher.GetTeacherByID(id);
            return View();

        }

        /// <summary>
        /// 获取教员擅长的技术
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
       
        public ActionResult GetCurcousData(int teacherid, int majorid)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                List<Curriculum> currlist = db_teacher.GetTeacherGoodCurriculum(teacherid, majorid);

                result.Data = currlist;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = "失败";
            }


            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 获取教员没有的技能
        /// </summary>
        /// <param name="majorid">专业id</param>
        /// <param name="teacherid">老师id</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetNewSkill(int majorid , int teacherid)
        {

            AjaxResult result = new AjaxResult();

            var temp = new List<Curriculum>();
            try
            {
                 temp = db_teacher.GetCurriculaOnTeacherNoHave(teacherid, majorid);

                result.Msg = "成功";
                result.Data = temp;
                result.ErrorCode = 200;
            }
            catch (Exception ex)
            {

                result.Msg = "失败";
                result.Data = temp;
                result.ErrorCode = 500;
            }
          

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 给教员添加擅长的技术
        /// </summary>
        /// <param name="ids">课程ID</param>
        /// <param name="teacherid">教员ID</param>
        /// <returns></returns>
        public ActionResult SetNewSkillToTeacher(string ids, int teacherid)
        {

           var arry = ids.Split(',');

            var arry1 = arry.ToList();
            arry1.RemoveAt(arry.Length - 1);


            AjaxResult result = new AjaxResult();

            try
            {
                db_teacher.SetNewSkillToTeacher(teacherid, arry1.ToArray());

                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = "失败";
            }
           
            return Json(result,JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetNoHaveMajorOnTeacher(int teacherid)
        {

            AjaxResult result = new AjaxResult();

            var list = new List<Specialty>();
            try
            {
               list = db_teacher.GetNoHaveMajorOnTeacher(teacherid);

                result.Data = list;
                result.ErrorCode = 200;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {

                result.Data = list;
                result.ErrorCode = 500;
                result.Msg = "失败";

            }



            return Json(result,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 给教员添加专业
        /// </summary>
        /// <returns></returns>
        public ActionResult SetMajorToTeacher(string ids, int teacherid)
        {

            var arry = ids.Split(',');

            var arry1 = arry.ToList();
            arry1.RemoveAt(arry.Length - 1);

            AjaxResult result = new AjaxResult();

            try
            {
                db_teacher.SetMajorToTeacher(arry1.ToArray(),teacherid);
                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = ex.Message;
            }


            return Json(result,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 移除教员在专业上对应的阶段
        /// </summary>
        /// <param name="grandid">阶段ID</param>
        /// <param name="teacherid">教员ID</param>
        /// <param name="majorid">专业ID</param>
        /// <returns></returns>
        public ActionResult RemoveGrandOnTeacherMajor(int grandid, int teacherid, int majorid)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_teacher.RemoveGrandOnTeacherMajor(grandid, teacherid, majorid);
                result.Data = null;
                result.ErrorCode = 200;
                result.Msg= "成功";
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
        /// 移除教员擅长的课程
        /// </summary>
        /// <param name="teacherid">教员ID</param>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult removeTeacherGoodSkill(int teacherid, int courseid)
        {

            AjaxResult result = new AjaxResult();

            try
            {
                db_teacher.removeTeacherGoodSkill(teacherid, courseid);

                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Application()
        {
            return View();
        }


        /// <summary>
        /// 调课课表单
        /// </summary>
        /// <returns></returns>
        public ActionResult AdjustmentCourse()
        {
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            //获取当前登录老师的班级
            TeacherClassBusiness db = new TeacherClassBusiness();
           var classScadu = db.GetCrrentMyClass(teacher.TeacherID); //班级
            ViewBag.classList = classScadu;


            //提供专业老师

           var teachers = db_teacher.GetTeachers();

            List<EmployeesInfo> emplist = new List<EmployeesInfo>();

            foreach (var item in teachers)
            {
               var empobj = db_teacher.GetEmpByEmpNo(item.EmployeeId);

                if (empobj != null)
                    emplist.Add(empobj);
            }

            ViewBag.Teachers = emplist;



            return View();
        }


        /// <summary>
        /// 获取班级的课程
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseData(string classnumber, string date, string specific)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                ReconcileManeger db_reconcileTemp = new ReconcileManeger();

                //获取当前登录用户
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                //获取班级对象

                TeacherClassBusiness db = new TeacherClassBusiness();
               var classObj = db.AllClassSchedule().Where(d => d.id == int.Parse(classnumber)).FirstOrDefault();
                var resconcileList = db_reconcileTemp.GetReconcile(DateTime.Parse(date), classObj.ClassNumber, specific);

                var resultList = resconcileList.Where(d => d.EmployeesInfo_Id == user.EmpNumber).ToList();

                List<ResconcileView> resconcileViewList = new List<ResconcileView>();

                foreach (var item in resultList)
                {
                    var tempObj = db_reconcileTemp.ConvertToView(item);

                    resconcileViewList.Add(tempObj);

                }


                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = resconcileViewList;

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "失败";
                result.Data = null;

            }

         

            return Json(result);
        }


        /// <summary>
        /// 调课操作 
        /// </summary>
        /// <param name="convertCourse"></param>
        /// <returns></returns>
        /// []
        /// 
        [HttpPost]
        public ActionResult AdjustmentCourse(ConvertCourse convertCourse)
        {

            AjaxResult result = new AjaxResult();

            try
            {

                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

                convertCourse.ApplyDate = DateTime.Now;
                convertCourse.IsDel = false;
                convertCourse.TeacherID = teacher.TeacherID;
                convertCourse.AttendClassTeacher = teacher.TeacherID;
                
                //添加记录
                db_teacher.AdjustmentCourse(convertCourse);
                 
                result.Msg = "成功";
                result.Data = "";
                result.ErrorCode = 200;

            }
            catch (Exception ex)
            {
                result.Msg = "失败";
                result.Data = "";
                result.ErrorCode = 500;

            }
            //1

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 代课表单视图
        /// </summary>
        public ActionResult SubstituteTeachCourse()
        {

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            //获取当前登录老师的班级
            TeacherClassBusiness db = new TeacherClassBusiness();
            var classScadu = db.GetCrrentMyClass(teacher.TeacherID); //班级
            ViewBag.classList = classScadu;

            //提供专业老师

            var teachers = db_teacher.GetTeachers();

            List<EmployeesInfo> emplist = new List<EmployeesInfo>();

            foreach (var item in teachers)
            {
                var empobj = db_teacher.GetEmpByEmpNo(item.EmployeeId);

                if (empobj != null)
                    emplist.Add(empobj);
            }

            ViewBag.Teachers = emplist;
            return View();
             
        }


        /// <summary>
        /// 代课表单提交
        /// </summary>
        /// <param name="substituteTeachCourse"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubstituteTeachCourse(SubstituteTeachCourse substituteTeachCourse)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();
                substituteTeachCourse.Applier = teacher.TeacherID; //申请人为当前登录人
                substituteTeachCourse.ApplyDate = DateTime.Now;
                substituteTeachCourse.IsDel = false;

                db_teacher.SubstituteTeachCourse(substituteTeachCourse);
                result.Msg = "成功";
                result.Data = "";
                result.ErrorCode = 200;
            }
            catch (Exception ex)
            {

                result.Msg = "失败";
                result.Data = "";
                result.ErrorCode = 500;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 加课表单视图
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCourse()
        {
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            //获取当前登录老师的班级
            TeacherClassBusiness db = new TeacherClassBusiness();
            var classScadu = db.GetCrrentMyClass(teacher.TeacherID); //班级
            ViewBag.classList = classScadu;

            ///提供课程数据
            CourseBusiness db_course = new CourseBusiness();

            var courselist = db_course.GetCurriculas().ToList();

            ViewBag.CourseList = courselist;
            return View();

        }



        /// <summary>
        /// 提交加课表单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCourse(AddCourse addCourse)
        {
            AjaxResult result = new AjaxResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 员工详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EmpDetailData(string empId)
        {
            AjaxResult result = new AjaxResult();

            try
            {
               var emp = db_teacher.GetEmpByEmpNo(empId);

               var empview = db_teacher.ConvertToEmpDetailView(emp);

                result.Data = empview;
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

    }
}