using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlHelper.Models
{
    public class Player
    {
        public PlayerClub Club { get; set; }
        public int Victories3vs3 { get; set; }
        public bool IsQualifiedFromChampionshipChallenge { get; set; }
        public PlayerIcon Icon { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public int Trophies { get; set; }
        public int ExpLevel { get; set; }
        public int ExpPoints { get; set; }
        public int HighestTrophies { get; set; }
        public int PowerPlayPoints { get; set; }
        public int HighestPowerPlayPoints { get; set; }
        public int SoloVictories { get; set; }
        public int DuoVictories { get; set; }
        public int BestRoboRumbleTime { get; set; }
        public int BestTimeAsBigBrawler { get; set; }
        public List<BrawlerStat> Brawlers { get; set; }
        public string NameColor { get; set; }
    }

    public class PlayerClub
    {
        public string Tag { get; set; }
        public string Name { get; set; }
    }

    public class PlayerIcon
    {
        public int Id { get; set; }
    }

    public class BrawlerStat
    {
        
        public string BrawlerName { get; set; }
        public int Rank { get; set; }
        public int PowerLevel { get; set; }
        public int Trophies { get; set; }
        
    }
}