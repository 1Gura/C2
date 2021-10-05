using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class EVM
    {
        /// <summary>
        /// время смены терминала = 30
        /// </summary>
        public int time = 30;


        /// <summary>
        /// Буффер
        /// </summary>
        public List<Zadacha> stash = new List<Zadacha>();

        /// <summary>
        /// Количество символов оставшихся для обработки
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public int Work(Terminal terminal, int h)
        {

            return terminal.Task.N -= terminal.M * h;
        }

        public void timeReset(int time = 30)
        {
            this.time = time;
        }


    }
}
