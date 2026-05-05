using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MonsterVox.Data
{
    [System.Serializable]
    public class PlayerData
    {
        private const string SAVE_FILE_NAME = "monster_vox_save.json";

        public int totalCoins;
        public List<string> unlockedThemeIDs = new List<string> { "Theme_01" };
        public List<string> unlockedMonsterIDs = new List<string> { "Mon_01" };
        public int currentSlotLevel = 1;

        /// <summary>
        /// Returns the max number of active stage slots based on currentSlotLevel.
        /// Level 1 = 3 slots, Level 2 = 4 slots, Level 3 = 5 slots (max).
        /// </summary>
        public int GetMaxSlots()
        {
            return currentSlotLevel + 2; // 1→3, 2→4, 3→5
        }

        public bool IsThemeUnlocked(string themeID)
        {
            return unlockedThemeIDs.Contains(themeID);
        }

        public bool IsMonsterUnlocked(string monsterID)
        {
            return unlockedMonsterIDs.Contains(monsterID);
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this, true);
            string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            File.WriteAllText(path, json);
        }

        public static PlayerData Load()
        {
            string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                if (data != null) return data;
            }

            // Return fresh save with defaults
            return new PlayerData();
        }
    }
}
