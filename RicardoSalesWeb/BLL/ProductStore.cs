using Newtonsoft.Json;

namespace RicardoSalesWeb.BLL
{
    public class ProductStore
    {
        private readonly DAL.DataAgents dataAgents;
        public ProductStore(IConfiguration configuration)
        {
            dataAgents = new DAL.DataAgents(configuration, "ProductStore");
        }
        public async Task<IEnumerable<Entity.ProductStoreModel>> Get(int? Id)
        {
            List<Entity.ProductStoreModel> response;
            try

            {
                string data = await dataAgents.ActionGet().ConfigureAwait(false);
                response = JsonConvert.DeserializeAnonymousType(data, new List<Entity.ProductStoreModel>());
                if (Id.HasValue)
                {
                    response.RemoveAll(x => x.StoreId != Id);
                }
            }
            catch (Exception ex)
            {
                response = new List<Entity.ProductStoreModel>();
            }
            return response;
        }
        public async Task<bool> Create(Entity.ProductStoreModel model)
        {
            try
            {

                string data = await dataAgents.ActionPost(model).ConfigureAwait(false);
                bool res = JsonConvert.DeserializeObject<bool>(data);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> updateDelete(Entity.ProductStoreModel model)
        {
            try
            {
                string data = await dataAgents.ActionDelete(model).ConfigureAwait(false);
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
