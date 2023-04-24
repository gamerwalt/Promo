using hrvster.Infrastructure.Facebook;
using hrvster.Models.Facebook;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Harvesters.Facebook
{
	public interface IFacebookPageSerivce
	{
		Task CreateAsync(Guid userUid, List<FacebookPage> facebookPages);
		Task<FacebookPage> GetPageAsync(string pageId);
		Task<IEnumerable<FacebookPage>> GetPagesAsync(Guid userUid);
	}

	public class FacebookPageSerivce : IFacebookPageSerivce
	{
		private readonly ILogger<FacebookPageSerivce> _logger;
		private readonly IFacebookPageRepository _facebookPageRepository;

		public FacebookPageSerivce(
			ILoggerFactory loggerFactory,
			IFacebookPageRepository facebookPageRepository)
		{
			_logger = loggerFactory.CreateLogger<FacebookPageSerivce>();
			_facebookPageRepository = facebookPageRepository;
		}

		public async Task CreateAsync(Guid userUid, List<FacebookPage> facebookPages)
		{
			await _facebookPageRepository.DeleteAllPagesAsync(userUid);

			foreach(var page in facebookPages)
			{
				await _facebookPageRepository.CreateAsync(userUid, page);
			}
		}

		public async Task<FacebookPage> GetPageAsync(string pageId)
			=> await _facebookPageRepository.GetPageAsync(pageId);

		public async Task<IEnumerable<FacebookPage>> GetPagesAsync(Guid userUid)
		{
			return await _facebookPageRepository.GetAllPagesAsync(userUid);
		}
	}
}
