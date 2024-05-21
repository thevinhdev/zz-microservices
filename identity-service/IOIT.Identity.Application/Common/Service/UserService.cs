using IOIT.Identity.Application.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOIT.Identity.Application.Common.Service
{
    public static class UserService
    {
        public static string PlusActiveKey(string key1, string key2)
        {
            string str = "";
            char[] str1 = key1.ToCharArray();
            char[] str2 = key2.ToCharArray();
            for (int i = 0; i < str1.Length; i++)
            {
                int k = int.Parse(str1[i].ToString()) + int.Parse(str2[i].ToString());
                if (k > 1) k = 1;
                str += k;
            }
            return str;
        }

        public static List<MenuDTO> CreateMenu(List<MenuDTO> list, int k)
        {
            var listMenu = list.Where(e => e.MenuParent == k).ToList();
            if (listMenu.Count > 0)
            {
                List<MenuDTO> menus = new List<MenuDTO>();
                foreach (var item in listMenu)
                {
                    char[] str = item.ActiveKey.ToCharArray();
                    if (int.Parse(str[8].ToString()) == 1)
                    {
                        MenuDTO menu = new MenuDTO();
                        menu.MenuId = item.MenuId;
                        menu.Code = item.Code;
                        menu.Name = item.Name;
                        menu.Url = item.Url;
                        menu.Icon = item.Icon;
                        menu.MenuParent = item.MenuParent;
                        menu.ActiveKey = item.ActiveKey;
                        menu.listMenus = CreateMenu(list, item.MenuId);
                        menus.Add(menu);
                    }
                }
                return menus;
            }
            return new List<MenuDTO>();
        }
    }
}
