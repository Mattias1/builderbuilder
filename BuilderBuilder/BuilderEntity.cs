namespace BuilderBuilder;

public class BuilderEntity {
  public bool Persistable { get; set; }

  public string Name { get; set; }

  public List<Field> Fields { get; }

  public BuilderEntity(bool persistable) {
    Persistable = persistable;
    Name = "";
    Fields = new List<Field>();
  }
}

public struct Field {
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
