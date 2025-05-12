namespace OrdersManagement.BO.Auth
{
    public class UserBO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string? Permission { get; set; }
        public int? RoleId { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
    }

    public class RegisterUserBO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }

    public class RefreshTokenBO
    {
        /// <summary>
        /// Giá trị chuỗi refresh token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Thời điểm hết hạn token
        /// </summary>
        public DateTime Expires { get; set; }
        /// <summary>
        /// Token có còn hợp lệ theo thời gian không
        /// </summary>
        public bool IsExpired => DateTime.UtcNow >= Expires;
        /// <summary>
        /// Thời điểm token được tạo ra
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Đánh dấu token bị vô hiệu hóa (nếu có)
        /// </summary>
        public DateTime? Revoked { get; set; }
        /// <summary>
        /// Token còn hợp lệ để sử dụng hay không
        /// </summary>
        public bool IsActive => Revoked == null && !IsExpired;
    }

    public class TokenBO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }


    public class LoginResultBO
    {
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
 
}
