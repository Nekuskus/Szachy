using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            Chat_TextBox.Text = "";
            ChatClient.Init();
            ChatClient.StartReading();
            await Task.Run(() => {
                while(true)
                {
                    if(ChatClient.read_strings.Count > 0)
                    {
                        lock(ChatClient.read_strings)
                        {
                            Chat_TextBox.Text += $"Example_User: {ChatClient.read_strings.Dequeue()}"; 
                        }
                    }
                    System.Threading.Thread.Sleep(350);
                }
            });
        }
    }
}
