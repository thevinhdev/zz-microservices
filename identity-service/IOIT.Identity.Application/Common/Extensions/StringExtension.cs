using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace IOIT.Identity.Application.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsEmail(this string text)
        {
            try
            {
                var m = new MailAddress(text);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static T ConvertTo<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                    return (T)converter.ConvertFromString(input);
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static int ConvertToInt(this string input)
        {
            try
            {
                int value;
                int.TryParse(input, out value);
                return value;
            }
            catch (NotSupportedException)
            {
                return 0;
            }
        }
        public static bool EqualIgnoreCase(this string source, string toCheck)
        {
            return string.Equals(source, toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static List<int> ToListInt(this string input, char[] splitCharacter)
        {
            var result = new List<int>();

            try
            {
                var list = input.Split(splitCharacter, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var item in list)
                {
                    result.Add(int.Parse(item));
                }
                return result;
            }
            catch (NotSupportedException)
            {
                return new List<int>();
            }
        }

        public static string GenerateReferralCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return "GJP-" + new String(stringChars);
        }

        public static string GenerateUserCode()
        {
            Random random = new Random();
            DateTime timeValue = DateTime.MinValue;
            int rand = random.Next(3600) + 1; // add one to avoid 0 result.
            timeValue = timeValue.AddMinutes(rand);
            byte[] b = System.BitConverter.GetBytes(timeValue.Ticks);
            string voucherCode = ByteToString(b);

            return string.Format("{0}-{1}-{2}",
                voucherCode.Substring(0, 4),
                voucherCode.Substring(4, 4),
                voucherCode.Substring(8, 4));
        }

        public static string ByteToString(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException("input");
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", "b");
        }

    }
}
