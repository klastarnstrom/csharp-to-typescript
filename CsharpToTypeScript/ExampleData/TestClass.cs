using CsharpToTypeScript.Library.Attributes;

namespace CsharpToTypeScript.ExampleData;

[TsGenerate]
public class TestClass1
{
    public string ValueProperty { get; set; }
    public string[] ValueArrayProperty { get; set; }
    public TestEnum1 EnumProperty { get; set; }
}

[TsGenerate]
public class TestClass2
{
    public TestClass1 ObjectProperty { get; set; }
    public TestClass1[] ObjectArrayProperty { get; set; }
    public List<TestClass1> ObjectListProperty { get; set; }
}

public enum TestEnum1
{
    
}

public class TestClass : ITestInterface
{
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
