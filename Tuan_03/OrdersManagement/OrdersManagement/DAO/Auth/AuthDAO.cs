using OrdersManagement.BO.Auth;
using OrdersManagement.DAO.Global;

namespace OrdersManagement.DAO.Auth
{
    public class AuthDAO : BaseDAO
    {
        public async Task<UserBO> GetUserByCredentialsAsync(UserBO userLogin)
        {
            var userInfors = await ExecStoreToObjectAsync<UserBO>(new List<object> { userLogin.UserName }, "orders.get_userinfor");
            return userInfors.FirstOrDefault();
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime refreshExpiry)
        {
            await ExecStoreNoneQueryAsync(new List<object> { userId, refreshToken, refreshExpiry }, "orders.update_user_token");
        }

        public async Task CreateUserAsync(string userName, string email, string passwordHash)
        {
            await ExecStoreNoneQueryAsync(new List<object> { userName, email, passwordHash }, "orders.insert_user");
        }
    }
}
