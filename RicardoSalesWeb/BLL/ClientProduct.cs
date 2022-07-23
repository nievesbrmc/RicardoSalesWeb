using Newtonsoft.Json;

namespace RicardoSalesWeb.BLL
{
    public class ClientProduct
    {
        private readonly DAL.DataAgents dataAgents;
        public ClientProduct(IConfiguration configuration)
        {
            dataAgents = new DAL.DataAgents(configuration, "ClientProduct");
        }

        public async Task<IEnumerable<Entity.ClientProductModel>> Get(int? Id)
        {
            string data = await dataAgents.ActionGet().ConfigureAwait(false);
            List<Entity.ClientProductModel> dataList = JsonConvert.DeserializeAnonymousType(data, new List<Entity.ClientProductModel>());
            return dataList;
        }
        public async Task<bool> Create(Entity.ClientProductModel model)
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
        public async Task<bool> updateDelete(Entity.ClientProductModel model)
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
