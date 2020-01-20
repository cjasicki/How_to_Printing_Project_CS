using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example7_CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //*****
        //** Global Variable
        //*****
        DataTable dtNewData = null;
        Form frmMsg = new Form();

        //*****
        //** Form
        //*****
        //*******************************************************
        //** Procedure: Form1_FormClosing()
        //**   Disposes of datatable
        //*******************************************************
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Exit Yes or No?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                TimedWindow();
                Int32 intPause = 0;
                while (intPause < 100000000)
                {
                    intPause += 1;
                }
            }
            if (dtNewData != null)
            {
                dtNewData.Dispose();
            }
        }

        private void TimedWindow()
        {
            TextBox txtBox = new TextBox(); //Programmatically create textbox control
            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 3000;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
            //Create the from
            frmMsg.Width = 500;
            frmMsg.Height = 300;
            frmMsg.Text = "Goodbye";//title of the from
            //create control to add to from
            txtBox.Text = "Goodbye";
            txtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtBox.Font = new Font("Tahoma", 35);
            txtBox.Width = 400;
            txtBox.Height = 250;
            txtBox.BackColor = System.Drawing.Color.Gainsboro;
            frmMsg.BackColor = System.Drawing.Color.Gainsboro;
            frmMsg.Controls.Add(txtBox);
            frmMsg.Show();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            frmMsg.Close();
        }
    
        //*******************************************************
        //** Procedure: Form1_Load()
        //**   Initializes data entry datatable
        //*******************************************************
        private void Form1_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            CreateDataTable();
        }

        //*****
        //** Procedure
        //***** 
        //*******************************************************
        //** Procedure: CreateDataTable()
        //**   Creates an empty datatable for data entry
        //*******************************************************
        private void CreateDataTable()
        {
            lblError.Text = "";

            //** Remove any existing datatable
            if (dtNewData != null)
            {
                dtNewData.Dispose();
            }

            dtNewData = new DataTable();
            dtNewData.TableName = "NewDataEntry";
            dtNewData.Columns.Add("GridCol1", typeof(string));
            dtNewData.Columns.Add("GridCol2", typeof(string));
            dtNewData.Columns.Add("GridCol3", typeof(string));
            dtNewData.Columns.Add("GridCalcCol", typeof(string));

            dgvDisplay.DataSource = dtNewData;

            // Size columns and set other formatting for DataGridView
            dgvDisplay.AllowUserToAddRows = false;
            dgvDisplay.AllowUserToDeleteRows = false;
            dgvDisplay.AllowUserToOrderColumns = false;
            dgvDisplay.Columns[0].Width = 200;
            dgvDisplay.Columns[1].Width = 125;
            dgvDisplay.Columns[2].Width = 125;
            dgvDisplay.Columns[3].Width = 150;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pdPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font fDoc;
            Single sglXPos;
            Single sglYPos;
            Int32 intRow;
            Decimal decTotalValue;

            fDoc = new Font("Arial", 12);
            e.Graphics.DrawString("Title of Page",fDoc, System.Drawing.Brushes.Black, Convert.ToSingle(250.0),Convert.ToSingle(75.0));

            sglYPos = Convert.ToSingle(125);
            e.Graphics.DrawString("GridCol1", fDoc, System.Drawing.Brushes.Black, Convert.ToSingle(30.0), sglYPos);
            e.Graphics.DrawString("GridCol2", fDoc, System.Drawing.Brushes.Black, Convert.ToSingle(200.0), sglYPos);
            e.Graphics.DrawString("GridCol3", fDoc, System.Drawing.Brushes.Black, Convert.ToSingle(400.0), sglYPos);
            e.Graphics.DrawString("GridCalcCol", fDoc, System.Drawing.Brushes.Black, Convert.ToSingle(600.0), sglYPos);
            decTotalValue = Convert.ToDecimal(0.0);
            for (intRow = 0; intRow < dtNewData.Rows.Count; intRow++)
            {
                sglXPos = Convert.ToSingle(30);
                sglYPos += Convert.ToSingle(fDoc.Height);
                e.Graphics.DrawString(dtNewData.Rows[intRow]["GridCol1"].ToString(), fDoc, System.Drawing.Brushes.Black, sglXPos, sglYPos);

                sglXPos = Convert.ToSingle(200);
                e.Graphics.DrawString(dtNewData.Rows[intRow]["GridCol2"].ToString(), fDoc, System.Drawing.Brushes.Black, sglXPos, sglYPos);

                sglXPos = Convert.ToSingle(400);
                e.Graphics.DrawString(dtNewData.Rows[intRow]["GridCol3"].ToString(), fDoc, System.Drawing.Brushes.Black, sglXPos, sglYPos);

                sglXPos = Convert.ToSingle(600);
                e.Graphics.DrawString(dtNewData.Rows[intRow]["GridCalcCol"].ToString(), fDoc, System.Drawing.Brushes.Black, sglXPos, sglYPos);
                decTotalValue += Convert.ToDecimal(dtNewData.Rows[intRow]["GridCalcCol"]);

            }
            sglYPos += (Convert.ToSingle(fDoc.Height * 2));
            e.Graphics.DrawString("Total Value:" + decTotalValue.ToString("c"), fDoc, System.Drawing.Brushes.Black, Convert.ToSingle(50.0),sglYPos);
        }
        // procedure: btnAdd_Click()
        // add data to displayed grid
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //Boolean blnlsOK = true;
            Decimal decValue = 0m;

            lblError.Text = "";
            Decimal intx;
            // Verify 3rd field is numeric
            if (Decimal.TryParse(txtThird.Text, out decValue))
            {
                dtNewData.Rows.Add(txtFirst.Text, txtSecond.Text, txtThird.Text, (25.0M * decValue).ToString("N2"));
                txtFirst.Text = "";
                txtSecond.Text = "";
                txtThird.Text = "";
            }
            else
            {
                lblError.Text = "Column 3 must be a numeric value - ";
                //blnlsOK = false;
            }
        }

        // Procedure: btnPrint_Click()
        // print displayed grid
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DialogResult dlgAnswer;
            lblError.Text = "";

            dlgAnswer = pdlgData.ShowDialog();
            if (dlgAnswer == DialogResult.OK)
            {
                pdPrint.PrinterSettings = pdlgData.PrinterSettings;

                // Call BeginPrint here to print cover page, ext...
                // Print a single page. Normally will be a while loop/
                pdPrint.Print();

                // Call EndPrint here to print summary page
            }
        }
    }
}
