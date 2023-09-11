// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Zaabee.RabbitMQ.Demo.VNext;

public static class Program
{
    private const string ExchangeNormal = "Exchange.Normal"; //定义一个用于接收 正常 消息的交换机
    private const string ExchangeRetry = "Exchange.Retry"; //定义一个用于接收 重试 消息的交换机
    private const string ExchangeFail = "Exchange.Fail"; //定义一个用于接收 失败 消息的交换机
    private const string QueueNormal = "Queue.Noraml"; //定义一个用于接收 正常 消息的队列
    private const string QueueRetry = "Queue.Retry"; //定义一个用于接收 重试 消息的队列
    private const string QueueFail = "Queue.Fail"; //定义一个用于接收 失败 消息的队列

    public static void Main()
    {
        var factory = new ConnectionFactory();
        {
            factory.HostName = "127.0.0.1";
            factory.Port = 5672;
            factory.VirtualHost = "VH1"; //选择虚拟主机
            factory.UserName = "user1";
            factory.Password = "123456";
            factory.AutomaticRecoveryEnabled = true; //开启自动恢复连接(默认为 false，在异常中断时无法自动恢复连接)
            //factory.ClientProvidedName = "测试程序",
            factory.ClientProperties.Add("connection_name", "测试程序"); //#为兼容3.5.7显示客户端名称问题
            factory.ClientProperties.Add("tag", "测试程序"); //#为解决5.2.0版本不显示客户端名称问题
        }
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        //声明交换机
        channel.ExchangeDeclare(exchange: ExchangeNormal, type: "topic");
        channel.ExchangeDeclare(exchange: ExchangeRetry, type: "topic");
        channel.ExchangeDeclare(exchange: ExchangeFail, type: "topic");

        //定义队列参数
        var queueNormalArgs = new Dictionary<string, object>();
        {
            queueNormalArgs.Add("x-dead-letter-exchange", ExchangeFail); //指定死信交换机，用于将 Noraml 队列中失败的消息投递给 Fail 交换机
        }
        var queueRetryArgs = new Dictionary<string, object>();
        {
            queueRetryArgs.Add("x-dead-letter-exchange", ExchangeNormal); //指定死信交换机，用于将 Retry 队列中超时的消息投递给 Noraml 交换机
            queueRetryArgs.Add("x-message-ttl", 60000); //定义 queueRetry 的消息最大停留时间 (原理是：等消息超时后由 broker 自动投递给当前绑定的死信交换机)
            //定义最大停留时间为防止一些 待重新投递 的消息、没有定义重试时间而导致内存溢出
        }
        var queueFailArgs = new Dictionary<string, object>();
        {
            //暂无
        }

        //声明队列
        channel.QueueDeclare(queue: QueueNormal, durable: true, exclusive: false, autoDelete: false,
            arguments: queueNormalArgs);
        channel.QueueDeclare(queue: QueueRetry, durable: true, exclusive: false, autoDelete: false,
            arguments: queueRetryArgs);
        channel.QueueDeclare(queue: QueueFail, durable: true, exclusive: false, autoDelete: false,
            arguments: queueFailArgs);

        //为队列绑定交换机
        channel.QueueBind(queue: QueueNormal, exchange: ExchangeNormal, routingKey: "#");
        channel.QueueBind(queue: QueueRetry, exchange: ExchangeRetry, routingKey: "#");
        channel.QueueBind(queue: QueueFail, exchange: ExchangeFail, routingKey: "#");

        #region 创建一个普通消息消费者

        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var _sender = (EventingBasicConsumer)sender; //消息传送者
                var _channel = _sender.Model; //消息传送通道
                var _message = (BasicDeliverEventArgs)e; //消息传送参数
                var _headers = _message.BasicProperties.Headers; //消息头
                var _content = Encoding.UTF8.GetString(_message.Body.Span); //消息内容
                var _death = default(Dictionary<string, object>); //死信参数

                if (_headers != null && _headers.ContainsKey("x-death"))
                    _death = (Dictionary<string, object>)(_headers["x-death"] as List<object>)[0];

                #region 消息处理

                try
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        $"{DateTime.Now:HH:mm:ss.fff}\t(1.0)消息接收：\r\n\t[deliveryTag={_message.DeliveryTag}]\r\n\t[consumerID={_message.ConsumerTag}]\r\n\t[exchange={_message.Exchange}]\r\n\t[routingKey={_message.RoutingKey}]\r\n\t[content={_content}]");

                    throw new Exception("模拟消息处理失败效果。");

                    //处理成功时
                    Console.WriteLine(
                        $"{DateTime.Now:HH:mm:ss.fff}\t(1.1)处理成功：\r\n\t[deliveryTag={_message.DeliveryTag}]");

                    //消息确认 (销毁当前消息)
                    _channel.BasicAck(deliveryTag: _message.DeliveryTag, multiple: false);
                }

                #endregion

                catch (Exception ex)
                {
                    #region 消息处理失败时

                    var retryCount = (long)(_death?["count"] ?? default(long)); //查询当前消息被重新投递的次数 (首次则为0)

                    Console.WriteLine(
                        $"{DateTime.Now:HH:mm:ss.fff}\t(1.2)处理失败：\r\n\t[deliveryTag={_message.DeliveryTag}]\r\n\t[retryCount={retryCount}]");

                    if (retryCount >= 2)

                        #region 投递第3次还没消费成功时，就转发给 exchangeFail 交换机

                    {
                        //消息拒绝（投递给死信交换机，也就是上边定义的 ("x-dead-letter-exchange", _exchangeFail)）
                        _channel.BasicNack(deliveryTag: _message.DeliveryTag, multiple: false, requeue: false);
                    }

                    #endregion

                    #region 否则转发给 exchangeRetry 交换机

                    else
                    {
                        var interval = (retryCount + 1) * 10; //定义下一次投递的间隔时间 (单位：秒)
                        //如：首次重试间隔10秒、第二次间隔20秒、第三次间隔30秒

                        //定义下一次投递的间隔时间 (单位：毫秒)
                        _message.BasicProperties.Expiration = (interval * 1000).ToString();

                        //将消息投递给 _exchangeRetry (会自动增加 death 次数)
                        _channel.BasicPublish(exchange: ExchangeRetry, routingKey: _message.RoutingKey,
                            basicProperties: _message.BasicProperties, body: _message.Body);

                        //消息确认 (销毁当前消息)
                        _channel.BasicAck(deliveryTag: _message.DeliveryTag, multiple: false);
                    }

                    #endregion
                }

                #endregion
            };
            channel.BasicConsume(queue: QueueNormal, autoAck: false, consumer: consumer);
        }

        #endregion

        #region 创建一个失败消息消费者

        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (_, e) =>
            {
                var message = (BasicDeliverEventArgs)e; //消息传送参数
                var content = Encoding.UTF8.GetString(message.Body.Span); //消息内容

                Console.WriteLine();
                Console.WriteLine(
                    $"{DateTime.Now:HH:mm:ss.fff}\t(2.0)发现失败消息：\r\n\t[deliveryTag={message.DeliveryTag}]\r\n\t[consumerID={message.ConsumerTag}]\r\n\t[exchange={message.Exchange}]\r\n\t[routingKey={message.RoutingKey}]\r\n\t[content={content}]");
            };

            channel.BasicConsume(queue: QueueFail, autoAck: true, consumer: consumer);
        }

        #endregion

        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\t 正在运行中...");

        string? cmd;
        while ((cmd = Console.ReadLine()) != "close")

            #region 模拟正常消息发布

        {
            var msgProperties = channel.CreateBasicProperties();
            var msgContent = $"消息内容_{DateTime.Now:HH:mm:ss.fff}_{cmd}";

            channel.BasicPublish(exchange: ExchangeNormal, routingKey: "亚洲.中国.经济", basicProperties: msgProperties,
                body: Encoding.UTF8.GetBytes(msgContent));

            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\t 发送成功：{msgContent}");
            Console.WriteLine();
        }

        #endregion

        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\t 正在关闭...");

        channel.ExchangeDelete(ExchangeNormal);
        channel.ExchangeDelete(ExchangeRetry);
        channel.ExchangeDelete(ExchangeFail);
        channel.QueueDelete(QueueNormal);
        channel.QueueDelete(QueueRetry);
        channel.QueueDelete(QueueFail);
        //channel.Abort();
        channel.Close(200, "Goodbye!");
        channel.Dispose();
        connection.Close(200, "Goodbye!");
        connection.Dispose();

        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\t 运行结束。");
        Console.ReadKey();
    }
}