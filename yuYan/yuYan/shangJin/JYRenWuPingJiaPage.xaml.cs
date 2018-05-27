using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using cn.bmob.api;
using cn.bmob.io;
using Microsoft.Phone.Tasks;
namespace HTML5App1.shangJin
{
    public partial class JYRenWuPingJiaPage : PhoneApplicationPage
    {
        public JYRenWuPingJiaPage()
        {
            InitializeComponent();
            itemsForShangJin = new ObservableCollection<HuiFuMode>();
            this.DataContext = this;

        }
        string renWuid;
        public ObservableCollection<HuiFuMode> itemsForShangJin { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> dic = this.NavigationContext.QueryString;
            if (dic.ContainsKey("renWuid"))
            {
                renWuid = dic["renWuid"];
                freashData();
            }
        }
        public void freashData()
        {
            itemsForShangJin.Clear();
            BmobWindowsPhone bmob = JYCaoZuo.getCaoZuo().bmob;


            BmobQuery que = new BmobQuery();

            que.WhereEqualTo("isJinYan", 0);
            que.WhereEqualTo("renWuId", renWuid);
            que.OrderByDescending("createdAt");
         //   que.Skip(3);
            que.Limit(40);
            bmob.Find<RenWuHuiFu>("RenWuHuiFu", que, (respon, exc) => {
                if (exc != null)
                {
                //    JYCaoZuo.getCaoZuo().showMessagemBox("回复出错");
                    return;
                }
                if (respon.results.Count > 0)
                {

                    this.Dispatcher.BeginInvoke(delegate{
                        foreach(RenWuHuiFu re in respon.results){
                            HuiFuMode mo = new HuiFuMode();
                            mo.NiCheng = re.huiFuNiCheng;
                            mo.Content = re.huiFuContet;
                            mo.UserId = re.huiFuUserId;
                            itemsForShangJin.Add(mo);
                        } 
                    });
            
                }
                else
                { 
                //无人评论
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            JYCaoZuo.getCaoZuo().addaoZuo("任务评价页面 评价");

            if (txtForHuiFu.Text.Length < 3)
            {
                MessageBox.Show("评价内容太短啦");
                return;
            }

            if (JYCaoZuo.getCaoZuo().user.isJinYan == 1)
            {
                MessageBox.Show("您已被禁言！");
                JYCaoZuo.getCaoZuo().addaoZuo("任务评价页面 评价--被禁言");
                return;
            }

            RenWuHuiFu re = new RenWuHuiFu();
            re.renWuId = renWuid;
            re.huiFuContet = txtForHuiFu.Text;
            re.huiFuNiCheng = JYCaoZuo.getCaoZuo().user.niCheng;
            re.huiFuUserId = JYCaoZuo.getCaoZuo().user.userId;
            re.isJinYan = JYCaoZuo.getCaoZuo().user.isJinYan;
            re.zhiChi = 0;

            BmobWindowsPhone bmob = JYCaoZuo.getCaoZuo().bmob;

            bmob.Create("RenWuHuiFu", re, (respon, exc) => {
                if (exc != null)
                {

                    return;
                }
                JYCaoZuo.getCaoZuo().showMessagemBox("回复成功，您的评论等待审核通过就能出现在评论列表。");
                this.Dispatcher.BeginInvoke(delegate {

                    freashData();
                });
            
            });
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //任务回复中 点击玩家详情
            return;
            HuiFuMode mode = (sender as LongListSelector).SelectedItem as HuiFuMode;

            this.NavigationService.Navigate(new Uri("/shangJin/JYWanJiaInfoPage.xaml?userId=" + mode.UserId, UriKind.Relative));
   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
              MarketplaceDetailTask mar = new Microsoft.Phone.Tasks.MarketplaceDetailTask();
            mar.ContentIdentifier = "fd7c46dc-50bc-4e41-8e91-9814d85fcdf4";
            mar.Show();
        }
    }
}