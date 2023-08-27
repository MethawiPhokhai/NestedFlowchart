using NestedFlowchart.Declaration;

namespace NestedFlowchart.Rules
{
    public interface ITypeBaseRule
    {
        string GetTypeByInitialMarkingType(int type, int page);
        string GetTypeByPageOnly(int page);
    }

    public class TypeBaseRule : ITypeBaseRule
    {
        public string GetTypeByInitialMarkingType(int type, int page)
        {
            if (type == (int)eDeclareType.IsArray)
            {
                return "INTs";
            }
            else if (type == (int)eDeclareType.IsNone)
            {
                return GetTypeByCountSubPage(page);
            }
            else if (type == (int)eDeclareType.IsInteger)
            {
                return "aa";
            }
            else
            {
                throw new Exception("Invalid type");
            }
        }

        public string GetTypeByPageOnly(int page)
        {
            return GetTypeByCountSubPage(page);
        }

        private string GetTypeByCountSubPage(int page)
        {
            return page switch
            {
                0 => "loopi",
                1 => "loopj",
                2 => "loopk",
                3 => "loopl",
                4 => "loopm",
                _ => throw new Exception("Invalid page"),
            };
        }
    }
}