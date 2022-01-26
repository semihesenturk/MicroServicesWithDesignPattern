using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        #region Variables
        private readonly ILogger<PaymentFailedEventConsumer> _logger;
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public PaymentFailedEventConsumer(ILogger<PaymentFailedEventConsumer> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        #endregion


        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();
                }
            }
            _logger.LogInformation($"Stock was released! for orderId={context.Message.OrderId}");
        }
    }
}
