using CURD_Basic.BO.Product;
using System.Linq;

namespace CURD_Basic.BLL.Product
{
    public class ProductBLL
    {
        private static List<ProductBO> products = new List<ProductBO>();
        private static int _nextProductId = 1;
        public List<ProductBO> GetAllProduct()
        {
            return products;
        }

        public ProductBO GetProductByID(int productId)
        {
            return products.FirstOrDefault(x => x.ProductId == productId);
        }

        public bool CreateProduct(ProductBO product)
        {
            product.ProductId = _nextProductId++;
            products.Add(product);
            return true;
        }

        public bool UpdateProduct(int productId, ProductBO product)
        {
            var existing = products.FirstOrDefault(x => x.ProductId == productId);
            if (existing == null) return false;

            existing.ProductName = product.ProductName;
            existing.Price = product.Price;
            return true;
        }

        public bool DeleteProduct(int productId)
        {
            var product = products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null) return false;

            products.Remove(product);
            return true;
        }
    }
}
