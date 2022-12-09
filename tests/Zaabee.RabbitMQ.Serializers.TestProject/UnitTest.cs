namespace Zaabee.RabbitMQ.Serializers.TestProject;

public class UnitTest
{
    [Fact]
    public void JilTest() =>
        SerializerTest(new Jil.Serializer());

    [Fact]
    public void NewtonsoftJsonTest() =>
        SerializerTest(new NewtonsoftJson.Serializer());

    [Fact]
    public void SystemTextJsonTest() =>
        SerializerTest(new SystemTextJson.Serializer());

    [Fact]
    public void Utf8JsonTest() =>
        SerializerTest(new Utf8Json.Serializer());

    private static void SerializerTest(IJsonSerializer serializer)
    {
        var testModel = GetTestModel();
        var str = serializer.ToJson(testModel);
        var result = serializer.FromJson<TestModel>(str)!;

        Assert.Equal(
            Tuple.Create(testModel.Id, testModel.Age, testModel.CreateTime, testModel.Name, testModel.Gender),
            Tuple.Create(result.Id, result.Age, result.CreateTime, result.Name, result.Gender));
    }

    private static TestModel GetTestModel() =>
        new()
        {
            Id = Guid.NewGuid(),
            Age = new Random().Next(0, 100),
            CreateTime = new DateTime(2017, 1, 1).ToUniversalTime(),
            Name = "apple",
            Gender = Gender.Female
        };
}