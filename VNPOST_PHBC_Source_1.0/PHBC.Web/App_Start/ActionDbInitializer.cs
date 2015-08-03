using PHBC.DAO;
using PHBC.DAO.Bussiness;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PHBC.Web.Constants;
using PHBC.DAO.Models;
using Elmah;

namespace PHBC.Web
{
    public class ActionDbInitializer
    {
        public ActionDbInitializer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <modify>
        /// Author  date        Comment
        /// Anhhn   9/6/2015    Tao moi
        /// </modify>
        public void InitializeControllAction()
        {
            List<SysAction> lstAction = new List<SysAction>();
            //Account
            List<ActionDefine> lstActionDefine = InitializerAction.getActionDefine();
            SysAction action;
            string[] actionCustom;
            string[] actionParam;
            string[] actionCustomDesc;
            string[] actionCustomMenu;
            string controllerDesc = string.Empty;
            string[] lstActionType = ActionType.GroupAll.Split('|');
            string[] lstActionTypeDesc = ActionTypeDesc.GroupAll.Split('|');
            string[] lstActionTypeMenu = ActionTypeMenu.GroupAll.Split('|');
            foreach (ActionDefine item in lstActionDefine)
            {
                action = new SysAction();
                //if (!string.IsNullOrWhiteSpace(item.Component))
                //    action. = item.Component;
                if (!string.IsNullOrWhiteSpace(item.AreaName))
                    action.Area = item.AreaName.ToLower();
                else action.Area = "";
                if (!string.IsNullOrWhiteSpace(item.ControllerName))
                    action.Controller = item.ControllerName.ToLower();
                if (!string.IsNullOrWhiteSpace(item.ControllerDes))
                    controllerDesc = item.ControllerDes;
                if (!string.IsNullOrWhiteSpace(item.ActionIgnore))
                {
                    for (int i = 0; i < lstActionType.Length; i++ )
                    {
                        if (!item.ActionIgnore.Contains(lstActionType[i]))
                            lstAction.Add(buildAction(action, lstActionType[i], lstActionTypeDesc[i] + " " + controllerDesc, lstActionTypeMenu[i], ""));
                    }
                }
                else
                {
                    for (int i = 0; i < lstActionType.Length; i++)
                    {
                        lstAction.Add(buildAction(action, lstActionType[i], lstActionTypeDesc[i] + " " + controllerDesc, lstActionTypeMenu[i], ""));
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.ActionCustom))
                {
                    actionCustom = item.ActionCustom.Split('|');
                    int countCustom = actionCustom.Length;
                    if (!string.IsNullOrWhiteSpace(item.ActionParam))
                        actionParam = item.ActionParam.Split('|');
                    else
                        actionParam = newStringArray(countCustom);
                    if (!string.IsNullOrWhiteSpace(item.ActionCustomDes))
                        actionCustomDesc = item.ActionCustomDes.Split('|');
                    else
                        actionCustomDesc = newStringArray(countCustom);
                    if (!string.IsNullOrWhiteSpace(item.ActionCustomMenu))
                        actionCustomMenu = item.ActionCustomMenu.Split('|');
                    else
                        actionCustomMenu = newStringArray(countCustom);
                    for (int i = 0; i < countCustom; i++)
                        lstAction.Add(buildAction(action, actionCustom[i], actionCustomDesc[i], actionCustomMenu[i], actionParam[i]));
                }
            }
            ISysActionBussiness AcBsc = new SysActionBussiness();
            try{
                AcBsc.UpdateActions(lstAction);
            }
            catch (Exception e)
            {
                Exception ex = new Exception("ID = 1", e);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// Tao Action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="code"></param>
        /// <param name="area"></param>
        /// <param name="desc"></param>
        /// <param name="pram"></param>
        /// <returns></returns>
        private SysAction buildAction(SysAction actionDefine, string action, string desc,string isMenu, string pram)
        {
            SysAction sAction = new SysAction();
            sAction.Controller = actionDefine.Controller;
            sAction.Area = actionDefine.Area;
            sAction.Action = action;
            sAction.Description = desc;
            sAction.Params = pram.ToLower();
            string area = string.IsNullOrWhiteSpace(actionDefine.Area) ? "" : actionDefine.Area.ToLower() + "_";
            sAction.Code = area + actionDefine.Controller.ToLower() + "_" + action.ToLower();
            sAction.IsMenu = isMenu == "1"? true : false;
            return sAction;
        }

        private string[] newStringArray(int n)
        {
            string[] result = new string[n];
            for (int i = 0; i < n; i++)
                result[i] = string.Empty;
            return result;
        }

        /* Repository not suport EF 6
        private void buildActionRepository(string controller, string action, string code, string area, string desc, string pram)
        {
            controller = controller.ToLower();
            action = action.ToLower();
            code = code.ToLower();
            area = area.ToLower();
            desc = desc.ToLower();
            pram = pram.ToLower();
            //Neu code da xu ly roi thi khong xu ly nua
            if (lstCode.Any(c => c.CompareTo(code) == 0))
                return;
            lstCode.Add(code);
            //Neu code chua co thi the moi
            if(lstCurrentAction == null || lstCurrentAction.Count==0 || !lstCurrentAction.Any(c=>c.Code.CompareTo(code)==0))
            { 
                SysAction sAction = new SysAction();
                sAction.Controller = controller;
                sAction.Area = area;
                sAction.Action = action;
                sAction.Description = desc;
                sAction.Params = pram;
                sAction.Code = code;
                lstActionAdd.Add(sAction);
            }
            else
            {
                SysAction sAction = lstCurrentAction.Single(c => c.Code.CompareTo(code) == 0);
                
                bool hashChangeMenu = false;
                bool hashChange = false;
                if (sAction.Controller.CompareTo(controller) != 0)
                {
                    hashChangeMenu = true;
                    hashChange = true;
                }
                else if (sAction.Action.CompareTo(action) != 0)
                {
                    hashChangeMenu = true;
                    hashChange = true;
                }
                else if (sAction.Area.CompareTo(area) != 0)
                {
                    hashChangeMenu = true;
                    hashChange = true;
                }
                if (sAction.Description.CompareTo(desc) != 0)
                {
                    hashChange = true;
                }
                if (sAction.Params.CompareTo(pram) == 0)
                {
                    hashChangeMenu = true;
                    hashChange = true;
                }
                if (hashChange)
                {
                    sAction.Controller = controller;
                    sAction.Action = action;
                    sAction.Area = area;
                    sAction.Description = desc;
                    sAction.Params = pram;
                    lstActionModify.Add(sAction);
                }
                if(hashChangeMenu)
                {
                    SysAction changeMenuAcion = new SysAction();
                    changeMenuAcion.Code = code;
                    changeMenuAcion.Controller = controller;
                    changeMenuAcion.Action = action;
                    changeMenuAcion.Area = area;
                    changeMenuAcion.Description = desc;
                    changeMenuAcion.Params = pram;
                    lstChangeMenu.Add(changeMenuAcion);
                }
            }
        }
         */
    }
}