using SignalR_SqlTableDependency.Hubs;
using SignalR_SqlTableDependency.Models;
using TableDependency.SqlClient;

namespace SignalR_SqlTableDependency.SubscribeTableDependencies
{
    public class SubscribeSaleTableDependency : ISubscribeTableDependency
    {
        private SqlTableDependency<Sale> _tableDependency = null;
        private DashboardHub _dashboardHub = null;

        public SubscribeSaleTableDependency(DashboardHub dashboardHub)
        {
            _dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {

            _tableDependency = new SqlTableDependency<Sale>(connectionString);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Sale)} SqlTableDependency error :{e.Error}");
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Sale> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _dashboardHub.SendSales();
            }
        }
    }
}
