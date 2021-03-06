﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.Entity;
namespace SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness
{
    using System.IO;
    using NPOI.SS.UserModel;
    using NPOI.HSSF.UserModel;
    using NPOI.XSSF.UserModel;
    using SiliconValley.InformationSystem.Entity.ViewEntity.SalaryView;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.AttendanceAnormalyBusiness;
    public class AttendanceInfoManage : BaseBusiness<AttendanceInfo>
    {
        RedisCache rc = new RedisCache();
        /// <summary>
        /// 将员工考勤表数据存储到redis服务器中
        /// </summary>
        /// <returns></returns>
        public List<AttendanceInfo> GetADInfoData()
        {
            rc.RemoveCache("InRedisATDData");
            List<AttendanceInfo> atdinfolist = new List<AttendanceInfo>();
            if (atdinfolist == null || atdinfolist.Count == 0)
            {
                atdinfolist = this.GetList();
                rc.SetCache("InRedisATDData", atdinfolist);
            }
            atdinfolist = rc.GetCache<List<AttendanceInfo>>("InRedisATDData");
            return atdinfolist;
        }

        /// <summary>
        /// 编辑考勤表禁用员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool EditEmpStateToAds(string empid, string time)
        {
            bool result = false;
            try
            {
                var ymtime = DateTime.Parse(time);
                var ads = this.GetADInfoData().Where(e => e.EmployeeId == empid && DateTime.Parse(e.YearAndMonth.ToString()).Year == ymtime.Year && DateTime.Parse(e.YearAndMonth.ToString()).Month == ymtime.Month).FirstOrDefault();
                ads.IsDel = true;
                this.Update(ads);
                rc.RemoveCache("InRedisATDData");
                result = true;
                BusHelper.WriteSysLog("考勤表禁用员工成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);

            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return result;

        }

        #region 从excel导入考勤信息到系统

        ///// <summary>
        ///// 拿到考勤excel表中的第一个单元
        ///// </summary>
        ///// <param name="stream"></param>
        ///// <param name="contentType"></param>
        ///// <returns></returns>
        //public AjaxResult ImportDataFormExcel(Stream stream, string contentType)
        //{

        //    IWorkbook workbook = null;

        //    if (contentType == "application/vnd.ms-excel")
        //    {
        //        workbook = new HSSFWorkbook(stream);
        //    }

        //    if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //    {
        //        workbook = new XSSFWorkbook(stream);
        //    }

        //    ISheet sheet = workbook.GetSheetAt(0);
        //    var result = ExcelImportAtdSql(sheet);
        //    stream.Close();
        //    stream.Dispose();
        //    workbook.Close();

        //    return result;
        //}

        ///// <summary>
        ///// 将导过来的excel数据存入excel视图类（单元一的考勤数据）
        ///// </summary>
        ///// <param name="sheet"></param>
        ///// <returns></returns>
        //public List<MyAtdDataFromExcelView> CreateExcelData(ISheet sheet)
        //{
        //    List<MyAtdDataFromExcelView> result = new List<MyAtdDataFromExcelView>();
        //    int num = 3;
        //    AjaxResult ajaxresult = new AjaxResult();
        //    try
        //    {
        //        //获取第一行数据（年月份）
        //        //  string time = sheet.GetRow(1).Cells[1].StringCellValue;
        //        string time1 = sheet.GetRow(0).Cells[0].StringCellValue;
        //        string[] str = time1.Split('至');
        //        // string[] strtime = str[1];
        //        var time = str[1];
        //        //获取第二行数据（应到勤天数）
        //        //   double DeserveToRegularDays = sheet.GetRow(2).Cells[1].NumericCellValue;
        //        while (true)
        //        {
        //            MyAtdDataFromExcelView matd = new MyAtdDataFromExcelView();
        //            num++;
        //            //获取第三行"姓名"列的数据
        //            var getrow = sheet.GetRow(num);
        //            if (getrow == null)
        //            {
        //                break;
        //            }
        //            #region 循环拿值
        //            //姓名[0]
        //            string name = getrow.GetCell(0).StringCellValue;
        //            //工号(钉钉号)[3]
        //            string ddid = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(3))) ? null : getrow.GetCell(3).ToString();
        //            //到勤天数[5]
        //            string workeddays = getrow.GetCell(5).ToString();

        //            //迟到次数[8]
        //            string tardyNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(8))) ? null : getrow.GetCell(8).ToString();
        //            //早退次数[13]
        //            string leaveEarlyNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(13))) ? null : getrow.GetCell(13).ToString();
        //            //上班缺卡次数[15]
        //            string workAbsentNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(15))) ? null : getrow.GetCell(15).ToString();
        //            //下班缺卡次数[16]
        //            string offDutyAbsentNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(16))) ? null : getrow.GetCell(16).ToString();
        //            //旷工天数[17]
        //            string AbsenteeismDays = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(17))) ? null : getrow.GetCell(17).ToString();

        //            //请假天数[20](事假)
        //            string leaveddays = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(20))) ? null : getrow.GetCell(20).ToString();

        //            //请假记录
        //            string leaveRecord = "";
        //            //迟到记录
        //            string tardyRecord = "";
        //            //早退记录
        //            string leaveEarlyRecord = "";
        //            //上班缺卡记录
        //            string workAbsentRecord = "";
        //            //下班缺卡记录
        //            string OffDutyAbsentRecord = "";
        //            //加班记录
        //            string OvertTimeRecord = "";
        //            //调休记录
        //            string DaysoffRecord = "";
        //            //旷工记录
        //            string AbsenteeismRecord = "";
        //            #endregion

        //            int cells = 25;
        //            while (true)
        //            {//(循环拿到日期列数据)
        //                var getcell = getrow.GetCell(cells);
        //                if (getcell == null)
        //                {
        //                    break;
        //                }
        //                var titlerow = sheet.GetRow(2);//表头行（日期）
        //                var title = cells - 24;

        //                if (getcell.StringCellValue.Contains("迟到"))
        //                {
        //                    tardyRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("早退"))
        //                {
        //                    leaveEarlyRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("上班缺卡"))
        //                {
        //                    workAbsentRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("下班缺卡"))
        //                {
        //                    OffDutyAbsentRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("事假"))
        //                {
        //                    leaveRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("加班"))
        //                {
        //                    OvertTimeRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("调休"))
        //                {
        //                    DaysoffRecord += title + "号," + getcell.StringCellValue + ";";
        //                }
        //                else if (getcell.StringCellValue.Contains("旷工"))
        //                {
        //                    AbsenteeismRecord += title + "号," + getcell.StringCellValue + ";";
        //                }

        //                //迟到扣款
        //                // string tardyWithhold = "";
        //                //早退扣款
        //                //
        //                cells++;

        //            }
        //            // string leaveddays = getrow.GetCell(3) == null ? null : getrow.GetCell(3).NumericCellValue.ToString();
        //            // string tardyWithhold = getrow.GetCell(10) == null ? null : getrow.GetCell(10).NumericCellValue.ToString();
        //            // string leaveWithhold = getrow.GetCell(13) == null ? null : getrow.GetCell(13).NumericCellValue.ToString();
        //            // string remark = getrow.GetCell(14) == null ? null : getrow.GetCell(14).StringCellValue;

        //            matd.YearAndMonth = Convert.ToDateTime(time);
        //            matd.EmpName = name;
        //            matd.EmpDDid = Convert.ToInt32(ddid);
        //            matd.ToRegularDays = Convert.ToInt32(workeddays);
        //            matd.LeaveDays = Convert.ToDecimal(leaveddays);
        //            matd.LeaveRecord = leaveRecord;
        //            matd.WorkAbsentNum = Convert.ToInt32(workAbsentNum);
        //            matd.WorkAbsentRecord = workAbsentRecord;
        //            matd.OffDutyAbsentNum = Convert.ToInt32(offDutyAbsentNum);
        //            matd.OffDutyAbsentRecord = OffDutyAbsentRecord;
        //            matd.TardyNum = Convert.ToInt32(tardyNum);
        //            matd.TardyRecord = tardyRecord;
        //            matd.LeaveEarlyNum = Convert.ToInt32(leaveEarlyNum);
        //            matd.LeaveEarlyRecord = leaveEarlyRecord;
        //            matd.OvertTimeRecord = OvertTimeRecord;
        //            matd.DaysoffRecord = DaysoffRecord;
        //            matd.AbsenteeismRecord = AbsenteeismRecord;
        //            matd.AbsenteeismDays = Convert.ToDecimal(AbsenteeismDays);
        //            //matd.TardyWithhold =tardyWithhold==null?matd.TardyWithhold=null: Convert.ToInt32(tardyWithhold);

        //            //matd.LeaveWithhold =leaveWithhold==null?matd.LeaveWithhold=null: Convert.ToInt32(leaveWithhold);
        //            //matd.Remark = remark;

        //            result.Add(matd);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        result = null;
        //    }
        //    return result;

        //}

        ///// <summary>
        ///// 将excel数据类的数据存入到数据库的考勤表中
        ///// </summary>
        ///// <returns></returns>
        //public AjaxResult ExcelImportAtdSql(ISheet sheet)
        //{
        //    EmployeesInfoManage empmanage = new EmployeesInfoManage();
        //    var ajaxresult = new AjaxResult();
        //    List<AttendanceInfoErrorDataView> attdatalist = new List<AttendanceInfoErrorDataView>();
        //    try
        //    {
        //        var mateviewlist = CreateExcelData(sheet);

        //        //获取第一个人的出勤天数，将它设为默认的应出勤天数
        //        //  var days = mateviewlist.FirstOrDefault().ToRegularDays;
        //        foreach (var item in mateviewlist)
        //        {
        //            AttendanceInfo atd = new AttendanceInfo();
        //            AttendanceInfoErrorDataView attview = new AttendanceInfoErrorDataView();
        //            if (!empmanage.DDidIsExist(item.EmpDDid))
        //            {//判断员工钉钉号是否为空
        //                attview.empname = item.EmpName;
        //                attview.errorExplain = "工号为空！";
        //                attdatalist.Add(attview);
        //            }
        //            else
        //            {

        //                var emp = empmanage.GetEmpByDDid(item.EmpDDid);

        //                atd.EmployeeId = emp.EmployeeId;
        //                atd.YearAndMonth = item.YearAndMonth;
        //                atd.DeserveToRegularDays = GetDeserveToRegularDays(emp.EmployeeId, (DateTime)item.YearAndMonth);
        //                atd.ToRegularDays = item.ToRegularDays;

        //                atd.LeaveRecord = item.LeaveRecord;
        //                atd.LeaveDays = item.LeaveDays;
        //                atd.WorkAbsentNum = item.WorkAbsentNum;
        //                atd.WorkAbsentRecord = item.WorkAbsentRecord;
        //                atd.OffDutyAbsentNum = item.OffDutyAbsentNum;
        //                atd.OffDutyAbsentRecord = item.OffDutyAbsentRecord;
        //                atd.TardyNum = item.TardyNum;
        //                atd.TardyRecord = item.TardyRecord;
        //                atd.LeaveEarlyNum = item.LeaveEarlyNum;
        //                atd.LeaveEarlyRecord = item.LeaveEarlyRecord;

        //                atd.OvertTimeRecord = item.OvertTimeRecord;
        //                atd.OvertTimeDuration = item.OvertTimeDuration;
        //                atd.DaysoffRecord = item.DaysoffRecord;
        //                atd.DaysoffDuration = item.DaysoffDuration;
        //                atd.AbsenteeismRecord = item.AbsenteeismRecord;
        //                atd.AbsenteeismDays = item.AbsenteeismDays;

        //                //atd.TardyWithhold = item.TardyWithhold;
        //                //atd.LeaveWithhold = item.LeaveWithhold;

        //                atd.Remark = item.Remark;
        //                atd.IsDel = false;
        //                atd.IsApproval = false;
        //                this.Insert(atd);
        //                rc.RemoveCache("InRedisATDData");
        //                //AddAbsentAnormaly(atd.WorkAbsentRecord, atd.EmployeeId, (DateTime)atd.YearAndMonth);//上班缺卡异常添加
        //                //AddAbsentAnormaly(atd.OffDutyAbsentRecord, atd.EmployeeId, (DateTime)atd.YearAndMonth);//下班缺卡异常添加
        //            }
        //        }
        //        if (mateviewlist.Count() - attdatalist.Count() == mateviewlist.Count())
        //        {//说明没有出错数据，导入的数据全部添加成功
        //            ajaxresult.Success = true;
        //            ajaxresult.ErrorCode = 100;
        //            ajaxresult.Msg = mateviewlist.Count().ToString();
        //            ajaxresult.Data = attdatalist;
        //        }
        //        else
        //        {//说明有出错数据，导入的数据条数就是导入的数据总数-错误数据总数
        //            ajaxresult.Success = true;
        //            ajaxresult.ErrorCode = 200;
        //            ajaxresult.Msg = (mateviewlist.Count() - attdatalist.Count()).ToString();
        //            ajaxresult.Data = attdatalist;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ajaxresult.Success = false;
        //        ajaxresult.ErrorCode = 500;
        //        ajaxresult.Msg = ex.Message;
        //        ajaxresult.Data = "0";
        //    }
        //    return ajaxresult;
        //}

        // <summary>

        /// <summary>
        /// 拿到考勤excel表中的第一个单元
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public AjaxResult ImportDataFormExcel(Stream stream, string contentType)
        {
            IWorkbook workbook = null;

            if (contentType == "application/vnd.ms-excel")
            {
                workbook = new HSSFWorkbook(stream);
            }

            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                workbook = new XSSFWorkbook(stream);
            }

            ISheet sheet = workbook.GetSheetAt(0);
            var result = ExcelImportAtdSql(sheet);
            stream.Close();
            stream.Dispose();
            workbook.Close();

            return result;
        }
 
        /// <summary>
        /// 将excel数据类的数据存入到数据库的考勤表中
        /// </summary>
        /// <returns></returns>
        public AjaxResult ExcelImportAtdSql(ISheet sheet)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var ajaxresult = new AjaxResult();
            int num = 2;
            List<AttendanceInfoErrorDataView> attdatalist = new List<AttendanceInfoErrorDataView>();
            try
            {
                    //获取第二行数据（年月份）
                    string time1 = sheet.GetRow(1).Cells[0].StringCellValue;
                    string[] str = time1.Split('-');
                    var time = str[1];

                    while (true)
                    {
                        num++;
                        var getrow = sheet.GetRow(num);
                        if (getrow == null)
                        {
                            break;
                        }
                        #region 循环拿值
                        //工号(钉钉号)[0]
                        string ddid = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(0))) ? null : getrow.GetCell(0).ToString();
                        //姓名[1]
                        string name = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(1))) ? null : getrow.GetCell(1).ToString();
                        //应到勤天数[2]
                        string deserveToRegularDays = getrow.GetCell(2).ToString();
                        //到勤天数[3]
                        string workeddays = getrow.GetCell(3).ToString();
                        //请假天数[4](事假)
                        string leaveddays = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(4))) ? null : getrow.GetCell(4).ToString();
                        //请假记录[5]
                        string leaveRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(5))) ? null : getrow.GetCell(5).ToString();
                        //上班缺卡次数[6]
                        string workAbsentNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(6))) ? null : getrow.GetCell(6).ToString();
                        //上班缺卡记录[7]
                        string workAbsentRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(7))) ? null : getrow.GetCell(7).ToString();

                        //中午缺卡次数[8]
                        string noonAbsentNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(8))) ? null : getrow.GetCell(8).ToString();
                        //中午缺卡记录[9]
                        string noonAbsentRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(9))) ? null : getrow.GetCell(9).ToString();

                        //下班缺卡次数[10]
                        string offDutyAbsentNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(10))) ? null : getrow.GetCell(10).ToString();
                        //下班缺卡记录[11]
                        string OffDutyAbsentRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(11))) ? null : getrow.GetCell(11).ToString();

                        //迟到次数[12]
                        string tardyNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(12))) ? null : getrow.GetCell(12).ToString();
                        //迟到记录[13]
                        string tardyRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(13))) ? null : getrow.GetCell(13).ToString();

                        //早退次数[14]
                        string leaveEarlyNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(14))) ? null : getrow.GetCell(14).ToString();
                        //早退记录[15]
                        string leaveEarlyRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(15))) ? null : getrow.GetCell(15).ToString();

                        //加班时长[16]
                        string OvertTimeDuration = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(16))) ? null : getrow.GetCell(16).ToString();
                        //加班记录[17]
                        string OvertTimeRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(17))) ? null : getrow.GetCell(17).ToString();

                        //调休时长[18]
                        string DaysoffDuration = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(18))) ? null : getrow.GetCell(18).ToString();
                        //调休记录[19]
                        string DaysoffRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(19))) ? null : getrow.GetCell(19).ToString();

                        //旷工天数[20]
                        string absenteeismDays = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(20))) ? null : getrow.GetCell(20).ToString();
                        //旷工天数[21]
                        string absenteeismRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(21))) ? null : getrow.GetCell(21).ToString();
                        //外出次数[22]
                        string GoOutNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(22))) ? null : getrow.GetCell(22).ToString();
                        //外出记录[23]
                        string GoOutRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(23))) ? null : getrow.GetCell(23).ToString();
                        //出差天数[24]
                        string EvectionNum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(24))) ? null : getrow.GetCell(24).ToString();
                        //出差记录[25]
                        string EvectionRecord = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(25))) ? null : getrow.GetCell(25).ToString();

                        //出差记录[26]
                        string Remark = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(26))) ? null : getrow.GetCell(26).ToString();


                        #endregion

                        string year_month = time;
                 
                    AttendanceInfo atd = new AttendanceInfo();
                    AttendanceInfoErrorDataView attview = new AttendanceInfoErrorDataView();
                    if (!empmanage.DDidIsExist(Convert.ToInt32(ddid)))
                    {//判断员工钉钉号是否为空
                        attview.empname = name;
                        attview.errorExplain = "工号为空！";
                        attdatalist.Add(attview);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(deserveToRegularDays))
                        {
                            attview.empname = name;
                            attview.errorExplain = "应出勤天数为空！";
                            attdatalist.Add(attview);
                        }
                        else
                        {
                             if (string.IsNullOrEmpty(workeddays))
                            {
                                attview.empname = name;
                                attview.errorExplain = "实际出勤天数为空！";
                                attdatalist.Add(attview);
                            }
                            else
                            {

                                var emp = empmanage.GetEmpByDDid(Convert.ToInt32(ddid));

                                atd.EmployeeId = emp.EmployeeId;
                                atd.YearAndMonth = Convert.ToDateTime(year_month);
                                atd.DeserveToRegularDays = Convert.ToDecimal(deserveToRegularDays);
                                atd.ToRegularDays = Convert.ToDecimal(workeddays);

                                atd.LeaveDays = Convert.ToDecimal(leaveddays);
                                atd.LeaveRecord = leaveRecord;

                                atd.WorkAbsentNum = Convert.ToInt32(workAbsentNum);
                                atd.WorkAbsentRecord = workAbsentRecord;
                                atd.NoonAbsentNum = Convert.ToInt32(noonAbsentNum);
                                atd.NoonAbsentRecord = noonAbsentRecord;
                                atd.OffDutyAbsentNum = Convert.ToInt32(offDutyAbsentNum);
                                atd.OffDutyAbsentRecord = OffDutyAbsentRecord;

                                atd.TardyNum = Convert.ToInt32(tardyNum);
                                atd.TardyRecord = tardyRecord;
                                atd.LeaveEarlyNum = Convert.ToInt32(leaveEarlyNum);
                                atd.LeaveEarlyRecord = leaveEarlyRecord;

                                atd.OvertTimeDuration = Convert.ToDecimal(OvertTimeDuration);
                                atd.OvertTimeRecord = OvertTimeRecord;
                                atd.DaysoffDuration = Convert.ToDecimal(DaysoffDuration);
                                atd.DaysoffRecord = DaysoffRecord;
                                atd.AbsenteeismDays = Convert.ToDecimal(absenteeismDays);
                                atd.AbsenteeismRecord = absenteeismRecord;
                                atd.GoOutNum = Convert.ToInt32(GoOutNum);
                                atd.GoOutRecord = GoOutRecord;
                                atd.EvectionNum = Convert.ToDecimal(EvectionNum);
                                atd.EvectionRecord = EvectionRecord;

                                //atd.TardyWithhold = item.TardyWithhold;
                                //atd.LeaveWithhold = item.LeaveWithhold;
                                atd.AbsenteeismWithhold = GetAbsenteeismWithhold(emp.EmployeeId, Convert.ToDouble(atd.WorkAbsentNum + atd.NoonAbsentNum + atd.OffDutyAbsentNum));

                                atd.Remark = Remark;
                                atd.IsDel = false;
                                atd.IsApproval = false;
                                this.Insert(atd);
                                rc.RemoveCache("InRedisATDData");
                            }
                        }
                       
                    }
                }
                int exceldatasum = num - 3;
                if (exceldatasum - attdatalist.Count() == exceldatasum)
                {//说明没有出错数据，导入的数据全部添加成功
                    ajaxresult.Success = true;
                    ajaxresult.ErrorCode = 100;
                    ajaxresult.Msg = exceldatasum.ToString();
                    ajaxresult.Data = attdatalist;
                }
                else
                {//说明有出错数据，导入的数据条数就是导入的数据总数-错误数据总数
                    ajaxresult.Success = true;
                    ajaxresult.ErrorCode = 200;
                    ajaxresult.Msg = (exceldatasum - attdatalist.Count()).ToString();
                    ajaxresult.Data = attdatalist;
                }
            }
            catch (Exception ex)
            {
                ajaxresult.Success = false;
                ajaxresult.ErrorCode = 500;
                ajaxresult.Msg = ex.Message;
                ajaxresult.Data = "0";
            }
            return ajaxresult;
        }
        #endregion


        /// <summary>
        /// 计算应出勤天数
        /// </summary>
        /// <param name="empid">员工编号</param>
        /// <param name="year_month">当前年月份</param>
        /// <returns></returns>
        public decimal GetDeserveToRegularDays(string empid, DateTime year_month)
        {
            int result = 0;
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var days = DateTime.DaysInMonth(year_month.Year, year_month.Month);//获取该月份总天数

            int week = 0;//正常情况下员工的休息时间
            var month = year_month.Month;
            //招生旺季时员工单休
            if (month == 6 || month == 7 || month == 8 || month == 9)
            {
                for (int i = 1; i < DateTime.DaysInMonth(year_month.Year, year_month.Month) + 1; i++)
                {
                    var theday = year_month.AddDays(i - year_month.Day).DayOfWeek.ToString();

                    if (theday == "Sunday")
                    {
                        week += 1;
                    }
                }
            }
            else//非招生旺季时员工双休
            {
                for (int i = 1; i < DateTime.DaysInMonth(year_month.Year, year_month.Month) + 1; i++)
                {
                    var theday = year_month.AddDays(i - year_month.Day).DayOfWeek.ToString();

                    if (theday == "Saturday" || theday == "Sunday")
                    {
                        week += 1;
                    }
                }
            }

            var dept = empmanage.GetDeptByEmpid(empid).DeptName;
            var position = empmanage.GetPositionByEmpid(empid).PositionName;
            if (dept == "后勤部" && (position != "后勤主任" || position != "后勤专员" || position != "水电工"))
            {
                result = days - 3;//后勤部不参与打卡的员工月休3天
            }
            else if (dept == "市场部")
            {
                result = days;//市场部默认出勤天数为满月
            }
            else
            {
                result = days - week;
            }
            return result;
        }

        /// <summary>
        /// 计算旷工扣费
        /// </summary>
        /// <param name="empid">员工编号</param>
        /// <param name="absenteeismDays">旷工天数</param>
        /// <returns></returns>
        public decimal GetAbsenteeismWithhold(string empid, double absenteeismDays)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var result = 0;
            var emprank = empmanage.IsGeneralStarffOrSuperior(empid);//true为普通员工
            if (absenteeismDays == 0.5)
            {
                if (emprank)
                {
                    result = 50;
                }
                else
                {
                    result = 100;
                }
            }
            else if (absenteeismDays == 1)
            {
                if (emprank)
                {
                    result = 100;
                }
                else
                {
                    result = 200;
                }
            }
            else if (absenteeismDays > 1 && absenteeismDays < 3)
            {
                if (emprank)
                {
                    result = 300;
                }
                else
                {
                    result = 600;
                }
            }
            else
            {
                result = 1000;
            }
            return result;
        }

        //迟到扣款
        public decimal TardyWithhold(string tardyRecord)
        {
            var result = 0;
            return result;
        }

        /// <summary>
        /// 上/下班缺卡的异常添加
        /// </summary>
        /// <param name="AbsentRecord">上/下班缺卡记录</param>
        /// <param name="empid">员工编号</param>
        /// <param name="year_month">年月份</param>
        /// <returns></returns>
        public bool AddAbsentAnormaly(string AbsentRecord, string empid, DateTime year_month)
        {
            bool result = false;
            AttendanceAnormalyManage aamanage = new AttendanceAnormalyManage();
            AttendanceAnormalyDetailsManage aadetailmanage = new AttendanceAnormalyDetailsManage();
            try
            {
                if (!string.IsNullOrEmpty(AbsentRecord))
                {
                    string[] str = AbsentRecord.Split(';');
                    foreach (var item in str)
                    {
                        string[] newstr = item.Split(',');
                        AttendanceAnormalyDetails aadetail = new AttendanceAnormalyDetails();
                        aadetail.AnormalyDay = newstr[0];//异常具体日期
                        aadetail.AnormalyTypeId = aamanage.GetaaidByaaname(newstr[1]);//异常类型
                        aadetail.EmployeeId = empid;
                        aadetail.YearAndMonth = year_month;
                        aadetail.IsDel = false;
                        aadetailmanage.Insert(aadetail);
                        result = true;
                    }
                }

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 用于考勤异常表中将'是否属实'属性改为无效时把该员工考勤的缺卡次数也进行改变
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="year_month"></param>
        /// <param name="aatypeid"></param>
        /// <returns></returns>
        public AjaxResult AbsentNumChange(string empid, DateTime year_month, int aatypeid)
        {
            AttendanceAnormalyManage aamanage = new AttendanceAnormalyManage();
            var ajaxresult = new AjaxResult();
            try
            {
                var atd = this.GetList().Where(s => MatchYear_month((DateTime)s.YearAndMonth, year_month) == true).FirstOrDefault();
                if (aamanage.GetaanameByaaid(aatypeid) == "上班缺卡")
                {
                    atd.WorkAbsentNum = atd.WorkAbsentNum - 1;
                }
                else
                {
                    atd.OffDutyAbsentNum = atd.OffDutyAbsentNum - 1;
                }
                this.Update(atd);
                ajaxresult = this.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = this.Error(ex.Message);
            }
            return ajaxresult;
        }

        /// <summary>
        /// 匹配两个日期是否同年同月（true为同年同月）
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public bool MatchYear_month(DateTime date1, DateTime date2)
        {
            var result = false;
            var year1 = date1.Year;
            var month1 = date1.Month;
            var year2 = date2.Year;
            var month2 = date2.Month;
            if (year1 == year2 && month1 == month2)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
