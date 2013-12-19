namespace SunStar_CMS.admin.Classes.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;    
    using System.Reflection;
    using SunStar_CMS.admin.Classes.Objects;
    using SunStar_CMS.admin.Classes.ControlValues;

    class EnumConvertUtils
    {
        public static object ToDbValue(Enum value)
        {            
            if (value == null)
            {
                return null;
            }
            BidirHashtable<object, EnumValueAttribute> map
                = EnumToAttributeMap(value.GetType());
            return map[value].DbValue;
        }
        
        public static object ToDisplayValue(Enum value)
        {            
            if (value == null)
            {
                return null;
            }
            BidirHashtable<object, EnumValueAttribute> map
                = EnumToAttributeMap(value.GetType());
            return map[value].DisplayValue;
        }

        public static object DbValueToEnum<T>(object dbValue)
        {
            if (Convert.IsDBNull(dbValue))
            {
                return null;
            }
            else
            {
                BidirHashtable<object, object> map = EnumToDbValueMap(typeof(T));
                return map.ReverseLookup(dbValue);
            }
        }

        public static IDictionary<object, object> ToDictionary<T>()
        {
            IDictionary<object, object> map = new Dictionary<object, object>();
            IDictionary<object, EnumValueAttribute> map2 = EnumToAttributeMap(typeof(T));
            foreach (object key in map2.Keys)
            {
                map.Add(map2[key].DbValue, map2[key].DisplayValue);
            }
            return map;
        }

        #region private stuff
        public static BidirHashtable<object, EnumValueAttribute>
            EnumToAttributeMap(Type enumType)
        {
            BidirHashtable<object, EnumValueAttribute> retval
                = new BidirHashtable<object, EnumValueAttribute>();

            foreach (FieldInfo fi in enumType.GetFields())
            {
                if (fi.FieldType.BaseType == typeof(Enum))
                {
                    EnumValueAttribute[] attrs =
                        (EnumValueAttribute[])fi.GetCustomAttributes(
                        typeof(EnumValueAttribute), false);
                    if (attrs.Length > 0)
                    {
                        retval.Add(Enum.Parse(enumType, fi.Name), attrs[0]);                            
                    }
                }
            }
            return retval;
        }

        private static BidirHashtable<object, object>
            EnumToDbValueMap(Type enumType)
        {
            BidirHashtable<object, object> retval
                = new BidirHashtable<object, object>();

            foreach (FieldInfo fi in enumType.GetFields())
            {
                if (fi.FieldType.BaseType == typeof(Enum))
                {
                    EnumValueAttribute[] attrs =
                        (EnumValueAttribute[])fi.GetCustomAttributes(
                        typeof(EnumValueAttribute), false);
                    if (attrs.Length > 0)
                    {
                        retval.Add(Enum.Parse(enumType, fi.Name), attrs[0].DbValue);
                    }
                }
            }
            return retval;
        }
        #endregion
    }
}
