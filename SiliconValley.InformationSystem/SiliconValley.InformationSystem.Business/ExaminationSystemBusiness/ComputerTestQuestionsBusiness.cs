﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView;
    using SiliconValley.InformationSystem.Util;


    /// <summary>
    /// 机试题业务类
    /// </summary>
    public class ComputerTestQuestionsBusiness : BaseBusiness<MachTestQuesBank>
    {

        private readonly QuestionLevelBusiness db_questionLevel;

        private readonly TeacherBusiness db_teacher;

        private readonly BaseBusiness<EmployeesInfo> db_emp;

        /// <summary>
        /// 试卷业务类实例
        /// </summary>
        private readonly ExaminationPaperBusiness db_ExamPaper;

        public ComputerTestQuestionsBusiness()
        {
            db_questionLevel = new QuestionLevelBusiness();
            db_emp = new BaseBusiness<EmployeesInfo>();
            db_teacher = new TeacherBusiness();
            db_ExamPaper = new ExaminationPaperBusiness();
        }

        /// <summary>
        /// 获取所有机试题
        /// </summary>
        /// <returns></returns>
        public List<MachTestQuesBank> AllComputerTestQuestion()
        {
            return this.GetList();
        }

        public ComputerTestQuestionsView ConvertToComputerTestQuestionsView(MachTestQuesBank machTestQuesBank, bool IsNeedProposition)
        {

            ComputerTestQuestionsView computerTestQuestionsView = new ComputerTestQuestionsView();

            computerTestQuestionsView.Grand = machTestQuesBank.Grand;

            CourseBusiness courseBusiness = new CourseBusiness();
            if (machTestQuesBank.Course == 0)
            {
                computerTestQuestionsView.Course = null;
            }
            else
            {
                computerTestQuestionsView.Course = courseBusiness.GetCurriculas().Where(d => d.CurriculumID == machTestQuesBank.Course).FirstOrDefault();
            }

            computerTestQuestionsView.CreateDate = machTestQuesBank.CreateDate;
            computerTestQuestionsView.ID = machTestQuesBank.ID;
            computerTestQuestionsView.IsUsing = machTestQuesBank.IsUsing;
            computerTestQuestionsView.Level = db_questionLevel.AllQuestionLevel().Where(d => d.LevelID == machTestQuesBank.Level).FirstOrDefault();

            if (IsNeedProposition)
            {

                var empid = db_teacher.GetEntity(machTestQuesBank.Proposition).EmployeeId;
                computerTestQuestionsView.Proposition = db_emp.GetList().Where(c => c.EmployeeId == empid).FirstOrDefault();

            }

            else
            {
                computerTestQuestionsView.Proposition = null;
            }
            computerTestQuestionsView.SaveURL = machTestQuesBank.SaveURL;
            computerTestQuestionsView.Title = machTestQuesBank.Title;

            //计算被使用次数

            computerTestQuestionsView.UsageCount = db_ExamPaper.AllComputerTestQuestionPaper().Where(d => d.MachineTestQuestionID == machTestQuesBank.ID).Count();

            return computerTestQuestionsView;


        }

        /// <summary>
        /// 读取rtf文件
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public string Read_RTF_File(string path)
        {

            System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

            // Get the contents of the RTF file. Note that when it is
            // stored in the string, it is encoded as UTF-16.
            string s = System.IO.File.ReadAllText(path);

            // Display the RTF text.
            //System.Windows.Forms.MessageBox.Show(s);

            // Convert the RTF to plain text.
            rtBox.Rtf = s;
            string plainText = rtBox.Text;

            // Display plain text output in MessageBox because console
            // cannot display Greek letters.
            System.Windows.Forms.MessageBox.Show(plainText);

            return plainText;

        }


        /// <summary>
        /// 启用或者禁用
        /// </summary>
        public void DisableOrEnable(List<string> ComputerTestQuestionIds)
        {

            foreach (var item in ComputerTestQuestionIds)
            {

                var obj = this.AllComputerTestQuestion().Where(d => d.ID == int.Parse(item)).FirstOrDefault();

                bool isUsing = obj.IsUsing == true ? false : true;

                obj.IsUsing = isUsing;

                this.Update(obj);

            }

        }


        /// <summary>
        /// 判断这套机试题是否被使用过
        /// </summary>
        /// <returns></returns>
        public bool IsItUsed(int id)
        {
            var list = db_ExamPaper.AllComputerTestQuestionPaper();

            foreach (var item in list)
            {
                if (item.ID == id)
                {
                    return true;
                }
            }

            return false;


        }

        public AjaxResult Delete(int id)
        {
            AjaxResult result = new AjaxResult();

            //被使用的选择题不能被删除

            if (!this.IsItUsed(id))
            {
                var obj = this.AllComputerTestQuestion().Where(d => d.ID == id).FirstOrDefault();

                //可以删除

                try
                {
                    this.Delete(obj);

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



            }
            else
            {
                result.ErrorCode = 400;
                result.Msg = "失败";
                result.Data = null;
            }

            return result;

        }

        /// <summary>
        /// 筛选解答题题目
        /// </summary>
        /// <returns></returns>
        public List<MachTestQuesBank> Search(string Title, int course)
        {

            List<MachTestQuesBank> resultlist = new List<MachTestQuesBank>();

            if (course == 0 && Title == "")
            {
                resultlist = this.AllComputerTestQuestion().ToList();
            }

            if (course != 0 && Title == "")
            {
                resultlist = this.AllComputerTestQuestion().Where(d => d.Course == course).ToList();
            }

            if (Title != "" && course != 0)
            {
                resultlist = this.AllComputerTestQuestion().Where(d => d.Course == course && d.Title.Contains(Title)).ToList();
            }
            if (Title != "" && course == 0)
            {
                resultlist = this.AllComputerTestQuestion().Where(d => d.Title.Contains(Title)).ToList();
            }

            return resultlist;
        }

    }
}
