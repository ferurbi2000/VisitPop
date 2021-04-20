using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Models.ViewModels;

namespace VisitPop.MVC.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public MetaData PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; }
            = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public string PageClassLabel { get; set; }
        public string PageClassLinks { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");

            //Nuevo
            int startPage;
            int endPage;
            int LinksPerPage = 5;
            if (PageModel.TotalPages > 1)
            {
                if (PageModel.TotalPages <= LinksPerPage)
                {
                    startPage = 1;
                    endPage = PageModel.TotalPages;
                }
                else
                {
                    if (PageModel.PageNumber + LinksPerPage - 1 > PageModel.TotalPages)
                    {
                        startPage = PageModel.PageNumber - ((PageModel.PageNumber + LinksPerPage - 1)
                            - PageModel.TotalPages);
                        endPage = (PageModel.PageNumber + LinksPerPage - 1) -
                            ((PageModel.PageNumber + LinksPerPage - 1) - PageModel.TotalPages);
                    }
                    else
                    {
                        if (LinksPerPage != 2)
                        {
                            startPage = PageModel.PageNumber - (LinksPerPage / 2);
                            if (startPage < 1)
                            {
                                startPage = 1;
                            }
                            endPage = startPage + LinksPerPage - 1;
                        }
                        else
                        {
                            startPage = PageModel.PageNumber;
                            endPage = PageModel.PageNumber + LinksPerPage - 1;
                        }
                    }
                }

                //TagBuilder labelDiv;
                //labelDiv = new TagBuilder("div");
                //labelDiv.AddCssClass(PageClassLabel);
                //labelDiv.InnerHtml.Append($"Showing {PageModel.PageNumber} of {PageModel.TotalPages}");
                //result.InnerHtml.AppendHtml(labelDiv);
                TagBuilder linkDiv = new TagBuilder("div");
                linkDiv.InnerHtml.AppendHtml(GeneratePageLinks("First", 1, urlHelper));

                if (PageModel.HasPrevious)
                {
                    linkDiv.InnerHtml.AppendHtml(GeneratePageLinks("Previous", PageModel.PageNumber-1, urlHelper));
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    linkDiv.InnerHtml.AppendHtml(GeneratePageLinks(i.ToString(), i, urlHelper));
                }

                if (PageModel.HasNext)
                {
                    linkDiv.InnerHtml.AppendHtml(GeneratePageLinks("Next", PageModel.PageNumber + 1, urlHelper));
                }

                linkDiv.InnerHtml.AppendHtml(GeneratePageLinks("Last", PageModel.TotalPages, urlHelper));

                linkDiv.AddCssClass(PageClassLinks);
                result.InnerHtml.AppendHtml(linkDiv);
                output.Content.AppendHtml(result.InnerHtml);

            }
        }

        private TagBuilder GeneratePageLinks(string iterator, int pageNumber, IUrlHelper urlHelper)
        {
            string url;
            
            TagBuilder tag = new TagBuilder("a");

            PageUrlValues["Page"] = pageNumber;

            url = urlHelper.Action(PageAction, PageUrlValues);

            tag.Attributes["href"] = url;
            tag.AddCssClass(PageClass);
            if (iterator != "First" && iterator != "Last" && iterator != "Previous" && iterator != "Next")
            {
                tag.AddCssClass(Convert.ToInt32(iterator) == PageModel.PageNumber ? PageClassSelected : PageClassNormal);
            }
            else
            {
                tag.AddCssClass(pageNumber == PageModel.PageNumber ? PageClassSelected : PageClassNormal);
            }
            tag.InnerHtml.Append(iterator.ToString());

            return tag;

        }
    }
}
