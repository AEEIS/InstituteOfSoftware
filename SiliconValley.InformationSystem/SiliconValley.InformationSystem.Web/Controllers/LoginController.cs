﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.UserManeger;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Depository.CellPhoneSMS;

using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Web.Controllers
{
    //  /Login/LoginIndex
    [IgnoreLogin]
    public class LoginController : Controller
    {
        UsersInfoManeger userinfo = new UsersInfoManeger();
        // GET: Login
        //登录页面
        public ActionResult LoginIndex()
        {
            return View();
        }
        //登录方法
        public ActionResult LoginFunction(Base_User u,string loginType)
        {
            ErrorResult err = new ErrorResult();
            try
            {
                string pwd = Util.Extention.ToMD5String(u.Password);

                if (loginType == "emp")
                {

                    Base_User findu = userinfo.GetList().Where(find => find.UserName == u.UserName && find.Password == pwd).FirstOrDefault();
                    if (findu != null)
                    {
                        SessionHelper sessionHelper = new SessionHelper();


                        SessionHelper.Session["UserId"] = findu.UserId;
                        err.Success = true;
                        err.Msg = "登陆成功!";
                        err.Data = "/Base_SysManage/Base_SysMenu/Index";

                        //获取权限

                        var permisslist = PermissionManage.GetOperatorPermissionValues();

                        SessionHelper.Session["OperatorPermission"] = permisslist;


                        return Json(err, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        err.Msg = "用户名或密码错误";
                    }
                }


                if (loginType == "student")
                {
                    BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();

                    StudentInformation student = db_student.GetEntity(u.UserName);

                    if (student == null)
                    {
                        err.Msg = "用户名或密码错误";

                    }
                    else
                    {
                        if (student.Password == u.Password)
                        {
                            SessionHelper.Session["UserId"] = student.StudentNumber;
                            SessionHelper.Session["studentnumber"] = student.StudentNumber;
                            err.Success = true;
                            err.Msg = "登陆成功!";
                            err.Data = "/Student/Index";

                            //获取权限

                            var permisslist = PermissionManage.GetOperatorPermissionValues();

                            SessionHelper.Session["OperatorPermission"] = permisslist;
                            HttpCookie http = new HttpCookie("isfirst", "true");
                            Response.AppendCookie(http);

                            return Json(err, JsonRequestBehavior.AllowGet);
                        }
                        else {
                            err.Msg = "用户名或密码错误";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, EnumType.LogType.系统异常);
            }
            return Json(err, JsonRequestBehavior.AllowGet);
        }

        //手机短信实例
        public ActionResult PhoneSMS()
        {
            string number = "13204961361";
            string smsText = "达磊，下雨了！！！";
            string t = PhoneMsgHelper.SendMsg(number, smsText);
            return View();
        }
    }
}