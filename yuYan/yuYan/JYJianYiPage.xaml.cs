using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HTML5App1
{
    public partial class JYJianYiPage : PhoneApplicationPage
    {
        public JYJianYiPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            JYCaoZuo.getCaoZuo().addaoZuo("点击了建议回馈");
            //建议回馈
            if (txtContent.Text.Length < 6)
            {

                MessageBox.Show("您的建议太过简短，不能说明详细的问题，请多写点哦。");
                return;
            }
            //建议回馈
            if (txtForQQ.Text.Length < 6)
            {

                MessageBox.Show("您的联系方式可能不正确，将会导致我们得知您的反馈却无法反馈，请填写联系您的信息。");
                return;
            }

            JYCaoZuo.getCaoZuo().addJianYI(txtContent.Text,txtForQQ.Text);
        }
    }
}