using NavalVessels.Models.Contracts;
using System.Text;

namespace NavalVessels.Models
{
    public class Battleship : Vessel, IBattleship
    {
        private const double BattleshipInitialArmorThickness = 300;

        public Battleship(string name, double mainWeaponCaliber, double speed)
            : base(name, mainWeaponCaliber, speed, BattleshipInitialArmorThickness)
        {
        }

        public bool SonarMode { get; private set; }

        public override void RepairVessel()
        {
            ArmorThickness = BattleshipInitialArmorThickness;
        }

        public void ToggleSonarMode()
        {
            SonarMode = !SonarMode;

            if (SonarMode == true)
            {
                MainWeaponCaliber += 40;
                Speed -= 5;
            }
            else
            {
                MainWeaponCaliber -= 40;
                Speed += 5;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.AppendLine(base.ToString());
            builder.AppendLine($" *Sonar mode: {(SonarMode ? "ON" : "OFF")}");
            return builder.ToString().TrimEnd();
        }
    }
}
