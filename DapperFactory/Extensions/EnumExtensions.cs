using System;
using System.ComponentModel;
using System.Reflection;

namespace Chambersoft.DapperFactory.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the description decorated attribute string from an enum or its ToString() representation.
        /// </summary>
        /// <param name="item">The enum to get the string description of.</param>
        /// <returns>The string description.</returns>
        public static string GetDescription(this Enum item)
        {
            string retValue = string.Empty;
            if (item != null)
            {
                Type t = item.GetType();
                MemberInfo[] memInfo = t.GetMember(item.ToString());
                if (memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs.Length > 0)
                    {
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }
                retValue = item.ToString();
            }
            return retValue;
        }
    }
}
