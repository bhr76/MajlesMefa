
namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus
{

    public interface ISoaPlusAuthorizationService
    {

        public Task<string> GetToken();

    }
}
