using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using yuYan.Resources;
using CMMCompiler;
using System.Diagnostics;
using Microsoft.Phone.Tasks;

namespace yuYan
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();


            string bmobId = "2bf438a29c5411c813e6e50a1aedfd0c";
            JYCaoZuo.getCaoZuo().page = this;
            JYCaoZuo.getCaoZuo().init(bmobId);


            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
            string daiMa = "int i = 0;while(i<100){ i = i+1;write(i);}";

            //使用方法
            //1语义分析
            LexicalAnalysis la = new LexicalAnalysis();

            string outStr = la.Analyze(daiMa);
            List<object> errList = la.errlist;
            Debug.WriteLine(outStr);


            Analysis ciFaFenXi = new Analysis();
            //判断语义是否有误
            if (ciFaFenXi.syntaxAalysis(la))
            {

            }
            else
            {
                Debug.WriteLine(ciFaFenXi.errInfo.ToString());
            }
            //2 执行算法
            MidCode m0 = new MidCode(ciFaFenXi);
            m0.Scan();
            Interpret runner = new Interpret();
            runner.GetRun(m0.c);
            //得到结果
            string jieGuo = runner.jieGuo;
            Debug.WriteLine(jieGuo);

            m0.clear();
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            //帮助说明
       string message =      "CMM语言是C语言的一个子类  CMM语言解释器  解释器在语法分析的基础上，对语法分析程序的输出，语法树遍历，完成语义检查并生成代码，代码以四元式的形式输出，cmm解释器解释执行四元式序列，完成对cmm语言的解释。 ";
       MessageBox.Show(message);
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            //查看已保存的代码列表
            MessageBox.Show("下一次更新版本添加此功能");
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //保存现在的代码
            MessageBox.Show("下一次更新版本添加此功能");
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            //商店评价
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //执行结果
            //使用方法
            //1语义分析
   
            string daiMa = txtForDaiMa.Text;
            JYCaoZuo.getCaoZuo().addaoZuo("点击了执行代码：" + daiMa);
            LexicalAnalysis la = new LexicalAnalysis();

            string outStr = la.Analyze(daiMa);
            List<object> errList = la.errlist;
            Debug.WriteLine(outStr);
          

            Analysis ciFaFenXi = new Analysis();
            //判断语义是否有误
            if (ciFaFenXi.syntaxAalysis(la))
            {

            }
            else
            {

                txtForJieGuo.Text = ciFaFenXi.errInfo.ToString() + "\n" + outStr;
                Debug.WriteLine(ciFaFenXi.errInfo.ToString());
                return;
            }
            //2 执行算法
            MidCode m0 = new MidCode(ciFaFenXi);
            m0.Scan();
            Interpret runner = new Interpret();
            runner.GetRun(m0.c);
            //得到结果
            string jieGuo = runner.jieGuo;
            Debug.WriteLine(jieGuo);
            txtForJieGuo.Text =    jieGuo + "\n" + outStr;

            m0.clear();
            JYCaoZuo.getCaoZuo().addaoZuo("点击了执行代码：" + daiMa + ",结果：" + txtForJieGuo.Text);
        }

        private void txtForDaiMa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            //交流区
            JYCaoZuo.getCaoZuo().addaoZuo("交流区");
            this.NavigationService.Navigate(new Uri("/shangJin/JYRenWuPingJiaPage.xaml?renWuid=代码交流", UriKind.Relative));

        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            //给我建议
            JYCaoZuo.getCaoZuo().addaoZuo("给我建议");
            this.NavigationService.Navigate(new Uri("/JYJianYiPage.xaml", UriKind.Relative));
        }

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}