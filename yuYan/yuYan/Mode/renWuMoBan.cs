using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class renWuMoBan : BmobTable
{
    public string MiaoShu;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);

        this.MiaoShu = input.getString("MiaoShu").Replace("||", "\r\n");
        


    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("MiaoShu", MiaoShu.Replace("\r\n", "||"));


    }
}
 
