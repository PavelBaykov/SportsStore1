using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product,int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(x => x.Product.ProductID == product.ProductID);

            if (line == null)
            {
                lineCollection.Add(new CartLine() {Product = product,Quantity = quantity});
            }
            else
            {
                line.Quantity +=quantity;
            }
        }

        

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(x => x.Product.ProductID == product.ProductID);
        }
    }
}
