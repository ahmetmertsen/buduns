using AutoMapper;
using blogapp_server.Application.Dtos;
using blogapp_server.Application.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Features.Followers.Queries.GetAllByUserId
{
    public class GetAllFollowersByUserIdQueryHandler : IRequestHandler<GetAllFollowersByUserIdQuery, List<FollowerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllFollowersByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<FollowerDto>> Handle(GetAllFollowersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var followers = await _unitOfWork.FollowerRepository.GetAllFollowersByUserIdAsync(request.UserId);
            var response = _mapper.Map<List<FollowerDto>>(followers);
            return response;
        }
    }
}
