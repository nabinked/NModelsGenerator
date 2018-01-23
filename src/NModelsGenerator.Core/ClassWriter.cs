using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using NModelsGenerator.Common;
using DbColumn = NModelsGenerator.Common.DbColumn;

namespace NModelsGenerator.Core
{
    public class ClassWriter
    {
        private readonly string singleTab = "\t";
        private string doubleTab = "\t\t";
        private readonly string _nameSpace;
        private readonly string _className;
        private readonly ExpandoObject _classAttributes;
        private readonly IDictionary<string, string> _variables;
        private readonly List<string> _baseTypeList;
        private readonly HashSet<string> _imports;
        private readonly List<string> _syntaxLines;
        private readonly IEnumerable<string> _baseClassProperties;

        public ClassWriter(string nameSpace, string className,
                            List<string> imports = null,
                            List<string> baseTypeList = null,
                            ExpandoObject classAttributes = null,
                            IDictionary<string, string> variables = null,
                            IEnumerable<string> baseClassProperties = null)
        {
            _nameSpace = nameSpace;
            _className = className;
            _classAttributes = classAttributes;
            _variables = variables;
            _baseTypeList = baseTypeList ?? new List<string>();
            _imports = InitializeImports(imports);
            _baseClassProperties = baseClassProperties ?? new List<string>();

            _syntaxLines = new List<string>();

        }

        private HashSet<string> InitializeImports(List<string> imports)
        {
            var importSet = new HashSet<string>(imports ?? new List<string>()) { "System" };
            return importSet;
        }

        public void Start(IList<DbColumn> properties, string filePath)
        {
            AddUsingDirectives();
            AddNewLine();
            AddNameSpace();
            AddNewLine();
            AddOpeningCurlyBraces();
            AddClassAttributes();
            AddClassDeclaration();
            AddOpeningCurlyBraces(singleTab);
            AddProperties(properties);
            AddClosingCurlyBraces(singleTab);
            AddClosingCurlyBraces();

            WriteToFile(filePath);
        }

        private void AddProperties(IList<DbColumn> columns)
        {
            foreach (DbColumn column in columns)
            {
                if (_baseClassProperties.Any() && _baseClassProperties.Contains(column.ColumnName.ToPasacalCase().ToLower())) continue;
                _syntaxLines.Add($"{doubleTab}public {GetDataType(column.DataType)} {column.ColumnName.ToPasacalCase()} {{get; set;}}");
                AddNewLine();
            }
        }

        private void AddNewLine()
        {
            _syntaxLines.Add("");
        }

        private void AddClosingCurlyBraces(string tab = "")
        {
            _syntaxLines.Add($"{tab}}}");
        }

        private void AddOpeningCurlyBraces(string tab = "")
        {
            _syntaxLines.Add($"{tab}{{");
        }

        private void AddClassDeclaration()
        {
            var declaration = $"{singleTab}public class {_className}";
            if (_baseTypeList.Any())
            {
                declaration += $" : {string.Join(", ", _baseTypeList)}";
            }

            _syntaxLines.Add(declaration);
        }

        private void AddClassAttributes()
        {
            var attributes = GetClassAttributes();
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));
            foreach (var attribute in attributes)
            {
                var line = $"{singleTab + attribute}";
                _syntaxLines.Add(ReplaceVariables(line));
            }

        }

        private List<string> GetClassAttributes()
        {
            var classAttributeList = new List<string>();
            var attributes = (IDictionary<string, object>)_classAttributes;
            if (_classAttributes != null && _classAttributes.Any())
                foreach (var attribute in attributes)
                {
                    var attributeString = "[";
                    attributeString += attribute.Key;
                    if (attribute.Value is ExpandoObject)
                    {
                        var properties = (IDictionary<string, object>)attribute.Value;
                        if (properties != null && properties.Any())
                        {
                            attributeString += "(";

                            foreach (var property in properties)
                            {
                                object finalValue = ReplaceVariables(property.Value.ToString());
                                if (bool.TryParse(finalValue.ToString(), out var boolValue))
                                {
                                    finalValue = boolValue;
                                }
                                else
                                {
                                    finalValue = $"\"{finalValue}\"";
                                }
                                attributeString += $"{property.Key} = {finalValue}";
                                if (property.Key != properties.Last().Key)
                                {
                                    attributeString += ", ";
                                }
                            }

                            attributeString += ")";
                        }
                    }
                    else if (attribute.Value is string)
                    {
                        attributeString += $"(\"{attribute.Value}\")";
                    }
                    attributeString += "]";
                    classAttributeList.Add(attributeString);
                }
            return classAttributeList;
        }

        private string ReplaceVariables(string value)
        {

            foreach (var variable in _variables)
            {
                value = value.Replace(variable.Key, variable.Value);
            }
            return value;
        }

        private void AddNameSpace()
        {
            _syntaxLines.Add($"namespace {_nameSpace}");
        }

        private void WriteToFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllLines(path, _syntaxLines);
        }

        private void AddUsingDirectives()
        {
            foreach (var import in _imports)
            {
                _syntaxLines.Add($"using {import};");
            }
        }

        private string GetDataType(string dataType)
        {
            switch (dataType)
            {
                //Number
                case "bigint":
                    return "long";
                case "int":
                    return "int";
                case "integer":
                    return "int";
                case "smallint":
                    return "short";
                case "money":
                    return "decimal";
                case "double precision":
                    return "decimal";
                case "numeric":
                    return "decimal";


                //Date
                case "date":
                    return "DateTime";
                case "time without time zone":
                    return "DateTime";
                case "timestamp with time zone":
                    return "DateTime";
                case "timestamp without time zone":
                    return "DateTime";

                case "boolean":
                    return "bool";


                default:
                    return "string";
            }
        }
    }
}
