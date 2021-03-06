﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace Converter
{
    public class MyListOfSensors : List<Sencors>
    {
        public Sencors getSensorByKKSName(string kks)
        {
            foreach (Sencors item in this)
            {
                if (item.KKS_Name == kks)
                {
                    return item;
                }
            }
            return null;
        }
        public Sencors getOneKKSByIndex(int index)
        {
            return this[index];
        }

        private int DetectType(string filename)
        {
            string[] temp = filename.Split('.');
            switch (temp[1])
            {
                case "txt":
                    return 1;
                    break;
                case "sti":
                    return 2;
                    break;
                default:
                    return -1;
                    break;
            }
            return -1;
        }
        public void LoadFromFile(string filename, MyListOfSensors y)
        {
            if (DetectType(filename) != -1)
            {
                switch (DetectType(filename))
                {
                    case 1:

                        this.LoadAPIK(filename, y);
                        break;

                    case 2:
                        this.LoadSTI2(filename, y);
                        break;
                    //редактировать
                 
                    default:
                        break;
                }
            }
        }

        public void LoadSTI1(string filename, MyListOfSensors p)
        {
            string line = "";
            StreamReader myFile = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
            line = myFile.ReadLine();
            List<string> myKksFast = new List<string>();
            myKksFast = line.Split('\t').ToList();
            for (int i = 0; i < myKksFast.Count; i++)
            {
                if (myKksFast[i] == " " || myKksFast[i] == "")
                {
                    myKksFast.RemoveAt(i);
                }
            }

            for (int i = 0; i < myKksFast.Count; i++)
            {
                Sencors myonekks = new Sencors();
                myonekks.KKS_Name = myKksFast[i];
                p.Add(myonekks);
            }
            p.RemoveAt(5);

            while ((line = myFile.ReadLine()) != null)
            {
                List<string> myHelpList = new List<string>();
                List<string> myValues = new List<string>();
                myValues.Clear();
                myHelpList.Clear();
                myHelpList = line.Split('\t').ToList();
                for (int i = 0; i < myHelpList.Count; i++)
                {
                    if (myHelpList[i] == " " || myHelpList[i] == "")
                    {
                        myHelpList.RemoveAt(i);
                    }
                }
                myValues.AddRange(myHelpList);

                for (int i = 0; i < myValues.Count; i++)
                {
                    Record myRec = new Record();
                    myRec.value1 = double.Parse(myValues[0].Replace(".", ",").Trim());
                    DateTime WindowsTime = new DateTime(1970, 1, 1).AddSeconds(myRec.value1);
                    myRec.DateTime = WindowsTime;
                    myRec.Value = double.Parse(myValues[i].Replace(".", ",").Trim());
                    p[i].MyListRecordsForOneKKS.Add(myRec);
                }
            }
            p.RemoveAt(0);
        }

        public void LoadSTI2(string filename, MyListOfSensors p)
        {
            string line = "";
            StreamReader mysr = new StreamReader(filename, Encoding.GetEncoding("UTF-8"));
            MyListOfSensors MyList = new MyListOfSensors();
            MyList.Clear();
            line = mysr.ReadLine();
            string[] strarray1 = { "R1", "R2", "Rc", "J1", "J2", "J3" };

            for (int i = 0; i < strarray1.Length; i++)
            {
                Sencors myonekks = new Sencors();
                myonekks.KKS_Name = strarray1[i];
                MyList.Add(myonekks);
            }

            while ((line = mysr.ReadLine()) != null)
            {
                List<string> helper = new List<string>();
                List<double> helper1 = new List<double>();
               // try
               // {
                    helper = line.Split('\t').ToList();

                    for (int i = 0; i < helper.Count; i++)
                    {
                        helper1.Add(double.Parse(helper[i].Replace('.', ',')));
                    }

                    for (int i = 1; i < helper1.Count; i++)
                    {
                        Record OneRec = new Record();

                        OneRec.DateTime = DateTime.FromOADate(helper1[0]);
                        OneRec.Value = helper1[i];
                        MyList[i - 1].MyListRecordsForOneKKS.Add(OneRec);
                        //            MyList[MyList.Count - N + i - 1].MyListRecordsForOneKKS.Add(OneRec);
                    }
              //  }
             //   catch
            //    {

              //  }
                helper.Clear();
                helper1.Clear();

            }
            for (int i = 3; i < MyList.Count-1; i++)
            {
                p.Add(MyList[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                p.Add(MyList[i]);
            }
            
            mysr.Close();
        }
        public void LoadAPIK(string filename, MyListOfSensors p)
        {
            string line = "";
            StreamReader mysr = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
            MyListOfSensors MyList = new MyListOfSensors();
            MyList.Clear();

            List<string> strarray = new List<string>();
            List<string> strarray1 = new List<string>();
            line = mysr.ReadLine();

            strarray1 = line.Split('\t').ToList();
            strarray1.RemoveAt(0);
            strarray.Add("Время реальное");
            strarray.Add("Время СКУД");
            strarray.AddRange(strarray1);
            strarray.RemoveAt(2);

            int i2 = 0;
            foreach (string item in strarray)
            {
                // i2++;
                if (i2 >= 0)
                {
                    Sencors myonekks = new Sencors();
                    myonekks.KKS_Name = item;
                    MyList.Add(myonekks);
                }
                i2++;

            }
            int N = strarray.Count() - 1;
            double[] mytempdouble = new double[strarray.Count];
            while (line != null)
            {
                line = mysr.ReadLine();
                if (line != null)
                {
                    mytempdouble = line.Replace('.', ',').Split('\t').Select(n => double.Parse(n)).ToArray();
                    //MessageBox.Show(mytempdouble[mytempdouble.Count()-1].ToString());

                    for (int i = 0; i < mytempdouble.Length; i++)
                    {
                        Record OneRec = new Record();

                        OneRec.DateTime = DateTime.FromOADate(mytempdouble[0]);
                        if(i == 53 )
                        { 
                        OneRec.Value = mytempdouble[i] * 32;
                        }
                        else 
                        {
                            OneRec.Value = mytempdouble[i];
                        }

                        MyList[MyList.Count - N + i - 1].MyListRecordsForOneKKS.Add(OneRec);
                    }
                }
            }
           // MessageBox.Show(MyList[53].KKS_Name);
           MyList.RemoveAt(0);
           MyList.RemoveAt(0);
       //     MyList.RemoveAt(MyList);
         //   MyList.RemoveAt(1);
          //  MyList.RemoveAt(2);
            p.AddRange(MyList);
            MyList.Clear();

       

            //Закрытие потока
            mysr.Close();
        }

        public List<double> getХvaluesByIndexStartEnd(int len)
        {
            List<double> temp = new List<double>();

            for (int i = 0; i < len; i++)
            {
                temp.Add((double)i);
            }

            return temp;

        }
        public List<double> getYvaluesByIndexStartEnd(int index, int start, int end)
        {
            List<double> temp = new List<double>();

            for (int i = start; i < end; i++)
            {
                temp.Add(this[index].MyListRecordsForOneKKS[i].Value);
            }

            return temp;

        }
    }

    public class Record
    {
        public DateTime DateTime;
        public double Value;

        public double value1;
        //   public double ValueTimeForDAT;
        public Record()
        {

        }
        public Record(DateTime r, double t)
        {
            this.DateTime = r;
            this.Value = t;
        }
    }

    public class Sencors : IComparable
    {
        int L;
        public string KKS_Name { get; set; }
        public List<Record> MyListRecordsForOneKKS;
        public Sencors()
        {
            this.MyListRecordsForOneKKS = new List<Record>();
            L = this.MyListRecordsForOneKKS.Count;

        }

        public Sencors(string sss)
        {
            Record OneRecord = new Record();
            OneRecord.DateTime = Convert.ToDateTime(sss.Split('\t')[0].Replace('.', '/').Replace(',', '.').Trim());//line.Split('\t')[0]//для вывода с милисекундами
            OneRecord.Value = double.Parse(sss.Split('\t')[2].Replace('.', ',').Replace('-', '0').Trim());//line.Split('\t')[2]//и прочерками, но в стрингах

            this.MyListRecordsForOneKKS = new List<Record>();
            this.KKS_Name = sss.Split('\t')[1];
            this.MyListRecordsForOneKKS.Add(OneRecord);
        }

        //public Sencors(DateTime t , double y, int l)
        //{
        //    this.MyListRecordsForOneKKS[l].Value = y;

        //}
        public int CompareTo(object other)
        {
            var oth = other as Sencors;
            return this.KKS_Name.CompareTo(oth.KKS_Name);
        }
    }
}
