using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class daZhongXiao : BmobTable
{
    public double daPeiLv;
    public int qiHao;
    public int type;
    public double xiaoPeiLv;
    public double zhongPeiLv;

    public override void readFields(BmobInput input)
    {
        base.readFields(input);

        this.daPeiLv = input.getDouble("daPeiLv").Get();
        this.qiHao = input.getInt("qiHao").Get();
        this.type = input.getInt("type").Get();
        this.xiaoPeiLv = input.getDouble("xiaoPeiLv").Get();
        this.zhongPeiLv = input.getDouble("zhongPeiLv").Get();


    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("daPeiLv", daPeiLv);
        output.Put("qiHao", qiHao);
        output.Put("type", type);
        output.Put("xiaoPeiLv", xiaoPeiLv);
        output.Put("zhongPeiLv", zhongPeiLv);

    }
}
