using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPhoneApp
{
    public partial class Form1 : Form
    {
        private List<string> dictionary;
        private string currentInput;
        private Dictionary<string, string[]> t9Mapping;
        private Dictionary<string, int> keyPressCounter;
        private Dictionary<string, DateTime> lastKeyPressTime;
        private const int keyPressTimeout = 1000; // 1 second
        private Button[] buttons;
        private bool isDragging = false;
        private Point lastPoint;
        private string filePath = @"C:\Users\Andriy\source\repos\ProjectPhoneApp\ProjectPhoneApp\dictionary.txt";

        public Form1()
        {
            InitializeComponent();
            InitializeButtons();
            dictionary = new List<string> { "hello", "world", "test", "message", "text", "yutube", "csharp", "java" };
            currentInput = string.Empty;
            keyPressCounter = new Dictionary<string, int>();
            lastKeyPressTime = new Dictionary<string, DateTime>();
            InitializeT9Mapping();
            InitializeKeyPressCounter();
        }

        private void InitializeButtons()
        {
            int buttonWidth = 60;
            int buttonHeight = 53;
            int startX = 12;
            int startY = 250;
            int padding = 10;
            int arrowSize = 30;

            string[] buttonLabels = new string[]
            {
             "1", "2\nАБВГ\nABC", "3\nДУЖЗ\nDEF",
             "4\nІЙКЛ\nGHI", "5\nМНОП\nJKL", "6\nРСТУ\nMNO",
             "7\nФХСЧ\nPQRS", "8\nШЩЦЬ\nTUV", "9\nЄЇЮЯ\nWXYZ",
             "*\n", "0\n+", "#\n",
            };

            this.buttons = new Button[buttonLabels.Length];
            for (int i = 0; i < buttonLabels.Length; i++)
            {
                this.buttons[i] = new Button
                {
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(startX + (i % 3) * (buttonWidth + padding), startY + (i / 3) * (buttonHeight + padding)),
                    Name = "button" + buttonLabels[i],
                    Text = buttonLabels[i],
                    UseVisualStyleBackColor = true,
                    Tag = buttonLabels[i].Substring(0, 1)
                };
                this.buttons[i].Click += NumericButton_Click;
                this.Controls.Add(this.buttons[i]);
            }

            // Створення кнопок стрілок
            var arrows = new[]
            {
            new { Tag = "Up", Text = "↑", Location = new Point(100, 130) },
            new { Tag = "Down", Text = "↓", Location = new Point(100, 210) },
            new { Tag = "Left", Text = "←", Location = new Point(50, 170) },
            new { Tag = "Right", Text = "→", Location = new Point(150, 170) },
            new { Tag = "Select", Text = "OK", Location = new Point(100, 170) }

            };

            foreach (var arrow in arrows)
            {
                Button button = CreateArrowButton(arrow.Tag, arrow.Text, arrow.Location);
                this.Controls.Add(button);
            }
        }

        private Button CreateButton(string label, Point location)
        {
            Button button = new Button();
            button.Size = new System.Drawing.Size(60, 53);
            button.Location = location;
            button.Text = label;
            button.Tag = label.Substring(0, 1);
            button.Click += new System.EventHandler(this.NumericButton_Click);
            return button;
        }

        private Button CreateArrowButton(string tag, string text, Point location)
        {
            Button button = new Button();
            button.Size = new System.Drawing.Size(30, 30);
            button.Location = location;
            button.Text = text;
            button.Tag = tag;
            button.Click += new System.EventHandler(this.ArrowButton_Click);
            return button;
        }

        private void ArrowButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string direction = button.Tag.ToString();
                HandleArrowButton(direction);
            }
        }

        private void HandleArrowButton(string direction)
        {
            if (listBoxSuggestions.Items.Count == 0)
                return;

            int currentIndex = listBoxSuggestions.SelectedIndex;

            switch (direction)
            {
                case "Up":
                    if (currentIndex > 0)
                        listBoxSuggestions.SelectedIndex = currentIndex - 1;
                    break;
                case "Down":
                    if (currentIndex < listBoxSuggestions.Items.Count - 1)
                        listBoxSuggestions.SelectedIndex = currentIndex + 1;
                    break;
                case "Select":
                    if (currentIndex >= 0)
                        textBoxInput.Text = listBoxSuggestions.SelectedItem.ToString();
                    break;
            }
        }

        private void MoveSelection(string direction)
        {
        }

        private void InitializeT9Mapping()
        {
            t9Mapping = new Dictionary<string, string[]>
            {
                { "2", new string[] { "2", "а", "б", "в", "г","a", "b", "c" } },
                { "3", new string[] { "3", "д", "е", "ж", "з", "d", "e", "f" } },
                { "4", new string[] { "4", "і", "й", "к", "л", "g", "h", "i" } },
                { "5", new string[] { "5", "м", "н", "о", "п", "j", "k", "l" } },
                { "6", new string[] { "6", "р", "с", "т", "у", "m", "n", "o" } },
                { "7", new string[] { "7", "ф", "х", "с", "ч", "p", "q", "r", "s" } },
                { "8", new string[] { "8", "ш", "щ", "ц", "ь", "t", "u", "v" } },
                { "9", new string[] { "9", "є", "ї", "ю", "я", "w", "x", "y", "z" } },
                { "0", new string[] { "0", "+" } }, 
                { "*", new string[] { "*" } }, 
                { "#", new string[] { "#" } }
            };
        }

        private void InitializeKeyPressCounter()
        {
            for (int i = 2; i <= 10; i++)
            {
                keyPressCounter[i.ToString()] = 0;
                lastKeyPressTime[i.ToString()] = DateTime.MinValue;
            }
        }

        private void NumericButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string buttonText = button.Tag.ToString();

                if (t9Mapping.ContainsKey(buttonText))
                {
                    if (buttonText == "0" || buttonText == "*" || buttonText == "#")
                    {
                        textBoxInput.Text += t9Mapping[buttonText][0];
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        if (now - lastKeyPressTime[buttonText] > TimeSpan.FromMilliseconds(keyPressTimeout))
                        {
                            keyPressCounter[buttonText] = 0;
                        }
                        else
                        {
                            keyPressCounter[buttonText]++;
                            if (keyPressCounter[buttonText] >= t9Mapping[buttonText].Length)
                            {
                                keyPressCounter[buttonText] = 0;
                            }
                        }

                        lastKeyPressTime[buttonText] = now;

                        // Get the corresponding character
                        string letter = t9Mapping[buttonText][keyPressCounter[buttonText]];

                        // Replace the last character if it was entered by the same button
                        if (textBoxInput.Text.Length > 0 && textBoxInput.Text.Last().ToString() == t9Mapping[buttonText][(keyPressCounter[buttonText] == 0 ? t9Mapping[buttonText].Length : keyPressCounter[buttonText]) - 1])
                        {
                            textBoxInput.Text = textBoxInput.Text.Substring(0, textBoxInput.Text.Length - 1);
                        }
                        textBoxInput.Text += letter;

                        // Update the current input
                        currentInput = textBoxInput.Text;
                        FindSuggestions(currentInput);
                    }
                }
            }
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            currentInput = textBoxInput.Text;
            if (!string.IsNullOrEmpty(currentInput))
            {
                Task.Run(() => FindSuggestions(currentInput));
            }
        }

        private void FindSuggestions(string input)
        {
            var suggestions = dictionary.Where(word => word.StartsWith(input)).ToList();
            Invoke(new Action(() =>
            {
                listBoxSuggestions.DataSource = suggestions;
                listBoxSuggestions.SelectedIndex = -1; // Скинути вибір
            }));
        }

        private void buttonAddWord_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentInput) && !dictionary.Contains(currentInput))
            {
                dictionary.Add(currentInput);
                MessageBox.Show("Word added to dictionary");

                // Збереження оновленого словника у файл
                SaveDictionaryToFile(filePath);
            }
        }

        private void buttonSelectWord_Click(object sender, EventArgs e)
        {
            if (listBoxSuggestions.SelectedItem != null)
            {
                textBoxInput.Text = listBoxSuggestions.SelectedItem.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDictionaryFromFile(filePath);
        }

        private void listBoxSuggestions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastPoint = e.Location;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPoint = PointToScreen(e.Location);
                Location = new Point(currentPoint.X - lastPoint.X, currentPoint.Y - lastPoint.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void SaveDictionaryToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string word in dictionary)
                    {
                        writer.WriteLine(word);
                    }
                }
                MessageBox.Show("Dictionary saved to file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving dictionary: " + ex.Message);
            }
        }

        private void LoadDictionaryFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    dictionary.Clear();
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            dictionary.Add(line.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dictionary: " + ex.Message);
            }
        }

        private void buttonDeleteLastChar_Click(object sender, EventArgs e)
        {
            if (listBoxSuggestions.SelectedItem != null)
            {
                string selectedWord = listBoxSuggestions.SelectedItem.ToString();

              
                var items = (List<string>)listBoxSuggestions.DataSource;
                if (items != null)
                {
                    items.Remove(selectedWord);
                  
                    listBoxSuggestions.DataSource = null;
                    listBoxSuggestions.DataSource = items;
                }

             
                RemoveWordFromFile(selectedWord);


                dictionary.Remove(selectedWord);
                SaveDictionaryToFile(filePath);
            }
            else if (textBoxInput.Text.Length > 0)
            {
                textBoxInput.Text = textBoxInput.Text.Substring(0, textBoxInput.Text.Length - 1);
            }
        }

        private void RemoveWordFromFile(string word)
        {
      
            var lines = File.ReadAllLines(filePath).ToList();

            // Видалення рядка, що містить слово
            lines.RemoveAll(line => line.Trim().Equals(word, StringComparison.OrdinalIgnoreCase));

         
            File.WriteAllLines(filePath, lines);
        }
    }
}