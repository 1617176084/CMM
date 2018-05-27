using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class yaZhu : BmobTable
{
    public string qiHaoObjectId;
    public BmobInt qiHao;
    public string userId;
    public BmobDouble jieE;
    public BmobInt type;
    public BmobInt typeForKaiJiang;
    public BmobBoolean isLingQu;

    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        this.qiHaoObjectId = input.getString("qiHaoObjectId");
        this.qiHao = input.getInt("qiHao");
        this.userId = input.getString("userId");
        this.jieE = input.getDouble("jieE");
        this.type = input.getInt("type");
        this.typeForKaiJiang = input.getInt("typeForKaiJiang");
        this.isLingQu = input.getBoolean("isLingQu");



    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("qiHaoObjectId", qiHaoObjectId);
        output.Put("qiHao", qiHao);
        output.Put("userId", userId);
        output.Put("jieE", jieE);
        output.Put("type", type);
        output.Put("typeForKaiJiang", typeForKaiJiang);
        output.Put("isLingQu", isLingQu);


    }
}
 
