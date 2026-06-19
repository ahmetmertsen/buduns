using blogapp_server.Application.Dtos;
using blogapp_server.Application.UnitOfWork;
using MediatR;

namespace blogapp_server.Application.Features.Posts.Queries.GetDailyTopPosts
{
    public class GetDailyTopPostsQueryHandler : IRequestHandler<GetDailyTopPostsQuery, List<TopPostDto>>
    {
        private const int TopPostLimit = 50;
        private readonly IUnitOfWork _unitOfWork;

        public GetDailyTopPostsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TopPostDto>> Handle(GetDailyTopPostsQuery request, CancellationToken cancellationToken)
        {
            var turkeyTimeZone = GetTurkeyTimeZone();
            var nowInTurkey = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);
            var todayStartInTurkey = nowInTurkey.Date;
            var tomorrowStartInTurkey = todayStartInTurkey.AddDays(1);
            var startDateUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartInTurkey, turkeyTimeZone);
            var endDateUtc = TimeZoneInfo.ConvertTimeToUtc(tomorrowStartInTurkey, turkeyTimeZone);

            var topPosts = await _unitOfWork.PostRepository.GetDailyTopPostsAsync(startDateUtc, endDateUtc, TopPostLimit, cancellationToken);

            for (int i = 0; i < topPosts.Count; i++)
            {
                topPosts[i].Rank = i + 1;
            }

            return topPosts;
        }

        private static TimeZoneInfo GetTurkeyTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
            }
        }
    }
}
