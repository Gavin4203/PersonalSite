using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PassDA
    {
        private string _pass = Environment.GetEnvironmentVariable("PASS");

        //Will access the password
        public string[] GetPassword()
        {
            try
            {
                return _pass.Split(':');
            }
            catch
            {
                return (null);
            }
        }
    }
}
