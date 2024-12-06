using System;
using System.IO;
using System.Text;
using UnityEngine;
namespace Kitchen.Scripts.CodeGenerator
{
    /// <summary>
    /// Generate simple class with string consts
    /// </summary>
    public static class SimpleCodeGenerator
    {

        public static void Generate(ClassNode node)
        {
            node.Name = node.Name.Replace(" ", "_").UnderscoreToCamelCase();
            var writer = new StreamWriter(Application.dataPath + node.Path + node.Name + ".cs");
            var builder = new StringBuilder();
            try
            {
                builder.AppendLine("namespace " + node.Namespace);
                builder.AppendLine("{");
                builder.AppendLine("\tpublic static class " + node.Name);
                builder.AppendLine("\t{");
                foreach (var nodeProperty in node.Properties)
                {
                    builder.AppendLine("\t\tpublic const string " + nodeProperty.Name.Replace('.', '_').UnderscoreToCamelCase() + " = \"" + nodeProperty.Value + "\";");
                }
                builder.AppendLine("\t}");
                builder.AppendLine("}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            finally
            {
                writer.Write(builder.ToString());
                writer.Close();
            }
        }
        
        private static string UnderscoreToCamelCase(this string name)
        {
            if (string.IsNullOrEmpty(name) || !name.Contains("_"))
            {
                return name;
            }
            var array = name.Split('_');
            for(var i = 0; i < array.Length; i++)
            {
                var s = array[i];
                var first = string.Empty;
                var rest = string.Empty;
                if (s.Length > 0)
                {
                    first = char.ToUpperInvariant(s[0]).ToString();
                }
                if (s.Length > 1)
                {
                    rest = s[1..];
                }
                array[i] = first + rest;
            }
            var newname = string.Join("", array);
            if (newname.Length <= 0)
            {
                newname = name;
            }
            return newname;
        }
    }
}
