using OrdersManagement.BO.OrderBO;
using OrdersManagement.DAO.OrderDAO;

namespace OrdersManagement.BLL.OrderBLL
{
    public class OrderBLL
    {
        public async Task<List<OrderResponseBO>> GetOrderAsync(SearchParamBO seachParam)
        {
            OrderDAO orderDAO = new OrderDAO();
            return await orderDAO.GetOrderAsync(seachParam);
        }
    }
}
