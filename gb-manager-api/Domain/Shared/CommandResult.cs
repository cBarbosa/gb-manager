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
                if (Data.GetType().IsSerializable)
                {
                    if (Data.GetType().IsSerializable && (((IEnumerable<object>)Data).Count() > 1))
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