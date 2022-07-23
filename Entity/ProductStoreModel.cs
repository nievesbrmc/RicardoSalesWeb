using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProductStoreModel
    {
        public int ProductStoreId { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public DateTime? DateInserted { get; set; }
    }

    public class ProductStoreActions
    {
        public ProductModel? ProductModel { get; set; }
        public StoreModel? StoreModel { get; set; }
    }
}
