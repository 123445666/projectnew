using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.Web.Permission
{
    public class PermissonController
    {
        public PermissonController()
        {
            RoleAdmin = false;
            actionPermisson = new List<string>();
        }
        public bool RoleAdmin { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public List<string> actionPermisson { get; set; }
        public AppPermission appPermisson { get; set; }
        public bool hasPermisson(string action)
        {
            if (RoleAdmin)
                return true;
            if (actionPermisson.Count == 0)
                return false;
            string _action = action.ToLower();
            return actionPermisson.Any(a => a.Equals(_action));
        }
        public bool hasPermisson(string _areaName, string _controllerName, string _actionName)
        {
            return appPermisson.hasPermisson(_areaName, _controllerName, _actionName);
        }
        public void AddPermisson(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                return;
            if (actionPermisson == null)
                actionPermisson = new List<string>();
            if (this.hasPermisson(action))
                return;
            actionPermisson.Add(action.ToLower());
        }
        public void RemovePermisson(string action)
        {

            if (actionPermisson == null || actionPermisson.Count == 0 || string.IsNullOrWhiteSpace(action))
                return;
            string _action = action.ToLower();
            int index = actionPermisson.FindIndex(a => a == _action);
            if (index >= 0)
                actionPermisson.RemoveAt(index);

        }
    }
}
