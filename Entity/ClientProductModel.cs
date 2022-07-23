using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ClientProductModel
    {
        public int ClientProductId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public DateTime? DateInserted { get; set; }
    }
    public class SalesActions
    {
        public ClientModel? ClientModel { get; set; }
        public ProductModel? ProductModel { get; set; }
        public int? Quantity { get; set; }

    }
}
