using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorMapeos.TransferStructs
{
    public class ResultValue<T>: BaseResult
    {
        public T Value { get; set; }
    }
}