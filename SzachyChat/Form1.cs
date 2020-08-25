using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SzachyChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void WriteInColour(Color c, string s, int start, int length)
        {
            Chat_TextBox.SelectionStart = Chat_TextBox.Text.Length;
            Chat_TextBox.SelectionLength = length;
            Chat_TextBox.SelectionColor = c;
            Chat_TextBox.AppendText(s.Substring(start, length));
            Chat_TextBox.SelectionColor = Color.Black;
            Chat_TextBox.AppendText(s.Substring(length));
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            Names_Label.Text = Names_Label.Text.Replace("Nick_1", ChatClient.thisNick).Replace("Color_1", ChatClient.thisColour).Replace("Nick_2", ChatClient.enemyNick).Replace("Color_2", ChatClient.enemyColour);
            Chat_TextBox.ForeColor = Color.Black;
            WriteInColour(Color.Red, $"{ChatClient.thisNick}: Hi\n", 0, ChatClient.thisNick.Length);
            WriteInColour(Color.PaleVioletRed, $"{ChatClient.enemyNick}: Hello\n", 0, ChatClient.enemyNick.Length);
            WriteInColour(Color.Aquamarine, "Abcabcabc: a message", 0, 9);
            await Task.Run(() => ChatClient.StartReading());
            
            await Task.Run(() => {
                while(true)
                {
                    if(ChatClient.read_strings.Count > 0)
                    {
                        lock(ChatClient.read_strings)
                        {
                            Chat_TextBox.Text += $"{ChatClient.enemyNick}: {ChatClient.read_strings.Dequeue()}\n"; 
                        }
                    }
                    System.Threading.Thread.Sleep(350);
                }
            });
        }
    }
}
