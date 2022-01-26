using MassTransit;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        #region variables
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _endpoint;
        #endregion

        #region Constructor
        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint endpoint)
        {
            _logger = logger;
            _endpoint = endpoint;
        }
        #endregion

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} was withdrawn from credit card! for user={context.Message.BuyerId}");

                await _endpoint.Publish(new PaymentCompletedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was not withdrawn from credit card for user={context.Message.BuyerId}");

                await _endpoint.Publish(new PaymentFailedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId, Message = "not enough balance", OrderItems =context.Message.OrderItems });
            }
        }
    }
}
