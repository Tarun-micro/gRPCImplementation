using Grpc.Core;
using Order.Infrastructure.Model;
using SharedLib;

namespace Order.Infrastructure.Service
{
    public class OrderService : IOrderService
    {
        private readonly ProductGrpc.ProductGrpcClient _client;
        public OrderService(ProductGrpc.ProductGrpcClient client)
        {
            _client = client;
        }

        public async Task<string> CreateOrder(CreateOrderRequestModel requestModel)
        {
            ProductRequestModel request = new() { Id = (int)requestModel.ProductId };
            try
            {
                GetProductResponse response = await _client.GetProductsAsync(request);
                if(response != null && response.Qty < requestModel.ProductQty) {
                    return "Qty not Sufficient";
                }
            }
            catch (RpcException e)
            {
                return e.ToString();
            }
            return "Order Created Successfully";            
        }
    }
}
