using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using System.Diagnostics;
using System.Linq;

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
            try
            {
                Transfrom transfrom = new();

                //Read XML Flowchart
                var allFlowChartElements = transfrom.Extracttion(this.txt_InputPath);

                //Sorted Flowchart Element
                //input : allFlowchartElement
                //output : sortedFlowchart
                //Iterate until nextElement is null
                List<XMLCellNode> sortedFlowcharts = new List<XMLCellNode>();
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
                foreach(var s in sortedFlowcharts)
                {
                    s.NodeType = transfrom.CheckFCNodeType(s);
                }

                //Bind to datasource table after formatted
                dg_ElementPreview.DataSource = sortedFlowcharts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("2. Transform Error : ", ex.Message);
            }

        }

        private void btn_ExportToCPN_Click(object sender, EventArgs e)
        {
            try
            {
                //Write Result Path
                txt_ReultPath.Text = @"D://NestedFlowchartToCPNResult";

                //Create CPN File
                string emptyCPNTemplate = File.ReadAllText(@"G:\My Drive\CU\MasterProject\Project\Git\NestedFlowchart\EmptyCPNTools\EmptyNet.cpn");





                //Insert each CPN Node into empty template
                //Start

                var startPlace = File.ReadAllText(@"G:\My Drive\CU\MasterProject\Project\Git\NestedFlowchart\Templates\StartPlace.txt");


                var spit = emptyCPNTemplate.Split("<pageattr name=\"New Page\"/>");
                spit[0] = string.Concat(spit[0], "<pageattr name=\"New Page\"/>");

                spit[0] = string.Concat(spit[0], startPlace);

                string firstCPN = spit[0] + spit[1];

                //Write to CPN File
                File.WriteAllText(@"G:\My Drive\CU\MasterProject\Project\Git\NestedFlowchart\Result\11.cpn", firstCPN);


            }
            catch (Exception ex)
            {
                MessageBox.Show("3. Export to CPN Error : ", ex.Message);
            }

        }
    }
}