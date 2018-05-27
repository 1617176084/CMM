/*
 * 作者：黎国梁
 * 班级：三班
 * 学号：200632580088
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;



 
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CMMCompiler
{
    /// <summary>
    /// 该类是用作对词法分析得到单词的基础上再进行的语法和语义分析
    /// 可以说是整个编译器程序最重要的类之一
    /// </summary>
    class Analysis
    {
        #region 全局声明变量
        //保存一个单词
        public string next = null;

        //保存所有在词法分析里得到的单词
        public string[] token;

        //保存所有单词的类型
        public string[] format;

        //保存所有单词在源码中的位置
        public string[] line;

        //保存当前单词位置
        public int n;

        //保存分析所在层
        public int level = 1;

        //保存变量表
        public List<object> IDlist = new List<object>();
        public List<object> IDtablelist = new List<object>();

        //保存临时变量表
        public List<object> TempIDlist = new List<object>();
        public List<object> TempIDtablelist = new List<object>();

        //保存临时数组表
        public List<object> LZlist = new List<object>();
        public List<object> LZtablelist = new List<object>();
         
        //保存数组表
        public List<object> SZlist = new List<object>();
        public List<object> SZtablelist = new List<object>();

        //保存出错信息
        public List<object> errInfo = new List<object>();

        //保存临时的位置
        int tmp;
        #endregion
             /// <summary>
              /// 做语法和语义分析的方法函数
             /// </summary>
        /// <param name="la">该类是编译各个阶段都要用到的基本类，用作词法分析、语法分析和给其他类传递分析后的信息集</param>
             /// <returns></returns>
        public bool syntaxAalysis(LexicalAnalysis la)
        {
            this.clear();
            this.errInfo.Clear();
            int tokennum = la.tokens.Count;
            if (tokennum != 0)
            {
                List<object> li1 = new List<object>();
                string[] st1 = new string[tokennum + 1];
                st1[tokennum] = "";
                for (int a = 0; a < tokennum; a++)
                {
                    li1 = (List<object>)la.tokens[a];
                    st1[a] = li1[0].ToString();
                }
                this.token = st1;

                List<object> li2 = new List<object>();
                string[] st2 = new string[tokennum + 1];
                st2[tokennum] = "";
                for (int a = 0; a < tokennum; a++)
                {
                    li2 = (List<object>)la.tokens[a];
                    st2[a] = li2[1].ToString();
                }
                this.format = st2;

                List<object> li3 = new List<object>();
                string[] st3 = new string[tokennum + 1];
                st3[tokennum] = "";
                for (int a = 0; a < tokennum; a++)
                {
                    li3 = (List<object>)la.tokens[a];
                    st3[a] = li3[2].ToString();
                }
                this.line = st3;
            }
            if (!this.Scan())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 扫描单词，分析语法
        /// </summary>
        /// <returns>返回扫描是否正确结果</returns>
        public bool Scan()
        {
            n = 0;
            next = getToken(ref n);
            return isStmtSequence();
        }

        #region 扫描过程中语法分析判断的方法函数
        /// <summary>
        /// 判断是不是为expression
        /// </summary>
        private bool isExpression()
        {
            if (isTerm())
            {

                while (next == "+" || next == "-")
                {
                    next = getToken(ref n);
                    if (!isTerm())
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return false;

        }

        /// <summary>
        /// 判断是不是为term
        /// </summary>
        private bool isTerm()
        {
            if (isFactor())
            {
                while (next == "*" || next == "/")
                {
                    if (next == "/" && (getToken(ref n) == "0"))
                    {
                        errAdd("错误代码:  除数为0", getLine(n-1));
                        return false;
                    }
                    next = getToken(ref n);
                    if (next == ";") return true;
                    if (!isFactor())
                        return false;

                }
                return true;
            }
            else
                return false;

        }

        /// <summary>
        /// 判断是不是为facter :(expression)|int | identifier |real|array
        /// </summary>
        private bool isFactor()
        {
            if (next == "(")
            {
                next = getToken(ref n);
                if (isExpression())
                {
                    if (next == ")")
                    {
                        next = getToken(ref n);
                        return true;
                    }
                }
                return false;
            }

            else
            {
                switch (format[n - 1])
                {
                    case "int":
                        next = getToken(ref n);
                        return true;
                    case "real":
                        if (!isReal())
                        {
                            errAdd("错误代码:  real类型数字 与 int类型不匹配", getLine(n - 1));
                            return false;
                        }
                        next = getToken(ref n);
                        return true;
                    case "identifier":
                        if (SM(next, n))
                        {
                            errAdd("错误代码:  此变量未声明", getLine(n - 1));
                            return false;
                        }
                        if (!isType(n - 1))
                        {
                            errAdd("错误代码:  real与int类型不匹配", getLine(n - 1));
                            return false;
                        }
                        next = getToken(ref n);
                        if (next == "[")
                        {
                            if (!isArray())
                            {
                                errAdd("错误代码:  数组有错误", getLine(n - 1));
                                return false;
                            }
                            if (!SZdown(n - 5))
                            {
                                errAdd("错误代码:  数组越界", getLine(n - 1));
                                return false;
                            }
                            if (!isSZvalue(n - 5))
                            {
                                errAdd("错误代码:  数组下标所对应的变量未赋值", getLine(n - 1));
                                return false;
                            }
                        }
                        return true; ;
                }
                return false;
            }
        }

        /// <summary>
        /// 判断是不是数组 : identifier[int | identifier]
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isArray()
        {
            if (next == "[")
            {
                next = getToken(ref n);
                if (format[n - 1] == "int" || format[n - 1] == "identifier")
                {
                    next = getToken(ref n);
                    if (next == "]")
                    {
                        next = getToken(ref n);
                        return true;
                    }
                }
                else next = getToken(ref n);
            }
            return false;
        }

        /// <summary>
        /// 判断是不是条件expression comparison-op expression
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isCondition()
        {
            if (isExpression())
            {
                n = n - 2;
                next = getToken(ref n);
                toLS(next, n);
                try
                {
                    TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
                    LStmp.Type = "real";
                }
                catch
                {
                    SZtable SZtmp = (SZtable)SZtablelist[0];
                    SZtmp.Type = "real";
                }
                next = getToken(ref n);
                switch (next)
                {
                    case "<>":
                    case "==":
                    case "<":
                        next = getToken(ref n);

                        break;
                }
                if (isExpression())
                {
                    clsLS();
                    return true;
                }
            }
            errAdd("错误代码:  判断条件有错误", getLine(n - 1));
            return false;
        }

        /// <summary>
        /// 判读是否为赋值语句 identifier|array = expression 
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isAssignStmt()
        {

            if (format[n - 1] == "identifier")
            {
                if (!toLS(next, n + 2))
                {
                    errAdd("错误代码:  赋值变量未声明", getLine(n - 1));
                    return false;
                }
                next = getToken(ref n);
                if (next == "[")
                {
                    if (!isArray())
                    {
                        errAdd("错误代码:  数组有错误", getLine(n - 1));
                        return false;
                    }
                    if (!SZdown(n - 5))
                    {
                        errAdd("错误代码:  数组越界", getLine(n - 1));
                        return false;
                    }
                    if (!toSZvalue(n - 5))
                    {
                        errAdd("错误代码:  数组下标所对应的变量未赋值", getLine(n - 1));
                        return false;
                    }
                }
                if (next == "=")
                {
                    n = n - 2;
                    if (getToken(ref n) != "]")
                    {
                        if (!toValueSM(n - 1))
                            return false;
                    }
                    n++;
                    next = getToken(ref n);
                    if (isExpression())
                    {
                        if (next == ";")
                        {

                            clsLS();
                            clsLZ();
                            next = getToken(ref n);
                            return true;
                        }

                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否为语句
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isStatement()
        {
            switch (next)
            {
                case "if":
                    if (isIfStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的if语句", getLine(n - 1));
                        return false;
                    }
                case "while":
                    if (isWhileStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的while语句", getLine(n - 1));
                        return false;
                    }
                case "read":
                    if (isReadStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的read语句", getLine(n - 1));
                        return false;
                    }
                case "write":
                    if (isWriteStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的write语句", getLine(n - 1));
                        return false;
                    }
                case "int":
                    if (isIntOrRealStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的int声明语句", getLine(n - 1));
                        return false;
                    }
                case "real":
                    if (isIntOrRealStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的real声明语句", getLine(n - 1));
                        return false;
                    }
                default:
                    if (isAssignStmt()) return true;
                    else
                    {
                        errAdd("错误代码:  错误的赋值语句", getLine(n - 1));
                        return false;
                    }
            }
        }

        /// <summary>
        /// 判断是否为if条件语句 If-stmt:if (exp) {Stmt-sequence} [else {Stmt-sequence}]
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isIfStmt()
        {
            if (next == "if")
            {
                next = getToken(ref n);
                if (next == "(")
                {
                    next = getToken(ref n);
                    if (isCondition())
                    {
                        if (next == ")")
                        {
                            next = getToken(ref n);
                            if (next == "{")
                            {
                                level++;
                                next = getToken(ref n);
                                while (isStatement()) ;
                                if (next == "}")
                                {
                                    clsLevel(level);
                                    level--;
                                    next = getToken(ref n);
                                    if (next == "else")
                                    {
                                        next = getToken(ref n);
                                        if (next == "{")
                                        {
                                            level++;
                                            next = getToken(ref n);
                                            while (isStatement()) ;
                                            if (next == "}")
                                            {
                                                clsLevel(level);
                                                level--;
                                                next = getToken(ref n); return true;
                                            }
                                        }
                                        return false;
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否为while循环语句 while-stmt: while (condition) {Stmt-sequence }
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isWhileStmt()
        {
            if (next == "while")
            {
                next = getToken(ref n);
                if (next == "(")
                {
                    ;
                    next = getToken(ref n);
                    if (isCondition())
                    {
                        if (next == ")")
                        {

                            next = getToken(ref n);
                            if (next == "{")
                            {
                                level++;
                                next = getToken(ref n);
                                while (isStatement()) ;

                                if (next == "}")
                                {
                                    clsLevel(level);
                                    level--;

                                    next = getToken(ref n);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否是read操作 read-stmt := read identifier;
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isReadStmt()
        {
            if (next == "read")
            {
                next = getToken(ref n);

                if (next == "(")
                {
                    next = getToken(ref n);
                    if (format[n - 1] == "identifier")
                    {
                        if (SM(next, -1))
                        {
                            errAdd("错误代码:  此变量未声明", getLine(n - 1));
                            return false;
                        }
                        next = getToken(ref n);
                        if (next == "[")
                        {
                            if (!isArray())
                            {
                                errAdd("错误代码:  数组有错误", getLine(n - 1));
                                return false;
                            }

                            if (!SZdown(n - 5))
                            {
                                errAdd("错误代码:  数组越界", getLine(n - 1));
                                return false;
                            }
                            if (!toSZvalue(n - 5))
                            {
                                errAdd("错误代码:  数组下标所对应的变量未赋值", getLine(n - 1));
                                return false;
                            }
                        }

                        if (next == ")")
                        {
                            toValueRead(n - 2);
                            next = getToken(ref n);
                            if (next == ";")
                            {
                                next = getToken(ref n);
                                return true;
                            }
                            next = getToken(ref n);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否是write操作语句 write-stmt := write factor2;
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isWriteStmt()
        {
            if (next == "write")
            {
                next = getToken(ref n);
                if (isExpression())
                {
                    if (next == ";")
                    {
                        next = getToken(ref n);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否是声明定义语句
        /// int-stmt := int identifier | int assign-stmt
        /// real-stmt := real identifier | real assign-stmt
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isIntOrRealStmt()
        {
            if (next == "int" || next == "real")
            {
                next = getToken(ref n);
                if (format[n - 1] == "identifier")
                {
                    if (!SM1(next, n - 2))
                    {
                        errAdd("错误代码:  此变量已声明", getLine(n - 1));
                        return false;
                    }
                    next = getToken(ref n);
                    if (next == "[")
                    {
                        if (!isArray())
                        {
                            errAdd("错误代码:  数组有错误", getLine(n - 1));
                            return false;
                        }
                        if (next == "=") return false;
                    }
                    if (next == "=")
                    {
                        toValueSM(n - 2);
                        next = getToken(ref n);
                        if (!isExpression()) return false;
                    }
                    if (next == ";")
                    {
                        try
                        {
                            toBS();
                        }
                        catch
                        {
                            try
                            {
                                toSZ();
                            }
                            catch
                            { return false; }
                        }

                        next = getToken(ref n);
                        return true;
                    }

                }
                return false;
            }
            return false;
        }

        // Stmt-sequence :=  statement { statement}
        /// <summary>
        /// 判断语句是否表达正确
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isStmtSequence()
        {
            do
            {
                if (!isStatement())
                {
                    return false;                 
                }
            } while (n < format.Length - 1);
            return true;
        }
        #endregion

        #region 语法分析过程需要用到的其他方法函数
        /// <summary>
        /// 清空之前可能分析过的保存信息
        /// </summary>
        public void clear()
        {
            IDlist.Clear();
            IDtablelist.Clear();
            TempIDlist.Clear();
            TempIDtablelist.Clear();
            LZlist.Clear();
            LZtablelist.Clear();
            SZlist.Clear();
            SZtablelist.Clear();
        }

        /// <summary>
        /// 变量是否声明过(声明过为false)——声明语句中用
        /// </summary>
        /// <param name="next">要判断的变量</param>
        /// <param name="n">变量所在位置</param>
        /// <returns>返回判断结果</returns>
        private bool SM1(string next, int n)
        {
            if (IDlist.Contains(next) || SZlist.Contains(next))
                return false;
            else
            {
                n = n + 2;
                if (getToken(ref n) == "[")
                {
                    LZlist.Add(next);
                    tmp = n - 3;
                    try
                    {
                        LZtablelist.Add(new TempSZtable(next, getToken(ref tmp), Convert.ToInt32(getToken(ref n)), level));
                    }
                    catch { return false; }
                    return true;
                }
                else
                {
                    TempIDlist.Add(next);
                    n = n - 3;
                    TempIDtablelist.Add(new TempIDtable(next, getToken(ref n), level));
                    return true;
                }
            }
        }

        /// <summary>
        /// 变量是否声明过(声明过为false)
        /// </summary>
        /// <param name="next">要判断的变量</param>
        /// <param name="n">变量所在位置</param>
        /// <returns>返回判断结果</returns>
        private bool SM(string next, int n)
        {
            if (IDlist.Contains(next) || SZlist.Contains(next))
            {
                if (IDlist.Contains(next))
                {
                    int indexID = IDlist.IndexOf(next);
                    IDtable BStmp = (IDtable)IDtablelist[indexID];

                    if (n == -1)
                        return false;
                    if (BStmp.Valued == false)
                        return true;
                    return false;
                }
                else
                {
                    int indexSZ = SZlist.IndexOf(next);
                    SZtable SZtmp = (SZtable)SZtablelist[indexSZ];

                    if (n == -1)
                        return false;

                    return false;
                }
            }
            else
            {
                if (n == -1)
                    return true;
                n = n + 2;

                if (getToken(ref n) == "[")
                {
                    LZlist.Add(next);
                    tmp = n - 3;

                    try
                    {
                        LZtablelist.Add(new TempSZtable(next, getToken(ref tmp), Convert.ToInt32(getToken(ref n)), level));
                    }
                    catch
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    TempIDlist.Add(next);
                    n = n - 3;
                    TempIDtablelist.Add(new TempIDtable(next, getToken(ref n), level));
                    return true;
                }
            }
        }

        /// <summary>
        /// 赋值语句中，检查被赋值变量是否声明(声明过为true，并将其放入临时变量表中)
        /// </summary>
        /// <param name="next">要判断的变量</param>
        /// <param name="n">变量所在位置</param>
        /// <returns>返回判断结果</returns>
        private bool toLS(string next, int n)
        {
            if (!IDlist.Contains(next) && !SZlist.Contains(next))
            {
                return false;
            }
            else
            {
                if (IDlist.Contains(next))
                {
                    int indexID = IDlist.IndexOf(next);
                    IDtable BStmp = (IDtable)IDtablelist[indexID];
                    TempIDlist.Add(BStmp.Name);
                    TempIDtablelist.Add(new TempIDtable(BStmp.Name, BStmp.Type, BStmp.Level, BStmp.Valued));
                    return true;
                }
                else
                {
                    int indexSZ = SZlist.IndexOf(next);
                    SZtable SZtmp = (SZtable)SZtablelist[indexSZ];
                    LZlist.Add(SZtmp.Name);
                    n--;
                    LZtablelist.Add(new TempSZtable(SZtmp.Name, SZtmp.Type, SZtmp.Length, SZtmp.Level));
                    return true;
                }
            }
        }

        /// <summary>
        /// 清理临时变量表中的成员
        /// </summary>
        private void clsLS()
        {
            TempIDlist.Clear();
            TempIDtablelist.Clear();
        }

        /// <summary>
        /// 清理临时变量表中的成员
        /// </summary>
        private void clsLZ()
        {
            LZlist.Clear();
            LZtablelist.Clear();
        }

        /// <summary>
        /// 把临时变量表中的成员加入到IDtable中
        /// </summary>
        private void toBS()
        {
            IDlist.Add(TempIDlist[0]);
            TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
            IDtablelist.Add(new IDtable(LStmp.Name, LStmp.Type, LStmp.Level, LStmp.Valued));
            clsLS();

        }

        /// <summary>
        /// 把临时数组表中的成员加入到SZtable中
        /// </summary>
        private void toSZ()
        {

            SZlist.Add(LZlist[0]);
            TempSZtable LZtmp = (TempSZtable)LZtablelist[0];

            if (LZtmp.Value.Count == 0)
            {
                SZtablelist.Add(new SZtable(LZtmp.Name, LZtmp.Type, LZtmp.Length, LZtmp.Level));
            }
            else
            {
                SZtablelist.Add(new SZtable(LZtmp.Name, LZtmp.Type, LZtmp.Length, LZtmp.Level, Convert.ToInt32(LZtmp.Value[0])));
            }

            clsLZ();

        }

        /// <summary>
        /// 判断临时变量表中的成员是不是real类型(如果是real，返回true)
        /// </summary>
        /// <returns>返回判断结果</returns>
        private bool isReal()
        {
            try
            {
                TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];

                if (LStmp.Type != "real")
                {
                    return false;
                }
            }
            catch
            {
                try
                {
                    TempSZtable LZtmp = (TempSZtable)LZtablelist[0];

                    if (LZtmp.Type != "real")
                    {
                        return false;
                    }
                }
                catch
                {
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// 等号两边的变量类型是否相同(相同为true)(等号左边为real时，肯定为true)
        /// </summary>
        /// <param name="next">要判断的变量</param>
        /// <param name="n">变量所在位置</param>
        /// <returns>返回判断结果</returns>
        private bool isType(int n)
        {
            if (TempIDlist.Count == 0 && LZlist.Count == 0)
                return true;
            try //变量——变量
            {
                TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
                int indexID = IDlist.IndexOf(getToken(ref n));
                IDtable BStmp = (IDtable)IDtablelist[indexID];

                if (LStmp.Type == BStmp.Type)
                    return true;
                else if (LStmp.Type == "real")
                    return true;
            }
            catch
            {
                try //数组——数组
                {
                    n--;
                    TempSZtable LZtmp = (TempSZtable)LZtablelist[0];
                    int indexSZ = SZlist.IndexOf(getToken(ref n));
                    SZtable SZtmp = (SZtable)SZtablelist[indexSZ];

                    if (LZtmp.Type == SZtmp.Type)
                        return true;
                    else if (LZtmp.Type == "real")
                        return true;
                }
                catch
                {
                    try //变量——数组
                    {
                        n--;
                        TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
                        int indexSZ = SZlist.IndexOf(getToken(ref n));
                        SZtable SZtmp = (SZtable)SZtablelist[indexSZ];

                        if (LStmp.Type == SZtmp.Type)
                            return true;
                        else if (LStmp.Type == "real")
                            return true;
                    }
                    catch
                    {
                        try //数组——变量
                        {
                            n++;
                            TempSZtable LZtmp = (TempSZtable)LZtablelist[0];
                            int indexID = IDlist.IndexOf(getToken(ref n));
                            IDtable BStmp = (IDtable)IDtablelist[indexID];

                            if (LZtmp.Type == BStmp.Type)
                                return true;
                            else if (LZtmp.Type == "real")
                                return true;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 给变量赋值的时候，将其在变量表中的valued改为true
        /// </summary>
        private void toValue()
        {
            TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
            LStmp.Valued = true;
        }

        /// <summary>
        /// 给变量赋值的时候，将其在变量表中的valued改为true
        /// </summary>
        private bool toValueSM(int n)
        {
            string next = getToken(ref n);

            try
            {
                int index = IDlist.IndexOf(next);
                IDtable tmpBS = (IDtable)IDtablelist[index];
                tmpBS.Valued = true;
                return true;
            }
            catch
            {
                try
                {
                    int index = TempIDlist.IndexOf(next);
                    TempIDtable tmpLS = (TempIDtable)TempIDtablelist[index];
                    tmpLS.Valued = true;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 给数组某个下标变量赋值的时候，将其下标添加进在数组表中对应的valued
        /// </summary>
        /// <param name="next">要判断的变量</param>
        /// <param name="n">变量所在位置</param>
        /// <returns>返回判断结果</returns>
        private bool toSZvalue(int n)
        {
            tmp = n + 2;
            string next = getToken(ref n);
            int index = SZlist.IndexOf(next);
            SZtable tmpSZ = (SZtable)SZtablelist[index];
            string next1 = getToken(ref tmp);

            try
            {
                tmpSZ.Value.Add(Convert.ToInt32(next1));
                return true;
            }
            catch
            {
                int index1 = IDlist.IndexOf(next1);
                IDtable BStmp = (IDtable)IDtablelist[index1];
                if (BStmp.Type == "real")
                {
                    errAdd("错误代码:  数组下标不为整型", getLine(n - 1));
                    return false;
                }
                if (BStmp.Valued == false)
                {
                    errAdd("错误代码:  数组下标未赋值", getLine(n - 1));
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// read时，将read的变量的valued改为true
        /// </summary>
        /// <param name="n">变量所在位置</param>
        private void toValueRead(int n)
        {
            string next = getToken(ref n);

            try
            {
                if (IDlist.Contains(next))
                {
                    int indexID = IDlist.IndexOf(next);
                    IDtable BStmp = (IDtable)IDtablelist[indexID];
                    BStmp.Valued = true;
                }
                else
                {
                    int indexSZ = SZlist.IndexOf(next);
                    SZtable SZtmp = (SZtable)SZtablelist[indexSZ];
                    n++;
                    SZtmp.Value.Add(Convert.ToInt32(getToken(ref n)));
                }
            }
            catch
            {
                n = n - 4;
                next = getToken(ref n);
                int indexSZ = SZlist.IndexOf(next);
                SZtable SZtmp = (SZtable)SZtablelist[indexSZ];
                n++;

                try
                {
                    int next1 = Convert.ToInt32(getToken(ref n));
                    if (!SZtablelist.Contains(next1))
                    {
                        SZtmp.Value.Add(next1);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 退出嵌套时，删除嵌套内声明的变量和数组
        /// </summary>
        /// <param name="level">当前所在层</param>
        private void clsLevel(int level)
        {
            int b = IDtablelist.Count;
            int z = SZtablelist.Count;
            for (int index = 0; index < b; index++)
            {
                IDtable tmpBS = (IDtable)IDtablelist[index];

                if (tmpBS.Level == level)
                {
                    IDtablelist.RemoveAt(index);
                    IDlist.RemoveAt(index);
                    b--;
                }
            }
            for (int index = 0; index < z; index++)
            {
                SZtable tmpSZ = (SZtable)SZtablelist[index];

                if (tmpSZ.Level == level)
                {
                    SZtablelist.RemoveAt(index);
                    SZlist.RemoveAt(index);
                    z--;
                }
            }
        }

        /// <summary>
        /// 是否超出数组下界
        /// </summary>
        /// <param name="n">要判断的位置</param>
        /// <returns>返回判读结果</returns>
        private bool SZdown(int n)
        {
            if (SZlist.Contains(getToken(ref n)))
            {
                n--;
                int index = SZlist.IndexOf(getToken(ref n));
                SZtable SZtmp = (SZtable)SZtablelist[index];
                n++;

                try
                {
                    if (SZtmp.Length <= Convert.ToInt32(getToken(ref n)))
                        return false;
                    else
                        return true;
                }
                catch
                {
                    n--;
                    string next = getToken(ref n);
                    if (IDlist.Contains(next))
                    {
                        return true;
                    }
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 数组下标所对应的元素是否已经赋值
        /// </summary>
        /// <param name="n">要判断的位置</param>
        /// <returns>返回判读结果</returns>
        private bool isSZvalue(int n)
        {
            tmp = n + 2;
            string next = getToken(ref n);

            try
            {
                int index = Convert.ToInt32(getToken(ref tmp));
                int SZindex = SZlist.IndexOf(next);
                SZtable SZtmp = (SZtable)SZtablelist[SZindex];
                return SZtmp.Value.Contains(index);
            }
            catch
            {
                tmp = tmp - 1;
                string index = getToken(ref tmp);

                if (IDlist.Contains(index))
                {
                    int index1 = IDlist.IndexOf(index);
                    IDtable BStmp = (IDtable)IDtablelist[index1];
                    if (BStmp.Valued == true)
                        return true;
                    else
                        return false;
                }

                return false;
            }
        }

        /// <summary>
        /// 取单词函数
        /// </summary>
        /// <param name="n">单词所在位置</param>
        /// <returns>返回得到的单词</returns>
        public string getToken(ref int n)
        {
            if (n < token.Length)
            {
                string s = token[n];
                n++;
                return s;
            }
            return null;
        }

        /// <summary>
        /// 取单词所在源码的行号
        /// </summary>
        /// <param name="n">单词所在位置</param>
        /// <returns>返回行号数</returns>
        private string getLine(int n)
        {
            if (n < token.Length)
            {
                string s = line[n];
                return s;
            }
            return null;
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="info">信息说明</param>
        /// <param name="line">错误在源码中的行号</param>
        private void errAdd(string info, string line)
        {
            err e = new err(info, line);
            errInfo.Add(e);
        }
        #endregion
    }

    /// <summary>
    /// err类用于在分析过程中发现错误的时候
    /// </summary>
    class err
    {
        //错误信息
        public string info;

        //错误在源码中的行号
        public string line;

        /// <summary>
        /// err类的构造函数
        /// </summary>
        /// <param name="info">信息说明</param>
        /// <param name="line">错误在源码中的行号</param>
        public err(string info, string line)
        {
            this.info = info;
            this.line = line;
        }
    }
}