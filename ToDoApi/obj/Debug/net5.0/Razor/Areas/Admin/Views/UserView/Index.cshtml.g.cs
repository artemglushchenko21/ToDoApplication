#pragma checksum "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fdd6a989d831bb538c24de0ae6da6c021867c2c9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_UserView_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/UserView/Index.cshtml")]
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
#line 1 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\_ViewImports.cshtml"
using ToDoApi;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\_ViewImports.cshtml"
using ToDoApi.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\_ViewImports.cshtml"
using ToDoApi.Models.ToDoTaskElements;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\_ViewImports.cshtml"
using ToDoApi.Models.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\_ViewImports.cshtml"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\_ViewImports.cshtml"
using ToDoMvc.Areas.Admin.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fdd6a989d831bb538c24de0ae6da6c021867c2c9", @"/Areas/Admin/Views/UserView/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a632659ac7abdcc8f3468f2cab5553ed9de007fa", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_UserView_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<UserViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditUser", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-outline-info btn-sm mr-2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("float: left"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
  
    ViewData["Title"] = " | Manage Users";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2 class=\"mb-2\">Manage Users</h2>\r\n\r\n<table class=\"table table-bordered table-striped table-sm\">\r\n    <thead>\r\n        <tr><th>First name</th><th>Last name</th><th>Email</th><th>Roles</th><th></th></tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 13 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
         if (Model.ApplicationUsers.Count() == 0)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr><td colspan=\"5\">There are no user accounts.</td></tr>\r\n");
#nullable restore
#line 16 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
        }
        else
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
             foreach (ApplicationUser user in Model.ApplicationUsers)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("    <tr>\r\n        <td>");
#nullable restore
#line 22 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
       Write(user.FirstName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        <td>");
#nullable restore
#line 23 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
       Write(user.LastName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        <td>");
#nullable restore
#line 24 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
       Write(user.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        <td>\r\n");
#nullable restore
#line 26 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
             foreach (string roleName in user.RoleNames)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div>");
#nullable restore
#line 28 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
                Write(roleName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 29 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </td>\r\n        <td>\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fdd6a989d831bb538c24de0ae6da6c021867c2c97925", async() => {
                WriteLiteral("\r\n                <span class=\"far fa-edit\"></span>\r\n            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 32 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
                                      WriteLiteral(user.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </td>\r\n    </tr>\r\n");
#nullable restore
#line 37 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "C:\Users\user\source\repos\ToDoApplication\ToDoApi\Areas\Admin\Views\UserView\Index.cshtml"
             
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<UserViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
