using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public abstract class BaseFileRepository<T> where T : class, IEntity {
    private readonly string _filePath;

    protected BaseFileRepository(string filePath) {
        this._filePath = filePath;
    }

    public async Task<T> AddAsync(T item) {
        string itemsJson = await File.ReadAllTextAsync(_filePath);
        List<T> items = JsonSerializer.Deserialize<List<T>>(itemsJson) ?? new List<T>();

        int maxId = items.Count > 0 ? items.Max(i => i.Id) : 0;
        item.Id = maxId + 1;

        items.Add(item);
        itemsJson = JsonSerializer.Serialize(items);
        await File.WriteAllTextAsync(_filePath, itemsJson);
        return item;
    }

    public async Task UpdateAsync(T item) {
        string itemsJson = await File.ReadAllTextAsync(_filePath);
        List<T> items = JsonSerializer.Deserialize<List<T>>(itemsJson) ?? new List<T>();

        int index = items.FindIndex(i => i.Id == item.Id);
        if (index == -1) {
            throw new InvalidOperationException($"Item with id {item.Id} not found");
        }

        items[index] = item;
        itemsJson = JsonSerializer.Serialize(items);
        await File.WriteAllTextAsync(_filePath, itemsJson);
    }

    public async Task DeleteAsync(int id) {
        string itemsJson = await File.ReadAllTextAsync(_filePath);
        List<T> items = JsonSerializer.Deserialize<List<T>>(itemsJson) ?? new List<T>();

        int index = items.FindIndex(i => i.Id == id);
        if (index == -1) {
            throw new InvalidOperationException($"Item with id {id} not found");
        }

        items.RemoveAt(index);
        itemsJson = JsonSerializer.Serialize(items);
        await File.WriteAllTextAsync(_filePath, itemsJson);
    }

    public async Task<T> GetSingleAsync(int id) {
        string itemsJson = await File.ReadAllTextAsync(_filePath);
        List<T> items = JsonSerializer.Deserialize<List<T>>(itemsJson) ?? new List<T>();

        T item = items.FirstOrDefault(i => i.Id == id);
        if (item == null) {
            throw new InvalidOperationException($"Item with id {id} not found");
        }

        return item;
    }

    public IQueryable<T> GetMany() {
        string itemsJson = File.ReadAllTextAsync(_filePath).Result;
        List<T> items = JsonSerializer.Deserialize<List<T>>(itemsJson) ?? new List<T>();
        return items.AsQueryable();
    }
}