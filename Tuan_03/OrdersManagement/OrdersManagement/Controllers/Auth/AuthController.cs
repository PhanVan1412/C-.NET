using Microsoft.AspNetCore.Mvc;
using OrdersManagement.BLL.Auth;
using OrdersManagement.BO.Auth;

namespace OrdersManagement.Controllers.Auth
{
    [Route("/api/[Controller]/[Action]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthBLL _authBLL;

        public AuthController(AuthBLL authBLL)
        {
            _authBLL = authBLL;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserBO userLogin)
        {
            var token = await _authBLL.LoginAsync(userLogin);
            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserBO userRegister) 
        {
            var isSuccess = await _authBLL.CreateUserAsync(userRegister);
            return Ok("Tạo tài khoản thành công.");
        }
    }
}
