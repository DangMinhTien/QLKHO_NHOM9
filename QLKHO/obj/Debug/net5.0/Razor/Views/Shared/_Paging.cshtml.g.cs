#pragma checksum "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f31a1df739fa87247d897714b2b3c115ffc864c8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Paging), @"mvc.1.0.view", @"/Views/Shared/_Paging.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\_ViewImports.cshtml"
using QLKHO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\_ViewImports.cshtml"
using QLKHO.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f31a1df739fa87247d897714b2b3c115ffc864c8", @"/Views/Shared/_Paging.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc7800cf4d9c80d7c0757427513f9fadf931b8b3", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__Paging : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<QLKHO.Helper.PagingModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 9 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
  
    int currentPage = Model.currentPage;
    int countPages = Model.countPage;
    var generateUrl = Model.generateUrl;

    if (currentPage > countPages)
        currentPage = countPages;

    if (countPages <= 1) return;


    int? preview = null;
    int? next = null;

    if (currentPage > 1)
        preview = currentPage - 1;

    if (currentPage < countPages)
        next = currentPage + 1;

    // Các trang hiện thị trong điều hướng
    List<int> pagesRanges = new List<int>();


    int delta = 5;             // Số trang mở rộng về mỗi bên trang hiện tại
    int remain = delta * 2;     // Số trang hai bên trang hiện tại

    pagesRanges.Add(currentPage);
    // Các trang phát triển về hai bên trang hiện tại
    for (int i = 1; i <= delta; i++)
    {
        if (currentPage + i <= countPages)
        {
            pagesRanges.Add(currentPage + i);
            remain--;
        }

        if (currentPage - i >= 1)
        {
            pagesRanges.Insert(0, currentPage - i);
            remain--;
        }

    }
    // Xử lý thêm vào các trang cho đủ remain
    //(xảy ra ở đầu mút của khoảng trang không đủ trang chèn  vào)
    if (remain > 0)
    {
        if (pagesRanges[0] == 1)
        {
            for (int i = 1; i <= remain; i++)
            {
                if (pagesRanges.Last() + 1 <= countPages)
                {
                    pagesRanges.Add(pagesRanges.Last() + 1);
                }
            }
        }
        else
        {
            for (int i = 1; i <= remain; i++)
            {
                if (pagesRanges.First() - 1 > 1)
                {
                    pagesRanges.Insert(0, pagesRanges.First() - 1);
                }
            }
        }
    }


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<ul class=\"pagination\">\r\n    <!-- Previous page link -->\r\n");
#nullable restore
#line 85 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
     if (preview != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item\">\r\n            <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 2210, "\"", 2238, 1);
#nullable restore
#line 88 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
WriteAttributeValue("", 2217, generateUrl(preview), 2217, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Trang trước</a>\r\n        </li>\r\n");
#nullable restore
#line 90 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item disabled\">\r\n            <a class=\"page-link\" href=\"#\" tabindex=\"-1\" aria-disabled=\"true\">Trang trước</a>\r\n        </li>\r\n");
#nullable restore
#line 96 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <!-- Numbered page links -->\r\n");
#nullable restore
#line 99 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
     foreach (var pageitem in pagesRanges)
    {
        if (pageitem != currentPage)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item\">\r\n                <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 2661, "\"", 2690, 1);
#nullable restore
#line 104 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
WriteAttributeValue("", 2668, generateUrl(pageitem), 2668, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
#nullable restore
#line 105 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
               Write(pageitem);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </a>\r\n            </li>\r\n");
#nullable restore
#line 108 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li class=\"page-item active\" aria-current=\"page\">\r\n                <a class=\"page-link\" href=\"#\">");
#nullable restore
#line 112 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
                                         Write(pageitem);

#line default
#line hidden
#nullable disable
            WriteLiteral(" <span class=\"sr-only\">(current)</span></a>\r\n            </li>\r\n");
#nullable restore
#line 114 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
        }
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n    <!-- Next page link -->\r\n");
#nullable restore
#line 119 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
     if (next != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item\">\r\n            <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 3130, "\"", 3155, 1);
#nullable restore
#line 122 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
WriteAttributeValue("", 3137, generateUrl(next), 3137, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Trang sau</a>\r\n        </li>\r\n");
#nullable restore
#line 124 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <li class=\"page-item disabled\">\r\n            <a class=\"page-link\" href=\"#\" tabindex=\"-1\" aria-disabled=\"true\">Trang sau</a>\r\n        </li>\r\n");
#nullable restore
#line 130 "C:\Users\LENOVO\OneDrive\Documents\Wordspace\ThucTapCN\Code\QLKHO\QLKHO\Views\Shared\_Paging.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</ul>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<QLKHO.Helper.PagingModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
