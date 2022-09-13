using hrvster.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace hrvster.Extensions.OptionsConfiguration
{
	public static class OptionConfigurationExtensions
	{
		public static IServiceCollection AddDrawSettingConfiguration(this IServiceCollection services, IConfiguration configuration)
			=> services
				.Configure<DrawSettingOptions>(configuration)
				.Configure<DrawSettingOptions>(configuration.GetSection(DrawSettingOptions.SectionName))
				.AddTransient(s => s.GetService<IOptions<DrawSettingOptions>>().Value);

		public static IServiceCollection AddMailSettings(this IServiceCollection services, IConfiguration configuration)
			=> services
				.Configure<MailSettingOptions>(configuration)
				.Configure<MailSettingOptions>(configuration.GetSection("MailSettings"))
				.AddTransient(s => s.GetService<IOptions<MailSettingOptions>>().Value);

		public static IServiceCollection AddSendGrid(this IServiceCollection services, IConfiguration configuration)
			=> services
				.Configure<SendGridOptions>(configuration)
				.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName))
				.AddTransient(s => s.GetService<IOptions<SendGridOptions>>().Value);

		public static IServiceCollection AddPromotionConfigurationOptions(
			this IServiceCollection services,
			IConfiguration configuration)
				=> services
					.Configure<PromotionConfigurationOptions>(configuration)
					.Configure<PromotionConfigurationOptions>(configuration.GetSection(PromotionConfigurationOptions.SectionName))
					.AddTransient(s => s.GetService<IOptions<PromotionConfigurationOptions>>().Value);

		public static IServiceCollection AddNotificationConfigurationOptions(
			this IServiceCollection services,
			IConfiguration configuration)
				=> services
					.Configure<NotificationServiceOptions>(configuration)
					.Configure<NotificationServiceOptions>(configuration.GetSection(NotificationServiceOptions.SectionName))
					.AddTransient(s => s.GetService<IOptions<NotificationServiceOptions>>().Value);

		public static IServiceCollection AddHarvsterConfigurationOption(
			this IServiceCollection services,
			IConfiguration configuration)
				=> services
					.Configure<HarvesterSettingsOptions>(configuration)
					.Configure<HarvesterSettingsOptions>(configuration.GetSection(HarvesterSettingsOptions.SectionName))
					.AddTransient(s => s.GetService<IOptions<HarvesterSettingsOptions>>().Value);
	}
}
