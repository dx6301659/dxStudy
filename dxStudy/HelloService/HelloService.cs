namespace HelloService
{
    public class HelloService : IHelloService
    {
        public string SayHello(string name)
        {
            return $"Hello，I am {name}";
        }
    }
}
