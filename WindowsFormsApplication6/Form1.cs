using Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        private System.Collections.ArrayList customers = new System.Collections.ArrayList();
        private MyVirtualClass customerInEdit;
        private int rowInEdit = -1;
        private bool rowScopeCommit = true;
        public Form1()
        {
            InitializeComponent();
        }

        public MyListOfSensors MyAllSensorsAPIK = new MyListOfSensors();
        public MyListOfSensors MyAllSensorsFASTER = new MyListOfSensors();

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                MyAllSensorsAPIK.LoadFromFile(openFileDialog1.FileName, MyAllSensorsAPIK);
            }
            label1.Text = openFileDialog1.FileName;
            for (int i = 0; i <MyAllSensorsAPIK.Count; i++)
            {
                checkedListBox1.Items.Add(MyAllSensorsAPIK[i].KKS_Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {

                MyAllSensorsAPIK.LoadFromFile(openFileDialog2.FileName, MyAllSensorsFASTER);
            }
            label2.Text = openFileDialog2.FileName;
            for (int i = 0; i < MyAllSensorsFASTER.Count; i++)
            {
                checkedListBox2.Items.Add(MyAllSensorsFASTER[i].KKS_Name);
            }
        }

        List<int> Indexes = new List<int>();

        public MyListOfSensors MyAllSensorsAPIKnew = new MyListOfSensors();
        MyListOfSensors EndCollection = new MyListOfSensors();

        int[] MyConstIndexParametr = { 57, 79, 74, 229, 99, 98, 97, 238, 234, 48, 50, 60, 51, 56, 240 };

        private int n1(int index)
        {
            int p = 0;
            for (int i = index; i < MyAllSensorsFASTER[0].MyListRecordsForOneKKS.Count; i++)
            {
                for (int j = 0; j < MyAllSensorsAPIK[0].MyListRecordsForOneKKS.Count; j++)
                {
                    if (MyAllSensorsFASTER[0].MyListRecordsForOneKKS[i].DateTime.ToString() == MyAllSensorsAPIK[0].MyListRecordsForOneKKS[j].DateTime.ToString())
                    {
                        p = j;
                        break;
                    }
                }
            }
            return p;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int indexBEGIN = 0;
                if (MyAllSensorsFASTER[0].MyListRecordsForOneKKS[0].DateTime.ToOADate() > MyAllSensorsAPIK[0].MyListRecordsForOneKKS[0].DateTime.ToOADate())
                {
                   // MessageBox.Show("Привет");
                    for (int j = 0; j < MyAllSensorsAPIK[0].MyListRecordsForOneKKS.Count; j++)
                    {
                    // MessageBox.Show("Привет");
                        if (MyAllSensorsFASTER[0].MyListRecordsForOneKKS[0].DateTime.ToString() == MyAllSensorsAPIK[0].MyListRecordsForOneKKS[j].DateTime.ToString())
                        {
                            indexBEGIN = j;
                            break;
                        }
                    }
                }
            //    MessageBox.Show(indexBEGIN.ToString());
                if (MyAllSensorsFASTER[0].MyListRecordsForOneKKS[0].DateTime.ToOADate() < MyAllSensorsAPIK[0].MyListRecordsForOneKKS[0].DateTime.ToOADate())
                {
                 //   MessageBox.Show("Пока");
                    for (int j = 0; j < MyAllSensorsFASTER[0].MyListRecordsForOneKKS.Count; j++)
                    {
                        if (MyAllSensorsAPIK[0].MyListRecordsForOneKKS[0].DateTime.ToString() == MyAllSensorsFASTER[0].MyListRecordsForOneKKS[j].DateTime.ToString())
                        {
                            indexBEGIN = j;
                            break;
                        }
                    }
                }
                int p = 0;
                MyListOfSensors ALLparametrs = new MyListOfSensors();

                ALLparametrs.AddRange(MyAllSensorsFASTER);
                for (int i = 0; i < MyConstIndexParametr.Length; i++)
                {
                    ALLparametrs.Add(MyAllSensorsAPIK[MyConstIndexParametr[i]]);
                }

                StreamWriter MyFasterFile = new StreamWriter(saveFileDialog1.FileName + ".dat", true, Encoding.GetEncoding("Windows-1251"));
                MyFasterFile.Write("Время" + "\t");
                for (int i = 0; i < ALLparametrs.Count; i++)
                {
                    MyFasterFile.Write(ALLparametrs[i].KKS_Name + "\t");
                }
                MyFasterFile.WriteLine();
                //   int j = 0;
                double unixtime = 0;
                double unixtime1 = 0;
                int myindex = indexBEGIN;
                //   int indexAPIK = 0;
                if (MyAllSensorsFASTER[0].MyListRecordsForOneKKS[0].DateTime.ToOADate() < MyAllSensorsAPIK[0].MyListRecordsForOneKKS[0].DateTime.ToOADate())
                {
                    MessageBox.Show("Воу!");
                    indexBEGIN = 0;
                }

                try
                {
                    for (int j = myindex; j < ALLparametrs[0].MyListRecordsForOneKKS.Count; j++)
                    {
                        int Raznica = 0;
                        unixtime = ((ALLparametrs[0].MyListRecordsForOneKKS[j].DateTime) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
                        if (j != myindex)
                        {
                            unixtime1 = ((ALLparametrs[0].MyListRecordsForOneKKS[j - 1].DateTime) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
                            Raznica = (int)unixtime - (int)unixtime1;
                        }

                        if (Raznica == 1)
                        {
                            indexBEGIN++;
                        }
                        //      unixtime1 = ((ALLparametrs[0].MyListRecordsForOneKKS[j-1].DateTime) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;    
                        //   double MyINT = ((ALLparametrs[0].MyListRecordsForOneKKS[indexBEGIN].DateTime) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
                      //  MessageBox.Show(unixtime.ToString());
                        MyFasterFile.Write((unixtime - 1461609276).ToString().Replace(',', '.') + "\t");
                        var ii = 0;
                        //MessageBox.Show(((int)unixtime - (int)unixtime1).ToString());
                        for (int i = 0; i < ALLparametrs.Count; i++)
                        {
                            if (i < 6)
                            {
                                MyFasterFile.Write(ALLparametrs[i].MyListRecordsForOneKKS[j].Value.ToString().Replace(',', '.') + "\t");
                            }
                            if (i >= 6)
                            {

                                MyFasterFile.Write(ALLparametrs[i].MyListRecordsForOneKKS[indexBEGIN].Value.ToString().Replace(',', '.') + "\t");

                                //if (j == indexBEGIN)
                                //{
                                //    MyFasterFile.Write(ALLparametrs[i].MyListRecordsForOneKKS[indexBEGIN].Value + "\t");
                                //}
                                //if (j != indexBEGIN && ((int)unixtime - (int)unixtime1 != 1))
                                //{
                                //    if (ii==0)
                                //    {
                                //        MyFasterFile.Write(ALLparametrs[i].MyListRecordsForOneKKS[indexBEGIN].Value + "\t");
                                //    }
                                //    if (ii != 0)
                                //    {
                                //        MyFasterFile.Write(ALLparametrs[i].MyListRecordsForOneKKS[indexBEGIN+ii].Value + "\t");
                                //    }
                                //}
                                //if (j != indexBEGIN && ((int)unixtime - (int)unixtime1 == 1))
                                //{
                                //    ii = 1;
                                //    MyFasterFile.Write(ALLparametrs[i].MyListRecordsForOneKKS[indexBEGIN+ii].Value + "\t");
                                //}        
                                if (i == ALLparametrs.Count - 1)
                                {
                                    MyFasterFile.WriteLine();
                                }
                            }
                        }
                        if (j != indexBEGIN && ((int)unixtime - (int)unixtime1 != 1))
                        {
                            ii++;
                        }

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MyFasterFile.Close();
            }
        //    double unixtime1 = ((MyAllSensorsAPIK[0].MyListRecordsForOneKKS[MyAllSensorsAPIK[0].MyListRecordsForOneKKS.Count - 1].DateTime) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;

        //    MessageBox.Show(unixtime.ToString() + " " + unixtime1.ToString());
                //for (int k = 0; k < MyConstIndexParametr.Length; k++)
                //{
                //    Sencors myonekks = new Sencors();
                //    myonekks.KKS_Name = MyAllSensorsAPIK[MyConstIndexParametr[k]].KKS_Name;
                //    //   MessageBox.Show(MyAllSensorsAPIK[Indexes[k]].KKS_Name.ToString());


                //    for (int i = 0; i < MyAllSensorsFASTER[0].MyListRecordsForOneKKS.Count; i++)
                //    {
                //        Record OneRec = new Record();

                //        for (int j = 0; j < MyAllSensorsAPIK[0].MyListRecordsForOneKKS.Count; j++)
                //        {
                //            if (MyAllSensorsFASTER[0].MyListRecordsForOneKKS[i].DateTime.ToString() == MyAllSensorsAPIK[MyConstIndexParametr[k]].MyListRecordsForOneKKS[j].DateTime.ToString())
                //            {
                //              //  MessageBox.Show(MyAllSensorsFASTER[0].MyListRecordsForOneKKS[i].DateTime.ToString() +" " + MyAllSensorsAPIK[MyConstIndexParametr[k]].MyListRecordsForOneKKS[j].DateTime.ToString());
                //                OneRec.DateTime = MyAllSensorsFASTER[0].MyListRecordsForOneKKS[i].DateTime;
                //                OneRec.Value = MyAllSensorsAPIK[MyConstIndexParametr[k]].MyListRecordsForOneKKS[j].Value;
                //                myonekks.MyListRecordsForOneKKS.Add(OneRec);
                //                break;
                //            }
                //      //      y.WriteLine(j.ToString());
                //        }
                //      //  y.WriteLine("1");
                //    }
                //    MyAllSensorsAPIKnew.Add(myonekks);
                // //   y.WriteLine(MyConstIndexParametr.ToString());
                //    //        MessageBox.Show(myonekks.MyListRecordsForOneKKS.Count.ToString());
                //    //    MessageBox.Show(MyAllSensorsAPIKnew[0].MyListRecordsForOneKKS.Count.ToString());
                //}

                ////    MyAllSensorsAPIKnewa

                ////   MyListOfSensors EndCollection = new MyListOfSensors();
                //EndCollection.AddRange(MyAllSensorsFASTER);
                ////        EndCollection.AddRange(MyAllSensorsAPIKnew);

                ////  EndCollection.RemoveAt(2);
                ////      EndCollection.RemoveAt(4);
                //EndCollection.Insert(0, EndCollection[3]);
                //EndCollection.Insert(1, EndCollection[5]);
                //EndCollection.Insert(2, EndCollection[7]);

                //EndCollection.RemoveAt(EndCollection.Count - 1);
                //EndCollection.RemoveAt(EndCollection.Count - 1);
                //EndCollection.RemoveAt(EndCollection.Count - 1);


                //EndCollection[0].KKS_Name = "J1";
                //EndCollection[1].KKS_Name = "J2";
                //EndCollection[2].KKS_Name = "J3";
                //EndCollection[3].KKS_Name = "R1";
                //EndCollection[4].KKS_Name = "R2";
                //EndCollection[5].KKS_Name = "R3";
                //EndCollection.AddRange(MyAllSensorsAPIKnew);
                //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                //    StreamWriter MyConvertFile = new StreamWriter(saveFileDialog1.FileName + ".dat", true, Encoding.GetEncoding("Windows-1251"));
                //    MyConvertFile.Write("Время" + "\t");
                //    for (int i = 0; i < EndCollection.Count; i++)
                //    {
                //        MyConvertFile.Write(EndCollection[i].KKS_Name + "\t");

                //      //  MessageBox.Show(EndCollection[i].KKS_Name.ToString());
                //    }

                //    MyConvertFile.WriteLine();



                //    for (int i = 0; i < EndCollection[EndCollection.Count - 1].MyListRecordsForOneKKS.Count; i++)
                //    {


                //        double t = EndCollection[0].MyListRecordsForOneKKS[i].DateTime.ToOADate();
                //        DateTime p = DateTime.FromOADate(t);

                //        double unixtime = (Convert.ToDateTime(p.ToString("dd.MM.yy HH:mm:ss.fff")) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;



                //        MyConvertFile.Write((unixtime).ToString().Replace(',', '.'));
                //        for (int j = 0; j < EndCollection.Count; j++)
                //        {
                //            MyConvertFile.Write("\t" + EndCollection[j].MyListRecordsForOneKKS[i].Value.ToString().Replace(',', '.'));
                //            // MyConvertFile.WriteLine();
                //        }
                //        MyConvertFile.WriteLine();
                //    }

                ////    MyConvertFile.Close();


                //    //  MessageBox.Show(MyAllSensorsAPIKnew.Count.ToString());
                //}
            
        
        }
        private void t()
        {
            for (int i = 0; i < MyAllSensorsAPIK.Count; i++)
            {
                if(checkedListBox1.Text == MyAllSensorsAPIK[i].KKS_Name)
                {
                    MessageBox.Show(i.ToString());
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "Файл АПИК";
            tabPage2.Text = "Файл FASTER";
            tabPage3.Text = "Параметры";
            tabPage4.Text = "Данные";
            tabPage5.Text = "Параметры";
            tabPage6.Text = "Данные";

            this.dataGridView1.VirtualMode = true;
            this.dataGridView2.VirtualMode = true;

            DataGridViewTextBoxColumn companyNameColumn = new DataGridViewTextBoxColumn();
            companyNameColumn.HeaderText = "Время";
            companyNameColumn.Name = "Время";
            this.dataGridView1.Columns.Add(companyNameColumn);

            DataGridViewTextBoxColumn companyNameColumn1 = new DataGridViewTextBoxColumn();
            companyNameColumn1.HeaderText = "Значение";
            companyNameColumn1.Name = "Значение";
            this.dataGridView1.Columns.Add(companyNameColumn1);

            DataGridViewTextBoxColumn companyNameColumn2 = new DataGridViewTextBoxColumn();
            companyNameColumn2.HeaderText = "Время";
            companyNameColumn2.Name = "Время";
            this.dataGridView2.Columns.Add(companyNameColumn2);

            DataGridViewTextBoxColumn companyNameColumn3 = new DataGridViewTextBoxColumn();
            companyNameColumn3.HeaderText = "Значение";
            companyNameColumn3.Name = "Значение";
            this.dataGridView2.Columns.Add(companyNameColumn3);

            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 200;

            dataGridView2.Columns[0].Width = 200;
            dataGridView2.Columns[1].Width = 200;
        }

        private void dataGridView2_CellValueNeeded(object sender,
     System.Windows.Forms.DataGridViewCellValueEventArgs e)
        {
            //this.dataGridView1.RowCount = 1;
            // If this is the row for new records, no values are needed.
              if (e.RowIndex == this.dataGridView2.RowCount - 1) return;
            //   if (e.RowIndex == this.dataGridView3.RowCount - 1) return;

            MyVirtualClass customerTmp = null;

            // Store a reference to the Customer object for the row being painted.
            if (e.RowIndex == rowInEdit)
            {
                customerTmp = this.customerInEdit;
            }
            else
            {
                customerTmp = (MyVirtualClass)this.customers[e.RowIndex];
            }

            // Set the cell value to paint using the Customer object retrieved.
            switch (this.dataGridView2.Columns[e.ColumnIndex].Name)
            {
                case "Время":
                    e.Value = customerTmp.time;
                    break;

                case "Значение":
                    e.Value = customerTmp.value;
                    break;
            }
        }
        private void dataGridView1_CellValueNeeded(object sender,
        System.Windows.Forms.DataGridViewCellValueEventArgs e)
        {
            //this.dataGridView1.RowCount = 1;
            // If this is the row for new records, no values are needed.
            if (e.RowIndex == this.dataGridView1.RowCount - 1) return;
            //   if (e.RowIndex == this.dataGridView3.RowCount - 1) return;

            MyVirtualClass customerTmp = null;

            // Store a reference to the Customer object for the row being painted.
            if (e.RowIndex == rowInEdit)
            {
                customerTmp = this.customerInEdit;
            }
            else
            {
                customerTmp = (MyVirtualClass)this.customers[e.RowIndex];
            }

            // Set the cell value to paint using the Customer object retrieved.
            switch (this.dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "Время":
                    e.Value = customerTmp.time;
                    break;

                case "Значение":
                    e.Value = customerTmp.value;
                    break;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Visible = false;
                Sencors myOneKKS = new Sencors();
                myOneKKS = MyAllSensorsAPIK.getOneKKSByIndex(checkedListBox1.SelectedIndex);
                this.dataGridView1.CellValueNeeded += new
                    DataGridViewCellValueEventHandler(dataGridView1_CellValueNeeded);

                dataGridView1.Rows.Clear();
                customers.Clear();

                for (int i = 0; i < myOneKKS.MyListRecordsForOneKKS.Count; i++)
                {
                    this.customers.Add(new MyVirtualClass(myOneKKS.MyListRecordsForOneKKS[i].DateTime.ToString("dd.MM.yy HH:mm:ss.fff"), myOneKKS.MyListRecordsForOneKKS[i].Value.ToString()));

                }
                if (this.dataGridView1.RowCount == 0)
                {
                    this.dataGridView1.RowCount = 1;

                }

                this.dataGridView1.RowCount = myOneKKS.MyListRecordsForOneKKS.Count;
                dataGridView1.Visible = true;
            }
            catch (Exception ex0)
            {
                MessageBox.Show(ex0.Message);
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Visible = false;
                Sencors myOneKKS = new Sencors();
                myOneKKS = MyAllSensorsFASTER.getOneKKSByIndex(checkedListBox2.SelectedIndex);
                this.dataGridView2.CellValueNeeded += new
                    DataGridViewCellValueEventHandler(dataGridView2_CellValueNeeded);

                dataGridView1.Rows.Clear();
                customers.Clear();

                for (int i = 0; i < myOneKKS.MyListRecordsForOneKKS.Count; i++)
                {
                    this.customers.Add(new MyVirtualClass(myOneKKS.MyListRecordsForOneKKS[i].DateTime.ToString("dd.MM.yy HH:mm:ss.fff"), myOneKKS.MyListRecordsForOneKKS[i].Value.ToString()));

                }
                if (this.dataGridView2.RowCount == 0)
                {
                    this.dataGridView2.RowCount = 1;

                }

                this.dataGridView2.RowCount = myOneKKS.MyListRecordsForOneKKS.Count;
                dataGridView2.Visible = true;
            }
            catch (Exception ex0)
            {
                MessageBox.Show(ex0.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            t();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            MyAllSensorsAPIK.Clear();
            MyAllSensorsAPIKnew.Clear();
            MyAllSensorsFASTER.Clear();
            EndCollection.Clear();
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            label1.Text = "";
            label2.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

    }
}
