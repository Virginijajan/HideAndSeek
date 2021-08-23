using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class LocationWithHidingPlace:Location
    {
        public string HidingPlace;
        public List<Opponent> Opponents = new List<Opponent>();
        
        public LocationWithHidingPlace(string name, string hidingPlace) : base(name)
        {
            HidingPlace = hidingPlace;
        }
       
        public void Hide(Opponent opponent)
        {
            Opponents.Add(opponent);
        }

        public IEnumerable<Opponent> CheckHidingPlace()
        {
            var opponents = new List<Opponent>();

            if (Opponents.Count > 0)
            {
                foreach (var opponent in Opponents)
                    opponents.Add(opponent);
                Opponents.Clear();   
            }
            return opponents;

        }
    }
}
