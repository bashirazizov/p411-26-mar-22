#pragma checksum "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\Shared\Components\Footer\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "013504e3a31118a7f11bd675d3c8ba99f2cebdfa"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_Footer_Default), @"mvc.1.0.view", @"/Views/Shared/Components/Footer/Default.cshtml")]
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
#line 1 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\_ViewImports.cshtml"
using vesper;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\_ViewImports.cshtml"
using vesper.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"013504e3a31118a7f11bd675d3c8ba99f2cebdfa", @"/Views/Shared/Components/Footer/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"17897c4fa94152f53af907319263bbc8fc6ab07f", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_Footer_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<FooterViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<footer id=""footer"">
    <div class=""container"">
        <div class=""row d-flex align-items-center"">
            <div class=""col-lg-6 text-lg-left text-center"">
                <div class=""contact-about"">
                    <h3>Vesperr</h3>
                    <p>
                        ");
#nullable restore
#line 9 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\Shared\Components\Footer\Default.cshtml"
                   Write(Model.FooterText);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </p>\r\n                    <div class=\"social-links\">\r\n");
#nullable restore
#line 12 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\Shared\Components\Footer\Default.cshtml"
                         foreach (SocialLink item in Model.SocialLinks)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <a");
            BeginWriteAttribute("href", " href=\"", 544, "\"", 561, 1);
#nullable restore
#line 14 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\Shared\Components\Footer\Default.cshtml"
WriteAttributeValue("", 551, item.Link, 551, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"twitter\"><i");
            BeginWriteAttribute("class", " class=\"", 581, "\"", 602, 2);
            WriteAttributeValue("", 589, "bi", 589, 2, true);
#nullable restore
#line 14 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\Shared\Components\Footer\Default.cshtml"
WriteAttributeValue(" ", 591, item.Icon, 592, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></i></a>\r\n");
#nullable restore
#line 15 "C:\Users\Bashir Azizov\Desktop\p411\p411-26-mar-22\vesper\vesper\Views\Shared\Components\Footer\Default.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    </div>
                </div>
            </div>
            <div class=""col-lg-6"">
                <nav class=""footer-links text-lg-right text-center pt-2 pt-lg-0"">
                    <a href=""#intro"" class=""scrollto"">Home</a>
                    <a href=""#about"" class=""scrollto"">About</a>
                    <a href=""#"">Privacy Policy</a>
                    <a href=""#"">Terms of Use</a>
                </nav>
            </div>
        </div>
    </div>
</footer>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<FooterViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
