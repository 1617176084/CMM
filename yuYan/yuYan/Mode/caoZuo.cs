using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class caoZuo : BmobTable
    {
    public string userId;
    public string content;
    public override void readFields(BmobInput input)
    {
        base.readFields(input);

        this.userId = input.getString("userId");
        this.content = input.getString("content");
        

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        output.Put("userId", userId);
        output.Put("content", content);
     
    }
    }
