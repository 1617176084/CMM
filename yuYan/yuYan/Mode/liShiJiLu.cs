using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cn.bmob.io;

class liShiJiLu : BmobTable
{
    /// <summary>
    /// 用户id
    /// </summary>
  public  string userId;
    /// <summary>
    /// 金币操作
    /// </summary>
  public string jinBiCaoZuo;
    /// <summary>
    /// 备注
    /// </summary>
  public string beiZhu;

    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        userId = input.getString("userId");
        jinBiCaoZuo = input.getString("jinBiCaoZuo");
        beiZhu = input.getString("beiZhu");
       
 

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("userId", userId);
        output.Put("jinBiCaoZuo", jinBiCaoZuo);
        output.Put("beiZhu", beiZhu);
 


    }
}
