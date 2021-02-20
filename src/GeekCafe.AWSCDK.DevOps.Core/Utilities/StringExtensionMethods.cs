using System;
using System.Security.Cryptography;
using System.Text;

namespace GeekCafe.AWSCDK.DevOps.Core.Utilities
{
    public static class StringExtensionMethods
    {

        /// <summary>
        /// Generates a Hash based on a string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GenerateCheckSum<T>(this T item)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var ue = new UnicodeEncoding();

            //Convert the string into an array of bytes.
            var bytes = ue.GetBytes(json);

            //Create a new instance of the SHA256 class to create
            //the hash value.
            var sha = SHA256.Create();

            //Create the hash value from the array of bytes.

            var hash = sha.ComputeHash(bytes);

            var results = BitConverter.ToString(hash).Replace("-", "");
            return results;

        }

        /// <summary>
        /// Model to Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T model)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Json to a Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Turns an Enum to a friendly readable string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnumToString(this Enum value, string delimiter = "_")
        {
            return value.ToString().Replace(delimiter, " ");
        }

    }
}
