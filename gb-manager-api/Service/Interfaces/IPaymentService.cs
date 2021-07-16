using System.Threading.Tasks;

namespace gb_manager.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<object> GetTest(string resourceId);
    }
}