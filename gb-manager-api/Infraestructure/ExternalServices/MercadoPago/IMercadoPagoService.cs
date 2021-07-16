using gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain.Resource;
using System;
using System.Threading.Tasks;

namespace gb_manager.Infraestructure.ExternalServices.MercadoPago
{
    public interface IMercadoPagoService
    {
        Task<IResource> GetByResourceId(string resourceId);
        Task<IResource> GetCheckoutPreferencesSearch();
    }
}