using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JObjectParamsCreator
{
    public class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(ParamsCreator.Create<ParamsHolder>().ToString());
            }

        }
    }

    public class ParamsHolder
    {
        public static StringParam SomeStringParam => new StringParam("someStringParam", 10, '0', '9');
        public static StringParam SomeLongParam => new StringParam("someLongParam", 5, '3', '6');
        public static EnumParam<EnumName> SomeEnumNameParam => new EnumParam<EnumName>("someEnumParam");
    }

    public static class ParamsCreator
    {
        public static JObject Create<T>()
        {
            JObject obj = new JObject();
            BaseParam param = null;
            foreach (var prop in typeof(T).GetProperties())
            {
                param = (BaseParam)prop.GetValue(prop);
                obj[param.Name] = param.Create();
            }

            return obj;
        }
    }

    public abstract class BaseParam
    {
        public string Name { get; set; }
        public abstract string Create();
    }

    public class StringParam : BaseParam
    {
        public int Length { get; set; }
        public char MinVal { get; set; }
        public char MaxVal { get; set; }

        private Random Random;

        public StringParam(string name, int length, char minVal, char maxVal)
        {
            Name = name;
            Length = length;
            MinVal = minVal;
            MaxVal = MaxVal;
            Random = new Random();
        }
        public override string Create()
        {
            string result = "";
            for (int i = 0; i < Length; i++)
            {
                result += Random.Next(0, 10).ToString(); //replace with the string creation function i have
            }

            return result;
        }
    }

    public class EnumParam<TEnum> : BaseParam
        where TEnum : struct, IConvertible
    {
        private Random Random;

        public EnumParam(string name)
        {
            Name = name;
            Random = new Random();
        }

        public override string Create()
        {
            var enumValues = Enum.GetValues(typeof(TEnum));
            var index = Random.Next(0, enumValues.Length);
            return enumValues.GetValue(index).ToString(); // replace toString with toText
        }
    }

    public enum EnumName
    {
        First,
        Second,
        Third
    }
}