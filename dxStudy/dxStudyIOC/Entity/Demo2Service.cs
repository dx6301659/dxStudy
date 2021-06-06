using System;

namespace dxStudyIOC
{
    public class Demo2Service : IDemo2Service
    {
        public string Test()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
