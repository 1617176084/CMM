using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class message : BmobTable
{
    /// <summary>
    /// 0 普通信息 包括审核通过推送，领取了多少金币等等 1系统推送包括节假日 等官方推送
    /// </summary>
    public string messgeType;
    /// <summary>
    /// 发送人的ID
    /// </summary>
    public string sendUserId;
    /// <summary>
    /// 接受人的Id
    /// </summary>
    public string jieShouUserId;
    /// <summary>
    /// 消息标题
    /// </summary>
    public string title;
    /// <summary>
    /// 消息内容
    /// </summary>
    public string content;

    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        messgeType = input.getString("messgeType");
        sendUserId = input.getString("sendUserId");
        jieShouUserId = input.getString("jieShouUserId");

        title = input.getString("title");
        content = input.getString("content");

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("messgeType", messgeType);
        output.Put("sendUserId", sendUserId);
        output.Put("jieShouUserId", jieShouUserId);
        output.Put("title", title);
        output.Put("content", content);

    }
}
 
