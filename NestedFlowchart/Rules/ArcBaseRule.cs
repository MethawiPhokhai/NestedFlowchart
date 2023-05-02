using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Windows.Forms;

namespace NestedFlowchart.Rules
{
    public class ArcBaseRule
    {
        public (ArcModel? arcModel, PreviousNode previousNode) CreateArcWithPreviousNode(
            TempArrow arrow, 
            string type,
            PositionManagements position, 
            string arrayName,
            List<PreviousNode> previousNodes, 
            bool isDeclaredI)
        {
            bool IsUsePreviousFalse = false;
            //หาตัวแรกที่ id ตรงกับ Destination เพื่อเอามาสร้าง Arc ต่อกัน
            var sourceNode = previousNodes.FirstOrDefault(x => x.elementId == arrow.Source);
            var destinationNode = previousNodes.FirstOrDefault(x => x.elementId == arrow.Destination);

            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            string arcVariable = isDeclaredI ? DeclareArcVariable(arrayName, destinationNode.CurrentMainPage) : arrayName;

            //กรณีลากใส่ CN2
            if (arrow.Id.Contains("3K-55"))
            {
                IsUsePreviousFalse = true;
            }
            //กรณีลากใส่ P5
            else if (arrow.Id.Contains("KSG-26"))
            {
                type = "transition";
                arcVariable = "(i,j2,array)";
            }

            //ถ้าเป็น place ให้ใช้ PtoT, ถ้าเป็น transition ให้ใช้ TtoP
            string orientation = (type == "place") ? "PtoT" : "TtoP";

            ArcModel arcModel = new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = orientation,
                Type = arcVariable
            };

            if (type == "place")
            {
                arcModel.PlaceEnd = sourceNode?.currentPlaceModel?.Id1;
                arcModel.TransEnd = destinationNode?.currentTransitionModel?.Id1;
            }
            else
            {
                arcModel.TransEnd = IsUsePreviousFalse ? 
                    sourceNode.currentFalseTransitionModel.Id1 :
                    sourceNode?.currentTransitionModel?.Id1;

                arcModel.PlaceEnd = destinationNode?.currentPlaceModel?.Id1;
            }

            return (arcModel, destinationNode);
        }

        public ArcModel CreateArcforOutputPortPlace(
            PositionManagements position)
        {
            return new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                PlaceEnd = "ID1412948787", //P4
                TransEnd = "ID1412848807", //GF2
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = "TtoP",
                Type = "(i,array)"
            };
        }

        public ArcModel CreateArcforEndPlace(
        PositionManagements position)
        {
            return new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                PlaceEnd = "ID1412948808", //End
                TransEnd = "ID1412848787", //GF1
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = "TtoP",
                Type = "array"
            };
        }

        public ArcModel CreateArcforCN2(
            PositionManagements position)
        {
            return new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                PlaceEnd = "ID1412948805", //CN2
                TransEnd = "ID1412848817", //GF3
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = "TtoP",
                Type = "(i,j,array)"
            };
        }

        public string DeclareArcVariable(string arrayName, int countSubPage)
        {
            //arc variable
            string arcVariable = string.Empty;
            switch (countSubPage)
            {
                case 0:
                    arcVariable = $"(i,{arrayName})";
                    break;
                case 1:
                    arcVariable = $"(i,j,{arrayName})";
                    break;
            }

            return arcVariable;
        }

    }
}
