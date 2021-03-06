﻿using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
   public class ConversionToViewBusiness
    {

        private StaffAccdationBusiness dbstaffacc;
        private ProScheduleForTrainees dbprotrainees;
        private AccdationinformationBusiness dbacc;
        private DormInformationBusiness dbdorm;
        private ProStudentInformationBusiness dbproStudentInformation;
      
        private ProStudentInformationBusiness dbproStudentInformationBusiness;
        /// <summary>
        /// 员工实体对象转化为视图对象带房间编号的 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<RoomArrangeEmpinfoView> EmpinfoToRoomArrangeEmpinfoView(List<EmployeesInfo> data,bool live) {
            List<RoomArrangeEmpinfoView> ll = new List<RoomArrangeEmpinfoView>();
            dbdorm = new DormInformationBusiness();
            foreach (var item in data)
            {
                RoomArrangeEmpinfoView roomArrangeEmpinfo = new RoomArrangeEmpinfoView();
                PositionManage positionManage = new PositionManage();
                Position position = positionManage.GetEntity(item.PositionId);
                roomArrangeEmpinfo.EmployeeId = item.EmployeeId;
                roomArrangeEmpinfo.EmpName = item.EmpName;
                roomArrangeEmpinfo.Phone = item.Phone;
                DepartmentManage departmentManage = new DepartmentManage();
                Department department = departmentManage.GetEntity(position.DeptId);
                roomArrangeEmpinfo.DeptName = department.DeptName;
                roomArrangeEmpinfo.Sex = item.Sex;
                roomArrangeEmpinfo.datatype = "staff";
                if (live)
                {
                    dbstaffacc = new StaffAccdationBusiness();
                    var querystaffacc = dbstaffacc.GetStaffAccdationsByEmpinfoID(item.EmployeeId);
                    if (querystaffacc != null)
                    {
                        roomArrangeEmpinfo.DormID = querystaffacc.DormId;
                        DormInformation querydorm = dbdorm.GetDormByDorminfoID(querystaffacc.DormId);
                        roomArrangeEmpinfo.DormName = querydorm.DormInfoName;
                        ll.Add(roomArrangeEmpinfo);
                    }
                }
                else
                {
                    ll.Add(roomArrangeEmpinfo);
                }
                
               
            }
            return ll;
        }

        /// <summary>
        /// 用于居住学生信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="live">转化的时候判断是否是居住信息，true是居住人员信息，false 普通信息</param>
        /// <returns></returns>
        public List<ProStudentView> StudentInformationToProStudentView(List<StudentInformation> data, bool live) {
            dbprotrainees = new ProScheduleForTrainees();
            dbacc = new AccdationinformationBusiness();
            dbdorm = new DormInformationBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            List<ProStudentView> result = new List<ProStudentView>();
            foreach (var item in data)
            {
                ProStudentView proStudentView = new ProStudentView();
                proStudentView.ClassNO = dbprotrainees.GetTraineesByStudentNumber(item.StudentNumber).ClassID;
                proStudentView.EmpinfoName = dbproStudentInformation.GetEmpinfoByStudentNumber(item.StudentNumber).EmpName;
                proStudentView.Name = item.Name;
                proStudentView.Sex = item.Sex;
                proStudentView.StudentNumber = item.StudentNumber;
                proStudentView.Telephone = item.Telephone;
                proStudentView.datatype = "student";
                if (live)
                {
                    Accdationinformation queryacc = dbacc.GetAccdationByStudentNumber(item.StudentNumber);
                    if (queryacc != null)
                    {
                        proStudentView.DormID = queryacc.DormId;
                        DormInformation querydorm = dbdorm.GetDormByDorminfoID(queryacc.DormId);
                        proStudentView.DormName = querydorm.DormInfoName;
                        result.Add(proStudentView);
                    }
                }
                else
                {
                    result.Add(proStudentView);
                }
                
                
            }
            return result;
        }

        /// <summary>
        /// 将学生班级集合对象转化为学生的页面model集合对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="live">同上</param>
        /// <returns></returns>
        public List<ProStudentView> ScheduleForTraineesToProStudentView(List<ScheduleForTrainees> data, bool live)
        {
            dbproStudentInformationBusiness = new ProStudentInformationBusiness();
            List<StudentInformation> list0 = new List<StudentInformation>();
            foreach (var item in data)
            {
                list0.Add(dbproStudentInformationBusiness.GetEntity(item.StudentID));

            }
            return this.StudentInformationToProStudentView(list0, live);
        }
    }
}
