using System.ServiceModel;

namespace HelloService
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        string SayHello(string name);
    }
}
