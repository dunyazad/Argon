using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class ObjectBase
    {
        public string Name { get; set; }
        public Guid ID { get; private set; } = Guid.NewGuid();

        public ObjectBase(string name)
        {
            Name = name;
        }
    }
}
