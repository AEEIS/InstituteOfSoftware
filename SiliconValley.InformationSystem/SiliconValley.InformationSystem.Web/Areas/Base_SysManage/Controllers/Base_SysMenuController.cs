﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Base_SysManage.Controllers
{
    public class Base_SysMenuController : BaseMvcController
    {
        // GET: Base_SysManage/Base_SysMenu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMenus()
        {

            //加入登陆的用户是Admin

            //Base_UserBusiness userdb = new Base_UserBusiness();
            //Base_User user = userdb.GetList().Where(u => u.UserId == "Admin").FirstOrDefault();

            //SessionHelper._Session session = new SessionHelper._Session();
            //session["UserId"] = user.UserId;

            //1.0：学生登陆的时候，
            //提前询问的是自主就业的学生，事先要排除掉，就不加载这个就业意向这个菜单
            List<Menu> menus = SystemMenuManage.GetOperatorMenu();

            return Json(menus, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取登陆人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserClass()
        {
           
            //session["UserId"] = user.UserId;
         return Json(SystemMenuManage.UserClass(), JsonRequestBehavior.AllowGet);
        }
    }
}