using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Users.ViewModels
{
    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LoginModel2 : LoginModel
    {
        public string JwtKey { get; set; }
        public string JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; }
    }

    public class UserLogin
    {
        public long userId { get; set; }
        public long? userMapId { get; set; }
        public int? projectId { get; set; }
        public int? departmentId { get; set; }
        public int? positionId { get; set; }
        public int companyId { get; set; }
        public int languageId { get; set; }
        public int websiteId { get; set; }
        public string logoWebsite { get; set; }
        public string fullName { get; set; }
        public string avata { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int? type { get; set; }
        public int? status { get; set; }
        public int? roleMax { get; set; }
        public byte? roleLevel { get; set; }
        public string roleCode { get; set; }
        public bool isRoleGroup { get; set; }
        public string access_token { get; set; }
        public string access_key { get; set; }
        public List<MenuDTO> listMenus { get; set; }
    }

    public class MenuDTO
    {
        public int MenuId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int MenuParent { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string ActiveKey { get; set; }
        public int? Status { get; set; }
        public List<MenuDTO> listMenus { get; set; }
    }
}
