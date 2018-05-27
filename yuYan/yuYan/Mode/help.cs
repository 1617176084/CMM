using cn.bmob.io;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class help : BmobTable
{
 public   string keFuQQ;
 public string zhuanHuaFei;
 public string shearMessage;
 public string tongYiTiaoKuan;
 public string kaMiWangZhi;
 public string renWuBangZhu;
 public double zhuCeZengSong;
 public double pingJiaZengSong;
 public double upVipNeedJin;
 public BmobBoolean isHideBiDaXiao;
 /// <summary>
/// 最高版本号 低于此版本号的将被提示去更新
 /// </summary>
 public BmobDouble banBenHao;
 public string banBenShuoMing;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        this.isHideBiDaXiao = input.getBoolean("isHideBiDaXiao");
        this.banBenHao = input.getDouble("banBenHao");
        this.banBenShuoMing = input.getString("banBenShuoMing");

        this.renWuBangZhu = input.getString("renWuBangZhu");
        this.keFuQQ = input.getString("keFuQQ");
        this.zhuanHuaFei = input.getString("zhuanHuaFei");
        this.shearMessage = input.getString("shearMessage");
        this.kaMiWangZhi = input.getString("kaMiWangZhi");
        this.tongYiTiaoKuan = input.getString("tongYiTiaoKuan");
        this.zhuCeZengSong = Double.Parse(input.getDouble("zhuCeZengSong").ToString());
        this.pingJiaZengSong = Double.Parse(input.getDouble("pingJiaZengSong").ToString());
        this.upVipNeedJin = input.getDouble("upVipNeedJin").Get();
    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("isHideBiDaXiao", isHideBiDaXiao);
        output.Put("renWuBangZhu", renWuBangZhu);
        output.Put("keFuQQ", keFuQQ);
        output.Put("zhuanHuaFei", zhuanHuaFei);
        output.Put("kaMiWangZhi", kaMiWangZhi);
        output.Put("shearMessage", shearMessage);
        output.Put("tongYiTiaoKuan", tongYiTiaoKuan);
        output.Put("zhuCeZengSong", zhuCeZengSong);
        output.Put("pingJiaZengSong", pingJiaZengSong);
        output.Put("upVipNeedJin", upVipNeedJin);
        output.Put("banBenShuoMing", banBenShuoMing);
        output.Put("banBenHao", banBenHao);
    }
}
