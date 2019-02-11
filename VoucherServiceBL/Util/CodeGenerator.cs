using System;
using System.Collections.Generic;
using System.Text;
using VoucherServiceBL.Model;

namespace VoucherServiceBL.Util
{
    public class CodeGenerator
    {
        /// <summary>
        /// Create a code with specific length and Character set
        /// </summary>
        /// <param name="length">the number of characters to be Generated e.g A code with length of 5 (12345)</param>
        /// <param name="characterSet">A pool of Characters to generate number from</param>
        /// <returns></returns>
        public static string GenerateCode(int length, string characterSet)
        {
            Random random = new Random();
            StringBuilder result;
            // Generate a random number
            result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characterSet[random.Next(characterSet.Length)]);
            }
            return result.ToString();
        }
                
        /// <summary>
        /// Creates a code specifying a pattern, characterset and a separator
        /// </summary>
        /// <param name="pattern">a sequence of # characters separated by a separator. e.g. ## - ###</param>
        /// <param name="characters">the set of numbers,alphabet or both(alphanumeric) character from which a code can be generated.
        /// Can only be specified using #
        /// </param>
        /// <param name="separator">a character that separates a pattern</param>
        /// <returns>code as string</returns>
        public static string GenerateCodeWithPattern(string pattern, string characters, string separator)
        {
            // int separatorPosition = pattern.IndexOf(separator);
            var charsBeforeAndAfterSeparator = pattern.Split(separator, 2); //[##, ##] 
            var charsBeforeLength = charsBeforeAndAfterSeparator[0].Length;
            var charsAfterLength = charsBeforeAndAfterSeparator[1].Length;

            var beforeCode = GenerateCode(charsBeforeLength, characters);
            var afterCode = GenerateCode(charsAfterLength, characters);

            return $"{beforeCode}{separator}{afterCode}";
            
        }

        /// <summary>
        /// Add defined Prefix to generated code
        /// </summary>
        /// <param name="prefix">Given set of Strings to be Concatenated to the beginning of Generated Code</param>
        /// <param name="code">A system of characters, letters or symbols used for representation</param>
        /// <returns></returns>
        public static string GetCodeWithPrefix(string prefix, string code)
        {
            return prefix + code;
        }

        /// <summary>
        /// Add defined Suffix to generated code
        /// </summary>
        /// <param name="code">A system of characters, letters or symbols used for representation</param>
        /// <param name="suffix">Given set of Strings to be Concatenated at the end of Generated Code</param>
        /// <returns></returns>
        public static string GetCodeWithSuffix(string code, string suffix)
        {
            return code + suffix;
        }

        public static string Encrypt(string code)
        {
            Console.WriteLine("Original: " + code);
            byte[] byteCode = Encoding.UTF8.GetBytes(code);
            string codeEncrpted = Convert.ToBase64String(byteCode);
            Console.WriteLine("Encrpted Code: " + codeEncrpted);
            return codeEncrpted;
        }

        public static string Decrypt(string code)
        {
            
            byte[] byteCode = Convert.FromBase64String(code);
            string codeDecrypted = Encoding.UTF8.GetString(byteCode);
            Console.WriteLine("Decrypted Code: " + codeDecrypted);
            return codeDecrypted;
        }

        public static string HashedCode(VoucherRequest voucherRequest)  //TODO: move to a util class so it can be shared by all services
        {
            string hashedCode; //pattern or length; prefix; suffix
            string characterSet;
            string code;

            switch (voucherRequest.CharacterSet.ToLower() )
            {
                case "alphabet": characterSet = Constants.ALPHABET_CHARACTERS; break;
                case "numeric": characterSet = Constants.NUMBER_CHARACTERS; break;
                case "alphanumeric": characterSet = Constants.ALPHABET_CHARACTERS + Constants.NUMBER_CHARACTERS; break;
                default : characterSet = Constants.ALPHABET_CHARACTERS + Constants.NUMBER_CHARACTERS; break;
            }

            if (!string.IsNullOrEmpty(voucherRequest.CodePattern))
            {
                code = CodeGenerator.GenerateCodeWithPattern(
                    voucherRequest.CodePattern, characterSet, voucherRequest.Separator);
            }

            else //length is specified 
                code = CodeGenerator.GenerateCode(voucherRequest.CodeLength, characterSet);

            if (!string.IsNullOrEmpty(voucherRequest.Prefix))
                code = CodeGenerator.GetCodeWithPrefix(voucherRequest.Prefix, code);

            if (!string.IsNullOrEmpty(voucherRequest.Suffix))
                code = CodeGenerator.GetCodeWithSuffix(code, voucherRequest.Suffix);

            hashedCode = CodeGenerator.Encrypt(code);

            return hashedCode;
        }
    }
}
