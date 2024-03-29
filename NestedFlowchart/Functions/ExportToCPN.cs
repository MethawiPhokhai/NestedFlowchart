﻿using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using NestedFlowchart.Rules;
using NestedFlowchart.Templates;
using System.Configuration;

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
        private readonly OutputRule _outputRule;

        public ExportToCPN(Rule1 rule1, Rule2 rule2, Rule3 rule3, Rule4 rule4
            , Rule5 rule5, Rule6 rule6, Rule7 rule7, TransformationApproach approach, OutputRule outputRule)
        {
            _rule1 = rule1;
            _rule2 = rule2;
            _rule3 = rule3;
            _rule4 = rule4;
            _rule5 = rule5;
            _rule6 = rule6;
            _rule7 = rule7;
            _approach = approach;
            _outputRule = outputRule;
        }

        public void ExportFile(string? TemplatePath, string? ResultPath, List<XMLCellNode> sortedFlowcharts)
        {
            string[] allTemplates = ReadAllTemplate(TemplatePath);

            #region Loop variables
            var loop1 = ConfigurationManager.AppSettings["loop1"]?.ToString() ?? "loop1";
            var loop2 = ConfigurationManager.AppSettings["loop2"]?.ToString() ?? "loop2";
            var loop3 = ConfigurationManager.AppSettings["loop3"]?.ToString() ?? "loop3";
            var loop4 = ConfigurationManager.AppSettings["loop4"]?.ToString() ?? "loop4";
            var loop5 = ConfigurationManager.AppSettings["loop5"]?.ToString() ?? "loop5";
            #endregion

            #region AppleRules

            int declareType = 0;
            int countSubPage = 0;
            int variableCount = 0;
            PageDeclare pages = new PageDeclare();
            List<PreviousNode> previousNodes = new List<PreviousNode>();

            //Declare ArrayName for arc
            bool isDeclaredI = false;
            string arrayName = string.Empty;

            //Declare page position
            PositionManagements page1Position = new PositionManagements();
            PositionManagements page2Position = new PositionManagements();
            PositionManagements page3Position = new PositionManagements();
            PositionManagements page4Position = new PositionManagements();
            PositionManagements page5Position = new PositionManagements();

            List<TempArrow> arrows = new List<TempArrow>();

            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                var flowchartType = sortedFlowcharts[i].NodeType.ToLower();
                var flowchartValue = sortedFlowcharts[i].ValueText;
                var flowchartValueRemoveSpace = flowchartValue.Replace(" ", string.Empty).Trim();
                System.Diagnostics.Debug.WriteLine(flowchartValue);

                //Arrow
                if (flowchartType == "arrow")
                {
                    #region Arrow

                    if (arrows.Any())
                    {
                        CreateArc(allTemplates, pages, previousNodes, isDeclaredI, arrayName, page1Position, page2Position, page3Position, page4Position, page5Position, arrows, declareType);

                        // if initial marking เป็น type none, ให้เอาตัวแปรมาแทน x เช่น i แทน x ในหน้า main
                        if (previousNodes.LastOrDefault().InitialMarkingType == (int)eDeclareType.IsNone)
                        {
                            arrayName = previousNodes.LastOrDefault().ArrayName;
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

                    #endregion Arrow
                }
                //Rule1 : Start
                else if (flowchartType == "start")
                {
                    #region Rule1

                    var currentPlaceModel = _rule1.ApplyRule(page1Position);

                    PreviousNode pv = AssignPreviousNode(sortedFlowcharts[i].ID, declareType, previousNodes, i, "", currentPlaceModel, null, "", 0, 0);
                    previousNodes.Add(pv);

                    #endregion Rule1
                }
                //Rule2 : Initialize Process
                else if (flowchartType == "process" && sortedFlowcharts[i - 2].NodeType.ToLower() == "start")
                {
                    #region Rule2

                    //ถ้าไม่มี Initialize process แล้ว element ต่อไปเป็น i
                    //Nested-Loop
                    if ((flowchartValue.ToLower().Trim().Contains($"{loop1} =")))
                    {
                        //Set Initial Marking
                        (var arcVar, _, declareType, variableCount) = _rule2.AssignInitialMarking(sortedFlowcharts, arrayName, previousNodes.LastOrDefault(), i);
                        arrayName = arcVar;

                        //Create Start place
                        var startPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], previousNodes.LastOrDefault().currentPlaceModel);

                        //เรียกไปหา Rule3 (สร้าง Transition i)

                        #region rule3_1

                        //In case declare more than 1 line
                        var code = sortedFlowcharts[i].ValueText.Replace("<br>", "\n").ToLower().Replace("int", "").Replace(";", "");

                        var (rule3Place, rule3Transition, rule3Arc1) = _rule3.ApplyRuleWithoutHierarchy(code, arrayName, page1Position, previousNodes.LastOrDefault(), declareType);

                        PreviousNode pv = AssignPreviousNode(sortedFlowcharts[i].ID, declareType, previousNodes, i, flowchartValue.ToLower().Trim().Substring(0, 1).Trim(), rule3Place, rule3Transition, "place", 0, 0);
                        previousNodes.Add(pv);

                        var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3Place);
                        var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
                        var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);

                        var rule3OldString = startPlace + place1 + transition + arc1;

                        CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule3OldString);

                        #endregion rule3_1
                    }
                    //BubbleSort/Nested-If
                    else
                    {
                        //Set Initial Marking
                        (var arcVar, var cSeg, declareType, variableCount) = _rule2.AssignInitialMarking(sortedFlowcharts, arrayName, previousNodes.LastOrDefault(), i);
                        arrayName = arcVar;

                        //Apply Rule
                        var (rule2Place, rule2Transition, rule2Arc1) = _rule2.ApplyRule(arrayName, cSeg, page1Position);

                        //Rule2 need to create Rule1 here because initial marking
                        var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], previousNodes.LastOrDefault().currentPlaceModel);

                        PreviousNode pv = AssignPreviousNode(sortedFlowcharts[i].ID, declareType, previousNodes, i, "", rule2Place, rule2Transition, "place", 0, 0);
                        previousNodes.Add(pv);

                        var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule2Arc1);
                        var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule2Transition);
                        var place2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule2Place);
                        var rule2String = place1 + place2 + transition + arc1;

                        CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule2String);
                    }

                    #endregion Rule2
                }
                //Rule3 : I=0, J=1 , Rule4
                else if (flowchartType == "process")
                {
                    //Case Not Nested => Define i
                    if (flowchartValueRemoveSpace.ToLower().Trim().Contains($"{loop1}="))
                    {
                        #region rule3_1
                        //In case declare more than 1 line
                        var code = sortedFlowcharts[i].ValueText.Replace("<br>", "\n").ToLower().Replace("int", "").Replace(";", "");

                        var (rule3Place, rule3Transition, rule3Arc1) = _rule3.ApplyRuleWithoutHierarchy(code, arrayName, page1Position, previousNodes.LastOrDefault(), declareType);

                        PreviousNode pv = AssignPreviousNode(sortedFlowcharts[i].ID, declareType, previousNodes, i, "", rule3Place, rule3Transition, "place", 0, 0);
                        previousNodes.Add(pv);

                        var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3Place);
                        var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
                        var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);

                        var rule3OldString = place1 + transition + arc1;

                        CreatePageNodeByCountSubPage(pv.CurrentSubPage, pages, rule3OldString);

                        #endregion rule3_1
                    }
                    //Case Nested => Create Hierachy Tool
                    else if (flowchartValueRemoveSpace.ToLower().Trim().Contains($"{loop2}=") || flowchartValueRemoveSpace.ToLower().Trim().Contains($"{loop3}=")
                        || flowchartValueRemoveSpace.ToLower().Trim().Contains($"{loop4}=") || flowchartValueRemoveSpace.ToLower().Trim().Contains($"{loop5}="))
                    {
                        #region Rule3_2

                        PositionManagements mainPagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);
                        PositionManagements subPagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentSubPage + 1, page1Position, page2Position, page3Position, page4Position, page5Position); //Need +1

                        //Get subpage by current page id
                        string subPageId = pages.subPageModel1.Id;
                        string subPageName = pages.subPageModel1.Name;
                        switch (previousNodes.LastOrDefault().CurrentSubPage)
                        {
                            case 1:
                                subPageId = pages.subPageModel2.Id;
                                subPageName = pages.subPageModel2.Name;
                                break;

                            case 2:
                                subPageId = pages.subPageModel3.Id;
                                subPageName = pages.subPageModel3.Name;
                                break;

                            case 3:
                                subPageId = pages.subPageModel4.Id;
                                subPageName = pages.subPageModel4.Name;
                                break;
                        }

                        var (rule3InputPlace, rule3OutputPlace, rule3InputPlace2, rule3OutputPlace2, rule3PS2,
                                                    rule3Transition, rule3Transition2,
                                                    rule3Arc1, rule3Arc2, rule3Arc3, rule3Arc4, rule3Arc5,
                                                    rule3Transition3, rule3Arc6) = _rule3.ApplyRuleWithHierarchy(
                                                    allTemplates[(int)TemplateEnum.SubStrTemplate],
                                                    allTemplates[(int)TemplateEnum.PortTemplate],
                                                    subPageId,
                                                    subPageName,
                                                    sortedFlowcharts[i].ValueText,
                                                    arrayName,
                                                    previousNodes.LastOrDefault(),
                                                    mainPagePosition,
                                                    subPagePosition,
                                                    declareType);

                        //Going to subpage page first
                        PreviousNode pv = AssignPreviousNode(
                                            sortedFlowcharts[i].ID,
                                            declareType,
                                            previousNodes,
                                            i,
                                            "",
                                            rule3InputPlace,
                                            rule3Transition3,
                                            (rule3Transition3 == null) ? "transition" : "place",
                                            previousNodes.LastOrDefault().CurrentMainPage,
                                            previousNodes.LastOrDefault().CurrentSubPage);

                        pv.previousPlaceModel = rule3PS2; //เอาไว้เป็น previous ของ subpage เพื่อไปต่อ node ต่อไป
                        pv.outputPortMainPagePlaceModel = rule3OutputPlace;
                        pv.outputPortSubPagePlaceModel = rule3OutputPlace2;
                        previousNodes.Add(pv);

                        //Set TransitionId for page instance
                        switch (previousNodes.LastOrDefault().CurrentSubPage)
                        {
                            case 0:
                                pages.subPageModel1.SubPageTransitionId = rule3Transition.Id1;
                                break;

                            case 1:
                                pages.subPageModel2.SubPageTransitionId = rule3Transition.Id1;
                                break;

                            case 2:
                                pages.subPageModel3.SubPageTransitionId = rule3Transition.Id1;
                                break;

                            case 3:
                                pages.subPageModel4.SubPageTransitionId = rule3Transition.Id1;
                                break;
                        }

                        //Main Page
                        var inputPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3InputPlace);
                        var subPageTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);
                        var outputPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3OutputPlace);
                        var arc0 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
                        var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc2);
                        var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc3);

                        //กรณี Connect ต่อจาก Place ให้สร้าง Transition ก่อน
                        var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition3);
                        var arc5 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc6);

                        var rule3OldString = inputPlace + transition + arc5 + subPageTransition + outputPlace + arc0 + arc1 + arc2;

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

                        #endregion Rule3_2
                    }
                    else
                    {
                        if (flowchartValueRemoveSpace.Contains($"temp=array[{loop2}+1]"))
                        {
                            #region Rule4_1

                            PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);
                            var (rule4Place, rule4Transition, rule4Arc) = _rule4.ApplyRuleWithCodeSegment(
                                                                          arrayName,
                                                                          previousNodes.LastOrDefault(),
                                                                          pagePosition,
                                                                          declareType);

                            PreviousNode pv = AssignPreviousNode(sortedFlowcharts[i].ID,
                                                declareType,
                                                previousNodes,
                                                i,
                                                "",
                                                rule4Place,
                                                rule4Transition,
                                                "transition",
                                                previousNodes.LastOrDefault().CurrentMainPage,
                                                previousNodes.LastOrDefault().CurrentSubPage);
                            previousNodes.Add(pv);

                            var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule4Place);
                            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule4Arc);
                            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule4Transition);

                            var rule4String = place1 + transition + arc1;
                            CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule4String);

                            #endregion Rule4_1
                        }
                        else if (flowchartValueRemoveSpace.Contains($"{loop2}++") || flowchartValueRemoveSpace.Contains($"{loop3}++") ||
                            flowchartValueRemoveSpace.Contains($"{loop4}++") || flowchartValueRemoveSpace.Contains($"{loop5}++"))
                        {
                            #region Rule4_2

                            //ลบ page เพื่อกลับไปหน้าก่อนหน้า
                            if (previousNodes.LastOrDefault().IsBacktoPreviousPage)
                            {
                                previousNodes.LastOrDefault().CurrentMainPage--;
                                previousNodes.LastOrDefault().IsBacktoPreviousPage = false;
                            }

                            var loopVariable = flowchartValue.Replace('+', ' ').Trim();

                            PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);
                            var rule4Transition = _rule4.ApplyRuleWithCodeSegment2(
                                                                    loopVariable,
                                                                    pagePosition);

                            PreviousNode pv = AssignPreviousNode(
                                sortedFlowcharts[i].ID,
                                declareType,
                                previousNodes,
                                i,
                                "",
                                null,
                                rule4Transition,
                                "place",
                                previousNodes.LastOrDefault().CurrentMainPage,
                                previousNodes.LastOrDefault().CurrentSubPage);
                            pv.IsBacktoPreviousPage = true; //Set เพื่อให้ arc ต่อไป ยังอยู่หน้าเดิม แต่หลังจากสร้าง Arc แล้ว ให้กลับไป page ก่อนหน้า
                            previousNodes.Add(pv);

                            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule4Transition);

                            var rule4String = transition;
                            CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule4String);

                            #endregion Rule4_2
                        }
                        else if (flowchartValueRemoveSpace.Contains($"{loop1}++"))
                        {
                            #region Rule4_3

                            //ลบ page เพื่อกลับไปหน้าก่อนหน้า
                            if (previousNodes.LastOrDefault().IsBacktoPreviousPage)
                            {
                                previousNodes.LastOrDefault().CurrentMainPage--;
                                previousNodes.LastOrDefault().IsBacktoPreviousPage = false;
                            }

                            var loopVariable = flowchartValue.Replace('+', ' ').Trim();

                            PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);
                            var rule4Transition = _rule4.ApplyRuleWithCodeSegment2(
                                                                    loopVariable,
                                                                    pagePosition);

                            PreviousNode pv = AssignPreviousNode(
                                                sortedFlowcharts[i].ID,
                                                declareType,
                                                previousNodes,
                                                i,
                                                "",
                                                null,
                                                rule4Transition,
                                                "transition",
                                                previousNodes.LastOrDefault().CurrentMainPage,
                                                previousNodes.LastOrDefault().CurrentSubPage);
                            previousNodes.Add(pv);

                            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule4Transition);

                            var rule4String = transition;
                            CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule4String);

                            #endregion Rule4_3
                        }
                    }
                }
                //Rule 5 Connector
                else if (flowchartType == "connector")
                {
                    #region Rule5

                    PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);
                    var (rule5Place, rule5Transition, rule5Arc1, previousTypeReturn) = _rule5.ApplyRule(
                                    arrayName,
                                    previousNodes.LastOrDefault(),
                                    pagePosition,
                                    declareType);

                    PreviousNode pv = AssignPreviousNode(
                                        sortedFlowcharts[i].ID,
                                        declareType,
                                        previousNodes,
                                        i,
                                        "",
                                        rule5Place,
                                        rule5Transition,
                                        previousTypeReturn,
                                        previousNodes.LastOrDefault().CurrentMainPage,
                                        previousNodes.LastOrDefault().CurrentSubPage);
                    previousNodes.Add(pv);

                    //Set หลังจากที่ประกาศ i เพื่อให้ Arc Variable ต่อไปใช้ (i,array)
                    isDeclaredI = true;

                    var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule5Place);
                    var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule5Arc1);
                    var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule5Transition);
                    var rule5String = place1 + transition + arc1;

                    CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, rule5String);

                    #endregion Rule5
                }
                //Rule 6 Decision
                else if (flowchartType == "condition")
                {
                    #region Rule6

                    PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);

                    string beforeCondition = sortedFlowcharts[i].ValueText.Contains("%") ?
                        sortedFlowcharts[i].ValueText.Replace("%", "mod") :
                        sortedFlowcharts[i].ValueText;

                    //Remove <br> from condition
                    beforeCondition = beforeCondition.Replace("<br>", "");

                    string trueCondition = _rule6.CreateTrueCondition(beforeCondition, arrayName);
                    string falseCondition = _rule6.CreateFalseDecision(trueCondition);

                    var (rule6Place, rule6FalseTransition, rule6TrueTransition, rule6Arc1, rule6Arc2) = _rule6.ApplyRule(
                        previousNodes.LastOrDefault(),
                        trueCondition,
                        falseCondition,
                        arrayName,
                        pagePosition,
                        declareType);

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

                    #endregion Rule6
                }
                //Rule 7 End
                else if (flowchartType == "end")
                {
                    #region Rule7

                    var rule7Place = _rule7.ApplyRule(
                        page1Position,
                        declareType,
                        countSubPage);

                    PreviousNode pv = AssignPreviousNode(
                                        sortedFlowcharts[i].ID,
                                        declareType,
                                        previousNodes,
                                        i,
                                        "",
                                        rule7Place,
                                        null,
                                        "transition",
                                        0,
                                        0);
                    previousNodes.Add(pv);

                    var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule7Place);

                    var rule7String = place1;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule7String);

                    #endregion Rule7
                }
                else if (flowchartType == "output")
                {
                    #region Output

                    PositionManagements pagePosition = GetPagePositionByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);
                    var isEndNext = sortedFlowcharts[i + 2].NodeType.ToLower() == "end";

                    //Apply Rule
                    var (outputRuleTransition1, outputRuleArc1, outputRulePlace, outputRuleTransition2, outputRuleArc2, previousTypeReturn) = _outputRule.ApplyRule(arrayName, pagePosition, previousNodes.LastOrDefault(), declareType, isEndNext);

                    PreviousNode pv = AssignPreviousNode(
                                        sortedFlowcharts[i].ID,
                                        declareType,
                                        previousNodes,
                                        i,
                                        "",
                                        outputRulePlace,
                                        (outputRuleTransition1 != null) ? outputRuleTransition1 : outputRuleTransition2,
                                        previousTypeReturn,
                                        previousNodes.LastOrDefault().CurrentMainPage,
                                        previousNodes.LastOrDefault().CurrentSubPage);

                    //Invert outputTransition1 and outputTransition2
                    pv.outputPreviousTransition = (outputRuleTransition1 == null) ? outputRuleTransition1 : outputRuleTransition2;
                    pv.IsUsePreviousFalse = isEndNext;
                    previousNodes.Add(pv);

                    var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], outputRuleArc1);
                    var transition1 = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], outputRuleTransition1);
                    var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], outputRuleArc2);
                    var transition2 = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], outputRuleTransition2);
                    var place = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], outputRulePlace);
                    var outputRuleString = place + transition1 + arc1 + transition2 + arc2;

                    CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentMainPage, pages, outputRuleString);

                    #endregion Output
                }
            }

            //Last Arc to End node
            CreateArc(allTemplates, pages, previousNodes, isDeclaredI, arrayName, page1Position, page2Position, page3Position, page4Position, page5Position, arrows, declareType);

            #endregion AppleRules

            string allColorSet = _approach.CreateAllColorSets(_approach, allTemplates, declareType, variableCount);

            string allVar = _approach.CreateAllVariables(_approach, allTemplates, arrayName, declareType);

            string allPage = _approach.CreateAllPages(_approach, allTemplates, pages);

            string allInstances = _approach.CreateAllInstances(_approach, allTemplates, pages);

            string firstCPN = string.Format(allTemplates[(int)TemplateEnum.EmptyCPNTemplate],
                allColorSet + allVar, allPage, allInstances);

            //Write to CPN File
            File.WriteAllText(ResultPath + @"\Result.cpn", firstCPN);
        }

        private void CreateArc(string[] allTemplates, PageDeclare pages, List<PreviousNode> previousNodes, bool isDeclaredI, string arrayName, PositionManagements page1Position, PositionManagements page2Position, PositionManagements page3Position, PositionManagements page4Position, PositionManagements page5Position, List<TempArrow> arrows, int type)
        {
            var currentPreviousNode = previousNodes.LastOrDefault();

            // Toggle type of Rule 6 ถ้ามี condition ต่อกัน 2 อัน หรือ
            if ((currentPreviousNode.IsPreviousNodeCondition && previousNodes.ElementAtOrDefault(previousNodes.Count - 2).IsPreviousNodeCondition))
            {
                currentPreviousNode.Type = (currentPreviousNode.Type == "transition") ? "place" : "transition";
            }

            // Get page position from the page
            PositionManagements pagePosition = GetPagePositionByCountSubPage(currentPreviousNode.CurrentMainPage, page1Position, page2Position, page3Position, page4Position, page5Position);

            //ถ้า Destination ลากไป End แล้วไม่มี Transition ให้สร้าง Transition
            var destinationNode = previousNodes.FirstOrDefault(x => x.elementId == arrows.LastOrDefault().Destination);
            if (destinationNode.elementId.Contains("Rj-32") && currentPreviousNode.elementId.Contains("li-r-14"))
            {
                //Transition
                TransitionModel tr = new TransitionModel()
                {
                    Id1 = IdManagements.GetlastestTransitionId(),
                    Id2 = IdManagements.GetlastestTransitionId(),
                    Id3 = IdManagements.GetlastestTransitionId(),
                    Id4 = IdManagements.GetlastestTransitionId(),
                    Id5 = IdManagements.GetlastestTransitionId(),

                    Name = IdManagements.GetlastestTransitionName(),

                    xPos1 = pagePosition.xPos1,
                    yPos1 = pagePosition.GetLastestyPos1()
                };

                //Transition to End
                ArcModel a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr.Id1,
                    PlaceEnd = destinationNode.currentPlaceModel.Id1,

                    xPos = pagePosition.GetLastestxArcPos(),
                    yPos = pagePosition.GetLastestyArcPos(),

                    Orientation = "TtoP", //Transition to Place
                    Type = arrayName
                };

                var t1 = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], tr);
                var aa1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], a1);

                var s1 = t1 + aa1;

                CreatePageNodeByCountSubPage(previousNodes.LastOrDefault().CurrentSubPage, pages, s1);

                destinationNode.currentTransitionModel = tr;
                destinationNode.IsConnectedEnd = true;
            }

            // Create arc with previous node
            var (arc, arc2, pv, currentMain, currentSub) = CreateArcWithPreviousNode(arrows.LastOrDefault(), currentPreviousNode.Type, pagePosition, arrayName, previousNodes, isDeclaredI, type);

            // Create arc and page node
            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], arc);
            CreatePageNodeByCountSubPage(currentMain, pages, arc1);

            // Create arc and page node
            var arc22 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], arc2);
            CreatePageNodeByCountSubPage(currentSub, pages, arc22);

            // If a subpage is created, continue with the subpage
            if (pv.IsCreateSubPage)
            {
                pv.CurrentMainPage = pv.CurrentSubPage;
                pv.currentPlaceModel = pv.previousPlaceModel; // Set previous place for the next node of the subpage
            }
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

        private PositionManagements GetPagePositionByCountSubPage(int countSubPage, PositionManagements page1Position, PositionManagements page2Position, PositionManagements page3Position, PositionManagements page4Position, PositionManagements page5Position)
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

                case 2:
                    pagePosition = page3Position;
                    break;

                case 3:
                    pagePosition = page4Position;
                    break;

                case 4:
                    pagePosition = page5Position;
                    break;
            }

            return pagePosition;
        }

        private PreviousNode AssignPreviousNode(string elementId, int declareType, List<PreviousNode> previousNodes, int i, string arrayName, PlaceModel currentPlaceModel, TransitionModel currentTransitionModel, string type, int currentMainPage, int currentSubPage)
        {
            PreviousNode pv = new PreviousNode();
            pv.elementId = elementId;
            pv.currentPlaceModel = currentPlaceModel;
            pv.currentTransitionModel = currentTransitionModel;
            pv.Type = type;
            pv.InitialMarkingType = declareType;
            pv.ArrayName = arrayName;
            pv.CurrentMainPage = currentMainPage;
            pv.CurrentSubPage = currentSubPage;
            return pv;
        }
    }
}