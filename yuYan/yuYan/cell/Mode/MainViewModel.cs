using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

 
using cn.bmob.api;
using cn.bmob.io;
using Microsoft.Phone.Controls;

public class MainViewModel : INotifyPropertyChanged
{
    BmobWindowsPhone bmob;
    public PhoneApplicationPage page;
    public MainViewModel()
    {
        bmob = JYCaoZuo.getCaoZuo().bmob; 
        this.Items = new ObservableCollection<JYJinBiJiLuMode>();
        itemsForDuiHuan = new ObservableCollection<JYDuiHuanJiLuModel>();
    }


    public ObservableCollection<JYDuiHuanJiLuModel> itemsForDuiHuan { get; private set; }

    /// <summary>
    /// ItemViewModel 对象的集合。
    /// </summary>
    public ObservableCollection<JYJinBiJiLuMode> Items { get; private set; }

    private string _sampleProperty = "Sample Runtime Property Value";
    /// <summary>
    /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值
    /// </summary>
    /// <returns></returns>
    public string SampleProperty
    {
        get
        {
            return _sampleProperty;
        }
        set
        {
            if (value != _sampleProperty)
            {
                _sampleProperty = value;
                NotifyPropertyChanged("SampleProperty");
            }
        }
    }

    /// <summary>
    /// 返回本地化字符串的示例属性
    /// </summary>
    public string LocalizedSampleProperty
    {
        get
        {
            return "";
            // return AppResources.SampleProperty;
        }
    }

    public bool IsDataLoaded
    {
        get;
        private set;
    }
 
    /// <summary>
    /// 读取金币记录
    /// </summary>
    public void loadJinBI()
    {
        this.Items.Clear();
        BmobQuery query = new BmobQuery();
        query.WhereEqualTo("jieShouUserId", JYCaoZuo.getCaoZuo().user.userId);
        query.OrderByDescending("createdAt");
        query.Limit(50);
        bmob.Find<message>("message", query, (resp, exception) =>
        {

            if (exception != null)
            {
                JYCaoZuo.getCaoZuo().showMessagem("查询异常" + exception.Message);

                return;
            }
            JYCaoZuo.getCaoZuo().showMessagem("查 结果" + resp);
            this.page.Dispatcher.BeginInvoke(delegate
            {
                foreach (message li in resp.results)
                {
                    JYJinBiJiLuMode mode = new JYJinBiJiLuMode();
                    mode.MessgeType = li.messgeType;
                    mode.SendUserId = li.sendUserId;
                    mode.JieShouUserId = li.jieShouUserId;
                    mode.Title = li.title;
                    mode.Content = li.content;
                    mode.CreatTime = li.createdAt;
                    mode.objiectId = li.objectId;
                    // liShiJiLu li = resp.results[0];
                    this.Items.Add(mode);
                }
            });

        });
    }
    public void loadDuiHuanList()
    {
        this.itemsForDuiHuan.Clear();
        BmobQuery query = new BmobQuery();
        query.OrderByDescending("createdAt");
        query.WhereEqualTo("duiHuanType", 1);
        BmobQuery query2 = new BmobQuery();
        query2.WhereEqualTo("duiHuanType",0);
        query.Or(query2);
       // query.WhereEqualTo("duiHuan", JYCaoZuo.getCaoZuo().user.userId);
        query.Limit(10);
        bmob.Find<duiHuan>("duiHuan", query, (resp, exception) =>
        {

            if (exception != null)
            {
                JYCaoZuo.getCaoZuo().showMessagem("查询异常" + exception.Message);

                return;
            }
            JYCaoZuo.getCaoZuo().showMessagem("查 结果" + resp);
            this.page.Dispatcher.BeginInvoke(delegate
            {
                foreach (duiHuan li in resp.results)
                {
                    JYDuiHuanJiLuModel duicell = new JYDuiHuanJiLuModel();
                    try
                    {

                        duicell.userId = li.userId;
                        duicell.name = "*" +li.shiMing.Substring(1,li.shiMing.Length -1);
                        duicell.qq = li.qq;

                        string type = "无";
                        if (li.duiHuanType == 0)
                        {
                            type = "等待发货";
                        }
                        if (li.duiHuanType == 1)
                        {
                            type = "已发货";
                        }

                        duicell.content = int.Parse(li.jinE) / 10.0 + "元" + "(" + type + "--" + li.createdAt + ")";

                        duicell.likeNumber ="+"+ li.likeNumber;
                        duicell.objictId = li.objectId;
                    }
                    catch { }
                    this.itemsForDuiHuan.Add(duicell);
                }
            });

        });
    }


    /// <summary>
    /// 创建一些 ItemViewModel 对象并将其添加到 Items 集合中。
    /// </summary>
    public void LoadData()
    {

        // 示例数据；替换为实际数据
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
        //this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

        this.IsDataLoaded = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(String propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (null != handler)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}