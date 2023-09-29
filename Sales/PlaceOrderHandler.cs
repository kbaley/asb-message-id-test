using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales
{    
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        static readonly Random random = new Random();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            var messageId = context.MessageHeaders[NServiceBus.Headers.MessageId];
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}, MessageId = {messageId}");

            // This is normally where some business logic would occur

            #region ThrowTransientException
            // Uncomment to test throwing transient exceptions
            //if (random.Next(0, 5) == 0)
            //{
            //    throw new Exception("Oops");
            //}
            #endregion

            #region ThrowFatalException
            // Uncomment to test throwing fatal exceptions
            //throw new Exception("BOOM");
            #endregion

            return Task.CompletedTask;
        }
    }
}
