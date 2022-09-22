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
                throw new Exception(string.Format("Error {0} from {1} method", ex.Message, "Extracttion()"));
            }
        }

        public List<RouterTable> BindRouteTable(DataGridView dg_RouteTable, List<XMLCellNode> allFlowChartElement)
        {

            List<XMLCellNode> findRouteTable = allFlowChartElement
                .FindAll(a => a.Style.StartsWith("endArrow=classic") 
                || a.Style.StartsWith("edgeStyle=orthogonalEdgeStyle"));

            List<RouterTable> routeTables = new List<RouterTable>();
            int RowNumer = 0;
            foreach (var item in findRouteTable)
            {
                var source = allFlowChartElement.FirstOrDefault(a => a.ID == item.Source);
                var target = allFlowChartElement.FirstOrDefault(a => a.ID == item.Target);

                RowNumer += 1;
                routeTables.Add(new RouterTable
                {
                    RowNumber = RowNumer,
                    ArrowID = item.ID,
                    ArrowText = item.ValueText,
                    SourceID = item.Source,
                    SourceType = CheckFCNodeType(source),
                    SourceText = source.ValueText,
                    TargetID = item.Target,
                    TargetType = CheckFCNodeType(target),
                    TargetText = target.ValueText
                });
            }

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

            
            return routeTables;

        }

        public XMLCellNode? SortedFlowchartElement(List<XMLCellNode> allFlowChartElement,
            List<XMLCellNode> sortedFlowchart,
            List<XMLCellNode> tempDecisionElement,
            XMLCellNode? target)
        {
            //Find all if decision node
            if (target.Style.Contains("flowchart.decision") || target.Style.Contains("rhombus"))
            {
                var arrows = allFlowChartElement.FindAll(x => x.Source == target?.ID);
                XMLCellNode? lastElement;

                //First direction
                tempDecisionElement.Add(allFlowChartElement.Find(x => x.ID == arrows.FirstOrDefault().Target));

                //Second direction
                sortedFlowchart.Add(arrows.LastOrDefault());
                lastElement = allFlowChartElement.Find(x => x.ID == arrows.LastOrDefault().Target);
                sortedFlowchart.Add(lastElement);

                return lastElement;
            }
            else
            {
                //Arrow from source to target
                var arrow = allFlowChartElement.Find(x => x.Source == target?.ID);
                if (arrow == null && target.ValueText.ToLower() == "end") //Case End node
                {
                    arrow = allFlowChartElement.Find(x => x.Target == target?.ID);
                }

                if (!sortedFlowchart.Contains(arrow))
                {
                    sortedFlowchart.Add(arrow);
                }

                //Next Element
                var element = allFlowChartElement.Find(x => x.ID == arrow?.Target);

                if (!sortedFlowchart.Contains(element)) //If exist, not add and sent next temp element
                {
                    sortedFlowchart.Add(element);
                }
                else
                {
                    var lastTemp = tempDecisionElement.LastOrDefault();

                    sortedFlowchart.Add(lastTemp);

                    //If used, remove it
                    tempDecisionElement.Remove(lastTemp);
                    return lastTemp;
                }

                return element;
            }
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
