using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PHBC.DAO.Common
{
    public static class Utils
    {
        //Số bản ghi tối thiểu của một trang
        private const int pageSizeMin = 10;
        //Số bản ghi mặc định của một trang
        private const int pageSizeDefault = 20;
        /// <summary>
        /// Phân trang và trả ra số trang và danh sách trong trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">Trang can lay du lieu</param>
        /// <param name="pageSize">So ban ghi cua mot trang. Neu khong co thi nay theo default</param>
        /// <param name="pageCount">tra ra tong so trang</param>
        /// <returns></returns>
        public static List<T> buildPage<T>(IQueryable<T> query, int page, ref int pageSize, out int pageCount)
        {
            List<T> result = new List<T>();
            int totalItemCount = query.Count();
            //neu khong co du lieu thi return
            if (totalItemCount == 0)
            {
                pageCount = 0;
                return result;
            }
            //Neu khong co pageSize thì lấy theo default
            if (pageSize < 1)
                pageSize = pageSizeDefault;
            //pageSize nho hon 10 thi dat bang 10
            if (pageSize < pageSizeMin)
                pageSize = pageSizeMin;
            //tinh pagecount
            pageCount = totalItemCount / pageSize;
            //Neu so luong con du thi tang pagecount len 1
            if (totalItemCount % pageSize > 0) pageCount++;
            //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
            if (page >= pageCount)
                page = pageCount - 1;
            else
                page = page - 1;//So trang bat dau tu 0
            return query.Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Phân trang và trả ra số trang và danh sách trong trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">Trang can lay du lieu</param>
        /// <param name="pageSize">So ban ghi cua mot trang. Neu khong co thi nay theo default</param>
        /// <param name="pageCount">tra ra tong so trang</param>
        /// <returns></returns>
        public static List<T> buildPage<T>(IQueryable<T> query, int page, ref int pageSize, out int totalItemCount, out int pageCount)
        {
            List<T> result = new List<T>();
            totalItemCount = query.Count();
            //neu khong co du lieu thi return
            if (totalItemCount == 0)
            {
                pageCount = 0;
                return result;
            }
            //Neu khong co pageSize thì lấy theo default
            if (pageSize < 1)
                pageSize = pageSizeDefault;
            //pageSize nho hon 10 thi dat bang 10
            if (pageSize < pageSizeMin)
                pageSize = pageSizeMin;
            //tinh pagecount
            pageCount = totalItemCount / pageSize;
            //Neu so luong con du thi tang pagecount len 1
            if (totalItemCount % pageSize > 0) pageCount++;
            //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
            if (page >= pageCount)
                page = pageCount - 1;
            else
                page = page - 1;//So trang bat dau tu 0
            return query.Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Chuyển dự liệu từ list ra table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="FHead">True - Lay dislay name cua field vao row1</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data, bool FHead = false)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)

                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            if (FHead)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.DisplayName ?? row[prop.Name];
            }

            foreach (T item in data)
            {

                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)

                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);

            }

            return table;

        }

        public static bool stringEquals(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
                return true;
            if (string.IsNullOrEmpty(a))
                return false;
            if (string.IsNullOrEmpty(b))
                return false;
            return string.Equals(a, b);
        }

        #region getAttribute
        public static string getDislayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }
        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
            where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        public static object getValueProperty<T>(T data, string propertyName, out Type _type) where T : class
        {
            _type = null;
            object result = null;
            if (string.IsNullOrWhiteSpace(propertyName) || data == null)
                return result;
            PropertyInfo pi = data.GetType().GetProperties().Where(a => a.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (pi == null)
                return result;
            result = pi.GetValue(data);
            _type = pi.PropertyType;
            return result;
        }
        public static object getValueProperty<T>(T data, string propertyName) where T : class
        {
            object result = null;
            if (string.IsNullOrWhiteSpace(propertyName) || data == null)
                return result;
            PropertyInfo pi = data.GetType().GetProperties().Where(a => a.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (pi == null)
                return result;
            result = pi.GetValue(data);
            return result;
        }



        //danh sach kieu dữ liệu
        static string[] dsType = { "(int|short|byte)", "(double|decimal|float)", "date", "bool" };
        /// <summary>
        /// Lấy kiểu dữ liệu theo Enums.KieuDuLieu(Số nguyên, số thực, ngày, bool). Mặc định là kiểu string
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static Enums.KieuDuLieu getKieuDuLieu(Type _type)
        {
            if (_type == null)
                return Enums.KieuDuLieu.text;
            string typeName = _type.Name.ToLower();
            Regex rgx;
            //Note: Hiện tại chưa phân biệt dữ liệu null và not null
            //Kiểu dữ liệu không phải nullable
            for (int i = 0; i < dsType.Length; i++)
            {
                rgx = new Regex(dsType[i], RegexOptions.IgnoreCase);
                if (rgx.IsMatch(typeName))
                {
                    return (Enums.KieuDuLieu)i;
                }
            }
            //Kiểu dữ liệu là nullable
            string fullName = _type.FullName.ToLower();
            for (int i = 0; i < dsType.Length; i++)
            {
                rgx = new Regex(dsType[i], RegexOptions.IgnoreCase);
                if (rgx.IsMatch(fullName))
                {
                    return (Enums.KieuDuLieu)i;
                }
            }
            return Enums.KieuDuLieu.text;
        }
        #endregion
        #region BoDau
        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"

        };



        public static string RemoveSign4VietnameseString(string str)
        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;

        }

        public static string GetFriendlyTitle(string title, bool remapToAscii = false, int maxlength = 80)
        {
            if (title == null)
            {
                return string.Empty;
            }


            int length = title.Length;
            bool prevdash = false;
            StringBuilder stringBuilder = new StringBuilder(length);
            char c;


            for (int i = 0; i < length; ++i)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase 
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
                    (c == '\\') || (c == '-') || (c == '_') || (c == '='))
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append(' ');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int previousLength = stringBuilder.Length;


                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }


                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }


                if (i == maxlength)
                {
                    break;
                }
            }


            if (prevdash)
            {
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        private static string RemapInternationalCharToAscii(char character)
        {
            string s = character.ToString().ToLowerInvariant();
            if ("àåáâäãåąāáàạảãâấầậẩẫăắằặẳẵ".Contains(s))
            {
                return "a";
            }
            else if ("èéêëęéèẹẻẽêếềệểễ".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïıíìịỉĩ".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőðóòọỏõôốồộổỗơớờợởỡ".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭůúùụủũưứừựửữ".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿýỳỵỷỹ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (s.Equals('ř'))
            {
                return "r";
            }
            else if (s.Equals('ł'))
            {
                return "l";
            }
            else if ("đđ".Contains(s))
            {
                return "d";
            }
            else if (s.Equals('ß'))
            {
                return "ss";
            }
            else if (s.Equals('Þ'))
            {
                return "th";
            }
            else if (s.Equals('ĥ'))
            {
                return "h";
            }
            else if (s.Equals('ĵ'))
            {
                return "j";
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
