using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WindowsInput;
using WindowsInput.Native;
using System.Configuration;
using System.ComponentModel.Design.Serialization;
using DarkModeForms;

namespace Test
{
    public partial class Test : Form
    {
        InputSimulator sim = new InputSimulator();

        public Test()
        {
            InitializeComponent();
            _ = new DarkModeCS(this);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Choose a model...")
            {
                richTextBox1.AppendText($"Error: Please choose a model");
            }

            var model = comboBox1.SelectedIndex;
            richTextBox1.Clear();
            textBox1.Select();
        }

        private async Task<string> GetChatGptResponse(string userInput)
        {
            var client = new RestClient("https://api.pawan.krd/v1/chat/completions");
            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("Authorization", "Bearer pk-TUWjjuQEPXPAAfMwiaOSQyneQlCODKxQetTgVWAypweyMlXH");
            request.AddHeader("Content-Type", "application/json");

            var requestBody = new
            {
                model = comboBox1.Text,
                max_tokens = 100,
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = userInput }
                }
            };

            request.AddJsonBody(requestBody);

            try
            {
                RestResponse response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                    return jsonResponse.choices[0].message.content.ToString();
                }
                else
                {
                    return $"Error: {response.StatusDescription} - {response.Content}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            textBox1.Clear();
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void vocalButton_Click(object sender, EventArgs e)
        {
            textBox1.Select();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_H);
        }

        private async void sendButton_Click(object sender, EventArgs e)
        {
            while (true)
            {
                string userInput = textBox1.Text;

                if (string.IsNullOrWhiteSpace(userInput)) break;
                if (userInput == "/exit") {
                    this.Close();
                    break;
                }

                richTextBox1.AppendText("You: " + userInput + "\n\n");
                textBox1.Clear();

                if (!(comboBox1.Text == "pai-001" || comboBox1.Text == "pai-001-light" || comboBox1.Text == "gpt-3.5-unfiltered"))
                {
                    richTextBox1.AppendText($"Error: Please choose a model" + "\n\n");
                    richTextBox1.ForeColor = Color.Crimson;
                    break;
                }
                else
                {
                    richTextBox1.ForeColor = SystemColors.GrayText;
                }

                string response = await GetChatGptResponse(userInput);
                richTextBox1.AppendText("ChatGPT: " + response + "\n\n");
            }
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                while (true)
                {
                    string userInput = textBox1.Text;

                    if (string.IsNullOrWhiteSpace(userInput)) break;
                    if (userInput == "/exit")
                    {
                        this.Close();
                        break;
                    }
                    richTextBox1.AppendText("You: " + userInput + "\n\n");
                    textBox1.Clear();

                    if (!(comboBox1.Text == "pai-001" || comboBox1.Text == "pai-001-light" || comboBox1.Text == "gpt-3.5-unfiltered"))
                    {
                        richTextBox1.AppendText($"Error: Please choose a model" + "\n\n");
                        richTextBox1.ForeColor = Color.Crimson;
                        break;
                    }
                    else
                    {
                        richTextBox1.ForeColor = SystemColors.GrayText;
                    }

                    string response = await GetChatGptResponse(userInput);
                    richTextBox1.AppendText("ChatGPT: " + response + "\n\n");
                }
            }
        }
    }
}
