using TinyPinyin;

// ReSharper disable once CheckNamespace
namespace System.Application.Services.Implementation;

/// <summary>
/// 使用 <see cref="PinyinHelper"/>(https://github.com/promeG/TinyPinyin) or (https://github.com/hueifeng/TinyPinyin.Net) 实现的拼音功能
/// </summary>
internal sealed partial class PinyinImpl : IPinyin
{
    static string GetPinyin(string str, string separator)
    {
        string? value = PinyinHelper.GetPinyin(str, separator);
        return value ?? string.Empty;
    }

    string IPinyin.GetPinyin(string s, PinyinFormat format) => format switch
    {
        PinyinFormat.UpperVerticalBar => GetPinyin(s, Pinyin.SeparatorVerticalBar.ToString()) ?? string.Empty,
        PinyinFormat.AlphabetSort => GetPinyin(s, string.Empty) ?? string.Empty,
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
    };

    bool IPinyin.IsChinese(char c)
    {
        return PinyinHelper.IsChinese(c);
    }

    static string? GetPinyin(char c)
    {
        return PinyinHelper.GetPinyin(c);
    }

    string[] IPinyin.GetPinyinArray(string s)
    {
        if (string.IsNullOrEmpty(s)) return Array.Empty<string>();
        // 没有提供字典或选择器，按单字符转换输出
        return s.Select(x => GetPinyin(x) ?? string.Empty).ToArray();
    }
}
