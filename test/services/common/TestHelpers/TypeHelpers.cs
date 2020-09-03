// <copyright file="TypeHelpers.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace Mmm.Iot.Common.TestHelpers
{
    public static class TypeHelpers
    {
        public static T CreateInstance<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(type.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args, null, null);
            return (T)instance;
        }

        public static PropertyInfo GetPropertyInfo<T>(string propertyName)
        {
            var type = typeof(T);
            return GetPropertyInfo(type, propertyName);
        }

        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propertyInfo = null;
            do
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (propertyInfo == null && type != null);
            return propertyInfo;
        }

        public static object GetPropertyValue(this object @object, string propertyName)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            var objectType = @object.GetType();
            var propertyInfo = GetPropertyInfo(objectType, propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), string.Format("Couldn't find property {0} in type {1}", propertyName, objectType.FullName));
            }

            return propertyInfo.GetValue(@object, null);
        }

        public static void SetPropertyValue(this object @object, string propertyName, object value)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            var objectType = @object.GetType();
            var propertyInfo = GetPropertyInfo(objectType, propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), string.Format("Couldn't find property {0} in type {1}", propertyName, objectType.FullName));
            }

            propertyInfo.SetValue(@object, value, null);
        }
    }
}