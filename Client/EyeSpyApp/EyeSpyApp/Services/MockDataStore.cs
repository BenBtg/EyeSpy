using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EyeSpyApp.Models;

[assembly: Xamarin.Forms.Dependency(typeof(EyeSpyApp.Services.MockDataStore))]
namespace EyeSpyApp.Services
{
    public class MockDataStore : IDataStore<HouseholdMember>
    {
        List<HouseholdMember> items;

        public MockDataStore()
        {
            items = new List<HouseholdMember>();
            var mockItems = new List<HouseholdMember>
            {
                new HouseholdMember { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new HouseholdMember { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new HouseholdMember { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new HouseholdMember { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new HouseholdMember { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new HouseholdMember { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(HouseholdMember item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(HouseholdMember item)
        {
            var _item = items.Where((HouseholdMember arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((HouseholdMember arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<HouseholdMember> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<HouseholdMember>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}