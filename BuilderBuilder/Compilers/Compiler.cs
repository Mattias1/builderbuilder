using System.Text;

namespace BuilderBuilder
{
    abstract class Compiler
    {
        public virtual int TabSize => 4;

        protected virtual bool UseBrackets => true;

        protected BuilderEntity BuilderEntity { get; set; }

        protected StringBuilder StringBuilder { get; set; }
        private int _indent;

        public virtual string Compile(BuilderEntity entity) {
            BuilderEntity = entity;
            StringBuilder = new StringBuilder();

            Compile();

            return StringBuilder.ToString();
        }

        protected abstract void Compile();

        protected void AddEmptyLine() => AddEmptyLines(1);

        protected void AddEmptyLines(int numberOfLines) {
            for (int i = 0; i < numberOfLines; i++) {
                StringBuilder.AppendLine();
            }
        }

        protected void AddLine(string line) {
            StringBuilder.Append(new string(' ', _indent * TabSize));
            StringBuilder.AppendLine(line);
        }

        protected void OpenBlock() {
            if (UseBrackets) {
                AddLine("{");
            }
            _indent++;
        }

        protected void CloseBlock() => CloseBlocks(1);
        protected void CloseBlocks(int numberOfBlocks) {
            for (int i = 0; i < numberOfBlocks; i++) {
                _indent--;

                if (UseBrackets) {
                    AddLine("}");
                }
            }
        }

        protected string LocalVar(string name) {
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

        protected string PrivateVar(string name) {
            return '_' + LocalVar(name);
        }
    }
}
