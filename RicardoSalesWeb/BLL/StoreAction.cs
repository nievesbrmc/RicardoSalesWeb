using Newtonsoft.Json;

namespace RicardoSalesWeb.BLL
{
    public class StoreAction
    {
        private readonly DAL.DataAgents dataAgents;
        public StoreAction(IConfiguration configuration)
        {
            dataAgents = new DAL.DataAgents(configuration, "Stores");
        }

        public async Task<IEnumerable<Entity.StoreModel>> Get(int? Id)
        {
            List<Entity.StoreModel> response;
            try

            {
                string data = await dataAgents.ActionGet().ConfigureAwait(false);
                response = JsonConvert.DeserializeAnonymousType(data, new List<Entity.StoreModel>());
                if (Id.HasValue)
                {
                    response.RemoveAll(x => x.StoreId != Id);
                }
            }
            catch (Exception ex)
            {
                response = new List<Entity.StoreModel>();
            }
            return response;
        }

        public async Task<int> Create(Entity.StoreModel model)
        {
            try
            {

                string data = await dataAgents.ActionPost(model).ConfigureAwait(false);
                bool res = JsonConvert.DeserializeObject<bool>(data);
                if (res)
                {
                    data = await dataAgents.ActionGet().ConfigureAwait(false);
                    IEnumerable<Entity.StoreModel> list = JsonConvert.DeserializeAnonymousType(data, new List<Entity.StoreModel>());
                    Entity.StoreModel productNew = list.FirstOrDefault(x => x.Branch == model.Branch);
                    if (productNew != null)
                    {
                        return productNew.StoreId;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }

        public async Task<bool> updateDelete(Entity.StoreModel model, bool? delete)
        {
            try
            {
                string data = delete.HasValue && delete.Value ? await dataAgents.ActionDelete(model).ConfigureAwait(false) :
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
