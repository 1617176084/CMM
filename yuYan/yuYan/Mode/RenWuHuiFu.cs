using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RenWuHuiFu : BmobTable
{
    public string renWuId;
    public string huiFuUserId;
    public string huiFuNiCheng;
    public string huiFuContet;
    public int zhiChi;
    /// <summary>
    /// 是否被禁言 0 没有  1 被禁言
    /// </summary>
    public int isJinYan;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);

        this.renWuId = input.getString("renWuId");
        this.huiFuUserId = input.getString("huiFuUserId");
        this.huiFuNiCheng = input.getString("huiFuNiCheng");
        this.huiFuContet = input.getString("huiFuContet");
        this.zhiChi = input.getInt("zhiChi").Get();
        this.isJinYan = input.getInt("isJinYan").Get();


    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("renWuId",renWuId);
        output.Put("huiFuUserId",huiFuUserId);
        output.Put("huiFuNiCheng",huiFuNiCheng);
        output.Put("huiFuContet",huiFuContet);
        output.Put("zhiChi",zhiChi);

        output.Put("isJinYan", isJinYan);

    }

}
 
