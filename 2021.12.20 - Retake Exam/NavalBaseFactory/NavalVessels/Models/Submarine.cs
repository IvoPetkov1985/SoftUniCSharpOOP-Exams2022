using NavalVessels.Models.Contracts;
using System.Text;

namespace NavalVessels.Models
{
    public class Submarine : Vessel, ISubmarine
    {
        private const double SubmarineInitialArmorThickness = 200;

        public Submarine(string name, double mainWeaponCaliber, double speed)
            : base(name, mainWeaponCaliber, speed, SubmarineInitialArmorThickness)
        {
        }

        public bool SubmergeMode { get; private set; }

        public override void RepairVessel()
        {
            ArmorThickness = SubmarineInitialArmorThickness;
        }

        public void ToggleSubmergeMode()
        {
            SubmergeMode = !SubmergeMode;

            if (SubmergeMode == true)
            {
                MainWeaponCaliber += 40;
                Speed -= 4;
            }
            else
            {
                MainWeaponCaliber -= 40;
                Speed += 4;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.AppendLine(base.ToString());
            builder.AppendLine($" *Submerge mode: {(SubmergeMode ? "ON" : "OFF")}");
            return builder.ToString().TrimEnd();
        }
    }
}
