using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThomasPoC.Data
{
    public interface ICustomerRepo
    {

        Task<IList<Customer>> GetAllCustomers();

        Task<IList<Customer>> GetCustomersBySearchTerm(string searchTerm);

    }
}