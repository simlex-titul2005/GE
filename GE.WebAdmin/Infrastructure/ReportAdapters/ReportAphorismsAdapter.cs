using SX.WebCore.Adapters;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GE.WebAdmin.Infrastructure.ReportAdapters
{
    public sealed class ReportAphorismsAdapter : SxReportAdapter
    {
        public override DataTable DataTable
        {
            get
            {
                var table = new DataTable();
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
                {
                    using (var cmd = new SqlCommand("get_report_aphorisms @catId", conn))
                    {
                        cmd.Parameters.AddWithValue("catId", "");
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                }
                return table;
            }
        }
    }
}