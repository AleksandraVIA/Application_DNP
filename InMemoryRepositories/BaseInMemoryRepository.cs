using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace InMemoryRepositories {
    public abstract class BaseInMemoryRepository<T> where T : class, IEntity {
        protected List<T> items = new();

        protected BaseInMemoryRepository(List<T> initialData = null) {
            items = initialData;
        }

        public virtual Task<T> AddAsync(T item) {
            item.Id = items.Any() ? items.Max(i => i.Id) + 1 : 1;
            items.Add(item);
            return Task.FromResult(item);
        }

        public virtual Task UpdateAsync(T item) {
            var index = items.FindIndex(i => i.Id == item.Id);
            if (index == -1) {
                throw new InvalidOperationException(
                    $"Item with id {item.Id} not found");
            }

            items[index] = item;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id) {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) {
                throw new InvalidOperationException(
                    $"Item with id {id} not found");
            }

            items.Remove(item);
            return Task.CompletedTask;
        }

        public Task<T> GetSingleAsync(int id) {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) {
                throw new InvalidOperationException(
                    $"Item with id {id} not found");
            }

            return Task.FromResult(item);
        }

        public IQueryable<T> GetMany() {
            return items.AsQueryable();
        }
    }
}