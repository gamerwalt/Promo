using AutoMapper;
using hrvster.Models;

namespace hrvster.Core.Mapper
{
	public class EntityMap : Profile
	{
		public EntityMap()
		{
			CreateMap<Campaign, Models.WebModels.Promotion>()
				.ReverseMap();

			CreateMap<CampaignType, Models.WebModels.PromotionType>()
				.ReverseMap();

			CreateMap<Models.Account, Models.WebModels.Account>()
				.ReverseMap();
		}
	}
}
