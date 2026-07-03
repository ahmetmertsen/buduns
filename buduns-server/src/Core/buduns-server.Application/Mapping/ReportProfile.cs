using AutoMapper;
using buduns_server.Application.Dtos;
using buduns_server.Application.Common.Helpers;
using buduns_server.Domain.Entities;
using buduns_server.Domain.Enums;

namespace buduns_server.Application.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, ReportListDto>()
                .ForMember(destination => destination.ReporterUserName,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.UserName : null))
                .ForMember(destination => destination.ReporterFullName,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.FullName : null))
                .ForMember(destination => destination.TargetUserName,
                    options => options.MapFrom(source => source.TargetUser != null ? source.TargetUser.UserName : source.TargetType == ReportTargetType.User ? source.TargetOwnerUserNameSnapshot : null))
                .ForMember(destination => destination.TargetUserFullName,
                    options => options.MapFrom(source => source.TargetUser != null ? source.TargetUser.FullName : source.TargetType == ReportTargetType.User ? source.TargetOwnerFullNameSnapshot : null))
                .ForMember(destination => destination.TargetOwnerUserId,
                    options => options.MapFrom(source => source.TargetOwnerUserId ?? (source.TargetType == ReportTargetType.User ? source.TargetUserId : source.TargetType == ReportTargetType.Post && source.TargetPost != null ? source.TargetPost.UserId : source.TargetType == ReportTargetType.Comment && source.TargetComment != null ? source.TargetComment.UserId : null)))
                .ForMember(destination => destination.TargetOwnerUserName,
                    options => options.MapFrom(source => source.TargetOwnerUserNameSnapshot))
                .ForMember(destination => destination.TargetOwnerFullName,
                    options => options.MapFrom(source => source.TargetOwnerFullNameSnapshot))
                .ForMember(destination => destination.Priority,
                    options => options.MapFrom(source => ReportPriorityHelper.GetPriority(source.Reason)))
                .ForMember(destination => destination.TargetPostContentPreview, options => options.Ignore())
                .ForMember(destination => destination.TargetCommentContentPreview, options => options.Ignore())
                .ForMember(destination => destination.ReasonCounts, options => options.Ignore())
                .ForMember(destination => destination.ReportCount, options => options.Ignore())
                .ForMember(destination => destination.FirstReportDate, options => options.Ignore())
                .ForMember(destination => destination.LastReportDate, options => options.Ignore());

            CreateMap<Report, ReportDetailDto>()
                .ForMember(destination => destination.ReporterUserName,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.UserName : null))
                .ForMember(destination => destination.ReporterFullName,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.FullName : null))
                .ForMember(destination => destination.ReporterEmail,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.Email : null))
                .ForMember(destination => destination.TargetPostContent,
                    options => options.MapFrom(source => source.TargetPost != null ? source.TargetPost.Content : source.TargetContentSnapshot))
                .ForMember(destination => destination.TargetUserName,
                    options => options.MapFrom(source => source.TargetUser != null ? source.TargetUser.UserName : source.TargetType == ReportTargetType.User ? source.TargetOwnerUserNameSnapshot : null))
                .ForMember(destination => destination.TargetUserFullName,
                    options => options.MapFrom(source => source.TargetUser != null ? source.TargetUser.FullName : source.TargetType == ReportTargetType.User ? source.TargetOwnerFullNameSnapshot : null))
                .ForMember(destination => destination.TargetUserEmail,
                    options => options.MapFrom(source => source.TargetUser != null ? source.TargetUser.Email : null))
                .ForMember(destination => destination.TargetCommentContent, options => options.MapFrom(source => source.TargetComment != null ? source.TargetComment.Content : source.TargetContentSnapshot))
                .ForMember(destination => destination.TargetCommentUserId, options => options.MapFrom(source => source.TargetComment != null ? source.TargetComment.UserId : source.TargetType == ReportTargetType.Comment ? source.TargetOwnerUserId : null))
                .ForMember(destination => destination.TargetCommentUserName, options => options.MapFrom(source => source.TargetComment != null && source.TargetComment.User != null ? source.TargetComment.User.UserName : source.TargetType == ReportTargetType.Comment ? source.TargetOwnerUserNameSnapshot : null))
                .ForMember(destination => destination.TargetOwnerUserId,
                    options => options.MapFrom(source => source.TargetOwnerUserId ?? (source.TargetType == ReportTargetType.User ? source.TargetUserId : source.TargetType == ReportTargetType.Post && source.TargetPost != null ? source.TargetPost.UserId : source.TargetType == ReportTargetType.Comment && source.TargetComment != null ? source.TargetComment.UserId : null)))
                .ForMember(destination => destination.TargetOwnerUserName,
                    options => options.MapFrom(source => source.TargetOwnerUserNameSnapshot))
                .ForMember(destination => destination.TargetOwnerFullName,
                    options => options.MapFrom(source => source.TargetOwnerFullNameSnapshot))
                .ForMember(destination => destination.Priority,
                    options => options.MapFrom(source => ReportPriorityHelper.GetPriority(source.Reason)))
                .ForMember(destination => destination.ReviewedByUserName,
                    options => options.MapFrom(source => source.ReviewedByUser != null ? source.ReviewedByUser.UserName : null))
                .ForMember(destination => destination.CreatedDate,
                    options => options.MapFrom(source => source.CreatedAt))
                .ForMember(destination => destination.ReportCount, options => options.Ignore())
                .ForMember(destination => destination.RelatedReports, options => options.Ignore())
                .ForMember(destination => destination.ModerationActions, options => options.Ignore());

            CreateMap<Report, RelatedReportDto>()
                .ForMember(destination => destination.ReporterUserName,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.UserName : null))
                .ForMember(destination => destination.ReporterFullName,
                    options => options.MapFrom(source => source.ReporterUser != null ? source.ReporterUser.FullName : null))
                .ForMember(destination => destination.ReviewedByUserName,
                    options => options.MapFrom(source => source.ReviewedByUser != null ? source.ReviewedByUser.UserName : null));

            CreateMap<ModerationAction, ModerationActionDto>()
                .ForMember(destination => destination.ModeratorUserName,
                    options => options.MapFrom(source => source.ModeratorUser != null ? source.ModeratorUser.UserName : null));
        }
    }
}
