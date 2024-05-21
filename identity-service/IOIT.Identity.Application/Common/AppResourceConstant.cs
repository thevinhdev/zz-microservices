using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Common
{
    public static class Resources
    {
        #region "Common"

        public const string EXCEPTION_COMMON = "Có lỗi xẩy ra, vui lòng thử lại!";
        public const string EXCEPTION_UNKNOWN = "EXCEPTION_UNKNOWN";
        public const string RESOURCE_NOT_FOUND = "RESOURCE_NOT_FOUND";
        public const string ID_REQUIRED = "ID_REQUIRED";
        public const string TOKEN_INVALID = "TOKEN_INVALID";
        public const string TOKEN_NULL = "UN_AUTHORIZED";
        public const string ACTION_SUCCESS = "SUCCESS";
        #endregion

        #region "Auth"
        public const string USER_NOT_FOUND = "Tài khoản không tồn tại trong hệ thống hoặc đã bị khóa";
        public const string USER_ALREADY_EXISTED = "Tài khoản đã tồn tại";
        public const string USER_WAS_LOCKED = "Tài khoản đã bị khóa";
        public const string USER_ADD_SUCCESS = "Thêm mới tài khoản thành công";
        public const string USER_UPDATE_SUCCESS = "Cập nhật thông tin tài khoản thành công";
        public const string EMAIL_ALREADY_EXISTED = "Email dã tồn tại trong hệ thống";
        public const string REFERRALCODE_DOES_NOT_EXIST = "Mã giới thiệu không tồn tại trong hệ thống";
        public const string PASSWORD_INCORRECTLY = "Mật khẩu không chính xác";
        public const string LOGIN_SUCCESS = "Đăng nhập thành công!";
        public const string REGISTER_SUCCESS = "Đăng kí thành công!";
        public const string USER_CODE_NOT_NULL = "Vui lòng nhập mã tài khoản";
        public const string USER_CODE_MINLENGTH = "USER_CODE_MINLENGTH";
        #endregion

        #region "LanguageSetting"
        public const string LANGUAGE_CODE_REQUIRED = "LANGUAGE_CODE_REQUIRED";
        public const string LANGUAGE_CODE_LENGHT = "LANGUAGE_CODE_LENGHT";
        public const string LANGUAGEISOCODE_DOES_NOT_EXIST = "LANGUAGEISOCODE_DOES_NOT_EXIST";
        #endregion
    }
}
