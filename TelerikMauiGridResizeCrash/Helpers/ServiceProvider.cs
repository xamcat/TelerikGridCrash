using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikMauiGridResizeCrash.Helpers
{
    public class ServiceProvider
    {
        private static readonly object _locker = new object();

        private static IServiceProvider _instance;

        public static IServiceProvider Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        throw new NotImplementedException("Call Init() first");
                    }
                    return _instance;
                }
            }
        }

        public static void Init(Func<IServiceProvider> initializeFunc)
        {
            _instance = initializeFunc();
        }
    }
}
