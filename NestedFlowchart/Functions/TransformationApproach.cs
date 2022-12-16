﻿using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Templates;

namespace NestedFlowchart.Functions
{
    public class TransformationApproach
    {
        #region Create

        public string CreatePage(string pageTemplate, PageModel model)
        {
            return string.Format("\n" + pageTemplate, model.Id, model.Name, model.Node);
        }

        public string CreateColorSet(string colorTemplate, ColorSetModel model)
        {
            string newName = string.Empty;
            foreach (var item in model.Type)
            {
                newName += "<id>" + item + "</id>\n";
            }

            return string.Format(colorTemplate, model.Id, model.Name, newName, model.Text);
        }

        public string CreateVar(string varTemplate, VarModel model)
        {
            string newName = string.Empty;
            var name = model.Name.Split(',');
            foreach (var item in name)
            {
                newName += "<id>" + item + "</id>\n";
            }
            return string.Format(varTemplate, model.Id, model.Type, newName, model.Layout);
        }

        public string CreatePlace(string placeTemplate, PlaceModel model)
        {
            placeTemplate = string.Format("\n" + placeTemplate, model.Id1, model.Id2, model.Id3,
                model.Name, model.Type, model.InitialMarking,
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3,
                model.Port);

            return placeTemplate;
        }

        public string CreateTransition(string transitionTemplate, TransitionModel model)
        {
            string transition = string.Format("\n" + transitionTemplate, model.Id1, model.Id2, model.Id3, model.Id4, model.Id5,
                model.Name, model.Condition,
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3,
                model.xPos4, model.yPos4,
                model.xPos5, model.yPos5,
                model.CodeSegment,
                model.SubsitutetionTransition);

            return transition;
        }

        public string CreateArc(string arcTemplate, ArcModel model)
        {
            string arc = string.Format("\n" + arcTemplate, model.Id1, model.Id2,
                model.TransEnd, model.PlaceEnd,
                model.Orientation, model.Type,
                model.xPos, model.yPos);

            return arc;
        }

        public string CreateHierarchyInstance(string instanceTemplate, HierarchyInstanceModel model)
        {
            return string.Format(instanceTemplate, model.Id, model.Text, model.Closer);
        }

        public string CreateHierarchySubSt(string subStrTemplate, HierarchySubStModel model)
        {
            return string.Format("\n" + subStrTemplate, model.SubPageId,
                model.NewInputPlaceId, model.OldInputPlaceId,
                model.NewOutputPlaceId, model.OldOutputPlaceId,
                model.Id, model.Name,
                model.xPos, model.yPos);
        }

        public string CreateHierarchyPort(string portTemplate, HierarchyPortModel model)
        {
            return string.Format("\n" + portTemplate, model.Id,
                model.Type, model.xPos, model.yPos);
        }

        #endregion Create

        #region Create All

        public string CreateAllColorSets(TransformationApproach approach, string[] allTemplates)
        {
            ColorSetModel colorSetProduct1 = new ColorSetModel()
            {
                Id = IdManagements.GetlastestColorSetId(),
                Name = "loopi",
                Type = new List<string>()
                {
                    "INT",
                    "INTs"
                },
                Text = "colset loopi = product INT*INTs;"
            };

            var col1 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct1);
            var allColorSet = col1;
            return allColorSet;
        }

        public string CreateAllVariables(TransformationApproach approach, string[] allTemplates, string arrayName)
        {
            VarModel var1Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INTs",
                Name = arrayName,
                Layout = $"var {arrayName}: INTs;"
            };

            VarModel var2Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = "i,i2,j,j2",
                Layout = "var i,i2,j,j2: INT;"
            };

            var var1 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var1Model);
            var var2 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var2Model);
            var allVar = var1 + var2;
            return allVar;
        }

        public string CreateAllPages(TransformationApproach approach, string[] allTemplates, PageDeclare pages)
        {
            var page1 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                pages.mainPageModel);

            string page2 = (pages.subPageModel1.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                pages.subPageModel1) :
                string.Empty;

            string page3 = (pages.subPageModel2.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel2) :
                string.Empty;

            string page4 = (pages.subPageModel3.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel3) :
                string.Empty;

            string page5 = (pages.subPageModel4.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel4) :
                string.Empty;

            var allPage = page1 + page2 + page3 + page4 + page5;
            return allPage;
        }

        public string CreateAllInstances(TransformationApproach approach, string[] allTemplates, TransitionModel definejTransition)
        {
            //TODO: Define instance if have more than 2
            HierarchyInstanceModel inst = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "page=\"ID6\">",
                Closer = "{0}"
            };

            HierarchyInstanceModel inst2 = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "trans=\"" + definejTransition.Id1 + "\"",
                Closer = "/></instance>"
            };

            var instances = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst);
            var instances2 = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst2);

            var allInstances = string.Format(instances, instances2);
            return allInstances;
        }

        #endregion Create All
    }
}