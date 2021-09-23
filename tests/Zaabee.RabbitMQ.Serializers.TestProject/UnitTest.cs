using System;
using Xunit;
using Zaabee.Serializer.Abstractions;

namespace Zaabee.RabbitMQ.Serializers.TestProject
{
    public class UnitTest
    {
        [Fact]
        public void JilTest() => SerializerTest(new Jil.ZaabeeSerializer());

        [Fact]
        public void NewtonsoftJsonTest() => SerializerTest(new NewtonsoftJson.ZaabeeSerializer());

        [Fact]
        public void SystemTextJsonTest() => SerializerTest(new SystemTextJson.ZaabeeSerializer());

        [Fact]
        public void Utf8JsonTest() => SerializerTest(new Utf8Json.ZaabeeSerializer());

        [Fact]
        public void XmlTest() => SerializerTest(new Xml.ZaabeeSerializer());

        private static void SerializerTest(ITextSerializer serializer)
        {
            var testModel = GetTestModel();
            var bytes = serializer.SerializeToBytes(testModel);
            var str = serializer.SerializeToString(testModel);
            var result0 = serializer.DeserializeFromBytes<TestModel>(bytes);
            var result1 = serializer.DeserializeFromString<TestModel>(str);

            Assert.Equal(
                Tuple.Create(testModel.Id, testModel.Age, testModel.CreateTime, testModel.Name, testModel.Gender),
                Tuple.Create(result0.Id, result0.Age, result0.CreateTime, result0.Name, result0.Gender));

            Assert.Equal(
                Tuple.Create(testModel.Id, testModel.Age, testModel.CreateTime, testModel.Name, testModel.Gender),
                Tuple.Create(result1.Id, result1.Age, result1.CreateTime, result1.Name, result1.Gender));
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
}