/*
 * ���ߣ������
 * �༶������
 * ѧ�ţ�200632580088
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
    /// �����������Դʷ������õ����ʵĻ������ٽ��е��﷨���������
    /// ����˵��������������������Ҫ����֮һ
    /// </summary>
    class Analysis
    {
        #region ȫ����������
        //����һ������
        public string next = null;

        //���������ڴʷ�������õ��ĵ���
        public string[] token;

        //�������е��ʵ�����
        public string[] format;

        //�������е�����Դ���е�λ��
        public string[] line;

        //���浱ǰ����λ��
        public int n;

        //����������ڲ�
        public int level = 1;

        //���������
        public List<object> IDlist = new List<object>();
        public List<object> IDtablelist = new List<object>();

        //������ʱ������
        public List<object> TempIDlist = new List<object>();
        public List<object> TempIDtablelist = new List<object>();

        //������ʱ�����
        public List<object> LZlist = new List<object>();
        public List<object> LZtablelist = new List<object>();
         
        //���������
        public List<object> SZlist = new List<object>();
        public List<object> SZtablelist = new List<object>();

        //���������Ϣ
        public List<object> errInfo = new List<object>();

        //������ʱ��λ��
        int tmp;
        #endregion
             /// <summary>
              /// ���﷨����������ķ�������
             /// </summary>
        /// <param name="la">�����Ǳ�������׶ζ�Ҫ�õ��Ļ����࣬�����ʷ��������﷨�����͸������ഫ�ݷ��������Ϣ��</param>
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
        /// ɨ�赥�ʣ������﷨
        /// </summary>
        /// <returns>����ɨ���Ƿ���ȷ���</returns>
        public bool Scan()
        {
            n = 0;
            next = getToken(ref n);
            return isStmtSequence();
        }

        #region ɨ��������﷨�����жϵķ�������
        /// <summary>
        /// �ж��ǲ���Ϊexpression
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
        /// �ж��ǲ���Ϊterm
        /// </summary>
        private bool isTerm()
        {
            if (isFactor())
            {
                while (next == "*" || next == "/")
                {
                    if (next == "/" && (getToken(ref n) == "0"))
                    {
                        errAdd("�������:  ����Ϊ0", getLine(n-1));
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
        /// �ж��ǲ���Ϊfacter :(expression)|int | identifier |real|array
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
                            errAdd("�������:  real�������� �� int���Ͳ�ƥ��", getLine(n - 1));
                            return false;
                        }
                        next = getToken(ref n);
                        return true;
                    case "identifier":
                        if (SM(next, n))
                        {
                            errAdd("�������:  �˱���δ����", getLine(n - 1));
                            return false;
                        }
                        if (!isType(n - 1))
                        {
                            errAdd("�������:  real��int���Ͳ�ƥ��", getLine(n - 1));
                            return false;
                        }
                        next = getToken(ref n);
                        if (next == "[")
                        {
                            if (!isArray())
                            {
                                errAdd("�������:  �����д���", getLine(n - 1));
                                return false;
                            }
                            if (!SZdown(n - 5))
                            {
                                errAdd("�������:  ����Խ��", getLine(n - 1));
                                return false;
                            }
                            if (!isSZvalue(n - 5))
                            {
                                errAdd("�������:  �����±�����Ӧ�ı���δ��ֵ", getLine(n - 1));
                                return false;
                            }
                        }
                        return true; ;
                }
                return false;
            }
        }

        /// <summary>
        /// �ж��ǲ������� : identifier[int | identifier]
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
        /// �ж��ǲ�������expression comparison-op expression
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
            errAdd("�������:  �ж������д���", getLine(n - 1));
            return false;
        }

        /// <summary>
        /// �ж��Ƿ�Ϊ��ֵ��� identifier|array = expression 
        /// </summary>
        /// <returns>�����жϽ��</returns>
        private bool isAssignStmt()
        {

            if (format[n - 1] == "identifier")
            {
                if (!toLS(next, n + 2))
                {
                    errAdd("�������:  ��ֵ����δ����", getLine(n - 1));
                    return false;
                }
                next = getToken(ref n);
                if (next == "[")
                {
                    if (!isArray())
                    {
                        errAdd("�������:  �����д���", getLine(n - 1));
                        return false;
                    }
                    if (!SZdown(n - 5))
                    {
                        errAdd("�������:  ����Խ��", getLine(n - 1));
                        return false;
                    }
                    if (!toSZvalue(n - 5))
                    {
                        errAdd("�������:  �����±�����Ӧ�ı���δ��ֵ", getLine(n - 1));
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
        /// �ж��Ƿ�Ϊ���
        /// </summary>
        /// <returns>�����жϽ��</returns>
        private bool isStatement()
        {
            switch (next)
            {
                case "if":
                    if (isIfStmt()) return true;
                    else
                    {
                        errAdd("�������:  �����if���", getLine(n - 1));
                        return false;
                    }
                case "while":
                    if (isWhileStmt()) return true;
                    else
                    {
                        errAdd("�������:  �����while���", getLine(n - 1));
                        return false;
                    }
                case "read":
                    if (isReadStmt()) return true;
                    else
                    {
                        errAdd("�������:  �����read���", getLine(n - 1));
                        return false;
                    }
                case "write":
                    if (isWriteStmt()) return true;
                    else
                    {
                        errAdd("�������:  �����write���", getLine(n - 1));
                        return false;
                    }
                case "int":
                    if (isIntOrRealStmt()) return true;
                    else
                    {
                        errAdd("�������:  �����int�������", getLine(n - 1));
                        return false;
                    }
                case "real":
                    if (isIntOrRealStmt()) return true;
                    else
                    {
                        errAdd("�������:  �����real�������", getLine(n - 1));
                        return false;
                    }
                default:
                    if (isAssignStmt()) return true;
                    else
                    {
                        errAdd("�������:  ����ĸ�ֵ���", getLine(n - 1));
                        return false;
                    }
            }
        }

        /// <summary>
        /// �ж��Ƿ�Ϊif������� If-stmt:if (exp) {Stmt-sequence} [else {Stmt-sequence}]
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
        /// �ж��Ƿ�Ϊwhileѭ����� while-stmt: while (condition) {Stmt-sequence }
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
        /// �ж��Ƿ���read���� read-stmt := read identifier;
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
                            errAdd("�������:  �˱���δ����", getLine(n - 1));
                            return false;
                        }
                        next = getToken(ref n);
                        if (next == "[")
                        {
                            if (!isArray())
                            {
                                errAdd("�������:  �����д���", getLine(n - 1));
                                return false;
                            }

                            if (!SZdown(n - 5))
                            {
                                errAdd("�������:  ����Խ��", getLine(n - 1));
                                return false;
                            }
                            if (!toSZvalue(n - 5))
                            {
                                errAdd("�������:  �����±�����Ӧ�ı���δ��ֵ", getLine(n - 1));
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
        /// �ж��Ƿ���write������� write-stmt := write factor2;
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
        /// �ж��Ƿ��������������
        /// int-stmt := int identifier | int assign-stmt
        /// real-stmt := real identifier | real assign-stmt
        /// </summary>
        /// <returns>�����жϽ��</returns>
        private bool isIntOrRealStmt()
        {
            if (next == "int" || next == "real")
            {
                next = getToken(ref n);
                if (format[n - 1] == "identifier")
                {
                    if (!SM1(next, n - 2))
                    {
                        errAdd("�������:  �˱���������", getLine(n - 1));
                        return false;
                    }
                    next = getToken(ref n);
                    if (next == "[")
                    {
                        if (!isArray())
                        {
                            errAdd("�������:  �����д���", getLine(n - 1));
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
        /// �ж�����Ƿ�����ȷ
        /// </summary>
        /// <returns>�����жϽ��</returns>
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

        #region �﷨����������Ҫ�õ���������������
        /// <summary>
        /// ���֮ǰ���ܷ������ı�����Ϣ
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
        /// �����Ƿ�������(������Ϊfalse)���������������
        /// </summary>
        /// <param name="next">Ҫ�жϵı���</param>
        /// <param name="n">��������λ��</param>
        /// <returns>�����жϽ��</returns>
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
        /// �����Ƿ�������(������Ϊfalse)
        /// </summary>
        /// <param name="next">Ҫ�жϵı���</param>
        /// <param name="n">��������λ��</param>
        /// <returns>�����жϽ��</returns>
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
        /// ��ֵ����У���鱻��ֵ�����Ƿ�����(������Ϊtrue�������������ʱ��������)
        /// </summary>
        /// <param name="next">Ҫ�жϵı���</param>
        /// <param name="n">��������λ��</param>
        /// <returns>�����жϽ��</returns>
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
        /// ������ʱ�������еĳ�Ա
        /// </summary>
        private void clsLS()
        {
            TempIDlist.Clear();
            TempIDtablelist.Clear();
        }

        /// <summary>
        /// ������ʱ�������еĳ�Ա
        /// </summary>
        private void clsLZ()
        {
            LZlist.Clear();
            LZtablelist.Clear();
        }

        /// <summary>
        /// ����ʱ�������еĳ�Ա���뵽IDtable��
        /// </summary>
        private void toBS()
        {
            IDlist.Add(TempIDlist[0]);
            TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
            IDtablelist.Add(new IDtable(LStmp.Name, LStmp.Type, LStmp.Level, LStmp.Valued));
            clsLS();

        }

        /// <summary>
        /// ����ʱ������еĳ�Ա���뵽SZtable��
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
        /// �ж���ʱ�������еĳ�Ա�ǲ���real����(�����real������true)
        /// </summary>
        /// <returns>�����жϽ��</returns>
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
        /// �Ⱥ����ߵı��������Ƿ���ͬ(��ͬΪtrue)(�Ⱥ����Ϊrealʱ���϶�Ϊtrue)
        /// </summary>
        /// <param name="next">Ҫ�жϵı���</param>
        /// <param name="n">��������λ��</param>
        /// <returns>�����жϽ��</returns>
        private bool isType(int n)
        {
            if (TempIDlist.Count == 0 && LZlist.Count == 0)
                return true;
            try //������������
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
                try //���顪������
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
                    try //������������
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
                        try //���顪������
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
        /// ��������ֵ��ʱ�򣬽����ڱ������е�valued��Ϊtrue
        /// </summary>
        private void toValue()
        {
            TempIDtable LStmp = (TempIDtable)TempIDtablelist[0];
            LStmp.Valued = true;
        }

        /// <summary>
        /// ��������ֵ��ʱ�򣬽����ڱ������е�valued��Ϊtrue
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
        /// ������ĳ���±������ֵ��ʱ�򣬽����±���ӽ���������ж�Ӧ��valued
        /// </summary>
        /// <param name="next">Ҫ�жϵı���</param>
        /// <param name="n">��������λ��</param>
        /// <returns>�����жϽ��</returns>
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
                    errAdd("�������:  �����±겻Ϊ����", getLine(n - 1));
                    return false;
                }
                if (BStmp.Valued == false)
                {
                    errAdd("�������:  �����±�δ��ֵ", getLine(n - 1));
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// readʱ����read�ı�����valued��Ϊtrue
        /// </summary>
        /// <param name="n">��������λ��</param>
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
        /// �˳�Ƕ��ʱ��ɾ��Ƕ���������ı���������
        /// </summary>
        /// <param name="level">��ǰ���ڲ�</param>
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
        /// �Ƿ񳬳������½�
        /// </summary>
        /// <param name="n">Ҫ�жϵ�λ��</param>
        /// <returns>�����ж����</returns>
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
        /// �����±�����Ӧ��Ԫ���Ƿ��Ѿ���ֵ
        /// </summary>
        /// <param name="n">Ҫ�жϵ�λ��</param>
        /// <returns>�����ж����</returns>
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
        /// ȡ���ʺ���
        /// </summary>
        /// <param name="n">��������λ��</param>
        /// <returns>���صõ��ĵ���</returns>
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
        /// ȡ��������Դ����к�
        /// </summary>
        /// <param name="n">��������λ��</param>
        /// <returns>�����к���</returns>
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
        /// ��Ӵ�����Ϣ
        /// </summary>
        /// <param name="info">��Ϣ˵��</param>
        /// <param name="line">������Դ���е��к�</param>
        private void errAdd(string info, string line)
        {
            err e = new err(info, line);
            errInfo.Add(e);
        }
        #endregion
    }

    /// <summary>
    /// err�������ڷ��������з��ִ����ʱ��
    /// </summary>
    class err
    {
        //������Ϣ
        public string info;

        //������Դ���е��к�
        public string line;

        /// <summary>
        /// err��Ĺ��캯��
        /// </summary>
        /// <param name="info">��Ϣ˵��</param>
        /// <param name="line">������Դ���е��к�</param>
        public err(string info, string line)
        {
            this.info = info;
            this.line = line;
        }
    }
}