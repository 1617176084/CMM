using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class hongBao : BmobTable
{
    public string hongBaoId;
    public string jine;
    public string userId;
    /// <summary>
    /// 0 可以使用  1 已经使用   2 淘宝必须确认收货才能使用  
    /// </summary>
    public string hongBaoType;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        this.hongBaoId = input.getString("hongBaoId");
        this.jine   = input.getString("jine");
        this.userId = input.getString("userId");
        this.hongBaoType = input.getString("hongBaoType");
    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("hongBaoId", hongBaoId);
        output.Put("jine", jine);
        output.Put("userId", userId);
        output.Put("hongBaoType", hongBaoType);
     }
}
 