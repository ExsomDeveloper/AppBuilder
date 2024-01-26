using System;

namespace PimpochkaGames.CoreLibrary
{
    public class DefaultValueAttribute : Attribute
    {
        private bool? _boolValue;
        private int? _int32Value;
        private float? _singleValue;
        private string _stringValue;

        public bool BoolValue => _boolValue.GetValueOrDefault();
        public int Int32Value => _int32Value.GetValueOrDefault();
        public float SingleValue => _singleValue.GetValueOrDefault();
        public string StringValue => _stringValue;

        public DefaultValueAttribute(bool value)
        {
            _boolValue = value;
        }

        public DefaultValueAttribute(int value)
        {
            _int32Value = value;
        }

        public DefaultValueAttribute(float value)
        {
            _singleValue = value;
        }

        public DefaultValueAttribute(string value)
        {
            _stringValue = value;
        }

        public T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        public object GetValue(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return BoolValue;

                case TypeCode.Int32:
                    return Int32Value;

                case TypeCode.Single:
                    return SingleValue;

                case TypeCode.String:
                    return StringValue;

                default:
                    return null;
            }
        }
    }
}