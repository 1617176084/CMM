using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class shangJinPublice : BmobTable
{
    /// <summary>
    /// 创建者的ID
    /// </summary>
   public string userId;
    /// <summary>
    /// 任务的名称
    /// </summary>
   public string name;
    /// <summary>
    /// 任务的一句话描述
    /// </summary>
   public string yiJuHuaMiaoShu;
    /// <summary>
    /// 任务的详细描述
    /// </summary>
   public string xiangXiMiaoShu;
   
    /// <summary>
    /// 任务的总数量
    /// </summary>
   public int allNumber;
    /// <summary>
    /// 任务的剩余数量
    /// </summary>
   public int shengYuNumber;
    /// <summary>
    /// 这个任务完成会赠送个玩家的金币个数
    /// </summary>
   public int addJin;
    /// <summary>
    /// base64图片
    /// </summary>
   public string base64ImageStr;


 

    /// <summary>
    ///任务审核状态，用于发表任务 0提交至管理员审核中   1 审核通过 任务上架  2 审核失败，任务驳回 3 任务被管理员下架 4任务已经处理以后 不需要做任何处理
    /// </summary>
   public int shenHetype;

    /// <summary>
    /// 任务类型 1 下载应用或评分  2 分享到社交平台 3 链接到一个外部网站 4 贴桌面到桌面，的一个网址链接
    /// </summary>
   public int renWuType;

   public string renWuTypeContent;
    /// <summary>
    /// 被拒绝或者下架的消息
    /// </summary>
   public string juJueMessage;
   public override void readFields(BmobInput input)
   {
       base.readFields(input);
       userId = input.getString("userId");
       name = input.getString("name");
       yiJuHuaMiaoShu = input.getString("yiJuHuaMiaoShu");

       xiangXiMiaoShu = input.getString("xiangXiMiaoShu");
       allNumber = int.Parse(input.getInt("allNumber").ToString());
       shengYuNumber = int.Parse(input.getInt("shengYuNumber").ToString());
       addJin = int.Parse(input.getInt("addJin").ToString());
       base64ImageStr = input.getString("base64ImageStr"); 
       shenHetype = int.Parse(input.getInt("shenHetype").ToString());
       renWuType = int.Parse(input.getInt("renWuType").ToString());
       renWuTypeContent = input.getString("renWuTypeContent");
       juJueMessage = input.getString("juJueMessage");
   }
   public override void write(BmobOutput output, bool all)
   {
       base.write(output, all);
       output.Put("userId", userId);
       output.Put("name", name);
       output.Put("yiJuHuaMiaoShu", yiJuHuaMiaoShu);
       try
       {
           output.Put("xiangXiMiaoShu", xiangXiMiaoShu.Replace("\r", "||"));
       }
       catch { }
       output.Put("allNumber", allNumber);
       output.Put("shengYuNumber", shengYuNumber);
       output.Put("base64ImageStr", base64ImageStr);
       output.Put("addJin", addJin);
       output.Put("shenHetype", shenHetype);
       output.Put("renWuType", renWuType);
       output.Put("renWuTypeContent", renWuTypeContent);
       output.Put("juJueMessage", juJueMessage);

   }


}
 
