using System;

namespace dxStudyIOC
{
    public class DemoService
    {
        public string Test()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
