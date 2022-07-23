using Newtonsoft.Json;

namespace RicardoSalesWeb.BLL
{
    public class ProductBusiness
    {
        private readonly DAL.DataAgents dataAgents;
        public ProductBusiness(IConfiguration configuration)
        {
            dataAgents = new DAL.DataAgents(configuration, "Products");
        }
        public async Task<IEnumerable<Entity.ProductModel>> GetProducts(int? Id)
        {
            List<Entity.ProductModel> response;
            try

            {
                string data = await dataAgents.ActionGet().ConfigureAwait(false);
                response = JsonConvert.DeserializeAnonymousType(data, new List<Entity.ProductModel>());
                if (Id.HasValue)
                {
                    response.RemoveAll(x => x.ProductId != Id);
                }
            }
            catch (Exception ex)
            {
                response = new List<Entity.ProductModel>();
            }
            return response;
        }

        public async Task<int> Create(Entity.ProductModel model)
        {
            try
            {

                string data = await dataAgents.ActionPost(model).ConfigureAwait(false);
                bool res= JsonConvert.DeserializeObject<bool>(data);
                if (res)
                {
                    data = await dataAgents.ActionGet().ConfigureAwait(false);
                    IEnumerable<Entity.ProductModel> list = JsonConvert.DeserializeAnonymousType(data, new List<Entity.ProductModel>());
                    Entity.ProductModel productNew= list.FirstOrDefault(x=>x.Code == model.Code);
                    if (productNew!=null)
                    {
                        return productNew.ProductId;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }

        public async Task<bool> updateDelete(Entity.ProductModel model, bool? delete)
        {
            try
            {
                string data = delete.HasValue && delete.Value ? await dataAgents.ActionDelete(model).ConfigureAwait(false):
                    await dataAgents.ActionPut(model).ConfigureAwait(false);
                bool res = JsonConvert.DeserializeObject<bool>(data);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
