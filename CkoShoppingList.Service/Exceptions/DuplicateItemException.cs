using System;

namespace CkoShoppingList.Service.Exceptions
{
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message)
            : base(message)
        {
        }
    }
}