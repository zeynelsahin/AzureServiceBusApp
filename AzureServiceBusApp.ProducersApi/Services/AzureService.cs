using System.Runtime.CompilerServices;
using System.Text;
using AzureServiceBusApp.Common;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace AzureServiceBusApp.ProducersApi.Services;

public class AzureService
{
    private readonly ManagementClient _managementClient;

    public AzureService(ManagementClient managementClient)
    {
        _managementClient = managementClient;
    }

    public async Task SendMessageToQueue(string queueName, object messageContent,string messageType=null)
    {
        IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);
        await SendMessage(client, messageContent,messageType);
    }

    public async Task CreateQueueIfNotExists(string queueName)
    {
        if (!await _managementClient.QueueExistsAsync(queueName))
        {
            await _managementClient.CreateQueueAsync(queueName);
        }
    }

    public async Task CreateTopicIfNotExists(string topicName)
    {
        if (!await _managementClient.TopicExistsAsync(topicName))
        {
            await _managementClient.CreateTopicAsync(topicName);
        }
    }

    public async Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName, string messageType = null, string rulelName = null)
    {
        if (await _managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
            return;
        if (messageType != null)
        {
            var sd = new SubscriptionDescription(topicName, subscriptionName);
            var filter = new CorrelationFilter
            {
                Properties =
                {
                    ["MessageType"] = messageType
                }
            };
            var rd = new RuleDescription(rulelName ?? messageType + "Rule", filter);
            await _managementClient.CreateSubscriptionAsync(sd,rd);
        }
        else
        {
            await _managementClient.CreateSubscriptionAsync(topicName, subscriptionName);
        }
    }

    public async Task SendMessageTopic(string topicName, object messageContent,string messageType=null)
    {
        ITopicClient client = new TopicClient(Constants.ConnectionString, topicName);
        await SendMessage(client, messageContent,messageType);
    }

    private async Task SendMessage(ISenderClient client, object messageContent,string messageType=null)
    {
        var byteArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
        var message = new Message(byteArr);
        message.UserProperties["MessageType"] = messageType;
        await client.SendAsync(message);
    }
}