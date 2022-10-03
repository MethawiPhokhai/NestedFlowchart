using NestedFlowchart.Functions;
using NestedFlowchart.Models;

namespace NestedFlowchart.Declaration
{
    public class PageDeclare
    {
        //Main Page
        public PageModel mainPageModel = new PageModel()
        {
            Id = "ID6",
            Name = "New Page"
        };

        //New Subpage Page 1
        public PageModel subPageModel1 = new PageModel()
        {
            Id = IdManagements.GetlastestPageId(),
            Name = "New Subpage1",
        };

        //New Subpage Page 2
        public PageModel subPageModel2 = new PageModel()
        {
            Id = IdManagements.GetlastestPageId(),
            Name = "New Subpage2",
        };

        //New Subpage Page 3
        public PageModel subPageModel3 = new PageModel()
        {
            Id = IdManagements.GetlastestPageId(),
            Name = "New Subpage3",
        };

        //New Subpage Page 4
        public PageModel subPageModel4 = new PageModel()
        {
            Id = IdManagements.GetlastestPageId(),
            Name = "New Subpage4",
        };

    }
}
