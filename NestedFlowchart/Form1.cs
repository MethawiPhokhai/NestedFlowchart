using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Rules;
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
            Process.Start(new ProcessStartInfo { FileName = @"https://cpntools.org/", UseShellExecute = true });
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

                #region Read XML Flowchart
                var allFlowChartElements = transfrom.Extracttion(this.txt_InputPath);
                #endregion

                #region Sorted by finding the nextid of each element

                //Create Temp variable for store another direction of decision node
                List<XMLCellNode> tempDecisionElements = new List<XMLCellNode>();

                //Find a start node
                var startElement = allFlowChartElements.Find(x => x.ValueText.ToLower() == "start");
                sortedFlowcharts.Add(startElement);

                //Finding next id after start node (Initialization process)
                XMLCellNode? nextElement = transfrom.SortedFlowchartElement(allFlowChartElements, sortedFlowcharts, tempDecisionElements, startElement);
                while (nextElement != null)
                {
                    //Find next node
                    nextElement = transfrom.SortedFlowchartElement(allFlowChartElements, sortedFlowcharts, tempDecisionElements, nextElement);
                }

                //Removing the null the end of the node
                sortedFlowcharts.Remove(null);
                #endregion

                #region Add column NodeType
                foreach (var s in sortedFlowcharts)
                {
                    s.NodeType = transfrom.CheckFCNodeType(s);
                }
                #endregion

                #region Bind to datasource table after formatted
                dg_ElementPreview.DataSource = sortedFlowcharts;
                #endregion
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
                #region Read all template
                var TemplatePath = ConfigurationManager.AppSettings["TemplatePath"];
                var ResultPath = ConfigurationManager.AppSettings["ResultPath"];
                #endregion

                #region Write Result Path
                txt_ReultPath.Text = ResultPath;
                #endregion

                #region Export to CPN Tools file
                Rule1 rule1 = new Rule1();
                Rule2 rule2 = new Rule2();
                Rule3 rule3 = new Rule3();
                Rule4 rule4 = new Rule4();
                Rule5 rule5 = new Rule5();
                Rule6 rule6 = new Rule6();
                Rule7 rule7 = new Rule7();
                TransformationApproach approach = new TransformationApproach();

                var exportToCPN = new ExportToCPN(rule1, rule2, rule3, rule4, rule5, rule6, rule7, approach);
                exportToCPN.ExportFile(TemplatePath, ResultPath, sortedFlowcharts);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("3. Export to CPN Error : " + ex.Message, "Error");
            }
        }
    }
}