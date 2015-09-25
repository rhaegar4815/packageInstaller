using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PluralSight.UnitTest
{
    [TestClass]
    public class PackageIntallerTests
    {
        private readonly string[] _validInput =
        {
            "KittenService:",
            "Leetmeme: Cyberportal",
            "Cyberportal: Ice",
            "CamelCaser: KittenService",
            "Fraudstream: Leetmeme",
            "Ice:"
        };

        private readonly string[] _invalidInput =
        {
            "KittenService:",
            "Leetmeme: Cyberportal",
            "Cyberportal: Ice",
            "CamelCaser: KittenService",
            "Fraudstream:",
            "Ice: Leetmeme"
        };

        private readonly string[] _missingInput =
        {
            "KittenService:",
            "Leetmeme: Cyberportal",
            "Cyberportal: Ice",
            "CamelCaser: KittenService",
            "Fraudstream:"
        };

        private const string VALID_OUTPUT = "KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParsePackageFailure()
        {
            var packages = PackageInstaller.Program.ParsePackages(null);
        }

        [TestMethod]
        public void ParsePackageSuccess()
        {
            var packages = PackageInstaller.Program.ParsePackages(_validInput);
            
            Assert.IsNotNull(packages);
            Assert.AreEqual(packages.Count(), 6);
        }

        [TestMethod]
        [ExpectedException(typeof (PackageInstaller.PackageInputMissingException))]
        public void PackageValidationMissingFailure()
        {
            var packages = PackageInstaller.Program.ParsePackages(_missingInput);
            var output = PackageInstaller.Program.ValidatePackages(packages);
        }

        [TestMethod]
        [ExpectedException(typeof (PackageInstaller.PackageInputLoopException))]
        public void PackageValidationLoopFailure()
        {
            var packages = PackageInstaller.Program.ParsePackages(_invalidInput);
            var output = PackageInstaller.Program.ValidatePackages(packages);
        }

        [TestMethod]
        public void PackageValidationSuccess()
        {
            var packages = PackageInstaller.Program.ParsePackages(_validInput);
            var output = PackageInstaller.Program.ValidatePackages(packages);

            Assert.AreEqual(output, VALID_OUTPUT);
        }
    }
}
