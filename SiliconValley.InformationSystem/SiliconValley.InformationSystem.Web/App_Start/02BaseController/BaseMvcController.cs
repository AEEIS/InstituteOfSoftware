﻿namespace SiliconValley.InformationSystem.Web
{
    /// <summary>
    /// Mvc基控制器
    /// </summary>
    [CheckLogin]
    [CheckUrlPermission]
    public class BaseMvcController : BaseController
    {

    }
}