using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HomeWorkForUAV
{
    public class SearchMessageByOrder
    {
        public const int MAX_MESSAGE_LENGTH = 8;
        public int SignalIndex { get; set; }
        private IList<string> Messages { get; set; }
        public SearchMessageByOrder() {
            Messages = GetMessageToFile();
        }
        public SearchMessageByOrder(int signalIndex)
        {
            SignalIndex = signalIndex;
            Messages = GetMessageToFile();
        }
        public  void SearchMessage()
        {
            try
            {
                if (Messages[SignalIndex].Contains("False"))   //判断查询的消息内容是否是输入错误
                    Console.WriteLine("Error: " + SignalIndex);
                if (Messages[0].Contains("False"))    //判断记录的第一条记录是否正确
                    Console.WriteLine("Error: " + SignalIndex);
                else
                {
                    for (int i = 1; i <= SignalIndex; i++)    //从第二条开始判断到输入序号对应的信息
                    {
                        string[] arr = Messages[i].Split(' ');
                        if (!GetUAVState(i))
                        {
                            Console.WriteLine("Error: " + SignalIndex);
                            break;
                        }
                        else if (i == SignalIndex)
                        {
                            int[] indexs = ChangeArray(arr);
                            Console.Write("{0} {1}", arr[0], SignalIndex);
                            foreach (int index in indexs)
                                Console.Write(" " + index);
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Cannot find " + SignalIndex);
            }
        }

        public  bool GetUAVState(int signalIndex)
        {
            string message = Messages[signalIndex];
            if (message.Contains("False"))   //判断用户输入的编号之前的消息是否是无效,如果无效则后面的消息均为无效
                return false;
            else if (message.Split(' ').Length == MAX_MESSAGE_LENGTH && JudgeIndex(signalIndex))   //判断除第一条外的其余数据输入格式是否正确
                return true;
            else return false;
        }

        public bool JudgeIndex( int signalIndex)
        {            
            string[] curStr = Messages[signalIndex].Split(' ');  //得到输入序号的那条消息            
            string[] preStr = Messages[signalIndex - 1].Split(' ');  //得到输入序号的上一条消息
            int[] curArr = new int[] { Int32.Parse(curStr[1]), Int32.Parse(curStr[2]), Int32.Parse(curStr[3]) };
            int[] preArr = signalIndex-1 == 0? new int[] { Int32.Parse(preStr[1]), Int32.Parse(preStr[2]),
                Int32.Parse(preStr[3]) } : ChangeArray(preStr);   //上一条消息坐标
            for(int i = 0;i < curArr.Length; i++)
                if (curArr[i] != preArr[i])
                    return false;
            return true;
        }

        public  int[] ChangeArray(string[] message)
        {
            int[] arr = new int[3];
            int index = 0;
            for (int i = 1, j = 4; i <= 3 && j <=6; i++, j++)
                arr[index++] = Int32.Parse(message[i]) + Int32.Parse(message[j]);
            return arr;
        }

        public  IList<string> GetMessageToFile()
        {
            IList<string> messages = new List<string>();
            string line = "", path = "../files/UAVMessages.txt";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
                messages.Add(line);
            return messages;
        }
    }
}
