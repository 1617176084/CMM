using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class myUser : BmobTable
{
    public string userId;
    /// <summary>
    /// 真实姓名  也就是支付宝实名认证
    /// </summary>
    public string name;
    public double jinBi;
    public string phone;
    public string qq;
    /// <summary>
    /// 昵称也就是 唯一的一个自由定制名称
    /// </summary>
    public string niCheng;
    public string zhiFuBao;
    public string sheBei;
    /// <summary>
    /// 用户类型 0普通用户 1 VIP商户 2 管理员
    /// </summary>
    public int userType;
    /// <summary>
    /// 是否评价  0 正在读取信息  1未评价 2 已经评价
    /// </summary>
    public int isPingJia;

    /// <summary>
    /// 是否禁止发言  0 允许  1 禁止
    /// </summary>
    public int isJinYan;


    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        try
        {
            userId = input.getString("userId");
            name = input.getString("name");

            jinBi = double.Parse(input.getDouble("jinBi").ToString());
            phone = input.getString("phone");
            qq = input.getString("qq");
            niCheng = input.getString("niCheng");
            zhiFuBao = input.getString("zhiFuBao");
            sheBei = input.getString("sheBei");
            isPingJia = int.Parse(input.getInt("isPingJia").ToString());
            userType = int.Parse(input.getInt("userType").ToString());
            isJinYan = int.Parse(input.getInt("isJinYan").ToString());
        }
        catch { 
        
        }
    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("userId", userId);
        output.Put("name", name);
        output.Put("jinBi", jinBi);
        output.Put("phone", phone);
        output.Put("qq", qq);
        output.Put("niCheng", niCheng);
        output.Put("zhiFuBao", zhiFuBao);
        output.Put("isPingJia", isPingJia);
        output.Put("userType", userType);
        output.Put("sheBei", sheBei);
        output.Put("isJinYan", isJinYan);

    }
}
