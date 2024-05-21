using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Users.ViewModels
{
    public class ResUserLoginApp
    {
        public long UserId { get; set; }
        public long? UserMapId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? LanguageId { get; set; }
        public byte? Type { get; set; }
        public string FullName { get; set; }
        public string CardId { get; set; }
        public string Avata { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Sex { get; set; }
        public byte? Status { get; set; }
        public int? CountLogin { get; set; }
        public string access_key { get; set; }
        public string access_token { get; set; }
        public bool? IsPhoneConfirm { get; set; }
        public bool? IsRoleGroup { get; set; }
        public string RoleCode { get; set; }
        public string RegisterCode { get; set; }
        public string baseApi { get; set; }
        public string baseUrlImgPicture { get; set; }
        public string baseUrlImgThumbProduct { get; set; }
        public string baseUrlImgThumbNews { get; set; }
        public int? badgeNotification { get; set; }
        public int? badgeMail { get; set; }
        public int MetaCode { get; set; }
        public string MetaMess { get; set; }
    }
}
