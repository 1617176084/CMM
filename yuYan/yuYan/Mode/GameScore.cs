using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    class GameScore : BmobTable
    {
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string playerName { get; set; }

        /// <summary>
        /// 游戏分数
        /// </summary>
        public BmobInt score { get; set; }

        /// <summary>
        /// 是否作弊
        /// </summary>
    
        public BmobBoolean cheatMode { get; set; }

        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.playerName = input.getString("playerName");
            this.score = input.getInt("score");
            this.cheatMode = input.getBoolean("cheatMode");

        }
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("playerName", playerName);
            output.Put("score", score);
            output.Put("cheatMode", cheatMode);
        
        }
    }

