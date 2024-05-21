using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.Commons.Enum
{
    public class AppEnum
    {
        public static string DEFAULT_AVATAR = "/default/avatar.png";

        public enum EntityStatus
        {
            /// <summary>
            /// All
            /// </summary>
            NA = 0,
            NORMAL = 1,
            OK = 2,
            NOT_OK = 3,
            TEMP = 10,
            LOCK = 98,
            DELETED = 99,
        }

        public enum TypeUser
        {
            ADMINISTRATOR = 1, //quản trị hệ thống
            MANAGEMENT = 2, //ban quản lý
            TECHNICIANS = 3, //kỹ thuật viên
            RESIDENT_MAIN = 4, //chủ nhà
            RESIDENT_GUEST = 5, //khách thuê
        }

        public enum TypeFunction    // Phân quyền chức năng với người dùng và nhóm quyền
        {
            FUNCTION_USER = 1, // Người dùng - Chức năng
            FUNCTION_ROLE = 2,    // Nhóm quyền - Chức năng
        }

        public enum Action
        {
            VIEW = 0,
            CREATE = 1,
            UPDATE = 2,
            DELETED = 3,
            IMPORT = 4,
            EXPORT = 5,
            PRINT = 6,
            EDIT_ANOTHER_USER = 7,
            MENU = 8
        }

        public enum TypeAction    // Hành động
        {
            ACTION = 1, // Hành động
            NOTIFICATION = 2, // Thông báo
            NOTIFICATION_REQUIRE_SUPPORT = 3, // Thông báo yêu cầu hỗ trợ
            NOTIFICATION_CHAT = 4, // Thông báo có tin nhắn chat
            ACTION_ON_APP = 5, //Hành động trên app
            NOTIFICATION_CASH = 6, //Thông báo phí
            NOTIFICATION_NEWS = 7, //Thông báo bản tin
            NOTIFICATION_REQUEST_GROUP = 8, //Thông báo cho phân đăng kí, cấp mới, hủy, cấp lại thẻ
            NOTIFICATION_REG_CONSTRUCTION = 9, //Thông báo cho đăng ký thi công
            NOTIFICATION_TRANSPOST_REQ = 10, //Thông báo cho đăng ký chuyển hàng hóa vào ra
            NOTIFICATION_SERVICE_TECH = 11, //Thông báo cho đăng ký dịch vụ kỹ thuật
            NOTIFICATION_SERVICE_RENTAL = 12, // Thong bao cho dang ky dich vu cho thue / ban can ho
            NOTIFICATION_SERVICE_CLEAN = 13, // Thong bao cho dang ky dich vu ve sinh
        }

        public enum TypeResident
        {
            RESIDENT_MAIN = 1, //Chủ nhà
            RESIDENT_GUEST = 2, //Khách thuê
            RESIDENT_MEMBER = 3, // Thành viên căn hộ
            RESIDENT_GUEST_MEMBER = 4 //Thành viên khách thuê
        }

        public enum TypeRegister    // hình thức đăng ký
        {
            EMAIL = 1, // đăng ký bằng email
            PHONE_NUMBER = 2,    // đăng ký bằng sdt
            FACEBOOK = 3, // đăng ký bằng face
        }

        public enum PaymentStatus    // trang thái thanh toán
        {
            INIT = 1, // chưa thanh toán
            FULL = 2,    // đã thanh toán hết
            NOT_ENOUGH = 3,    // chưa thanh toán hết
            NOT_PAYMENT = 4,     // không thanh toán
            ERROR_PAYMENT = 5     // thanh toán lỗi
        }

        public enum PaymentMethod    // trang thái thanh toán
        {
            NOTPAY = 108, // ko thanh toán
            COD = 100, // ko thanh toán
            VIETELPAY = 99, // ko thanh toán
            ONEPAY = 92,    // one pay
            ONEPAY_OUT_QR = 91,    // paypal
            ONEPAY_OUT_AE_OUT = 90,    // wechat pay
            ONEPAY_OUT_AE_IN = 89,     // alipay
            ONEPAY_OUT = 88,     // onepay quốc tế
            ONEPAY_IN = 87,     // onepay nội địa
            ONEPAY_OUT_IN = 86,     // momo
            VNPAY = 85,     // vnpay
            ONES = 84,       //Thu từ bên OneS
            DIRECT = 80,      // thanh toán trực tiếp
            MSB = 109 //MSB
        }

        public enum FileType
        {
            IMAGE = 1,
            FILE = 2
        }

        public enum TypeEmployee
        {
            MANAGER = 1, //ban quản lý
            USER = 2, //nhân viên
        }

        public enum TypeDepartment
        {
            MANAGEMENT = 1, //Ban quản lý tòa nhà
            GROUP = 2 //Tổ, nhóm
        }

        public enum TypeAttactment
        {
            ATTACTMENT_REQUIRE_SP = 1, //Ảnh đính kèm với Yêu cầu hỗ trợ
            ATTACTMENT_NOTIFICATION = 2, //File đính kèm với thông báo
            ATTACTMENT_REQUIRE_SP_HISTORY = 3, //Ảnh đính kèm với Xử lý yêu cầu hỗ trợ
            ATTACTMENT_NEWS = 4, //File đính kèm với bài viết sổ tay cư dân, bảng tin , tin tức
            ATTACTMENT_VEHICLE_PARKING = 5,  //File đính kèm với thông tin đăng ký xe
            ATTACTMENT_SERVICE_PLUS = 6     //File đính kèm với thông tin đăng kí dịch vụ
        }

        public enum TypeNoteBook
        {
            NOTEBOOK = 1, //Bài viết sổ tay cư dân
            NEWS_RESIDENT = 2, //Bài viết bảng tin
            NEWS = 3, //Tin tức
            ENDOW = 4, // Ưu đãi cư dân
            PROMOTION = 5,   //Khuyến mãi
            NOTIFICATION = 6 //Tin thông báo
        }

        public enum TypeNotification
        {
            NOTIFICATION_PROJECT = 1, //Thông báo tới khu đô thị
            NOTIFICATION_TOWER = 2, //Thông báo tới tòa nhà
            NOTIFICATION_FLOOR = 3, //Thông báo tới tầng
            NOTIFICATION_APARTMENT = 4, //Thông báo tới căn hộ
            NOTIFICATION_RESIDENT = 5 //Thông báo tới cư dân cụ thể
        }

        public enum TypeLanguage
        {
            VIETNAMESE = 1,
            ENGLISH = 2
        }

        //Loại trạng thái xử lý yêu cầu hỗ trợ
        public enum TypeStatusRequireSupport
        {
            PENDING = 1, //Chờ tiếp nhận
            RECEIVE_AND_ASSIGN = 2, // Tiếp nhận và phân công
            HANDLING = 3, // Đang xử lý
            COMPLETE = 4, // Hoàn thành
            CANCEL = 5 // Hủy
        }

        //Loại lịch sử xử lý yêu cầu hỗ trợ
        public enum TypeRequireSupportHistory
        {
            ASSIGNED = 1, // bàn giao cho người khác hỗ trợ yêu cầu
            HANDLE = 2, //Xử lý
            CANCEL = 3 // cư dân hủy yêu cầu
        }

        public enum ResidentIdentifyType
        {
            NONE = 0,
            CCCD = 1,
            CMTND = 2,
            PASSPORT = 3,
        }

        public enum TypeFCM
        {
            NEW_MESSAGE = 1, //tin nhắn
            NEW_EMAIL = 2, //email
            NEW_REPLY = 3, //trả lời
            NEW_COMMENT = 4, //bình luận
            NEW_REQUIRE = 5, //yêu cầu
            NEW_CASH = 6, //thanh toán
            NEW_NEWS = 7, //bản tin
        }

        public enum ServiceOrderStatus
        {
            PENDING = 1, //Đã gủi yêu cầu dịch vụ kỹ thuật
            RECEIVE = 2, // Bộ phận kỹ thuật đang xử lý
            CONFIRM_SURVEY = 3, // Chờ xác nhận lịch khảo sát
            HANDLING_SURVEY = 4, // Thực hiện khảo sát
            CONFIRM_FEEING = 5, // Chờ xác nhận phí
            //CONFIRM_FEED = 6, // Đã xác nhận phí
            HANDLING = 7, // Thực hiện dịch vụ
            CONFIRM_COMPLETE = 8, // Xác nhận hoàn thành
            COMPLETE = 9, // Hoàn thành
                          //COMPLETE_PAY = 10, // hoàn thành thanh toán
                          //CONFIRM_PAY = 11, // hoàn thành thanh toán
                          //CANCEL = 12, // Hủy phiếu

        }

        public enum ServiceOrderStep
        {
            PENDING = 1, //Đã gủi yêu cầu dịch vụ kỹ thuật
            DATE_SURVEY = 2, // Bộ phận kỹ thuật đưa ra ngày khảo sát
            CONFIRM_SURVEY = 3, // Cư dân xác nhận lịch khảo sát
            HANDLING_SURVEY = 4, // Kỹ thuật thực hiện khảo sát
            CONFIRM_FEEING = 5, // Cư dân chờ xác nhận phí
            CONFIRM_FEED = 6, // Cư dân xác nhận thanh toán
            HANDLING = 7, // Thực hiện dịch vụ
            CONFIRM_COMPLETE = 8, // Xác nhận hoàn thành
            COMPLETE = 9, // Hoàn thành
            COMPLETE_PAY = 10, // Hủy phiếu
        }

        public enum ServiceOrderType
        {
            MP = 1, //Miễn phí
            CP = 2, // Có phí
            MP_CP = 3, // Cả 2
        }

        public enum ServiceMapType
        {
            ITEM = 1, //dịch vụ
            PRODUCT = 2, // vật tư
            PRICE = 3, // báo giá
        }

        //Trạng thái đơn đăng ký thi công
        public enum RegConstructionStatus
        {
            PENDING = 1,                    //Đăng ký mới
            REQUEST_ADD_INFORMATION = 2,    //Bổ sung thông tin
            UNAPPROVED = 3,                 //Không phê duyệt đăng ký
            APPROVED = 4,                   //Phê duyệt đăng ký
            COMPLETE = 5                    //Hoàn thành
        }

        //Trạng thái thi công
        public enum ConstructionStatus
        {
            IN_PROCESS = 1,          //Đang tiến hành thi công
            PAUSE = 2,              //Tạm dừng thi công
            CONTINUE = 3,              //Tạm dừng thi công
        }

        //Loại hồ sơ đăng ký thi công
        public enum TypeRegisterConstructionDoc
        {
            CONSTRUCTION_DRAWING = 1,   //Bản vẽ thi công
            OTHER = 2                   //Hồ sơ khác
        }

        //Trạng thái hồ sơ đăng ký thi công
        public enum RegisterConstructionDocStatus
        {
            PENDING = 1,                    //Đăng ký mới
            UNAPPROVED = 2,                 //Không phê duyệt đăng ký
            APPROVED = 3                   //Phê duyệt đăng ký
        }

        //Trạng thái đơn đăng ký nghiệm thu thi công
        public enum ConstructionAcceptanceFormStatus
        {
            PENDING = 1,                    //Đăng ký mới
            UNAPPROVED = 2,                 //Không phê duyệt đăng ký
            APPROVED = 3,                   //Phê duyệt đăng ký
            COMPLETE = 4                    //Hoàn thành
        }

        //Trạng thái hoàn cọc
        public enum RefundDepositStatus
        {
            NOT_YET = 1,                //Chưa hoàn cọc
            ALREADY = 2                 //Đã hoàn cọc
        }

        //Trạng thái đặt cọc
        public enum DepositStatus
        {
            NOT_YET = 1,                //Chưa đặt cọc
            ALREADY = 2                 //Đã đặt cọc
        }

        //Trạng thái nhận tiền cọc
        public enum ReceiveMoneyStatus
        {
            NOT_YET = 1,                //Chưa nhận tiền
            ALREADY = 2                 //Đã nhận tiền
        }

        public enum DepositType
        {
            ONLINE = 1,                 //Thanh toán online
            DIRECT = 2                  //Thanh toán trực tiếp
        }

        //Trạng thái nhà thầu
        public enum ContractorStatus
        {
            PENDING = 1,                    //Đăng ký mới
            UNAPPROVED = 2,                 //Không phê duyệt đăng ký
            APPROVED = 3                   //Phê duyệt đăng ký
        }

        //Loại map của hồ sơ đăng ký thi công
        public enum TypeRegisterConstructionDocMapping
        {
            REGISTER_CONSTRUCTION_FORM = 1, //Map với Đơn đăng ký thi công
            EXTENSION_CONSTRUCTION_FORM = 2  //Map với Đơn gia hạn thi công
        }

        public enum ExtensionConstructionStatus
        {
            PENDING = 1,                    //Trạng thái cư dân gửi đăng ký
            UNAPPROVED = 2,                 //Không Duyệt đơn đăng kí
            APPROVED = 3,                   //Duyệt đơn đăng kí
        }

        //Loại hóa đơn thanh toán tiền cọc cho dịch vụ, tiện ích
        public enum TypeCash
        {
            CARD_REGISTER = 1,              //Tiện ích đăng ký thẻ
            TRANSPOST_REQUEST = 2,          //Tiện ích chuyển hàng hóa vào ra
            CONSTRUCTION_REGISTER = 3,      //Đặt cọc Tiện ích đăng ký thi công
            CONSTRUCTION_REGISTER_DEPOSIT_EXTEND = 7,      //Đặt cọc bổ sung Tiện ích đăng ký thi công
            SERVICE_TECH = 4,               //Dịch vụ kỹ thuật 
            SERVICE_TECH_DEPOSIT = 8,               //Đặt cọc dịch vụ kỹ thuật 
            CLEANING_SERVICE = 5,           //Dịch vụ vệ sinh
            RENTAL_APARTMENT = 6            //Dịch vụ cho thuê nhà
        }

        public enum SlaServiceId
        {
            ChoThue = 1,
            KyThuat = 2,
            TheCuDanVeO = 4,
            ThiCong = 5,
            NghiemThu_ThiCong = 6,
            VeSinh = 7,
            ChuyenHangHoaVaoRa = 8,
            ThoiGianTrungBinhXuLyYeuCau = 9,
            ThoiGianTrungBinhPhanHoiYeuCau = 10
        }
        public enum SlaServiceDetailStep
        {
            /// <summary>
            /// for search all
            /// </summary>
            NA = 0,
            #region Cho thue
            /// <summary>
            /// Đăng ký mới => 4h phải chuyển sang trạng thái khác
            /// </summary>
            ChoThue_Step1 = 1,
            /// <summary>
            /// Đang xử lý => 4h phải chuyển sang trạng thái khác
            /// </summary>
            ChoThue_Step2 = 2,
            /// Từ lúc Trạng thái thanh toán phí dịch vụ là "Đã thanh toán" dịch vụ cho thuê/bán nhà phải đổi sang trạng thái Hoàn thành
            ChoThue_Step3 = 3,
            #endregion

            #region Ve sinh
            VeSinh_Step1 = 1, //them moi
            VeSinh_Step2 = 2, //C-One đang sử lí
            VeSinh_Step3 = 3, //Thuc hien khao sat
            VeSinh_Step4 = 4, // đã thanh toán
            #endregion

            #region ChuyenHangHoa
            ChuyenHangHoa_Step1 = 1, // Thêm mới
            ChuyenHangHoa_Step2 = 2, // Đang sử lí
            ChuyenHangHoa_Step3 = 3, // Đã hoàn thành
            #endregion

            #region TheCuDanVeO
            TheCuDanVeO_Step1 = 1, // Thêm mới
            TheCuDanVeO_Step2 = 2, // Đang sử lí
            TheCuDanVeO_Step3 = 3, // Phê duyệt đăng kí
            TheCuDanVeO_Step4 = 4, // Yêu cầu đóng phí
            TheCuDanVeO_Step5 = 5, // Chờ cấp thẻ
            TheCuDanVeO_Step6 = 6, // Chờ bàn giao
            #endregion

            #region Ky thuat
            KyThuat_Step1 = 1,
            KyThuat_Step2 = 2,
            KyThuat_Step4 = 4,
            KyThuat_Step11 = 11,
            #endregion

            #region Thi Công
            ThiCong_Step1 = 1, // Đăng ký mới => Phải chuyển trạng thái khác
            ThiCong_Step2 = 2,   // Đăng ký mới => Phản hồi lại kết quả đơn là phê duyệt hay không phê duyệt
            #endregion

            #region Nghiệm thu thi công
            NghiemThu_ThiCong_Step1 = 1, // Đăng ký mới => Phản hồi lại kết quả đơn là phê duyệt hay không phê duyệt
            #endregion
        }

        public enum SlaServiceLogType
        {
            /// <summary>
            /// for search all
            /// </summary>
            NA = -1,
            None = 0,
            Good = 1,
            Warning = 2,
            Serious = 3
        }
    }
}
