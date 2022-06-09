using System.Collections.Generic;

namespace hrvster.Models.Services
{
	public class ServiceContainer
	{
		public ServiceContainer()
		{
			Services = new List<Service>();
		}

		public List<Service> Services { get; set; }
	}
}
