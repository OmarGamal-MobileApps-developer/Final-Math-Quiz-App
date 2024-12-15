using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Math_Quiz_App
{
    public partial class Form3 : Form
    {
        public string SelectedDifficulty { get; set; }
        public int NumberOfQuestions { get; set; }
        public List<Question> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        public Form3(string difficulty, int numberOfQuestions)
        {
            InitializeComponent();

            SelectedDifficulty = difficulty;
            NumberOfQuestions = numberOfQuestions;
            _questions = QuestionGenerator.Generate(NumberOfQuestions, SelectedDifficulty);
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            var currentQuestion = _questions[_currentIndex];
            label1.Text = currentQuestion.Text;
            radioButton1.Text = Convert.ToString(currentQuestion.Choices[0]);
            radioButton2.Text = Convert.ToString(currentQuestion.Choices[1]);
            radioButton3.Text = Convert.ToString(currentQuestion.Choices[2]);
            radioButton4.Text = Convert.ToString(currentQuestion.Choices[3]);
        }

        private void CheckAnswer()
        {
            var currentQuestion = _questions[_currentIndex];
            string selectedChoice = "";
            if (radioButton1.Checked) selectedChoice = radioButton1.Text;
            else if (radioButton2.Checked) selectedChoice = radioButton2.Text;
            else if (radioButton3.Checked) selectedChoice = radioButton3.Text;
            else if (radioButton4.Checked) selectedChoice = radioButton4.Text;

            if (string.IsNullOrEmpty(selectedChoice))
            {
                MessageBox.Show("Please select an answer before proceeding to the next question.");
                return;
            }
            if (int.TryParse(selectedChoice, out int selectedAnswer))
            {
                if (selectedAnswer == currentQuestion.CorrectAnswer)
                {
                    _score++;
                    MessageBox.Show("Correct");
                }
                else
                {
                    MessageBox.Show($"Wrong Answer. The correct answer is {currentQuestion.CorrectAnswer}.");
                }
            }
            else
            {
                MessageBox.Show("Invalid choice selected.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckAnswer();

            if (_currentIndex < _questions.Count - 1)
            {
                _currentIndex++;
                DisplayQuestion();
            }
            else
            {
                Form4 form4 = new Form4(_score, _questions.Count);
                form4.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                DisplayQuestion();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // يمكن إضافة منطق تحميل الأسئلة في هذه المرحلة أيضًا إذا لزم الأمر
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // يمكنك إضافة منطق هنا إذا كان مطلوب
        }
    }

    public class Question
    {
        public string Text { get; set; }
        public List<int> Choices { get; set; }
        public int CorrectAnswer { get; set; }
    }

    public static class QuestionGenerator
    {
        public static List<Question> Generate(int count, string difficulty)
        {
            string connectionString = "Server=OMAR;Database=Quiz;Integrated Security=True;";
            var questions = new List<Question>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT TOP (@NumberOfQuestions) QuestionText, Choices, Answer
                        FROM Questions
                        WHERE DifficultyLevel = @DifficultyLevel
                        ORDER BY NEWID()";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumberOfQuestions", count);
                        command.Parameters.AddWithValue("@DifficultyLevel", difficulty);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var question = new Question
                                {
                                    Text = reader["QuestionText"].ToString(),
                                    Choices = reader["Choices"].ToString().Split(',').Select(int.Parse).ToList(),
                                    CorrectAnswer = int.Parse(reader["Answer"].ToString())
                                };
                                questions.Add(question);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            return questions;
        }
    }
}