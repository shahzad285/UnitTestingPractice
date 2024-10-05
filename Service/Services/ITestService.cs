using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface ITestService
    {
        Task<IEnumerable<TestItem>> GetAllItemsAsync();
        Task<TestItem> GetItemByIdAsync(int id);
        Task<TestItem> CreateItemAsync(TestItem item);
        Task<bool> UpdateItemAsync(int id, TestItem item);
        Task<bool> DeleteItemAsync(int id);
        Task<IEnumerable<TestItem>> SearchItemsAsync(string query);
    }
}
