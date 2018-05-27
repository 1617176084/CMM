using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class shangJinGeRen : BmobTable
{
    /// <summary>
    /// 玩家所执行的任务id 和 任务列表中的objiecId 保持一致
    /// </summary>
    public string renWuID;
    /// <summary>
    /// 这个任务完成会赠送个玩家的金币个数
    /// </summary>
    public int addJin;
    /// <summary>
    /// 任务的名称
    /// </summary>
    public string name;
    /// <summary>
    /// 完成这个任务的人的昵称
    /// </summary>
    public string niCheng;
    /// <summary>
    /// 任务的一句话描述
    /// </summary>
    public string yiJuHuaMiaoShu;
    /// <summary>
    /// 领取任务的人的ID
    /// </summary>
    public string userIdForLingQu;

    /// <summary>
    /// 发布这条任务人的ID
    /// </summary>
    public string userIdForFaBu;

 
    /// <summary>
    /// 任务状态 0 已领取 1 已上传截图 2 已经提交审核 3已被拒绝 4已经审核通过 5 已经领取过
    /// </summary>
    public string renWuType;

    /// <summary>
    /// 提交的的截图地址 玩家 所上传的截图  暂时只能上传一张 后续的都被覆盖掉
    /// </summary>
    public string tiJiaoImageUrl;

    /// <summary>
    /// 提交的消息
    /// </summary>
    public string tiJiaoMessage;
    /// <summary>
    /// 被拒绝的消息
    /// </summary>
    public string juJueMessage;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        addJin = int.Parse(input.getInt("addJin").ToString());
        name = input.getString("name");
        niCheng = input.getString("niCheng");
        yiJuHuaMiaoShu = input.getString("yiJuHuaMiaoShu");
        renWuID = input.getString("renWuID"); 
        renWuType = input.getString("renWuType");
        tiJiaoImageUrl = input.getString("tiJiaoImageUrl");
        tiJiaoMessage = input.getString("tiJiaoMessage");
        juJueMessage = input.getString("juJueMessage");
        userIdForLingQu = input.getString("userIdForLingQu");
        userIdForFaBu = input.getString("userIdForFaBu");
      
    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("renWuID", renWuID); 
        output.Put("renWuType", renWuType);
        output.Put("tiJiaoImageUrl", tiJiaoImageUrl);
        output.Put("juJueMessage", juJueMessage);
        output.Put("tiJiaoMessage", tiJiaoMessage);
        output.Put("userIdForLingQu", userIdForLingQu);
        output.Put("userIdForFaBu", userIdForFaBu);
        output.Put("addJin", addJin);
        output.Put("name", name);
        output.Put("niCheng", niCheng);
        output.Put("yiJuHuaMiaoShu", yiJuHuaMiaoShu);
 


    }
}
 
