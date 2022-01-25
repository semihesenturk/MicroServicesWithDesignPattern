using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;
using Shared;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        #region Variables
        private readonly IPublishEndpoint _publishEndpoint;
        #endregion

        #region Constructor
        public OrdersController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        #endregion

        #region Actions
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {

            var OrderCreatedEvent = new OrderCreatedEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = 111,
                Payment = new PaymentMessage
                {
                    CardName = orderCreate.Payment.CardName,
                    CardNumber = orderCreate.Payment.CardNumber,
                    CVV = orderCreate.Payment.CVV,
                    Expiration = orderCreate.Payment.Expiration,
                    TotalPrice = orderCreate.orderItems.Sum(x => x.Price * x.Count)
                }
            };

            orderCreate.orderItems.ForEach(item =>
            {
                OrderCreatedEvent.OrderItems.Add(new OrderItemMessage { Count = item.Count, ProductId = item.ProductId });
            });

            await _publishEndpoint.Publish(OrderCreatedEvent);

            return Ok();
        }
        #endregion
    }
}
