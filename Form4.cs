using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Math_Quiz_App
{
    public partial class Form4 : Form
    {
        public Form4(int score, int questionCount)
        {
            InitializeComponent();
            label2.Text = $"Your Score: {score} / {questionCount}";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Application.Exit(); // الخروج من التطبيق


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(); // إنشاء فورم البداية
            form1.Show(); // إظهار فورم البداية
            this.Close();
        }
    }
}
