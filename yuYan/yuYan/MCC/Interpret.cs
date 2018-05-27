/*
 * 作者：黎国梁
 * 班级：三班
 * 学号：200632580088
 */
using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.ComponentModel;

using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;

namespace CMMCompiler
{
    /// <summary>
    /// Interpret类,处理中间代码,得到最终执行结果
    /// </summary>
    public  class Interpret
    {
        //定义一个INT型变量,用来指针4元式的行数
        int index = 0;
        string[][] midListCode = new string[4][];
        AccessTable AccessTable = new AccessTable();

        public string jieGuo = "";
       
        /// <summary>
        /// 实现输入和输出的功能,因为C#应用程序不支持控制台,此处加入调用控制台的程序集
        /// </summary>
        public class Win32
        {
            [DllImport("kernel32.dll")]
            public static extern Boolean AllocConsole();
            [DllImport("kernel32.dll")]
            public static extern Boolean FreeConsole();
        }        

        /// <summary>
        /// 处理一个4元式的list得到执行结果
        /// </summary>
        public void GetRun(string[][] a)//传入一个4元式的list
        {
            //打开控制台

            jieGuo = "";
         //   Win32.AllocConsole();
            midListCode = a;
            try
            {
                while (true)
                {
                    switch (midListCode[0][index])
                    {
                        case "ADDI": DoADDI(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; // +
                        case "SUBI": DoSUBI(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; // -
                        case "MULTI": DoMULTI(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; // *
                        case "DIVIDI": DoDIVIDI(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; //  /
                        case "LT": DoLT(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; // <
                        case "ET": DoET(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; // ==
                        case "UNET": DoUNET(ref midListCode[1][index], ref midListCode[2][index], ref midListCode[3][index]); break; // <>
                        case "READI": DoREADI(ref midListCode[1][index]); break; // read
                        case "WRITEI": DoWRITEI(ref midListCode[1][index]); break; // write
                        case "REALI": DoREALI(ref midListCode[1][index], ref midListCode[3][index]); break; //  将类型为int转换为real类型
                        case "ASSIGN": DoASSIGN(ref midListCode[1][index], ref midListCode[2][index]); break; //  =赋值
                        case "JUMP0": DoJUMPO(midListCode[1][index], midListCode[2][index]); break; // 条件跳转
                        case "JUMP": DoJUMP(midListCode[2][index]); break; // 无条件跳转
                        default: break;
                    }

                    index++;

                    if (index == midListCode[0].GetLength(0)) //如果读满最后一个4元式代表已经全部处理了中间代码,输出"编译成功"在控制台上
                    {
                         Write("编译成功！");
                        break;
                    }
                    else
                        if (index > midListCode[0].GetLength(0)) //如果指针index超出midListCode[0]的长度,强行跳出,避免指针出界
                            break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                 Write("编译成功！");
            }
             WriteLine("请按回车退出,并查看中间代码.");
             ReadLine();
         //   Win32.FreeConsole();
        }

        #region 处理四元式时做运算和判断操作的方法函数
        /// <summary>
        /// 做加法的运算,直接修改4元式list中的最后一项result的值
        /// </summary>
        private void DoADDI(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if( IfError(arg1,arg2,a,b))
                AccessTable.retSetValue(result, Convert.ToString(Convert.ToDouble(a)+Convert.ToDouble(b)));
        }

        /// <summary>
        /// 做减法的运算,直接修改4元式list中的最后一项result的值
        /// </summary>
        private void DoSUBI(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);
            string pattern1 = @"^[0-9]+$";
            string pattern2 = @"^[0-9]+\.[0-9]+$";
            string change = Convert.ToString(Convert.ToDouble(a) - Convert.ToDouble(b));
            
            if (!Regex.IsMatch(change, pattern1) && !Regex.IsMatch(change, pattern2))
            {
                change = "negitive";
                IfError(arg1, arg2, "_", change);
            }
            else
                AccessTable.retSetValue(result, change);
        }
        
        /// <summary>
        /// 做乘法的运算,直接修改4元式list中的最后一项result的值
        /// </summary>
        private void DoMULTI(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if (IfError(arg1, arg2, a, b))
                AccessTable.retSetValue(result, Convert.ToString(Convert.ToDouble(a) * Convert.ToDouble(b)));
        }

        /// <summary>
        /// 做除法的运算,直接修改4元式list中的最后一项result的值,其中arg2不可以为0
        /// </summary>
        private void DoDIVIDI(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if (Convert.ToInt32(b) == 0)
                b = "IsZero";
            if (IfError(arg1, arg2, a, b))
            {
                AccessTable.retSetValue(result, Convert.ToString(Convert.ToDouble(a) / Convert.ToDouble(b)));
            }
        }

        /// <summary>
        /// 做小于的判断,如果是左小于右,修改4元式list中的最后一项result的值为true,否则为false
        /// </summary>
        private void DoLT(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if (IfError(arg1, arg2, a, b))
            {
                if (Convert.ToDouble(a) < Convert.ToDouble(b))
                    AccessTable.retSetValue(result, "true");
                else
                    AccessTable.retSetValue(result, "false");
            }
        }


        /// <summary>
        /// 做等于的判断,如果左等于右,修改4元式list中的最后一项result的值为true,否则为false
        /// </summary>
        private void DoET(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if (IfError(arg1, arg2, a, b))
            {
                if (Convert.ToDouble(a) == Convert.ToDouble(b))
                    AccessTable.retSetValue(result, "true");
                else
                    AccessTable.retSetValue(result, "false");
            }
        }

        /// <summary>
        /// 做不等于的判断,如果左不等于右,修改4元式list中的最后一项result的值为true,否则为false
        /// </summary>
        private void DoUNET(ref string arg1, ref string arg2, ref string result)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if (IfError(arg1, arg2, a, b))
            {
                if (Convert.ToDouble(a) != Convert.ToDouble(b))
                    AccessTable.retSetValue(result, "true");
                else
                    AccessTable.retSetValue(result, "false");
            }
        }
        #endregion


        #region 处理四元式时做其它操作的方法函数
        /// <summary>
        /// 接受用户输入的一行数据,以回车结束,修改4元式list中的arg1的值为该数据
        /// </summary>
        private void DoREADI(ref string arg1)
        {
            string a = AccessTable.getValue(arg1);
            string msg = "";
            string pattern1 = @"^[0-9]+$";//为纯数字
            string pattern2 = @"^[0-9]+\.[0-9]+$";//为小数

            if (a == null || a == "")
               a = "_";
            if (IfError(arg1, "_", a, "_"))
            {
                 WriteLine("请输入一个数据,按回车结束：");
                 Write("> ");
                msg =  ReadLine();

                switch (AccessTable.getType(arg1))
                {
                    case "int":
                        while (!Regex.IsMatch(msg, pattern1))
                        {
                             WriteLine("您输入的不是int型,请重新输入:");
                             Write("> ");
                            msg =  ReadLine();
                       }
                        break;
                    case "real":
                        while (!Regex.IsMatch(msg, pattern2))
                        {
                             WriteLine("您输入的不是real型,请重新输入:");
                             Write("> ");
                            msg =  ReadLine();
                        }
                        break;
                }
                AccessTable.retSetValue(arg1, msg);
            }
        }

        /// <summary>
        /// 输出4元式list中的第2项值,不修改任何信息.
        /// </summary>
        private void DoWRITEI(ref string arg1)
        {
            string a = AccessTable.getValue(arg1);

            if (IfError(arg1, "_", a, "_"))
                 WriteLine(AccessTable.getValue(arg1));          
        }

        /// <summary>
        /// 把4元式list中的第2项参数由int类型转化为real类型,赋值给临时变量result
        /// </summary>
        private void DoREALI(ref string arg1,  ref string result)
        {
           string a = AccessTable.getValue(arg1);

            if (IfError(arg1, "_", a, "_"))
                AccessTable.retSetValue(result, AccessTable.getValue(arg1));
        }

        /// <summary>
        /// 赋值, 把临时变量result的值,赋值给4元式list中的第2项参数
        /// </summary>
        private void DoASSIGN(ref string arg1, ref string arg2)
        {
            string a = AccessTable.getValue(arg1);
            string b = AccessTable.getValue(arg2);

            if (a == null||a=="")
                a = "_";

            if (IfError(arg1, arg2, a, b))
            {
                string change = AccessTable.getValue(arg2);
                AccessTable.retSetValue(arg1, change);
            }
        }

        /// <summary>
        /// 条件跳转,如果arg1的值为 false,跳到标签为result的4元式list处
        /// </summary>
        public void DoJUMPO(string arg1, string result)
        {
            if (AccessTable.getValue(arg1) == "false")
                index = jumpIndex (result);
        }

        /// <summary>
        /// 无条件跳转,直接跳到标签为result的4元式list处
        /// </summary>
        private void DoJUMP(string result)
        {
            index = jumpIndex(result);
        }
        #endregion

        /// <summary>
        /// 移动INDEX到跳转到的标签处
        /// 在4元式第2项查找到跳转的标签result，返回index
        /// </summary>
        /// <param name="result">查找的标签result</param>
        /// <returns>返回index</returns>
        private int jumpIndex(string result)
        {
            for (int n = 0; n < midListCode[0].GetLength(0); n++)
            {
                if(midListCode[1][n]==result )
                    return n;           
            }
            return index ;
        }

        /// <summary>
        /// 此方法用来判断代码可能出现的错误
        /// </summary>
        /// <param name="arg1">第一个运算变量的名称</param>
        /// <param name="arg2">第二个运算变量的值</param>
        /// <param name="a">第一个运算变量的名称</param>
        /// <param name="b">第二个运算变量的值</param>
        /// <returns>返回是否出现错误</returns>
        private bool IfError(string arg1,string arg2, string a,string b)
        {
            switch (a)
            {
                case "OutSide":
                    ErrorMsg(arg1, 1); //如果arg1出界,屏幕打印错误信息
                    return false;
                case null:
                case "":
                    ErrorMsg(arg1, 3); //如果arg1没有值,屏幕打印错误信息
                    return false;
                default:
                    break;
            }
            switch (b)
            {
                case "OutSide":
                    ErrorMsg(arg2, 1); //如果arg1出界,屏幕打印错误信息
                    return false;
                case null:
                case "":
                    ErrorMsg(arg2, 3); //如果arg1没有值,屏幕打印错误信息
                    return false;
                case "IsZero":
                    ErrorMsg((arg1 + "/" + arg2), 2); //如果arg2为0,屏幕打印错误信息
                    return false;
                case "negitive":
                    ErrorMsg((arg1 + "-" + arg2), 4);
                    return false;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// 此方法在屏幕上打印编译错误时的代码,和类型
        /// </summary>
        /// <param name="Msg">错误代码的名称</param>
        /// <param name="Num">错误代码的类型</param>
        private void ErrorMsg(string Msg, int Num)
        {
            switch (Num)
            {
                case 1:
                     WriteLine("编译失败,错误类型1:");
                     WriteLine("中间代码第" + (index + 1) + "行:  " + Msg + "  访问数组出界");
                    break;
                case 2:
                     WriteLine("编译失败,错误类型2:");
                     WriteLine("中间代码第" + (index + 1) + "行:  " + Msg + "  除数不可以为0");
                    break;
                case 3:
                     WriteLine("编译失败,错误类型3:");
                     WriteLine("中间代码第" + (index + 1) + "行:  " + Msg + "  没有被赋值，不可使用");
                    break;
                case 4:
                     WriteLine("编译失败,错误类型4:");
                    // WriteLine("中间代码第" + (index + 1) + "行:  " + Msg + "  此处结果为负数");
                    break;
                default:
                    break;
            }

            index = midListCode[0].GetLength(0); //如果出错, 指针跳转到最后一行4元式
        }
          // WriteLine("您输入的不是real型,请重新输入:");
          //                   Write("> ");
          //                  msg =  ReadLine();
        //ReadLine
        // 
        void Write(string liu)
        {
            jieGuo += liu;
        }
        void WriteLine(string liu)
        {
            Write(liu + "\n");
        }
          String ReadLine()
        {
            return "\n";
        }

    }
}