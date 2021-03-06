﻿using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.Common
{
    /// <summary>
    /// 操作者
    /// </summary>
    public static class Operator
    {
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public static string UserId
        {
            get
            {
                if (GlobalSwitch.RunModel == RunModel.LocalTest)


                    return "4edb3463-c98f-490e-b7c8-694f4436d015"; //徐宝平
                /*return "Admin";*/
                //return "039e40811175f-dbed6ef5-f06a-2804-9b40-ecfa2c7925f9";//为主任或者主任级别以上
                //return "039e40811175f-dbed6ef5-f06a-5702-9b40-ecfa2c7925f9"; //为普通员工

                else
                    return SessionHelper.Session["UserId"]?.ToString();
            }
        }

        public static Base_UserModel Property { get => Base_UserBusiness.GetTheUser(UserId); }

        #region 操作方法

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public static bool Logged()
        {
            return !UserId.IsNullOrEmpty();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userId">用户逻辑主键Id</param>
        public static void Login(string userId)
        {
            SessionHelper.Session["UserId"] = userId;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Logout()
        {
            SessionHelper.Session["UserId"] = null;
        }

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            return UserId == "Admin";
        }

        #endregion
    }
}
