using AutoMapper;
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
		}
	}
}
