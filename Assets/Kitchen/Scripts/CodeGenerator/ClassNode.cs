﻿namespace Kitchen.Scripts.CodeGenerator
{
    public struct ClassNode
    {
        public string Name;
        public string Path;
        public string Namespace;
        public PropertyNode<string>[] Properties;
    }

}
