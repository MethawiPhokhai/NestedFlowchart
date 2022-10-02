using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using System.Configuration;
using System.Diagnostics;

namespace NestedFlowchart
{
    public partial class Form1 : Form
    {
        List<XMLCellNode> sortedFlowcharts = new List<XMLCellNode>();
        public Form1()
        {
            InitializeComponent();
        }

        private void lnk_CPNTools_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //TODO: Cannot open CPN Tools link
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

        #endregion Close Button

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
            try
            {
                Transfrom transfrom = new();

                //Read XML Flowchart
                var allFlowChartElements = transfrom.Extracttion(this.txt_InputPath);

                //Sorted Flowchart Element
                //input : allFlowchartElement
                //output : sortedFlowchart
                //Iterate until nextElement is null

                List<XMLCellNode> tempDecisionElements = new List<XMLCellNode>();

                var startElement = allFlowChartElements.Find(x => x.ValueText.ToLower() == "start");
                sortedFlowcharts.Add(startElement);

                XMLCellNode? nextElement = transfrom.SortedFlowchartElement(allFlowChartElements, sortedFlowcharts, tempDecisionElements, startElement);
                while (nextElement != null)
                {
                    nextElement = transfrom.SortedFlowchartElement(allFlowChartElements, sortedFlowcharts, tempDecisionElements, nextElement);
                }
                sortedFlowcharts.Remove(null);

                //Add column NodeType
                foreach (var s in sortedFlowcharts)
                {
                    s.NodeType = transfrom.CheckFCNodeType(s);
                }

                //Bind to datasource table after formatted
                dg_ElementPreview.DataSource = sortedFlowcharts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("2. Transform Error : " + ex.Message, "Error");
            }
        }

        private void btn_ExportToCPN_Click(object sender, EventArgs e)
        {
            try
            {
                var TemplatePath = ConfigurationManager.AppSettings["TemplatePath"];
                var ResultPath = ConfigurationManager.AppSettings["ResultPath"];

                //Write Result Path
                txt_ReultPath.Text = ResultPath;

                //Export to CPN Tools file
                var exportToCPN = new ExportToCPN();
                exportToCPN.ExportFile(TemplatePath, ResultPath, sortedFlowcharts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("3. Export to CPN Error : " + ex.Message, "Error");
            }
        }
    }
}