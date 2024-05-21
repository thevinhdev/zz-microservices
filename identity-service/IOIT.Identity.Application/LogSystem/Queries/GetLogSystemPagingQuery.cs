using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Application.LogSystem.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace IOIT.Identity.Application.LogSystem.Queries
{
    public class GetLogSystemPagingQuery : FilterPagination, IRequest<ResTotal>
    {
        public string ApiGateway { get; set; }
        public string Token { get; set; }

        public class Handler : IRequestHandler<GetLogSystemPagingQuery, ResTotal>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private IHttpClientFactory _factory;
            private readonly ILogger<GetLogSystemPagingQuery> _logger;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IHttpClientFactory factory,
                ILogger<GetLogSystemPagingQuery> logger)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _factory = factory;
                _logger = logger;
            }

            public async Task<ResTotal> Handle(GetLogSystemPagingQuery request, CancellationToken cancellationToken)
            {
                List<LogSystemRes> logs = new List<LogSystemRes>();
                int total = 0;

                try
                {
                    HttpClient client = _factory.CreateClient();

                    string url = $"{request.ApiGateway}api/cms/action/getLogFromNotiService?page={request.page}&page_size={request.page_size}&query={request.query}&order_by={request.order_by}";

                    var requestGetUser = new HttpRequestMessage(HttpMethod.Get, url);
                    requestGetUser.Headers.Add("Accept", "application/json");
                    requestGetUser.Headers.Add("Authorization", request.Token);
                    var responseGetUser = await client.SendAsync(requestGetUser);

                    if (responseGetUser.IsSuccessStatusCode)
                    {
                        var contentGetUser = responseGetUser.Content.ReadAsStringAsync().Result;
                        var json = JsonConvert.DeserializeObject<dynamic>(contentGetUser);

                        if (json["data"] != null) 
                        {
                            logs = json["data"].ToObject<List<LogSystemRes>>();
                            total = json["metadata"];
                        }
                    }
                }
                catch (Exception ex) { _logger.LogError(ex.Message); }

                foreach (var log in logs)
                {
                    var user = await _userRepo.GetByKeyAsync((long)log.UserId);
                    if (user != null)
                    {
                        log.FullName = user.FullName;
                        log.UserName = user.UserName;
                    }
                }

                ResTotal resTotal = new ResTotal();
                resTotal.data = logs;
                resTotal.total = total;

                return resTotal;
            }

            public async Task<List<LogSystemRes>> GetLog(GetLogSystemPagingQuery rq)
            {
                List<LogSystemRes> data = new List<LogSystemRes>();

                

                return data;
            }
        }
    }
}
