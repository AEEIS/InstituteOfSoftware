﻿using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业部调研学生访谈业务类
    /// </summary>
    public class SurveyRecordsBusiness : BaseBusiness<SurveyRecords>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        /// <summary>
        /// 获取全部数据 没有伪删除的数据
        /// </summary>
        /// <returns></returns>
        public List<SurveyRecords> GetSurveys() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据班级编号返回学生访谈记录
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SurveyRecords> GetSurveyRecordsByclassno(int  classid) {
            var list = this.GetSurveys();
            List<SurveyRecords> dd = new List<SurveyRecords>();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var list1 = dbproScheduleForTrainees.GetTraineesByClassid(classid);
            foreach (var item in list)
            {
                foreach (var item1 in list1)
                {
                    if (item.StudentNO == item1.StudentID)
                    {
                        dd.Add(item);
                    }
                }
            }
            return dd;
        }

        /// <summary>
        ///获取cd类学生记录  如果第一次调研时C 或者时D  第二次调研却时A 或 者B 的话 就不算 反之
        /// </summary>
        /// <returns></returns>
        public List<SurveyRecords> GetCDSurveyRecords(List<SurveyRecords> data) {

            List<DuplicateSurveyRecords> Duplicatedata = new List<DuplicateSurveyRecords>();
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = data.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {
                    if (data[i].StudentNO==data[j].StudentNO)
                    {
                        if (Duplicatedata.Count!=0)
                        {
                            foreach (var item in Duplicatedata)
                            {
                                if (data[j].StudentNO == item.StudentNumber)
                                {
                                    item.Duplicatedata.Add(data[j]);
                                }
                                else
                                {
                                    DuplicateSurveyRecords view = new DuplicateSurveyRecords();
                                    view.StudentNumber = data[j].StudentNO;
                                    view.Duplicatedata.Add(data[j]);
                                    Duplicatedata.Add(view);
                                }
                            }
                        }
                        else
                        {
                            DuplicateSurveyRecords view = new DuplicateSurveyRecords();
                            var da = data[j];
                            view.StudentNumber = data[j].StudentNO;
                            view.Duplicatedata.Add(da);
                            Duplicatedata.Add(view);
                        }
                        data.Remove(data[j]);
                    }
                }
            }
            foreach (var item in Duplicatedata)
            {
                data.Add(item.Duplicatedata.OrderByDescending(a => a.RecordsDate).FirstOrDefault());
            }
       
            data= data.Where(a => a.SurRating.ToUpper() == "C" || a.SurRating.ToUpper() == "D").ToList();
            for (int i = 0; i < data.Count; i++)  //外循环是循环的次数
            {
                for (int j = data.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {

                    if (data[i].StudentNO == data[j].StudentNO)
                    {
                        data.RemoveAt(j);
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 根据班级获取CD类学生
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SurveyRecords> GetCDSurveyRecordsByclassid(int classid)
        {
            //quan bu cd 里面的数据有同一个人
            var list = this.GetSurveyRecordsByclassno(classid);
            var reuslt= this.GetCDSurveyRecords(list);
            List<SurveyRecords> dd = new List<SurveyRecords>();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var list1 = dbproScheduleForTrainees.GetTraineesByClassid(classid);
            foreach (var item in reuslt)
            {
                foreach (var item1 in list1)
                {
                    if (item.StudentNO == item1.StudentID)
                    {
                        dd.Add(item);
                    }
                }
            }
           
            return dd;
        }

        /// <summary>
        /// 根据学生编号返回该对应的访谈记录
        /// </summary>
        /// <returns></returns>
        public  List<SurveyRecords> GetSurveysByStudentNumber(string StudentNumber) {
           return this.GetSurveys().Where(a => a.StudentNO == StudentNumber).ToList();
        }

        /// <summary>
        /// 判断这个学生是否是cd
        /// </summary>
        /// <returns></returns>
        public bool judgeStuisCD(string StudentNumber) {
            bool result = false;
            SurveyRecords survey = this.GetSurveysByStudentNumber(StudentNumber).OrderByDescending(a => a.RecordsDate).FirstOrDefault();
            if (survey != null)
            {
                if (survey.SurRating.ToUpper() == "C" || survey.SurRating.ToUpper() == "D")
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
