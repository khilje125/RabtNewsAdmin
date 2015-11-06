using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

/// <summary>
/// Summary description for ObjectExtensions
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts the object to Long. 
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns long value</returns>
    public static long ToLong(this object input)
    {

        long output = 0;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToInt64(input);
        }
        catch { return output; }

        return output;
    }


    /// <summary>
    /// Converts the object to Double. 
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns double value</returns>
    public static double ToDouble(this object input)
    {
        double output = 0;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToDouble(input);
        }
        catch { return output; }

        return output;
    }
    /// <summary>
    /// Converts the object to int. 
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns int value</returns>
    public static int ToInt(this object input)
    {
        int output = 0;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToInt32(input);
        }
        catch { return output; }

        return output;
    }
    /// <summary>
    /// Converts the object to short. 
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns short value</returns>
    public static short ToShort(this object input)
    {
        short output = 0;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToInt16(input);
        }
        catch { return output; }

        return output;
    }
    /// <summary>
    /// Converts the object to short. 
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns short value</returns>
    public static char ToChar(this object input)
    {
        char output = '\u0000';

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToChar(input);
        }
        catch { return output; }

        return output;
    }
    /// <summary>
    /// Converts the object to bool.
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns bool value</returns>
    public static bool ToBoolean(this object input)
    {
        bool output = false;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToBoolean(input);
        }
        catch { return output; }

        return output;
    }
    /// <summary>
    /// Converts the object to decimal.
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns decimal value</returns>
    public static decimal ToDecimal(this object input)
    {
        decimal output = 0.0M;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToDecimal(input);
        }
        catch { return output; }

        return output;
    }

    /// <summary>
    /// Converts the object to save string to avoid exception.
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns string value</returns>
    public static string ToSafeString(this object input)
    {
        return (input ?? string.Empty).ToString();
    }
    /// <summary>
    /// Converts the object to datetime, It will return minimum value in case of exception.
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">Object value </param>
    /// <returns>Retruns datetime value</returns>
    public static DateTime ToDateTime(this object input)
    {
        DateTime output = DateTime.MinValue;

        if ((input == null) || (input == DBNull.Value))
            return output;
        try
        {
            if (string.Empty != input.ToString())
                output = Convert.ToDateTime(input);
        }
        catch { return output; }

        return output;
    }
    /// <summary>
    /// Get DateTime value and returns DBNull value in case of exception or MinValue.
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input">datetime value </param>
    /// <returns>Retruns object value</returns>
    public static object ToDbDateTime(this DateTime input)
    {
        if (input == DateTime.MinValue)
        {
            return DBNull.Value;
        }
        else
        {
            return input;
        }
    }
    /// <summary>
    ///Returns the actual value and DBNull in case of Extention. 
    /// </summary>
    /// <param name="CreatedBy">VAKAS </param> 
    /// <param name="CreatedDate">09-2010</param>
    /// <param name="ModifiedDate">06-10-2010</param>
    /// <param name="input"> bool </param>
    /// <returns>Retruns object value</returns>
    public static object ToDbBool(this bool? input)
    {

        if (input == null)
        {
            return DBNull.Value;
        }
        else
        {
            return input;
        }
    }

    public static object ToDbBool(this bool input)
    {

        if (input == false)
        {
            return DBNull.Value;
        }
        else
        {
            return input;
        }
    }

    public static string GetAttributeFromNode(this XmlNode node, string attribute)
    {
        if (node == null || attribute == null)
            return string.Empty;

        if (node.Attributes == null)
            return string.Empty;

        if (node.Attributes[attribute] == null)
            return string.Empty;

        return node.Attributes[attribute].Value;
    }

    public static string GetValueFromNode(this XmlNode node)
    {
        if (node == null || node.Value == null)
            return "";

        return node.Value.Trim();
    }

    public static string GetInnerTextFromNode(this XmlNode node)
    {
        if (node == null)
            return "";

        return node.InnerText.Trim();
    }
    public static string GetInnerXML(this XmlNode node)
    {
        if (node == null)
            return "";

        return node.InnerXml.Trim();
    }
    public static string GetOuterXML(this XmlNode node)
    {
        if (node == null)
            return "";

        return node.OuterXml.Trim();
    }
    #region Domain Name

    private static HashSet<string> _tlds = new HashSet<string>
        {
            "co.uk",
            "com"
        };

    public static string GetDomainFromUri(this Uri uri)
    {
        return GetDomainFromHostName(uri.Host);
    }

    public static string GetDomainFromHostName(this string hostName)
    {
        string[] hostNameParts = hostName.Split('.');

        if (hostNameParts.Length == 1)
            return hostNameParts[0];

        int matchingParts = FindMatchingParts(hostNameParts, 1);

        return GetPartOfHostName(hostNameParts, hostNameParts.Length - matchingParts);
    }

    private static int FindMatchingParts(string[] hostNameParts, int offset)
    {
        if (offset == hostNameParts.Length)
            return hostNameParts.Length;

        string domain = GetPartOfHostName(hostNameParts, offset);

        if (_tlds.Contains(domain.ToLowerInvariant()))
            return (hostNameParts.Length - offset) + 1;

        return FindMatchingParts(hostNameParts, offset + 1);
    }

    private static string GetPartOfHostName(string[] hostNameParts, int offset)
    {
        var sb = new StringBuilder();

        for (int i = offset; i < hostNameParts.Length; i++)
        {
            if (sb.Length > 0)
                sb.Append('.');

            sb.Append(hostNameParts[i]);
        }

        string domain = sb.ToString();
        return domain;
    }
   
    #endregion
}


