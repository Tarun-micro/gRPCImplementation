using Grpc.Core;
using Order.Infrastructure.Model;
using SharedLib;
using System.Threading;

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

        public async Task<string> GetOrderDetails()
        {
            string products = string.Empty;
            try
            {
                var streamingCall =  _client.GetProductsList(new VoidPayLoad());
                await foreach (var GetProductResponse in streamingCall.ResponseStream.ReadAllAsync())
                {
                    products = products + GetProductResponse.ToString();
                }
            }
            catch (RpcException e) 
            {
                return e.ToString();
            }
            return products;
        }

        public async Task<string> InitiateClientStreaming()
        {
            try
            {
                var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                using AsyncClientStreamingCall<ProductRequestModel, GetProductResponse> clientStreamingCall = _client.GetProductListClientStream(cancellationToken: cancellationToken.Token);
                var i = 0;
                while (true)
                {
                    if (i >= 10)
                    {
                        await clientStreamingCall.RequestStream.CompleteAsync();
                        break;
                    }
                    else
                    {
                        //write to stream
                        await clientStreamingCall.RequestStream.WriteAsync(new ProductRequestModel { Id = i });
                        i++;
                    }
                }
                var response = await clientStreamingCall;
                return response.ToString();
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                return ex.ToString();
            }
        }
    }
}
