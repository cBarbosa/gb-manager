using System.Collections.Generic;
using System.Linq;

namespace gb_manager.Domain.Shared
{
    public class CommandResult
    {
        public CommandResult(
            bool success
            , string message
            , object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int? Quantity {
            get
            {
                if (Data != null && (Data.GetType().ReflectedType != null && Data.GetType().ReflectedType.Name.Equals("Enumerable")))
                {
                    if (Data.GetType().ReflectedType.Name.Equals("Enumerable") && (((IEnumerable<object>)Data).Count() > 0))
                    {
                        return ((IEnumerable<object>)Data).Count();
                    }
                }
                return null;
            }
        }
        public object Data { get; set; }
    }
}