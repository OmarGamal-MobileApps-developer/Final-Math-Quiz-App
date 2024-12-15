using System;
using System.Windows.Forms;

namespace Math_Quiz_App
{
    public partial class Form2 : Form
    {
        public string SelectedDifficulty { get; private set; }
        public int NumberOfQuestions { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBoxDifficulty.Items.AddRange(new string[] { "Easy", "Normal", "Hard" });
            comboBoxNumberOfQuestions.Items.AddRange(new string[] { "5", "10", "15", "20" });
        }

        private void buttonStartQuiz_Click(object sender, EventArgs e)
        {
            if (comboBoxDifficulty.SelectedIndex != -1 && comboBoxNumberOfQuestions.SelectedIndex != -1)
            {
                SelectedDifficulty = comboBoxDifficulty.SelectedItem.ToString();
                NumberOfQuestions = int.Parse(comboBoxNumberOfQuestions.SelectedItem.ToString());

                Form3 form3 = new Form3(SelectedDifficulty, NumberOfQuestions);
                form3.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please select difficulty level and number of questions");
            }
        }
    }
}