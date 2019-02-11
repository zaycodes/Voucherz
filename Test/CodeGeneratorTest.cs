using NUnit.Framework;
using System;
using VoucherServiceBL.Util;

namespace Test
{
    public class CodeGeneratorTest
    {
        [Test]
        public static void GenerateCodeWithPattern_shouldReturnRandomCode_WithCorrectPattern()
        {
            var pattern = "##-###";
            var chars = "f3467fj56565i";
            var separator = "-";

            var genCode = CodeGenerator.GenerateCodeWithPattern(pattern, chars, separator);

            Console.WriteLine($">>>>>{genCode}>>>>>>");
            Assert.That(() => genCode.Contains("-"));
            Assert.That(() =>
            {
                return genCode.Substring(0, genCode.IndexOf("-")).Length == 2 &&
                genCode.Substring(genCode.IndexOf("-") + 1).Length == 3;
            });
        }
    }
}
