using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace HelloServiceHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //启动宿主，从而启动服务
            using (var host = new MyHelloHost())
            {
                host.Open();
                Console.ReadLine();
            }
        }
    }

    /*
     * 定义宿主（服务对象）
     */
    public class MyHelloHost : IDisposable
    {
        private ServiceHost _myHelloHost;
        public const string BaseAddress = "net.pipe://localhost";
        public const string HelloServiceAddress = "Hello";
        public static readonly Type ServiceType = typeof(HelloService.HelloService);
        public static readonly Type ContractType = typeof(HelloService.IHelloService);
        public static readonly Binding HelloBinding = new NetNamedPipeBinding();

        public MyHelloHost()
        {
            CreateHelloServiceHost();
        }

        protected void CreateHelloServiceHost()
        {
            _myHelloHost = new ServiceHost(ServiceType, new Uri(BaseAddress));  //创建宿主（服务对象）
            _myHelloHost.AddServiceEndpoint(ContractType, HelloBinding, HelloServiceAddress); //添加Endpoint
        }

        public void Open()
        {
            Console.WriteLine("Starting Service......");
            _myHelloHost.Open();
            Console.WriteLine("Service Started.......");
        }

        public void Dispose()
        {
            if (_myHelloHost != null)
                (_myHelloHost as IDisposable).Dispose();
        }
    }
}
