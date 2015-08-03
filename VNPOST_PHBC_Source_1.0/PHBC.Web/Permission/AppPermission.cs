using PHBC.DAO.Models;
using PHBC.Web.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PHBC.DAO.Bussiness;
using PHBC.DAO;

namespace PHBC.Web.Permission
{
    public class AppPermission
    {
        private Hashtable lstPermission;
        private List<MenuView> lstMenu;
        private bool fAdmin;
        private string userName;
        public UserModel userInfo;
        public AppPermission(string _userName)
        {
            this.userName = _userName.ToLower();
            fAdmin = CheckAdmin(userName);
            IUserBussiness iUserBussiness = new UserBussiness();
            userInfo = iUserBussiness.getByUserName(userName);
            List<string> lstActionCode = new List<string>();
            if(fAdmin)
            {
                lstMenu = iUserBussiness.getMenuByAction(lstActionCode, true);
                return;
            }
            List<SysAction> lstAction = iUserBussiness.getActionByUserName(userName);
            lstActionCode = lstAction.Select(a => a.Code).ToList();
            if (lstAction != null && lstAction.Count > 0)
            {
                lstMenu = iUserBussiness.getMenuByAction(lstActionCode);
                buildPermisson(lstAction);
            }
            else
            {
                lstMenu = new List<MenuView>();
            }
            lstActionCode = null;
            lstAction = null;
            iUserBussiness.Dispose();
        }
        public bool FAdmin { get { return this.fAdmin; } }

        public UserModel UserInfo { get { return this.userInfo; } }
        public List<MenuView> getMenu
        {
            get { return this.lstMenu; }
        }
        private void buildPermisson(List<SysAction> lstAction)
        {
            lstPermission = new Hashtable();
            string curentController="";
            string curentArea = "";
            PermissonController permisson = null;
            lstAction.ForEach(a => {
                if (!curentArea.Equals(a.Area) || !curentController.Equals(a.Controller))
                {
                    curentArea = a.Area.ToLower();
                    curentController = a.Controller.ToLower();
                    if (permisson != null)
                        lstPermission.Add(permisson.AreaName + permisson.ControllerName, permisson);
                    permisson = new PermissonController();
                    permisson.AreaName = curentArea;
                    permisson.ControllerName = curentController;
                }
                permisson.actionPermisson.Add(a.Action.ToLower());
            });
        }
        public PermissonController getPermision(string areaName, string controllerName)
        {
            if (fAdmin)
            {
                PermissonController permisson = new PermissonController();
                permisson.RoleAdmin = true;
                return permisson;
            }
            string _controllerName = controllerName.ToLower();
            string _areaName = string.IsNullOrWhiteSpace(areaName)? "" : areaName.ToLower();
            return getPermissonController(_areaName, _controllerName);
        }
        public bool hasPermisson(string areaName, string controllerName, string action)
        {
            if (fAdmin)
                return true;
            return getPermision(areaName, controllerName).hasPermisson(action);
        }
        public bool CheckAdmin(string userName){
            if(ConfigurationManager.AppSettings.GetValues("UserAdmin") != null)
            {
                string userAdmin = ConfigurationManager.AppSettings.GetValues("UserAdmin")[0].ToLower();
                return userAdmin.Split(',').Any(a => a.Equals(userName));
            }
            return false;
        }
        private PermissonController getPermissonController(string areaName, string controllerName)
        {
            if (lstPermission.ContainsKey(areaName + controllerName))
                return lstPermission[areaName + controllerName] as PermissonController;
            return new PermissonController();
        }
    }
}
