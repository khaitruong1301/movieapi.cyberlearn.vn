using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI
{
    public  class MessageConstant
    {
        public static string MESSAGE_ERROR_400 = "Yêu cầu không hợp lệ!";
        public static string MESSAGE_ERROR_401 = "Không có quyền truy cập!";
        public static string MESSAGE_ERROR_403 = "Không đủ quyền truy cập!";
        public static string MESSAGE_ERROR_404 = "Không tìm thấy tài nguyên!";
        public static string MESSAGE_ERROR_500 = "Dữ liệu không hợp lệ!";
        public static string MESSAGE_SUCCESS_200 = "Xử lý thành công!";
        public static string ERROR = "Dữ liệu không hợp lệ!";
        public static string BAD_REQUEST = "Không tìm thấy tài nguyên!";

        public static string INSERT_SUCCESS = "Thêm mới thành công!";
        public static string INSERT_ERROR = "Thêm mới thất bại!";

        public static string UPDATE_SUCCESS = "Cập nhật thành công!";
        public static string UPDATE_ERROR = "Cập nhật thất bại!";

        public static string DELETE_SUCCESS = "Xóa thành công!";
        public static string DELETE_ERROR = "Xóa thất bại!";

        public static string SIGNUP_SUCCESS = "Đăng ký tài khoản thành công!";
        public static string SIGNUP_ERROR = "Đăng ký tài khoản thất bại!";
        public static string EMAIL_EXITST = "Email đã được sử dụng!";
        public static string INFO_EXITST = "Email hoặc facebook đã được sử dụng!";
        public static string ACCOUNT_EXITST = "Tài khoản đã tồn tại!";
        public static string ACCOUNT_EXITST_TASK = "User đã được thêm vào task!";

        public static string SIGNIN_WRONG = "Sai email hoặc mật khẩu!";
        public static string SIGNIN_SUCCESS = "Đăng nhập thành công!";
        public static string SIGNIN_ERROR = "Đăng nhập thất bại!";

        public static string TOKEN_GENERATE_ERROR = "Không thể tạo token!";

        public static string UPLOAD_SUCCESS = "Upload thành công!";
        public static string UPLOAD_ERROR = "Upload thất bại!";



        public static string MESSAGE_NOTFOUND_GROUP = "Mã nhóm không hợp lệ !";

    }
}
