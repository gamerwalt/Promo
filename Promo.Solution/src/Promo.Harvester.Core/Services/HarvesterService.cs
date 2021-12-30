using MediatR;
using Promo.Clients.Internal;
using Promo.Core.Models;
using Promo.Harvester.Core.Notifications;
using Promo.Harvester.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Promo.Harvester.Core.Services
{
    //ideally, we need to put this in a queue to make things faster
    //from the queue, we add it to the database 
    //but then also make sure we send out an event that will allow us to get that result into individual campaign databases/tables
    //but the campaign service needs to know about this
    public interface IHarvesterService
    {
        Task SaveTweetReceivedAsync(string json, ITweet tweet);
    }

    public class HarvesterService : IHarvesterService
    {
        private readonly IHarvesterRepository _repository;
        private readonly IMediator _mediator;
        private readonly IRulesClient _rulesClient;

        public HarvesterService(IHarvesterRepository repository, IMediator mediator, IRulesClient rulesClient)
        {
            _repository = repository;
            _mediator = mediator;
            _rulesClient = rulesClient;
        }

        public async Task SaveTweetReceivedAsync(string json, ITweet tweet)
        {

            List<Rule> rules = await _rulesClient.GetRulesForReceivedTweetAsync(tweet.Hashtags);

            var userUid = Guid.NewGuid();
            var tweetUid = Guid.NewGuid();

            //we should throw these into a queue
            foreach (var rule in rules.Distinct())
            {
                var harvestedUid = Guid.NewGuid();
                await _repository.CreateHarvestedRecordAsync(harvestedUid, rule.RuleUid, json);
                await _repository.CreateTweetUserAsync(userUid, tweet.CreatedBy);
                await _repository.CreateTweetRecordAsync(tweetUid, harvestedUid, rule.RuleUid, userUid, tweet);

                await _mediator.Publish(new NewHashTagTweetReceived(rule, harvestedUid, userUid, tweet));
            }
        }
    }
}
