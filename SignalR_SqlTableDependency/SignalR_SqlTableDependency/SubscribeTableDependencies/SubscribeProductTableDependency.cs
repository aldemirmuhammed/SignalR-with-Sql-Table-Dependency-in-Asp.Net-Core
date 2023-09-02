using SignalR_SqlTableDependency.Hubs;
using SignalR_SqlTableDependency.Models;
using TableDependency.SqlClient;

namespace SignalR_SqlTableDependency.SubscribeTableDependencies
{
    public class SubscribeProductTableDependency : ISubscribeTableDependency
    {
        private SqlTableDependency<Product> _tableDependency = null;
        private DashboardHub _dashboardHub = null;

        public SubscribeProductTableDependency(DashboardHub dashboardHub)
        {
            _dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {

            _tableDependency = new SqlTableDependency<Product>(connectionString);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Product)} SqlTableDependency error :{e.Error}");
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Product> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _dashboardHub.SendProducts();
            }
        }
    }
}
