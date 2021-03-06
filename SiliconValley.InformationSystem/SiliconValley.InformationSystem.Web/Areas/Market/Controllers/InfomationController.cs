﻿using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.StuInfomationType_Maneger;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
    public class InfomationController : Controller
    {
        private StuInfomationTypeManeger StuInfomationType_Entity;
        // GET: /Market/Infomation/AddInfomationType
        public ActionResult InfomationIndexView()
        {
            return View();
        }

        //获取信息类型所有的数据
        public ActionResult GetInfomationData(int page, int limit)
        {
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            List<StuInfomationType> infomation_List = StuInfomationType_Entity.GetList().OrderByDescending(i => i.Id).ToList();
            List<StuInfomationType> infomation_List2 = infomation_List.Skip((page - 1) * limit).Take(limit).ToList();
            var Jsondata = new
            {
                code = 0, //解析接口状态,
                msg = "", //解析提示文本,
                count = infomation_List.Count, //解析数据长度
                data = infomation_List2 //解析数据列表
            };
            return Json(Jsondata, JsonRequestBehavior.AllowGet);
        }

        //修改信息类型
        public ActionResult EditInfomationType(string id, string name, string state,string shuxing)
        {
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            int Id = Convert.ToInt32(id);

            if (shuxing!=null)
            {
                try
                {
                    StuInfomationType findinfomation = StuInfomationType_Entity.GetList().Where(i => i.Id == Id).FirstOrDefault();
                    StuInfomationType Isrepeat = StuInfomationType_Entity.GetList().Where(i => i.Name == name).FirstOrDefault();
                    if (findinfomation != null && Isrepeat == null && shuxing!=null)
                    {
                        switch (shuxing)
                        {
                            case "Name":
                                findinfomation.Name = name;
                                break;
                            case "Rmark":
                                findinfomation.Rmark = name;
                                break;
                        }
                       
                        StuInfomationType_Entity.Update(findinfomation);
                    }
                    else if (findinfomation == null)
                    {
                        BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:数据未能查找到该值", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                        return Json("数据有误，请重试!", JsonRequestBehavior.AllowGet);
                    }
                    else if (Isrepeat != null)
                    {
                        return Json("数据重复", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    //将错误填写到日志中     
                    BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                    return Json("no", JsonRequestBehavior.AllowGet);
                }

            }
            if (!string.IsNullOrEmpty(state))
            {
                StuInfomationType findinfomation = StuInfomationType_Entity.GetList().Where(i => i.Id == Id).FirstOrDefault();
                if (findinfomation != null)
                {
                    try
                    {
                        bool Isdel = Convert.ToBoolean(state) == true ? false : true;
                        findinfomation.IsDelete = Isdel;
                        StuInfomationType_Entity.Update(findinfomation);
                    }
                    catch (Exception ex)
                    {
                        //将错误填写到日志中     
                        BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                        return Json("修改错误，请重试", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            BusHelper.WriteSysLog("操作人:" + UserName + "编辑成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        //添加信息类型
        public ActionResult AddInfomationType(StuInfomationType new_s)
        {
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            new_s.IsDelete = false;
            AjaxResult a = StuInfomationType_Entity.Add_Data(new_s);
            return Json(a,JsonRequestBehavior.AllowGet);
        }

        //添加页面
        public ActionResult AddView()
        {
            return View();
        }
    }
}