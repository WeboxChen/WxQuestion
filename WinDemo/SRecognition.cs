using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinDemo
{
    public class SRecognition
    {
        public SpeechRecognitionEngine recognizer = null;//语音识别引擎  
        public DictationGrammar dictationGrammar = null; //自然语法  
        public System.Windows.Forms.Control cDisplay; //显示控件  

        public SRecognition(string[] fg) //创建关键词语列表  
        {
            CultureInfo myCIintl = new CultureInfo("zh-CN");
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())//获取所有语音引擎  
            {
                if (config.Culture.Equals(myCIintl) && config.Id == "MS-2052-80-DESK")
                {
                    recognizer = new SpeechRecognitionEngine(config);
                    break;
                }//选择识别引擎
            }
            if (recognizer != null)
            {
                InitializeSpeechRecognitionEngine(fg);//初始化语音识别引擎  
                dictationGrammar = new DictationGrammar();
            }
            else
            {
                MessageBox.Show("创建语音识别失败");
            }
            //Grammar mar = new Grammar()
        }
        private void InitializeSpeechRecognitionEngine(string[] fg)
        {
            recognizer.SetInputToDefaultAudioDevice();//选择默认的音频输入设备  
            Grammar customGrammar = CreateCustomGrammar(fg);
            //根据关键字数组建立语法  
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(customGrammar);
            //加载语法  
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            //recognizer.SpeechHypothesized += new EventHandler <SpeechHypothesizedEventArgs>(recognizer_SpeechHypothesized);  
        }
        public void BeginRec(Control tbResult)//关联窗口控件  
        {
            TurnSpeechRecognitionOn();
            TurnDictationOn();
            cDisplay = tbResult;
        }
        public void over()//停止语音识别引擎  
        {
            TurnSpeechRecognitionOff();
        }
        public virtual Grammar CreateCustomGrammar(string[] fg) //创造自定义语法  
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(new Choices(fg));
            return new Grammar(grammarBuilder);
        }
        private void TurnSpeechRecognitionOn()//启动语音识别函数  
        {
            if (recognizer != null)
            {
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                //识别模式为连续识别  
            }
            else
            {
                MessageBox.Show("创建语音识别失败");
            }
        }
        private void TurnSpeechRecognitionOff()//关闭语音识别函数  
        {
            if (recognizer != null)
            {
                recognizer.RecognizeAsyncStop();
                TurnDictationOff();
            }
            else
            {
                MessageBox.Show("创建语音识别失败");
            }
        }
        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //识别出结果完成的动作，通常把识别结果传给某一个控件  
            string text = e.Result.Text;
            cDisplay.Text += text;
        }
        private void TurnDictationOn()
        {
            if (recognizer != null)
            {
                recognizer.LoadGrammar(dictationGrammar);
                //加载自然语法  
            }
            else
            {
                MessageBox.Show("创建语音识别失败");
            }
        }
        private void TurnDictationOff()
        {
            if (dictationGrammar != null)
            {
                recognizer.UnloadGrammar(dictationGrammar);
                //卸载自然语法  
            }
            else
            {
                MessageBox.Show("创建语音识别失败");
            }
        }
    }
}
