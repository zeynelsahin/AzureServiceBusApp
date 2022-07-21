namespace AzureServiceBusApp.Common;

public static class Constants
{
    public const string ConnectionString = "Endpoint=sb://zeynel.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IDXhv79e9e+6imXp4Yby6J1CvSGZQMFO2pj/N3UWHtI=";
    public const string OrderCreatedQueue = "OrderCreatedQueue";
    public const string OrderDeletedQueue = "OrderDeletedQueue";
    public const string Topic = "OrderTopic";
    public const string OrderCreatedSub = "OrderCreatedSub";
    public const string OrderDeletedSub = "OrderDeletedSub";
}