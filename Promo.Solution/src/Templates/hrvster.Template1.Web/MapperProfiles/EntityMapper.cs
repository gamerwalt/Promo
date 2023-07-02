using AutoMapper;
using hrvster.Template1.Web.Models;
using hrvster.Templates.Models.Members;

namespace hrvster.Template1.Web.MapperProfiles
{
	public class EntityMapper : Profile
	{
		public EntityMapper()
		{
			CreateMap<CreateMember, Member>()
				.ReverseMap();

			CreateMap<MatchBoard, Games.Match.Models.MatchBoard>()
				.ReverseMap();

			//hrvster.Games.Match.Models.MatchGame->hrvster.Template1.Web.Models.MatchGame
			CreateMap<MatchGame, Games.Match.Models.MatchGame>()
				.ReverseMap();
		}
	}
}