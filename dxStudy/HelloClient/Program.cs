using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace HelloClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //调用代理类从而调用宿主中的服务
            using (var proxy = new HelloProxy())
            {
                Console.WriteLine(proxy.Say("dingxu"));
                Console.ReadLine();
            }
        }
    }

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string Say(string name);
    }

    /*
     * 创建客户端服务代理类
     */
    public class HelloProxy : ClientBase<HelloService.IHelloService>, IService
    {
        #region 注意：定义的Binding和EndPointAddress要和宿主类里面的一致
        public static readonly Binding HelloBinding = new NetNamedPipeBinding(); //定义Binding
        public static readonly EndpointAddress HelloAddress = new EndpointAddress(new Uri("net.pipe://localhost/Hello")); //定义Endpoint地址
        #endregion

        public HelloProxy() : base(HelloBinding, HelloAddress) { }

        public string Say(string name)
        {
            return Channel.SayHello(name);
        }
    }
}
