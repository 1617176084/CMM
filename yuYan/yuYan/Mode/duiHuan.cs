using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class duiHuan : BmobTable
{
    public string userId;
    public string shiMing;
    public string zhiFuBaoHao;
    public string jinE;
    public string qq;
    /// <summary>
    /// 兑换类型 0 用户刚提交  1 成功给用户发货 2 此次兑换无效，金币回滚 3 此次兑换无效，用户作弊，封号 4用户信息错误 无法发货
    /// </summary>
    public int    duiHuanType;
    /// <summary>
    /// 羡慕这个兑换记录的个数
    /// </summary>
    public int likeNumber;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        try
        {
            this.userId = input.getString("userId");
            this.shiMing = input.getString("shiMing");
            this.zhiFuBaoHao = input.getString("zhiFuBaoHao");
            this.qq = input.getString("qq");
            this.jinE = input.getString("jinE");
            this.duiHuanType = int.Parse(input.getInt("duiHuanType").ToString());
            this.likeNumber = int.Parse(input.getInt("likeNumber").ToString());
        }
        catch
        { }

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        try
        {
            output.Put("userId", userId);
            output.Put("shiMing", shiMing);
            output.Put("zhiFuBaoHao", zhiFuBaoHao);
            output.Put("qq", qq);
            output.Put("jinE", jinE);
            output.Put("duiHuanType", duiHuanType);
            output.Put("likeNumber", likeNumber);
        }
        catch { 
        
        }
    }
}
