using System.Collections.Generic;

namespace BuilderBuilder
{
    public class BuilderEntity
    {
        public string Name { get; set; }

        public List<Field> Fields { get; private set; }

        public BuilderEntity() {
            Name = "";
            Fields = new List<Field>();
        }
    }

    public struct Field
    {
        public enum InverseHandlingType { None, OneToOne, OneToMany, ManyToOne, ManyToMany };

        public string Type;
        public string Name;
        public InverseHandlingType InverseHandling;

        public Field(string type, string name, InverseHandlingType inverseHandling = InverseHandlingType.None) {
            Type = type;
            Name = name;
            InverseHandling = inverseHandling;
        }
    }
}
