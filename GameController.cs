using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class GameController
    {
        public SavedGame savedGame = new SavedGame();
        public static string S(int s) => s == 1 ? "" : "s";
        public Location CurrentLocation { get; set; }

        public string Status => "You are in the " + CurrentLocation.Name + ". You see the following exits:" + Environment.NewLine + " - " + string.Join(Environment.NewLine + " - ", CurrentLocation.ExitList) + statusUpdate;
        
        public string statusUpdate = "";
        public string Prompt => $"{MoveNumber}: Which direction do you want to go (or type 'check'): ";

        public int MoveNumber { get; set; } = 1;

        public readonly IEnumerable<Opponent> Opponents = new List<Opponent>()
        {
             new Opponent("Joe"),
             new Opponent("Bob"),
             new Opponent("Ana"),
             new Opponent("Owen"),
             new Opponent("Jimmy"),
        };

        public List<Opponent> foundOpponents = new List<Opponent>();

        public GameController()
        {
            House.ClearHidingPlaces();
            foreach (var opponent in Opponents)
                opponent.Hide();

            CurrentLocation = House.Entry;
        }

        public bool GameOver => Opponents.Count() == foundOpponents.Count();

        public bool Move(Direction direction)
        {
            if (CurrentLocation.Exits.ContainsKey(direction))
            {
                CurrentLocation = CurrentLocation.GetExit(direction);
                return true;
            }
            else return false;
        }

        public string ParseInput(string input)
        { 
            if (input.Count()>=4&&input.ToUpper().Substring(0,4) == "SAVE")
            {
                var fileName = input.Substring(5);
                fileName = fileName.Trim();

                if (!ParseFileName(fileName)) return "Invalid file name";

                savedGame.SaveGame(fileName, this);
                CurrentLocation = House.Entry;
                MoveNumber = 1;
                foundOpponents = new List<Opponent>();
                House.ClearHidingPlaces();
                foreach (var opponent in Opponents)
                    opponent.Hide();
                UpdateStatus();
                
                return ($"Saved current game to {fileName}");
            }
            if (input.Count() >= 4 && input.ToUpper().Substring(0, 4) == "LOAD")
            {
                var fileName = input.Substring(5);
                fileName = fileName.Trim();
                if (!ParseFileName(fileName)) return "Invalid file name";

                if (savedGame.LoadGame(fileName, this) == true)
                {
                    UpdateStatus();
                    return ($"Loaded game from {fileName}");
                }
                else return "Invalid file name";
            }
            if (input.ToUpper() == "CHECK")
            {
                MoveNumber++;
                if (CurrentLocation is LocationWithHidingPlace locationWithHidingPlace)
                {
                    if (locationWithHidingPlace.Opponents.Count > 0)
                    {
                        var foundOpponentsNumber = locationWithHidingPlace.Opponents.Count();
                        foundOpponents.AddRange(locationWithHidingPlace.CheckHidingPlace().Select(o => o).ToList());
                        UpdateStatus();
                        return $"You found {foundOpponentsNumber} opponent{S(foundOpponentsNumber)} hiding {locationWithHidingPlace.HidingPlace}";
                    }
                    else
                    {
                        UpdateStatus();
                        return $"Nobody was hiding {locationWithHidingPlace.HidingPlace}";
                    }
                }
                else
                {
                    UpdateStatus();
                    return $"There is no hiding place in the {CurrentLocation.Name}";
                } 
            }

            Direction direction;

            if (Enum.TryParse(input, out direction))
            {
                if (Move(direction))
                {
                    MoveNumber++;
                    UpdateStatus();
                    return $"Moving {direction}";
                }
                else return "There's no exit in that direction";
            }
            else return "That's not a valid direction";
        }

        void UpdateStatus()
        {
            statusUpdate = "";
            if (CurrentLocation is LocationWithHidingPlace locationWithHiding)
                statusUpdate += Environment.NewLine + "Someone could hide " + locationWithHiding.HidingPlace;
            if (foundOpponents.Count == 0)
                statusUpdate += Environment.NewLine + "You have not found any opponents";
            else
            {
                statusUpdate += Environment.NewLine+"You have found " + foundOpponents.Count + " of " + Opponents.Count() + " opponents: " + string.Join(", ", foundOpponents.Select(o => o.Name).ToList());
            }
        }

       public bool ParseFileName(string fileName)
        {
            List<char> prohibitedChar = new List<char>() {'/', '\\', '*','%', '?', ':', '|', '"', 
            '<', '>', '.', ',', ';', '=', ' '};
            
            if (fileName.Count() > 0&&fileName.Count()<20)
            {
                foreach(var character in prohibitedChar)
                {
                    if (fileName.Contains(character))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
