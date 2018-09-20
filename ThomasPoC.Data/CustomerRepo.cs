﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThomasPoC.Data.DAL;

namespace ThomasPoC.Data
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly CustomerDbContext _context = null;
        private ILogger _log = null;

        public CustomerRepo(IOptions<Settings> settings, ILogger<CustomerRepo> logger)
        {
            _context = new CustomerDbContext(settings);
            _log = logger;
        }

        public async Task<IList<Customer>> GetAllCustomers()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string url = "https://jsonplaceholder.typicode.com/posts";
                    Task<string> response = httpClient.GetStringAsync(url);
                    IList<Customer> customers = JsonConvert.DeserializeObject<IList<Customer>>(await response);

                    return customers;
                }
            }
            catch (Exception ex)
            {
                _log.LogCritical($"GetAllCustomers failed", ex);
                throw ex;
            }
        }

        public async Task<IList<Customer>> GetCustomersBySearchTerm(string s)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

                    string url = "https://jsonplaceholder.typicode.com/posts";
                    Task<string> response = httpClient.GetStringAsync(url);
                    IList<Customer> customers = JsonConvert.DeserializeObject<IList<Customer>>(await response);

                    customers = customers.Where(x =>
                        x.title.Contains(s) || x.body.Contains(s) ||
                        Convert.ToString(x.userId).Contains(s)).ToList();

                    return customers;
                }
            }
            catch (Exception ex)
            {
                _log.LogCritical($"GetCustomersBySearchTerm with searchterm {s} failed", ex);
                throw ex;
            }
        }

    }
}
