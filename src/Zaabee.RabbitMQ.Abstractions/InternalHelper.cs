using System.Linq;

namespace Zaabee.RabbitMQ.Abstractions;

internal static class InternalHelper
{
    private static readonly ConcurrentDictionary<Type, string> TopicNameDic = new();

    internal static string GetTopicName(Type type) =>
        TopicNameDic.GetOrAdd(
            type,
            _ =>
                type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault()
                    is MessageVersionAttribute msgVerAttr
                    ? $"{type}[{msgVerAttr.Version}]"
                    : type.ToString()
        );

    internal static string GenerateQueueName<T>(Func<Action<T>> resolve, bool isExclusive)
    {
        var handle = resolve();
        var messageName = GetTopicName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]{(isExclusive ? $"[{Guid.NewGuid()}]" : string.Empty)}";
    }

    internal static string GenerateQueueName<T>(Func<Func<T, Task>> resolve, bool isExclusive)
    {
        var handle = resolve();
        var messageName = GetTopicName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]{(isExclusive ? $"[{Guid.NewGuid()}]" : string.Empty)}";
    }

    internal static string GenerateQueueName<T>(Func<Func<T, ValueTask>> resolve, bool isExclusive)
    {
        var handle = resolve();
        var messageName = GetTopicName(typeof(T));
        return $"{handle.Method.ReflectedType?.FullName}.{handle.Method.Name}[{messageName}]{(isExclusive ? $"[{Guid.NewGuid()}]" : string.Empty)}";
    }
}
