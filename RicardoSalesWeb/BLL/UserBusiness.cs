using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace RicardoSalesWeb.BLL
{
    public class UserBusiness
    {
        private readonly DAL.DataAgents dataAgents;
        public UserBusiness(IConfiguration configuration) 
        {
            dataAgents = new DAL.DataAgents(configuration, "Clients");
        }
        public async Task<Entity.ClientModel> Login(string usr, string psw)
        {
            try
            {
                string data = await dataAgents.ActionGet().ConfigureAwait(false);
                List<Entity.ClientModel> users = JsonConvert.DeserializeAnonymousType(data, new List<Entity.ClientModel>());
                return users.FirstOrDefault(x => x.Email == usr && x.Password == psw);
            }
            catch (Exception ex)
            {
                return new Entity.ClientModel();
            }
        }

        public async Task<bool> Register(Entity.ClientModel model)
        {
            try
            {
                string data = await dataAgents.ActionPost(model).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<bool>(data);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> Update(Entity.ClientModel model, bool? delete)
        {
            try
            {
                string data = delete.HasValue && delete.Value ? await dataAgents.ActionDelete(model).ConfigureAwait(false): await dataAgents.ActionPut(model).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<bool>(data);
            }
            catch (Exception err)
            {
                return false;
            }
        }

        public async Task<List<Entity.ClientModel>> GetClients([Optional] int? Id)
        {
            List<Entity.ClientModel> response;
            try

            {
                string data = await dataAgents.ActionGet().ConfigureAwait(false);
                response = JsonConvert.DeserializeAnonymousType(data, new List<Entity.ClientModel>());
                if (Id.HasValue)
                {
                    response.RemoveAll(x => x.ClientId != Id);
                }
            }
            catch (Exception ex)
            {
                         response = new List<Entity.ClientModel>();
            }
            return response;
        }
    }
}
