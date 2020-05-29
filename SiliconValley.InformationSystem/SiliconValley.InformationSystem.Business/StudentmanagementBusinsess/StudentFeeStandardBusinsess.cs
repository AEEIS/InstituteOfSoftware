﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.FinaceBusines;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.EnrollmentBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;

namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{

    public class StudentFeeStandardBusinsess : BaseBusiness<StudentFeeStandard>
    {
        private static int count = 0;

        //专业表
        BaseBusiness<Specialty> special = new BaseBusiness<Specialty>();
        //明目类型
        BaseBusiness<CostitemsX> costitemssX = new BaseBusiness<CostitemsX>();
        //班主任表
        HeadmasterBusiness headmasters = new HeadmasterBusiness();
        //学员班级
        ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
        //阶段专业表
        BaseBusiness<StageGrade> stagegrade = new BaseBusiness<StageGrade>();
        //阶段表
        BaseBusiness<Grand> geand = new BaseBusiness<Grand>();
        //班级表
        ClassScheduleBusiness classschedu = new ClassScheduleBusiness();
        //学费详情
        BaseBusiness<Studenttuitionfeestandard> Feestandard = new BaseBusiness<Studenttuitionfeestandard>();
        //学员信息
        StudentInformationBusiness studentInformationBusiness = new StudentInformationBusiness();
        //费用明目
        CostitemsBusiness costitemsBusiness = new CostitemsBusiness();
        //自考本科
        EnrollmentBusinesse enrollmentBusiness = new EnrollmentBusinesse();
        //升学阶段
        BaseBusiness<GotoschoolStage> GotoschoolStageBusiness = new BaseBusiness<GotoschoolStage>();
        //审核缴费业务类
        BaseBusiness<Payview> PayviewBusiness = new BaseBusiness<Payview>();
        //核对缴费是否成功业务类
        BaseBusiness<Paymentverification> PaymentverificationBusiness = new BaseBusiness<Paymentverification>();
        //审核缴费信息对应业务类
        BaseBusiness<PayviewPaymentver> PayviewPaymentverBusiness = new BaseBusiness<PayviewPaymentver>();
        /// <summary>
        /// 获取所有学员数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="Name">姓名</param>
        /// <param name="Sex">性别</param>
        /// <param name="StudentNumber">学号</param>
        /// <param name="identitydocument">身份证号码</param>
        /// <returns></returns>
        public object GetDate(int page, int limit, string Name, string Sex, string StudentNumber, string identitydocument)
        {
            //班主任带班
            BaseBusiness<HeadClass> Hoadclass = new BaseBusiness<HeadClass>();
            //    List<StudentInformation>list=  dbtext.GetPagination(dbtext.GetIQueryable(),page,limit, dbtext)
            // List<StudentInformation> list = studentInformationBusiness.Mylist("StudentInformation").Where(a => a.IsDelete ==false).ToList();
            List<StudentInformation> list = studentInformationBusiness.GetList().Where(a => a.IsDelete == false).ToList();

            if (!string.IsNullOrEmpty(Name))
            {
                list = list.Where(a => a.Name.Contains(Name)).ToList();
            }
            if (!string.IsNullOrEmpty(Sex))
            {
                bool sex = Convert.ToBoolean(Sex);
                list = list.Where(a => a.Sex == sex).ToList();
            }
            if (!string.IsNullOrEmpty(StudentNumber))
            {
                list = list.Where(a => a.StudentNumber.Contains(StudentNumber)).ToList();
            }
            if (!string.IsNullOrEmpty(identitydocument))
            {
                list = list.Where(a => a.identitydocument.Contains(identitydocument)).ToList();
            }


            var xz = list.Select(a => new
            {
                a.StudentNumber,
                a.Name,
                a.Sex,
                a.BirthDate,
                a.identitydocument,
                ClassName = scheduleForTraineesBusiness.SutdentCLassName(a.StudentNumber)==null?"暂无": scheduleForTraineesBusiness.SutdentCLassName(a.StudentNumber).ClassID,
                Headmasters = headmasters.Listheadmasters(a.StudentNumber) == null ? "暂无" : headmasters.Listheadmasters(a.StudentNumber).EmpName

            }).ToList();
            var dataList = xz.OrderBy(a => a.StudentNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = xz.Count,
                data = dataList
            };
            return data;

        }
        /// <summary>
        /// 下拉框获取价格
        /// </summary>
        /// <param name="Grand_id">阶段</param>
        /// <param name="Rategory">明目类型</param>
        /// <returns></returns>
        public Costitems Singlecostitems(int? Grand_id, int Rategory)
        {
            var list = costitemsBusiness.costitemslist().Where(a => a.Rategory == Rategory);
            if (Grand_id != null)
            {
                list = list.Where(a => a.Grand_id == Grand_id);
            }
            return list.FirstOrDefault();
        }
        //学员费用
        BaseBusiness<StudentFeeRecord> studentfee = new BaseBusiness<StudentFeeRecord>();
        //财务人员
        BaseBusiness<FinanceModel> finacemo = new BaseBusiness<FinanceModel>();
        /// <summary>
        /// 添加学员费用
        /// </summary>
        /// <param name="studentFeeRecord">费用对象数据</param>
        /// <returns></returns>
        public AjaxResult AddstudentFeeRecord(StudentFeeRecord studentFeeRecord)
        {
            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            AjaxResult retus = null;
            try
            {
                var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
                studentFeeRecord.FinanceModelid = fine.id;
                studentFeeRecord.IsDelete = false;
                studentFeeRecord.AddDate = DateTime.Now;

                studentfee.Insert(studentFeeRecord);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "录入费用成功";
                BusHelper.WriteSysLog("添加费用费用数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }
        /// <summary>
        /// 自考本科费用录入
        /// </summary>
        /// <param name="studentFeeRecord">数据对象</param>
        /// <param name="Rategorys">多个费用明目id</param>
        /// <returns></returns>
        public AjaxResult Tuitionandfees(Payview studentFee)
        {
            List<Payview> listFeeRecord = new List<Payview>();

            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
            AjaxResult retus = null;
            try
            {
            
                studentFee.FinanceModelid = fine.id;
                studentFee.IsDelete = false;
                studentFee.AddDate = DateTime.Now;
                this.Studentpayment(studentFee.StudenID, fine.id, 1);
                listFeeRecord.Add(studentFee);
                SessionHelper.Session["person"] = listFeeRecord;
               
              
                PayviewBusiness.Insert(listFeeRecord);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "录入费用成功";
                BusHelper.WriteSysLog("添加费用费用数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;

        }
        /// <summary>
        /// 没有阶段的费用明目在这获取费用
        /// </summary>
        /// <param name="id">明目id</param>
        /// <returns></returns>
        public int Otherexpenses(string id)
        {

            string[] mingmu = id.Split(',');
            //价格
            int price = 0;
            foreach (var item in mingmu)
            {
                int cid = int.Parse(item);
                price = price + Convert.ToInt32(costitemsBusiness.GetEntity(cid).Amountofmoney);
            }
            return price;

        }
        /// <summary>
        /// 根据名目类型获取明目
        /// </summary>
        /// <param name="Typeid">类型id</param>
        /// <returns></returns>
        public List<Costitems> ListTuitionandfees(int Typeid)
        {
            return costitemsBusiness.costitemslist().Where(a => a.Rategory == Typeid).ToList();
        }
        /// <summary>
        /// 根据学号获取学杂费获取当前没有缴费的名目
        /// </summary>
        /// <param name="StudentID">学号</param>
        /// <param name="Typeid"><明目类型/param>
        /// <returns></returns>
        public List<Costitems> boolTuitionandfees(string StudentID, int Typeid)
        {
            var list = this.ListTuitionandfees(Typeid);
            var FeeRecordsList = this.SinglestudentFeeRecords(StudentID);
            foreach (var item in FeeRecordsList)
            {
                list = list.Where(a => a.id != item.Costitemsid).ToList();
            }
            return list;

        }
        /// <summary>
        /// 获取所有缴费记录
        /// </summary>
        /// <returns></returns>
        public List<StudentFeeRecord> ListstudentFeeRecords()
        {
            return studentfee.GetList().Where(a => a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取单个学员所有缴费记录
        /// </summary>
        /// <param name="Studentid">学号</param>
        /// <returns></returns>
        public List<StudentFeeRecord> SinglestudentFeeRecords(string Studentid)
        {
            return this.ListstudentFeeRecords().Where(a => a.StudenID == Studentid).ToList();
        }
        /// <summary>
        /// 学员费用查询复杂
        /// </summary>
        /// <param name="Grand_id">阶段</param>
        /// <returns></returns>
        public object Studentfeepayment(int Grand_id, string studentid)
        {
            int countfee = 0;
            var idcost = costitemssX.GetList().Where(a => a.IsDelete == false && a.Name == "学杂费").FirstOrDefault().id;
            var costitemslist = costitemsBusiness.costitemslist().Where(a => a.Rategory == idcost&&a.IsDelete==false).ToList();
            foreach (var item in costitemslist)
            {
                var x = PayviewBusiness.GetList().Where(a => a.IsDelete == false && a.StudenID == studentid && a.Costitemsid == item.id).FirstOrDefault();
                if (x!= null)
                {
                    var Paymentverid = PayviewPaymentverBusiness.GetList().Where(a => a.Payviewid == x.ID).FirstOrDefault().Paymentver;
                  if(PaymentverificationBusiness.GetEntity(Paymentverid).Passornot==true|| PaymentverificationBusiness.GetEntity(Paymentverid).Passornot==null)
                    {
                        countfee++;
                    }
                }
            
            }

            return costitemsBusiness.GetList().Where(a => a.Grand_id == Grand_id&&a.IsDelete==false).Select(a => new { a.id, a.Name, a.Amountofmoney, Rategory = costitemssX.GetEntity(a.Rategory).Name, countfee }).ToList();
        }
        /// <summary>
        /// 根据学号获取姓名，性别，班级，身份证，学号
        /// </summary>
        /// <param name="studentid">学号</param>
        /// <returns></returns>
        public object StudentFind(string studentid)
        {

            var a = studentInformationBusiness.GetEntity(studentid);
            var ClassID = scheduleForTraineesBusiness.SutdentCLassName(a.StudentNumber).ID_ClassName;
            var x = new
            {
                StudentNumber = a.StudentNumber,//学号
                Name = a.Name,//姓名
                identitydocument = a.identitydocument,//身份证号码
                Sex = a.Sex,//性别,

                GrandName = classschedu.GetClassGrand(ClassID, 22),//阶段
                classa = classschedu.GetList().Where(q => q.IsDelete == false && q.ClassStatus == false && q.id == ClassID).FirstOrDefault().ClassNumber//班级号                                                                                                                                              //a => a.IsDelete == false && a.ClassStatus == false
            };
            return x;
        }
      
        /// <summary>
        /// 学员阶段费用录入
        /// </summary>
        /// <param name="studentFeeRecords">集合数据</param>
        /// <returns></returns>
        public AjaxResult StudentPrices(List<StudentFeeRecord> studentFeeRecords, string Remarks)
        {
            string studentid = "";
            int Costitemsid = 0;
            AjaxResult retus = null;
            List<Payview> listFeeRecord = new List<Payview>();
            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
            if (fine == null)
            {

                retus = new ErrorResult();
                retus.Msg = "对不起,当前登陆账号不能进行缴费！";
                retus.Success = false;
                retus.ErrorCode = 500;
            }
            else
            {
                Costitemsid = fine.id;
                foreach (var item in studentFeeRecords)
                {
                    studentid= item.StudenID;
                  
                    Payview studentFeeRecord = new Payview();
                  
                    studentFeeRecord.IsDelete = false;
                    studentFeeRecord.AddDate = DateTime.Now;
                    studentFeeRecord.FinanceModelid = fine.id;
                    studentFeeRecord.Costitemsid = item.Costitemsid;
                    studentFeeRecord.Amountofmoney = item.Amountofmoney;
                    studentFeeRecord.StudenID = item.StudenID;
                    studentFeeRecord.Remarks = Remarks;
                    listFeeRecord.Add(studentFeeRecord);
                }
                try
                {
                    SessionHelper.Session["person"] = listFeeRecord;
                    PayviewBusiness.Insert(listFeeRecord);
                    //添加核对表
                    Paymentverification paymentverification = new Paymentverification();
                    paymentverification.Passornot = null;
                    paymentverification.OddNumbers = null;
                    paymentverification.AddDate = null;
                    PaymentverificationBusiness.Insert(paymentverification);
                    List<PayviewPaymentver> payviewPaymentverslist = new List<PayviewPaymentver>();
                    //
                 var FeeRecordlist=  PayviewBusiness.GetList().Where(a => a.StudenID == studentid && a.FinanceModelid == Costitemsid).OrderByDescending(a => a.ID).Take(listFeeRecord.Count()).ToList();
                    foreach (var item in FeeRecordlist)
                    {
                        PayviewPaymentver payviewPaymentver = new PayviewPaymentver();
                        payviewPaymentver.Paymentver = PaymentverificationBusiness.GetList().OrderByDescending(a => a.id).Take(1).FirstOrDefault().id;
                        payviewPaymentver.Payviewid = item.ID;
                        payviewPaymentverslist.Add(payviewPaymentver);
                    }
                    PayviewPaymentverBusiness.Insert(payviewPaymentverslist);
                    retus = new SuccessResult();
                    retus.Success = true;
                    retus.Msg = "录入费用成功";
                    BusHelper.WriteSysLog("录入费用模拟数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                catch (Exception ex)
                {

                    retus = new ErrorResult();
                    retus.Msg = "服务器错误";
                    retus.Success = false;
                    retus.ErrorCode = 500;
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
            }
            return retus;
        }
        /// <summary>
        /// 验证当前是否有操作权限
        /// </summary>
        /// <returns></returns>
        public bool IsFinanc()
        {
            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
            if (fine == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 收据项目及费用详情
        /// </summary>
        /// <param name="studentFeeRecords">集合数据</param>
        /// <returns></returns>
        public object Receiptdata(List<Payview> studentFeeRecords)
        {
            int Stage = Convert.ToInt32(SessionHelper.Session["Stage"]);
            return studentFeeRecords.Select(a => new
            {
                a.Amountofmoney,
                ostitemsName = costitemsBusiness.GetEntity(a.Costitemsid).Name,
                GrandName = Stage > 0 ? geand.GetEntity(Stage).GrandName : "",
                Rategory = costitemssX.GetEntity(costitemsBusiness.GetEntity(a.Costitemsid).Rategory).Name,
                Remarks = a.Remarks,
                a.AddDate

            }).ToList();
        }
        /// <summary>
        /// 根据学号查询出所有缴费记录
        /// </summary>
        /// <param name="student">学号</param>
        /// <returns></returns>
        public List<vierprice> FienPrice(string student)
        {
            List<object> students = new List<object>();
            var st = studentfee.GetList().Where(a => a.IsDelete == false && a.StudenID == student).ToList();
            foreach (var item in st)
            {
                StudentFeeRecord record = new StudentFeeRecord();

                students.Add(Convert.ToDateTime(item.AddDate).ToLongDateString().ToString());
            }
            var x = students.Distinct().ToList();
            List<vierprice> listvier = new List<vierprice>();
            // string date = Convert.ToDateTime(item.AddDate).ToLongDateString().ToString();
            foreach (var item1 in x)
            {
                vierprice vierprices = new vierprice();
                vierprices.Date = item1.ToString();
                List<vierprice> view = new List<vierprice>();
                foreach (var item in st)
                {

                    string date = Convert.ToDateTime(item.AddDate).ToLongDateString().ToString();
                    if (item1.ToString() == date)
                    {
                        var costit = costitemsBusiness.GetEntity(item.Costitemsid);
                        vierprice stivier = new vierprice();
                        stivier.Amountofmoney = (decimal)item.Amountofmoney;
                        stivier.CostitemName = costit.Name;
                        stivier.GrandName = costit.Grand_id != null ? geand.GetEntity(costit.Grand_id).GrandName : "";
                        stivier.Rategory = costitemssX.GetEntity(costit.Rategory).Name;
                        view.Add(stivier);
                    }
                }
                vierprices.Chicked = view;
                listvier.Add(vierprices);
            }
            return listvier;

        }
        /// <summary>
        /// 其它缴费数据操作
        /// </summary>
        /// <param name="StudenID">学号</param>
        /// <param name="Consumptionname">名称</param>
        /// <param name="Amountofmoney">金额</param>
        /// <param name="Remarks">备注</param>
        /// <param name="Typeid">名目</param>
        /// <returns></returns>
        public AjaxResult Otherconsumption(string StudenID, string Consumptionname, decimal Amountofmoney, string Remarks, int Typeid)
        {
            AjaxResult retus = null;
            try
            {
                //当前登陆人
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();
                var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
                var x = costitemsBusiness.GetList().Where(a => a.Name == Consumptionname).FirstOrDefault();
                if (x == null)
                {
                    Costitems costitems = new Costitems();
                    costitems.Amountofmoney = Amountofmoney;
                    costitems.Name = Consumptionname;
                    costitems.IsDelete = false;
                    costitems.Rategory = Typeid;
                    costitemsBusiness.Insert(costitems);
                    x = costitemsBusiness.GetList().Where(a => a.Name == Consumptionname).OrderByDescending(a => a.id).FirstOrDefault();
                }
                List<Payview> liststudents = new List<Payview>();
                Payview studentFee = new Payview();
                studentFee.StudenID = StudenID;
                studentFee.Amountofmoney = Amountofmoney;
                studentFee.AddDate = DateTime.Now;
                studentFee.Remarks = Remarks;
                studentFee.IsDelete = false;
                studentFee.Costitemsid = x.id;
                studentFee.FinanceModelid = fine.id;
                PayviewBusiness.Insert(studentFee);
                liststudents.Add(studentFee);
                this.Studentpayment(StudenID, fine.id,1);
                SessionHelper.Session["person"] = liststudents;
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "缴费成功";
                BusHelper.WriteSysLog("其它缴费数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;

        }
        /// <summary>
        /// 添加学员费用方法
        /// </summary>
        /// <param name="studentid">学号</param>
        /// <param name="Costitemsid">操作人</param>
        /// <returns></returns>
        public bool Studentpayment(string studentid,int Costitemsid,int count)
        {
            bool str = false;
            try
            {
                str=true;
                //添加核对表
                Paymentverification paymentverification = new Paymentverification();
                paymentverification.Passornot = null;
                paymentverification.OddNumbers = null;
                paymentverification.AddDate = null;
                PaymentverificationBusiness.Insert(paymentverification);
                List<PayviewPaymentver> payviewPaymentverslist = new List<PayviewPaymentver>();
                //
                var FeeRecordlist = PayviewBusiness.GetList().Where(a => a.StudenID == studentid && a.FinanceModelid == Costitemsid).OrderByDescending(a => a.ID).Take(count).ToList();
                foreach (var item in FeeRecordlist)
                {
                    PayviewPaymentver payviewPaymentver = new PayviewPaymentver();
                    payviewPaymentver.Paymentver = PaymentverificationBusiness.GetList().OrderByDescending(a => a.id).Take(1).FirstOrDefault().id;
                    payviewPaymentver.Payviewid = item.ID;
                    payviewPaymentverslist.Add(payviewPaymentver);
                }
                PayviewPaymentverBusiness.Insert(payviewPaymentverslist);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return str;
        }
        /// <summary>
        /// 获取学员缴费数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="StudentID">学号</param>
        /// <param name="Name">姓名</param>
        /// <param name="TypeID">类型</param>
        /// <param name="qBeginTime">开始时间</param>
        /// <param name="qEndTime">结束时间</param>
        /// <returns></returns>
        public List<StudentFeeRecordView> Nominaldata(string StudentID, string Name, string TypeID, string qBeginTime, string qEndTime)
        {
            EmployeesInfoManage employeesInfoManage = new EmployeesInfoManage();
            count++;
            //.Mylist("StudentFeeRecord")
            var x = studentfee.GetList().Where(a=>a.IsDelete==false).Select(a => new StudentFeeRecordView
            {
                StudenID = a.StudenID,
                AddDate = (DateTime)a.AddDate,
                Name = studentInformationBusiness.GetEntity(a.StudenID).Name,
                FinancialstaffName = employeesInfoManage.GetEntity(finacemo.GetEntity(a.FinanceModelid).Financialstaff).EmpName,
                ID = a.ID,
                Amountofmoney = (decimal)a.Amountofmoney,
                Costitemsid = a.Costitemsid,
                ClassName = scheduleForTraineesBusiness.SutdentCLassName(a.StudenID) == null ? "暂无班级" : scheduleForTraineesBusiness.SutdentCLassName(a.StudenID).ClassID,
                CostitemsName = costitemsBusiness.GetEntity(a.Costitemsid).Name,
                StageName= costitemsBusiness.GetEntity(a.Costitemsid).Grand_id==null?"": geand.GetEntity(costitemsBusiness.GetEntity(a.Costitemsid).Grand_id).GrandName
            }).ToList();
            if (count == 1)
            {
                x = x.Where(a => Convert.ToDateTime(a.AddDate).ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
            }
            if (!string.IsNullOrEmpty(StudentID))
            {
                x = x.Where(a => a.StudenID == StudentID).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                x = x.Where(a => a.Name.Contains(Name)).ToList();
            }
            if (!string.IsNullOrEmpty(TypeID))
            {
                int typeid = int.Parse(TypeID);

                var mycosti = costitemsBusiness.GetList().Where(a => a.Rategory == typeid).ToList();

                List<StudentFeeRecordView> feereview = new List<StudentFeeRecordView>();
                //往z集合添加数据
                foreach (var item in mycosti)
                {
                    var mx = x.Where(a => a.Costitemsid == item.id).ToList();
                    feereview.AddRange(mx);
                }
                x = feereview;
            }
            if (!string.IsNullOrEmpty(qBeginTime))
            {
                DateTime time = Convert.ToDateTime(qBeginTime);
                x = x.Where(a => Convert.ToDateTime(a.AddDate).ToShortDateString().ToDateTime() >= time).ToList();
            }
            if (!string.IsNullOrEmpty(qEndTime))
            {
                DateTime time = Convert.ToDateTime(qEndTime);
                x = x.Where(a => Convert.ToDateTime(a.AddDate).ToShortDateString().ToDateTime() <= time).ToList();
            }

            return x;

        }
        /// <summary>
        /// 获取总额统计
        /// </summary>
        /// <param name="StudentID">学号</param>
        /// <param name="Name">姓名</param>
        /// <param name="TypeID">类型</param>
        /// <param name="qBeginTime">开始时间</param>
        /// <param name="qEndTime">结束时间</param>
        /// <returns></returns>
        public List<TotalcostView> DateTatal(string StudentID, string Name, string TypeID, string qBeginTime, string qEndTime)
        {
            List<TotalcostView> totalcostViews = new List<TotalcostView>();
            var cost = costitemssX.GetList().Select(a => new TotalcostView { TypeID = a.id, TypeName = a.Name }).ToList();
            foreach (var item in cost)
            {
                var mycosti = costitemsBusiness.GetList().Where(a => a.Rategory == item.TypeID).ToList();
                TotalcostView view = new TotalcostView();
                view.TypeName = item.TypeName;

                foreach (var item1 in mycosti)
                {
                    TotalcostView views = new TotalcostView();
                    views.TypeID = item1.id;
                    view.ListTotalcostView.Add(views);
                }
                totalcostViews.Add(view);

            }
            var x = this.Nominaldata(StudentID, Name, TypeID, qBeginTime, qEndTime);
            List<TotalcostView> MyViewList = new List<TotalcostView>();

            foreach (var item1 in totalcostViews)
            {
                TotalcostView totalcost = new TotalcostView();
                totalcost.TypeName = item1.TypeName;
                totalcost.Total = 0;
                foreach (var item2 in item1.ListTotalcostView)
                {
                    foreach (var item in x)
                    {
                        if (item2.TypeID == item.Costitemsid)
                        {
                            totalcost.Total= totalcost.Total +item.Amountofmoney;
                        }
                    }
                }
                MyViewList.Add(totalcost);

            }

            return MyViewList;
        }
        /// <summary>
        /// 阶段应缴模型类
        /// </summary>
        public class ViewGotosch
        {
            /// <summary>
            /// 阶段id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 应交
            /// </summary>
            public decimal Price { get; set; }
        }
        /// <summary>
        /// 模型类
        /// </summary>
        public class ViewCostit
        {
            public string StudentID { get; set; }
            public int? GrandID { get; set; }
            public decimal? Price { get; set; }
        }
        /// <summary>
        /// 获取学员信息
        /// </summary>
        /// <param name="ClassName">班级号</param>
        /// <param name="StudentID">学号</param>
        /// <returns></returns>
        public DetailedcostView FineDetail(int ClassName,string StudentID)
        {
            var student = studentInformationBusiness.GetEntity(StudentID);
           DetailedcostView detailedcostView = new DetailedcostView();
            detailedcostView.ClassName = classschedu.GetEntity(ClassName).ClassNumber;
            detailedcostView.Name = student.Name;
            detailedcostView.Stidentid = student.StudentNumber;
            detailedcostView.Sex = student.Sex == false ? "女" : "男";
            detailedcostView.HeadmasterName = headmasters.ClassHeadmaster(ClassName) == null ? "无" : headmasters.ClassHeadmaster(ClassName).EmpName;
            detailedcostView.Phone = headmasters.ClassHeadmaster(ClassName) == null ? "无" : headmasters.ClassHeadmaster(ClassName).Phone;
            return detailedcostView;
        }
        /// <summary>
        /// 拿到学生欠费的数据
        /// </summary>
        /// <param name="ClassID"></param>
        /// <returns></returns>
        public List<DetailedcostView> TuitionFine(int ClassID)
        {
            //拿到学生已经交了费用的
            List<DetailedcostView> listdetailedc = new List<DetailedcostView>();
            //升学阶段
            BaseBusiness<GotoschoolStage> GotoschoolStageBusiness = new BaseBusiness<GotoschoolStage>();
            //阶段应交费用
            List<ViewGotosch> gotosches = new List<ViewGotosch>();
            //获取阶段价格单
            var costit= costitemsBusiness.GetList().Where(a => a.Grand_id != null).ToList();
               //当前阶段
             var ClassGrade = classschedu.GetEntity(ClassID).grade_Id;
            //拿到应该缴费的阶段
            var Gotoschool = classschedu.RecursionStage(ClassGrade);
            //拿到最后一个阶段
           var co= Gotoschool.Where(a => a.CurrentStageID == geand.GetList().Where(w => w.GrandName == "Y1" && w.IsDelete == false).FirstOrDefault().Id).FirstOrDefault();
            Gotoschool.Remove(co);
            Gotoschool.Add(new GotoschoolStage { CurrentStageID = ClassGrade, NextStageID = 0, ID = 0 });
           
            var StudentClass = classschedu.FintClassSchedule(ClassID);
            //拿到阶段以及应缴的费用
            foreach (var item in Gotoschool)
            {
                ViewGotosch viewGotosch = new ViewGotosch();
                viewGotosch.Price = 0;
                viewGotosch.id = item.CurrentStageID;
                foreach (var item1 in costit)
                {
                    if (item.CurrentStageID==item1.Grand_id)
                    {
                        viewGotosch.Price = viewGotosch.Price + item1.Amountofmoney;
                    }
                }
                gotosches.Add(viewGotosch);
            }

            var student = classschedu.ClassStudentneList(ClassID);
            foreach (var item in student)
            {
              var fee=  studentfee.GetList().Where(a => a.StudenID == item.StuNameID).Select(a=>new ViewCostit { GrandID= costitemsBusiness.GetEntity(a.Costitemsid).Grand_id, Price=a.Amountofmoney, StudentID=a.StudenID}).ToList();

                foreach (var item1 in Gotoschool)
                {
                    DetailedcostView detailedcostView = new DetailedcostView();
                    detailedcostView.CurrentStageID = geand.GetEntity(item1.CurrentStageID).GrandName;
                    detailedcostView.StagesID = item1.CurrentStageID;
                    detailedcostView.Amountofmoney = 0;
                    if (fee.Count>0)
                    {
                        foreach (var item2 in fee)
                        {
                            detailedcostView.Stidentid = item2.StudentID;
                            if (item2.GrandID == item1.CurrentStageID)
                            {
                                detailedcostView.Amountofmoney = detailedcostView.Amountofmoney + (decimal)item2.Price;
                            }
                        }
                    }
                    else
                    {
                        detailedcostView.Stidentid = item.StuNameID;
                        detailedcostView.Amountofmoney = 0;
                    }
                  
                    listdetailedc.Add(detailedcostView);
                }
            }
            List<DetailedcostView> listdetailedcs = new List<DetailedcostView>();                                
            foreach (var item in gotosches)
            {
                foreach (var item1 in listdetailedc)
                {
                    if (item.id==item1.StagesID)
                    {
                        if (item1.Amountofmoney< item.Price)
                        {
                            DetailedcostView detailedcostView =this.FineDetail(ClassID, item1.Stidentid);
                            detailedcostView.Amountofmoney = item1.Amountofmoney;
                            detailedcostView.CurrentStageID = item1.CurrentStageID;
                            detailedcostView.ShouldJiao = item.Price;
                            detailedcostView.Surplus = item.Price - item1.Amountofmoney;
                            detailedcostView.StagesID = item1.StagesID;
                            listdetailedcs.Add(detailedcostView);
                        }
                    }
                   
                }
            }
            return listdetailedcs;
        }
        /// <summary>
        /// 更改商品状态
        /// </summary>
        /// <param name="id">商品id</param>
        /// <param name="Isdele">正常或者禁用</param>
        /// <returns></returns>
        public bool PaymentcommodityIsdele(int id, bool Isdele)
        {
            bool bol = true;
            try
            {
                var cost = costitemsBusiness.GetEntity(id);
                var costlist = costitemsBusiness.GetList().Where(a => a.Grand_id == cost.Grand_id && a.Name == cost.Name && a.Rategory == cost.Rategory && a.IsDelete == false).ToList();
                foreach (var item in costlist)
                {
                    item.IsDelete = true;
                }
                costitemsBusiness.Update(costlist);
                cost.IsDelete = Isdele;
                costitemsBusiness.Update(cost);
                BusHelper.WriteSysLog("修改学员学费及其它商品表", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {
                bol = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return bol;

        }

        public class CostitemViews
        {
            public int id { get; set; }
            public string studentid { get; set; }
            public string name { get; set; }
            public string IDnumber { get; set; }
            public decimal Amountofmoney { get; set; }
            public DateTime AddDate { get; set; }
            public bool? Passornot { get; set; }
            //单号
            public string OddNumbers { get; set; }
        }
        /// <summary>
        /// 获取待入账数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="StudentID">学号</param>
        /// <param name="Name">姓名</param>
        /// <param name="IsaDopt">状态</param>
        /// <param name="OddNumbers">单号</param>
        /// <returns></returns>
        public object Expenseentry(int page, int limit,string StudentID, string Name, string IsaDopt, string OddNumbers)
        {
            List<CostitemViews> costlist = new List<CostitemViews>();
            foreach (var item in PaymentverificationBusiness.GetList())
            {
                CostitemViews costitemViews = new CostitemViews();
                costitemViews.id = item.id;
                costitemViews.Passornot = item.Passornot;
                costitemViews.OddNumbers = item.OddNumbers;
                var pay = PayviewPaymentverBusiness.GetList().Where(z => z.Paymentver == item.id).ToList();
                foreach (var item1 in pay)
                {
                    var x = PayviewBusiness.GetEntity(item1.Payviewid);
                    costitemViews.name = studentInformationBusiness.GetEntity(x.StudenID).Name;
                    costitemViews.IDnumber= studentInformationBusiness.GetEntity(x.StudenID).identitydocument;
                    costitemViews.studentid = x.StudenID;
                    costitemViews.Amountofmoney= costitemViews.Amountofmoney+ (decimal)x.Amountofmoney;
                    costitemViews.AddDate =(DateTime) x.AddDate;
                }
                costlist.Add(costitemViews);
            }
            if (!string.IsNullOrEmpty(StudentID))
            {
                costlist = costlist.Where(a => a.studentid.Contains(StudentID)).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                costlist = costlist.Where(a => a.name.Contains(Name)).ToList();
            }
            if (!string.IsNullOrEmpty(IsaDopt))
            {
                if (IsaDopt=="null")
                {
                    costlist = costlist.Where(a => a.Passornot == null).ToList();
                }
                else { costlist = costlist.Where(a => a.Passornot == Convert.ToBoolean(IsaDopt)).ToList(); }
            }
            if (!string.IsNullOrEmpty(OddNumbers))
            {
                costlist = costlist.Where(a => a.OddNumbers == OddNumbers).ToList();
            }
            var dataList = costlist.OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = costlist.Count,
                data = dataList
            };
            return data;
        }
        /// <summary>
        /// 审批页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<vierprice> FienPricesa(int id)
        {
            List<object> students = new List<object>();
            var pay = PayviewPaymentverBusiness.GetList().Where(z => z.Paymentver == id).ToList();
            List<Payview> st = new List<Payview>();
            foreach (var item in pay)
            {
                var st1 = PayviewBusiness.GetEntity(item.Payviewid);
                st.Add(st1);
                students.Add(Convert.ToDateTime(st1.AddDate).ToLongDateString().ToString());
            }

            var x = students.Distinct().ToList();
            List<vierprice> listvier = new List<vierprice>();
            // string date = Convert.ToDateTime(item.AddDate).ToLongDateString().ToString();
            foreach (var item1 in x)
            {
                vierprice vierprices = new vierprice();
                vierprices.Date = item1.ToString();
                List<vierprice> view = new List<vierprice>();
                foreach (var item in st)
                {

                    string date = Convert.ToDateTime(item.AddDate).ToLongDateString().ToString();
                    if (item1.ToString() == date)
                    {
                        var costit = costitemsBusiness.GetEntity(item.Costitemsid);
                        vierprice stivier = new vierprice();
                        stivier.Amountofmoney = (decimal)item.Amountofmoney;
                        stivier.CostitemName = costit.Name;
                        stivier.GrandName = costit.Grand_id != null ? geand.GetEntity(costit.Grand_id).GrandName : "";
                        stivier.Rategory = costitemssX.GetEntity(costit.Rategory).Name;
                        view.Add(stivier);
                    }
                }
                vierprices.Chicked = view;
                listvier.Add(vierprices);
            }
            return listvier;
        }
        /// <summary>
        /// 审核入账是否成功
        /// </summary>
        /// <param name="id">核对缴费是否成功编号</param>
        /// <param name="whether">是否入账</param>
        /// <param name="OddNumbers">单号</param>
        /// <returns></returns>
        public AjaxResult Tuitionentry(int id,bool whether,string OddNumbers)
        {
            AjaxResult retus = null;
            try
            {
                retus = new SuccessResult();
                retus.Success = true;
             
                if (whether)
                {
                 
                    var x = PaymentverificationBusiness.GetEntity(id);
                    x.Passornot = whether;
                    x.OddNumbers = OddNumbers;
                    x.AddDate = DateTime.Now;
                    PaymentverificationBusiness.Update(x);
                    var pay = PayviewPaymentverBusiness.GetList().Where(a => a.Paymentver == id).ToList();
                    List<StudentFeeRecord> studentFeeRecordslist = new List<StudentFeeRecord>();
                    foreach (var item in pay)
                    {
                        var view = PayviewBusiness.GetEntity(item.Payviewid);
                        StudentFeeRecord studentFeeRecord = new StudentFeeRecord();
                        studentFeeRecord.AddDate = view.AddDate;
                        studentFeeRecord.Amountofmoney = view.Amountofmoney;
                        studentFeeRecord.Costitemsid = view.Costitemsid;
                        studentFeeRecord.FinanceModelid = view.FinanceModelid;
                        studentFeeRecord.IsDelete = view.IsDelete;
                        studentFeeRecord.Remarks = view.Remarks;
                        studentFeeRecord.StudenID = view.StudenID;
                        studentFeeRecordslist.Add(studentFeeRecord);
                    }
                    studentfee.Insert(studentFeeRecordslist);
                    if (studentFeeRecordslist.Count<2)
                    {
                      if(costitemssX.GetEntity( costitemsBusiness.GetEntity(studentFeeRecordslist[0].Costitemsid).Rategory).Name== "自考本科费用")
                        {
                            var FeeRecords = studentFeeRecordslist[0];
                            Enrollment enrollment = new Enrollment();
                            enrollment.StudentNumber = FeeRecords.StudenID;
                            enrollment.IsDelete = false;
                            enrollmentBusiness.AddEnro(enrollment, FeeRecords.Costitemsid);
                        }
                     
                    }
                  
                    retus.Msg = "入账成功";
                }
                else
                {
                    var x = PaymentverificationBusiness.GetEntity(id);
                    x.Passornot = whether;
                    x.AddDate = DateTime.Now;
                    x.OddNumbers = OddNumbers;
                    PaymentverificationBusiness.Update(x);
                    retus.Msg = "作废成功";
                }
             
                BusHelper.WriteSysLog("审核缴费数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }
        //预入费业务类
        BaseBusiness<Preentryfee> Preentryfeebusenn = new BaseBusiness<Preentryfee>();
        //备案业务类
        StudentDataKeepAndRecordBusiness studentDataKeepAndRecordBusiness = new StudentDataKeepAndRecordBusiness();
        /// <summary>
        /// 预入费缴纳
        /// </summary>
        /// <param name="preentryfee">数据对象</param>
        /// <returns></returns>
        public object PaytheadvancefeeAdd(Preentryfee preentryfee)
        {
           AjaxResult retus = null;
            try
            {
                preentryfee.AddDate = DateTime.Now;
                preentryfee.IsDit = false;
                Preentryfeebusenn.Insert(preentryfee);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "缴费成功";
                BusHelper.WriteSysLog("审核缴费数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }

        public class Preeviews
        {
            public int id { get; set; }
            public string StuName { get; set; }
            public string Stuphone { get; set; }
            public string StuSex { get; set; }
            public string empName { get; set; }
            public string StuEntering { get; set; }
            public bool? Refundornot { get; set; }
            public decimal Amountofmoney { get; set; }
            public string identitydocument { get; set; }
            public string ClassNumber { get; set; }
        }
        public object PreentryfeeDates(int page, int limit)
        {
            var preenlist = Preentryfeebusenn.GetList();
            List<Preeviews> list = new List<Preeviews>();
            foreach (var item in preenlist)
            {
              var Keep=  studentDataKeepAndRecordBusiness.GetSudentDataAll().Where(a=>a.Id==item.keeponrecordid).FirstOrDefault();
                var x = new Preeviews()
                {
                    id= item.id,
                    StuName= Keep.StuName,
                    Stuphone=Keep.Stuphone,
                    StuSex= Keep.StuSex,
                    empName=Keep.empName,
                    StuEntering=  Keep.StuEntering,
                    Refundornot= item.Refundornot,
                    Amountofmoney= item.Amountofmoney,
                    identitydocument= item.identitydocument,
                    ClassNumber= classschedu.GetEntity(item.ClassID).ClassNumber
                };
                list.Add(x);
            }
            var dataList = list.OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = dataList
            };
            return data;
       

        }
    }
}
