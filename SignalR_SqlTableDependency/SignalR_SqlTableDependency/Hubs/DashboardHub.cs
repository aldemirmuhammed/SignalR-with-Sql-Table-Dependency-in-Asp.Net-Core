using Microsoft.AspNetCore.SignalR;
using SignalR_SqlTableDependency.Models;
using SignalR_SqlTableDependency.Repositories;

namespace SignalR_SqlTableDependency.Hubs
{
    public class DashboardHub : Hub
    {

        public string ConnectionString { get; set; }

        private ProductRepository _productRepository;

        private SaleRepository _saleRepository;

        private CustomerRepository _customerRepository;

        public DashboardHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            ConnectionString = connectionString;
            _productRepository = new ProductRepository(connectionString);
            _saleRepository = new SaleRepository(connectionString);
            _customerRepository = new CustomerRepository(connectionString);
        }

        public async Task SendProducts()
        {
            var products = _productRepository.GetProducts();
            await Clients.All.SendAsync("ReceivedProducts", products);
            var productsForGraph = _productRepository.GetProductsForGraph();
            await Clients.All.SendAsync("ReceivedProductsForGraph", productsForGraph);

        }


        public async Task SendSales()
        {
            var sales= _saleRepository.GetSales();
            await Clients.All.SendAsync("ReceivedSales", sales);

            var salesForGraph = _saleRepository.GetSalesForGraph();
            await Clients.All.SendAsync("ReceivedSalesForGraph", salesForGraph);
        }

        public async Task SendCustomers()
        {
            var customer = _customerRepository.GetCustomers();
            await Clients.All.SendAsync("ReceivedCustomers", customer);

            var customerForGraph = _customerRepository.GetCustomersForGraph();
            await Clients.All.SendAsync("ReceivedCustomersForGraph", customerForGraph);
        }
    }
}
