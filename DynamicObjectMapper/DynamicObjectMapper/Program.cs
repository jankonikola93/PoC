using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace DynamicObjectMapper
{
    static class Program
    {
        static void Main(string[] args)
        {
            var destination = Map(LoadDestinationTemplate(), string.Empty, LoadSource(), LoadMappings());
            Console.WriteLine(destination?.ToString());
        }

        private static JObject Map(JToken destinatinoTemplate, string parentPath, JObject source, IList<Mapping> mappings, IList<int> indexes = null)
        {
            indexes = indexes ?? new List<int>();
            JsonReader reader = destinatinoTemplate.CreateReader();
            JTokenWriter writer = new JTokenWriter();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Integer:
                    case JsonToken.Raw:
                    case JsonToken.String:
                        var relativePath = GetRelativePath(reader.Path, parentPath);
                        var mapping = mappings.FirstOrDefault(x => x.Destination == relativePath);
                        if (mapping == null)
                        {
                            writer.WriteNull();
                        }
                        else
                        {
                            var absoluteSourcePath = GetAbsolutePath(mapping.Source, indexes);
                            writer.WriteValue(source.SelectToken(absoluteSourcePath));
                        }
                        break;
                    case JsonToken.StartArray:
                        var jsonArrayTemplate = (JArray)destinatinoTemplate.SelectToken(reader.Path);
                        var jsonArrayElementTemplate = jsonArrayTemplate?.FirstOrDefault();
                        var relativeArrayPath = GetRelativePath(reader.Path, parentPath);
                        var arrayMapping = mappings.FirstOrDefault(x => x.Destination == relativeArrayPath);
                        if (arrayMapping == null)
                        {
                            writer.WriteNull();
                            reader.Skip();
                            break;
                        }

                        var absoluteArrayPath = GetAbsolutePath(arrayMapping.Source, indexes);
                        var sourceArray = source.SelectToken(absoluteArrayPath);
                        if (sourceArray == null || sourceArray as JArray == null)
                        {
                            writer.WriteNull();
                            reader.Skip();
                            break;
                        }

                        var sourceJArray = (JArray)sourceArray;
                        writer.WriteStartArray();
                        if (jsonArrayElementTemplate == null)
                        {
                            foreach (var item in sourceJArray)
                            {
                                writer.WriteValue(item);
                            }
                        }
                        else
                        {
                            var jsonArrayElementTemplateToken = JToken.Parse(jsonArrayElementTemplate.ToString());
                            foreach (var item in sourceJArray)
                            {
                                var mappedArrayValue = Map(jsonArrayElementTemplateToken, relativeArrayPath, source, mappings, indexes.Append(sourceJArray.IndexOf(item)).ToList());
                                writer.WriteRawValue(mappedArrayValue?.ToString());
                            }
                        }
                        writer.WriteEndArray();
                        reader.Skip();
                        break;
                    case JsonToken.EndArray:
                        break;
                    default: writer.WriteToken(reader, false); break;
                }
            }

            return (JObject)writer.Token;
        }

        private static string GetRelativePath(string path, string parentPath)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var relativePath = Regex.Replace(path, @"\[[0-9]*\]", string.Empty);
            relativePath = string.IsNullOrEmpty(parentPath) ? relativePath : $"{parentPath}.{relativePath}";

            return relativePath;
        }

        private static string GetAbsolutePath(string path, IList<int> indexes)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var absolutePath = path;
            if (indexes != null && indexes.Any())
            {
                var regex = new Regex(@"\*");
                foreach (var item in indexes)
                {
                    absolutePath = regex.Replace(absolutePath, item.ToString(), 1);
                }
            }

            return absolutePath;
        }
        private static IList<Mapping> LoadMappings()
        {
            return new List<Mapping>
            {
                new Mapping
                {
                    Source = "Manufacturers[0].Name",
                    Destination = "Info.Manufacturer",
                },
                new Mapping
                {
                    //Source = "Manufacturers[0].Products[0].Name",
                    Source = "$.Manufacturers[?(@.Name == 'Contoso')].Products[0].Name",
                    Destination = "Product",
                },
                new Mapping
                {
                    Source = "Manufacturers[0].Products[0].Price",
                    Destination = "Info.Price",
                },
                // new Mapping
                // {
                //     Source = "Manufacturers[1].Products",
                //     Destination = "Products",
                // },
                // new Mapping
                // {
                //     Source = "Manufacturers[1].Products[*].Name",
                //     Destination = "Products.Name",
                // },
                // new Mapping
                // {
                //     Source = "Manufacturers[1].Products[*].Price",
                //     Destination = "Products.Price",
                // },
                new Mapping
                {
                    Source = "Stores",
                    Destination = "Prodavnice",
                },
                new Mapping
                {
                    Source = "Manufacturers",
                    Destination = "Proizvodjaci",
                },
                new Mapping
                {
                    Source = "Manufacturers[*].Name",
                    Destination = "Proizvodjaci.Name",
                },
                new Mapping
                {
                    Source = "Manufacturers[*].Products",
                    Destination = "Proizvodjaci.Products",
                },
                new Mapping
                {
                    Source = "Manufacturers[*].Products[*].Name",
                    Destination = "Proizvodjaci.Products.Name",
                },
                new Mapping
                {
                    Source = "Manufacturers[*].Products[*].Price",
                    Destination = "Proizvodjaci.Products.Price",
                },
                new Mapping
                {
                    Source = "Manufacturers[*].Products[*].Tags",
                    Destination = "Proizvodjaci.Products.Tags",
                },
            };
        }

        private static JObject LoadDestinationTemplate()
        {
            //return JObject.Parse(@"{ 'Products': [{'Name': '', 'Price': 0}] }");
            //return JObject.Parse(@"{ 'Stores': [], 'Products': [{'Name': '', 'Price': 0}] }");
            return JObject.Parse(@"{ 'Prodavnice': [], 'Proizvodjaci': [ {'Name': '', 'Products': [{'Name': '', 'Price': 0, 'Tags': [] }] } ] }");
            //return JObject.Parse(@"{ 'Proizvodjaci': [ {'Name': '', 'Products': [{'Name': '', 'Price': 0}]} ] }");
            //return JObject.Parse(@"{ 'Stores': [], 'Manufacturers': [ {'Name': ''} ] }");
            //return JObject.Parse(@"{ 'Product': '', 'Info': {'Manufacturer': '', 'Price': 0} }");
        }

        private static JObject LoadSource()
        {
            return JObject.Parse(@"{
                'Stores': [
                    'Lambton Quay',
                    'Willis Street'
                ],
                'Manufacturers': [
                    {
                    'Name': 'Acme Co',
                    'Products': [
                        {
                        'Name': 'Anvil',
                        'Price': 50,
                        'Tags': ['A', 'B', 'C']
                        }
                    ]
                    },
                    {
                    'Name': 'Contoso',
                    'Products': [
                        {
                        'Name': 'Elbow Grease',
                        'Price': 99.95
                        },
                        {
                        'Name': 'Headlight Fluid',
                        'Price': 4
                        }
                    ]
                    }
                ]
                }");
        }
    }
}
