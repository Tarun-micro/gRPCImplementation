using Order.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Service
{
    public interface IOrderService
    {
        Task<string> CreateOrder(CreateOrderRequestModel requestModel);
    }
}
