using System;
using System.Collections.Generic;
using System.Linq;

namespace PluralSight.PackageInstaller
{
    public class Program
    {
        private static List<string> _iteratedPackages = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Parsing package input...");
            var packages = ParsePackages(args);

            try
            {
                Console.WriteLine("Validating package dependencies...");
                var output = ValidatePackages(packages);
                Console.WriteLine("Validation passed.");
                Console.Write("Output: {0}", output);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Validation failed;");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        public static IEnumerable<Tuple<string, string>> ParsePackages(string[] packages)
        {
            if(packages == null)
                throw new ArgumentNullException();

            var lstPackages = new List<Tuple<string, string>>();
            foreach (var package in packages)
            {
                var packageName = package.Substring(0, package.IndexOf(":"));
                var packageDependency = package.Substring(package.IndexOf(":") + 1, package.Length - (package.IndexOf(":") + 1));

                packageName = packageName.Replace("\r\n", string.Empty);
                packageDependency = packageDependency.Trim();

                lstPackages.Add(new Tuple<string, string>(packageName, packageDependency));
            }

            return lstPackages;
        }

        public static string ValidatePackages(IEnumerable<Tuple<string, string>> packages)
        {
            _iteratedPackages = new List<string>();
            
            var lstPkgInstallOrder = new List<string>();
            foreach (var package in packages)
            {
                if (lstPkgInstallOrder.Contains(package.Item1)) continue;
                if (String.IsNullOrWhiteSpace(package.Item2) || lstPkgInstallOrder.Contains(package.Item2))
                {
                    lstPkgInstallOrder.Add(package.Item1);
                    continue;
                }

                var dependency = GetPackageDependency(packages, package.Item2);
                var requiredDependencies = dependency.Split(',').Where(d => !lstPkgInstallOrder.Contains(d));
                
                lstPkgInstallOrder.AddRange(requiredDependencies);
                lstPkgInstallOrder.Add(package.Item1);
            }

            return String.Join(", ", lstPkgInstallOrder);
        }

        private static string GetPackageDependency(IEnumerable<Tuple<string, string>> packages, string dependency)
        {
            var package = packages.FirstOrDefault(p => p.Item1 == dependency);
            if (package == null)
                throw new PackageInputMissingException();

            if(_iteratedPackages.Contains(dependency))
                throw new PackageInputLoopException();
            _iteratedPackages.Add(dependency);

            if (String.IsNullOrWhiteSpace(package.Item2))
                return package.Item1;

            var packageDependency = GetPackageDependency(packages, package.Item2);
            return String.Join(",", packageDependency, dependency);
        }
    }

    public class PackageInputMissingException : Exception
    {
        public PackageInputMissingException() : base("Package input is missing") { }
    }

    public class PackageInputLoopException : Exception
    {
        public PackageInputLoopException() : base("Package dependency loop") { }
    }
}
