﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using Base_SysManage;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using EmployeesBusiness;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using System.Linq.Dynamic;
    using SiliconValley.InformationSystem.Business.CourseSchedulingSysBusiness;
    using SiliconValley.InformationSystem.Util;

    public class TeacherBusiness : BaseBusiness<Teacher>
    {

        //员工 业务上下文
        private readonly EmployeesInfoManage db_emp;



        //阶段 业务上下文

        private readonly GrandBusiness db_grand;

        //专业上下文

        private readonly SpecialtyBusiness db_specialty;
        public TeacherBusiness()
        {
            db_grand = new GrandBusiness();
            db_emp = new EmployeesInfoManage();
            db_specialty = new SpecialtyBusiness();

        }


        public bool EmpIsDel(Teacher teacher)
        {
            return (bool)db_emp.GetList().Where(d => d.EmployeeId == teacher.EmployeeId).FirstOrDefault().IsDel;
        }



        /// <summary>
        /// 根据ID获取教员
        /// </summary>
        /// <param name="">教员ID</param>
        /// <returns>教员实体</returns>
        public Teacher GetTeacherByID(int? id)
        {
            Teacher teacher = this.GetList().Where(t => t.TeacherID == id).FirstOrDefault();


            if (EmpIsDel(teacher))
            {
                return null;
            }

            return teacher;
        }


        /// <summary>
        /// 获取所有教员
        /// </summary>
        /// <returns></returns>
        public List<Teacher> GetTeachers()
        {

            List<Teacher> resultlist = new List<Teacher>();

            //从缓存中获取
            RedisCache redisCache = new RedisCache();

            resultlist = redisCache.GetCache<List<Teacher>>("TeacherList");

            if (resultlist == null || resultlist.Count == 0)
            {

                resultlist = new List<Teacher>();


                var temp = this.GetList().Where(d => d.IsDel == false).ToList();

                foreach (var item in temp)
                {
                    if (!EmpIsDel(item))
                    {
                        resultlist.Add(item);
                    }
                }


                redisCache.SetCache("TeacherList", resultlist);
            }

            return resultlist.OrderByDescending(d=>d.TeacherID) .ToList();

        }

        /// <summary>
        /// / 更具员工编号获取员工
        /// </summary>
        /// <param name="EmpNo">员工编号</param>
        /// <returns>员工实体</returns>
        public EmployeesInfo GetEmpByEmpNo(string EmpNo)
        {


            return db_emp.GetList().Where(d => d.EmployeeId == EmpNo && d.IsDel == false).FirstOrDefault();
        }


        /// <summary>
        /// 根据教员ID 获取教员阶段
        /// </summary>
        /// <param name="id">j教员ID</param>
        public List<Grand> GetGrandByTeacherID(int id)
        {
            List<Grand> resultList = new List<Grand>();

            BaseBusiness<TecharOnstageBearing> business = new BaseBusiness<TecharOnstageBearing>();

            var bus = business.GetList().Where(t => t.TeacherID == id).ToList();

            foreach (var item in bus)
            {
                var tempobj = db_grand.GetList().Where(t => t.Id == item.Stage && t.IsDelete == false).FirstOrDefault();

                    if (tempobj != null)
                    {
                        if (!Grand.IsInList(resultList, tempobj))
                        {
                            resultList.Add(tempobj);
                        }
                        

                       
                    }
            }

            return resultList;
        }

        /// <summary>
        /// 根据教员ID 获取教员的技术专业
        /// </summary>
        /// <param name="ID">教员ID</param>
        /// <returns>返回专业集合</returns>
        public List<Specialty> GetMajorByTeacherID(int ID)
        {
            List<Specialty> returnList = new List<Specialty>();


            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var majors = baseBusiness.GetList().Where(t => t.TeacherID == ID).ToList();

            foreach (var item in majors)
            {
                var dd = db_specialty.GetList().Where(t => t.Id == item.Major && t.IsDelete == false).FirstOrDefault();

                if (!db_specialty.IsInList(returnList, dd))
                {
                    returnList.Add(dd);
                }
            }
            return returnList;

        }

        public bool ContainDic(Dictionary<Specialty, List<Grand>> source, Specialty key)
        {

            foreach (var item in source.Keys)
            {
                if (item.Id == key.Id)
                {

                    return true;
                }
            }

            return false;

        }

        public TeacherDetailView GetTeacherView(int id)
        {

            BaseBusiness<Position> curent_posi = new BaseBusiness<Position>();

            BaseBusiness<Department> curent_Dep = new BaseBusiness<Department>();

            TeacherDetailView teacherResult = new TeacherDetailView();

            //获取教员信息
            Teacher t = this.GetTeacherByID(id);

            //获取教员基本信息
            EmployeesInfo emp = this.GetEmpByEmpNo(t.EmployeeId);
            teacherResult.emp = emp;

            var post = curent_posi.GetList().Where(d => d.Pid == emp.PositionId && d.IsDel == false).FirstOrDefault();


            teacherResult.TeacherID = t.TeacherID;

            teacherResult.Position = post;

            var dep = curent_Dep.GetList().Where(d => d.DeptId == post.DeptId && d.IsDel == false).FirstOrDefault();


            teacherResult.Department = dep;


            //获取教员阶段信息
            teacherResult.Grands = this.GetGrandByTeacherID(t.TeacherID);

            //获取教员专业信息
            teacherResult.Major = this.GetMajorByTeacherID(t.TeacherID);

            //获取技术信息
            teacherResult.AttendClassStyle = t.AttendClassStyle;
            teacherResult.ProjectExperience = t.ProjectExperience;
            teacherResult.TeachingExperience = t.TeachingExperience;
            teacherResult.WorkExperience = t.WorkExperience;

            return teacherResult;

        }



        /// <summary>
        /// 获取教员的专业没有阶段
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="marjorId"></param>
        /// <returns></returns>
        public List<Grand> GetNoGrand(int teacherId, int majorId)
        {


            List<Grand> resultList = new List<Grand>();

            Specialty Major = db_specialty.GetSpecialtyByID(majorId);
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var haveGrands = baseBusiness.GetList().Where(t => t.TeacherID == teacherId && t.Major == majorId).ToList().OrderBy(t => t.Stage).ToList();

            //获取全部阶段 
            List<Grand> grands = db_grand.GetList().Where(d => d.IsDelete == false).OrderBy(t => t.Id).ToList();

            foreach (var item in grands)//1 2 3
            {
                if (!contains(item.Id, haveGrands))
                {
                    resultList.Add(item);

                }
            }

            //var ss = resultList.Distinct() as List<Grand>;

            return resultList;
        }

        public bool contains(int id, List<TecharOnstageBearing> grands)
        {
            return grands.Where(e => e.Stage == id).ToList().Count > 0;

        }

        public List<Grand> GetHaveGrand(int teacherid, int majorid)
        {

            List<Grand> grands = new List<Grand>();

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var lsit = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == majorid).ToList();

            var grandlist = db_grand.GetList().Where(d => d.IsDelete == false);

            foreach (var item in grandlist)
            {
                foreach (var item1 in lsit)
                {
                    if (item.Id == item1.Stage)
                    {
                        grands.Add(item);
                    }
                }
            }

            return grands;

        }


        public List<EmployeesInfo> employeesInfos()
        {

            BaseBusiness<Department> DepbaseBusiness = new BaseBusiness<Department>();

            var deplist = DepbaseBusiness.GetList().Where(d => d.IsDel == false).ToList();

            var emps = db_emp.GetList().Where(d => d.IsDel == false).OrderBy(d => d.PositionId).ToList();

            BaseBusiness<Position> PobaseBusiness = new BaseBusiness<Position>();

            var ind = PobaseBusiness.GetList().Where(d => d.IsDel == false).Where(d => d.DeptId == 2).ToList().OrderBy(d => d.Pid).ToList();


            List<EmployeesInfo> result = new List<EmployeesInfo>();

            foreach (var item in emps)
            {
                foreach (var item1 in ind)
                {
                    if (item.PositionId == item1.Pid)
                    {
                        result.Add(item);
                    }
                }
            }
            var list = new List<EmployeesInfo>();

            foreach (var item in result)
            {
                var ss = this.GetTeachers().Where(d => d.EmployeeId == item.EmployeeId).ToList();

                if (ss.Count <= 0 || ss == null)
                {
                    list.Add(item);
                }

            }

            return list;
        }


        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>

        public List<Teacher> BrushSelectionTeacher(string Name, string Phone)
        {
            return null;
        }



        /// <summary>
        /// 根据岗位获取教员
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>

        public List<Teacher> BrushSelectionTeacher(int positionId)
        {

            List<Teacher> resultlist = new List<Teacher>();

            var emps = db_emp.GetList().Where(d => d.PositionId == positionId && d.IsDel == false).ToList();

            var teachers = this.GetTeachers();

            foreach (var e in emps)
            {

                foreach (var t in teachers)
                {
                    if (t.EmployeeId == e.EmployeeId)
                    {
                        resultlist.Add(t);

                    }
                }

            }

            return resultlist;
        }


        /// <summary>
        /// 获取阶段教员
        /// </summary>
        /// <param name="grandid"></param>
        /// <returns></returns>
        public List<Teacher> BrushSelectionByGrand(int grandid)
        {
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var ss = baseBusiness.GetList().Where(d => d.Stage == grandid).ToList();

            List<TecharOnstageBearing> onstageBearings = new List<TecharOnstageBearing>();


            foreach (var item in ss)
            {
                if (!TecharOnstageBearing.ISContainTeacher(onstageBearings, item.TeacherID))
                {
                    onstageBearings.Add(item);
                }
            }

            List<Teacher> resultlist = new List<Teacher>();

            foreach (var item in onstageBearings)
            {
                resultlist.Add(this.GetTeachers().Where(d => d.TeacherID == item.TeacherID).FirstOrDefault());
            }

            return resultlist;
        }

        /// <summary>
        ///获取教员
        /// </summary>
        /// <param name="majorid">阶段</param>
        /// <returns>老师集合</returns>

        public List<Teacher> BrushSelectionByMajor(int majorid)
        {

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var ss = baseBusiness.GetList().Where(d => d.Major == majorid).ToList();

            List<TecharOnstageBearing> onstageBearings = new List<TecharOnstageBearing>();


            foreach (var item in ss)
            {
                if (!TecharOnstageBearing.ISContainTeacher(onstageBearings, item.TeacherID))
                {
                    onstageBearings.Add(item);
                }
            }

            List<Teacher> resultlist = new List<Teacher>();

            foreach (var item in onstageBearings)
            {
                resultlist.Add(this.GetTeachers().Where(d => d.TeacherID == item.TeacherID && d.IsDel == false).FirstOrDefault());
            }

            return resultlist;
        }


        public List<Teacher> getTeacherByMajorAndGrand(int majorid, int grandid)
        {

            List<Teacher> resultlist = new List<Teacher>();

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var ss = baseBusiness.GetList().Where(d => d.Major == majorid && grandid == d.Stage).ToList();


            foreach (var item in ss)
            {
                resultlist.Add(this.GetTeachers().Where(d => d.TeacherID == item.TeacherID && d.IsDel == false).FirstOrDefault());
            }

            return resultlist;
        }

        /// <summary>
        /// 获取教员擅长的技术
        /// </summary>
        /// <param name="teacherid">教员id</param>
        /// <param name="majorid">专业id</param>
        /// <returns></returns>
        public List<Curriculum> GetTeacherGoodCurriculum(int teacherid, int majorid)
        {

            List<Curriculum> resultlist = new List<Curriculum>();

            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();
            BaseBusiness<Curriculum> Curriculum_db = new BaseBusiness<Curriculum>();
            var temp = goodskill_db.GetList().Where(d => d.TearchID == teacherid).ToList();


            foreach (var item in temp)
            {
                var curr = Curriculum_db.GetList().Where(d => d.IsDelete == false && d.CurriculumID == item.Curriculum).FirstOrDefault();

                if (curr.MajorID == majorid)
                {

                    resultlist.Add(curr);
                }

            }

            return resultlist;
        }


        public List<Curriculum> GetCurriculaOnTeacherNoHave(int teacherid, int majorid)
        {



            var resultlist = new List<Curriculum>();

            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();

            BaseBusiness<Curriculum> curr_db = new BaseBusiness<Curriculum>();

            var temp1 = goodskill_db.GetList().Where(d => d.TearchID == teacherid).ToList().OrderBy(d => d.Curriculum).ToList();

            var temp2 = db_specialty.GetList().Where(d => d.IsDelete == false && d.Id == majorid).FirstOrDefault();

            var temp3 = curr_db.GetList().Where(d => d.IsDelete == false && d.MajorID == majorid).ToList();


            var all = new List<Curriculum>();

            foreach (var item in temp1)
            {
                var cur = temp3.Where(d => d.CurriculumID == item.Curriculum).FirstOrDefault();

                if ( cur !=null && cur.MajorID == majorid)
                {

                    all.Add(cur);

                }
            }
            CurriculumBusiness curriculumBusiness = new CurriculumBusiness();

            //这个专业的技能

            all = all.OrderBy(d => d.CurriculumID).ToList();


            foreach (var item in temp3)
            {

                if (!curriculumBusiness.IsHave(all, item.CurriculumID))
                {
                    resultlist.Add(item);

                }

            }

            return resultlist;
        }

        /// <summary>
        /// 给教员添加新的技能
        /// </summary>
        /// <param name="teacherid"></param>
        /// <param name="currid"></param>
        public void SetNewSkillToTeacher(int teacherid, string[] currid)
        {

            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();


            foreach (var item in currid)
            {
                if (goodskill_db.GetList().Where(d => d.TearchID == teacherid && d.Curriculum == int.Parse(item)).FirstOrDefault() == null)
                {

                    GoodSkill goodSkill = new GoodSkill();
                    goodSkill.Curriculum = int.Parse(item);
                    goodSkill.TearchID = teacherid;

                    goodskill_db.Insert(goodSkill);

                }
            }

        }
        /// <summary>
        /// 给教员分配阶段
        /// </summary>
        /// <param name="ids">阶段</param>
        /// <param name="majorid">专业id</param>
        /// <param name="teacherid">教员id</param>
        /// <returns></returns>
        public void SetGrandToTeacherOnMajor(string[] grands, int majorid, int teacherid)
        {
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            if (this.GetTeacherByID(teacherid) != null)
            {

                foreach (var item in grands)
                {

                    var tempobj = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Stage == int.Parse(item) && majorid == d.Major).FirstOrDefault();

                    var tempobj1 = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Stage == null && majorid == d.Major).FirstOrDefault();

                    if (tempobj == null  && tempobj1==null)
                    {

                        TecharOnstageBearing techarOnstageBearing = new TecharOnstageBearing();

                        techarOnstageBearing.TeacherID = teacherid;

                        techarOnstageBearing.Stage = int.Parse(item);

                        techarOnstageBearing.Major = majorid;


                        baseBusiness.Insert(techarOnstageBearing);

                    }

                    if (tempobj1 != null)
                    {

                        tempobj1.Stage = int.Parse(item);
                        baseBusiness.Update(tempobj1);
                    }



                }


            }


        }


        /// <summary>
        /// 获取教员没有的专业
        /// </summary>
        /// <param name="teacherid"></param>
        /// <returns></returns>

        public List<Specialty> GetNoHaveMajorOnTeacher(int teacherid)
        {
            List<Specialty> resultlist = new List<Specialty>();

            if (this.GetTeacherByID(teacherid) == null)
            {
                return resultlist;
            }


            List<Specialty> havemajors = this.GetMajorByTeacherID(teacherid).OrderBy(d => d.Id).ToList();

            List<Specialty> templist = db_specialty.GetList().OrderBy(d => d.Id).ToList();

            foreach (var item in templist)
            {
                if (!Specialty.IsContain(havemajors, item))
                {
                    resultlist.Add(item);
                }
            }

            return resultlist;
        }


        /// <summary>
        /// 给教员添加专业
        /// </summary>
        public void SetMajorToTeacher(string[] ids, int teacherid)
        {

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            if (this.GetTeacherByID(teacherid) != null)
            {

                foreach (var item in ids)
                {
                    var tempobj1 = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == int.Parse(item) && d.Stage == null).FirstOrDefault();

                    if (tempobj1 == null)
                    {

                        //添加
                        TecharOnstageBearing techarOnstageBearing = new TecharOnstageBearing();
                        techarOnstageBearing.TeacherID = teacherid;
                        techarOnstageBearing.Major = int.Parse(item);

                        baseBusiness.Insert(techarOnstageBearing);

                    }

                }
            }

        }
        /// <summary>
        /// 移除教员在专业上对应的阶段
        /// </summary>
        /// <param name="grandid">阶段ID</param>
        /// <param name="teacherid">教员ID</param>
        /// <param name="majorid">专业ID</param>
        /// <returns></returns>
        public void RemoveGrandOnTeacherMajor(int grandid, int teacherid, int majorid)
        {

            if (this.GetTeacherByID(teacherid) == null)
                return;


            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var temp = baseBusiness.GetList();
            var temp2list = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == majorid).ToList();
            foreach (var item in temp)
            {

                
                    if (item.TeacherID == teacherid && item.Major == majorid && item.Stage == grandid)
                    {
                        if (temp2list.Count > 1)
                        {

                            baseBusiness.Delete(item);
                             break;   
                        }
                        else
                        {
                            item.Stage = null;

                            baseBusiness.Update(item);

                            break;
                        }

                   
                    }
                


               
            }
        }


        /// <summary>
        /// 移除教员擅长的课程
        /// </summary>
        /// <param name="teacherid">教员ID</param>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public void removeTeacherGoodSkill(int teacherid, int courseid)
        {
            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();

            if (this.GetTeacherByID(teacherid) == null)
                return;

           var tempobj= goodskill_db.GetList().Where(d => d.TearchID == teacherid && d.Curriculum == courseid).FirstOrDefault();

            if (tempobj == null)
                return;

            goodskill_db.Delete(tempobj);

        }


        /// <summary>
        /// 添加教员
        /// </summary>
        /// <returns></returns>
        
        public void AddTeacher(Teacher teacher)
        {

                teacher.IsDel = false;

                teacher.MinimumCourseHours = 0;

         

                this.Insert(teacher);

                //更新缓存
                RedisCache redisCache = new RedisCache();

                redisCache.RemoveCache("TeacherList");


              
            
           

        }


    }
}
