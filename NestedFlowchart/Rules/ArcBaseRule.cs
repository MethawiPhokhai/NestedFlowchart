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
            PositionManagements position, 
            string arrayName,
            List<PreviousNode> previousNodes, 
            bool isDeclaredI)
        {
            //หาตัวแรกที่ id ตรงกับ Destination เพื่อเอามาสร้าง Arc ต่อกัน
            var found = previousNodes.FirstOrDefault(x => x.elementId == arrow.Destination);

            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            string arcVariable = isDeclaredI ? DeclareArcVariable(arrayName, found.CurrentMainPage) : arrayName;

            //กรณีลากใส่ CN2
            if (arrow.Destination.Contains("KSG-18"))
            {
                arcVariable = "(i,j,array2)";
            }

            //ถ้าเป็น place ให้ใช้ PtoT, ถ้าเป็น transition ให้ใช้ TtoP
            string orientation = found.Type == "place" ? "PtoT" : "TtoP";

            ArcModel arcModel = new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = orientation,
                Type = arcVariable
            };

            if (found.Type == "place")
            {
                arcModel.PlaceEnd = found?.previousPlaceModel?.Id1;
                arcModel.TransEnd = found?.currentTransitionModel?.Id1;
            }
            else
            {
                arcModel.TransEnd = found?.previousTransitionModel?.Id1;
                arcModel.PlaceEnd = found?.currentPlaceModel?.Id1;
            }

            return (arcModel, found);
        }

        public (ArcModel? arcModel, PreviousNode previousNode) CreateArcforFalseCondition(
            TempArrow arrow,
            PositionManagements position,
            string arrayName,
            List<PreviousNode> previousNodes,
            bool isDeclaredI)
        {
            //หาตัวแรกที่ id ตรงกับ Destination เพื่อเอามาสร้าง Arc ต่อกัน
            var found = previousNodes.FirstOrDefault(x => x.elementId == arrow.Destination);

            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            string arcVariable = isDeclaredI ? DeclareArcVariable(arrayName, found.CurrentMainPage) : arrayName;

            ArcModel arcModel = new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                PlaceEnd = found?.previousPlaceModel?.Id1,
                TransEnd = found?.currentFalseTransitionModel?.Id1,
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = "PtoT",
                Type = arcVariable
            };

            return (arcModel, found);
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
