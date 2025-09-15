using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Api;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.CreateActionReferenceCommand;
using MajlesMefa.Back.UseCases.Commmands.CreateMokatebeByApi;
using MajlesMefa.Back.UseCases.Commmands.LoginCommand;
using MajlesMefa.Back.Utilities.Captcha;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.Back.Utilities.Message;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Models.Auth;

namespace MajlesMefa.UI.Views.TaminEjtemayiAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class TaminApiController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public TaminApiController(IMapper mapper, IMediator mediator, IConfiguration configuration)
        {
            _mapper = mapper;
            _mediator = mediator;
            _configuration = configuration;
        }

        [RequestLimit(NoOfRequest = 15, Seconds = 10)]
        [HttpPost("getAccessToken")]
        public async Task<IActionResult> GetAccessToken(LoginApiVm loginVm)
        {

            try
            {
                var token = await _mediator.Send(new LoginCommand()
                {
                    Username = loginVm.Username,
                    Password = loginVm.Password,
                });

                var jsonRes = new
                {
                    accsessToken = new
                    {
                        token = token.AccessToken,
                        expireTime = DateTimeOffset.Now.AddSeconds(token.AccessTokenExpiresInSeconds)
                    },
                    refreshToken = new
                    {
                        token = token.RefreshToken,
                        expireTime = DateTimeOffset.Now.AddSeconds(token.RefresshTokenExpiresInSeconds)
                    }
                };
                return Json(jsonRes);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = ex.Message,
                    Status = StatusCodes.Status401Unauthorized,
                    Instance = HttpContext.Request.Path
                });
            }
            
        }


        [RequestLimit(NoOfRequest = 30, Seconds = 30)]
        [AuthApi]
        [HttpPost("addMokatebe")]
        public async Task<IActionResult> CreateAsync(MokatebeRequestDto request, CancellationToken cancellationToken)
        {
            
            var command = new CreateMokatebeByApiCommand
            {
                CategoryId = request.CategoryParentId,
                DataEntryData = request.MokatebeData as MokatebeInputDto,
                Description = request.Description,
                SenatorUUID = request.SenatorUUID,
                Title = request.Title,
            };



            try
            {
                MokatebeInputDto mokatebeInput = (MokatebeInputDto)command.DataEntryData;
                if (request.MokatebeData.PeygiriDateShamsi != null)
                {
                    mokatebeInput.PeygiriDate = request.MokatebeData.PeygiriDateShamsi.ToMiladiDate();
                }
                if (request.MokatebeData.PasokhDateShamsi != null)
                {
                    mokatebeInput.PasokhDate = request.MokatebeData.PasokhDateShamsi.ToMiladiDate();
                }
                mokatebeInput.TarikhDabirKhane = request.MokatebeData.TarikhDabirKhaneShamsi.ToMiladiDate();
                mokatebeInput.TarikhNameNamayande = request.MokatebeData.TarikhNameNamayandeShamsi.ToMiladiDate();

                //mokatebeInput.PasokhDate = request.PasokhDate.ToMiladiDate();
                command.DataEntryData = mokatebeInput;

                Guid dataEntryId = await _mediator.Send(command, cancellationToken);
                return Json(new { traceId = dataEntryId });
            }
            catch (ArgumentException ex)
            {

                return Json(new { message = ex.Message , code=400 });
            }
            catch (Exception ex) {
                return Json(new { message = ex.Message });
            }
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 30)]
        [AuthApi]
        [HttpPost("addMokatebePeygiri")]
        public async Task<IActionResult> CreateMokatebePeygiriAsync(PeygiriDto request, CancellationToken cancellationToken)
        {
            try
            {
                request.PeygiriKonandeId = new Guid(_configuration.GetSection("TaminPeygiriKonandeId").Value);
                var command = request.ConvertToCommand();
                var rslt = await _mediator.Send(command, cancellationToken);
                return Json(new { traceId = rslt });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 30)]
        [AuthApi]
        [HttpPost("UpdateMokatebeAnswer")]
        public async Task<IActionResult> CreateMokatebeAnswerAsync(MokatebeAnswerDto request, CancellationToken cancellationToken)
        {
            try
            {
                var command = request.ConvertToCommand();
                var rslt = await _mediator.Send(command, cancellationToken);
                return Json(new { traceId = rslt });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }


    }
}
