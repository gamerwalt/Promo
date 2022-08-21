using AutoMapper;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;

namespace hrvster.Core.Mapper
{
	public class EntityMap : Profile
	{
		public EntityMap()
		{
			CreateMap<Models.Campaign, Promotion>()
				.ForMember(d => d.PromotionType, o => o.MapFrom(s => s.CampaignType))
				.ReverseMap();

			CreateMap<Models.CampaignType, PromotionType>()
				.ReverseMap();

			CreateMap<Models.Account, Account>()
				.ReverseMap();

			CreateMap<Models.Sweep, Sweepstake>()
				.ReverseMap();

			CreateMap<Models.Rule, Rule>()
				.ForMember(dest => dest.WatchType, opt => opt.MapFrom(src => src.WatchTypeCode))
			.ReverseMap();

			CreateMap<Models.SweepEntryRule, EntryRule>()
				.ReverseMap();

			CreateMap<Models.DrawRule, DrawRule>()
				.ForMember(dest => dest.DrawRuleUid, opt => opt.MapFrom(src => src.SweepDrawRuleUid))
				.ReverseMap();

			CreateMap<Models.WatchType, WatchType>()
				.ReverseMap();
		}
	}
}
