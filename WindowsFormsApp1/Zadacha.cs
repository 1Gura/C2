using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Zadacha
    {
        private int id { get; set; }
        public int Id
        {
            get
            {
                return Id;
            }

            set
            {
                if (value > 0)
                {
                    Id = value;
                }
                else
                {
                    Id = 0;
                }
            }
        }

        public int N { get; set; }

        public Zadacha(int N = 60)
        {
            this.N = N;
        }

    }
}
