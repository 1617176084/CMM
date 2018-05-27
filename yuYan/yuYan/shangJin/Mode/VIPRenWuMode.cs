using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VIPRenWuMode : INotifyPropertyChanged
{
    string niCheng;

    public string NiCheng
    {
        get { return niCheng; }
        set { niCheng = value;
        Property("NiCheng");
        }
    }
    string yiJuHuaLiuYan;

    public string YiJuHuaLiuYan
    {
        get { return yiJuHuaLiuYan; }
        set { yiJuHuaLiuYan = value;
        Property("YiJuHuaLiuYan");
        }
    }
    string objictId;

    public string ObjictId
    {
        get { return objictId; }
        set { objictId = value;

        Property("ObjictId");
        }
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
 
