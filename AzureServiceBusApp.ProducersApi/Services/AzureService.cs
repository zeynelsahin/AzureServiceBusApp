using System.Text;
using AzureServiceBusApp.Common;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace AzureServiceBusApp.ProducersApi.Services;

public class AzureService
{
    private ManagementClient _managementClient;
    public AzureService(ManagementClient managementClient)
    {
        _managementClient = managementClient;
    }
    public async Task SendMessageToQueue(string queueName, object messageContent)
    {
        IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);
        var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
        var message = new Message(byteArray);
        await client.SendAsync(message);
    }

    public async Task CreateQueueIfNotExists(string queueName)
    {
        if (!await _managementClient.QueueExistsAsync(queueName))
        {
            await _managementClient.CreateQueueAsync(queueName);
        }
    }
}