using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class JYJianYiModel : BmobTable
{
    public string userId;
    public string content;
    public string qq;
  /// <summary>
  ///是否处理过这条建议 false没有  true 已经处理
  /// </summary>
    public bool isChuLi;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        try
        {
            this.userId = input.getString("userId");
            this.content = input.getString("content");
            this.qq = input.getString("qq");
            this.isChuLi = input.getBoolean("isChuLi").Get();
        }
        catch
        { 
        
        }

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("userId", userId);
        output.Put("content", content);
        output.Put("qq", qq);
        output.Put("isChuLi", isChuLi);
    }
}
 
