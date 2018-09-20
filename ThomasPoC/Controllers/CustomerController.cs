using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using ThomasPoC.Data;
using ThomasPoC.Data.DAL;

namespace ThomasPoC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        private ILogger _log = null;

        public CustomerController(ICustomerRepo customerRepo, ILogger<CustomerRepo> logger, IOptions<Settings> settings)
        {
            _customerRepo = customerRepo;
            _log = logger;
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        //[Produces("application/json")]
        [HttpGet("GetAllcustomers")]
        //[ODataRoute("GetCustomersBySearchTerm)")]
        //[EnableQuery]
        public async Task<IActionResult> GetAllCustomers()
        {
            IEnumerable<Customer> customers = await _customerRepo.GetAllCustomers();

            if (!customers.Any() || customers.Count() == 0)
            {
                return NotFound(customers);
            }
            return Ok(customers);
            //return Json(customers);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [HttpGet("GetCustomersBySearchTerm")]
        //[ODataRoute("GetCustomersBySearchTerm)")]
        //[EnableQuery]
        public async Task<IActionResult> GetCustomersBySearchTerm(string s)
        {
            if (s == null || s == "")
            {
                return UnprocessableEntity();
            }

            IEnumerable<Customer> customers = await _customerRepo.GetCustomersBySearchTerm(s);

            if (!customers.Any() || customers.Count() == 0)
            {
                return NotFound(s);
            }
            return Ok(customers);
        }

        //Future service layer / API gateway
        //public async Task<BackOfficeResponse<List<Country>>> ReturnAllCountriesAsync()
        //{
        //    return await _service.ProcessAsync<List<Country>>(BackOfficeEndpoint.CountryEndpoint, "returnCountries");
        //}

    }
}
