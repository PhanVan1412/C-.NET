using Npgsql;
using OrdersManagement.BO.OrderBO;
using OrdersManagement.DAO.Global;

namespace OrdersManagement.DAO.OrderDAO
{
    public class OrderDAO : BaseDAO
    {
        public async Task<List<OrderResponseBO>> GetOrderAsync(SearchParamBO searchParam)
        {
            return await ExecStoreToObjectAsync<OrderResponseBO>(new List<object> { searchParam.FromDate, searchParam.ToDate, searchParam.Status, searchParam.PageIndex, searchParam.PageSize }, "orders.get_orders_shr");
        }
    }
}
