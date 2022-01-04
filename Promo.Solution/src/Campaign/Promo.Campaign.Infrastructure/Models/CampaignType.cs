using Newtonsoft.Json.Converters;
using Promo.Core;
using System;
using System.Text.Json.Serialization;

namespace Promo.Campaign.Infrastructure.Models
{
    public class CampaignType
    {
        public int CampaignTypeId { get; set; }
        public Guid CampaignTypeUid { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public CampaignTypeCode CampaignTypeCode { get; set; }
        public string CampaignTypeName { get; set; }
        public string CamapaignTypeDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
