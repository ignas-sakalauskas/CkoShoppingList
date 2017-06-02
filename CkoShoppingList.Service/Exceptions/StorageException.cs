using System;

namespace CkoShoppingList.Service.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException(string message)
            : base(message)
        {
        }
    }
}