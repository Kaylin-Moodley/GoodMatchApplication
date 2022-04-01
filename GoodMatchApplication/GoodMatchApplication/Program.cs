using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoodMatchApplication
{
    class Program
    {
        //Returns the first name
        public static String getName1()
        {
            bool isValid = false;
            String name1 = "";
            while (isValid == false)
            {
                Console.WriteLine("Please enter the first name: ");
                name1 = Console.ReadLine();
                if(name1.All(char.IsLetter) != true || name1== "")
                {
                    Console.WriteLine("Invalid value entered!");
                }
                else
                {
                    isValid = true;
                }
            }
            Console.WriteLine("Name entered successfully \n");
            return name1;
        }

        //Returns the second name
        public static String getName2()
        {
            bool isValid = false;
            String name2 = "";
            while (isValid == false)
            {
                Console.WriteLine("Please enter the second name: ");
                name2 = Console.ReadLine();
                if (name2.All(char.IsLetter) != true || name2 == "")
                {
                    Console.WriteLine("Invalid value entered!");
                }
                else
                {
                    isValid = true;
                }
            }
            Console.WriteLine("Name entered successfully \n");
            return name2;
        }

        //Concatenates the two names and returns the result
        public static String concatNames(String name1, String name2)
        {
            return name1.ToLower() + " matches " + name2.ToLower();
        }

        //Calculate the frequency of each letter and returns the result
        public static String calcFrequency(String combNames)
        {
            int len = combNames.Length;
            int idx = 0;
            char[] seenArr = new char[len];
            String strNum = "";
            char ch;
            bool isSeen;
            for (int i = 0; i < len; i++)
            {
                ch = combNames[i];
                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }
                isSeen = false;
                for (int j = 0; j < idx; j++)
                {
                    if (ch == seenArr[j])
                    {
                        isSeen = true;
                        break;
                    }
                }
                if (isSeen)
                {
                    continue;
                }
                int freq = 1;
                for (int k = i + 1; k < len; k++)
                {
                    if (ch == combNames[k])
                    {
                        freq++;
                    }
                }
                seenArr[idx] = ch;
                strNum = strNum + freq.ToString();
                idx++;
            }

            return strNum;
        }

        //Reduces the frequency result to two digits and returns the result
        public static int reduceDigits(String strFreq)
        {
            int num1 = 0;
            int num2 = 0;
            int sum = 0;
            int len = strFreq.Length;
            int front = 0;
            int back = len - 1;
            int perc = 0;
            String newNum = "";
            char middle;
            bool isTwoDgt = false;
            while (isTwoDgt == false)
            {
                for (int i = 0; i < (int)(len / 2); i++)
                {
                    num1 = Int32.Parse(strFreq[front].ToString());
                    num2 = Int32.Parse(strFreq[back].ToString());
                    front++;
                    back--;
                    sum = num1 + num2;
                    newNum = newNum + sum.ToString();
                }
                if (len % 2 != 0)
                {
                    middle = strFreq[(int)((len - 1) / 2)];
                    newNum = newNum + middle.ToString();
                }
                if (newNum.Length == 2)
                {
                    isTwoDgt = true;
                }
                strFreq = newNum;
                len = strFreq.Length;
                front = 0;
                back = len - 1;
                newNum = "";
            }
            perc = Int32.Parse(strFreq);
            return perc;
        }

        //Generates and returns the output message
        public static String getMessage(String name1, String name2, int perc)
        {
            String msg = "";
            msg = name1 + " matches " + name2 + " " + perc.ToString() + "%";
            if (perc >= 80)
            {
                msg = msg + ", good match";
            }
            return msg;
        }

        public static void Main(String[] args)
        {
            String name1 = "";
            String name2 = "";
            String combNames = "";
            String strFreq = "";
            int perc = 0;
            String msg = "";
            String userInput = "";
            bool isValid = false;

            Console.WriteLine("Welcome to Good Match Application \n");

            while (isValid == false)
            {
                Console.WriteLine("Please Choose An Input Option:");
                Console.WriteLine("1 - Manual Input");
                Console.WriteLine("2 - CSV File Input \n");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        isValid = true;
                        name1 = getName1();
                        name2 = getName2();
                        combNames = concatNames(name1, name2);
                        strFreq = calcFrequency(combNames);
                        perc = reduceDigits(strFreq);
                        msg = getMessage(name1, name2, perc);
                        Console.Write(msg);
                        break;
                    case "2":
                        isValid = true;
                        bool isPathValid = false;

                        while(isPathValid == false)
                        {
                            Console.WriteLine("Please enter the path of your CSV file:");
                            String path = Console.ReadLine();
                            try
                            {
                                using (var reader = new StreamReader(path))
                                {
                                    isPathValid = true;
                                    List<string> nameList = new List<string>();
                                    List<string> genderList = new List<string>();

                                    //populates nameList and genderList
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var value = line.Split(',');

                                        nameList.Add(value[0]);
                                        genderList.Add(value[1]);
                                    }

                                    //removes duplicates
                                    for (int i = 0; i < nameList.Count; i++)
                                    {

                                        for (int j = i + 1; j < nameList.Count; j++)
                                        {
                                            if (nameList[i] == nameList[j] && genderList[i] == genderList[j])
                                            {
                                                nameList.RemoveAt(j);
                                                genderList.RemoveAt(j);
                                            }
                                        }
                                    }

                                    List<string> maleList = new List<string>();
                                    List<string> femaleList = new List<string>();

                                    //populates maleList and femaleList
                                    for (int i = 0; i < nameList.Count; i++)
                                    {
                                        if (genderList[i] == "m")
                                        {
                                            maleList.Add(nameList[i]);
                                        }
                                        else if (genderList[i] == "f")
                                        {
                                            femaleList.Add(nameList[i]);
                                        }
                                    }

                                    try
                                    {

                                        StreamWriter sw = new StreamWriter("Output.txt");

                                        //Matches sets and writes results to text file
                                        for (int i = 0; i < maleList.Count; i++)
                                        {
                                            for (int j = 0; j < femaleList.Count; j++)
                                            {
                                                name1 = maleList[i];
                                                name2 = femaleList[j];
                                                combNames = concatNames(name1, name2);
                                                strFreq = calcFrequency(combNames);
                                                perc = reduceDigits(strFreq);
                                                msg = getMessage(name1, name2, perc);
                                                sw.WriteLine(msg);
                                            }
                                        }
                                        sw.Close();
                                        Console.WriteLine("Content is written successfully to Output.txt");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception: " + ex.Message);                                       
                                    }
                                }
                            }
                            catch (FileNotFoundException fnf)
                            {
                                Console.WriteLine("File not found!");
                                isPathValid = false;
                            }
                        }


                        break;
                    default:
                        Console.WriteLine("Invalid value entered!");
                        break;

                }
            }

                Console.ReadKey();
        }
    }
}

