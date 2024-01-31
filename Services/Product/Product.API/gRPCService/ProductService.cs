using Grpc.Core;
using Product.Domain.DomainEntities;
using SharedLib;
using domain = Product.Domain.DomainEntities;
namespace Product.API.gRPCService
{
    public class ProductService : ProductGrpc.ProductGrpcBase
    {
        public override async Task<GetProductResponse> GetProducts(ProductRequestModel request, ServerCallContext context)
        {
            var products = new List<domain.Product>();
            products.Add(new domain.Product(1, "Samsung Phone", 4));
            products.Add(new domain.Product(2, "Sony Phone", 4));
            products.Add(new domain.Product(3, "Apple Phone", 3));
            products.Add(new domain.Product(4, "Aplle Device", 0));
            var product = products.FirstOrDefault(d=>d.Id == request.Id);
            if (product is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Could not found product with id: {request.Id}"));
            }
            await Task.CompletedTask;
            return new GetProductResponse()
            {
                Description = "Description",
                Id = product.Id,
                Name = product.Name,
                Qty = product.Qty,
            };
        }

        public override async Task GetProductsList(VoidPayLoad request, IServerStreamWriter<GetProductResponse> responseStream, ServerCallContext context)
        {
            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested && i < 50)
            {
                await Task.Delay(10);
                await responseStream.WriteAsync(new GetProductResponse()
                {
                    Description = "Description",
                    Id = 1,
                    Name = "SmartPhones",
                    Qty = 24,
                });
                i++;
            }
        }

        public override async Task<GetProductResponse> GetProductListClientStream(IAsyncStreamReader<ProductRequestModel> requestStream, ServerCallContext context)
        {
            var response = new GetProductResponse();
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var i = requestStream.Current;
                response.Id = i.Id;
            }
            return await Task.FromResult<GetProductResponse>(response);
        }

        public override Task GetProductListDuplexStream(IAsyncStreamReader<ProductRequestModel> requestStream, IServerStreamWriter<GetProductResponse> responseStream, ServerCallContext context)
        {
            return base.GetProductListDuplexStream(requestStream, responseStream, context);
        }
    }
}
