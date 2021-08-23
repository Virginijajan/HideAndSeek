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
        public Dictionary<string, string> HidingOpponents { get; set; }
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
            WriteFile(fileName);         
        }
        public bool LoadGame(string fileName, GameController gameController)
        {
            if (ReadFile(fileName, gameController))
            {
                gameController.CurrentLocation = House.GetLocationByName(gameController.savedGame.PlayersLocation);
                gameController.MoveNumber = gameController.savedGame.MoveNumber;
                var foundOpponents = gameController.savedGame.FoundOpponentsNames.Select(o => new Opponent(o));
                gameController.foundOpponents.Clear();
                gameController.foundOpponents.AddRange(foundOpponents);

                House.ClearHidingPlaces();

                Location location;
                foreach (var opponent in gameController.savedGame.HidingOpponents)
                {
                    location = House.GetLocationByName(opponent.Value);
                    (location as LocationWithHidingPlace).Hide(new Opponent(opponent.Key));
                }
                return true;
            }
            else return false;           
        }

        string GetPath(string fileName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var folder = Directory.CreateDirectory(path + Path.DirectorySeparatorChar + "saved_game");
            path = path + Path.DirectorySeparatorChar+"saved_game"+Path.DirectorySeparatorChar + fileName + ".json";
            return path;
        }
        
        void WriteFile(string fileName)
        {
            var savedLocationString = JsonSerializer.Serialize(this);
            var path = GetPath(fileName);
            File.WriteAllText(path, savedLocationString);
        }
        bool ReadFile(string fileName, GameController gameController)
        {
            var path = GetPath(fileName);
            string savedGameString = "";
            if (File.Exists(path))
            {
                savedGameString = File.ReadAllText(path);
            }
            else return false;

            File.Delete(path);
            gameController.savedGame = JsonSerializer.Deserialize<SavedGame>(savedGameString);
            return true;
        }
    }
}
