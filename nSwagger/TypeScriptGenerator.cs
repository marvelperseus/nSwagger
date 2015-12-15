﻿namespace nSwagger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class CoderStringBuilder
    {
        private readonly StringBuilder stringBuilder;
        private string _padding;
        private int indent = 0;

        public CoderStringBuilder()
        {
            stringBuilder = new StringBuilder();
        }

        public void AppendLine()
        {
            stringBuilder.AppendLine();
        }

        public void AppendLine(string line, bool noPadding = false)
        {
            var padding = "";
            if (!noPadding)
            {
                padding = _padding;
            }

            stringBuilder.AppendLine(padding + line);
        }

        public void Indent()
        {
            indent += 4;
            _padding = "".PadRight(indent, ' ');
        }

        public void Outdent()
        {
            indent -= 4;
            _padding = "".PadRight(indent, ' ');
        }

        public override string ToString() => stringBuilder.ToString();

        internal void Append(string content, bool noPadding = false)
        {
            var padding = "";
            if (!noPadding)
            {
                padding = _padding;
            }

            stringBuilder.Append(padding + content);
        }
    }

    public class TypeScriptGenerator
    {
        private List<string> existingInterfaces = new List<string>();
        private Regex splitOutGeneric = new Regex("(?<generic>\\w+)\\[(?<type>\\w+)]");

        public string ItemTypeCleaner(Item item)
        {
            if (!string.IsNullOrWhiteSpace(item.Ref))
            {
                return RefToClass(item.Ref);
            }

            if (item.Type.Equals("array", StringComparison.OrdinalIgnoreCase))
            {
                return "[]";
            }

            return JsonToJSTypeConverter(item.Type);
        }

        public void Run(Specification[] specifications, Configuration swaggerConfig, string target)
        {
            existingInterfaces.Add("any");
            var output = new CoderStringBuilder();
            output.AppendLine($"// This file was autogenerated by nSwagger {swaggerConfig.nSwaggerVersion} - changes made to it maybe lost if nSwagger is run again");
            output.AppendLine($"namespace {swaggerConfig.Namespace} {{");
            output.Indent();
            foreach (var specification in specifications)
            {
                Process(output, specification);
            }

            output.Outdent();
            output.AppendLine("}");

            File.WriteAllText(target, output.ToString());
        }

        private void AddAPICall(CoderStringBuilder output, Operation operation)
        {
            if (operation == null)
            {
                return;
            }

            var parameters = "";
            if (operation.Parameters != null)
            {
                var optional = "";
                if (operation.Parameters.All(_ => !_.Required))
                {
                    optional = "?";
                }

                parameters = $"parameters{optional}: {operation.OperationId}Request";
            }

            var operationContent = new StringBuilder(operation.OperationId + "(" + parameters + "): ");
            var success = operation.Responses.FirstOrDefault(_ => _.HttpStatusCode >= 200 && _.HttpStatusCode <= 299);
            if (success == null || success.Schema == null)
            {
                operationContent.Append("PromiseLike<void>;");
            }
            else
            {
                operationContent.Append($"PromiseLike<{SchemaTypeCleaner(success.Schema)}>;");
            }

            output.AppendLine(operationContent.ToString());
        }

        private void AddAPIRequest(CoderStringBuilder output, Operation operation)
        {
            if (operation == null)
            {
                return;
            }

            if (operation.Parameters != null)
            {
                AddParameterInterface(output, operation.OperationId + "Request", operation.Parameters);
            }
        }

        private void AddClass(CoderStringBuilder output, string sourceName, Property[] properties)
        {
            var enums = new List<EnumInfo>();
            var name = CleanClassName(sourceName);
            if (existingInterfaces.Contains(name))
            {
                return;
            }

            existingInterfaces.Add(name);

            output.AppendLine($"export class {name} {{");
            output.Indent();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyType = PropertyTypeCleaner(property);
                if (property.Enum != null)
                {
                    propertyType = "string";
                    enums.Add(new EnumInfo
                    {
                        EnumPropertyName = propertyName,
                        EnumValues = property.Enum,
                        EnumClassName = name + propertyName
                    });
                }

                output.AppendLine($"{propertyName}: {propertyType};");
            }

            output.Outdent();
            output.AppendLine("}");
            output.AppendLine();

            foreach (var @enum in enums)
            {
                output.AppendLine($"export enum {@enum.EnumClassName} {{");
                output.Indent();
                var addComma = false;
                foreach (var enumValue in @enum.EnumValues)
                {
                    if (!addComma)
                    {
                        addComma = true;
                    }
                    else
                    {
                        output.AppendLine(",", true);
                    }

                    output.Append(enumValue);
                }
                output.AppendLine();
                output.Outdent();
                output.AppendLine("}");
                output.AppendLine();
            }
        }

        private void AddParameterInterface(CoderStringBuilder output, string sourceName, Parameter[] parameters)
        {
            var name = CleanClassName(sourceName);
            if (existingInterfaces.Contains(name))
            {
                return;
            }

            existingInterfaces.Add(name);

            output.AppendLine($"export interface {name} {{");
            output.Indent();
            foreach (var parameter in parameters)
            {
                var propertyName = parameter.Name;
                if (!parameter.Required)
                {
                    propertyName += "?";
                }

                var propertyType = "any";
                var bodyParameter = parameter as BodyParameter;
                if (bodyParameter != null)
                {
                    propertyType = SchemaTypeCleaner(bodyParameter.Schema);
                }

                var otherParameter = parameter as OtherParameter;
                if (otherParameter != null)
                {
                    var arrayParameter = parameter as OtherArrayParameter;
                    if (arrayParameter != null)
                    {
                        propertyType = CleanClassName(arrayParameter.Items[0].Type) + "[]";
                    }
                    else
                    {
                        propertyType = CleanClassName(otherParameter.Type);
                    }
                }

                output.AppendLine($"{propertyName}: {propertyType};");
            }

            output.Outdent();
            output.AppendLine("}");
            output.AppendLine();
        }

        private string CleanClassName(string sourceName) => JsonToJSTypeConverter(sourceName.Replace("[", "").Replace("]", ""));

        private string JsonToJSTypeConverter(string jsonType)
        {
            if (jsonType.Equals("object", StringComparison.OrdinalIgnoreCase))
            {
                return "any";
            }

            if (jsonType.Equals("integer", StringComparison.OrdinalIgnoreCase))
            {
                return "number";
            }

            return jsonType;
        }

        private void Process(CoderStringBuilder output, Specification specification)
        {
            output.AppendLine($"export module {specification.Info.Title} {{");
            output.Indent();
            foreach (var defination in specification.Definations)
            {
                AddClass(output, defination.Name, defination.Properties);
            }

            foreach (var path in specification.Paths)
            {
                AddAPIRequest(output, path.Delete);
                AddAPIRequest(output, path.Get);
                AddAPIRequest(output, path.Head);
                AddAPIRequest(output, path.Options);
                AddAPIRequest(output, path.Patch);
                AddAPIRequest(output, path.Post);
                AddAPIRequest(output, path.Put);
            }

            output.AppendLine("export interface API {");
            output.Indent();
            output.AppendLine("setToken(value:string, headerOrQueryName:string, isQuery:boolean):void;");

            foreach (var path in specification.Paths)
            {
                AddAPICall(output, path.Delete);
                AddAPICall(output, path.Get);
                AddAPICall(output, path.Head);
                AddAPICall(output, path.Options);
                AddAPICall(output, path.Patch);
                AddAPICall(output, path.Post);
                AddAPICall(output, path.Put);
            }

            output.Outdent();
            output.AppendLine("}");

            output.Outdent();
            output.AppendLine("}");
        }

        private string PropertyTypeCleaner(Property property)
        {
            if (!string.IsNullOrWhiteSpace(property.Ref))
            {
                return RefToClass(property.Ref);
            }

            if (property.Type.Equals("array", StringComparison.OrdinalIgnoreCase))
            {
                return RefToClass(property.ArrayItemType) + "[]";
            }

            return JsonToJSTypeConverter(property.Type);
        }

        private string RefToClass(string @ref) => @ref.Substring(@ref.IndexOf("/", 2, StringComparison.Ordinal) + 1);

        private string SchemaTypeCleaner(Schema property)
        {
            if (!string.IsNullOrWhiteSpace(property.Ref))
            {
                return CleanClassName(RefToClass(property.Ref));
            }

            if (property.Type.Equals("array", StringComparison.OrdinalIgnoreCase))
            {
                var array = "[]";
                if (property.Items != null && property.Items.Length == 1)
                {
                    array = CleanClassName(ItemTypeCleaner(property.Items[0]) + "[]");
                }
                else {
                    array = "any[]";
                }

                return array;
            }

            return CleanClassName(property.Type);
        }

        private class EnumInfo
        {
            public string EnumClassName { get; set; }

            public string EnumPropertyName { get; set; }

            public string[] EnumValues { get; set; }
        }
    }
}