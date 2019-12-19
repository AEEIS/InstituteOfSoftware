using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace SiliconValley.InformationSystem.Business.Base_SysManage
{
   
    public class Base_SysRoleBusiness : BaseBusiness<Base_SysRole>
    {
        #region �ⲿ�ӿ�

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public List<Base_SysRole> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //ģ����ѯ
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_SysRole GetTheData(string id)
        {
            return GetEntity(id);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="newData">����</param>
        public void AddData(Base_SysRole newData)
        {
            Insert(newData);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateData(Base_SysRole theData)
        {
            Update(theData);
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="theData">ɾ��������</param>
        public void DeleteData(List<string> ids)
        {
            //ɾ����ɫ
            Delete(ids);
        }

        /// <summary>
        /// ����Ȩ��
        /// </summary>
        /// <param name="roleId">��ɫId</param>
        /// <param name="permissions">Ȩ��ֵ</param>
        public void SavePermission(string roleId,List<string> permissions)
        {
            Service.Delete_Sql<Base_PermissionRole>(x => x.RoleId == roleId);
            List<Base_PermissionRole> insertList = new List<Base_PermissionRole>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionRole
                {
                    Id=Guid.NewGuid().ToSequentialGuid(),
                    RoleId=roleId,
                    PermissionValue=newPermission
                });
            });

            Service.Insert(insertList);
            PermissionManage.ClearUserPermissionCache();
        }

        public AjaxResult createRole(string roleName, string businessName)
        {
            AjaxResult result = new AjaxResult();

            //������֤ �Ƿ����
           var existrole = this.GetList().Where(d => d.RoleName == roleName).FirstOrDefault();

            if (existrole != null)
            {
                //�Ѵ���
                result.ErrorCode = 444;
                result.Msg = "��ɫ�����ظ���";
                result.Data = null;

                return result;
            }

            //********************************************************************//

            Base_SysRole role = new Base_SysRole();
            role.BusinessName = businessName;
            role.Id = Guid.NewGuid().ToString();
            role.RoleId = Guid.NewGuid().ToString();
            role.RoleName = roleName;

            this.AddData(role);

            result.ErrorCode = 200;
            result.Msg = "�ɹ���";
            result.Data = null;

            return result;

        }



        /// <summary>
        /// ��ȡ��ɫ��ӵ�е�urlȨ��
        /// </summary>
        /// <returns></returns>
        public List<PermissionModule> RolePermission(string roleId)
        {

           var result = PermissionManage.GetRolePermissionModules(roleId);

            return result;
        }

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }
}