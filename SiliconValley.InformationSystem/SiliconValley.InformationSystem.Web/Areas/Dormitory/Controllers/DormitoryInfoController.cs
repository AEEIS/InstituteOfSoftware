﻿using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{

    /// <summary>
    /// 宿舍管理控制器
    /// </summary>
    public class DormitoryInfoController : Controller
    {
        /// <summary>
        /// 栋楼业务类
        /// </summary>
        private TungFloorBusiness dbtungfloor;
        /// <summary>
        /// 栋业务类
        /// </summary>
        private TungBusiness dbtung;

        /// <summary>
        /// 楼层业务类
        /// </summary>
        private DormitoryfloorBusiness dbfloor;

        /// <summary>
        ///宿舍房间业务类
        /// </summary>
        private DormInformationBusiness dbdorm;

        /// <summary>
        /// 房间居住信息业务类
        /// </summary>
        private AccdationinformationBusiness dbacc;

        /// <summary>
        /// 居住数量业务类
        /// </summary>
        private RoomStayNumberBusiness dbroomnumber;

        /// <summary>
        /// 房间类型业务类
        /// </summary>
        private RoomStayTypeBusiness dbroomtype;

        /// <summary>
        /// 学生跟居住房间
        /// </summary>
        private dbacc_dbstu dbaccstu;

        /// <summary>
        /// 居住信息跟房间数量
        /// </summary>
        private dbacc_dbroomnumber dbaccroomnumber;

        /// <summary>
        /// 寝室长
        /// </summary>
        private DormitoryLeaderBusiness dbleader;

        /// <summary>
        /// 我的学生业务类
        /// </summary>
        private ProStudentInformationBusiness dbprostudent;

        /// <summary>
        /// 用于查询剩余床位对象
        /// </summary>
        private dbacc_dbben_dbroomnumber_dbdorm dbacc_Dbben_Dbroomnumber_Dbdorm;

        // GET: Dormitory/DormitoryInfo

        /// <summary>
        /// 主页面。
        /// </summary>
        /// <returns></returns>
        public ActionResult DormitoryIndex()
        {
            return View();
        }

        /// <summary>
        /// 创建左侧的树形菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {
            dbtung = new TungBusiness();
            dbfloor = new DormitoryfloorBusiness();
            dbtungfloor = new TungFloorBusiness();
            //返回的结果
            resultdtree result = new resultdtree();

            //状态
            dtreestatus dtreestatus = new dtreestatus();

            //获取父级对象楼
            List<Tung> tunglist = dbtung.GetTungs();


            //最外层的儿子数据
            List<dtreeview> childrendtreedata = new List<dtreeview>();

            foreach (var item in tunglist)
            {
                dtreeview seconddtree = new dtreeview();
                try
                {
                    Tung fortung = dbtung.GetTungByTungID(item.Id);
                    
                    seconddtree.nodeId = fortung.Id.ToString();
                    seconddtree.context = fortung.TungName;
                    seconddtree.last = false;
                    seconddtree.parentId = "0";
                    seconddtree.level = 1;
                    seconddtree.spread = true;
                    try
                    {
                        List<TungFloor> floorlist = dbtungfloor.GetTungFloorByTungID(item.Id);

                        List<dtreeview> floortreelist = new List<dtreeview>();

                        foreach (var floor in floorlist)
                        {
                            try
                            {
                                var floorobj = dbfloor.GetDormitoryfloorByFloorID(floor.FloorId);
                                dtreeview floortree = new dtreeview();

                                floortree.nodeId = floorobj.ID.ToString();
                                floortree.context = floorobj.FloorName;
                                floortree.last = true;
                                floortree.parentId = item.Id.ToString();
                                seconddtree.level = 2;
                                floortreelist.Add(floortree);
                                dtreestatus.code = "200";
                                dtreestatus.message = "操作成功";
                            }
                            catch (Exception ex)
                            {
                                dtreestatus.code = "1";
                                dtreestatus.code = "操作失败";
                                throw;
                            }
                        }
                        seconddtree.children = floortreelist;
                    }
                    catch (Exception ex)
                    {
                        dtreestatus.code = "1";
                        dtreestatus.code = "操作失败";
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    dtreestatus.code = "1";
                    dtreestatus.code = "操作失败";
                    throw;
                }
                childrendtreedata.Add(seconddtree);
            }

         

            result.status = dtreestatus;
            result.data = childrendtreedata;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///根据 点击那个树形菜单产生房间数据 
        /// </summary>
        /// <param name="TungID"></param>
        /// <param name="FloorID"></param>
        /// <returns></returns>
        public ActionResult EstablishRoom(int? TungID, int? FloorID)
        {
            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbacc = new AccdationinformationBusiness();
            dbroomnumber = new RoomStayNumberBusiness();
            TungFloor resulttungfloorobj = new TungFloor();
            DistinguishView result = new DistinguishView();
            if (string.IsNullOrEmpty(TungID.ToString())&&string.IsNullOrEmpty(FloorID.ToString()))
            {
                try
                {
                    resulttungfloorobj = dbtungfloor.GetTungFloors().FirstOrDefault();
                    BusHelper.WriteSysLog("在/Dormitory/DormitoryInfo/EstablishRoom中调用TungFloorBusiness业务类中GetTungFloors方法", Entity.Base_SysManage.EnumType.LogType.查询数据success);
                }
                catch (Exception ex)
                {

                    BusHelper.WriteSysLog("在/Dormitory/DormitoryInfo/EstablishRoom中调用TungFloorBusiness业务类中GetTungFloors方法", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                }
            }
            else
            {
                try
                {
                    resulttungfloorobj = dbtungfloor.GetTungFloorByTungIDAndFloorID(TungID, FloorID);
                    BusHelper.WriteSysLog("在/Dormitory/DormitoryInfo/EstablishRoom中调用TungFloorBusiness业务类中GetTungFloorByTungIDAndFloorID方法", Entity.Base_SysManage.EnumType.LogType.查询数据success);

                }
                catch (Exception ex)
                {
                    BusHelper.WriteSysLog("在/Dormitory/DormitoryInfo/EstablishRoom中调用TungFloorBusiness业务类中GetTungFloorByTungIDAndFloorID方法", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                }
               
            }

            try
            {
                
                List<DormInformation> dormlist = dbdorm.GetDormsByTungFloorID(resulttungfloorobj.Id);
                BusHelper.WriteSysLog("在/Dormitory/DormitoryInfo/EstablishRoom中调用DormInformationBusiness业务类中GetDormsByTungFloorID方法", Entity.Base_SysManage.EnumType.LogType.查询数据success);
                List<DormInformationView> leftroom = new List<DormInformationView>();
                List<DormInformationView> rightroom = new List<DormInformationView>();
                foreach (var item in dormlist)
                {
                    DormInformationView  newobject= new DormInformationView();
                    //判断房间是否满人
                    List<Accdationinformation> queryobject= dbacc.GetAccdationinformationByDormId(item.ID);

                    RoomStayNumber queryobject1= dbroomnumber.GetRoomStayNumberByRoomStayNumberId(item.RoomStayNumberId);

                    newobject.Id = item.ID;
                    newobject.RoomStayNumber = queryobject1.StayNumber;
                    newobject.RoomStayTypeId = item.RoomStayTypeId;
                    newobject.SexType = item.SexType;
                    newobject.DormInfoName = item.DormInfoName;

                    if (queryobject.Count==queryobject1.StayNumber)
                        newobject.isfull = true;
                    else
                        newobject.isfull = false;
                    if (item.Direction)
                    {
                        rightroom.Add(newobject);
                    }
                    else
                    {
                        leftroom.Add(newobject);
                    }

                }
                result.leftroom = leftroom;
                result.rightroom = rightroom;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("在/Dormitory/DormitoryInfo/EstablishRoom中调用DormInformationBusiness业务类中GetDormsByTungFloorID方法", Entity.Base_SysManage.EnumType.LogType.查询数据error);
            }
           
            Thread.Sleep(1000);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 为栋添加楼层
        /// </summary>
        /// <param name="TungID"></param>
        /// <returns></returns>
        public ActionResult ForTungAddFloor(int TungID)
        {
            dbtung = new TungBusiness();
            dbfloor = new DormitoryfloorBusiness();
            dbtungfloor = new TungFloorBusiness();
            AjaxResult result = new AjaxResult();

            try
            {
                List<TungFloor> floorlist = dbtungfloor.GetTungFloorByTungID(TungID);
                try
                {
                    List<Dormitoryfloor> list = dbfloor.GetDormitoryfloors();

                    var lastfloor = list.LastOrDefault();

                    int floornumber = int.Parse(lastfloor.FloorName.Substring(0, 1));

                    if (floorlist.Count==list.Count)
                    {
                        Dormitoryfloor mynew = new Dormitoryfloor();
                        mynew.CreationTime = DateTime.Now;
                        mynew.FloorName = (floornumber + 1).ToString() + "楼";
                        mynew.IsDelete = false;
                        mynew.Remark = "创建于" + mynew.CreationTime.Year + mynew.CreationTime.Month + mynew.CreationTime.Day + "添加楼层操作";
                        try
                        {
                            dbfloor.Insert(mynew);
                            BusHelper.WriteSysLog(mynew.Remark + "Dormitory/DormitoryInfo/ForTungAddFloor", Entity.Base_SysManage.EnumType.LogType.添加数据);
                            dbfloor = new DormitoryfloorBusiness();
                            var nowobj = dbfloor.GetDormitoryfloors().Where(a => a.FloorName == mynew.FloorName).FirstOrDefault();
                            if (dbtungfloor.CustomAdd(TungID, nowobj.ID))
                            {
                                result.Success = true;
                                result.Msg = "添加成功";
                            }
                            else
                            {
                                result.Success = false;
                                result.Msg = "添加失败";
                            }
                        }
                        catch (Exception ex)
                        {

                            BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/ForTungAddFloor", Entity.Base_SysManage.EnumType.LogType.添加数据error);
                        }
                    }
                    else
                    {
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            foreach (var item in floorlist)
                            {
                                if (list[i].ID == item.FloorId)
                                {
                                    list.Remove(list[i]);
                                }
                            }
                        }
                        var addfloorobj = list.OrderByDescending(a => a.ID).LastOrDefault();
                        if (dbtungfloor.CustomAdd(TungID, addfloorobj.ID))
                        {
                            result.Success = true;
                            result.Msg = "添加成功";
                        }
                        else
                        {
                            result.Success = false;
                            result.Msg = "添加失败";
                        }
                    }
                }
                catch (Exception ex)
                {

                    BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/ForTungAddFloor", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                }
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/ForTungAddFloor", Entity.Base_SysManage.EnumType.LogType.查询数据error);

            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddTungPage() {
            return View();
        }

        [HttpPost]
        public ActionResult AddTungPage(string TungName,string TungAddress) {

            AjaxResult result = new AjaxResult();

            dbtung = new TungBusiness();
            Tung tung = new Tung();
            tung.CreationTime = DateTime.Now;
            tung.IsDel = false;
            tung.Remark ="创建于" + tung.CreationTime.Year + tung.CreationTime.Month + tung.CreationTime.Day + "添加栋操作";
            tung.TungName = TungName;
            tung.TungAddress = TungAddress;
            try
            {
                dbtung.Insert(tung);
                BusHelper.WriteSysLog("添加数据位置于Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据);
                result.Msg = "添加成功";
                result.Success = true;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据error);
                result.Msg = "添加失败";
                result.Success = false;
            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddRoomPage(string Direction,int Floorid) {
            dbroomtype = new RoomStayTypeBusiness();
            dbroomnumber = new RoomStayNumberBusiness();
            ViewBag.Direction = Direction;
            ViewBag.Floorid = Floorid;
            var data = dbroomtype.GetRoomStayTypes();
           var SelectListItemlist= data.Select(a => new SelectListItem()
            {
                Text = a.RoomStayTypeName,
                Value = a.Id.ToString()
            });
            ViewBag.SelectListItemlist = SelectListItemlist;

            var RoomNumber = dbroomnumber.GetRoomStayNumbers().Select(a => new SelectListItem()
            {
                Text = a.StayNumber + "人制",
                Value = a.Id.ToString()
            });
            ViewBag.RoomNumber = RoomNumber;
            return View();
        }


        [HttpPost]
        /// <summary>
        /// post 添加
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public ActionResult AddRoomPage(DormInformation jsonStr) {
            AjaxResult result = new AjaxResult();
            dbdorm = new DormInformationBusiness();
            //仓库
            if (jsonStr.RoomStayTypeId==5)
            {
                jsonStr.RoomStayNumberId = 0;
            }
            if (jsonStr.RoomStayTypeId!=1)
            {
                jsonStr.SexType = 0;
            }
            jsonStr.CreationTime = DateTime.Now;
            try
            {
                dbdorm.Insert(jsonStr);
                BusHelper.WriteSysLog("添加数据位置于Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据);
                result.Msg = "添加成功";
                result.Success = true;
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据error);
                result.Msg = "添加失败";
                result.Success = false;
            }
            

            return Json(result,JsonRequestBehavior.AllowGet);
        }

       

        [HttpGet]
        /// <summary>
        /// 寝室安排
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomArrangePage() {
            return View();
        }


        /// <summary>
        /// 用于加载未居住的学生数据
        /// </summary>
        /// <returns></returns>
        public ActionResult UninhabitedList() {

            dbaccstu = new dbacc_dbstu();
            var data= dbaccstu.GetUninhabitedData();

            layuitableview<ProStudentView> returnObj = new layuitableview<ProStudentView>();

            if (data!=null)
            {
               var dud= data.Select(a => new ProStudentView()
                {
                    InsitDate=a.InsitDate,
                    Name=a.Name,
                    Reack=a.Reack,
                    Sex=a.Sex,
                    StudentNumber=a.StudentNumber,
                    Telephone=a.Telephone
               }).ToList();
                returnObj.count = dud.Count();
                returnObj.data = dud;

            }
           
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

         
        /// <summary>
        /// 查询寝室
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public ActionResult ChoiceInfo(bool sex) {

            dbdorm = new DormInformationBusiness();
            dbaccroomnumber = new dbacc_dbroomnumber();
           
            AjaxResult ajaxResult = new AjaxResult();
            List<DormInformation> dormlist = new List<DormInformation>();
            if (sex)
            {
                try
                {
                   var data= dbdorm.GetMaleDorm();
                    foreach (var item in data)
                    {
                        if (!dbaccroomnumber.IsFull(item.ID, item.RoomStayNumberId))
                        {
                            dormlist.Add(item);
                        }
                    }
                    var dormInfoViews= this.dormInfoViews(dormlist);
                    ajaxResult.Data = dormInfoViews;
                    ajaxResult.Success = true;
                    BusHelper.WriteSysLog("查询男寝室数据Dormitory/DormitoryInfo/ChoiceInfo", Entity.Base_SysManage.EnumType.LogType.查询数据success);
                }
                catch (Exception ex)
                {

                    ajaxResult.Data = "";
                    ajaxResult.Success = false;
                    BusHelper.WriteSysLog(ex.Message+"查询男寝室数据Dormitory/DormitoryInfo/ChoiceInfo", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                }
              
            }
            else
            {
              
                try
                {
                   var data= dbdorm.GetFemaleDorm();
                    foreach (var item in data)
                    {
                        if (!dbaccroomnumber.IsFull(item.ID, item.RoomStayNumberId))
                        {
                            dormlist.Add(item);
                        }
                    }
                    var dormInfoViews = this.dormInfoViews(dormlist);
                    ajaxResult.Data = dormInfoViews;
                    ajaxResult.Success = true;
                    BusHelper.WriteSysLog("查询女寝室数据Dormitory/DormitoryInfo/ChoiceInfo", Entity.Base_SysManage.EnumType.LogType.查询数据success);
                }
                catch (Exception ex)
                {

                    ajaxResult.Data = "";
                    ajaxResult.Success = false;
                    BusHelper.WriteSysLog(ex.Message + "查询女寝室数据Dormitory/DormitoryInfo/ChoiceInfo", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                }
            }
            
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将房间实体对象转化为页面model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<DormInfoView> dormInfoViews(List<DormInformation> data) {
            dbleader = new DormitoryLeaderBusiness();
            dbprostudent = new ProStudentInformationBusiness();
            List<DormInfoView> dormInfoViews = new List<DormInfoView>();
            foreach (var item in data)
            {
                DormInfoView myview = new DormInfoView();
                myview.ID = item.ID;
                myview.DormInfoName = item.DormInfoName;
                var queryleader = dbleader.GetLeader(item.ID);

                if (queryleader == null)
                {
                    myview.StudentName = "暂定";
                }
                else
                {
                    var querystudent = dbprostudent.GetStudent(queryleader.StudentNumber);
                    myview.StudentName = querystudent.Name;
                }
                dormInfoViews.Add(myview);
            }
            return dormInfoViews;
        }

        /// <summary>
        /// 用于单选框点击事件 返回房间信息
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public ActionResult Checkdorm(int DorminfoID)
        {
            dbdorm = new DormInformationBusiness();
            dbleader = new DormitoryLeaderBusiness();
            dbprostudent = new ProStudentInformationBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            var dormobj = dbdorm.GetDormByDorminfoID(DorminfoID);
            DormInfoView view = new DormInfoView();
            view.ID = dormobj.ID;
            view.DormInfoName = dormobj.DormInfoName;
            var leaderobj = dbleader.GetLeader(dormobj.ID);
            if (leaderobj==null)
            {
                view.StudentName = "暂定";
            }
            else
            {
                var prostudentonbj = dbprostudent.GetStudent(leaderobj.StudentNumber);
                view.StudentName = prostudentonbj.Name;
            }
    
            ajaxResult.Success = true;
            ajaxResult.Data = view;
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回房间剩余的床位数
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public ActionResult BedInfo(int DorminfoID) {
            AjaxResult ajaxResult = new AjaxResult();
            dbacc_Dbben_Dbroomnumber_Dbdorm = new dbacc_dbben_dbroomnumber_dbdorm();
            try
            {
                var querydata=dbacc_Dbben_Dbroomnumber_Dbdorm.GetSurplusbyDorminfoID(DorminfoID);
                BusHelper.WriteSysLog("位于Dormitory/DormitoryInfo/BedInfo", Entity.Base_SysManage.EnumType.LogType.查询数据success);
                ajaxResult.Data = querydata;
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message+ "位于Dormitory/DormitoryInfo/BedInfo", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                ajaxResult.Data = "";
                ajaxResult.Success = false;
            }
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加居住信息
        /// </summary>
        /// <param name="BedId"></param>
        /// <param name="DormId"></param>
        /// <param name="Studentnumber"></param>
        /// <returns></returns>
        public ActionResult ArrangeDorm(int BedId, int DormId, string Studentnumber)
        {
            dbacc = new AccdationinformationBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            Accdationinformation accdationinformation = new Accdationinformation();
            accdationinformation.CreationTime = DateTime.Now;
            accdationinformation.IsDel = false;
            accdationinformation.Remark = string.Empty;
            accdationinformation.StayDate = DateTime.Now;
            accdationinformation.BedId = BedId;
            accdationinformation.DormId = DormId;
            accdationinformation.Studentnumber = Studentnumber;
            ajaxResult.Success= dbacc.AddAcc(accdationinformation);
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);

        }

        #region 房间详细页面
        [HttpGet]
        /// <summary>
        /// 房间详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomdeWithPage(int DorminfoID)
        {
            dbdorm = new DormInformationBusiness();
            DormInformation dorm= dbdorm.GetDormByDorminfoID(DorminfoID);
            //if (dorm.SexType)
            //{

            //}
            ViewBag.DormInfoname= dorm.DormInfoName;
            
            ViewBag.DormInformation = DorminfoID;
            return View();
        }

        /// <summary>
        /// 加载房间的床位以及居住信息
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public ActionResult LoadBedAndAccdation(int DorminfoID)
        {
            AjaxResult ajaxResult = new AjaxResult();
            RoomdeWithPageView view= new RoomdeWithPageView();
            dbacc_Dbben_Dbroomnumber_Dbdorm = new dbacc_dbben_dbroomnumber_dbdorm();
            dbacc = new AccdationinformationBusiness();
           

            try
            {
                List<BenNumber> Beddata = dbacc_Dbben_Dbroomnumber_Dbdorm.GetBensByDorminfoID(DorminfoID);
                
               List<Accdationinformation> Accdata=  dbacc.GetAccdationinformationByDormId(DorminfoID);
                ajaxResult.Success = true;
                view.BedList = Beddata;
                view.AccdationList = Accdata;
                ajaxResult.Data = view;
            }
            catch (Exception ex)
            {

                ajaxResult.Success = false ;
                ajaxResult.Data = "";
            }
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult test() {
            XElement xe = XElement.Load(@"F:\Projects\硅谷信息平台版本更新\1.0.9\SiliconValley.InformationSystem\SiliconValley.InformationSystem.Web\xmlfile\RoomdeWithPage.xml");
            IEnumerable<XElement> elements = from ele in xe.Elements("book")
                                             select ele;
            foreach (var ele in elements)
            {
                //子节点使用Element
                var aa =   ele.Element("author").Value;
                //属性使用Attribute
                var aa =   ele.Attribute("ISBN").Value;
            }
            return View();
        }


    }
}