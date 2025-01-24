using CSharpToTypeScript.LibraryNew.Attributes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace CSharpToTypeScript.ExampleData;

[TsGenerate]
public class OtherClass
{
    public Dictionary<string, int> ValueProperty { get; set; }
}

public enum TestEnum1
{
    Value1,
    Value2,
    Value3
}

public class ClassWithNullableProperty
{
    public string? NullableStringProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    public TestClass NonNullableObjectProperty  { get; set; }
    public TestClass? NullableObjectProperty { get; set; }
}

public class GenericClass<T1, T2>
{
    public T1 ValueProperty { get; set; }
    public T2 ValueProperty2 { get; set; }
}

[TsGenerate]
public class InheritingGenericClass : GenericClass<string, int>
{
    public string ValueProperty3 { get; set; }
}

[TsGenerate]
public record TestClass : ITestInterface
{
    public string[] StringArrayProperty { get; set; }
    public List<string> StringListProperty { get; set; }
    public IEnumerable<string> StringEnumerableProperty { get; set; }
    public TestEnum1[] TestEnumArrayProperty { get; set; }
    public TestEnum1 EnumProperty { get; set; }
    public DateTime DateTimeProperty { get; set; }
    public string StringProperty { get; set; }
    public byte ByteProperty { get; set; }
    public sbyte SbyteProperty { get; set; }
    public short ShortProperty { get; set; }
    public ushort UshortProperty { get; set; }
    public int IntProperty { get; set; }
    public uint UintProperty { get; set; }
    public long LongProperty { get; set; }
    public ulong UlongProperty { get; set; }
    public float FloatProperty { get; set; }
    public double DoubleProperty { get; set; }
    public decimal DecimalProperty { get; set; }
    public char CharProperty { get; set; }
    public bool BoolProperty { get; set; }
    public string? NullableStringProperty { get; set; }
    public byte? NullableByteProperty { get; set; }
    public sbyte? NullableSbyteProperty { get; set; }
    public short? NullableShortProperty { get; set; }
    public ushort? NullableUshortProperty { get; set; }
    public int? NullableIntProperty { get; set; }
    public uint? NullableUintProperty { get; set; }
    public long? NullableLongProperty { get; set; }
    public ulong? NullableUlongProperty { get; set; }
    public float? NullableFloatProperty { get; set; }
    public double? NullableDoubleProperty { get; set; }
    public decimal? NullableDecimalProperty { get; set; }
    public char? NullableCharProperty { get; set; }
    public bool? NullableBoolProperty { get; set; }
}

public class SecondTestClass
{
    // public ArrayItemTestClass TestClassProperty { get; set; }
    public TestClass[] TestClassArrayProperty { get; set; }
    public List<TestClass> TestClassListProperty { get; set; }
    public IEnumerable<TestClass> TestClassEnumerableProperty { get; set; }
}

public interface ITestInterface
{
    string StringProperty { get; set; }
    byte ByteProperty { get; set; }
    sbyte SbyteProperty { get; set; }
    short ShortProperty { get; set; }
    ushort UshortProperty { get; set; }
    int IntProperty { get; set; }
    uint UintProperty { get; set; }
    long LongProperty { get; set; }
    ulong UlongProperty { get; set; }
    float FloatProperty { get; set; }
    double DoubleProperty { get; set; }
    decimal DecimalProperty { get; set; }
    char CharProperty { get; set; }
    bool BoolProperty { get; set; }
    string? NullableStringProperty { get; set; }
    byte? NullableByteProperty { get; set; }
    sbyte? NullableSbyteProperty { get; set; }
    short? NullableShortProperty { get; set; }
    ushort? NullableUshortProperty { get; set; }
    int? NullableIntProperty { get; set; }
    uint? NullableUintProperty { get; set; }
    long? NullableLongProperty { get; set; }
    ulong? NullableUlongProperty { get; set; }
    float? NullableFloatProperty { get; set; }
    double? NullableDoubleProperty { get; set; }
    decimal? NullableDecimalProperty { get; set; }
    char? NullableCharProperty { get; set; }
    bool? NullableBoolProperty { get; set; }
}