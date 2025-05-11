namespace BuilderBuilder;

public class BuilderEntity {
  public bool Persistable { get; set; }

  public string Name { get; set; }

  public List<Field> Fields { get; }

  public BuilderEntity(bool persistable) {
    Persistable = persistable;
    Name = "";
    Fields = [];
  }
}

public readonly struct Field {
  public enum InverseHandlingType { None, OneToOne, OneToMany, ManyToOne, ManyToMany };

  public readonly string Type;
  public readonly string Name;
  public readonly InverseHandlingType InverseHandling;

  public Field(string type, string name, InverseHandlingType inverseHandling = InverseHandlingType.None) {
    Type = type;
    Name = name;
    InverseHandling = inverseHandling;
  }
}
