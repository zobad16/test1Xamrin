using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public static class Extensions
    {

        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            //es string[] result = "bobjoecat".SplitBy(3); // [bob, joe, cat]
            if (String.IsNullOrEmpty(str))
                throw new ArgumentException();
            if (chunkLength < 1)
                throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }


        public static string Reverse(this string input)
        {
            string ret = new string(input.ToCharArray().Reverse().ToArray());
            return ret;
        }


        public static string UppercaseFirstLetter(this string value)
        {
            // Uppercase the first letter in the string.
            if (value.Length > 0)
            {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);

            }
            return value;
        }

        public static void SettaEnabled(this StackLayout sl, Boolean isEnabled)
        {
            foreach (var c in sl.Children)
            {
                if (c.GetType() == typeof(StackLayout))
                {
                    (c as StackLayout).SettaEnabled(isEnabled);
                }
                else
                {
                    c.IsEnabled = isEnabled;
                }


            }

        }



    }

    public static class ImageSourceExtensions
    {
        public static async Task<Stream> GetStreamAsync(this StreamImageSource imageSource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageSource.Stream != null)
            {
                return await imageSource.Stream(cancellationToken);
            }
            return null;
        }
    }

}


