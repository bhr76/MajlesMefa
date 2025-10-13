using System.Net.Http.Headers;

namespace MajlesMefa.Core.ApplicationService.Services.SOAPlus
{
    public class SoaPlusHttpClientRequestDelegatingHandler
      : DelegatingHandler
    {
        private readonly ISoaPlusAuthorizationService _authorizationService;

        public SoaPlusHttpClientRequestDelegatingHandler(ISoaPlusAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //if (request.Method == HttpMethod.Post)
            //{
            var token = await _authorizationService.GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //}
            return await base.SendAsync(request, cancellationToken);
        }
    }
}

