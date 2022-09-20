using NestedFlowchart.Functions;
using System.Diagnostics;

namespace NestedFlowchart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lnk_CPNTools_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://google.com");
        }



        #region Close Button
        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_close_MouseHover(object sender, EventArgs e)
        {
            btn_close.ForeColor = Color.Black;
        }

        private void btn_close_MouseLeave(object sender, EventArgs e)
        {
            btn_close.ForeColor = SystemColors.ControlDarkDark;
        }

        #endregion


        private void btn_ImportXML_Click(object sender, EventArgs e)
        {
            ImportXml importXml = new ImportXml();
            OpenFileDialog openFileDialog1 = importXml.LoadXMLFile();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                importXml.WriteInputPath(this.txt_InputPath, openFileDialog1);

                importXml.WriteInputPreview(this.txt_InputPath, this.txt_InputPreview);
            }
        }

        private void btn_Transfrom_Click(object sender, EventArgs e)
        {
            Transfrom transfrom = new Transfrom();
            var allFlowChartElement = transfrom.Extracttion(this.txt_InputPath);

            transfrom.BindExtractTable(this.dg_ElementPreview, allFlowChartElement);

            var routerTables = transfrom.BindRouteTable(this.dg_RouteTable, allFlowChartElement);

            //Write Result Path

            //Create CPN File

        }
    }
}