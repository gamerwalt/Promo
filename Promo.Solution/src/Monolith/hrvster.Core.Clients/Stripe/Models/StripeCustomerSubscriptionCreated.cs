using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Clients.Stripe.Models
{
	public class StripeCustomerSubscriptionCreated
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string ObjectType { get; set; }

		[JsonProperty("api_version")]
		public DateTimeOffset ApiVersion { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("data")]
		public Data Data { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("pending_webhooks")]
		public long PendingWebhooks { get; set; }

		[JsonProperty("request")]
		public Request Request { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}

	public class Data
	{
		[JsonProperty("object")]
		public MainObject Object { get; set; }
	}

	public class MainObject
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string ObjectTypeName { get; set; }

		[JsonProperty("application")]
		public string Application { get; set; }

		[JsonProperty("application_fee_percent")]
		public string ApplicationFeePercent { get; set; }

		[JsonProperty("automatic_tax")]
		public AutomaticTax AutomaticTax { get; set; }

		[JsonProperty("billing_cycle_anchor")]
		public long BillingCycleAnchor { get; set; }

		[JsonProperty("billing_thresholds")]
		public string BillingThresholds { get; set; }

		[JsonProperty("cancel_at")]
		public DateTime CancelAt { get; set; }

		[JsonProperty("cancel_at_period_end")]
		public bool CancelAtPeriodEnd { get; set; }

		[JsonProperty("canceled_at")]
		public DateTime CanceledAt { get; set; }

		[JsonProperty("cancellation_details")]
		public CancellationDetails CancellationDetails { get; set; }

		[JsonProperty("collection_method")]
		public string CollectionMethod { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("current_period_end")]
		public long CurrentPeriodEnd { get; set; }

		[JsonProperty("current_period_start")]
		public long CurrentPeriodStart { get; set; }

		[JsonProperty("customer")]
		public string Customer { get; set; }

		[JsonProperty("days_until_due")]
		public int DaysUntilDue { get; set; }

		[JsonProperty("default_payment_method")]
		public string DefaultPaymentMethod { get; set; }

		[JsonProperty("default_source")]
		public string DefaultSource { get; set; }

		[JsonProperty("default_tax_rates")]
		public List<string> DefaultTaxRates { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("discount")]
		public string Discount { get; set; }

		[JsonProperty("ended_at")]
		public DateTime EndedAt { get; set; }

		[JsonProperty("items")]
		public Items Items { get; set; }

		[JsonProperty("latest_invoice")]
		public string LatestInvoice { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public Metadata Metadata { get; set; }

		[JsonProperty("next_pending_invoice_item_invoice")]
		public string NextPendingInvoiceItemInvoice { get; set; }

		[JsonProperty("on_behalf_of")]
		public string OnBehalfOf { get; set; }

		[JsonProperty("pause_collection")]
		public string PauseCollection { get; set; }

		[JsonProperty("payment_settings")]
		public PaymentSettings PaymentSettings { get; set; }

		[JsonProperty("pending_invoice_item_interval")]
		public string PendingInvoiceItemInterval { get; set; }

		[JsonProperty("pending_setup_intent")]
		public string PendingSetupIntent { get; set; }

		[JsonProperty("pending_update")]
		public string PendingUpdate { get; set; }

		[JsonProperty("plan")]
		public Plan Plan { get; set; }

		[JsonProperty("quantity")]
		public long Quantity { get; set; }

		[JsonProperty("schedule")]
		public string Schedule { get; set; }

		[JsonProperty("start_date")]
		public long StartDate { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("test_clock")]
		public string TestClock { get; set; }

		[JsonProperty("transfer_data")]
		public string TransferData { get; set; }

		[JsonProperty("trial_end")]
		public DateTime TrialEnd { get; set; }

		[JsonProperty("trial_settings")]
		public TrialSettings TrialSettings { get; set; }

		[JsonProperty("trial_start")]
		public DateTime TrialStart { get; set; }
	}

	public class AutomaticTax
	{
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }
	}

	public class CancellationDetails
	{
		[JsonProperty("comment")]
		public string Comment { get; set; }

		[JsonProperty("feedback")]
		public string Feedback { get; set; }

		[JsonProperty("reason")]
		public string Reason { get; set; }
	}

	public class Items
	{
		[JsonProperty("object")]
		public string ItemObject { get; set; }

		[JsonProperty("data")]
		public List<Datum> Data { get; set; }

		[JsonProperty("has_more")]
		public bool HasMore { get; set; }

		[JsonProperty("total_count")]
		public long TotalCount { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}

	public class Datum
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string DataObject { get; set; }

		[JsonProperty("billing_thresholds")]
		public string BillingThresholds { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("metadata")]
		public Metadata Metadata { get; set; }

		[JsonProperty("plan")]
		public Plan Plan { get; set; }

		[JsonProperty("price")]
		public Price Price { get; set; }

		[JsonProperty("quantity")]
		public long Quantity { get; set; }

		[JsonProperty("subscription")]
		public string Subscription { get; set; }

		[JsonProperty("tax_rates")]
		public List<string> TaxRates { get; set; }
	}

	public class Metadata
	{
	}

	public class Plan
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string PlanObject { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		[JsonProperty("aggregate_usage")]
		public string AggregateUsage { get; set; }

		[JsonProperty("amount")]
		public long Amount { get; set; }

		[JsonProperty("amount_decimal")]
		public long AmountDecimal { get; set; }

		[JsonProperty("billing_scheme")]
		public string BillingScheme { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("interval")]
		public string Interval { get; set; }

		[JsonProperty("interval_count")]
		public long IntervalCount { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public Metadata Metadata { get; set; }

		[JsonProperty("nickname")]
		public string Nickname { get; set; }

		[JsonProperty("product")]
		public string Product { get; set; }

		[JsonProperty("tiers_mode")]
		public string TiersMode { get; set; }

		[JsonProperty("transform_usage")]
		public string TransformUsage { get; set; }

		[JsonProperty("trial_period_days")]
		public string TrialPeriodDays { get; set; }

		[JsonProperty("usage_type")]
		public string UsageType { get; set; }
	}

	public class Price
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string PriceObject { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		[JsonProperty("billing_scheme")]
		public string BillingScheme { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("custom_unit_amount")]
		public string CustomUnitAmount { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("lookup_key")]
		public string LookupKey { get; set; }

		[JsonProperty("metadata")]
		public Metadata Metadata { get; set; }

		[JsonProperty("nickname")]
		public string Nickname { get; set; }

		[JsonProperty("product")]
		public string Product { get; set; }

		[JsonProperty("recurring")]
		public Recurring Recurring { get; set; }

		[JsonProperty("tax_behavior")]
		public string TaxBehavior { get; set; }

		[JsonProperty("tiers_mode")]
		public string TiersMode { get; set; }

		[JsonProperty("transform_quantity")]
		public int TransformQuantity { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("unit_amount")]
		public long UnitAmount { get; set; }

		[JsonProperty("unit_amount_decimal")]
		public long UnitAmountDecimal { get; set; }
	}

	public class Recurring
	{
		[JsonProperty("aggregate_usage")]
		public string AggregateUsage { get; set; }

		[JsonProperty("interval")]
		public string Interval { get; set; }

		[JsonProperty("interval_count")]
		public long IntervalCount { get; set; }

		[JsonProperty("trial_period_days")]
		public string TrialPeriodDays { get; set; }

		[JsonProperty("usage_type")]
		public string UsageType { get; set; }
	}

	public class PaymentSettings
	{
		[JsonProperty("payment_method_options")]
		public string PaymentMethodOptions { get; set; }

		[JsonProperty("payment_method_types")]
		public string PaymentMethodTypes { get; set; }

		[JsonProperty("save_default_payment_method")]
		public string SaveDefaultPaymentMethod { get; set; }
	}

	public class TrialSettings
	{
		[JsonProperty("end_behavior")]
		public EndBehavior EndBehavior { get; set; }
	}

	public class EndBehavior
	{
		[JsonProperty("missing_payment_method")]
		public string MissingPaymentMethod { get; set; }
	}

	public class Request
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("idempotency_key")]
		public Guid IdempotencyKey { get; set; }
	}
}
