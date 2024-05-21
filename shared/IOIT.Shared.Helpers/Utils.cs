using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IOIT.Shared.Helpers
{
    public class Utils
    {

        public static string ConvertPhone(string phoneIn)
        {
            string phoneOut = "";
            if (phoneIn.Length >= 9)
            {
                string phoneInNew = phoneIn;
                phoneOut = "84" + phoneInNew.Substring(phoneIn.Length - 9, 9);
            }
            return phoneOut;
        }

        public static bool CheckPhone(string phoneIn, string phoneOn)
        {
            bool check = false;
            if (phoneIn != null && phoneOn != null)
            {
                if (phoneIn.Length >= 9 && phoneOn.Length >= 9)
                {
                    if (phoneIn == phoneOn) check = true;
                    else
                    {
                        string phoneInNew = phoneIn;
                        string phoneNew = "84" + phoneInNew.Substring(phoneIn.Length - 9, 9);
                        if (phoneNew == phoneOn) check = true;
                        else
                        {
                            string phoneOnNew = phoneOn;
                            string phoneNew2 = "84" + phoneOnNew.Substring(phoneOn.Length - 9, 9);
                            if (phoneNew2 == phoneNew) check = true;
                            else check = false;
                        }
                    }
                }
            }
            return check;
        }

        public static string FormatDecimal(string s)
        {
            string stmp = s;
            int amount;
            amount = (int)(s.Length / 3);
            if (s.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ",");
            }
            return stmp;
        }

        public static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        public static string ConvertContents(string Contents, string domain)
        {
            string result = "";
            string link = domain + "uploads/images/";
            if (Contents != null && Contents != "")
            {
                result = Contents.Replace("/uploads/images/", link);
            }
            return result;
        }

        public static bool checkNumber(string text)
        {
            long val;
            return long.TryParse(text, out val);
        }

        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ConvertMoneyToString(string money)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;
            string[] arrListStr = money.Split('.');
            string number = arrListStr[0];
            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48].ToUpper() + " ĐỒNG";

            return doc.ToUpper() + " ĐỒNG";
        }

        public static string NonUnicodeV2(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }

            return Regex.Replace(text.ToLower().Replace(@"'", String.Empty), @"[^\w]+", " ").Replace("\"", " ").Replace(":", " ").ToLower();
        }

        public static string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }

            return Regex.Replace(text.ToLower().Replace(@"'", String.Empty), @"[^\w]+", "-").Replace("\"", "-").Replace(":", "-").ToLower();
        }

        public static string ColumnAdress(int col)
        {
            if (col <= 26)
            {
                return Convert.ToChar(col + 64).ToString();
            }
            int div = col / 26;
            int mod = col % 26;
            if (mod == 0) { mod = 26; div--; }
            return ColumnAdress(div) + ColumnAdress(mod);
        }

        public static int GetWeekOrderInYear(DateTime time)
        {
            CultureInfo myCI = CultureInfo.CurrentCulture;
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            return myCal.GetWeekOfYear(time, myCWR, myFirstDOW);
        }

        public static string ConvertRomanNumber(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ConvertRomanNumber(number - 1000);
            if (number >= 900) return "CM" + ConvertRomanNumber(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ConvertRomanNumber(number - 500);
            if (number >= 400) return "CD" + ConvertRomanNumber(number - 400);
            if (number >= 100) return "C" + ConvertRomanNumber(number - 100);
            if (number >= 90) return "XC" + ConvertRomanNumber(number - 90);
            if (number >= 50) return "L" + ConvertRomanNumber(number - 50);
            if (number >= 40) return "XL" + ConvertRomanNumber(number - 40);
            if (number >= 10) return "X" + ConvertRomanNumber(number - 10);
            if (number >= 9) return "IX" + ConvertRomanNumber(number - 9);
            if (number >= 5) return "V" + ConvertRomanNumber(number - 5);
            if (number >= 4) return "IV" + ConvertRomanNumber(number - 4);
            if (number >= 1) return "I" + ConvertRomanNumber(number - 1);
            throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
        }

        public static int getQuarter(int month)
        {
            int quarter = 1;
            if (month >= 1 && month <= 3)
                quarter = 1;
            if (month >= 4 && month <= 6)
                quarter = 2;
            if (month >= 7 && month <= 9)
                quarter = 3;
            if (month >= 10 && month <= 12)
                quarter = 4;
            return quarter;
        }

        private static readonly string[] VietnameseSigns = new string[]

        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"

        };

        public static string unsignString(string str)
        {
            try
            {
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {

                    for (int j = 0; j < VietnameseSigns[i].Length; j++)

                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

                }
                Regex r = new Regex("(?:[^a-z0-9 @]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                return r.Replace(str, String.Empty).ToLower();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string unsignString2(string str)
        {
            try
            {
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {

                    for (int j = 0; j < VietnameseSigns[i].Length; j++)

                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

                }
                Regex r = new Regex("(?:[^a-z0-9 ,@]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                return r.Replace(str, String.Empty).ToLower();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        public static DateTime UnixTimeStampMilisecondToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static long DateTimeToUnixTimeStamp(DateTime date)
        {
            try
            {
                return (long)date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            }
            catch (Exception ex)
            {
                return (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
        }


        public static void createFile(string fileName, byte[] input, string file_type)
        {
            //extract path
            int lastIndex = fileName.LastIndexOf("\\");
            string path = fileName.Substring(0, lastIndex);
            Directory.CreateDirectory(path);
            MemoryStream ms = new MemoryStream(input);
            File.WriteAllBytes(fileName, input);
            ms.Dispose();
        }

        //private static RotateFlipType GetOrientationToFlipType(int orientationValue)
        //{
        //    RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

        //    switch (orientationValue)
        //    {
        //        case 1:
        //            rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        //            break;
        //        case 2:
        //            rotateFlipType = RotateFlipType.RotateNoneFlipX;
        //            break;
        //        case 3:
        //            rotateFlipType = RotateFlipType.Rotate180FlipNone;
        //            break;
        //        case 4:
        //            rotateFlipType = RotateFlipType.Rotate180FlipX;
        //            break;
        //        case 5:
        //            rotateFlipType = RotateFlipType.Rotate90FlipX;
        //            break;
        //        case 6:
        //            rotateFlipType = RotateFlipType.Rotate90FlipNone;
        //            break;
        //        case 7:
        //            rotateFlipType = RotateFlipType.Rotate270FlipX;
        //            break;
        //        case 8:
        //            rotateFlipType = RotateFlipType.Rotate270FlipNone;
        //            break;
        //        default:
        //            rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        //            break;
        //    }

        //    return rotateFlipType;
        //}

        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumberString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
