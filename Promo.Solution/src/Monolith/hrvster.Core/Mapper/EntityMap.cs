using AutoMapper;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;

namespace hrvster.Core.Mapper
{
	public class EntityMap : Profile
	{
		public EntityMap()
		{
			CreateMap<Models.Campaign, Promotion>();
			CreateMap<Models.CampaignType, PromotionType>();
			CreateMap<Models.Account, Account>();
			CreateMap<Models.Sweep, Sweepstake>();

			CreateMap<Models.Rule, Rule>()
				.ForMember(dest => dest.WatchType, opt => opt.MapFrom(src => src.WatchTypeCode));

			CreateMap<Models.SweepEntryRule, EntryRule>();
			CreateMap<Models.DrawRule, DrawRule>();
		}
	}
}
