using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6_OS
{
    public class Process
    {
        private int size = 0;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public int GetSegments(int i)
        {
            if (size % i > 0)
                return size / i + 1;
            return size / i;
        }
    }
}
