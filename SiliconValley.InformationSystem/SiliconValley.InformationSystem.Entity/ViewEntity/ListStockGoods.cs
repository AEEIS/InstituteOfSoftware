﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 这是一个加载添加页面时显示库存里的物品列表所使用的类
    /// </summary>
   public class ListStockGoods
    {
        /// <summary>
        /// 物品Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName { get; set; }
    }
}
