namespace NapCatSharp.EventPushModels;

public static class EnumExtension<TEnum>
    where TEnum : struct, Enum
{
    private static TEnum[] Values { get; set; } = Enum.GetValues<TEnum>();
    private static string[] Names { get; set; } = Enum.GetNames<TEnum>();

    public static TEnum? GetValue(string vauleName)
    {
        bool nameEq(TEnum f) => vauleName.Equals(f.ToString());
        if (Values.Any(nameEq)) { // TODO: edit to IndexOfAny
            return Values.First(nameEq);
        }
        return null;
    }

    public static bool Contains(string valueName)
    {
        return Values.Any(f => valueName.Equals(valueName));
    }
}
