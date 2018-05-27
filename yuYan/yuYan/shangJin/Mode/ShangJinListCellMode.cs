using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShangJinListCellMode : INotifyPropertyChanged
{
    public string objiectId;
    public string userId;
    public string imageUrl;

    /// <summary>
    ///任务审核状态，用于发表任务 0提交至管理员审核中   1 审核通过 任务上架  2 审核失败，任务驳回 3 任务被管理员下架 
    /// </summary>
    public int renWuType; 

    private string shenHetype;
    /// <summary>
    /// 被拒绝或者下架的消息
    /// </summary>
    public string juJueMessage;
    public string ShenHetype
    {
        get { return shenHetype; }
        set { shenHetype = value; Property("shenHetype"); }
    }
    string name;
    public string Name
    {
        get { return name; }
        set { name = value;
        Property("Name");
        }
    }
    string miaoShu;

    public string MiaoShu
    {
        get { return miaoShu; }
        set { miaoShu = value;
        Property("MiaoShu");
        }
            
    }
    string addJin;

    public string AddJin
    {
        get { return addJin; }
        set { addJin = value;
        Property("AddJin");
        }
    }
    string shengYuShu;

    public string ShengYuShu
    {
        get { return shengYuShu; }
        set { shengYuShu = value;
        Property("ShengYuShu");
        }
    }
 
    // 摘要: 
    //     在更改属性值时发生。
   public event PropertyChangedEventHandler PropertyChanged;
    public void Property(string _name)
    {
        if (PropertyChanged!= null)
        PropertyChanged(this, new PropertyChangedEventArgs(_name));
    }
}
 
