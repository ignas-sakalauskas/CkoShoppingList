using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CkoShoppingList.Service.Exceptions;

namespace CkoShoppingList.Service.Services
{
    public class StorageService : IStorageService
    {
        private readonly ConcurrentDictionary<string, int> _drinksDictionary;

        public StorageService(ConcurrentDictionary<string, int> concurrentDictionary)
        {
            _drinksDictionary = concurrentDictionary ?? throw new ArgumentNullException(nameof(concurrentDictionary));
        }

        public IList<KeyValuePair<string, int>> GetDrinks()
        {
            return _drinksDictionary.ToList();
        }

        public KeyValuePair<string, int> GetDrink(string name)
        {
            if (!_drinksDictionary.ContainsKey(name))
            {
                throw new ItemNotFoundException($"Drink '{name}' not found.");
            }

            if (!_drinksDictionary.TryGetValue(name, out int value))
            {
                throw new StorageException($"Error retrieving value for '{name}' from storage.");
            }

            return new KeyValuePair<string, int>(name, value);
        }

        public KeyValuePair<string, int> AddDrink(KeyValuePair<string, int> drink)
        {
            if (_drinksDictionary.ContainsKey(drink.Key))
            {
                throw new DuplicateItemException($"Drink '{drink.Key}' already exists.");
            }

            if (!_drinksDictionary.TryAdd(drink.Key, drink.Value))
            {
                throw new StorageException($"Error adding '{drink.Key}' into storage.");
            }

            return drink;
        }

        public KeyValuePair<string, int> UpdateDrink(string name, KeyValuePair<string, int> drink)
        {
            if (!_drinksDictionary.ContainsKey(name))
            {
                throw new ItemNotFoundException($"Drink '{name}' not found.");
            }

            if (!_drinksDictionary.TryGetValue(name, out int currentValue))
            {
                throw new StorageException($"Error retrieving value for '{name}' from storage.");
            }

            if (!_drinksDictionary.TryUpdate(name, drink.Value, currentValue))
            {
                throw new StorageException($"Error updating value for '{name}' from storage.");
            }

            return new KeyValuePair<string, int>(name, drink.Value);
        }

        public void DeleteDrink(string name)
        {
            if (!_drinksDictionary.ContainsKey(name))
            {
                throw new ItemNotFoundException($"Drink '{name}' not found.");
            }

            if (!_drinksDictionary.TryRemove(name, out int _))
            {
                throw new StorageException($"Error deleting '{name}' from storage.");
            }
        }
    }
}