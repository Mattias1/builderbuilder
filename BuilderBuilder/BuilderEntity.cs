using System.Collections.Generic;

namespace BuilderBuilder
{
    class BuilderEntity
    {
        public string Name { get; set; }

        public List<Field> Fields { get; private set; }

        public BuilderEntity() {
            Fields = new List<Field>();
        }
    }

    struct Field
    {
        public string Type;
        public string Name;

        public Field(string type, string name) {
            Type = type;
            Name = name;
        }
    }
}
