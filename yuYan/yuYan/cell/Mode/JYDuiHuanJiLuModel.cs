using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 public class JYDuiHuanJiLuModel : INotifyPropertyChanged
{
     public string userId;

     string _name;
    public string name { get {
        return _name;
    } set {
        _name = value;
        PropertyChangedValue("name");

    } }
    string _qq;
    public string qq
    {
        get
        {
            return _qq;
        }
        set
        {
            _qq = value;
            PropertyChangedValue("qq");
        }
    }
    string _content;
    /// <summary>
    /// 兑换的现金 和发货状态
    /// </summary>
    public string content
    {
        get
        {
            return _content;
        }
        set
        {
            _content = value;
            PropertyChangedValue("content");

        }
    }
    string _likeNumber;
    public string likeNumber
    {
        get
        {
            return _likeNumber;
        }
        set
        {
            _likeNumber = value;
            PropertyChangedValue("likeNumber");

        }
    }

    public string objictId;

    // 摘要: 
    //     在更改属性值时发生。
     public event PropertyChangedEventHandler PropertyChanged;
    public void PropertyChangedValue(string chengName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(chengName));
        }
    }

}
 
