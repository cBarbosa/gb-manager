using gb_manager.Infraestructure.ExternalServices.MercadoPago;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace gb_manager.Service
{
    public class PaymentService: IPaymentService
    {
        private readonly ILogger<PaymentService> logger;
        private readonly IMercadoPagoService mercadoService;

        public PaymentService(
            IMercadoPagoService _mercadoService,
            ILogger<PaymentService> _logger)
        {
            logger = _logger;
            mercadoService = _mercadoService;
        }

        public async Task<object> GetTest(string resourceId)
        {
            return await mercadoService.GetByResourceId("13171557-ffb12b99-e30c-42b8-a45d-ac8dd7c5c1d6");
        }
    }
}