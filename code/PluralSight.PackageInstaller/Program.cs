using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight.PackageInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            var packages = ParsePackages(args);
        }

        public static IEnumerable<Tuple<string, string>> ParsePackages(string[] packages)
        {
            var lstPackages = new List<Tuple<string, string>>();
            foreach (var package in packages)
            {
                var packageName = package.Substring(0, package.IndexOf(":"));
                var packageDependency = package.Substring(package.IndexOf(":") + 1, package.Length - (package.IndexOf(":") + 1));

                packageName = packageName.Replace("\r\n", string.Empty);

                lstPackages.Add(new Tuple<string, string>(packageName, packageDependency));
            }

            return lstPackages;
        }
    }
}
