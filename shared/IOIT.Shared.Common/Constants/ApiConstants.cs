using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.Commons.Constants
{
    public class ApiConstants
    {
        public static class StatusCode
        {
            public const int Success200 = 200;
            public const int Success201 = 201;
            public const int Valid210 = 210;
            public const int Valid211 = 211;
            public const int Valid212 = 212;
            public const int Valid213 = 213;
            public const int NoPermision = 222;
            public const int Error400 = 400;
            public const int Error401 = 401;
            public const int Error404 = 404;
            public const int Error422 = 422;
            public const int Error500 = 500;
            public const int Error600 = 600;
        }

        public static class MessageResource
        {
            #region common
            public const string ACCTION_SUCCESS = "Thành công!";
            public const string EXCEPTION_UNKNOWN = "Unknown exception!";
            public const string ADD_SUCCESS = "Thêm mới thành công!";
            public const string UPDATE_SUCCESS = "Update success!";
            public const string UNAUTHORIZE = "Unthorize! No right.";
            public const string DELETE_SUCCESS = "Delete success!.";

            public static string MISS_DATA_MESSAGE = "Thông tin không đủ!";  //error_code = 210
            public static string BAD_REQUEST_MESSAGE = "Lỗi sai dữ liệu!";  //error_code = 400
            public static string NOT_FOUND_VIEW_MESSAGE = "Không tìm thấy mục cần xem!"; //error_code = 404
            public static string NOT_FOUND_UPDATE_MESSAGE = "Không tìm thấy mục cần sửa!"; //error_code = 404
            public static string NOT_FOUND_DELETE_MESSAGE = "Không tìm thấy mục cần xóa!"; //error_code = 404
            public static string ERROR_EXIST_MESSAGE = "Mục này đã tồn tại!";   //error_code = 211

            public static string ERROR_400_MESSAGE = "Có lỗi xảy ra. Xin vui lòng thử lại sau!";    //error_code = 400
            public static string ERROR_500_MESSAGE = "Hệ thống xảy ra lỗi. Xin vui lòng thử lại sau!";

            public const string NOT_FOUND = "Not found!";
            public const string INVALID = "Invalid!";
            #endregion

            #region check role
            public const string NOPERMISION_VIEW_MESSAGE = "Bạn không có quyền xem dữ liệu tới mục này!";    //error_code = 222
            public const string NOPERMISION_UPDATE_MESSAGE = "Bạn không có quyền cập nhật mục này!";   //error_code = 222
            public const string NOPERMISION_CREATE_MESSAGE = "Bạn không có quyền thêm mới mục này!";   //error_code = 222
            public const string NOPERMISION_DELETE_MESSAGE = "Bạn không có quyền xoá mục này!";   //error_code = 222
            public const string NOPERMISION_ACCEPT_MESSAGE = "Bạn không có quyền duyệt đơn đăng ký!";
            public const string NOPERMISION_ACTION_MESSAGE = "Bạn không có quyền thực hiện thao tác này!";
            #endregion
        }

        public static class ErrorCode
        {
            #region shared
            public const string ERROR_SERVER = "ERROR_SERVER";
            public const string ERROR_AUTHORIZED = "ERROR_AUTHORIZED";
            public const string ERROR_VALIDATION = "ERROR_VALIDATION";
            public const string ERROR_BADREQUEST = "ERROR_BADREQUEST";
            public const string ERROR_NOTFOUND = "ERROR_NOTFOUND";
            public const string ERROR_UNKNOWN = "ERROR_UNKNOWN";
            public const string ERROR_UNPERMISSION = "ERROR_UNPERMISSION";
            public const string ERROR_CONNECTION = "ERROR_CONNECTION";
            public const string ERROR_TIMEOUT = "ERROR_TIMEOUT";
            public const string ERROR_PERMISSSION = "ERROR_PERMISSSION";
            #endregion

            #region common-service
            public const string ERROR_PROMOTION = "ERROR_PROMOTION";
            public const string ERROR_PROJECTID_NOT_NULL = "ERROR_PROJECTID_NOT_NULL";
            public const string ERROR_UPLOAD_FILE_FORMAT = "ERROR_UPLOAD_FILE_FORMAT";
            public const string ERROR_UPLOAD_FILE_SIZING = "ERROR_UPLOAD_FILE_SIZING";
            public const string ERROR_APARTMENT_CODE_NOT_EXIST = "ERROR_APARTMENT_CODE_NOT_EXIST";
            public const string ERROR_APARTMENT_CODE_EXIST = "ERROR_APARTMENT_CODE_EXIST";
            public const string ERROR_APARTMENT_NOT_EXIST = "ERROR_APARTMENT_NOT_EXIST";
            public const string ERROR_CANNOT_DELETE_APARTMENT = "ERROR_CANNOT_DELETE_APARTMENT";
            public const string ERROR_BANNER_NOT_EXIST = "ERROR_BANNER_NOT_EXIST";
            public const string ERROR_CATEGORY_NOT_EXIST = "ERROR_CATEGORY_NOT_EXIST";
            public const string ERROR_CONTACT_NOT_EXIST = "ERROR_CONTACT_NOT_EXIST";
            public const string ERROR_COUNTRY_NOT_EXIST = "ERROR_COUNTRY_NOT_EXIST";
            public const string ERROR_DEPARTMENT_NOT_EXIST = "ERROR_DEPARTMENT_NOT_EXIST";
            public const string ERROR_DEPARTMENT_CODE_EXIST = "ERROR_DEPARTMENT_CODE_EXIST";
            public const string ERROR_DEPARTMENT_CANNOT_DELETE = "ERROR_DEPARTMENT_CANNOT_DELETE";
            public const string DEPARTMENT_MUST_ONCE_MANAGERMENT = "DEPARTMENT_MUST_ONCE_MANAGERMENT";
            public const string ERROR_FLOOR_NOT_EXIST = "ERROR_FLOOR_NOT_EXIST";
            public const string ERROR_FLOOR_CODE_EXIST = "ERROR_FLOOR_CODE_EXIST";
            public const string ERROR_FLOOR_CANNOT_DELETE = "ERROR_FLOOR_CANNOT_DELETE";
            public const string ERROR_PROJECT_NOT_EXIST = "ERROR_PROJECT_NOT_EXIST";
            public const string ERROR_PROJECT_CODE_EXIST = "ERROR_PROJECT_CODE_EXIST";
            public const string ERROR_PROJECT_CANNOT_DELETE = "ERROR_PROJECT_CANNOT_DELETE";
            public const string ERROR_TOWER_NOT_EXIST = "ERROR_TOWER_NOT_EXIST";
            public const string ERROR_TOWER_CODE_EXIST = "ERROR_TOWER_CODE_EXIST";
            public const string ERROR_TOWER_CANNOT_DELETE = "ERROR_TOWER_CANNOT_DELETE";
            public const string ERROR_NEWS_NOT_EXIST = "ERROR_NEWS_NOT_EXIST";
            public const string ERROR_TYPEATTRIBUTE_NOT_EXIST = "ERROR_TYPEATTRIBUTE_NOT_EXIST";
            public const string ERROR_TYPEATTRIBUTE_CODE_EXIST = "ERROR_TYPEATTRIBUTE_CODE_EXIST";
            public const string ERROR_PROVINCE_NOT_EXIST = "ERROR_PROVINCE_NOT_EXIST";
            public const string ERROR_PROVINCE_CODE_EXIST = "ERROR_PROVINCE_CODE_EXIST";
            public const string ERROR_WARD_NOT_EXIST = "ERROR_WARD_NOT_EXIST";
            public const string ERROR_DISTRICT_NOT_EXIST = "ERROR_DISTRICT_NOT_EXIST";
            public const string ERROR_HOTLINE_NOT_EXIST = "ERROR_HOTLINE_NOT_EXIST";
            public const string LOG_CONFIRM_CHANGE_PASSWORD_CARPARKING = "LOG_CONFIRM_CHANGE_PASSWORD_CARPARKING";
            #endregion

            #region utilities-service
            public const string ERROR_CONSTRUCTIONFORM_NOT_EXIST = "ERROR_CONSTRUCTIONFORM_NOT_EXIST";
            public const string ERROR_CONSTRUCTIONFORM_DELETED = "ERROR_CONSTRUCTIONFORM_DELETED";
            public const string ERROR_EXTENSION_CONSTRUCTIONFORM_NOT_EXIST = "ERROR_EXTENSION_CONSTRUCTIONFORM_NOT_EXIST";
            public const string ERROR_EXTENSION_CONSTRUCTIONFORM_UPDATE_FAILED = "ERROR_EXTENSION_CONSTRUCTIONFORM_UPDATE_FAILED";
            public const string ERROR_CONSTRUCTIONACEEPTANCE_FINISHED = "ERROR_CONSTRUCTIONACEEPTANCE_FINISHED";
            public const string ERROR_CONSTRUCTIONACEEPTANCE_CREATED_FAILED = "ERROR_CONSTRUCTIONACEEPTANCE_CREATED_FAILED";
            public const string ERROR_CONSTRUCTIONACEEPTANCE_UPDATE_FAILED = "ERROR_CONSTRUCTIONACEEPTANCE_UPDATE_FAILED";
            public const string ERROR_CONSTRUCTIONFORM_CREATED_FAILED = "ERROR_CONSTRUCTIONFORM_CREATED_FAILED";
            public const string ERROR_CONSTRUCTIONFORM_UPDATE_FAILED = "ERROR_CONSTRUCTIONFORM_UPDATE_FAILED";
            public const string ERROR_CONTRACTOR_CREATED_FAILED = "ERROR_CONTRACTOR_CREATED_FAILED";
            public const string ERROR_CONTRACTOR_UPDATE_FAILED = "ERROR_CONTRACTOR_UPDATE_FAILED";
            public const string ERROR_CONTRACTOR_NOT_FOUND = "ERROR_CONTRACTOR_NOT_FOUND";
            public const string ERROR_DEPOSIT_CREATE_FAILED = "ERROR_DEPOSIT_CREATE_FAILED";
            public const string ERROR_DEPOSIT_UPDATE_FAILED = "ERROR_DEPOSIT_UPDATE_FAILED";
            public const string ERROR_DEPOSIT_NOT_FOUND = "ERROR_DEPOSIT_NOT_FOUND";

            public const string ERROR_REGISTER_TERM_EXIST = "ERROR_REGISTER_TERM_EXIST";
            public const string ERROR_REGISTER_TERM_NOT_EXIST = "ERROR_REGISTER_TERM_NOT_EXIST";

            public const string ERROR_CARDREQUEST_NOT_EXIST = "ERROR_CARDREQUEST_NOT_EXIST";
            public const string ERROR_REQUESTGROUP_NOT_EXIST = "ERROR_REQUESTGROUP_NOT_EXIST";
            public const string ERROR_REQUESTGROUP_FINISHED = "ERROR_REQUESTGROUP_FINISHED";
            public const string ERROR_RESIDENTCARD_NOT_EXIST = "ERROR_RESIDENTCARD_NOT_EXIST";
            public const string ERROR_RESIDENT_CREATE_FAILED = "ERROR_RESIDENT_CREATE_FAILED";
            public const string ERROR_REQUESTGROUP_VALIDATION = "ERROR_REQUESTGROUP_VALIDATION";
            public const string ERROR_REQUESTGROUP_PROJECT_REQUIRE_CARPARKING_CONFIG = "ERROR_REQUESTGROUP_PROJECT_REQUIRE_CARPARKING_CONFIG";
            public const string ERROR_APARTMENT_CODE_NOT_EXIST_CARPARKING = "ERROR_APARTMENT_CODE_NOT_EXIST_CARPARKING";
            public const string ERROR_MOTOCYCLE_CARD_NOT_EXIST = "ERROR_MOTOCYCLE_CARD_NOT_EXIST";
            public const string ERROR_OTO_CARD_NOT_EXIST = "ERROR_OTO_CARD_NOT_EXIST";
            public const string ERROR_VERHICLE_IN_PARKING = "ERROR_VERHICLE_IN_PARKING";
            public const string ERROR_RESIDENTCARD_ACTIVED = "ERROR_RESIDENTCARD_ACTIVED";
            public const string ERROR_CARD_NOT_ACTIVE_CARPARKING = "ERROR_CARD_NOT_ACTIVE_CARPARKING";
            public const string ERROR_CARD_NOT_ACTIVE = "ERROR_CARD_NOT_ACTIVE";
            public const string ERROR_REQUESTGROUP_CARD_RENEW_EMPTY = "ERROR_REQUESTGROUP_CARD_RENEW_EMPTY";
            public const string INFO_DATA_CARPARKING = "INFO_DATA_CARPARKING";

            public const string ERROR_RENTA_APARTMENT_CANNOT_DELETE = "ERROR_RENTA_APARTMENT_CANNOT_DELETE";
            public const string ERROR_RENTA_APARTMENT_UNPAID = "ERROR_RENTA_APARTMENT_UNPAID";
            public const string ERROR_RENTA_APARTMENT_CREATE_FAILED = "ERROR_RENTA_APARTMENT_CREATE_FAILED";

            public const string ERROR_SERVICEFEE_NOT_EXIST = "ERROR_SERVICEFEE_NOT_EXIST";
            public const string ERROR_SERVICE_EVALUATED = "ERROR_SERVICE_EVALUATED";
            public const string ERROR_SERVICERATE_NOT_EXIST = "ERROR_SERVICERATE_NOT_EXIST";
            public const string ERROR_SERVICEORDER_VALIDATION = "ERROR_SERVICEORDER_VALIDATION";
            public const string ERROR_SERVICEORDER_CREATE_FAILED = "ERROR_SERVICEORDER_CREATE_FAILED";
            public const string ERROR_SERVICEORDER_CODE_EXIST = "ERROR_SERVICEORDER_CODE_EXIST";
            public const string ERROR_SERVICEORDER_NOT_EXIST = "ERROR_SERVICEORDER_NOT_EXIST";
            public const string ERROR_SERVICEORDER_UNPAID = "ERROR_SERVICEORDER_UNPAID";

            public const string ERROR_CATEGORYDOCUMENT_NOT_EXIST = "ERROR_CATEGORYDOCUMENT_NOT_EXIST";
            public const string ERROR_CATEGORYDOCUMENT_EXIST = "ERROR_CATEGORYDOCUMENT_EXIST";
            public const string ERROR_DOCUMENTCLEANING_NAME_EXIST = "ERROR_DOCUMENTCLEANING_NAME_EXIST";
            public const string ERROR_DOCUMENTCLEANING_NOT_EXIST = "ERROR_DOCUMENTCLEANING_NOT_EXIST";
            public const string ERROR_DOCUMENTCLEANING_CANNOT_DELETE = "ERROR_DOCUMENTCLEANING_CANNOT_DELETE";
            public const string ERROR_DOCUMENTCLEANING_CANNOT_CONFIRM = "ERROR_DOCUMENTCLEANING_CANNOT_CONFIRM";
            public const string ERROR_DOCUMENTCLEANING_REQUIRED_PAYMENT = "ERROR_DOCUMENTCLEANING_REQUIRED_PAYMENT";
            public const string ERROR_DOCUMENTCLEANING_UPATE_FEE = "ERROR_DOCUMENTCLEANING_UPATE_FEE";
            public const string ERROR_DOCUMENTCLEANING_VALIDATION = "ERROR_DOCUMENTCLEANING_VALIDATION";

            public const string ERROR_TRANSPORTREQUEST_NOT_EXIST = "ERROR_TRANSPORTREQUEST_NOT_EXIST";
            public const string ERROR_REFERRALCODE_NOT_EXIST = "ERROR_REFERRALCODE_NOT_EXIST";
            public const string ERROR_REFERRALCODE_NOT_AUTH = "ERROR_TRANSPORTREQUEST_NOT_AUTH";

            public const string ERROR_RATETYPE_EXIST = "ERROR_RATETYPE_EXIST";
            public const string ERROR_RATETYPE_NOT_EXIST = "ERROR_RATETYPE_NOT_EXIST";
            #endregion

            #region identity-service
            public const string ERROR_EMPLOYEE_NOT_EXIST = "ERROR_EMPLOYEE_NOT_EXIST";
            public const string ERROR_EMPLOYEE_CODE_EXIST = "ERROR_EMPLOYEE_CODE_EXIST";
            public const string ERROR_EMPLOYEE_PHONE_EXIST = "ERROR_EMPLOYEE_PHONE_EXIST";
            public const string ERROR_EMPLOYEE_CREATE_FAILED = "ERROR_EMPLOYEE_CREATE_FAILED";
            public const string ERROR_EMPLOYEE_UPDATE_FAILED = "ERROR_EMPLOYEE_UPDATE_FAILED";
            public const string ERROR_ROLE_NOT_EXIST = "ERROR_ROLE_NOT_EXIST";
            public const string ERROR_FUNCTION_EXISTED = "ERROR_FUNCTION_EXISTED";
            public const string ERROR_FUNCTION_NOT_EXIST = "ERROR_FUNCTION_NOT_EXIST";
            public const string ERROR_FUNCTION_CODE_EMPTY = "ERROR_FUNCTION_CODE_EMPTY";
            public const string ERROR_FUNCTIONROLE_NOT_EXIST = "ERROR_FUNCTIONROLE_NOT_EXIST";
            public const string ERROR_FUNCTION_CREATE_FAILED = "ERROR_FUNCTION_CREATE_FAILED";
            public const string ERROR_FUNCTION_UPDATE_FAILED = "ERROR_FUNCTION_UPDATE_FAILED";
            public const string ERROR_RESIDENT_NOT_EXIST = "ERROR_RESIDENT_NOT_EXIST";
            public const string ERROR_RESIDENTMAIN_NOT_EXIST = "ERROR_RESIDENTMAIN_NOT_EXIST";
            public const string ERROR_RESIDENTMAIN_NOT_ACTIVE = "ERROR_RESIDENTMAIN_NOT_ACTIVE";
            public const string ERROR_RESIDENT_PHONE_NOT_FOUND = "ERROR_RESIDENT_PHONE_NOT_FOUND";
            public const string ERROR_RESIDENT_UPDATE_FAILED = "ERROR_RESIDENT_UPDATE_FAILED";
            public const string ERROR_RESIDENT_DELETE_FAILED = "ERROR_RESIDENT_DELETE_FAILED";
            public const string ERROR_USER_NOT_EXIST = "ERROR_RESIDENT_NOT_EXIST";
            public const string ERROR_USER_LOCKED = "ERROR_USER_LOCKED";
            public const string ERROR_USERNAME_EXISTED = "ERROR_USERNAME_EXISTED";
            public const string ERROR_USERRESIDENT_NOT_EXIST = "ERROR_USERRESIDENT_NOT_EXIST";
            public const string ERROR_EMAIL_EXISTED = "ERROR_EMAIL_EXISTED";
            public const string ERROR_REGISTRATION_USER_FAILED = "ERROR_REGISTRATION_USER_FAILED";
            public const string ERROR_USER_UPDATE_FAILED = "ERROR_USER_UPDATE_FAILED";
            public const string ERROR_USER_DELETE_FAILED = "ERROR_USER_DELETE_FAILED";
            public const string ERROR_USER_LOGIN_FAILED = "ERROR_USER_LOGIN_FAILED";
            public const string ERROR_USER_LOGIN_NOT_FOUND = "ERROR_USER_LOGIN_NOT_FOUND";
            public const string ERROR_USER_NOT_ACTIVE = "ERROR_USER_NOT_ACTIVE";
            public const string ERROR_OPT_GET_FAILED = "ERROR_OPT_GET_FAILED";
            public const string ERROR_USERROLE_CREATE_FAILED = "ERROR_USERROLE_CREATE_FAILED";
            public const string ERROR_USERROLE_DELETE_FAILED = "ERROR_USERROLE_DELETE_FAILED";
            public const string ERROR_USERROLE_UPDATE_FAILED = "ERROR_USERROLE_UPDATE_FAILED";
            public const string ERROR_CODE_EMPTY = "ERROR_CODE_EMPTY";
            public const string ERROR_PASSWORD_NEW_EMPTY = "ERROR_PASSWORD_NEW_EMPTY";
            public const string ERROR_PASSWORD_NEW_EXISTED = "ERROR_PASSWORD_NEW_EXISTED";
            public const string ERROR_CODE_EXISTED = "ERROR_CODE_EXISTED";
            public const string ERROR_PHONE_EXISTED = "ERROR_PHONE_EXISTED";
            public const string ERROR_POSITION_DELETE_FAILED = "ERROR_POSITION_DELETE_FAILED";
            public const string ERROR_APARTMENT_ALREADY_OWNER = "ERROR_APARTMENT_ALREADY_OWNER";
            public const string ERROR_LAMGUAGE_ALREADY_EXIST = "ERROR_LANGUAGE_ALREADY_EXIST";

            public const string ERROR_UTILITIES_NAME_EXISTED = "ERROR_UTILITIES_NAME_EXISTED";
            public const string ERROR_UTILITIES_NOT_EXIST = "ERROR_UTILITIES_NOT_EXIST";

            public const string LOG_DATA_OPT = "LOG_DATA_OPT";
            #endregion

            #region invoice-service
            public const string LOG_APARTMENT_NOT_EXIST_ONES = "LOG_APARTMENT_NOT_EXIST_ONES";
            public const string LOG_DATA_GETCONFIGPAYMENT = "LOG_DATA_GETCONFIGPAYMENT";
            public const string LOG_DATA_GETUSERMANAGERMENT = "LOG_DATA_GETUSERMANAGERMENT";
            public const string LOG_DATA_GETADMININFO = "LOG_DATA_GETADMININFO";
            public const string LOG_DATA_ONES = "LOG_DATA_ONES";
            public const string ERROR_CASHITEM_NOT_EXIST = "ERROR_CASHITEM_NOT_EXIST";
            public const string ERROR_PAYMENTHISTORY_NOT_EXIST = "ERROR_PAYMENTHISTORY_NOT_EXIST";
            public const string ERROR_PAYMENTHISTORY_DELETED = "ERROR_PAYMENTHISTORY_DELETED";
            public const string ERROR_CASH_NOT_EXIST = "ERROR_CASH_NOT_EXIST";
            public const string ERROR_CASH_CREATE_FAILED = "ERROR_CASH_CREATE_FAILED";
            public const string ERROR_CASH_UPDATE_FAILED = "ERROR_CASH_UPDATE_FAILED";
            public const string ERROR_CASHITEM_NULL = "ERROR_CASHITEM_NULL";
            public const string ERROR_CASHITEM_PAID = "ERROR_CASHITEM_PAID";
            public const string ERROR_CONFIGPAYMENT_NULL = "ERROR_CONFIGPAYMENT_NULL";
            public const string ERROR_CASHITEM_TOTALVALUE_NULL = "ERROR_CASHITEM_TOTALVALUE_NULL";
            public const string ERROR_ONES_LOGIN_FAILED = "ERROR_ONES_LOGIN_FAILED";
            public const string ERROR_GET_LISTFEE_FAILED = "ERROR_GET_LISTFEE_FAILED";

            public const string LOG_DATA_CREATECASH_ONES = "LOG_DATA_CREATECASH_ONES";
            public const string LOG_DATA_CREATECASH_SERVICE_ONLINE = "LOG_DATA_CREATECASH_SERVICE_ONLINE";
            public const string LOG_DATA_CREATECASH_SERVICE_DIRECTLY = "LOG_DATA_CREATECASH_SERVICE_DIRECTKY";
            public const string LOG_DATA_START_UPDATEPAYMENT_ONEPAY = "LOG_DATA_START_UPDATEPAYMENT_ONEPAY";
            public const string LOG_DATA_END_UPDATEPAYMENT_ONEPAY = "LOG_DATA_END_UPDATEPAYMENT_ONEPAY";

            public const string LOG_DATA_SYNC_CASH_FROM_ONES = "LOG_DATA_SYNC_CASH_FROM_ONES";
            public const string LOG_DATA_DELETE_BILL_FROM_ONES = "LOG_DATA_DELETE_BILL_FROM_ONES";
            public const string LOG_DATA_UPDATE_BILL_FROM_ONES = "LOG_DATA_UPDATE_BILL_FROM_ONES";

            #endregion

            #region require-service
            public const string ERROR_CONVERSATION_NOT_EXIST = "ERROR_CONVERSATION_NOT_EXIST";
            public const string ERROR_CONVERSATION_DELETED = "ERROR_CONVERSATION_DELETED";
            public const string ERROR_REQUIRESUPPORT_CREATE_FAILED = "ERROR_REQUIRESUPPORT_CREATE_FAILED";
            public const string ERROR_REQUIRESUPPORT_UPDATE_FAILED = "ERROR_REQUIRESUPPORT_UPDATE_FAILED";
            public const string ERROR_REQUIRESUPPORT_NOT_EXIST = "ERROR_REQUIRESUPPORT_NOT_EXIST";
            public const string ERROR_REQUIRESUPPORTHISTORY_NOT_EXIST = "ERROR_REQUIRESUPPORTHISTORY_NOT_EXIST";
            #endregion

            #region notification-service
            public const string ERROR_ACTION_NOT_EXIST = "ERROR_ACTION_NOT_EXIST";
            public const string ERROR_OPT_TIMEOUT = "ERROR_OPT_TIMEOUT";
            public const string ERROR_GET_OTP = "ERROR_GET_OTP";
            public const string ERROR_OPT_NOT_EXIST = "ERROR_OPT_NOT_EXIST";
            #endregion

            #region payment-service
            public const string ERROR_CONFIGPAYMENT_NOT_EXIST = "ERROR_CONFIGPAYMENT_NOT_EXIST";
            public const string ERROR_PAYMENT_HISTORY_NOT_EXIST = "ERROR_PAYMENT_HISTORY_NOT_EXIST";
            #endregion
        }
    }
}
