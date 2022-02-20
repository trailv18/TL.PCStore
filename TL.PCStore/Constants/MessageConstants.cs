namespace TL.PCStore.Constants
{
    public class MessageConstants
    {
        public const string CATEGORY_SUCCESS_CREATE = "Thêm loại sản phẩm thành công!";
        public const string CATEGORY_ERROR_CREATE = "Thêm loại sản phẩm thất bại!";
        public const string CATEGORY_SUCCESS_UPDATE = "Sửa loại sản phẩm thành công!";
        public const string CATEGORY_ERROR_UPDATE = "Sửa loại sản phẩm thất bại!";
        public const string CATEGORY_ERROR_DELETE = "Xóa loại sản phẩm thất bại!";
        public const string CATEGORY_SUCCESS_DELETE = "Xóa loại sản phẩm thành công!";
        public const string CATEGORY_INVALID_DELETE = "Không thể xóa vì tồn tại loại sản phẩm trong bảng sản phẩm!";
        public const string CATEGORY_DUPLICATE_NAME = "Tên loại sản phẩm bị trùng.";
        public const string CATEGORY_ERROR_NAME_LENTH = "Tên loại sản phẩm phải từ 2 - 255!";
        public const string CATEGORY_NOT_EXIST = "Không tồn tại loại sản phẩm";

        public const string BRAND_SUCCESS_CREATE = "Thêm thương hiệu thành công!";
        public const string BRAND_ERROR_CREATE = "Thêm thương hiệu thất bại!";
        public const string BRAND_SUCCESS_UPDATE = "Sửa thương hiệu thành công!";
        public const string BRAND_ERROR_UPDATE = "Sửa thương hiệu thất bại!";
        public const string BRAND_SUCCESS_DELETE = "Xóa thương hiệu thành công!";
        public const string BRAND_ERROR_DELETE = "Xóa thương hiệu thất bại!";
        public const string BRAND_INVALID_DELETE = "Không thể xóa vì tồn tại thương hiệu trong bảng sản phẩm!";
        public const string BRAND_DUPLICATE_NAME = "Tên thương hiệu bị trùng!";
        public const string BRAND_ERROR_NAME_LENTH = "Tên thương hiệu phải từ 2 - 255!";
        public const string BRAND_NOT_EXIST = "Không tồn tại thương hiệu!";

        public const string PRODUCT_SUCCESS_CREATE = "Thêm sản phẩm thành công!";
        public const string PRODUCT_ERROR_CREATE = "Thêm sản phẩm thất bại!";
        public const string INVALID_FILE = "Vui lòng chọn ảnh!";
        public const string PRODUCT_DUPLICATE_NAME = "Tên sản phẩm bị trùng!";
        public const string PRODUCT_SUCCESS_UPDATE = "Sửa sản phẩm thành công!";
        public const string PRODUCT_ERROR_UPDATE = "Sửa sản phẩm thất bại!";
        public const string PRODUCT_SUCCESS_DELETE = "Xóa sản phẩm thành công!";
        public const string PRODUCT_ERROR_DELETE = "Xóa sản phẩm thất bại!";
        public const string PRODUCT_INVALID_DELETE = "Không thể xóa vì tồn tại sản phẩm trong đơn hàng!";
        public const string PRODUCT_ERROR_NAME_LENTH = "Tên sản phẩm phải từ 5 - 255!";
        public const string PRODUCT_ERROR_PRICE = "Giá sản phẩm phải từ 10.000 - 100.000.000!";
        public const string PRODUCT_ERROR_STOCK = "Số lượng sản phẩm phải từ 1 - 1000!";
        public const string PRODUCT_NOT_EXIST = "Không tồn tại sản phẩm";

        public const string ROLE_DUPLICATE_NAME = "Tên quyền bị trùng!";
        public const string ROLE_ERROR_NAME_LENTH = "Tên quyền phải từ 2 - 255!";
        public const string ROLE_SUCCESS_CREATE = "Thêm quyền thành công!";
        public const string ROLE_ERROR_CREATE = "Thêm quyền thất bại!";
        public const string ROLE_SUCCESS_UPDATE = "Sửa quyền thành công!";
        public const string ROLE_ERROR_UPDATE = "Sửa quyền thất bại!";
        public const string ROLE_SUCCESS_DELETE = "Xóa quyền thành công!";
        public const string ROLE_ERROR_DELETE = "Xóa quyền thất bại!";
        public const string ROLE_INVALID_DELETE = "Không thể xóa quyền. Chỉ có thể ẩn!";

        public const string USER_ERROR_LOGIN = "Email hoặc mật khẩu không đúng!";
        public const string USER_DUPLICTE_EMAIL = "Email đã tồn tại!";
        public const string USER_ERROR_CREATE = "Tạo tài khoản thất bại!";
        public const string USER_ERROR_NAME_LENTH = "Họ và tên phải từ 5 - 255!";
        public const string USER_ERROR_EMAIL = "Email không đúng định dạng!";
        public const string USER_ERROR_PASSWORD = "Mật khẩu phải từ 6 - 12!";
        public const string USER_PASSWORD_NOTMATCH = "Mật khẩu xác nhận không trùng nhau!";


        public const string NEWS_SUCCESS_DELETE = "Xóa bài viết thành công!";
        public const string NEWS_NOT_EXIST = "Bài viết không tồn tại!";
        public const string NEWS_ERROR_DELETE = "Xóa bài viết thất bại";
        public const string NEWS_ERROR_NAME_LENTH = "Tiêu đề bài viết phải từ 10 - 255!";
    }
}