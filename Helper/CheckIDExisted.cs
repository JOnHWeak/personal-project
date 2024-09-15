using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using personal_project.Models;

namespace personal_project.Helper
{
    public static class CheckIDExisted
    {
        public static async Task<bool> JobExists(pubsContext context, short jobId)
        {
            return await context.jobs.AnyAsync(j => j.job_id == jobId);
        }

        public static async Task<bool> PublisherExists(pubsContext context, string pubId)
        {
            return await context.publishers.AnyAsync(p => p.pub_id == pubId);
        }

        public static async Task<bool> EmployeeExists(pubsContext context, string empId)
        {
            return await context.employees.AnyAsync(e => e.emp_id == empId);
        }
        public static async Task<bool> StoreExists(pubsContext context, string storeId)
        {
            return await context.stores.AnyAsync(s => s.stor_id == storeId);
        }
        public static async Task<bool> TitleExists(pubsContext context, string titleId)
        {
            return await context.titles.AnyAsync(t => t.title_id == titleId);
        }
    }
}

