using
    Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;
using System.Dynamic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus
{

    public class SoaPlusService : ISoaPlusService
    {
        private HttpClient _httpClient;
        
        public SoaPlusService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        private async Task<T> SendPostRequest<T>(object model, string url)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            var jsonDeserialize = JsonConvert.DeserializeObject<ExpandoObject>(jsonData);
            var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var finalJson = JsonConvert.SerializeObject(jsonDeserialize, camelSettings);

            var dataContent = new StringContent(finalJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, dataContent);
            var result = await response.Content.ReadAsStringAsync();
           // _logger.LogDebug(result);
            return JsonConvert.DeserializeObject<T>(result);
        }


		public async Task<SoaPlusBaseResponse<SendSmsResponse>> SendSms(SendSmsRequest request)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(request);
                var jsonDeserialize = JsonConvert.DeserializeObject<ExpandoObject>(jsonData);
                var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var finalJson = JsonConvert.SerializeObject(jsonDeserialize, camelSettings);

                var dataContent = new StringContent(finalJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("public/1.0/send-sms-integrate", dataContent);
                var result = await response.Content.ReadAsStringAsync();
                var finalRes = JsonConvert.DeserializeObject<SendSmsResponse>(result);
                //_logger.LogDebug(finalRes.ToString());
                if (finalRes.ErrorModel.ErrorCode != 0)
                {
                    //_logger.LogError("در حال حاضر امکان ارسال پیامک وجود ندارد.");
                    return new SoaPlusBaseResponse<SendSmsResponse>(DateTime.Now, false, null, "در حال حاضر امکان ارسال پیامک وجود ندارد.");
                }
                return new SoaPlusBaseResponse<SendSmsResponse>(DateTime.Now, true, null, "پیامک با موفقیت ارسال شد.");
            }
            catch (Exception)
            {
                //_logger.LogError("خطا در فراخوانی سرویس ارسال پیامک");
                return new SoaPlusBaseResponse<SendSmsResponse>(DateTime.Now, false, null, "خطا در فراخوانی سرویس ارسال پیامک");
            }
        }

        public async Task<SoaPlusBaseResponse<GetShahkarInquiryResponse>> ShahkarInquiry(GetShahkarInquiryRequest request)
        {
            try
            {
                var result = await SendPostRequest<SoaPlusBaseResponse<GetShahkarInquiryResponse>>(request, "gov/1.0/ShahkarService");
                if (!result.Done || result.Result == null)
                {
                    return new SoaPlusBaseResponse<GetShahkarInquiryResponse>(DateTime.Now, false, null, "کد ملی و شماره موبایل وارد شده مطعلق به یک نفر نیست. ");
                }
                return result;
            }
            catch (Exception Ex)
            {

                return new SoaPlusBaseResponse<GetShahkarInquiryResponse>(DateTime.Now, false, null, "خطا در فراخوانی سرویس استعلام شاهکار");
            }
        }

     

        //public async Task<SoaPlusBaseResponse<List<GetBranchListResponseDto>>> GetAllBranches()
        //{
        //    var result = await SendPostRequest<List<GetBranchListResponseDto>>(null, "api/1/get-branch");
        //    return result;
        //}

        //public async Task<SoaGetTokenResponse> GetAllBranches(SoaGetTokenRequest request)
        //{
        //    var result = await SendPostRequest<SoaGetTokenResponse>(request, "api/1/get-branch");
        //    return result;
        //}
    }
}
