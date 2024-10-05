using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TestService : ITestService
    {
        private List<TestItem> _items;
        private int _nextId;

        public TestService()
        {
            InitializeDummyData();
        }

        private void InitializeDummyData()
        {
            _items = new List<TestItem>
            {
                new TestItem { Id = 1, Name = "Test Item 1", Quantity = 10, Email = "test1@example.com", CreatedDate = DateTime.UtcNow.AddDays(-5) },
                new TestItem { Id = 2, Name = "Sample Item 2", Quantity = 25, Email = "sample2@example.com", CreatedDate = DateTime.UtcNow.AddDays(-3) },
                new TestItem { Id = 3, Name = "Dummy Item 3", Quantity = 5, Email = "dummy3@example.com", CreatedDate = DateTime.UtcNow.AddDays(-1) },
                new TestItem { Id = 4, Name = "Example Item 4", Quantity = 15, Email = "example4@example.com", CreatedDate = DateTime.UtcNow },
                new TestItem { Id = 5, Name = "Test Item 5", Quantity = 30, Email = "test5@example.com", CreatedDate = DateTime.UtcNow, IsActive = false }
            };

            _nextId = _items.Max(i => i.Id) + 1;
        }

        public async Task<IEnumerable<TestItem>> GetAllItemsAsync()
        {
            return await Task.FromResult(_items);
        }

        public async Task<TestItem> GetItemByIdAsync(int id)
        {
            return await Task.FromResult(_items.FirstOrDefault(i => i.Id == id));
        }

        public async Task<TestItem> CreateItemAsync(TestItem item)
        {
            item.Id = _nextId++;
            _items.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<bool> UpdateItemAsync(int id, TestItem item)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return await Task.FromResult(false);
            }

            existingItem.Name = item.Name;
            existingItem.Quantity = item.Quantity;
            existingItem.Email = item.Email;
            existingItem.IsActive = item.IsActive;
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return await Task.FromResult(false);
            }

            _items.Remove(item);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<TestItem>> SearchItemsAsync(string query)
        {
            return await Task.FromResult(_items.Where(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
