using AutoMapper;
using hrvster.Games.Match.Models;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;

namespace hrvster.Web.Mapper
{
	public class EntityMap : Profile
	{
		public EntityMap()
		{
			CreateMap<hrvster.Models.Campaign, Promotion>()
				.ForMember(d => d.PromotionType, o => o.MapFrom(s => s.CampaignType))
				.ReverseMap();

			CreateMap<hrvster.Models.CampaignType, PromotionType>()
				.ReverseMap();

			CreateMap<hrvster.Models.Account, Account>()
				.ReverseMap();

			CreateMap<hrvster.Models.Sweep, Sweepstake>()
				.ReverseMap();
			CreateMap<hrvster.Models.Sweep, ViewSweep>();

			CreateMap<hrvster.Models.Rule, Rule>()
				.ForMember(dest => dest.WatchType, opt => opt.MapFrom(src => src.WatchTypeCode))
			.ReverseMap();

			CreateMap<hrvster.Models.SweepEntryRule, EntryRule>()
				.ReverseMap();

			CreateMap<hrvster.Models.DrawRule, DrawRule>()
				.ForMember(dest => dest.DrawRuleUid, opt => opt.MapFrom(src => src.SweepDrawRuleUid))
				.ReverseMap();

			CreateMap<hrvster.Models.WatchType, WatchType>()
				.ReverseMap();

			CreateMap<hrvster.Models.SweepEntry, SweepEntry>()
				.ReverseMap();

			CreateMap<hrvster.Models.SweepPrize, Prize>()
				.ReverseMap();

			CreateMap<hrvster.Models.SweepPrize, SweepPrize>()
				.ReverseMap();

			CreateMap<SweepPrize, Prize>()
				.ReverseMap();

			CreateMap<hrvster.Models.SweepWinner, Winner>()
				.ReverseMap();

			CreateMap<hrvster.Models.SweepBackupWinner, BackupWinner>()
				.ReverseMap();

			CreateMap<Games.Match.Models.MatchGame, Models.MatchGame>()
				.ForMember(dest => dest.NumberWinnable, opt => opt.MapFrom(src => src.NumberOfWinnableBoards))
				.ForMember(dest => dest.MatchGameUid, opt => opt.MapFrom(src => src.Uid))
				.ForMember(dest => dest.IsGenerated, opt => opt.MapFrom(src => src.HasBeenGenerated))
				.ForMember(dest => dest.ReadyToGenerate, opt => opt.MapFrom(src => src.ReadyToBeGenerated))
				.ReverseMap();

			CreateMap<Games.Match.Models.MatchImage, Models.MatchImage>()
				.ForMember(dest => dest.MatchGameName, opt => opt.MapFrom(src => src.MatchGame.Name))
				.ForMember(dest => dest.CampaignUid, opt => opt.MapFrom(src => src.MatchGame.CampaignUid))
				.ForMember(dest => dest.LocationUrl, opt => opt.MapFrom(src => src.LocationUrl))
				.ForMember(dest => dest.Uid, opt => opt.MapFrom(src => src.Uid))
				.ReverseMap();

			CreateMap<MatchGameRule, MatchRule>()
				.ForMember(dest => dest.WatchValue, opt => opt.MapFrom(src => src.WatchValue))
				.ForMember(dest => dest.RuleUid, opt => opt.MapFrom(src => src.Uid))
				.ForMember(dest => dest.RuleId, opt => opt.MapFrom(src => src.Id))
				.ReverseMap();
		}
	}
}
