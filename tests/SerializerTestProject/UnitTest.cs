using System;
using Xunit;
using Zaabee.RabbitMQ.ISerialize;

namespace SerializerTestProject
{
    public class UnitTest
    {
        [Fact]
        public void JilTest() => SerializerTest(new Zaabee.RabbitMQ.Jil.Serializer());

        [Fact]
        public void NewtonsoftJsonTest() => SerializerTest(new Zaabee.RabbitMQ.NewtonsoftJson.Serializer());

        [Fact]
        public void SystemTextJsonTest() => SerializerTest(new Zaabee.RabbitMQ.SystemTextJson.Serializer());

        [Fact]
        public void Utf8JsonTest() => SerializerTest(new Zaabee.RabbitMQ.Utf8Json.Serializer());

        [Fact]
        public void XmlTest() => SerializerTest(new Zaabee.RabbitMQ.Xml.Serializer());

        private static void SerializerTest(ISerializer serializer)
        {
            var testModel = GetTestModel();
            var bytes = serializer.Serialize(testModel);
            var text = serializer.BytesToText(bytes);
            var result0 = serializer.Deserialize<TestModel>(bytes);
            var result1 = serializer.FromText<TestModel>(text);

            Assert.Equal(
                Tuple.Create(testModel.Id, testModel.Age, testModel.CreateTime, testModel.Name, testModel.Gender),
                Tuple.Create(result0.Id, result0.Age, result0.CreateTime, result0.Name, result0.Gender));
            Assert.Equal(
                Tuple.Create(testModel.Id, testModel.Age, testModel.CreateTime, testModel.Name, testModel.Gender),
                Tuple.Create(result1.Id, result1.Age, result1.CreateTime, result1.Name, result1.Gender));
        }

        private static TestModel GetTestModel()
        {
            return new TestModel
            {
                Id = Guid.NewGuid(),
                Age = new Random().Next(0, 100),
                CreateTime = new DateTime(2017, 1, 1).ToUniversalTime(),
                Name = "apple",
                Gender = Gender.Female
            };
        }
    }
}