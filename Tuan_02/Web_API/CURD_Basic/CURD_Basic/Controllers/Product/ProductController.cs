using CURD_Basic.BLL.Product;
using CURD_Basic.BO.Product;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace CURD_Basic.Controllers.Product
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            ProductBLL productBLL = new ProductBLL();
            var products = productBLL.GetAllProduct();
            if (products == null)
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public IActionResult GetProductByID(int productId) 
        {
            ProductBLL productBLL = new ProductBLL();
            var product = productBLL.GetProductByID(productId);
            if (product == null) 
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductBO product)
        {
            ProductBLL productBLL = new ProductBLL();
            var result = productBLL.CreateProduct(product);
            if (!result)
                return BadRequest("Không thể thêm sản phẩm");
            return CreatedAtAction(nameof(GetProductByID), new { productId = product.ProductId }, product);
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int productId, ProductBO product)
        {

            ProductBLL productBLL = new ProductBLL();
            var result = productBLL.UpdateProduct(productId, product);
            if (!result)
                return BadRequest("Lỗi không thể câp nhật sản phẩm!");
            return Ok();
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId) 
        {
            ProductBLL productBLL = new ProductBLL();
            var result = productBLL.DeleteProduct(productId);
            if (!result)
                return BadRequest("Lỗi không thể xóa sản phẩm!");
            return Ok();
        }
    }
}
