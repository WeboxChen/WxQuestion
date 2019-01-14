using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinDemo
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
            string path = this.textBox1.Text;
            if (!System.IO.File.Exists(path))
                return;

            // 获取语音配置
            var configs = SpeechRecognitionEngine.InstalledRecognizers();
            var config = configs.First(x => string.Equals(x.Culture.Name, "zh-CN", StringComparison.CurrentCultureIgnoreCase));
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(config);
            Grammar grammer = new Grammar(path);
            //recognizer.re
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            //player.pl

        }
    }
}
