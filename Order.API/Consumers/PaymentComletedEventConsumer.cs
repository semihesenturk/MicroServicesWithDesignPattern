using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        #region Variables
        private readonly AppDbContext _context;
        private ILogger<PaymentCompletedEventConsumer> _logger;
        #endregion

        #region Constructor
        public PaymentCompletedEventConsumer(AppDbContext context, ILogger<PaymentCompletedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order Id= {context.Message.OrderId} status changed as {order.Status}");
            }
            else
            {
                _logger.LogError($"Order with id = {context.Message.OrderId} not found!");
            }
        }
    }
}
