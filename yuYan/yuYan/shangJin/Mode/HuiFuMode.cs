using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HuiFuMode : INotifyPropertyChanged
{
    string userId;

    public string UserId
    {
        get { return userId; }
        set { userId = value;
        Property("UserId");
        }
    }
    string niCheng;

    public string NiCheng
    {
        get { return niCheng; }
        set { niCheng = value; Property("NiCheng"); }
    }
    string content;

    public string Content
    {
        get { return content; }
        set { content = value; Property("Content"); }
    }
    string zhiChi;

    public string ZhiChi
    {
        get { return zhiChi; }
        set { zhiChi = value; Property("ZhiChi"); }
    }
    // 摘要: 
    //     在更改属性值时发生。
    public event PropertyChangedEventHandler PropertyChanged;
    public void Property(string _name)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(_name));
    }
}
