using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace AssemblyInfoCollecting
{
    class Program
    {
        private const string INSPECTED_ASSEMBLY = @"C:\Users\janko\source\repos\PoC\DynamicObjectMapper\bin\Debug\net5.0\DynamicObjectMapper.dll";
        static void Main(string[] args)
        {
            // Get the array of runtime assemblies.
            var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");

            // Create the list of assembly paths consisting of runtime assemblies and the inspected assembly.
            var paths = new List<string>(runtimeAssemblies);
            paths.Add(INSPECTED_ASSEMBLY);

            // Create PathAssemblyResolver that can resolve assemblies using the created list.
            var resolver = new PathAssemblyResolver(paths);
            var metadataLoadContext = new MetadataLoadContext(resolver);

            using (metadataLoadContext)
            {
                // Load assembly into MetadataLoadContext.
                Assembly assembly = metadataLoadContext.LoadFromAssemblyPath(INSPECTED_ASSEMBLY);
                AssemblyName name = assembly.GetName();

                // Print assembly information.
                Console.WriteLine($"{name.Name}: ");

                var types = assembly.GetTypes();

                // Print assembly types
                Console.WriteLine("    Types:");
                foreach (var item in types)
                {
                    Console.WriteLine($"        {item.Name}");
                    Console.WriteLine("            Methods:");
                    foreach (var method in item.GetMethods(
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Instance |
                        BindingFlags.Static |
                        BindingFlags.DeclaredOnly
                        ).Where(x => !x.IsSpecialName))
                    {
                        Console.WriteLine($"                {method.Name}");
                    }

                    Console.WriteLine("            Fields:");
                    foreach (var field in item.GetFields())
                    {
                        Console.WriteLine($"                {field.Name}");
                    }

                    Console.WriteLine("            Properties:");
                    foreach (var property in item.GetProperties())
                    {
                        Console.WriteLine($"                {property.Name}");
                    }
                }

                // Print assembly namespaces
                Console.WriteLine("    Namespaces:");
                foreach (var item in types.GroupBy(x => x.Namespace))
                {
                    Console.WriteLine($"        {item.Key}");
                }

                var fererencedAssemblies = assembly.GetReferencedAssemblies();

                // Print referenced assemblies
                Console.WriteLine("    Referenced assemblies:");
                foreach (var item in fererencedAssemblies)
                {
                    Console.WriteLine($"        {item.Name}");
                }
            }
        }
    }
}
