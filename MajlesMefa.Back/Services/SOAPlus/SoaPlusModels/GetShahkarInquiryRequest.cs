

namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus.SoaPlusModels
{
    public class GetShahkarInquiryRequest
    {
        public GetShahkarInquiryRequest(int identificationType, string identificationNo, string mobileNo)
        {
            this.identificationType = identificationType;
            this.identificaionNo = identificationNo;
            this.mobileNo = mobileNo;
        }

        public int identificationType { get; set; }
        public string identificaionNo { get; set; }

        public string mobileNo { get; set; }

    }
}
