namespace Shared
{
    public static class Constants
    {
        public const string StreamProviderNameDefault = "RMQProvider";
        
        public const string StreamNameSpaceCustomers = "orleans_customers";
        public const string StreamNameSpaceOrders = "orleans_orders";
        public const string StreamNameSpaceProducts = "orleans_products";
        
        public const int RmqProxyPort = 5672;
        public const string ClusterId = "example-workflow-deployment";
        public const string ServiceId = "example-workflow";
    }
}