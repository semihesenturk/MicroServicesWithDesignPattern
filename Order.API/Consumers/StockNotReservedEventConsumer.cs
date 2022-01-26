using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {

        #region Variables
        private readonly AppDbContext _context;
        private readonly ILogger<StockNotReservedEventConsumer> _logger;
        #endregion

        #region Constructor
        public StockNotReservedEventConsumer(AppDbContext context, ILogger<StockNotReservedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
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
