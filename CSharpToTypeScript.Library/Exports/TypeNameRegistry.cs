public class TypeNameRegistry
{
    private readonly Dictionary<Type, string> _typeNames = new();
    private readonly Dictionary<string, int> _nameCounters = new();

    public string GetTypeName(Type type)
    {
        if (_typeNames.TryGetValue(type, out var existingName))
        {
            return existingName;
        }

        var baseName = GetBaseTypeName(type);
        var uniqueName = GetUniqueName(baseName);
        _typeNames[type] = uniqueName;

        return uniqueName;
    }

    private string GetUniqueName(string baseName)
    {
        if (!_nameCounters.ContainsKey(baseName))
        {
            _nameCounters[baseName] = 0;
            return baseName;
        }

        _nameCounters[baseName]++;
        return $"{baseName}{_nameCounters[baseName]}";
    }

    private static string GetBaseTypeName(Type type)
    {
        var name = type.Name;

        if (type.IsGenericType)
        {
            name = name[..name.IndexOf('`')];
        }

        return name;
    }
}