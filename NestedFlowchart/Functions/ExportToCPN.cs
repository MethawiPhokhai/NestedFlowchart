using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using NestedFlowchart.Rules;
using NestedFlowchart.Templates;
using System.Linq;
using System.Text;

namespace NestedFlowchart.Functions
{
    public class ExportToCPN : ArcBaseRule
    {
        private readonly Rule1 _rule1;
        private readonly Rule2 _rule2;
        private readonly Rule3 _rule3;
        private readonly Rule4 _rule4;
        private readonly Rule5 _rule5;
        private readonly Rule6 _rule6;
        private readonly Rule7 _rule7;
        private readonly TransformationApproach _approach;

        public ExportToCPN(Rule1 rule1, Rule2 rule2, Rule3 rule3, Rule4 rule4
            , Rule5 rule5, Rule6 rule6, Rule7 rule7, TransformationApproach approach)
        {
            _rule1 = rule1;
            _rule2 = rule2;
            _rule3 = rule3;
            _rule4 = rule4;
            _rule5 = rule5;
            _rule6 = rule6;
            _rule7 = rule7;
            _approach = approach;
        }

        public void ExportFile(string? TemplatePath, string? ResultPath, List<XMLCellNode> sortedFlowcharts)
        {
            string[] allTemplates = ReadAllTemplate(TemplatePath);

            string allColorSet = _approach.CreateAllColorSets(_approach, allTemplates);

            #region AppleRules

            int countSubPage = 0;
            PageDeclare pages = new PageDeclare();
            List<PreviousNode> previousNodes = new List<PreviousNode>();

            #region Array Name
            /*
             * Declare Array name for arc
             */
            bool isDeclaredI = false;
            string arrayName = string.Empty;
			string arrayName2 = "array2";
			#endregion

            //Declare page position
            PositionManagements page1Position = new PositionManagements();
            PositionManagements page2Position = new PositionManagements();

            TransitionModel definejTransition = new TransitionModel();

            List<TempArrow> arrows = new List<TempArrow>();

            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                var flowchartType = sortedFlowcharts[i].NodeType.ToLower();
                var flowchartValue = sortedFlowcharts[i].ValueText;
                System.Diagnostics.Debug.WriteLine(flowchartValue);

                //Arrow
                if (flowchartType == "arrow")
                {
                    if (arrows.Any())
                    {
                        //Abandoned return arc
                        if (!arrows.LastOrDefault().Id.Contains("KSG-17"))
                        {
                            var currentPreviousNode = previousNodes.LastOrDefault();

                            // Toggle type of Rule 6 ถ้ามี condition ต่อกัน 2 อัน หรือ
                            if ((currentPreviousNode.IsPreviousNodeCondition && previousNodes.ElementAtOrDefault(previousNodes.Count - 2).IsPreviousNodeCondition))
                            {
                                currentPreviousNode.Type = (currentPreviousNode.Type == "transition") ? "place" : "transition";
                            }

                            // Get page position from the page
                            PositionManagements pagePosition = GetPagePositionByCountSubPage(currentPreviousNode.CurrentMainPage, page1Position, page2Position);

                            // Create arc with previous node
                            var (arc, pv) = CreateArcWithPreviousNode(arrows.LastOrDefault(), currentPreviousNode.Type, pagePosition, arrayName, previousNodes, isDeclaredI);

                            // Create arc and page node
                            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], arc);
                            CreatePageNodeByCountSubPage(pv.CurrentMainPage, pages, arc1);

                            // If a subpage is created, continue with the subpage
                            if (pv.IsCreateSubPage)
                            {
                                pv.CurrentMainPage = pv.CurrentSubPage;
                                pv.currentPlaceModel = pv.previousPlaceModel; // Set previous place for the next node of the subpage

                                // Toggle type of Rule3_2 if a place has been created in the subpage and Rule 6 checks for transitions
                                if (pv.CurrentMainPage > 0)
                                {
                                    pv.Type = (pv.Type == "transition") ? "place" : "transition";
                                }
                            }


                            //don't create for GF3
                            //if (!arrows.LastOrDefault().Id.Contains("KSG-21"))
                            //{
                            //    //Create arc to GF
                            //    if (sortedFlowcharts[i - 1].NodeType.ToLower() == "condition")
                            //    {
                            //        // Create arc with previous node
                            //        var (arc2, pv2) = CreateArcforFalseCondition(arrows.LastOrDefault(), pagePosition, arrayName, previousNodes, isDeclaredI);

                            //        // Create arc and page node
                            //        var arc22 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], arc2);
                            //        CreatePageNodeByCountSubPage(pv2.CurrentMainPage, pages, arc22);

                            //        //Create arc to Output port place
                            //        if (arrows.LastOrDefault().Id.Contains("KSG-30"))
                            //        {
                            //            var outputArc = CreateArcforOutputPortPlace(pagePosition);
                            //            var outputArc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], outputArc);
                            //            CreatePageNodeByCountSubPage(1, pages, outputArc1);
                            //        }
                            //    }

                            //    //Create arc to Output port place
                            //    if (arrows.LastOrDefault().Id.Contains("KSG-25"))
                            //    {
                            //        var GF3Arc = CreateArcforCN2(pagePosition);
                            //        var GF3Arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], GF3Arc);
                            //        CreatePageNodeByCountSubPage(1, pages, GF3Arc1);
                            //    }
                            //}
                        }
                    }

                    // Store arrow in temp for the next element to use
                    TempArrow arrow = new TempArrow()
                    {
                        Id = sortedFlowcharts[i].ID,
                        Source = sortedFlowcharts[i].Source,
                        Destination = sortedFlowcharts[i].Target
                    };
                    arrows.Add(arrow);
                }
                //Rule1 : Start
                else if (flowchartType == "start")
                {
                    #region Rule1
                    PreviousNode pv = new PreviousNode();
                    pv.currentPlaceModel = _rule1.ApplyRule(page1Position);

                    //keep element id for arrow can search
                    pv.elementId = sortedFlowcharts[i].ID;
                    previousNodes.Add(pv);
                    #endregion
                }
                //Rule2 : Initialize Process
                else if (flowchartType == "process" && sortedFlowcharts[i - 2].NodeType.ToLower() == "start")
                {
                    #region Rule2
                    //Set Initial Marking
                    arrayName = _rule2.AssignInitialMarking(sortedFlowcharts, arrayName, previousNodes.LastOrDefault(), i);

                    //Apply Rule
                    var (rule2Place, rule2Transition, rule2Arc1) = _rule2.ApplyRule(arrayName, page1Position);

                    //Rule2 need to create Rule1 here because initial marking
                    var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], previousNodes.LastOrDefault().currentPlaceModel);

                    //Set previous node for create arc next rule
                    PreviousNode pv = new PreviousNode();
                    pv.elementId = sortedFlowcharts[i].ID;
                    pv.currentPlaceModel = rule2Place;
                    pv.currentTransitionModel = rule2Transition;
                    pv.Type = "place";
                    previousNodes.Add(pv);

                    var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule2Arc1);
                    var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule2Transition);
                    var place2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule2Place);
                    var rule2String = place1 + place2 + transition + arc1;

                    CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule2String);
                    #endregion

                }
                //Rule3 : I=0, J=1 , Rule4
                else if (flowchartType == "process")
                {
                    //Case Not Nested => Define i
                    if (flowchartValue.ToLower().Trim().Contains("i ="))
                    {
                        #region rule3_1
                        //In case declare more than 1 line
                        var code = sortedFlowcharts[i].ValueText.Replace("<br>", "\n").ToLower().Replace("int", "").Replace(";", "");

                        var (rule3Place, rule3Transition, rule3Arc1) = _rule3.ApplyRuleWithoutHierarchy(code, arrayName, page1Position);

                        PreviousNode pv = new PreviousNode();
                        pv.elementId = sortedFlowcharts[i].ID;
                        pv.currentPlaceModel = rule3Place;
                        pv.currentTransitionModel = rule3Transition;
                        pv.Type = "place";
                        previousNodes.Add(pv);

                        var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3Place);
                        var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
                        var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);

                        var rule3OldString = place1 + transition + arc1;

                        CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule3OldString);
                        #endregion
                    }
                    //Case Nested => Create Hierachy Tool
                    else if (flowchartValue.ToLower().Trim().Contains("j =") || flowchartValue.ToLower().Trim().Contains("k =")
                        || flowchartValue.ToLower().Trim().Contains("l =") || flowchartValue.ToLower().Trim().Contains("m ="))
                    {
                        #region Rule3_2
                        var (rule3InputPlace, rule3OutputPlace, rule3InputPlace2, rule3OutputPlace2, rule3PS2,
                                                    rule3Transition, rule3Transition2,
                                                    rule3Arc1, rule3Arc2, rule3Arc3, rule3Arc4, rule3Arc5) = _rule3.ApplyRuleWithHierarchy(
                                                    allTemplates[(int)TemplateEnum.SubStrTemplate],
                                                    allTemplates[(int)TemplateEnum.PortTemplate],
                                                    pages.subPageModel1.Id,
                                                    sortedFlowcharts[i].ValueText,
                                                    arrayName,
                                                    previousNodes.LastOrDefault(),
                                                    page1Position,
                                                    page2Position
                                                    );

                        //Going to subpage page first
                        PreviousNode pv = new PreviousNode();
                        pv.elementId = sortedFlowcharts[i].ID;
                        pv.previousPlaceModel = rule3PS2; //เอาไว้เป็น previous ของ subpage เพื่อไปต่อ node ต่อไป
                        pv.currentPlaceModel = rule3InputPlace;
                        definejTransition = rule3Transition;
                        pv.Type = "transition";
                        previousNodes.Add(pv);


                        //Main Page
                        var inputPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3InputPlace);
                        var subPageTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);
                        var outputPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3OutputPlace);
                        var arc0 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
                        var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc2);
                        var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc3);

                        var rule3OldString = inputPlace + subPageTransition + outputPlace + arc0 + arc1 + arc2;

                        //Sub Page
                        var inputPlace2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3InputPlace2);
                        var outputPlace2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3OutputPlace2);
                        var afterInputTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition2);
                        var arc3 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc4);
                        var ps2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3PS2);
                        var arc4 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc5);

                        var rule3NewString = inputPlace2 + outputPlace2 + afterInputTransition + arc3 + ps2 + arc4;

                        //Add to main page 
                        CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule3OldString);

                        //Add to sub page
                        pv.CurrentSubPage++;
                        pv.IsCreateSubPage = true;
                        CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule3NewString);
                        #endregion
                    }
                    else
                    {
                        if (flowchartValue.Contains("temp = array[ j+1]"))
                        {
                            #region Rule4_1
                            PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position);
                            var (rule4Place, rule4Transition, rule4Arc) = _rule4.ApplyRuleWithCodeSegment(
                                                                                            arrayName,
                                                                                            previousNodes.LastOrDefault(),
                                                                                            pagePosition);

                            PreviousNode pv = new PreviousNode();
                            pv.elementId = sortedFlowcharts[i].ID;
                            pv.currentPlaceModel = rule4Place;
                            pv.currentTransitionModel = rule4Transition;
                            pv.Type = "transition";

                            //Set lastest page
                            pv.CurrentMainPage = previousNodes.LastOrDefault().CurrentMainPage;
                            pv.CurrentSubPage = previousNodes.LastOrDefault().CurrentSubPage;

                            previousNodes.Add(pv);

                            var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule4Place);
                            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule4Arc);
                            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule4Transition);

                            var rule4String = place1 + transition + arc1;
                            CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule4String);
                            #endregion
                        }
                        else if (flowchartValue.Contains("j ++"))
                        {
                            #region Rule4_2
                            PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position);
                            var rule4Transition = _rule4.ApplyRuleWithCodeSegment2(
                                                                    arrayName,
                                                                    previousNodes.LastOrDefault(),
                                                                    pagePosition);

                            PreviousNode pv = new PreviousNode();
                            pv.elementId = sortedFlowcharts[i].ID;
                            pv.currentTransitionModel = rule4Transition;
                            pv.Type = "place";

                            //Set lastest page
                            pv.CurrentMainPage = previousNodes.LastOrDefault().CurrentMainPage;
                            pv.CurrentSubPage = previousNodes.LastOrDefault().CurrentSubPage;

                            previousNodes.Add(pv);

                            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule4Transition);

                            var rule4String = transition;
                            CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule4String);
                            #endregion
                        }
                        else if (flowchartValue.Contains("i ++"))
                        {
                            #region Rule4_3
                            //Set back to main page
                            previousNodes.LastOrDefault().CurrentMainPage--;

                            PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position);
                            var (rule4Transition, rule4Arc1, rule4Arc2) = _rule4.ApplyRuleWithCodeSegment3(
                                                                    arrayName,
                                                                    previousNodes.LastOrDefault(),
                                                                    pagePosition);

                            PreviousNode pv = new PreviousNode();
                            pv.elementId = sortedFlowcharts[i].ID;
                            pv.currentTransitionModel = rule4Transition;
                            pv.Type = "transition";

                            //Set lastest page
                            pv.CurrentMainPage = previousNodes.LastOrDefault().CurrentMainPage;
                            pv.CurrentSubPage = previousNodes.LastOrDefault().CurrentSubPage;

                            previousNodes.Add(pv);

                            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule4Transition);

                            var rule4String = transition;
                            CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule4String);
                            #endregion
                        }
                    }
                }
                //Rule 5 Connector
                else if (flowchartType == "connector")
                {
                    #region Rule5
                    PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position);
                    var (rule5Place, rule5Transition, rule5Arc1, previousTypeReturn) = _rule5.ApplyRule(
                                    arrayName,
                                    previousNodes.LastOrDefault(),
                                    pagePosition,
                                    countSubPage);


                    PreviousNode pv = new PreviousNode();
                    pv.elementId = sortedFlowcharts[i].ID;
                    pv.currentPlaceModel = rule5Place;
                    pv.currentTransitionModel = rule5Transition;
                    pv.Type = previousTypeReturn;

                    //Set lastest page
                    pv.CurrentMainPage = previousNodes.LastOrDefault().CurrentMainPage;
                    pv.CurrentSubPage = previousNodes.LastOrDefault().CurrentSubPage;

                    previousNodes.Add(pv);

                    //Set หลังจากที่ประกาศ i เพื่อให้ Arc Variable ต่อไปใช้ (i,array)
                    isDeclaredI = true;

                    var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule5Place);
                    var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule5Arc1);
                    var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule5Transition);
                    var rule5String = place1 + transition + arc1;

                    CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule5String);
                    #endregion
                }
                //Rule 6 Decision
                else if (flowchartType == "condition")
                {
                    #region Rule6
                    PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position);
                    string trueCondition = _rule6.CreateTrueCondition(sortedFlowcharts[i].ValueText, arrayName);
                    string falseCondition = _rule6.CreateFalseDecision(trueCondition);

                    var (rule6Place, rule6FalseTransition, rule6TrueTransition, rule6Arc1, rule6Arc2) = _rule6.ApplyRule(
                        previousNodes.LastOrDefault(),
                        trueCondition,
                        falseCondition,
                        arrayName,
                        pagePosition);


                    PreviousNode pv = new PreviousNode();
                    pv.elementId = sortedFlowcharts[i].ID;
                    pv.currentTransitionModel = rule6TrueTransition;
                    pv.currentFalseTransitionModel = rule6FalseTransition;
                    pv.currentPlaceModel = rule6Place ?? previousNodes.LastOrDefault().currentPlaceModel;

                    //Set lastest page
                    pv.CurrentMainPage = previousNodes.LastOrDefault().CurrentMainPage;
                    pv.CurrentSubPage = previousNodes.LastOrDefault().CurrentSubPage;

                    //จำเป็นต้อง Set เป็น Place เพราะว่าต้องใช้ใน Rule3_2
                    pv.Type = "place";

                    //Set เพราะเอาไว้บอกตัวต่อไปว่า node ก่อนหน้าเป็น Condition
                    pv.IsPreviousNodeCondition = true;

                    previousNodes.Add(pv);

                    var ps3 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule6Place);
                    var trueTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule6TrueTransition);
                    var falseTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule6FalseTransition);
                    var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule6Arc1);
                    var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule6Arc2);

                    var rule6String = ps3 + trueTransition + falseTransition + arc1 + arc2;

                    CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule6String);
                    #endregion
                }
                //Rule 7 End
                else if (flowchartType == "end")
                {
                    #region Rule7
                    var rule7Place = _rule7.ApplyRule(
                        arrayName,
                        page1Position);

                    var rule7Arc1 = _rule7.CreateArcforEndPlace(page1Position);

                    var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule7Place);
                    var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule7Arc1);

                    var rule7String = place1 + arc1;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule7String);
                    #endregion
                }
            }

            #endregion AppleRules

            string allVar = _approach.CreateAllVariables(_approach, allTemplates, arrayName, arrayName2);

            string allPage = _approach.CreateAllPages(_approach, allTemplates, pages);

            string allInstances = _approach.CreateAllInstances(_approach, allTemplates, definejTransition);

            string firstCPN = string.Format(allTemplates[(int)TemplateEnum.EmptyCPNTemplate],
                allColorSet + allVar, allPage, allInstances);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }

        private string[] ReadAllTemplate(string? templatePath)
        {
            string[] templateNames =
            {
                "EmptyNet.txt", "Place.txt", "Transition.txt", "Arc.txt",
                "VarType.txt", "ColorSet.txt", "Hierarchy_Instance.txt",
                "Page.txt", "Hierarchy_SubPageTransition.txt", "Hierarchy_Port.txt"
            };

            string[] allTemplates = new string[templateNames.Length];

            for (int i = 0; i < templateNames.Length; i++)
            {
                string templateFilePath = Path.Combine(templatePath ?? string.Empty, templateNames[i]);
                allTemplates[i] = File.ReadAllText(templateFilePath);
            }

            return allTemplates;
        }


        private void CreatePageNodeByCountSubPage(int countSubPage, PageDeclare pages, string rule)
        {
            if (countSubPage < 0 || countSubPage > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(countSubPage), "โปรแกรม Support ไม่เกิน 5 nested loops, ปัจจุบันมี : " + countSubPage);
            }

            switch (countSubPage)
            {
                case 0:
                    pages.mainPageModel.Node += rule;
                    break;
                case 1:
                    pages.subPageModel1.Node += rule;
                    break;
                case 2:
                    pages.subPageModel2.Node += rule;
                    break;
                case 3:
                    pages.subPageModel3.Node += rule;
                    break;
                case 4:
                    pages.subPageModel4.Node += rule;
                    break;
            }
        }

        private PositionManagements GetPagePositionByCountSubPage(int countSubPage, PositionManagements page1Position, PositionManagements page2Position)
        {
            PositionManagements pagePosition = new PositionManagements();
            switch (countSubPage)
            {
                case 0:
                    pagePosition = page1Position;
                    break;
                case 1:
                    pagePosition = page2Position;
                    break;
            }

            return pagePosition;
        }


    }
}