/*
 * 作者：黎国梁
 * 班级：三班
 * 学号：200632580088
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CMMCompiler
{
    /// <summary>
    /// 该IDtable类用于存放变量表
    /// </summary>
    class IDtable
    {
        //变量名称
        private string name;

        //变量类型
        private string type;

        //变量所在层
        private int level;

        //变量是否被赋值
        private bool valued;

        //变量值
        private string value;

        /// <summary>
        /// IDtable的构造函数
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="type">变量类型</param>
        public IDtable(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// IDtable的构造函数
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="type">变量类型</param>
        /// <param name="level">所在层</param>
        /// <param name="valued">是否有变量值</param>
        public IDtable(string name, string type, int level, bool valued)
        {
            this.name = name;
            this.type = type;
            this.level = level;
            this.valued = valued;
        }

        /// <summary>
        /// 用于访问私有变量name的属性
        /// </summary>
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        /// <summary>
        /// 用于访问私有变量type的属性
        /// </summary>
        public string Type
        {
            get
            { return type; }
            set
            { type = value; }
        }

        /// <summary>
        /// 用于访问私有变量level的属性
        /// </summary>
        public int Level
        {
            get
            { return level; }
            set
            { level = value; }
        }

        /// <summary>
        /// 用于访问私有变量vauled的属性
        /// </summary>
        public bool Valued
        {
            get
            { return valued; }
            set
            { valued = value; }
        }

        /// <summary>
        /// 用于访问私有变量vaule的属性
        /// </summary>
        public string Value
        {
            get
            { return value; }
            set
            { this.value = value; }
        }
    }

    /// <summary>
    /// 该TempIDtable类用于存放临时变量表
    /// </summary>
    class TempIDtable
    {
        //变量名称
        private string name;

        //变量类型
        private string type;

        //变量所在层
        private int level;

        //变量是否被赋值
        private bool valued;

        //变量值
        private string value;

        /// <summary>
        /// TempIDtable的构造函数
        /// </summary>
        /// <param name="name"></param>
        public TempIDtable(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// TempIDtable的构造函数
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="type">变量类型</param>
        public TempIDtable(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// TempIDtable的构造函数
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="type">变量类型</param>
        /// <param name="level">所在层</param>
        public TempIDtable(string name, string type, int level)
        {
            this.name = name;
            this.type = type;
            this.level = level;
            this.valued = false;
        }

        /// <summary>
        /// TempIDtable的构造函数
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="type">变量类型</param>
        /// <param name="level">所在层</param>
        /// <param name="valued">是否有变量值</param>
        public TempIDtable(string name, string type, int level, bool valued)
        {
            this.name = name;
            this.type = type;
            this.level = level;
            this.valued = valued;
        }

        /// <summary>
        /// 用于访问私有变量name的属性
        /// </summary>
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        /// <summary>
        /// 用于访问私有变量type的属性
        /// </summary>
        public string Type
        {
            get
            { return type; }
            set
            { type = value; }
        }

        /// <summary>
        /// 用于访问私有变量level的属性
        /// </summary>
        public int Level
        {
            get
            { return level; }
            set
            { level = value; }
        }

        /// <summary>
        /// 用于访问私有变量vauled的属性
        /// </summary>
        public bool Valued
        {
            get
            { return valued; }
            set
            { valued = value; }
        }

        /// <summary>
        /// 用于访问私有变量vaule的属性
        /// </summary>
        public string Value
        {
            get
            { return value; }
            set
            { this.value = value; }
        }
    }

    /// <summary>
    /// 该SZtable类用于存在数组表
    /// </summary>
    class SZtable
    {
        //数组名称
        private string name;

        //数组类型
        private string type;

        //数组长度
        private int length;

        //数组所在层
        private int level;

        //数组成员值
        private List<object> value = new List<object>();

        //数组成员
        public List<object> element = new List<object>();

        /// <summary>
        /// SZtable的构造函数
        /// </summary>
        /// <param name="name">数组名称</param>
        /// <param name="type">数组类型</param>
        /// <param name="length">数组长度</param>
        public SZtable(string name, string type, int length)
        {
            this.name = name;
            this.type = type;
            this.length = length;
        }


        /// <summary>
        /// SZtable的构造函数
        /// </summary>
        /// <param name="name">数组名称</param>
        /// <param name="type">数组类型</param>
        /// <param name="length">数组长度</param>
        /// <param name="level">数组所在层</param>
        public SZtable(string name, string type, int length, int level)
        {
            this.name = name;
            this.type = type;
            this.length = length;
            this.level = level;
        }

        /// <summary>
        /// SZtable的构造函数
        /// </summary>
        /// <param name="name">数组名称</param>
        /// <param name="type">数组类型</param>
        /// <param name="length">数组长度</param>
        /// <param name="level">数组所在层</param>
        /// <param name="valued">数组成员值</param>
        public SZtable(string name, string type, int length, int level, int value)
        {
            this.name = name;
            this.type = type;
            this.length = length;
            this.level = level;
            this.value.Add(value);
        }

        /// <summary>
        /// 用于访问私有变量name的属性
        /// </summary>
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        /// <summary>
        /// 用于访问私有变量type的属性
        /// </summary>
        public string Type
        {
            get
            { return type; }
            set
            { type = value; }
        }

        /// <summary>
        /// 用于访问私有变量length的属性
        /// </summary>
        public int Length
        {
            get
            { return length; }
            set
            { length = value; }
        }

        /// <summary>
        /// 用于访问私有变量level的属性
        /// </summary>
        public int Level
        {
            get
            { return level; }
            set
            { level = value; }
        }

        /// <summary>
        /// 用于访问私有变量vaule的属性
        /// </summary>
        public List<object> Value
        {
            get
            { return value; }
            set
            { this.value.Add(value); }
        }
    }

    /// <summary>
    /// 该TempSZtable类用于存放临时数组表
    /// </summary>
    class TempSZtable
    {
        //数组名称
        private string name;

        //数组类型
        private string type;

        //数组长度
        private int length;

        //数组所在层
        private int level;

        //数组成员值
        private List<object> value = new List<object>();

        //数组成员
        
        public List<object> element = new List<object>();

        /// <summary>
        /// TempSZtable的构造函数
        /// </summary>
        /// <param name="name">数组名称</param>
        /// <param name="type">数组类型</param>
        /// <param name="length">数组长度</param>
        /// <param name="level">数组所在层</param>
        public TempSZtable(string name, string type, int length, int level)
        {
            this.name = name;
            this.type = type;
            this.length = length;
            this.level = level;
        }

        /// <summary>
        /// 用于访问私有变量name的属性
        /// </summary>
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        /// <summary>
        /// 用于访问私有变量type的属性
        /// </summary>
        public string Type
        {
            get
            { return type; }
            set
            { type = value; }
        }

        /// <summary>
        /// 用于访问私有变量length的属性
        /// </summary>
        public int Length
        {
            get
            { return length; }
            set
            { length = value; }
        }

        /// <summary>
        /// 用于访问私有变量level的属性
        /// </summary>
        public int Level
        {
            get
            { return level; }
            set
            { level = value; }
        }

        /// <summary>
        /// 用于访问私有变量vaule的属性
        /// </summary>
        public List<object> Value
        {
            get
            { return value; }
            set
            { this.value.Add(value); }
        }
    }
}