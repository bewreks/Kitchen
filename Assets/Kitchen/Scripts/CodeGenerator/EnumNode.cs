namespace Kitchen.Scripts.CodeGenerator
{
    public struct EnumNode
    {
        public string Name;
        public string Path;
        public string Namespace;
        public PropertyNode<int>[] Properties;
    }
}
