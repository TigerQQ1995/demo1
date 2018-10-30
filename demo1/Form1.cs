using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace demo1
{
    public partial class Form1 : Form
    {
        aubocontrol.Aubo AuboArm = new aubocontrol.Aubo();

        SerialPort port;

        #region 窗口初始化以及JZ串口设置
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

            if (port == null)
            {
                //COM4为Arduino使用的串口号，需根据实际情况调整
                port = new SerialPort("COM4", 9600);
                port.Open();
            }
        }
        #endregion

        #region JZ初始化
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
            }
        }

        //点亮
        private void rbOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (this.open.Checked)
            {
                PortWrite("0");
            }
        }

        //熄灭
        private void rbClose_CheckedChanged(object sender, EventArgs e)
        {
            if (this.close.Checked)
            {
                PortWrite("1");
            }
        }

        //向串口输出命令字符
        private void PortWrite(string message)
        {
            if (port != null && port.IsOpen)
            {
                port.Write(message);
                //port.WriteLine(message);
            }
        }
        #endregion

        #region 机器人开关态
        private void button1_Click(object sender, EventArgs e)
        {
            AuboArm.OpenRobot();


        }
        private void button4_Click(object sender, EventArgs e)
        {
            AuboArm.botclose();
        }
        #endregion

        #region movej
        private void button2_Click(object sender, EventArgs e)
        {
            Thread th_movej = new Thread(movej);
            th_movej.IsBackground = true;
            th_movej.Start();

           
        }//关节移动到
        //j1~j6     弧度    取值范围  -3.14/2  ~3.14/2
        //速度speed  取值范围  0~100
        private void movej()
        {
            //movej
            AuboArm.moveline(double.Parse(textBox16.Text), double.Parse(textBox17.Text), double.Parse(textBox18.Text),
              double.Parse(textBox19.Text), double.Parse(textBox20.Text), double.Parse(textBox21.Text), double.Parse(textBox22.Text));
        }
        #endregion

        #region line
        private void button3_Click(object sender, EventArgs e)
        {
            AuboArm.movelineto2(-0.067, -0.223, 0.88, 10);

//[-3.14/2  ~3.14/2]//速度speed  取值范围  0~100
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Thread th_line = new Thread(line);
            th_line.IsBackground = true;
            th_line.Start();
        }
        private void line()
        {
            AuboArm.movelineto2(double.Parse(L_x.Text), double.Parse(L_y.Text), double.Parse(L_z.Text), double.Parse(L_v.Text));
        }
        #endregion 
     

        #region 控制夹抓开关
        private void open_CheckedChanged(object sender, EventArgs e)
        {
            Thread th_open = new Thread(openJZ);
            th_open.IsBackground = true;
            th_open.Start();
        }
        private void openJZ()
        {
            if (this.open.Checked)
            {
                PortWrite("1");
            }
        }
        private void close_CheckedChanged(object sender, EventArgs e)
        {
            Thread th_close = new Thread(closeJZ);
            th_close.IsBackground = true;
            th_close.Start();
        }
        private void closeJZ()
        {
            if (this.close.Checked)
            {
                PortWrite("0");
            }
        }
        #endregion
    }
}
