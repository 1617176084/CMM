using cn.bmob.api;
using cn.bmob.io;
using HTML5App1;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using yuYan;
 

class JYCaoZuo
{
    static string tableNameForUser = "myUser";
    static string tableNameForCaoZuo = "liShiJiLu";
    static string tableNameForDuiHuan = "duiHuan";
    static string tableNameForHelp = "help";
    static string tableNameForCaoZuoJiLu = "caoZuo";
 public   TextBlock labForQian;
 public MainPage page;

   public BmobWindowsPhone bmob;
    public myUser user;
    public liShiJiLu jiLu;
    public help help;

    static JYCaoZuo caoZuo = null;
    
    
    /// <summary>
    /// 单利模式
    /// </summary>
    /// <returns></returns>
    public static JYCaoZuo getCaoZuo()
    {
        if (caoZuo == null)
        {
            caoZuo = new JYCaoZuo();
            
        }
        return caoZuo;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="bmobID"></param>
    public void init(string bmobID)
    {
     bmob = new BmobWindowsPhone();
     bmob.initialize(bmobID);
     bmob.StartPush();
     getHelp();
     //user = new myUser();
     //jiLu = new liShiJiLu();
    }
    public void getHelp()
    { 
     BmobQuery queryUser = new BmobQuery();
      //  queryUser.WhereEqualTo("userId",userId);

     bmob.Find<help>(tableNameForHelp, queryUser, (resp, exception) =>
     {
         if (exception != null)
         {
             showMessagem("查询异常" + exception.Message);
             return;
         }
         if (resp.results.Count > 0)
         {
             help = resp.results[0];

             
         

         }
         getUser();
     });
    }

    public void getUser()
    {
        object phoneName = Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");

      
      string userId = Convert.ToBase64String((byte[])phoneName);

        BmobQuery queryUser = new BmobQuery();
        queryUser.WhereEqualTo("userId",userId);

        bmob.Find<myUser>(tableNameForUser, queryUser, (resp, exception) =>
        {
            if (exception != null)
            {
                showMessagem("查询异常" + exception.Message);
               
                return;
            }
            if (resp.results.Count == 0)
            {
                showMessagem("没有改用户  创建");
                
                user = new myUser();
                user.userId = userId;
                user.isPingJia = 0;
                user.userType = 0;
                user.isJinYan = 0;
              //  user.jinBi = help.zhuCeZengSong;
                user.niCheng = "匿名";
                addCaoZuo( user.jinBi,"登陆赠送", user.jinBi);
         string sheBei =       DeviceExtendedProperties.GetValue("DeviceName").ToString(); //手机型号
         user.sheBei = sheBei;
                creatUser();

            }

            else
            {
                showMessagem("查找到该用户" + resp.results[0]);
                user = resp.results[0];
            //    updataJinBi(new Random().NextDouble()-0.5, "随机操作用户金币");
               
            }
        });

    }


    public void creatUser()
    {
     
        bmob.Create(tableNameForUser, user, (resp2, exception2) =>
        {
            if (exception2 != null)
            {
                showMessagem("查询异常" + exception2.Message);
                return;
            }
            showMessagem("创建成功" + resp2.ToString());
            getUser();

        });
    }
    public void addPushi()
    {
  //      PushParamter push = new PushParamter();
 //       bmob.StartPush();
        //bmob.Push(push, (response, exception) =>
        //{
        //    if (exception != null)
        //    {
        //        showMessagem("查询异常" + exception.Message);
        //        return;
        //    }
        //    showMessagemBox("更新成功");

        //});
    }

    /// <summary>
    /// 根据本地用户记录更新服务器
    /// </summary>
    public void updataUser()
    {

        bmob.Update(tableNameForUser, user.objectId,user, (resp, exception) => {

            if (exception != null)
            {
                showMessagem("查询异常" + exception.Message);
                return;
            }
            showMessagem("更新成功");
        });
    }
   /// <summary>
   /// 增加或减少金币
   /// </summary>
   /// <param name="nubber">增加或减少的金币数量</param>
   /// <param name="beiZhu">这次操作的备注</param>
    public void updataJinBi(double nubber, string beiZhu)
    {
        user.jinBi += nubber;
        addCaoZuo(nubber, beiZhu, user.jinBi);

        //增加
        updataUser();
        return;
        BmobQuery queryUser = new BmobQuery();
        queryUser.WhereEqualTo("userId", user.userId);


        bmob.Find<myUser>(tableNameForUser, queryUser, (resp, exception) =>
        {
            if (exception != null)
            {
                showMessagem("查询异常" + exception.Message);
                return;
            }
            if (resp.results.Count == 0)
            {
                showMessagem("没有改用户  创建");
                creatUser();
            }

            else
            {
                showMessagem("查找到该用户" + resp.results[0]);
                user = resp.results[0];

   
               
            }
        });
    }
    /// <summary>
    /// 添加一条金币操作的备注信息
    /// </summary>
    /// <param name="jinbi">金币操作数量</param>
    /// <param name="beizhu">备注</param>
    public void addCaoZuo(double jinbi,string beizhu,double nowjinBi)
    { 
     liShiJiLu caozuo = new liShiJiLu();
        caozuo.userId = user.userId;
        caozuo.jinBiCaoZuo = "金币" + ((jinbi > 0) ? ("+" + jinbi.ToString()) : jinbi.ToString());
        caozuo.beiZhu = "(金币操作后的数量:"+nowjinBi+")"+beizhu ;
        bmob.Create(tableNameForCaoZuo, caozuo, (respo, exception) => {
            if (exception != null)
            {
                showMessagem("添加异常" + exception.Message);
                return;
            }
        
        
        });
    }

    public void adduiHuan(string name,string haoMa ,double qian,string qq)
    {
   
        duiHuan dui = new duiHuan();
        dui.userId = user.userId;
        dui.shiMing = name;
        dui.zhiFuBaoHao = haoMa;
        dui.jinE = qian.ToString();
        dui.duiHuanType = 0;
        dui.qq = qq;
        dui.likeNumber = new Random().Next(2, 40);
        bmob.Create(tableNameForDuiHuan, dui, (respo, exception) =>
        {
            if (exception != null)
            {
                showMessagem("兑换异常" + exception.Message);
                return;
            }

            showMessagemBox("兑换成功，我们会在两个工作日内帮您结算完成，,每日发货众多，为保证能顺利给您发货，请务必联系客服QQ501333628。");
        });
    
    }

    /// <summary>
    /// 用户操作记录表
    /// </summary>
    /// <param name="message"></param>
    public void addaoZuo(string message)
    {
        try
        {
        caoZuo cao = new caoZuo();
        cao.userId = user.userId;
        cao.content = message;
       
            bmob.Create(tableNameForCaoZuoJiLu, cao, (respo, exception) =>
                {
                    if (exception != null)
                    {
                        showMessagem("兑换异常" + exception.Message);
                        return;
                    }


                });
        }
        catch
        { 
        
        }
    
    }
    /// <summary>
    /// 增加一条建议信息
    /// </summary>
    public void addJianYI(string message,string qq)
    {
        JYJianYiModel jian = new JYJianYiModel();
        jian.content = message;
        jian.userId = user.userId;
        jian.qq = qq;
        jian.isChuLi = false;
        bmob.Create("jianYi", jian, (resp, exception) =>
        {
            if (exception != null)
            {
                showMessagem("建议异常" + exception.Message);
                return;
            }
        
        });
        showMessagemBox("谢谢您的反馈，我们已经收到您的建议。");
    }
    /// <summary>
    /// 给人发送一条消息  软件内信息
    /// </summary>
    /// / <param name="title">消息标题</param>
    /// <param name="message">消息内容</param>
    /// <param name="jieShouUserId">接收人的Id</param>
    /// <param name="messgeType">消息类型 0 可回复 普通信息 包括审核通过推送，领取了多少金币等等 1不可回复 系统推送包括节假日 等官方推送</param>
 
    public void sendMessage(string title, string message, string jieShouUserId,string messgeType) { 
        message m = new message();
        m.messgeType = messgeType;
        m.sendUserId = user.userId;
        m.jieShouUserId = jieShouUserId;
        m.title = title;
        m.content = message;
        bmob.Create("message", m, (respon, exc) => {
            if (exc != null) {
                showMessagemBox("这条消息发送失败");
                return;
            }
            showMessagemBox("发送成功"); 
        });
    
    }

    /// <summary>
    /// 发送push
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    public void sendPush(string title, string message)
    {
        bmob.Push(new PushParamter().toast(title, message), (resp, ex) =>
        {
            string status = "OK";
            if (ex != null)
            {
                showMessagemBox("push发送失败");
                return;
            }
            showMessagemBox("push发送成功"); 
        });
    }

 public   void showMessagem(string message)
    {
        this.page.Dispatcher.BeginInvoke(delegate
        {
            try
            {
                labForQian.Text = user.jinBi.ToString();
                Debug.WriteLine(message);
            }
            catch
            { 
            
            }
          //  MessageBox.Show(message);
        });
    }

 public   void showMessagemBox(string message)
    {
        this.page.Dispatcher.BeginInvoke(delegate
        {
            try
            {
              //  labForQian.Text = user.jinBi.ToString();
                //   Debug.WriteLine(message);
                MessageBox.Show(message);
            }
            catch {  

            }
        });
    }
}