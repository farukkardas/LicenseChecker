using LicenseChecker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LicenseChecker
{
    public class AuthService
    {
        public async Task<ResponseModel> CheckKeyIsValid(string key,int appId,string secretKey)
        {
            var httpClient = new HttpClient();

            // Query request with all parameters
            var response = await httpClient.PostAsync($"http://localhost:7225/api/license/checklicense?keyLicense={key}&hwid={Program.GetHwid()}&appId={appId}&secretKey={secretKey}",null);

            ResponseModel responseModel = new ResponseModel();

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var deserializeContent =
                    JsonConvert.DeserializeObject<ResponseModel>(responseContent);

                if (deserializeContent != null)
                {
                    responseModel.Message = deserializeContent.Message;
                    responseModel.Success = deserializeContent.Success;
                }
            }

            return responseModel;
        }
    }
}