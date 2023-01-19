using FluentAssertions;
using hrvster.Core.Clients.Common;
using hrvster.Core.Handlers.Tests.Common;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using hrvster.Web.Mvc.Common.Steps;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Common.Steps;
using TestUtilities.Data;
using Xunit;

namespace hrvster.Core.Handlers.Tests
{
    [Collection("Sequential")]
    public class SweepsDrawHandlerTests : BuilderClassAbstract, IClassFixture<MediatorHandlerFeature>
    {
        private readonly MediatorHandlerFeature _feature;
        private DrawBackupWinnersForSweepstakesRequest _request;

        public SweepsDrawHandlerTests(MediatorHandlerFeature feature)
            => _feature = feature;

        [Fact]
        public async Task  We_Should_Draw_3_Backup_Winners_Per_Main_Winner()
        {
            _feature.GivenACleanSlate();

            var account = BuildNewAccount();
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());
            var profile = BuildNewTwitterProfile(account);

            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
            await _feature.ProfileRespository.UpsertProfileAsync(profile);

            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(
                account, 
                campaign, 
                TestData.SweepUid, 
                DateTime.Now.AddDays(-30), 
                DateTime.Now.AddDays(3), 
                DateTime.Now.AddHours(-1));
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
            sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

            var sweepEntryRule = BuildNewSweepEntryRule(TestData.EntryRuleUid, sweep, campaign, 3);
            await _feature.SweepEntryRulesRepository.CreateAsync(sweepEntryRule);

            var drawRule = BuildSweepDrawRule(TestData.SweepDrawRuleUid, sweep, 2, 3);
            await _feature.SweepDrawRuleRepository.CreateAsync(drawRule);

            var rule = BuildRule(TestData.RuleUid, campaign, sweep, "#Testinghashtag");
            await _feature.SweepRulesRepository.CreateRuleAsync(rule);
            var rules = await _feature.SweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
            rule = rules.FirstOrDefault();

            //Seed entries
            var sweepEntries = SeedEntriesFor(sweep, rule);
            foreach(var entry in sweepEntries)
            {
                await _feature.SweepEntryRepository.CreateAsync(entry);
            }

            await _feature.DrawService.ProcessDrawsAsync(default);

            var winners = await _feature.SweepWinnersRepository.GetSweepWinnersAsync(sweep.SweepUid);
            var backupWinners = await _feature.SweepWinnersRepository.GetBackupWinnersAsync(sweep.SweepUid);

            var totalWinners = winners.Count() + backupWinners.Count();
            totalWinners.Should().Be((drawRule.WinnerBackupCount * drawRule.WinnerCount) + drawRule.WinnerCount);
        }

        [Fact]
        public async Task We_Should_Upsert_The_Draw_Notification_Table_About_The_Draw()
        {
            _feature.GivenACleanSlate();
            
            var account = BuildNewAccount();
            var campaign = BuildNewCampaign(account, BuildNewCampaignType());
            var profile = BuildNewTwitterProfile(account);

            await _feature.AccountRepository.CreateAsync(account);
            await _feature.CampaignRepository.CreateCampaignAsync(campaign, TestData.WindowsTimezone, false);
            await _feature.ProfileRespository.UpsertProfileAsync(profile);

            account = await _feature.AccountRepository.GetAsync(account.AccountUid);
            campaign = await _feature.CampaignRepository.GetCampaignByUidAsync(campaign.CampaignUid);

            var sweep = BuildNewSweep(
                account,
                campaign,
                TestData.SweepUid,
                DateTime.Now.AddDays(-30),
                DateTime.Now.AddDays(3),
                DateTime.Now.AddHours(-1));
            await _feature.SweepsRepository.CreateAsync(sweep, TestData.WindowsTimezone, false);
            sweep = await _feature.SweepsRepository.GetAsync(sweep.SweepUid);

            var sweepEntryRule = BuildNewSweepEntryRule(TestData.EntryRuleUid, sweep, campaign, 3);
            await _feature.SweepEntryRulesRepository.CreateAsync(sweepEntryRule);

            var drawRule = BuildSweepDrawRule(TestData.SweepDrawRuleUid, sweep, 2, 3);
            await _feature.SweepDrawRuleRepository.CreateAsync(drawRule);

            var rule = BuildRule(TestData.RuleUid, campaign, sweep, "#Testinghashtag");
            await _feature.SweepRulesRepository.CreateRuleAsync(rule);
            var rules = await _feature.SweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
            rule = rules.FirstOrDefault();

            //Seed entries
            var sweepEntries = SeedEntriesFor(sweep, rule);
            foreach (var entry in sweepEntries)
            {
                await _feature.SweepEntryRepository.CreateAsync(entry);
            }

            await _feature.DrawService.ProcessDrawsAsync(default);

            _feature.EmailsTableShouldContainFieldEqualTo(MailTableFields.EmailAddress, TestData.EmailAddress);
            _feature.SweepNotificationTableShouldContainFieldEqualTo(SweepNotificationTableFields.SweepId, sweep.SweepId.ToString());
            _feature.SweepNotificationTableShouldContainFieldEqualTo(SweepNotificationTableFields.DrawAttempted, 1.ToString());
            _feature.SweepNotificationTableShouldContainFieldEqualTo(SweepNotificationTableFields.DrawHappened, 1.ToString());
        }
    }
}
