using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public static class House
    {
        public static Random Random = new Random();
        public static Location Entry;
        public static Dictionary<string, Location> locations;
        static House()
        {
            Entry = new Location("Entry");
            locations = new Dictionary<string, Location>();
            locations.Add("Entry", Entry);
            var rooms = new List<string>()
            {
               // "Attic",
                //"Master Bedroom",
               // "Master Bath",
                "Landing",
               // "Second Bathroom",
               // "Kids Room",
                //"Pantry",
               // "Nursery",
               // "Kitchen",
               // "Bathroom",
                "Hallway",
                //"Garage",
               // "Living Room"
            };
            
            foreach(var room in rooms)
            {
                locations.Add(room, new Location(room));
            }

            locations.Add("Garage", new LocationWithHidingPlace("Garage", "behind the car"));
            locations.Add("Kitchen", new LocationWithHidingPlace("Kitchen", "next to the stove"));
            locations.Add("Living Room", new LocationWithHidingPlace("Living Room", "behind the sofa"));
            locations.Add("Bathroom", new LocationWithHidingPlace("Bathroom", "behind the door"));
            locations.Add("Master Bedroom", new LocationWithHidingPlace("Master Bedroom", "inside a cabinet"));
            locations.Add("Master Bath", new LocationWithHidingPlace("Master Bath", "inside a cabinet"));
            locations.Add("Second Bathroom", new LocationWithHidingPlace("Second Bathroom", "inside a cabinet"));
            locations.Add("Kids Room", new LocationWithHidingPlace("Kids Room", "inside a cabinet"));
            locations.Add("Nursery", new LocationWithHidingPlace("Nursery", "inside a cabinet"));
            locations.Add("Pantry", new LocationWithHidingPlace("Pantry", "inside a cabinet"));
            locations.Add("Attic", new LocationWithHidingPlace("Attic", "in a trunk"));

            Entry.AddExit(Direction.Out, locations["Garage"]);
            Entry.AddExit(Direction.East, locations["Hallway"]);
            locations["Hallway"].AddExit(Direction.South, locations["Living Room"]);
            locations["Hallway"].AddExit(Direction.Northwest, locations["Kitchen"]);
            locations["Hallway"].AddExit(Direction.North, locations["Bathroom"]);
            locations["Hallway"].AddExit(Direction.Up, locations["Landing"]);
            locations["Landing"].AddExit(Direction.Southwest, locations["Nursery"]);
            locations["Landing"].AddExit(Direction.South, locations["Pantry"]);
            locations["Landing"].AddExit(Direction.Southeast, locations["Kids Room"]);
            locations["Landing"].AddExit(Direction.West, locations["Second Bathroom"]);
            locations["Landing"].AddExit(Direction.Northwest, locations["Master Bedroom"]);
            locations["Landing"].AddExit(Direction.Up, locations["Attic"]);
            locations["Master Bedroom"].AddExit(Direction.East, locations["Master Bath"]);

        }
        public static Location GetLocationByName(string name)
        {
            if (locations.ContainsKey(name))
                return locations[name];
            else return Entry;
        }
        public static Location RandomExit(Location location)
        {
           int randomNumber = Random.Next(location.Exits.Count);
           return location.Exits.Values.OrderBy(l => l.Name).ToList()[randomNumber];
        }
        public static void ClearHidingPlaces()
        {
           foreach(var location in locations)
            {
                if (location.Value is LocationWithHidingPlace locationWithHidingPlace)
                    locationWithHidingPlace.Opponents.Clear();
            }
        }
    }
}
