
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Heavy.Web.TagHelpers
{
    /// <summary>
    ///把TagHelper去掉， Email就是目标元素
    /// </summary>
    public class EmailTagHelper:TagHelper
    {
        public string MailTo { get; set; }
        //public override void Process(
        //    TagHelperContext context,
        //    TagHelperOutput output
        //)
        //{
        //    output.TagName = "a"; //Replaces<email>with<a>tag
        //    output.Attributes.SetAttribute("href", $"mailto:{MailTo}");
        //    output.Content.SetContent(MailTo);
        //}


        public async override void Process(
            TagHelperContext context,
            TagHelperOutput output
        )
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();//里面的内容取出来
            var target = content.GetContent();
            output.Attributes.SetAttribute("href", "mailto"+target);
            output.Content.SetContent(target);
        }
    }
}
