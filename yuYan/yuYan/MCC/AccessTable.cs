/*
 * 作者：黎国梁
 * 班级：三班
 * 学号：200632580088
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CMMCompiler
{
    /// <summary>
    /// AccessTable类,包含中间代码访问符号表的方法
    /// </summary>
    class AccessTable : MidCode
    {
        //可以为VARG,TARG,SARG,和常数
        string ARG = "";

        //可以为标识符
        string IN_1 = "";

        //数组下标,为标识符或者常数,如果不是数组,为空.
        string IN_2 = "";

        //为纯数字,用来判断数组下标是否为常数 还是 标识符
        string pattern = @"^[0-9]+$";

        /// <summary>
        /// 此方法，分解VARG(A)等，为VARG,A，返回A
        /// </summary>
        /// <param name="sth">需要分解的对象</param>
        /// <returns>返回A变量</returns>
        private string find_Table_Kind(string result)
        {
            int n = 0;
            ARG = "";
            IN_1 = "";
            IN_2 = "";

            char[] c = result.ToCharArray();

            while (n < c.Length && c[n].ToString() != "(")
            {
                ARG = ARG + c[n].ToString(); //得到ARG,用于访问表名
                n++;
            }

            n++;
            while (n < c.Length && c[n].ToString() != ")" && c[n].ToString() != "[")
            {
                IN_1 = IN_1 + c[n].ToString(); //得到IN_1,用于查找表内成员
                n++;
            }

            n++;
            while (n < c.Length && c[n].ToString() != "]")
            {
                IN_2 = IN_2 + c[n].ToString(); //得到IN_2,用于访问数组内部变量
                n++;
            }

            return IN_1;
        }

        /// <summary>
        /// 此方法返回result对应的所在表的value值
        /// </summary>
        /// <param name="sth">需要返回值的对象</param>
        /// <returns></returns>
        public string getValue(string result)
        {
            IN_1 = find_Table_Kind(result);
            switch (ARG)
            {
                case "VARG": //标识符
                    int indexID = IDlist.IndexOf(IN_1);
                    IDtable BStmp = (IDtable)IDtablelist[indexID];
                    return BStmp.Value;
                case "TARG": //临时变量
                    int indexTempID = TempIDlist.IndexOf(IN_1);
                    TempIDtable LStmp = (TempIDtable)TempIDtablelist[indexTempID];
                    return LStmp.Value;
                case "SARG": //数组

                    try //如果数组出界,返回一个空值null
                    {
                        if (!Regex.IsMatch(IN_2, pattern)) //如果数组下标为常数
                        {
                            int indexBS1 = IDlist.IndexOf(IN_2);
                            IDtable BStmp1 = (IDtable)IDtablelist[indexBS1];
                            IN_2 = BStmp1.Value; //得到数组下标的值
                        }

                        int indexSZ = SZlist.IndexOf(IN_1);
                        SZtable SZtmp = (SZtable)SZtablelist[indexSZ];
                        if (Convert.ToInt32(IN_2) > Convert.ToInt32(SZtmp.Length) - 1)
                        {
                            return "OutSide"; //数组越界
                        }
                        return Convert.ToString(SZtmp.element[Convert.ToInt32(IN_2)]);
                    }
                    catch (ArgumentNullException)
                    {
                        return null; //如果返回的为空值
                    }

                default:
                    return ARG; //如果为常数，得到常数的值       
            }
        }

        /// <summary>
        /// 此方法返回result在对应的符号表中的类型
        /// </summary>
        /// <param name="result">需要返回类型的对象</param>
        /// <returns></returns>
        public string getType(string result)
        {
            IN_1 = find_Table_Kind(result);
            switch (ARG)
            {
                case "VARG": //标识符
                    int indexID = IDlist.IndexOf(IN_1);
                    IDtable BStmp = (IDtable)IDtablelist[indexID];
                    return BStmp.Type;
                case "TARG": //临时变量
                    int indexTempID = TempIDlist.IndexOf(IN_1);
                    TempIDtable LStmp = (TempIDtable)TempIDtablelist[indexTempID];
                    return LStmp.Type;
                case "SARG": //数组
                    int indexSZ = SZlist.IndexOf(IN_1);
                    SZtable SZtmp = (SZtable)SZtablelist[indexSZ];
                    return SZtmp.Type;
                default:
                    return "real"; //理论上是不会运行到这里的，因为只在read的方法中才会使用，而read后面不会连接常数
            }
        }

        /// <summary>
        /// 此方法修改result对应的所在表的value值，为change
        /// </summary>
        /// <param name="result">需要重新赋值的对象</param>
        /// <param name="change">所赋的值</param>
        public void retSetValue(string result, string change)
        {
            string a = getValue(result);
            switch (ARG)
            {
                case "VARG":
                    int indexID = IDlist.IndexOf(IN_1);
                    IDtable BStmp = (IDtable)IDtablelist[indexID];
                    BStmp.Value = change;
                    break;
                case "TARG":
                    int indexTempID = TempIDlist.IndexOf(IN_1);
                    TempIDtable LStmp = (TempIDtable)TempIDtablelist[indexTempID];
                    LStmp.Value = change;
                    break;
                case "SARG":
                    if (!Regex.IsMatch(IN_2, pattern)) //如果数组下标为常数
                    {
                        int indexBS1 = IDlist.IndexOf(IN_2);
                        IDtable BStmp1 = (IDtable)IDtablelist[indexBS1];
                        IN_2 = BStmp1.Value;
                    }
                    int indexSZ = SZlist.IndexOf(IN_1);
                    SZtable SZtmp = (SZtable)SZtablelist[indexSZ];
                    SZtmp.element[Convert.ToInt32(IN_2)] = change;
                    break;
                default:
                    break;
            }

        }
    }
}