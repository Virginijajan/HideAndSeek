using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class Location
    {
        public string Name { get; private set; }
        public IDictionary<Direction, Location> Exits { get; private set; } 
            = new Dictionary<Direction, Location>();

        public Location(string name) => Name = name;
        public override string ToString() => Name;
        public IEnumerable<string> ExitList { get => Exits.Keys.OrderBy(key=>Math.Abs((int)key)).ThenBy(key=>(int)key)
                .Select(key=>$"the {Exits[key].Name} is {DescribeDirection(key)}");}
        public void AddExit(Direction direction, Location connectingLocation)
        {
            Exits[direction] = connectingLocation;
            AddReturnExit(direction, connectingLocation);

        }
        public Location GetExit(Direction direction)
        {
            if (Exits.ContainsKey(direction))
                return Exits[direction];
            else return this;
        }

        private string DescribeDirection(Direction d) => d switch
        {
            Direction.Up=>"Up",
            Direction.Down=>"Down",
            Direction.In=>"In",
            Direction.Out=>"Out",
            _=> $"to the {d}",
        };
        private void AddReturnExit(Direction direction, Location connectingLocation)
        {
            var returnDirection = -(int)direction;
            connectingLocation.Exits.Add((Direction)returnDirection, this);
        }
    }
}
