using dxStudyIOCByAssembly.Contract;
using System;

namespace dxStudyIOCByAssembly
{
    public class Demo3Service : IDemo3Service
    {
        public string Test()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
