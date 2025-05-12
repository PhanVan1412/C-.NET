using Microsoft.AspNetCore.Identity;
using OrdersManagement.BO.Auth;
using OrdersManagement.DAO.Auth;
using OrdersManagement.Services;

namespace OrdersManagement.BLL.Auth
{
    public class AuthBLL
    {

        private readonly AuthDAO _authDAO;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher<string> _passwordHasher;

        public AuthBLL(AuthDAO authDAO, JwtService jwtService)
        {
            _authDAO = authDAO;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<string>();
        }

        public async Task<LoginResultBO> LoginAsync(UserBO userLogin)
        {
            var userInfor = await _authDAO.GetUserByCredentialsAsync(userLogin);
            if (userInfor == null)
                return new LoginResultBO { IsSuccess = false, Message = "Tài khoản không tồn tại!" };

            var verifyResult = _passwordHasher.VerifyHashedPassword(null, userInfor.PasswordHash, userLogin.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return new LoginResultBO { IsSuccess = false, Message = "Sai mật khẩu!" };
            }

            var accessToken = _jwtService.GenerateToken(userInfor.UserId, userInfor.Permission);
            var refreshToken = _jwtService.GenarateRefreshToken();
            var refreshExpiry = DateTime.UtcNow.AddHours(20);

            await _authDAO.SaveRefreshTokenAsync(userInfor.UserId, refreshToken, refreshExpiry);

            return new LoginResultBO
            {
                IsSuccess = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> CreateUserAsync(RegisterUserBO userRegister)
        {
            try
            {
                if (userRegister == null)
                    throw new Exception("Vui lòng nhập thông tin để tạo tài khoản!");

                if (string.IsNullOrEmpty(userRegister.UserName))
                    throw new Exception("Tên tài khoản không được để trống!");

                if (string.IsNullOrEmpty(userRegister.Password))
                    throw new Exception("Mật khẩu không được để trống!");

                if (string.IsNullOrEmpty(userRegister.Email))
                    throw new Exception("Email không được để trống");

                // Hash mật khẩu
                string passwordHash = _passwordHasher.HashPassword(null, userRegister.Password);
                await _authDAO.CreateUserAsync(userRegister.UserName, userRegister.Email, passwordHash);

                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new Exception(message);
            }
        }
    }
}
