using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int sutki = 24 * 60 * 60;
        private int t1;
        private int t2;
        private int t3;
        private int t4;
        private int N;
        private int M;
        private int h;
        private int j;
        private string writePath = @"test.txt";

        private int countTask = 0;
        private int countStash = 0;
        private int terminalTasks1 = 0;
        private int terminalTasks2 = 0;
        private int terminalTasks3 = 0;

        private List<Terminal> terminals = new List<Terminal> { new Terminal(), new Terminal(), new Terminal() };
        EVM evm = new EVM();
        int k = 1;
        int kTask = 0;

        public Form1()
        {
            InitializeComponent();
            this.Draw();
        }
        public void Draw()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                if (
                textBox9.Text.Length > 0 &&
                textBox10.Text.Length > 0 &&
                textBox11.Text.Length > 0 &&
                textBox12.Text.Length > 0 &&
                textBox13.Text.Length > 0 &&
                textBox14.Text.Length > 0 &&
                textBox15.Text.Length > 0
                )
                {
                    countStash = 0;
                    countTask = 0;
                    terminalTasks1 = 0;
                    terminalTasks2 = 0;
                    terminalTasks3 = 0;

                    label16.Text = "В работе...";
                    timer1.Start();
                    await Task.Run(() =>
                    {
                        Start();
                    });
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля!");

                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Возникла ошибка " + err);
            }

        }

        private void Start()
        {
            try
            {
                t1 = Int32.Parse(textBox9.Text);
                t2 = Int32.Parse(textBox10.Text);
                t3 = Int32.Parse(textBox11.Text);
                t4 = Int32.Parse(textBox12.Text);
                N = Int32.Parse(textBox14.Text);
                M = Int32.Parse(textBox13.Text);
                h = Int32.Parse(textBox15.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Проверьте правильность введенных данных!");
            }


            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine("");
            }

            for (j = 3; j < sutki; j += h)
            {
                Thread.Sleep(1);
                setTasks();
                if (terminals[0].Task != null || terminals[1].Task != null || terminals[2].Task != null)
                {

                    /// Тут идет обработка терминалом задачи, если задача успела выполнится,
                    /// то просто переходим к след. терминалу(или если ее нет),
                    /// иначе, если закончилось отведенное время поместим недоделанную задачу в СТЭШ
                    /// и перейдем к след. терминалу

                    int workOver = 0;
                    int timeOver = 0;
                    switch (k)
                    {

                        case 1:
                            workOver = (evm.Work(terminals[0], h));
                            timeOver = evm.time -= h;
                            if (workOver <= 0 || timeOver <= 0)
                            {
                                evm.timeReset();
                                if (workOver <= 0)
                                {
                                    fileWrite("Задача была решена на 1 терминале" + " на " + j);
                                    terminalTasks1++;
                                    countTask++;
                                }
                                else
                                {
                                    evm.stash.Add(terminals[0].Task);
                                    fileWrite("Задача была помещена в очередь" + " на " + j);
                                    countStash++;
                                }
                                terminals[0].Task = null;
                                k = 2;
                            }
                            break;
                        case 2:

                            workOver = (evm.Work(terminals[1], h));
                            timeOver = evm.time -= h;
                            if (workOver <= 0 || timeOver <= 0)
                            {

                                evm.timeReset();
                                if (workOver <= 0)
                                {
                                    fileWrite("Задача была решена на 2 терминале" + " на " + j);
                                    terminalTasks2++;
                                    countTask++;
                                }
                                else
                                {
                                    evm.stash.Add(terminals[1].Task);
                                    fileWrite("Задача была помещена в очередь" + " на " + j);
                                    countStash++;
                                }
                                terminals[1].Task = null;
                                k = 3;
                            }
                            break;
                        case 3:

                            workOver = (evm.Work(terminals[2], h));
                            timeOver = evm.time -= h;
                            if (workOver <= 0 || timeOver <= 0)
                            {

                                evm.timeReset();
                                if (workOver <= 0)
                                {
                                    fileWrite("Задача была решена на 3 терминале" + " на " + j);
                                    terminalTasks3++;
                                    countTask++;
                                }
                                else
                                {
                                    evm.stash.Add(terminals[2].Task);
                                    fileWrite("Задача была помещена в очередь)" + " на " + j);
                                    countStash++;
                                }
                                terminals[2].Task = null;
                                k = 1;
                            }
                            break;
                    }
                }
                else
                {
                    if (evm.stash.Count() > 0)
                    {
                        terminals[k].Task = evm.stash.FirstOrDefault();
                        fileWrite("Задача была помещена в терминал из очереди" + " на " + j);
                    }
                }
                if (j >= sutki)
                {
                    RadioRemove();
                    sendMessage();
                    timer1.Stop();
                    label16.Text = "Работа завершена успехом";
                    MessageBox.Show("Работа завершена успехом");
                    return;
                }
            }
        }

        private void setTerminal(bool workOver)
        {
            evm.timeReset();
            if (!workOver)
            {
                evm.stash.Add(terminals[1].Task);
            }
            terminals[1].Task = null;
        }

        private void RadioBtnActivated(int k)
        {
            switch (k)
            {
                case 1:
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;

                    break;
                case 2:
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;

                    break;
                case 3:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    break;
            }
        }

        private void RadioRemove()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

        }
        /// <summary>
        ///Тут должны поступать задания на каждый терминал с учётом времени t1, t2, t3
        /// </summary>
        private void setTasks()
        {
            var count = 0;
            foreach (Terminal terminal in terminals)
            {
                terminal.interval -= h;
                if (terminal.interval <= 0)
                {
                    count++;
                    if (terminal.Task == null)
                    {
                        kTask = count;
                        var task = new Zadacha();
                        terminal.Task = task;
                        fileWrite(" " + "Задача поступила в терминал" + "№" + count + " на " + j);
                    }
                    else
                    {
                        evm.stash.Add(new Zadacha());
                        fileWrite("Задача была помещана в очередь т.к. Т" + count + "занят");
                        countStash++;

                    }
                    terminal.interval = 30;
                }
            }
        }

        private void sendMessage(int kTask)
        {
            switch (kTask)
            {
                case 0:
                    pictureBox8.Visible = true;
                    pictureBox9.Visible = false;
                    pictureBox10.Visible = false;
                    break;
                case 1:
                    pictureBox8.Visible = false;
                    pictureBox9.Visible = true;
                    pictureBox10.Visible = false;
                    break;
                case 2:
                    pictureBox8.Visible = false;
                    pictureBox9.Visible = false;
                    pictureBox10.Visible = true;
                    break;
            }
        }

        private void sendMessage()
        {
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
        }



        private void fileWrite(string str = "")
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    sw.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void process1_Exited(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RadioBtnActivated(k);
            sendMessage(kTask);
            textBox7.Text = countTask.ToString();
            textBox8.Text = countStash.ToString();
            textBox16.Text = terminalTasks1.ToString();
            textBox17.Text = terminalTasks2.ToString();
            textBox18.Text = terminalTasks3.ToString();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
