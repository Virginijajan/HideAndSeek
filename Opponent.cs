using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class Opponent
    {
        public readonly string Name;
        public Opponent(string name) => Name = name;
        public override string ToString() => Name;
        public void Hide()
        {
            var location = House.Entry;
            var randomNumber = House.Random.Next(10, 50);
            for(var n=0; n<randomNumber; n++)
            {
                location=House.RandomExit(location);
            }
            while (true)
            {
                if (location is LocationWithHidingPlace locationWithHidingPlace)
                {
                    locationWithHidingPlace.Hide(this);
                    System.Diagnostics.Debug.WriteLine($"{Name} is hiding " +
                    $"{(location as LocationWithHidingPlace).HidingPlace} in the {location.Name}");
                    return;
                }              
                else
                    location = House.RandomExit(location);
            }  
        }
    }
}
