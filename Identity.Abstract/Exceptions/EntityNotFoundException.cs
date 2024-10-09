using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Abstract.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(Type type, string key) : base(string.Format($"Entity {type} with id {key} does not exist")) { }
        public EntityNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
