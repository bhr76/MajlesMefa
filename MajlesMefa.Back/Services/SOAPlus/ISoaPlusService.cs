using MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels;

namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus
{
    public interface ISoaPlusService
    {

        public Task<SoaPlusBaseResponse<SendSmsResponse>> SendSms(SendSmsRequest request);

        public Task<SoaPlusBaseResponse<GetShahkarInquiryResponse>> ShahkarInquiry(GetShahkarInquiryRequest request);

    }
}
