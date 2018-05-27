/*
 * 作者：黎国梁
 * 班级：三班
 * 学号：200632580088
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections; 

namespace CMMCompiler
{
    /// <summary>
    /// 该类用于构造变量表和生成中间代码
    /// </summary>
    class MidCode
    {
        #region 声明全局变量
        //存储得到的下一个token值
        public string nextToken = null;

        //当前的分析的单词位置
        public int n;

        //用来标记是否用到real中间代码生成的临时变量
        public bool x = false;

        //标记第几个if ,while，用于label或jump标签的临时变量，方便后面跳转语句执行
        public int j = 0;

        //标记除if ,while的其他语句生成的临时变量，每生成一个，p++
        public int p = 0;

        //p1用来存储表达式中下一条中间代码的第一个参数
        public string p1 = null;

        //p2用来存储表达式中下一条中间代码的第二个参数
        public string p2 = null;

        //p2用来存储表达式中下一条中间代码的第三个参数
        public string p3 = null;

        //result用来存储表达式中所需用到的前面生成的中间代码的最后一个参数
        public string result = null;

        //存储对应IDtablelist中对象的标识符变量名
        public static List<object> IDlist = new List<object>();

        //存储标识符对象的表，对象中包含变量信息
        public static List<object> IDtablelist = new List<object>();

        //存储对应SZtablelist中对象的数组变量名
        public static List<object> SZlist = new List<object>();

        //存储数组对象的表，对象中包含数组信息
        public static List<object> SZtablelist = new List<object>();

        //存储对应TempIDtablelist 中对象的临时变量名
        public static List<object> TempIDlist = new List<object>();

        //存储临时变量对象的表，对象中包含临时变量的信息

        public static List<object> TempIDtablelist = new List<object>();

        //存储中间代码对象，对象中包含中间代码四元式的信息
        public List<object> MCodeList = new List<object>();

        //中间代码最后转化得到的2维数组，将作为参数传递到后面编译运行中间代码的类中
        public string[][] c = new string[4][];

        //将中间代码内容放在S中，用于在FORM中输出显示
        public string s = null;

        //分析类Analysis的一个实例
        public Analysis al;
        #endregion
        
        /// <summary>
        /// MidCode构造函数
        /// </summary>
        public MidCode()
        { }

        /// <summary>
        /// MidCode构造函数
        /// </summary>
        /// <param name="al">语法语义分析后得到的Analysis对象</param>
        public MidCode(Analysis al)
        {
            this.al = al;
        }
     
        /// <summary>
        /// 主方法，扫描生成中间语句，创建表
        /// </summary>
        public void Scan()
        {
            n = 0;
            nextToken = al.getToken(ref n);
            stmt_sequence();
            c[0] = new string[MCodeList.Count];
            c[1] = new string[MCodeList.Count];
            c[2] = new string[MCodeList.Count];
            c[3] = new string[MCodeList.Count];
            int v = 0;

            while (v < MCodeList.Count)
            {
                MCode m = (MCode)MCodeList[v];                
                s += m.func + " " + m.param1 + " " + m.param2 + " " + m.targ + "\r\n";

                //将List<object>转化为2维数组，存储在c中，以便后面编译运行的存取
                c[0][v] = m.func;
                c[1][v] = m.param1;
                c[2][v] = m.param2;
                c[3][v] = m.targ;
                v++;
            }
        }

        #region 处理if判断选择语句、while循环语句、read操作、write操作、赋值语句、声明定义语句等的方法函数
        /// <summary>
        /// 语句序列的处理
        /// </summary>
        private void stmt_sequence()
        {
            while (n < al.token.Length)
            {
                switch (nextToken)
                {
                    case "if":
                        if_stmt(); break;
                    case "while":
                        while_stmt(); break;
                    case "read":
                        read_stmt(); break;
                    case "write":
                        write_stmt(); break;
                    case "int":
                        intorreal_stmt(); break;
                    case "real":
                        intorreal_stmt(); break;
                    default:
                        if (al.format[n - 1] == "identifier") //经过语法语义分析，若第一个token为标识符，则一定为赋值语句
                        {
                            assign_stmt();
                            break;
                        }
                        else return;
                }
            }
        }

        /// <summary>
        /// 扫描到if语句时的处理，循环由标签和跳转语句完成
        /// </summary>
        private void if_stmt()
        {
            if (nextToken == "if")
            {
                int w = j;
                n++; j++;
                nextToken = al.getToken(ref n); //跳过"("
                con_stmt();
                n++; //跳过"{"
                MCodeList.Add(new MCode("JUMP0", result, "ELSE" + w, ""));
                nextToken = al.getToken(ref n);
                stmt_sequence();
                MCodeList.Add(new MCode("JUMP", "", "OUT" + w, ""));
                MCodeList.Add(new MCode("LABEL", "ELSE" + w, "", ""));
                nextToken = al.getToken(ref n);

                if (nextToken == "else")
                {
                    n++;
                    nextToken = al.getToken(ref n);
                    stmt_sequence();
                    nextToken = al.getToken(ref n);
                }
                MCodeList.Add(new MCode("LABEL", "OUT" + w, "", ""));

            }
        }

        /// <summary>
        /// 扫描到while语句时的处理，循环由标签和跳转语句完成
        /// </summary>
        private void while_stmt()
        {
            if (nextToken == "while")
            {
                int w = j;
                n++; j++;
                nextToken = al.getToken(ref n); //跳过"("
                MCodeList.Add(new MCode("LABEL", "IN" + w, "", ""));
                con_stmt();
                MCodeList.Add(new MCode("JUMP0", result, "OUT" + w, ""));
                n++; //跳过{ 
                nextToken = al.getToken(ref n);
                stmt_sequence();
                MCodeList.Add(new MCode("JUMP", "", "IN" + w, ""));
                MCodeList.Add(new MCode("LABEL", "OUT" + w, "", ""));

                nextToken = al.getToken(ref n);
            }
        }

        /// <summary>
        /// 扫描到read语句时的处理，后面是要用户输入值的变量
        /// </summary>
        private void read_stmt()
        {
            if (nextToken == "read")
            {
                nextToken = al.getToken(ref n);
                nextToken = al.getToken(ref n);
                result = nextToken;

                if (al.getToken(ref n) == "[")
                {
                    nextToken = al.getToken(ref n);
                    p1 = "SARG(" + result + "[" + nextToken + "])";
                    MCodeList.Add(new MCode("READI", p1, "", ""));
                    n = n + 2;
                    p1 = null;
                }
                else
                    MCodeList.Add(new MCode("READI", "VARG(" + result + ")", "", ""));
                nextToken = al.getToken(ref n);
                nextToken = al.getToken(ref n);
            }

        }

        /// <summary>
        /// 扫描到write语句时的处理，后面是要运算表达式
        /// </summary>
        private void write_stmt()
        {
            if (nextToken == "write")
            {
                nextToken = al.getToken(ref n);
                exp_stmt();
                MCodeList.Add(new MCode("WRITEI", result, "", ""));
                nextToken = al.getToken(ref n);
                result = null;
            }
        }

        /// <summary>
        /// 扫描到int或real时，后面是变量声明定义的语句
        /// </summary>
        private void intorreal_stmt()
        {
            string s1; string s2;
            if (nextToken == "int" || nextToken == "real")
            {
                s1 = nextToken;
                nextToken = al.getToken(ref n);
                s2 = nextToken;
                nextToken = al.getToken(ref n);

                if (nextToken == "[")
                {
                    if (!SZlist.Contains(s2))
                    {
                        nextToken = al.getToken(ref n);
                        int l = Int32.Parse(nextToken);
                        SZlist.Add(s2);
                        SZtable S = new SZtable(s2, s1, l);
                        SZtablelist.Add(S);
                        int d = 0;
                        while (d < l)
                        {
                            S.element.Add(null);
                            d++;
                        }
                        n = n + 2;
                    }
                    nextToken = al.getToken(ref n);
                }
                else
                {
                    if (!IDlist.Contains(s2))
                    {
                        IDlist.Add(s2);
                        IDtablelist.Add(new IDtable(s2, s1));
                    }

                    if (nextToken != ";")
                    {
                        n = n - 2;
                        nextToken = al.getToken(ref n);
                        assign_stmt();
                    }
                    else 
                        nextToken = al.getToken(ref n);
                }
            }
        }

        /// <summary>
        /// 扫描到标识符的时候，后面是变量赋值语句
        /// </summary>
        private void assign_stmt()
        {
            string tp;
            result = nextToken; tp = nextToken;
            nextToken = al.getToken(ref n);

            if (nextToken == "[")
            {
                Array();
                p1 = ("SARG(" + result + ")");
                tp = get_SZtype(tp);
            }
            else
            {
                p1 = "VARG(" + result + ")";
                tp = get_BStype(tp);

            }

            string s = p1;
            nextToken = al.getToken(ref n);
            if (tp == "real") x = true;
            exp_stmt(); x = false;
            p2 = result;
            p1 = s;
            MCodeList.Add(new MCode("ASSIGN", p1, p2, ""));
            nextToken = al.getToken(ref n);
        }
        #endregion

        #region 对运算表达式的处理以及数组处理的一些方法函数
        /// <summary>
        /// 表达式的处理，中间涉及到符号的优先级和临时变量的声明和存储问题，以递归形式实现
        /// </summary>
        private void exp_stmt()
        {
            string s1 = null;
            string s2 = null;
            string a = null;
            string[] t1;
            string[] t2;
            int i = 0;
            term_stmt();
            s2 += (result + " ");

            if (nextToken == "+" || nextToken == "-")
            {
                do
                {
                    s1 += (nextToken + " ");
                    nextToken = al.getToken(ref n);
                    term_stmt();
                    s2 += (result + " ");
                } while (nextToken == "+" || nextToken == "-");

                t1 = s1.Split(new char[] { ' ' });
                t2 = s2.Split(new char[] { ' ' });

                while (i < t1.Length - 1)
                {
                    switch (t1[i])
                    {
                        case "+": a = "ADDI"; break;
                        case "-": a = "SUBI"; break;
                    }

                    if (i == 0)
                    {
                        p1 = t2[0];
                        p2 = t2[1];
                    }
                    else
                    {
                        p1 = result;
                        p2 = t2[i + 1];
                    }

                    p3 = "TARG(t" + p + ")";
                    MCodeList.Add(new MCode(a, p1, p2, p3));
                    result = p3;
                    TempIDlist.Add("t" + p);
                    TempIDtablelist.Add(new TempIDtable("t" + p));
                    i++; 
                    p++;
                }
            }
        }

        /// <summary>
        /// term的实现与expression思想一致
        /// </summary>
        private void term_stmt()
        {
            string s3 = null;
            string s4 = null;
            string b = null;
            string[] t3;
            string[] t4;
            int k = 0;
            factor_stmt();
            s4 += (result + " ");

            if (nextToken == "*" || nextToken == "/")
            {
                do
                {
                    s3 += (nextToken + " ");
                    nextToken = al.getToken(ref n);
                    factor_stmt();
                    s4 += (result + " ");
                } while (nextToken == "*" || nextToken == "/");

                t3 = s3.Split(new char[] { ' ' });
                t4 = s4.Split(new char[] { ' ' });

                while (k < t3.Length - 1)
                {
                    switch (t3[k])
                    {
                        case "*": b = "MULTI"; break;
                        case "/": b = "DIVIDI"; break;
                    }

                    if (k == 0)
                    {
                        p1 = t4[0];
                        p2 = t4[1];
                    }
                    else
                    {
                        p1 = result;
                        p2 = t4[k + 1];
                    }

                    p3 = "TARG(t" + p + ")";
                    MCodeList.Add(new MCode(b, p1, p2, p3));
                    result = p3;
                    TempIDlist.Add("t" + p);
                    TempIDtablelist.Add(new TempIDtable("t" + p));
                    k++;
                    p++;
                }
            }
        }

        /// <summary>
        /// 对表达式因子的处理
        /// </summary>
        private void factor_stmt()
        {
            if (nextToken == "(")
            {
                nextToken = al.getToken(ref n);
                exp_stmt();
                nextToken = al.getToken(ref n);
            }
            else
            {
                switch (al.format[n - 1])
                {
                    case "int":
                        if (x == true)
                          iToReal(nextToken);
                        else 
                            result = nextToken;
                        nextToken = al.getToken(ref n);
                        break;
                    case "real":
                        result = nextToken;
                        nextToken = al.getToken(ref n);
                        break;
                    case "identifier":
                        result = nextToken;
                        nextToken = al.getToken(ref n);
                        if (nextToken == "[")
                        {
                            if (x == true)
                            {
                                nextToken = al.getToken(ref n);
                                szToReal(result, nextToken);
                                n++;
                                nextToken = al.getToken(ref n);
                            }
                            else
                            {
                                Array();
                                result = "SARG(" + result + ")";
                            }
                        }
                        else
                        {
                            if (x == true)
                            {
                                n=n-2; nextToken = al.getToken(ref n);
                                bsToReal(nextToken);
                                nextToken = al.getToken(ref n);
                            }
                            else result = "VARG(" + result + ")";
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 对数组的处理
        /// </summary>
        private void Array()
        {
            if (nextToken == "[")
            {
                nextToken = al.getToken(ref n);
                result += ("[" + nextToken + "]");
                n++;
                nextToken = al.getToken(ref n);
            }
        }
        #endregion

        #region 处理过程需要调用的其它方法函数
        /// <summary>
        /// 条件跳转的中间代码生成
        /// </summary>
        private void con_stmt()
        {
            x = true;
            exp_stmt();
            p1 = result;
            string e = nextToken;
            nextToken = al.getToken(ref n);
            exp_stmt();
            p2 = result;
            p3 = "TARG(b" + j + ")";
            result = p3;
            TempIDlist.Add("b" + j);
            TempIDtablelist.Add(new TempIDtable("b" + p, "string"));
            p++;

            switch (e)
            {
                case "<>": MCodeList.Add(new MCode("UNET", p1, p2, p3)); break;
                case "==": MCodeList.Add(new MCode("ET", p1, p2, p3)); break;
                case "<": MCodeList.Add(new MCode("LT", p1, p2, p3)); break;
            }

            x = false;
        }
        
        /// <summary>
        /// 得到标识符的类型
        /// </summary>
        /// <param name="s">标识符变量名</param>
        /// <returns>返回变量类型</returns>
        private string get_BStype(string s)
        {
            int r = IDlist.IndexOf(s);
            IDtable b = (IDtable)IDtablelist[r];
            return b.Type;
        }

        /// <summary>
        /// 得到数组类型
        /// </summary>
        /// <param name="s">数组名</param>
        /// <returns>返回数组类型</returns>
        private string get_SZtype(string s)
        {
            int r = SZlist.IndexOf(s);
            SZtable b = (SZtable)SZtablelist[r];
            return b.Type;

        }


        /// <summary>
        /// int型数字转化为 real型
        /// </summary>
        /// <param name="s">s为要转化为real的整数</param>
        private void iToReal(string s)
        {
            p3 = "TARG(t" + p + ")";
            MCodeList.Add(new MCode("REALI", s, "", p3));
            result = p3;
            TempIDlist.Add("t" + p);
            TempIDtablelist.Add(new TempIDtable("t" + p));
            p++;
        }
        

        /// <summary>
        /// 标志符类型若为int型，则转化为real
        /// </summary>
        /// <param name="s">要转化成real类型的标识符变量名</param>
        private void bsToReal(string s)
        {

            switch (get_BStype(s))
            {
                case "int":
                    p3 = "TARG(t" + p + ")";
                    MCodeList.Add(new MCode("REALI","VARG("+s+")" , "", p3));
                    result = p3;
                    TempIDlist.Add("t" + p);
                    TempIDtablelist.Add(new TempIDtable("t" + p));
                    p++; break;
                case "real": result = "VARG(" + s + ")"; break;
            }
        }

        /// <summary>
        ///  此方法用来给数组生成"REALI"的中间代码；数组元素若为int型，则转化为real
        /// </summary>
        /// <param name="s1">要转化为real类型的数组元素所在数组名</param>
        /// <param name="s2">数组下标</param>
        private void szToReal(string s1, string s2)
        {
            p3 = "TARG(t" + p + ")";
            switch (get_SZtype(s1))
            {
                case "int":
                    MCodeList.Add(new MCode("REALI", "SARG(" + s1 + "[" + s2 + "])", "", p3));
                    result = p3;
                    TempIDlist.Add("t" + p);
                    TempIDtablelist.Add(new TempIDtable("t" + p));
                    p++;
                    break;
                case "real":
                    result = "SARG(" + s1 + "[" + s2 + "])";
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 用于每次编译执行后，清理表中的信息
        /// </summary>
        public void clear()
        {
            IDlist.Clear();
            IDtablelist.Clear();
            SZlist.Clear();
            SZtablelist.Clear();
            TempIDlist.Clear();
            TempIDtablelist.Clear();
        }
    }

    /// <summary>
    /// 该类产生表示单个执行意思的中间代码
    /// </summary>
    class MCode
    {
        //要执行的操作
        public string func;

        //第一个操作数
        public string param1;

        //第二个操作数
        public string param2;

        //存放操作结果
        public string targ;

        /// <summary>
        /// MCode构造函数
        /// </summary>
        /// <param name="al">要执行的操作</param>
        /// <param name="p1">第一个操作数</param>
        /// <param name="p2">第二个操作数</param>
        /// <param name="p">存放操作结果</param>
        public MCode(string al, string p1, string p2, string p)
        {
            func = al;
            param1 = p1;
            param2 = p2;
            targ = p;
        }
    }
}