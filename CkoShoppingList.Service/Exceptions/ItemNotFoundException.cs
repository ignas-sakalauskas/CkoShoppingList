using System;

namespace CkoShoppingList.Service.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message)
            : base(message)
        {
        }
    }
}