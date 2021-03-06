﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.CourseSyllabus.Controllers
{
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;

    using SiliconValley.InformationSystem.Business.Common;

    using SiliconValley.InformationSystem.Business.CourseSchedulingSysBusiness;
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.ClassSchedule_Business;

    [CheckLogin]
    public class CourseController : Controller
    {
        private readonly CourseBusiness db_course;
     
        private readonly SpecialtyBusiness db_major;
        private readonly GrandBusiness db_grand;
        private readonly CourseTypeBusiness db_coursetype;
        private readonly BaseBusiness<ClassSchedule> db_class;
        private readonly EmployeesInfoManage db_emp;


        public CourseController()
        {
            this.db_course = new CourseBusiness();
            this.db_major = new SpecialtyBusiness();
            this.db_grand = new GrandBusiness();
            this.db_coursetype = new CourseTypeBusiness();
            db_class = new BaseBusiness<ClassSchedule>();
            db_emp = new EmployeesInfoManage();
        }



        // GET: /CourseSyllabus/Course/AddorEditfunction
        public ActionResult CourseIndex()
        {
            ViewBag.grands = db_grand.AllGrand();
            ViewBag.majors = db_major.GetSpecialties();

            return View();
        }



        /// <summary>
        /// 获取所有的课程数据
        /// </summary>
        /// <returns>课程json数据</returns>
        public ActionResult GetCourseData(int limit, int page)
        {
            var list = db_course.Curriculas();

            var skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

            List<CourseView> resultlist = new List<CourseView>();

            foreach (var item in skiplist)
            {
                resultlist.Add(db_course.ToCourseView(item));
            }

            var objresult = new
            {
                code = 0,
                msg = "",
                count = list.Count,
                data = resultlist
            };


            return Json(objresult, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SerachByMajor(int page, int limit, string major = null)
        {
          

            if (string.IsNullOrEmpty(major))
            {      
                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = new List<CourseView>(),
                    count = 0
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            try
            {        
                int majorid = int.Parse(major);

                var list = new List<Curriculum>();

                if (majorid == 0)
                {
                    list = db_course.Curriculas();
                }
                else
                {
                    list = db_course.GetCurriculaByMajor(majorid);
                }


                var skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

                List<CourseView> resultlist = new List<CourseView>();

                foreach (var item in skiplist)
                {
                    resultlist.Add(db_course.ToCourseView(item));
                }


                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = resultlist,
                    count = list.Count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = new List<CourseView>(),
                    count = 0
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SerachByGrand(int page, int limit, string grand = null)
        {


            if (string.IsNullOrEmpty(grand))
            {
                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = new List<CourseView>(),
                    count = 0
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            try
            {
                int grandid = int.Parse(grand);

                var list = new List<Curriculum>();

                if (grandid == 0)
                {
                    list = db_course.Curriculas();
                }
                else
                {
                    list = db_course.GetCurriculas().Where(d => d.Grand_Id == grandid).ToList();
                }


                var skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

                List<CourseView> resultlist = new List<CourseView>();

                foreach (var item in skiplist)
                {
                    resultlist.Add(db_course.ToCourseView(item));
                }


                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = resultlist,
                    count = list.Count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = new List<CourseView>(),
                    count = 0
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SerachByName(int page, int limit, string name = null)
        {
            if (name == null)
            {
                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = new List<CourseView>(),
                    count = 0
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            try
            {
              
                var list = db_course.Curriculas().Where(d=>d.CourseName.Contains(name)).ToList();

                var skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

                List<CourseView> resultlist = new List<CourseView>();

                foreach (var item in skiplist)
                {
                    resultlist.Add(db_course.ToCourseView(item));
                }


                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = resultlist,
                    count = list.Count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                var obj = new
                {
                    code = 0,
                    msg = "",
                    data = new List<CourseView>(),
                    count = 0
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 课程操作视图
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult OperationView(int? id)
        {
            Curriculum curriculum = new Curriculum();            

            //获取专业集合
            ViewBag.majors = db_major.GetSpecialties().Select(d => new SelectListItem() { Text = d.SpecialtyName, Value = d.Id.ToString() });
            
            //获取阶段集合
            ViewBag.grands = db_grand.GetList().Where(d => d.IsDelete == false).ToList().Select(d => new SelectListItem() { Text = d.GrandName, Value = d.Id.ToString() });

            //获取课程类型集合
            ViewBag.courseTypes = db_coursetype.GetCourseTypes().Select(d => new SelectListItem() { Text = d.TypeName, Value = d.Id.ToString() });

            if (id != null)
            {
                curriculum = db_course.GetCurriculas().Where(d => d.CurriculumID == id).FirstOrDefault();                                                 
            }

            return View(curriculum);

        }


        /// <summary>
        /// 课程的操作（添加 修改）
        /// </summary>
        /// <param name="courseView"></param>
        /// <param name="Grand"></param>
        /// <param name="Major"></param>
        /// <param name="CourseType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoOperation(Curriculum courseView, int Grand_Id, int MajorID, int CourseType_Id)
        {

            AjaxResult result = new AjaxResult();

            courseView.IsDelete = false;
            if (MajorID == 0)
            {
                courseView.MajorID = null;
            }
            else
            {
                courseView.MajorID = db_major.GetSpecialtyByID(MajorID).Id;
            }

            
            courseView.Grand_Id = db_grand.GetList().Where(d => d.Id == Grand_Id && d.IsDelete == false).FirstOrDefault().Id;
            courseView.CourseType_Id = db_coursetype.GetList().Where(d => d.IsDelete == false && d.Id == CourseType_Id).FirstOrDefault().Id;

            
            if (courseView.CurriculumID == 0)
            {

                try
                {
                    this.DoAdd(courseView);
                    result.ErrorCode = 200;
                    result.Msg = "成功";


                    BusHelper.WriteSysLog("新增课程", Entity.Base_SysManage.EnumType.LogType.添加数据);

                }
                catch (Exception ex)
                {
                    result.Data = null;
                    result.ErrorCode = 500;
                    result.Msg = "失败";

                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
            }
            else
            {
                try
                {
                    this.DoEdit(courseView);
                    result.ErrorCode = 200;
                    result.Msg = "成功";
                    BusHelper.WriteSysLog("修改课程", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
                catch (Exception ex)
                {
                    result.Data = null;
                    result.ErrorCode = 500;
                    result.Msg = "失败";
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void DoAdd(Curriculum curriculum)
        {
            db_course.DoAdd(curriculum);

        }


        public void DoEdit(Curriculum curriculum)
        {
            db_course.DoEdit(curriculum);

        }


        /// <summary>
        /// 课程详细页面 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailView(int id)
        {

            //获取专业集合
            ViewBag.majors = db_major.GetSpecialties().Select(d => new SelectListItem() { Text = d.SpecialtyName, Value = d.Id.ToString() });

            //获取阶段集合
            ViewBag.grands = db_grand.GetList().Where(d => d.IsDelete == false).ToList().Select(d => new SelectListItem() { Text = d.GrandName, Value = d.Id.ToString() });

            //获取课程类型集合
            ViewBag.courseTypes = db_coursetype.GetCourseTypes().Select(d => new SelectListItem() { Text = d.TypeName, Value = d.Id.ToString() });


            var course = db_course.GetCurriculas().Where(d => d.CurriculumID == id).FirstOrDefault();

            var courseView = db_course.ToCourseView(course);

            return View(courseView);
        }

        /// <summary>
        /// 课程类型视图
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseTypeIndex()
        {
            return View();
        }

        public ActionResult GetCourseTypedata(int limit, int page)
        {
            List<CourseType> CourseType_list = db_coursetype.GetList();//获取所有类型数据
            var datajson = CourseType_list.OrderByDescending(c => c.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var data = new {
                msg = "",
                code = 0,
                count = CourseType_list.Count,
                data = datajson
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddorEditfunction()
        {
            string key = Request.Form["myfield"];
            string value = Request.Form["value"];
            string id = Request.Form["ID"];           
            string isdel = Request.Form["Delete"];
            int count_my = db_coursetype.GetList().Where(c => string.IsNullOrEmpty(c.TypeName) == true).ToList().Count;
            try
            {
                if (count_my <= 0)
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        //添加
                        CourseType new_c = new CourseType();
                        new_c.TypeName = "";
                        new_c.IsDelete = false;
                        db_coursetype.Insert(new_c);

                     }
                    else
                    {
                        //编辑
                        CourseType find_c = db_coursetype.FindSingeData(id, true);
                        if (find_c != null)
                        {
                            switch (key)
                            {
                                case "TypeName":
                                    find_c.TypeName = value;
                                    break;
                                case "Rmark":
                                    find_c.Rmark = value;
                                    break;
                            }
                            if (!string.IsNullOrEmpty(isdel))
                            {
                                find_c.IsDelete = Convert.ToBoolean(isdel)==false?true:false;
                            }                             
                            db_coursetype.Update(find_c);
                        }
                       
                    }
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("类型名称未填写,不能下一步操作！！！", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }            
        }


        /// <summary>
        /// 班级课程安排
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassCourseArrangement()
        {
            var allclasslist = new List<ClassSchedule>();
            TeacherClassBusiness dbteacherclass = new TeacherClassBusiness();

            allclasslist = dbteacherclass.AllClassSchedule();

            ViewBag.classlist = allclasslist;

            ViewBag.grandlist = db_grand.AllGrand();


            return View();
        }

        public ActionResult ClassCourseArrangementData(int page, int limit)
        {
            ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
        
            TeacherClassBusiness dbteacherclass = new TeacherClassBusiness();

            List<ClassCourseView> resultlist = new List<ClassCourseView>();

            var allclasslist = dbclass.ClassList();

            BaseBusiness<ClassTeacher> dbclassTeacher = new BaseBusiness<ClassTeacher>();

            var list = dbclassTeacher.GetList();
            var skiplist = list.Skip((page - 1) * 8).Take(limit).ToList();

            list.ForEach(d=>
            {
                var tempobj =db_course.ConvertToView(d);

                if (tempobj != null) resultlist.Add(tempobj);
            });

            var objresult = new
            {

                code = 0,
                msg = "成功",
                count = list.Count,
                data = resultlist.OrderByDescending(d => d.BeginDate).ToList()

            };
            return Json(objresult, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SearchClassCourseArrangementData(int page, int limit, string classid, bool status,string grandId)
        {
            ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
            TeacherClassBusiness dbteacherclass = new TeacherClassBusiness();

            List<ClassCourseView> resultlist = new List<ClassCourseView>();

            List<ClassSchedule> classlist = new List<ClassSchedule>();

            if (string.IsNullOrEmpty(classid) && !string.IsNullOrEmpty(grandId))
            {
                var templist = dbclass.ClassList().Where(d => d.grade_Id == int.Parse(grandId)).ToList();

                if (templist != null) classlist.AddRange(templist);
            }

            if (!string.IsNullOrEmpty(classid) && !string.IsNullOrEmpty(grandId))
            {
                var templist = dbclass.ClassList().Where(d => d.grade_Id == int.Parse(grandId) && d.id==int.Parse(classid)).ToList();
                if (templist != null) classlist.AddRange(templist);
            }

            if (!string.IsNullOrEmpty(classid) && string.IsNullOrEmpty(grandId))
            {
                var templist = dbclass.ClassList().Where(d => d.id == int.Parse(classid)).ToList();
                if (templist != null) classlist.AddRange(templist);
            }

            if (string.IsNullOrEmpty(classid) && string.IsNullOrEmpty(grandId))
            {
                var templist = dbclass.ClassList();
                if (templist != null) classlist.AddRange(templist);
            }

         
            List<ClassTeacher> classteacherlist = new List<ClassTeacher>();

            classlist.ForEach(d=>
            {
                var templist = dbteacherclass.GetIQueryable().Where(c => c.ClassNumber == d.id && c.IsDel == status).ToList();
                if (templist != null) classteacherlist.AddRange(templist);
            });
            var skiplist = classteacherlist.Skip((page - 1) * 8).Take(8).ToList();

            foreach (var item in skiplist)
            {
                var teacherclass = db_course.ConvertToView(item);

                if (teacherclass != null)
                {
                    resultlist.Add(teacherclass);
                }
            }


            var objresult = new
            {

                code = 0,
                msg = "成功",
                count = resultlist.Count,
                data = resultlist.OrderByDescending(d => d.BeginDate).ToList()

            };

            return Json(objresult, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 编辑班级课程安排
        /// </summary>
        /// <param name="classid">班级id 否则传入0</param>
        /// <returns></returns>
        public ActionResult EditClassCourseArrangment()
        {


            ViewBag.grandlist = db_grand.AllGrand();


            //提供所有课程
            //ViewBag.courselist = db_course.GetCurriculas();

            return View();
        }

        /// <summary>
        /// 获取班级下一门课程
        /// </summary>
        /// <returns></returns>
        public ActionResult GetClassNextCourse(int classid)
        {
            AjaxResult returnresult = new AjaxResult();

            try
            {
                List<Curriculum> result = new List<Curriculum>();
                TeacherClassBusiness dbteacherclass = new TeacherClassBusiness();

                var classschedule = dbteacherclass.AllClassSchedule().Where(d => d.id == classid).FirstOrDefault();

                //判断有没有专业
                if (classschedule.Major_Id == null)
                {
                    //获取阶段所有课程

                    result = db_course.GetCurriculas().Where(d => d.Grand_Id == classschedule.grade_Id).ToList();
                    
                }
                else
                {
                    //获取阶段专业课程 和公共课

                    var courselist = db_course.GetCurriculas().Where(d => d.Grand_Id == classschedule.grade_Id && d.MajorID == classschedule.Major_Id).ToList(); //专业课
                    var courselist1 = db_course.GetCurriculas().Where(d => d.Grand_Id == classschedule.grade_Id && d.MajorID == null).ToList();//公公课


                    if (courselist != null)
                    {
                        result.AddRange(courselist);

                    }

                    if (courselist1 != null)
                    {


                        result.AddRange(courselist1);
                    }
                }

                returnresult.ErrorCode = 200;
                returnresult.Data = result;
            }
            catch (Exception ex)
            {

                returnresult.ErrorCode = 500;
                returnresult.Data = null;
            }


            return Json(returnresult, JsonRequestBehavior.AllowGet);
            
            
        }

        /// <summary>
        /// 获取擅长课程的教员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public ActionResult goodSkillTeacher(int courseid)
        {

            AjaxResult result = new AjaxResult();

            try
            {
               
                TeacherBusiness dbteacher = new TeacherBusiness();

                 var list = dbteacher.GetTeachers(isContains_Jiaowu: false, IsNeedDimission:false);
                var emplist = db_emp.GetAll();
                //List<EmployeesInfo> emplist = new List<EmployeesInfo>();
                //foreach (var item in list)
                //{
                //   var temp = db_emp.GetInfoByEmpID(item.EmployeeId);

                //    if (temp != null)
                //    {
                //        emplist.Add(temp);
                //    }

                //}

                BaseBusiness<GoodSkill> dbgoodskell = new BaseBusiness<GoodSkill>();


                List<EmployeesInfo> resultlist = new List<EmployeesInfo>();
                //foreach (var item in emplist)
                //{
                //    var teacher = dbteacher.GetTeachers().Where(d => d.EmployeeId == item.EmployeeId).FirstOrDefault();

                //    if (teacher != null)
                //    {
                //        var templist = dbgoodskell.GetList().Where(d => d.TearchID == teacher.TeacherID && d.Curriculum == courseid).FirstOrDefault();

                //        if (templist != null)
                //        {

                //            resultlist.Add(item);
                //        }
                //    }
                //}
                var templist = dbgoodskell.GetList().Where(d=>d.Curriculum == courseid).ToList();

                foreach (var item in templist)
                {
                   var teacher = list.Where(d => d.TeacherID == item.TearchID).FirstOrDefault();

                   var emp = emplist.Where(d => d.EmployeeId == teacher.EmployeeId).FirstOrDefault();

                    resultlist.Add(emp);
                }



                result.Msg = "成功";
                result.Data = resultlist;
                result.ErrorCode = 200;
            }
            catch (Exception ex)
            {
                result.Msg = "失败";
                result.Data = null;
                result.ErrorCode = 500;
            }

           


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditClassCourseArrangment(ClassTeacher classTeacher, string empnumber)
        {
            AjaxResult result = new AjaxResult();
            try
            {
                TeacherBusiness dbteacher = new TeacherBusiness();
                var teacher = dbteacher.GetTeachers().Where(d => d.EmployeeId == empnumber).FirstOrDefault();

                classTeacher.TeacherID = teacher.TeacherID;
                db_course.EditClassCourseArrangment(classTeacher);

                result.Data = null;
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
        /// 启用/禁用
        /// </summary>
        /// <param name="status">状态 true：启用  false：禁用</param>
        /// <param name="classteacherid"></param>
        /// <returns></returns>
        public ActionResult UsingOrProhibit( int classteacherid, string date)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_course.UsingOrProhibit(classteacherid, DateTime.Parse(date));

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "失败";
                result.Data = null;
            }

            return Json(result, JsonRequestBehavior.AllowGet);



        }

        public ActionResult UsingOrProhibitCourse(string courseIds)
        {
            var temp1 = courseIds.Split(',');

            var idlist = temp1.ToList();

            idlist.RemoveAt(temp1.Length - 1);

            AjaxResult result = new AjaxResult();

            try
            {
                foreach (var item in idlist)
                {
                    var tempobj = db_course.GetEntity(int.Parse(item));
                    var isdel = tempobj.IsDelete;
                    tempobj.IsDelete = !isdel;
                    db_course.Update(tempobj);
                }

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

            return Json(result);
           
        }

        public ActionResult ClassDataByGrand(string grandid)
        {
            ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
            var allclasslist = dbclass.ClassList().Where(d => d.grade_Id == int.Parse(grandid)).ToList();

            return Json(allclasslist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClassDataByGrandAjax(string grandid)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
               
                var allclasslist = dbclass.ClassList().Where(d => d.grade_Id == int.Parse(grandid)).ToList();
                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = allclasslist;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "失败";
                result.Data = null;
            }

         

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}