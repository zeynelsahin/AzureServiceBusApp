using System.Runtime.CompilerServices;
using AzureServiceBusApp.Common;
using AzureServiceBusApp.Common.Events;
using AzureServiceBusApp.ProducersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBusApp.ProducersApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly AzureService _azureService;

    public OrderController(AzureService azureService)
    {
        _azureService = azureService;
    }

    [HttpPost]
    public async Task CreateOrder(OrderDto order)
    {
        var orderCreatedEvent = new OrderCreatedEvent
        {
            Id = order.Id, ProductName = order.ProductName, CreatedOn = DateTime.Now
        };

        await _azureService.CreateQueueIfNotExists(Constants.OrderCreatedQueue);
        await _azureService.SendMessageToQueue(Constants.OrderCreatedQueue, orderCreatedEvent);
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteOrder(int id)
    {
        var orderDeletedEvent = new OrderDeletedEvent()
        {
            Id = id, CreatedOn = DateTime.Now
        };

        await _azureService.CreateQueueIfNotExists(Constants.OrderDeletedQueue);
        await _azureService.SendMessageToQueue(Constants.OrderCreatedQueue, orderDeletedEvent,"OrderDeleted");
    }

    [HttpPost("CreateTopic")]
    public async Task CreateTopic(OrderDto order)
    {
        var orderCreatedEvent = new OrderCreatedEvent
        {
            Id = order.Id, ProductName = order.ProductName, CreatedOn = DateTime.Now
        };
        
        await _azureService.CreateTopicIfNotExists(Constants.Topic);
        await _azureService.CreateSubscriptionIfNotExists(Constants.Topic, Constants.OrderCreatedSub,messageType:"OrderCreated","OrderCreatedOnly");
        await _azureService.SendMessageTopic(Constants.Topic, orderCreatedEvent,"OrderCreated");
    } 
    [HttpDelete("DeleteTopic")]
    public async Task DeleteTopic(int id)
    {
        var orderDeletedEvent = new OrderDeletedEvent()
        {
            Id = id, CreatedOn = DateTime.Now
        };
        
        await _azureService.CreateTopicIfNotExists(Constants.Topic);
        await _azureService.CreateSubscriptionIfNotExists(Constants.Topic, Constants.OrderDeletedSub,messageType:"OrderDeleted","OrderDeletedOnly");
        await _azureService.SendMessageTopic(Constants.Topic, orderDeletedEvent,"OrderDeleted");
    }
}