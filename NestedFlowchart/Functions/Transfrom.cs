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

        public XMLCellNode? SortedFlowchartElement(List<XMLCellNode> allFlowChartElements,
            List<XMLCellNode> sortedFlowcharts,
            List<XMLCellNode> tempDecisionElements,
            XMLCellNode? target)
        {
            //Find all if decision node
            if (target.Style.Contains("flowchart.decision") || target.Style.Contains("rhombus"))
            {
                #region Get All 2 Arrow (True, False)
                //! Need to sorted the element to make sure it going to the right direction
                var arrows = allFlowChartElements.FindAll(x => x.Source == target?.ID);
                XMLCellNode? lastElement = null;
                #endregion

                //เอา ID ไปหาใน Parent เพื่อเอา Value Text True/False ออกมา
                foreach(var arrow in arrows)
                {
                    arrow.ValueText = allFlowChartElements.Find(x => x.Parent == arrow.ID).ValueText;
                }

                foreach (var arrow in arrows)
                {
                    if (arrow.ValueText.ToLower() == "true")
                    {
                        //First Condition คือ True Condition
                        sortedFlowcharts.Add(arrow);
                        lastElement = allFlowChartElements.Find(x => x.ID == arrow.Target);
                        sortedFlowcharts.Add(lastElement);
                    }
                    else
                    {
                        //Second Condition คือ False Condition
                        //Keep in temp after finish first direction pop this to the last of the list
                        tempDecisionElements.Add(arrow);
                        tempDecisionElements.Add(allFlowChartElements.Find(x => x.ID == arrow.Target));
                    }
                }

                return lastElement;
            }
            else
            {
				#region Add Arrow to sortedFlowcharts
				var arrow = allFlowChartElements.Find(x => x.Source == target?.ID);

                //If end node
                if (arrow == null && target.ValueText.ToLower() == "end") //Case End node
                {
                    arrow = allFlowChartElements.Find(x => x.Target == target?.ID);
                }

                //Normal arrow
                if (!sortedFlowcharts.Contains(arrow))
                {
                    sortedFlowcharts.Add(arrow);
                }
				#endregion

                # region Next Element
				var element = allFlowChartElements.Find(x => x.ID == arrow?.Target);

                if (!sortedFlowcharts.Contains(element)) //If exist, not add and sent next temp element
                {
                    sortedFlowcharts.Add(element);
                }
                else
                {
                    #region Decision Element
                    var lastTemp = tempDecisionElements.LastOrDefault();

                    if (!sortedFlowcharts.Contains(lastTemp))
                    {
						sortedFlowcharts.Add(lastTemp);
					}

                    //If used, remove it
                    tempDecisionElements.Remove(lastTemp);
                    #endregion

                    #region Arrow
                    var arrowTemp = tempDecisionElements.LastOrDefault();

                    if (!sortedFlowcharts.Contains(arrowTemp))
                    {
                        sortedFlowcharts.Add(arrowTemp);
                    }

                    //If used, remove it
                    tempDecisionElements.Remove(arrowTemp);
                    #endregion

                    return lastTemp;
                }
				#endregion

				return element;
			}
        }

        public string CheckFCNodeType(XMLCellNode flowChart)
        {

            if (flowChart.ValueText.ToLower().Contains("start"))
            {
                return "Start";
            }
            else if (flowChart.Style.Contains("rounded=1") || flowChart.Style.Contains("rounded=0;whiteSpace=wrap;html=1;"))
            {
                return "Process";
            }
            else if (flowChart.Style.Contains("ellipse;whiteSpace=wrap;html=1;"))
            {
                return "Connector";
            }
            else if (flowChart.Style.Contains("flowchart.decision") || flowChart.Style.Contains("rhombus"))
            {
                return "Condition";
            }
            else if (flowChart.ValueText.ToLower().Contains("end"))
            {
                return "End";
            }
            else if (flowChart.Style.Contains("edgeStyle=orthogonalEdgeStyle") || flowChart.Style.Contains("endArrow=classic;"))
            {
                return "Arrow";
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
