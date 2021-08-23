using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HideAndSeek
{
   public class SavedGame
    {
        public string PlayersLocation { get; set; }
        public Dictionary<string, string> HidingOpponents { get; set; } = new Dictionary<string, string>();
        public List<string> FoundOpponentsNames { get; set; }
        public int MoveNumber { get; set; }

        
        public void SaveGame(string fileName, GameController gameController)
        {
            
            
            PlayersLocation = gameController.CurrentLocation.Name;
            MoveNumber = gameController.MoveNumber;
            FoundOpponentsNames = new List<string>();
            FoundOpponentsNames.AddRange(gameController.foundOpponents.Select(o => o.Name).ToList());
            HidingOpponents = new Dictionary<string, string>();
           
            var locations = House.locations.Select(location => location.Value).ToList();

           

            foreach(var location in locations)
            {
                if(location is LocationWithHidingPlace)
                foreach(var opponent in (location as LocationWithHidingPlace).Opponents)
                {
                    HidingOpponents[opponent.Name] = location.Name;
                }
            }

            var savedLocationString = JsonSerializer.Serialize(this);

            var path = GetPath(fileName);
            File.WriteAllText(path, savedLocationString);
        }
        public bool LoadGame(string fileName, GameController gameController)
        {
           
            var path = GetPath(fileName);
            string savedGameString = "";
            if (File.Exists(path))
            {
                savedGameString = File.ReadAllText(path);
            }
            else return false;
           
            File.Delete(path);
            SavedGame savedGame=JsonSerializer.Deserialize<SavedGame>(savedGameString);

            gameController.CurrentLocation = House.GetLocationByName(savedGame.PlayersLocation);
            gameController.MoveNumber = savedGame.MoveNumber;
           
            var foundOpponents= savedGame.FoundOpponentsNames.Select(o => new Opponent(o));
            gameController.foundOpponents.Clear();
            gameController.foundOpponents.AddRange(foundOpponents);

            House.ClearHidingPlaces();

            Location location;
            foreach(var opponent in savedGame.HidingOpponents)
            {
                location = House.GetLocationByName(opponent.Value);
                (location as LocationWithHidingPlace).Hide(new Opponent(opponent.Key));
            }
            return true;
        }

        string GetPath(string fileName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var folder = Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "saved_game");
            path = path + Path.DirectorySeparatorChar+"saved_game"+Path.DirectorySeparatorChar + fileName + ".json";
            return path;
        }      
    }
}
