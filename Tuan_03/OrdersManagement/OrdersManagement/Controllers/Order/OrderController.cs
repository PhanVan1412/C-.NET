using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersManagement.BLL.OrderBLL;
using OrdersManagement.BO.OrderBO;

namespace OrdersManagement.Controllers.Order
{
    [ApiController]
    [Route("/api/v1/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetOrder([FromBody] SearchParamBO seachParam)
        {
           OrderBLL orderBLL = new OrderBLL();
           var listOrder = await orderBLL.GetOrderAsync(seachParam);
            return Ok(listOrder);
        }
    }
}
