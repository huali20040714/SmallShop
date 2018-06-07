using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SmallShop.Utilities
{
    public static class HtmlHelperExtensions
    {
        #region Select

        /// <summary>
        /// 主要查询页面使用
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="hasDefault"></param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static MvcHtmlString Select(this HtmlHelper htmlHelper, Type type, string name = "", bool hasDefault = false, string defaultValue = "", string defaultText = "", string className = "", string attrs = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = $"ddl{type.Name}";
            }
            var html = string.Empty;
            attrs += string.IsNullOrEmpty(className) ? "" : $" class=\"{className}\"";
            html += $@"<select id=""{name}"" name=""{name}"" {attrs}>";
            if (hasDefault)
            {
                html += $@"<option value=""{defaultValue}"">{defaultText}</option>";
            }
            foreach (var item in type.ToDictionary())
            {
                if (item.Key.ToString() == defaultValue)
                {
                    html += $@"<option value=""{item.Key}"" selected=""selected"">{item.Value}</option>";
                }
                else
                {
                    html += $@"<option value=""{item.Key}"">{item.Value}</option>";
                }
            }
            html += $@"</select>";

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString SelectEditor(this HtmlHelper htmlHelper, Type type, int defaultValue, string name = "", string className = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = type.Name;
            }
            var html = string.Empty;
            html += $@"<select id=""{name}"" name=""{name}"" class=""{className}"">";
            foreach (var item in type.ToDictionary())
            {
                if (defaultValue == item.Key)
                    html += $@"<option value=""{item.Key}"" selected=""selected"">{item.Value}</option>";
                else
                    html += $@"<option value=""{item.Key}"">{item.Value}</option>";
            }
            html += $@"</select>";

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString SelectEditor(this HtmlHelper htmlHelper, Type type, bool defaultValue, string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = type.Name;
            }
            var html = string.Empty;
            html += $@"<select id=""{name}"" name=""{name}"">";
            foreach (var item in type.ToDictionary())
            {
                if (defaultValue && item.Key == 1)
                    html += $@"<option value=""{item.Key}"" selected=""selected"">{item.Value}</option>";
                else
                    html += $@"<option value=""{item.Key}"">{item.Value}</option>";
            }
            html += $@"</select>";

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString SelectEditor(this HtmlHelper htmlHelper, List<KeyValuePair<int, string>> items, string name, int defaultValue, string className = "")
        {
            var html = string.Empty;
            html += $@"<select id=""{name}"" name=""{name}"" class=""{className}"">";
            foreach (var pair in items)
            {
                if (defaultValue == pair.Key)
                    html += $@"<option value=""{pair.Key}"" selected=""selected"">{pair.Value}</option>";
                else
                    html += $@"<option value=""{pair.Key}"">{pair.Value}</option>";
            }
            html += $@"</select>";

            return MvcHtmlString.Create(html);
        }

        #endregion

        #region Checkbox

        public static MvcHtmlString CheckboxFlagsEditor(this HtmlHelper htmlHelper, Type type, int defaultValue, int filterValue, string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = type.Name;
            }
            var html = string.Empty;
            foreach (var item in type.ToDictionary())
            {
                if ((filterValue & item.Key) != item.Key)
                    continue;

                var check = string.Empty;
                if ((defaultValue & item.Key) == item.Key)
                    check = "checked='checked'";

                html += $@"<label class='p-short-cell'><input class='ace' type='checkbox' id='{name}-{item.Key}' name='{name}' value='{item.Key}' {check}><span class='lbl' for='{name}-{item.Key}'> {item.Value}</span></label>";
            }

            return MvcHtmlString.Create(html);
        }

        #endregion

        #region Radiobox

        public static MvcHtmlString RadioboxFlagsEditor(this HtmlHelper htmlHelper, Type type, int defaultValue, int filterValue, string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = type.Name;
            }
            var html = string.Empty;
            foreach (var item in type.ToDictionary())
            {
                if ((filterValue & item.Key) != item.Key)
                    continue;

                var check = string.Empty;
                if ((defaultValue & item.Key) == item.Key)
                    check = "checked='checked'";

                html += $@"<label class='p-short-cell'><input class='ace' type='radio' id='{name}-{item.Key}' name='{name}' value='{item.Key}' {check}><span class='lbl' for='{name}-{item.Key}'> {item.Value}</span></label>";
            }

            return MvcHtmlString.Create(html);
        }

        #endregion

        #region Pagination

        /// <summary>
        /// 绘制页面上的分页隐藏变量
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderPaginationHiddenInput(this HtmlHelper htmlHelper, IQuery queryParams = null)
        {
            if (queryParams == null)
                queryParams = QueryParams.Empty;

            string html = $@"
                        <input name='pageIndex' type='hidden' id='hdfPageIndex' value='{queryParams.PageIndex}' />
                        <input name='pageSize' type='hidden' id='hdfPageSize' value='{queryParams.PageSize}' />
                        <input name='recordCount' type='hidden' id='hdfRecordCount' value='{queryParams.RecordCount}' />
                        <input name='orderBy' type='hidden' id='hdfOrderBy' value='{queryParams.OrderBy}' />
                        <input name='isAsc' type='hidden' id='hdfIsAsc' value='{queryParams.IsAsc}' />
                    ";

            return MvcHtmlString.Create(html);
        }

        /// <summary>
        /// 绘制列表页面的底部分页控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderPagination(this HtmlHelper htmlHelper)
        {
            var html = @"<div class=""block-footer noprint"">
                            <div class=""float-left"">
                                <label for=""table-display"" style=""display: inline"">显示</label>
                                <select size=""1"" id=""pageSizeChange"">
                                    <option value=""10"" selected=""selected"">10</option>
                                    <option value=""20"">20</option>
                                    <option value=""25"">25</option>
                                    <option value=""50"">50</option>
                                    <option value=""100"">100</option>
                                </select>
                                <label for=""table-display"" style=""display: inline"">记录,</label>
                                <label class=""pagerRemark"" for=""table-display"" style=""display: inline"">Loading...</label>
                            </div>
                            <div class=""pager float-right"" style=""height: 30px; margin: 8px;""></div>
                        </div>";

            return MvcHtmlString.Create(html);
        }

        #endregion

        #region 设置输出字段内容(布尔类型、数值类型)颜色

        //输出布尔转中文、并设置颜色（红、绿）
        public static MvcHtmlString RenderBitText(this HtmlHelper htmlHelper, bool isLocked, string falseText = "正常", string trueText = "已锁定")
        {
            if (isLocked)
                return MvcHtmlString.Create($"<span class='red'>{trueText}</span>");
            else
                return MvcHtmlString.Create($"<span class='green'>{falseText}</span>");
        }

        //输出金额为红色和是绿色
        public static MvcHtmlString BuildToggleDomColorClass(this HtmlHelper htmlHelper, decimal value, bool target = true, string defaultValue = null)
        {
            string className;
            if (value > 0)
                className = target ? "red" : "green";
            else if (value < 0)
                className = target ? "green" : "red";
            else
                className = "";

            return MvcHtmlString.Create($"<span class=\"{className}\">{value.ToDecimalString(defaultValue)}</span>");
        }

        #endregion

        #region Table

        public static MvcHtmlString RenderTh(this HtmlHelper htmlHelper, string text, object orderby = null, int width = 0, string className = "")
        {
            var style = (width > 0) ? " style='width: " + width + "px;'" : "";
            if (orderby == null)
                return MvcHtmlString.Create($@"<th{style}>{text}</th>");
            else
                return MvcHtmlString.Create($@"<th{style}><a href=""javascript:void(0);"" title=""单击排序"" class=""btn-sort-order {className}"" sort-expression=""{orderby}"">{text}</a></th>");
        }

        public static MvcHtmlString RenderEditTd(this HtmlHelper htmlHelper, string title, string name, string value, bool isMust = false, string style = "")
        {
            var html = $@"<tr>
                              <td class='text-right align-middle' style='width:120px;'>{title}：</td>
                              <td><input type='text' id='{name}' name='{name}' value='{value}' style = '{style}' />{(isMust ? "（<span style='color: red'>必填</span>）" : "")}</td>
                          </tr>";

            return MvcHtmlString.Create(html);
        }

        #endregion
        
        #region 根据权限隐藏电话号码

        /// <summary>
        /// 根据权限隐藏电话号码
        /// </summary>
        public static MvcHtmlString DisplayUserTel(this HtmlHelper htmlHelper, string tel, bool hasPermission)
        {
            if (hasPermission || string.IsNullOrEmpty(tel))
                return MvcHtmlString.Create(tel);

            var displayTel = Regex.Replace(tel, "^(\\d{3})(\\d{4})(\\d+)", "$1****$3");
            return MvcHtmlString.Create(displayTel);
        }

        #endregion

        #region 是否为DEBUG模式

        public static bool IsDebug(this HtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        #endregion
    }
}