using NestedFlowchart.Models;
using System.Xml;

namespace NestedFlowchart.Functions
{
    public class Transfrom
    {
        public List<XMLCellNode> Extracttion(TextBox txtInputPath)
        {
            List<XMLCellNode> xmlCellNode = new List<XMLCellNode>();

            try
            {
                XmlDocument xmlFlowChart = new XmlDocument();
                xmlFlowChart.Load(txtInputPath.Text);

                var mgr = new XmlNamespaceManager(xmlFlowChart.NameTable);
                mgr.AddNamespace("a", "");

                var mxcellCollection = xmlFlowChart.SelectNodes("//a:diagram /a:mxGraphModel /a:root /a:mxCell", mgr);

                foreach (XmlElement xml in mxcellCollection)
                {
                    xmlCellNode.Add(new XMLCellNode
                    {
                        ID = xml.GetAttribute("id"),
                        ValueText = xml.HasAttribute("value") ? xml.Attributes["value"].Value : string.Empty,
                        Parent = xml.HasAttribute("parent") ? xml.Attributes["parent"].Value : string.Empty,
                        Source = xml.HasAttribute("source") ? xml.Attributes["source"].Value : string.Empty,
                        Target = xml.HasAttribute("target") ? xml.Attributes["target"].Value : string.Empty,
                        Style = xml.HasAttribute("style") ? xml.Attributes["style"].Value : string.Empty
                    });
                }

                return xmlCellNode;

            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error {0} from {1} method", ex.Message, "Extracttion()"));
            }
        }

        public void BindExtractTable(DataGridView dg_ElementPreview, List<XMLCellNode> allFlowChartElement)
        {
            dg_ElementPreview.DataSource = allFlowChartElement;
        }

        public List<RouterTable> BindRouteTable(DataGridView dg_RouteTable, List<XMLCellNode> allFlowChartElement)
        {

            List<XMLCellNode> findRouteTable = allFlowChartElement.FindAll(a => a.Style.StartsWith("endArrow=classic") || a.Style.StartsWith("edgeStyle=orthogonalEdgeStyle"));

            List<RouterTable> routeTables = new List<RouterTable>();
            int RowNumer = 0;
            //foreach (var item in findRouteTable)
            //{
            //    RowNumer = RowNumer + 1;
            //    routeTables.Add(new RouterTable
            //    {
            //        RowNumber = RowNumer,
            //        ArrowID = item.ID,
            //        ArrowText = item.ValueText,
            //        SourceID = item.Source,
            //        SourceType = CheckFCNodeType(allFlowChartElement.FirstOrDefault(a => a.ID == item.Source)),
            //        SourceText = allFlowChartElement.FirstOrDefault(a => a.ID == item.Source).ValueText,
            //        TargetID = item.Target,
            //        TargetType = CheckFCNodeType(allFlowChartElement.FirstOrDefault(a => a.ID == item.Target)),
            //        TargetText = allFlowChartElement.FirstOrDefault(a => a.ID == item.Target).ValueText
            //    });
            //}

            //เกิดปัญหา วนตาม Loop ไปเรื่อย ๆ
            //routeTables = ReorderToEqualRealFlowchart(routeTables);

            /* ลองสร้าง object แบบ Add ด้วยมือมาก่อน แล้วลองเอา Pattern ที่คิดจับ
             * ว่าสามารถรู้ได้ไหม ว่านี้คือ Loop
             * ต้องลองหลาย ๆ Pattern เช่น For if while
             */

            //For Pattern
            //routeTables.Add(new RouterTable {
            //    RowNumber = 1,
            //    ArrowID = "1",
            //    ArrowText = "Arrowtext",
            //    SourceID = "1",
            //    SourceType = "Process",
            //    SourceText = "x = 0",
            //    TargetID = "2",
            //    TargetType = "condition",
            //    TargetText = "x <= 5"
            //});

            //routeTables.Add(new RouterTable
            //{
            //    RowNumber = 2,
            //    ArrowID = "2",
            //    ArrowText = "Arrowtext",
            //    SourceID = "2",
            //    SourceType = "condition",
            //    SourceText = "x <= 5",
            //    TargetID = "3",
            //    TargetType = "process",
            //    TargetText = "do something"
            //});

            //routeTables.Add(new RouterTable
            //{
            //    RowNumber = 3,
            //    ArrowID = "3",
            //    ArrowText = "Arrowtext",
            //    SourceID = "3",
            //    SourceType = "process",
            //    SourceText = "do something",
            //    TargetID = "4",
            //    TargetType = "process",
            //    TargetText = "x = x+1"
            //});

            //routeTables.Add(new RouterTable
            //{
            //    RowNumber = 4,
            //    ArrowID = "4",
            //    ArrowText = "Arrowtext",
            //    SourceID = "4",
            //    SourceType = "process",
            //    SourceText = "x = x+1",
            //    TargetID = "5",
            //    TargetType = "end",
            //    TargetText = "end"
            //});

            //routeTables.Add(new RouterTable
            //{
            //    RowNumber = 5,
            //    ArrowID = "5",
            //    ArrowText = "Arrowtext",
            //    SourceID = "4",
            //    SourceType = "process",
            //    SourceText = "x = x+1",
            //    TargetID = "2",
            //    TargetType = "condition",
            //    TargetText = "x <= 5"
            //});

            //try
            //{
            //    if (routeTables[0].SourceType == "Process" &&
            //   routeTables[1].SourceType == "condition" &&
            //   routeTables[2].SourceType == "process" &&
            //   routeTables[3].SourceType == "process" &&
            //   routeTables[4].SourceType == "process")
            //    {
            //        string initial = "initial = '" + routeTables[0].SourceText + "'";
            //        string condition = " condition = '" + routeTables[1].SourceText + "'";
            //        string increment = " increment = '" + routeTables[3].SourceText + "'";


            //        string xmlbuild = "<for " + initial + " " + condition + " " + increment + " >";
            //        MessageBox.Show(xmlbuild);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}






            dg_RouteTable.DataSource = routeTables;
            return routeTables;

        }

        private List<RouterTable> ReorderToEqualRealFlowchart(List<RouterTable> routeTables)
        {
            var count = routeTables.Count;
            List<RouterTable> ordered = new List<RouterTable>();
            ordered.Add(routeTables.FirstOrDefault(x => x.SourceText == "Start") ?? new RouterTable());
            for (int i = 0; i < count; i++)
            {
                var items = routeTables.FindAll(x => x.SourceID == ordered[i].TargetID);
                foreach (var item in items)
                {
                    ordered.Add(item);
                    routeTables.Remove(item);
                }
            }

            return ordered;
        }

        private String CheckFCNodeType(XMLCellNode flowChart)
        {
            if (flowChart.Style.StartsWith("whiteSpace=wrap"))
            {
                return "Process";
            }
            else if (flowChart.Style.StartsWith("verticalLabelPosition"))
            {
                return "Connector";
            }
            else if (flowChart.Style.StartsWith("rounded"))
            {
                return flowChart.ValueText;
            }
            else if (flowChart.Style.StartsWith("rhombus"))
            {
                return "Condition";
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
