using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Templates;
using System.Configuration;

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
            if(model != null)
            {
                string newName = string.Empty;
                foreach (var item in model.Type)
                {
                    newName += "<id>" + item + "</id>\n";
                }

                return string.Format(colorTemplate, model.Id, model.Name, newName, model.Text, model.ColsetType);
            }

            return string.Empty;
        }

        public string CreateVar(string varTemplate, VarModel? model)
        {
            if(model != null)
            {
                string newName = string.Empty;
                var name = model.Name.Split(',');
                foreach (var item in name)
                {
                    newName += "<id>" + item + "</id>\n";
                }
                return string.Format(varTemplate, model.Id, model.Type, newName, model.Layout);
            }

            return string.Empty;
            
        }

        public string CreatePlace(string placeTemplate, PlaceModel? model)
        {
            if(model != null)
            {
                placeTemplate = string.Format("\n" + placeTemplate, model.Id1, model.Id2, model.Id3,
                model.Name, model.Type, model.InitialMarking,
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3,
                model.Port);

                return placeTemplate;
            }

            return string.Empty;
            
        }

        public string CreateTransition(string transitionTemplate, TransitionModel model)
        {
            if(model != null)
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

            return string.Empty;
        }

        public string CreateArc(string arcTemplate, ArcModel? model)
        {
            if(model != null)
            {
                string arc = string.Format("\n" + arcTemplate, model.Id1, model.Id2,
                model.TransEnd, model.PlaceEnd,
                model.Orientation, model.Type,
                model.xPos, model.yPos);

                return arc;
            }

            return string.Empty;
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

        public string CreateAllColorSets(TransformationApproach approach, string[] allTemplates, int declareType, int variableCount)
        {
            ColorSetModel colorSetProduct1 = null;
            ColorSetModel colorSetProduct2 = null;
            ColorSetModel colorSetProduct3 = null;
            ColorSetModel colorSetProduct4 = null;
            ColorSetModel colorSetProduct5 = null;

            if (declareType == (int)eDeclareType.IsNone)
            {
                colorSetProduct1 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopi",
                    Type = new List<string>()
                    {
                        "INT"
                    },
                    Text = "colset loopi = int;",
                    ColsetType = "int"
                };

                colorSetProduct2 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopj",
                    Type = new List<string>()
                    {
                        "INT",
                        "INT"
                    },
                    Text = "colset loopj = product INT*INT;",
                    ColsetType = "product"
                };

                colorSetProduct3 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopk",
                    Type = new List<string>()
                    {
                        "INT",
                        "INT",
                        "INT"
                    },
                    Text = "colset loopj = product INT*INT*INT;",
                    ColsetType = "product"
                };

                colorSetProduct4 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopl",
                    Type = new List<string>()
                    {
                        "INT",
                        "INT",
                        "INT",
                        "INT"
                    },
                    Text = "colset loopk = product INT*INT*INT*INT;",
                    ColsetType = "product"
                };

                colorSetProduct5 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopm",
                    Type = new List<string>()
                    {
                        "INT",
                        "INT",
                        "INT",
                        "INT",
                        "INT"
                    },
                    Text = "colset loopl = product INT*INT*INT*INT*INT;",
                    ColsetType = "product"
                };
            }
            else if(declareType == (int)eDeclareType.IsInteger)
            {
                var typeList = new List<string>();
                string textList = string.Empty;
                for (int i = 0; i < variableCount; i++)
                {
                    typeList.Add("INT");
                    textList += "INT*";
                }

                colorSetProduct1 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "aa",
                    Type = typeList,
                    Text = $"colset aa = product {textList.Remove(textList.Length - 1)};",
                    ColsetType = "product"
                };
            }
            else
            {
                colorSetProduct1 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopi",
                    Type = new List<string>()
                    {
                        "INT",
                        "INTs"
                    },
                    Text = "colset loopi = product INT*INTs;",
                    ColsetType = "product"
                };

                colorSetProduct2 = new ColorSetModel()
                {
                    Id = IdManagements.GetlastestColorSetId(),
                    Name = "loopj",
                    Type = new List<string>()
                    {
                        "INT",
                        "INT",
                        "INTs"
                    },
                    Text = "colset loopj = product INT*INT*INTs;",
                    ColsetType = "product"
                };
            }
            

            var col1 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct1);
            var col2 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct2);
            var col3 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct3);
            var col4 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct4);
            var col5 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct5);
            var allColorSet = col1 + col2 + col3 + col4 + col5;
            return allColorSet;
        }

        public string CreateAllVariables(TransformationApproach approach, string[] allTemplates, string arrayName, int declareType)
        {
            VarModel var1Model = null, var2Model = null, var3Model = null;
            if (declareType == (int)eDeclareType.IsArray)
            {
                var1Model = new VarModel()
                {
                    Id = IdManagements.GetlastestVarId(),
                    Type = "INTs",
                    Name = arrayName,
                    Layout = $"var {arrayName}: INTs;"
                };

                var2Model = new VarModel()
                {
                    Id = IdManagements.GetlastestVarId(),
                    Type = "INTs",
                    Name = arrayName + "2",
                    Layout = $"var {arrayName}2 : INTs;"
                };
            }
            else
            {
                //Use x when nested loop
                arrayName = (String.IsNullOrEmpty(arrayName)) ? "(x)" : arrayName;

                var3Model = new VarModel()
                {
                    Id = IdManagements.GetlastestVarId(),
                    Type = "INT",
                    Name = arrayName.Substring(1, arrayName.Length - 2),
                    Layout = $"var {arrayName.Substring(1, arrayName.Length - 2)}: INT;"
                };
            }

            #region Loop variables
            var loop1 = ConfigurationManager.AppSettings["loop1"]?.ToString() ?? "loop1";
            var loop2 = ConfigurationManager.AppSettings["loop2"]?.ToString() ?? "loop2";
            var loop3 = ConfigurationManager.AppSettings["loop3"]?.ToString() ?? "loop3";
            var loop4 = ConfigurationManager.AppSettings["loop4"]?.ToString() ?? "loop4";
            var loop5 = ConfigurationManager.AppSettings["loop5"]?.ToString() ?? "loop5";
            #endregion

            VarModel var4Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = $"{loop1},{loop1}2",
                Layout = $"var {loop1},{loop1}2: INT;"
            };

            VarModel var5Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = $"{loop2},{loop2}2",
                Layout = $"var {loop2},{loop2}2: INT;"
            };

            VarModel var6Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = $"{loop3},{loop3}2",
                Layout = $"var {loop3},{loop3}2: INT;"
            };

            VarModel var7Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = $"{loop4},{loop4}2",
                Layout = $"var {loop4},{loop4}2: INT;"
            };

            VarModel var8Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = $"{loop5},{loop5}2",
                Layout = $"var {loop5},{loop5}2: INT;"
            };

            VarModel var9Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = "z",
                Layout = "var z: INT;"
            };

            var var1 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var1Model);
            var var2 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var2Model);
			var var3 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var3Model);
            var var4 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var4Model);
            var var5 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var5Model);
            var var6 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var6Model);
            var var7 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var7Model);
            var var8 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var8Model);
            var var9 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var9Model);
            var allVar = var1 + var2 + var3 + var4 + var5 + var6 + var7 + var8 + var9;
            return allVar;
        }

        public string CreateAllPages(TransformationApproach approach, string[] allTemplates, PageDeclare pages)
        {
            var page1 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                pages.mainPageModel);

            string page2 = (!string.IsNullOrEmpty(pages.subPageModel1.Node)) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                pages.subPageModel1) :
                string.Empty;

            string page3 = (!string.IsNullOrEmpty(pages.subPageModel2.Node)) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel2) :
                string.Empty;

            string page4 = (!string.IsNullOrEmpty(pages.subPageModel3.Node)) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel3) :
                string.Empty;

            string page5 = (!string.IsNullOrEmpty(pages.subPageModel4.Node)) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel4) :
                string.Empty;

            var allPage = page1 + page2 + page3 + page4 + page5;
            return allPage;
        }

        public string CreateAllInstances(TransformationApproach approach, string[] allTemplates, PageDeclare page)
        {
            HierarchyInstanceModel inst = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = !string.IsNullOrEmpty(page.subPageModel1.SubPageTransitionId) ? "page=\"ID6\">" : "page=\"ID6\" />",
                Closer = "{0}"
            };

            var instances = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst);

            //Create instance for j
            string instances2 = string.Empty;
            if (!string.IsNullOrEmpty(page.subPageModel1.SubPageTransitionId))
            {
                HierarchyInstanceModel inst2 = new HierarchyInstanceModel()
                {
                    Id = IdManagements.GetlastestInstanceId(),
                    Text = "trans=\"" + page.subPageModel1.SubPageTransitionId + "\"",
                    Closer = !string.IsNullOrEmpty(page.subPageModel2.SubPageTransitionId) ? "> {0}" : "/></instance>"
                };


                instances2 = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst2);
            }

            //Create instance for k
            string instances3 = string.Empty;
            if (!string.IsNullOrEmpty(page.subPageModel2.SubPageTransitionId))
            {
                HierarchyInstanceModel inst3 = new HierarchyInstanceModel()
                {
                    Id = IdManagements.GetlastestInstanceId(),
                    Text = "trans=\"" + page.subPageModel2.SubPageTransitionId + "\"",
                    Closer = !string.IsNullOrEmpty(page.subPageModel3.SubPageTransitionId) ? "> {0}" : "/></instance></instance>"
                };


                instances3 = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst3);
            }

            //Create instance for l
            string instances4 = string.Empty;
            if (!string.IsNullOrEmpty(page.subPageModel3.SubPageTransitionId))
            {
                HierarchyInstanceModel inst4 = new HierarchyInstanceModel()
                {
                    Id = IdManagements.GetlastestInstanceId(),
                    Text = "trans=\"" + page.subPageModel3.SubPageTransitionId + "\"",
                    Closer = !string.IsNullOrEmpty(page.subPageModel4.SubPageTransitionId) ? "> {0}" : "/></instance></instance></instance>"
                };


                instances4 = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst4);
            }

            //Create instance for m
            string instances5 = string.Empty;
            if (!string.IsNullOrEmpty(page.subPageModel4.SubPageTransitionId))
            {
                HierarchyInstanceModel inst5 = new HierarchyInstanceModel()
                {
                    Id = IdManagements.GetlastestInstanceId(),
                    Text = "trans=\"" + page.subPageModel4.SubPageTransitionId + "\"",
                    Closer = "/></instance></instance></instance></instance>"
                };


                instances5 = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst5);
            }

            //m to l
            var mtol = string.Format(instances4, instances5);

            //l to k
            var ltok = string.Format(instances3, mtol);

            //k to j
            var ktoj = string.Format(instances2, ltok);

            //j to i
            var jtoi = string.Format(instances, ktoj);


            var allInstances = jtoi;
            return allInstances;
        }

        #endregion Create All
    }
}