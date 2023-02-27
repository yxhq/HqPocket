namespace HqPocket.RegularExpressions;

/// <summary>
/// 包含一些正则验证所需要的字符串
/// </summary>
public static class RegularPatterns
{
    /// <summary>
    /// 邮件正则匹配表达式
    /// </summary>
    public const string Mail=
        @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

    /// <summary>
    /// 手机号正则匹配表达式
    /// </summary>
    public const string Phone= @"^((13[0-9])|(15[^4,\d])|(18[0,5-9]))\d{8}$";

    /// <summary>
    /// IP正则匹配
    /// </summary>
    public const string Ip=
        @"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

    /// <summary>
    /// A类IP正则匹配
    /// </summary>
    public const string IpA=
        @"^(12[0-6]|1[0-1]\d|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

    /// <summary>
    /// B类IP正则匹配
    /// </summary>
    public const string IpB=
        @"^(19[0-1]|12[8-9]|1[3-8]\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

    /// <summary>
    /// C类IP正则匹配
    /// </summary>
    public const string IpC=
        @"^(19[2-9]|22[0-3]|2[0-1]\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

    /// <summary>
    /// D类IP正则匹配
    /// </summary>
    public const string IpD=
        @"^(22[4-9]|23\d\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

    /// <summary>
    /// E类IP正则匹配
    /// </summary>
    public const string IpE=
        @"^(25[0-5]|24\d\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
        + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

    /// <summary>
    /// 汉字正则匹配
    /// </summary>
    public const string Chinese= @"^[\u4e00-\u9fa5]$";

    /// <summary>
    /// Url正则匹配
    /// </summary>
    public const string Url=
        @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";

    /// <summary>
    /// 数字正则匹配
    /// </summary>
    public const string Number= @"^\d$";

    /// <summary>
    /// 计算性质数字正则匹配
    /// </summary>
    public const string Digits= @"[+-]?\d*\.?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?";

    /// <summary>
    /// 正整数正则匹配
    /// </summary>
    public const string PositiveInt= @"^[1-9]\d*$";

    /// <summary>
    /// 负整数正则匹配
    /// </summary>
    public const string NegativeInt= @"^-[1-9]\d*$ ";

    /// <summary>
    /// 整数正则匹配
    /// </summary>
    public const string Int= @"^-?[1-9]\d*$";

    /// <summary>
    /// 非负整数正则匹配
    /// </summary>
    public const string NonnegativeInt = @"^[1-9]\d*|0$";

    /// <summary>
    /// 非正整数正则匹配
    /// </summary>
    public const string NonpositiveInt = @"^-[1-9]\d*|0$";

    /// <summary>
    /// 正浮点数正则匹配
    /// </summary>
    public const string PositiveDouble = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";

    /// <summary>
    /// 负浮点数正则匹配
    /// </summary>
    public const string NegativeDouble = @"^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$";

    /// <summary>
    /// 浮点数正则匹配
    /// </summary>
    public const string Double= @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$";

    /// <summary>
    /// 非负浮点数正则匹配
    /// </summary>
    public const string NonnegativeDouble = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$";

    /// <summary>
    /// 非正浮点数正则匹配
    /// </summary>
    public const string NonpositiveDouble = @"^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$";

    /// <summary>
    /// 十六进制字符
    /// </summary>
    public const string Hex = "([0-9A-Fa-f]{2})";

    /// <summary>
    /// 十六进制字符串、空格分隔
    /// </summary>
    public const string HexWithSpace = "([0-9A-Fa-f]{2} )*([0-9A-Fa-f]{2})";

    public static string HexWithSeparator(string separator = " ")
    {
        return $"([0-9A-Fa-f]{{2}}{separator})*([0-9A-Fa-f]{{2}})";
    }

    /// <summary>
    /// 根据属性名称使用反射来获取值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object? GetValue(string propertyName) => typeof(RegularPatterns).GetField(propertyName)?.GetValue(null);
}
