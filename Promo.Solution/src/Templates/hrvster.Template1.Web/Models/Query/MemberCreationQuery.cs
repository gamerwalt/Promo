namespace hrvster.Template1.Web.Models.Query;

public record MemberCreationQuery(Guid GameUid, Guid RuleUid, string ExternalClient, string GameType);
