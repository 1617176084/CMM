using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class JYJinBiJiLuMode : INotifyPropertyChanged
{
 public   string objiectId;
    /// <summary>
    /// 0 普通信息 包括审核通过推送，领取了多少金币等等 1系统推送包括节假日 等官方推送
    /// </summary>
    private string messgeType;

    public string MessgeType
    {
        get { return messgeType; }
        set
        {
            messgeType = value;
            NotifyPropertyChanged("MessgeType");
        }
    }
    /// <summary>
    /// 发送人的ID
    /// </summary>
    private string sendUserId;

    public string SendUserId
    {
        get { return sendUserId; }
        set
        {
            sendUserId = value;
            NotifyPropertyChanged("SendUserId");
        }
    }
    /// <summary>
    /// 接受人的Id
    /// </summary>
    private string jieShouUserId;

    public string JieShouUserId
    {
        get { return jieShouUserId; }
        set
        {
            jieShouUserId = value;
            NotifyPropertyChanged("JieShouUserId");
        }
    }
    /// <summary>
    /// 消息标题
    /// </summary>
    private string title;

    public string Title
    {
        get { return title; }
        set { title = value; NotifyPropertyChanged("Title"); }
    }
    /// <summary>
    /// 消息内容
    /// </summary>
    private string content;

    public string Content
    {
        get { return content; }
        set { content = value; NotifyPropertyChanged("Content"); }
    }
    private string creatTime;

    public string CreatTime
    {
        get { return creatTime; }
        set
        {
            creatTime = value;
            NotifyPropertyChanged("CreatTime");
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string chengd)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(chengd));
        }
    }

}
 
