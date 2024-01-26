using System;
using System.Collections.Generic;
using System.Reflection;

namespace PimpochkaGames.CoreLibrary.Editor
{
    public class TypeCache
    {
        private static Dictionary<Type, string> _typeMap; 
        private static bool _isDirty;

        public static void Rebuild()
        {
            // Reset properties
            _isDirty   = true;
            _typeMap?.Clear();

            EnsureCacheIsUpdated();
        }

        public static Dictionary<MemberInfo, TAttribute> GetMembersWithAttribute<TAttribute>(MemberTypes memberTypes, BindingFlags bindingAttr)
            where TAttribute : Attribute
        {
            return GetMembersWithAttribute<MemberInfo, TAttribute>(memberTypes, bindingAttr);
        }

        public static Dictionary<FieldInfo, TAttribute> GetFieldsWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<FieldInfo, TAttribute>(MemberTypes.Field, bindingAttr);
        }

        public static Dictionary<PropertyInfo, TAttribute> GetPropertiesWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<PropertyInfo, TAttribute>(MemberTypes.Property, bindingAttr);
        }

        public static Dictionary<EventInfo, TAttribute> GetEventsWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<EventInfo, TAttribute>(MemberTypes.Event, bindingAttr);
        }

        public static Dictionary<MethodInfo, TAttribute> GetMethodsWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<MethodInfo, TAttribute>(MemberTypes.Method, bindingAttr);
        }

        private static void EnsureCacheIsUpdated()
        {
            if (!_isDirty) return;

            // Initialize object
            _isDirty = false;
            if (_typeMap == null)
            {
                _typeMap = new Dictionary<Type, string>(capacity: 1024);
            }

            // Add types to the cache
            foreach (var type in ReflectionUtility.FindAllTypes())
            {
                _typeMap.Add(type, type.FullName);
            }
        }

        private static Dictionary<TMemberInfo, TAttribute> GetMembersWithAttribute<TMemberInfo, TAttribute>(MemberTypes memberTypes, BindingFlags bindingAttr)
            where TMemberInfo : MemberInfo
            where TAttribute : Attribute
        {
            EnsureCacheIsUpdated();

            // Find all the methods with specified attribute
            var attributeType = typeof(TAttribute);
            var collection = new Dictionary<TMemberInfo, TAttribute>();
            foreach (var mapItem in _typeMap)
            {
                var currentType = mapItem.Key;

                AddMembersWithRequiredAttributes(currentType);

                //When we create a "concrete" class derived from a generic class, it will internally constructs a new class (replacing generic parameters with actual types). We need to query this type for required attributes as well.
                if (IsConstructedClosedGenericType(currentType.BaseType)) 
                {
                    AddMembersWithRequiredAttributes(currentType.BaseType);
                }
            }
            return collection;

            void AddMembersWithRequiredAttributes(Type type)
            {
                var members = type.FindMembers(memberTypes, bindingAttr, null, null);
                foreach (var memberInfo in members)
                {
                    var attributes = memberInfo.GetCustomAttributes(attributeType, false);
                    if (attributes.IsNullOrEmpty()) continue;

                    collection.Add(memberInfo as TMemberInfo, attributes[0] as TAttribute);
                }
            }

        }

        private static bool IsConstructedClosedGenericType(Type type)
        {
            return type != null && type.IsConstructedGenericType && !type.ContainsGenericParameters;
        }
    }
}