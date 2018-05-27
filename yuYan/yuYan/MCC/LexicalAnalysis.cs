/*
 * 作者：黎国梁
 * 班级：三班
 * 学号：200632580088
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
 
using System.Text.RegularExpressions;

namespace CMMCompiler
{
    /// <summary>
    /// 该类是编译各个阶段都要用到的基本类，用作词法分析、语法分析和给其他类传递分析后的信息集
    /// </summary>
    class LexicalAnalysis
    {
        #region 全局声明变量
        //保存源程序代码
        private static string sourceCode;

        //保存词法分析后所得的全部单词信息
        public List<object> tokens = new List<object>();

        //保存词法分析所读取的字符串信息
        public string output;

        //保存分析出来的错误信息
        public List<object> errlist = new List<object>();

        //保存分析过程中的出现错误的次数
        public int errornum = 0;
        
        //保存扫描源程序的当前字符位置
        private int l = 0;

        //保存构成一个完整单词的字符串
        string token;
        #endregion

        #region 用来获取字符、判断字符的一些静态方法和静态变量
        //能识别的保留字
        private static string keyWords = @"^(if|else|while|read|write|int|while|real)$";

        //能识别的限界符
        private static string SeparatorWords = @"^(\(|\)|\;|\{|\}|\[|\])$";

        //能识别的运算符
        private static string operators = @"^(\+|\-|\*|\/|\=|\<)$";

        //能识别的字母
        private static string letters = @"^[a-zA-Z]+$";

        //能识别的空格或者回车，换行，或tab
        private static string pattern = @"^[\s]+$";

        //能识别的数字
        private static string numerics = @"^[0-9]+$";
    
        /// <summary>
        /// 返回一个字符串中第n个位置的字符
        /// </summary>
        /// <param name="n">第n个位置</param>
        /// <returns>返回该位置的字符</returns>
        private static char getChar(int n)
        {
            char[] c = sourceCode.ToCharArray();
            if (n < c.Length)
                return c[n];
            return sourceCode[sourceCode.Length - 1];
        }

        /// <summary>
        /// 判断是否是单字分隔符
        /// </summary>
        /// <param name="str">要判断的字符</param>
        /// <returns>返回判读结果</returns>
        private static bool isSeparator(char str)
        {
            return Regex.IsMatch(str.ToString(), SeparatorWords);
        }

        /// <summary>
        /// 判读是否是单字运算符
        /// </summary>
        /// <param name="str">要判读的字符</param>
        /// <returns>返回判读结果</returns>
        private static bool isOperators(char str)
        {
            return Regex.IsMatch(str.ToString(), operators);
        }

        /// <summary>
        /// 判读是否是字母
        /// </summary>
        /// <param name="str">要判读的字符</param>
        /// <returns>返回判读结果</returns>
        private static bool isChar(char str)
        {
            return Regex.IsMatch(str.ToString(), letters);
        }

        /// <summary>
        /// 判读是否为空格或者回车，换行，或tab
        /// </summary>
        /// <param name="str">要判读的字符</param>
        /// <returns>返回判读结果</returns>
        private static bool isBlank(char str)
        {
            return Regex.IsMatch(str.ToString(), pattern);        
        }
        
        /// <summary>
        /// 判读是否为整数
        /// </summary>
        /// <param name="str">要判读的字符</param>
        /// <returns>返回判读结果</returns>
        private static bool isNumber(char str)
        {
            return Regex.IsMatch(str.ToString(), numerics);
        }
        #endregion

        #region 扫描分析源程序代码，得出所有的单词符号
        /// <summary>
        /// 该方法用于开始扫描源程序，分析单词符号
        /// </summary>
        /// <param name="s">要扫描的字符串代码</param>
        /// <returns>返回分析结果</returns>
        public string  Analyze(string  s)
        { 
            /* 
             * 因';',')';'}';']'可能和其他可识别符号串联在一起使用，
             * 将读取的字符串先处理（将前面所说符号2边加上空格），
             * 再赋给sourceCode，方便后面分析处理
             */
            string x = Regex.Replace(s.Trim(), @"\;", " ; ");
            string y = Regex.Replace(x, @"\)", " ) ");
            string z = Regex.Replace(y, @"\]", " ] ");
            string z1 = Regex.Replace(z, @"\(", " ( ");
            sourceCode = Regex.Replace(z1, @"\}", " } ") + " ";

            char ch;   //当前分析的字符
            int line = 1;   //当前分析的行号
            output = "===============词法分析开始===============\r\n";
            
            /* 按字符顺序依次扫描源程序代码，直至扫描结束 */
            while (l <= (sourceCode.Length - 1) )
            {
                token = null;
                ch = getChar(l);
                while (ch == ' ') //过滤掉空格字符
                {
                    l++; 
                    if ( l > sourceCode.Length - 1 ) 
                        break;
                    ch = getChar( l);
                }

                if (isChar(ch)) //以字母开头
                {
                    add(ref token, ref l, ref ch);
                    while ( isChar(ch) || isNumber(ch) || ch == '_')
                    {
                        token += ch;
                        l++;
                        if ( l > sourceCode.Length - 1 )
                            break;
                        ch = getChar(l);
                    }

                    if (getChar(l - 1) == '_') //如果最后一个字符是"_",错误数加1，输出错误
                    {
                        errornum++;
                        output += (token + "    非法标识符    error" + errornum + "\r\n");
                        errAdd(token + "    非法标识符", line);
                        l++; 
                        ch = getChar(l);
                    }
                    else
                    {
                        bool isKey = Regex.IsMatch(token.ToString(), keyWords) ; //判断是否为关键字
                        switch (isKey)
                        {
                            case true: //保留字
                                output += line + "  保留字  " + token + "\r\n"; 
                                record(token, "key", line); 
                                break;
                            default: //标识符
                                output += (line + "  标识符  " + token + "\r\n");
                                record(token, "identifier", line);
                                break;
                        }                
                    }

                }
                else if (ch == '\n'|| ch == '\r') //表示换行，行数加1
                {
                    l++;
                    ch = getChar(l);
                    line++;
                }
                else if (isBlank(ch)) //空格或回车，tab，过滤
                {
                    l++; 
                    ch = getChar(l);
                }
                else if (isNumber(ch)) //以数字开头
                {
                    add(ref token, ref l, ref ch);
                    while (isNumber(ch)) //判断后面是否为数字
                    {
                        token += ch;
                        l++; 
                        if (l >= sourceCode.Length - 1)
                            break;
                        ch = getChar(l);
                    }

                    if (ch == '.') //如果碰到小数点
                    {
                        add(ref token, ref l, ref ch);
                        if (!isNumber(ch)) //若小数点后不是数字，有错误
                        {
                            errornum++;
                            output += (line + "  非法数字  " + token + "\r\n");
                            errAdd(token + "    非法数字", line);
                        }
                        else 
                        {
                            while (isNumber(ch)) //向后循环到不是数字为止
                            {
                                add(ref token, ref l, ref ch);
                            }

                            if (isChar(ch)) //如果后面是字母，有错误，将直到读到类似空格作用的字符，输出错误 
                            {
                                errornum++;

                                while (!isBlank(ch))
                                {
                                    add(ref token, ref l, ref ch);
                                }
                                output += (line + "  非法标识符  " + token + "\r\n");
                                errAdd(token + "    非法标识符", line);
                            }
                            else //real常量值  
                            {
                                output += (line + "  常量值  " + token + "\r\n");
                                record(token, "real", line);                        
                            }
                          }
                    }
                    else //如果读到不是数字的地方不是小数点
                    { 
                        if ((isChar(ch)))
                        {
                            errornum++;

                            while (!isBlank(ch))
                            {
                                add(ref token, ref l, ref ch);
                            }
                            output += (line + "  非法标识符  " + token + "\r\n");
                            errAdd(token + "    非法标识符", line);
                        }
                        else //int常量值                        
                        {
                            output += (line + "  常量值  " + token + "\r\n");
                            record(token, "int", line);
                        }
                    }
                }
                else if (ch == '<') //以 '<'开头
                {
                    add(ref token, ref l, ref ch);
                    switch (ch)
                    {
                        case '>': //如果是对应的'>',将'<>'识别为一个整体单词
                            l++; 
                            token += ch;
                            ch = getChar(l);
                            if ((isSeparator(ch) == true) || (isOperators(ch) == true)) //如果后面跟可识别的非字母或者数字符号，有错误
                            {
                                errornum++;
                                while (!isBlank(ch))
                                {
                                    add(ref token, ref l, ref ch);
                                }
                                output += (line + "  非法字符串  " + token + "\r\n");
                                errAdd(token + "    非法字符串", line);
                            }
                            else // "<>"运算符
                            {
                                output += (line + "  运算符  <>\r\n");
                                record("<>", "relateOpr", line);
                            }
                            break;

                         default: //如果后面不跟'<'
                             if (isChar(ch) || isBlank(ch) || isNumber(ch)) //若后面是字母或数字，空格，则当做一个单词符号"<"运算符
                             {
                                 output += (line + "  运算符  <\r\n");
                                 record("<", "relateOpr", line);
                             }
                             else
                             {
                                 errornum++;
                                 while (!isBlank(ch))
                                 {
                                     add(ref token, ref l, ref ch);
                                 }
                                 output += (line + "  非法字符串  " + token + "\r\n");
                                 errAdd(token + "    非法字符串", line);
                             }
                             break;
                    }
                }
                else if (ch == '=') //以 '='开头，因为会有"=="运算符存在，所以其处理情况同'<'处理类似
                {
                    add(ref token, ref l, ref ch);
                    switch (ch)
                    {
                        case '=':
                            token += '=';
                            l++;
                            ch = getChar(l);
                            if ((isSeparator(ch) == true) || (isOperators(ch) == true))
                            {
                                while (!isBlank(ch))
                                {
                                    add(ref token, ref l, ref ch);
                                }
                                errornum++;
                                output += (line + "  非法字符串  " + token + "\r\n");
                                errAdd(token + "    非法字符串", line);
                            }
                            else // "=="运算符
                            {
                                output += (line + "  运算符  ==\r\n");
                                record("==", "relateOpr", line);
                            }
                            break;

                        default:
                            if (isChar(ch) || isBlank(ch) || isNumber(ch)) // "="运算符
                            {
                                output += (line + "  运算符  =\r\n");
                                record("=", "evaluateOpr", line);
                            }
                            else
                            {
                                errornum++;
                                while (!isBlank(ch))
                                {
                                    add(ref token, ref l, ref ch);
                                }
                                output += (line + "  非法字符串  " + token + "\r\n");
                                errAdd(token + "    非法字符串", line);
                            }
                            break;
                    }
                }
                else if (ch == '/') // 以 '/'开头
                {
                    bool result = false; //设置 result 变量，用于作为后面的循环中的判断出口
                    add(ref token, ref l, ref ch);

                    switch (ch)
                    {
                        case '*':  //  "/*"为注释的开始，消除包含在后面的所有字符直到"*/"
                            while (result == false) //result为真作为出口
                            {
                                if (l >= sourceCode.Length - 1) //已到最后一个字符不可能再扫描到*/2个字符，注释错误，break出循环
                                {  
                                    errornum++;
                                    output += (line + "  注释错误  \r\n");
                                    errAdd("注释错误", line);
                                    break;
                                }
                                l++;
                                ch = getChar(l);

                                while (ch != '*') //扫描到第一个为'*'的字符，退出while
                                {
                                    l++;
                                    ch = getChar(l);
                                    if (ch == '\n'||ch == '\r')
                                    {
                                        line++;
                                    }
                                    if (l >= sourceCode.Length - 1)
                                    {
                                        errornum++;
                                        output += (line + "  注释错误  \r\n");
                                        errAdd("注释错误", line);
                                        break;
                                    }
                                }

                                if (l >= sourceCode.Length - 1) //若是因读取到最后一个字符而退出，退出第二个while循环 
                                    break;
                                l++;
                                ch = getChar(l);

                                if (ch == '/') //当扫描到‘*’且紧接着下一个字符为‘/’才是注释的结束
                                {
                                    l++;
                                    ch = getChar(l);
                                    result = true;
                                    break;

                                }
                                l--;
                            }
                            break;

                        default: //字符'/'后不是'*'，类似于'<'后不是'>'处理情况
                            if (isChar(ch) || isBlank(ch) || isNumber(ch))
                            {
                                output += (line + "  运算符  " + "/\r\n");
                                record("/", "arithmeticOpr", line);
                                break;
                            }
                            else
                            {
                                errornum++;

                                while (!isBlank(ch))
                                {
                                    add(ref token, ref l, ref ch);
                                }
                                output += (line + "  非法字符串  " + token + "\r\n");
                                errAdd(token + "    非法字符串", line);
                            }
                            break;
                    }
                }
                else if ((isSeparator(ch) == true) || (isOperators(ch) == true)) //以除'<','/','='以外的其它可识别的特殊符号开头
                {
                    char d = ch;
                    add(ref token, ref l, ref ch);

                    if ((isSeparator(ch) == true) || (isOperators(ch) == true)) //因双符号在一起的符号情况已处理，此时两个特殊符号在一起为错误
                    {
                        errornum++;

                        while (!isBlank(ch))
                        {
                            add(ref token, ref l, ref ch);
                        }
                        output += (line + "  非法字符串  " + token + "\r\n");
                        errAdd(token + "    非法字符串", line);
                    }
                    else //如果可识别特殊符号后面不是特殊符号，将特殊符号判断为其相应类型，后面的等待继续扫描
                    {
                        if (isSeparator(d) == true)
                        {
                            output += (line + "  分隔符  " + d.ToString() + "\r\n");
                        }
                        else
                        {
                            output += (line + "  运算符  " + d.ToString() + "\r\n");
                        }
                        record(d.ToString(), "arithmeticOpr", line);
                    }
                }
                else //最后，如果扫描到为不可识别的符号，将其作为非法符号输出，后面的等待继续扫描判断
                {
                    errornum++;
                    output += (line + "  非法符号  " + ch.ToString() + "\r\n");
                    errAdd(ch.ToString() + "    非法符号", line);
                    l++;
                }
            }

            /*扫描完毕，报告错误字符个数，给output赋值*/
            output += "===============词法分析完毕===============";
            
            if (errornum != 0)
            {
             //   MessageBox.Show("共有" + errornum + "个词法错误，\r\n" + "请改正后再试！\r\n", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return output;
        }
        #endregion

        /// <summary>
        /// 增加一个单词符号信息到tokens中
        /// </summary>
        /// <param name="token">单词字符串</param>
        /// <param name="type">单词类型</param>
        private void record(string token, string type,int line)
        {
            List<object> node = new List<object>(); ;
            node.Add(token);
            node.Add(type);
            node.Add(line);
            tokens.Add(node);
        }

        /// <summary>
        /// 该方法是为了方便在分析过程中连续同样的几步动作一起放在该函数里调用
        /// </summary>
        /// <param name="s">当前单词符号</param>
        /// <param name="i">当前字符位置</param>
        /// <param name="c">当期字符</param>
        private void add(ref string s, ref int i, ref char c)
        {
            s += c;
            i++;
            c = getChar(i);
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="info">信息说明</param>
        /// <param name="line">错误在源码中的行号</param>
        private void errAdd(string info, int line)
        {
            err e = new err(info, line.ToString());
            errlist.Add(e);
        }
    }
}