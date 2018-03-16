using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HomeWorkForUAV
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the UAV's meaasge(when enter end will be end):");
            InputUAVMessage();
            SearchMessageByOrder searchMessage = new SearchMessageByOrder();
            while (true)
            {
                Console.WriteLine("Please enter the message number(0-):");
                int singalIndex = Convert.ToInt32(Console.ReadLine());
                searchMessage.SignalIndex = singalIndex;
                searchMessage.SearchMessage();
                Console.ReadKey();
            }
            
        }
        public static void InputUAVMessage()
        {
            string message = Console.ReadLine();
            string path = "../files/UAVMessages.txt";
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            while (!message.Equals("end"))
            {
                sw.WriteLine(message+" "+JudgeInput(message));   //开始写入 
                message = Console.ReadLine();
            } 
            sw.Flush();  //清空缓冲区                
            sw.Close();//关闭流
            fs.Close();
        }
        public static  bool JudgeInput(string message)
        {
            string[] arr = message.Split(' ');
            bool result = false;
            if(Regex.IsMatch(arr[0], @"^[a-zA-Z]+\d$+")){
                result = true;
                for (int i = 1;i < arr.Length; i++)
                {
                    if (!Regex.IsMatch(arr[i], @"^(-|\+)?\d+$"))
                    {
                        result = false;
                        break;
                    }
                }                
            }
            return result;
        }       
    }
}
