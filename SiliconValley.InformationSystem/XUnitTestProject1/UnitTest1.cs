using SiliconValley.InformationSystem.Business.EducationalBusiness;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
       

        public UnitTest1()
        {
            
        }

        [Fact]
        public void Test1()
        {
            //��һ�׶� ����ʵ��
            Staff_Cost_StatisticssBusiness db_Const = new Staff_Cost_StatisticssBusiness();

            //�ڶ��׶� Act ����
            var result = db_Const.Staff_CostData("202001030008", DateTime.Parse("2020-04-04"));

        }
    }
}
